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

    public async Task<IZoo> ReadZooAsync(Guid id, bool flat)
    {
        if (!flat)
        {
            //make sure the model is fully populated, try without include.
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Zoos.AsNoTracking()
                .Include(i => i.Animals)
                .Where(i => i.ZooId == id);

            return await query.FirstOrDefaultAsync<IZoo>();
        }
        else
        {
            //Not fully populated, compare the SQL Statements generated
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Zoos.AsNoTracking()
                .Where(i => i.ZooId == id);

            return await query.FirstOrDefaultAsync<IZoo>();
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
            query = _dbContext.Zoos.AsNoTracking()
                .Include(i => i.Animals);
        }

        var ret = new ResponsePageDto<IZoo>()
        {
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
        return ret;
    }

    public async Task<IZoo> DeleteZooAsync(Guid id)
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
        return item;   
    }

    public async Task<IZoo> UpdateZooAsync(ZooCuDto itemDto)
    {
        //Find the instance with matching id and read the navigation properties.
        var query1 = _dbContext.Zoos
            .Where(i => i.ZooId == itemDto.ZooId);
        var item = await query1
            .Include(i => i.Animals)
            .FirstOrDefaultAsync<ZooDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.ZooId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //Update navigation properties
        await navProp_ZooCUdto_to_ZooDbM(itemDto, item);

        //write to database model
        _dbContext.Zoos.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadZooAsync(item.ZooId, false);    
    }

    public async Task<IZoo> CreateZooAsync(ZooCuDto itemDto)
    {
        if (itemDto.ZooId != null)
            throw new ArgumentException($"{nameof(itemDto.ZooId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Zoo
        var item = new ZooDbM(itemDto);

        //Update navigation properties
        await navProp_ZooCUdto_to_ZooDbM(itemDto, item);

        //write to database model
        _dbContext.Zoos.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        
        //return the updated item in non-flat mode
        return await ReadZooAsync(item.ZooId, false);   
    }

    //from all Guid relationships in _itemDtoSrc finds the corresponding object in the database and assigns it to _itemDst 
    //as navigation properties. Error is thrown if no object is found corresponing to an id.
    private async Task navProp_ZooCUdto_to_ZooDbM(ZooCuDto itemDtoSrc, ZooDbM itemDst)
    {
        //update AnimalsDbM from list
        List<AnimalDbM> Animals = null;
        if (itemDtoSrc.AnimalsId != null)
        {
            Animals = new List<AnimalDbM>();
            foreach (var id in itemDtoSrc.AnimalsId)
            {
                var p = await _dbContext.Animals.FirstOrDefaultAsync(i => i.AnimalId == id);
                if (p == null)
                    throw new ArgumentException($"Item id {id} not existing");

                Animals.Add(p);
            }
        }
        itemDst.AnimalsDbM = Animals;
    }
}
