using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Seido.Utilities.SeedGenerator;
using Models.DTO;
using DbModels;
using DbContext;
using Configuration;
using Models;
using System.Security;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    #region contructors
    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync()
    {
        var info = new GstUsrInfoAllDto();
        info.Db = await _dbContext.InfoDbView.FirstAsync();
        info.Zoos = await _dbContext.InfoZoosView.ToListAsync();
        info.Animals = await _dbContext.InfoAnimalsView.ToListAsync();
        info.Employees = await _dbContext.InfoEmployeesView.ToListAsync();

        return new ResponseItemDto<GstUsrInfoAllDto>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = info
        };
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems)
    {
        //First of all make sure the database is cleared from all seeded data
        await RemoveSeedAsync(true);

        //Create a seeder
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);

        //Generate Zoos and persons to be employed
        var zoos = seeder.ItemsToList<ZooDbM>(nrOfItems);
        var persons = seeder.ItemsToList<EmployeeDbM>(seeder.Next(nrOfItems, 5*nrOfItems));

        //Assign CreditCards to persons with 50% probability
        foreach (var p in persons)
        {
            p.CreditCardDbM = (seeder.Bool) ? new CreditCardDbM(){FirstName = p.FirstName, LastName = p.LastName}.Seed(seeder) : null;
            p.CreditCardDbM?.EnryptAndObfuscate(_encryptions.AesEncryptToBase64);
#if DEBUG
            var temp = p.CreditCardDbM?.Decrypt(_encryptions.AesDecryptFromBase64<CreditCard>);
            if (temp?.CreditCardId != p.CreditCardDbM?.CreditCardId) throw new SecurityException("CreditCard encryption error");
#endif
        }

        //Assign Animals and Employees to all the Zoos
        foreach (var zoo in zoos)
        {
            zoo.AnimalsDbM = seeder.ItemsToList<AnimalDbM>(seeder.Next(5,51));

            //Employ between 2 and 8 persons from the list
            zoo.EmployeesDbM = seeder.UniqueIndexPickedFromList<EmployeeDbM>(seeder.Next(2, 9), persons);
        }

        //Note that all other tables are automatically set through ZooDbM Navigation properties
        _dbContext.Zoos.AddRange(zoos);

        await _dbContext.SaveChangesAsync();

        return await InfoAsync();
    }
    
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
            var parameters = new List<SqlParameter>();

            var retValue = new SqlParameter("retval", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var seededArg = new SqlParameter("seeded", seeded);

            parameters.Add(retValue);
            parameters.Add(seededArg);

            //there is no FromSqlRawAsync to I make one here
            var _query = await Task.Run(() =>
                _dbContext.InfoDbView.FromSqlRaw($"EXEC @retval = supusr.spDeleteAll @seeded",
                    parameters.ToArray()).AsEnumerable());

            //Execute the query and get the sp result set.
            //Although, I am not using this result set, but it shows how to get it
            GstUsrInfoDbDto result_set = _query.FirstOrDefault();

            //Check the return code
            int retCode = (int)retValue.Value;
            if (retCode != 0) throw new Exception("supusr.spDeleteAll return code error");

            return await InfoAsync();
    }

    public async Task<UsrInfoDto> SeedUsersAsync(int nrOfUsers, int nrOfSuperUsers, int nrOfSysAdmin)
    {
            _logger.LogInformation($"Seeding {nrOfUsers} users and {nrOfSuperUsers} superusers");
            
            //First delete all existing users
            foreach (var u in _dbContext.Users)
                _dbContext.Users.Remove(u);

            //add users
            for (int i = 1; i <= nrOfUsers; i++)
            {
                _dbContext.Users.Add(new UserDbM
                {
                    UserId = Guid.NewGuid(),
                    UserName = $"user{i}",
                    Email = $"user{i}@gmail.com",
                    Password = _encryptions.EncryptPasswordToBase64($"user{i}"),
                    Role = "usr"
                });
            }

            //add super user
            for (int i = 1; i <= nrOfSuperUsers; i++)
            {
                _dbContext.Users.Add(new UserDbM
                {
                    UserId = Guid.NewGuid(),
                    UserName = $"superuser{i}",
                    Email = $"superuser{i}@gmail.com",
                    Password = _encryptions.EncryptPasswordToBase64($"superuser{i}"),
                    Role = "supusr"
                });
            }

            //add system adminitrators
            for (int i = 1; i <= nrOfSysAdmin; i++)
            {
                _dbContext.Users.Add(new UserDbM
                {
                    UserId = Guid.NewGuid(),
                    UserName = $"sysadmin{i}",
                    Email = $"sysadmin{i}@gmail.com",
                    Password = _encryptions.EncryptPasswordToBase64($"sysadmin{i}"),
                    Role = "sysadmin"
                });
            }
            await _dbContext.SaveChangesAsync();

            var _info = new UsrInfoDto
            {
                NrUsers = await _dbContext.Users.CountAsync(i => i.Role == "usr"),
                NrSuperUsers = await _dbContext.Users.CountAsync(i => i.Role == "supusr"),
                NrSystemAdmin = await _dbContext.Users.CountAsync(i => i.Role == "sysadmin")
            };

            return _info;
    }
}
