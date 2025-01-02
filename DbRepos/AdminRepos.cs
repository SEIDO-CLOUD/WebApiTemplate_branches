using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Seido.Utilities.SeedGenerator;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    #region contructors
    public AdminDbRepos(ILogger<AdminDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync()
    {
        var info = new GstUsrInfoAllDto();
        info.Db = await _dbContext.InfoDbView.FirstAsync();
        info.Zoos = await _dbContext.InfoZoosView.ToListAsync();
        info.Animals = await _dbContext.InfoAnimalsView.ToListAsync();

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

        //Assign Address, Animals and Quotes to all the Zoos
        foreach (var zoo in zoos)
        {
            zoo.AnimalsDbM = seeder.ItemsToList<AnimalDbM>(seeder.Next(5,51));
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
            var nrZ = new SqlParameter("nrZ", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var nrA = new SqlParameter("nrA", SqlDbType.Int) { Direction = ParameterDirection.Output };

            parameters.Add(retValue);
            parameters.Add(seededArg);
            parameters.Add(nrZ);
            parameters.Add(nrA);

            //there is no FromSqlRawAsync to I make one here
            var _query = await Task.Run(() =>
                _dbContext.InfoDbView.FromSqlRaw($"EXEC @retval = supusr.spDeleteAll @seeded," +
                    $"@nrZ OUTPUT, @nrA OUTPUT",
                    parameters.ToArray()).AsEnumerable());

            //Execute the query and get the sp result set.
            //Although, I am not using this result set, but it shows how to get it
            GstUsrInfoDbDto result_set = _query.FirstOrDefault();

            //Check the return code
            int retCode = (int)retValue.Value;
            if (retCode != 0) throw new Exception("supusr.spDeleteAll return code error");

            return await InfoAsync();
    }
}
