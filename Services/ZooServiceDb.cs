using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class ZooServiceDb : IZooService {

    private readonly AdminDbRepos _adminRepo;
    private readonly ZooDbRepos _zooRepo;
    private readonly ILogger<ZooServiceDb> _logger;    
    
    public ZooServiceDb(AdminDbRepos adminRepo, ZooDbRepos zooRepo, ILogger<ZooServiceDb> logger)
    {
        _adminRepo = adminRepo;
        _zooRepo = zooRepo;
        _logger = logger;
    }

    #region Simple 1:1 calls in this case, but as Services expands, this will no longer be the case
    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync() => _adminRepo.InfoAsync();

    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems) => _adminRepo.SeedAsync(nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded) => _adminRepo.RemoveSeedAsync(seeded);

    public Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _zooRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat) => _zooRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id) => _zooRepo.DeleteItemAsync(id);
    public Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto item) => _zooRepo.UpdateItemAsync(item);
    public Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto item) => _zooRepo.CreateItemAsync(item);

    #endregion
}