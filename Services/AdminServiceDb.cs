using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class AdminServiceDb : IAdminService {

    private readonly AdminDbRepos _adminRepo;
    private readonly ILogger<AdminServiceDb> _logger;    
    
    public AdminServiceDb(AdminDbRepos adminRepo, ILogger<AdminServiceDb> logger)
    {
        _adminRepo = adminRepo;
        _logger = logger;
    }

    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync() => _adminRepo.InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems) => _adminRepo.SeedAsync(nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded) => _adminRepo.RemoveSeedAsync(seeded);
}