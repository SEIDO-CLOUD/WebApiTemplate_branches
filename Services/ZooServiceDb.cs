using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class ZooServiceDb : IZooService {

    private readonly AdminDbRepos _adminRepo;
    private readonly ILogger<ZooServiceDb> _logger;    
    
    public ZooServiceDb(AdminDbRepos adminRepo, ILogger<ZooServiceDb> logger)
    {
        _adminRepo = adminRepo;
        _logger = logger;
    }

    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync() => _adminRepo.InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems) => _adminRepo.SeedAsync(nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded) => _adminRepo.RemoveSeedAsync(seeded);

}