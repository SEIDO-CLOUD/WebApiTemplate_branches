using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class ZooServiceDb : IZooService {

    private readonly ZooDbRepos _zooRepo;
    private readonly AnimalDbRepos _animalRepo;
    private readonly EmployeeDbRepos _employeeRepo;
    private readonly CreditCardDbRepos _creditcardRepo;
    private readonly ILogger<ZooServiceDb> _logger;    
    
    public ZooServiceDb(AdminDbRepos adminRepo, ILogger<ZooServiceDb> logger)
    {
        _adminRepo = adminRepo;
        _logger = logger;
    }

}