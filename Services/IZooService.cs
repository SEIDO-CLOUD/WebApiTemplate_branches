using Models;
using Models.DTO;

namespace Services;

public interface IZooService {

    public Task<GstUsrInfoAllDto> InfoAsync();
    public Task<GstUsrInfoAllDto> SeedAsync(int nrOfItems);
    public Task<GstUsrInfoAllDto> RemoveSeedAsync(bool seeded);

    public Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<IZoo> ReadZooAsync(Guid id, bool flat);
    public Task<IZoo> DeleteZooAsync(Guid id);
    public Task<IZoo> UpdateZooAsync(ZooCuDto item);
    public Task<IZoo> CreateZooAsync(ZooCuDto item);

    public Task<ResponsePageDto<IAnimal>> ReadAnimalsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<IAnimal> ReadAnimalAsync(Guid id, bool flat);
    public Task<IAnimal> DeleteAnimalAsync(Guid id);
    public Task<IAnimal> UpdateAnimalAsync(AnimalCuDto item);
    public Task<IAnimal> CreateAnimalAsync(AnimalCuDto item);
}