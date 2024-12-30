using Models;
using Models.DTO;

namespace Services;

public interface IZooService {

    public Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync();
    public Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems);
    public Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded);

    public Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id);
    public Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto item);
    public Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto item);
}