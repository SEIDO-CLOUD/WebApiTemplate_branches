using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class ZooDbRepos
{
    private readonly ILogger<ZooDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    #region contructors
    public ZooDbRepos(ILogger<ZooDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat)
    {
        if (!flat)
        {
            //make sure the model is fully populated, try without include.
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Zoos.AsNoTracking()
                .Where(i => i.ZooId == id);

            var resp =  await query.FirstOrDefaultAsync<IZoo>();
            return new ResponseItemDto<IZoo>()
            {
                DbConnectionKeyUsed = _dbContext.dbConnection,
                Item = resp
            };
        }
        else
        {
            //Not fully populated, compare the SQL Statements generated
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Zoos.AsNoTracking()
                .Where(i => i.ZooId == id);

            var resp = await query.FirstOrDefaultAsync<IZoo>();
            return new ResponseItemDto<IZoo>()
            {
                DbConnectionKeyUsed = _dbContext.dbConnection,
                Item = resp
            };
        }   
    }

    public async Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        IQueryable<ZooDbM> query;
        if (flat)
        {
            query = _dbContext.Zoos.AsNoTracking();
        }
        else
        {
            query = _dbContext.Zoos.AsNoTracking();
        }

        return new ResponsePageDto<IZoo>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.Name.ToLower().Contains(filter) ||
                         i.City.ToLower().Contains(filter) ||
                         i.Country.ToLower().Contains(filter))).CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.Name.ToLower().Contains(filter) ||
                         i.City.ToLower().Contains(filter) ||
                         i.Country.ToLower().Contains(filter)))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IZoo>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Zoos
            .Where(i => i.ZooId == id);
        var item = await query1.FirstOrDefaultAsync<ZooDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Zoos.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IZoo>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

    public async Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto itemDto)
    {
        //Find the instance with matching id and read the navigation properties.
        var query1 = _dbContext.Zoos
            .Where(i => i.ZooId == itemDto.ZooId);
        var item = await query1
            .FirstOrDefaultAsync<ZooDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.ZooId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //write to database model
        _dbContext.Zoos.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadZooAsync(item.ZooId, false);    
    }

    public async Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto itemDto)
    {
        if (itemDto.ZooId != null)
            throw new ArgumentException($"{nameof(itemDto.ZooId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Zoo
        var item = new ZooDbM(itemDto);

        //write to database model
        _dbContext.Zoos.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        
        //return the updated item in non-flat mode
        return await ReadZooAsync(item.ZooId, false);
    }
}
