using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class ZooServiceDb : IZooService {

    private readonly AdminDbRepos _adminRepo;
    private readonly ZooDbRepos _zooRepo;
    private readonly AnimalDbRepos _animalRepo;
    private readonly EmployeeDbRepos _employeeRepo;
    private readonly ILogger<ZooServiceDb> _logger;    
    
    public ZooServiceDb(AdminDbRepos adminRepo, ZooDbRepos zooRepo, AnimalDbRepos animalRepo, EmployeeDbRepos employeeRepo, ILogger<ZooServiceDb> logger)
    {
        _adminRepo = adminRepo;
        _zooRepo = zooRepo;
        _animalRepo = animalRepo;
        _employeeRepo = employeeRepo;
        _logger = logger;
    }

    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync() => _adminRepo.InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems) => _adminRepo.SeedAsync(nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded) => _adminRepo.RemoveSeedAsync(seeded);

    public Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _zooRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat) => _zooRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id) => _zooRepo.DeleteItemAsync(id);
    public Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto item) => _zooRepo.UpdateItemAsync(item);
    public Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto item) => _zooRepo.CreateItemAsync(item);

    public Task<ResponsePageDto<IAnimal>> ReadAnimalsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _animalRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAnimal>> ReadAnimalAsync(Guid id, bool flat) => _animalRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IAnimal>> DeleteAnimalAsync(Guid id) => _animalRepo.DeleteItemAsync(id);
    public Task<ResponseItemDto<IAnimal>> UpdateAnimalAsync(AnimalCuDto item) => _animalRepo.UpdateItemAsync(item);
    public Task<ResponseItemDto<IAnimal>> CreateAnimalAsync(AnimalCuDto item) => _animalRepo.CreateItemAsync(item);

    public Task<ResponsePageDto<IEmployee>> ReadEmployeesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _employeeRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IEmployee>> ReadEmployeeAsync(Guid id, bool flat) => _employeeRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IEmployee>> DeleteEmployeeAsync(Guid id) => _employeeRepo.DeleteItemAsync(id);
    public Task<ResponseItemDto<IEmployee>> UpdateEmployeeAsync(EmployeeCuDto item) => _employeeRepo.UpdateItemAsync(item);
    public Task<ResponseItemDto<IEmployee>> CreateEmployeeAsync(EmployeeCuDto item) => _employeeRepo.CreateItemAsync(item);
}