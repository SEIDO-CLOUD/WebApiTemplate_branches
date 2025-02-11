using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;

using Models;
using Models.DTO;

namespace Services;


public class ZooServiceWapi : IZooService {

    private readonly ILogger<AdminServiceWapi> _logger; 
    private readonly HttpClient _httpClient;

    //To ensure Json deserializern is using the class implementations instead of the Interfaces 
    readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        Converters = {
            new AbstractConverter<Zoo, IZoo>(),
            new AbstractConverter<Animal, IAnimal>(),
            new AbstractConverter<Employee, IEmployee>(),
            
        },
    };
    
    public string BearerToken { 
    set
    {
        if (value != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
        }
    }}

    public ZooServiceWapi(ILogger<AdminServiceWapi> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(name: "ZooWebApi");

    }

    #region Zoo CRUD
    public async Task<ResponsePageDto<IZoo>> ReadZoosAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        string uri = $"zoo/readitems?seeded={seeded}&flat={flat}&filter={filter}&pagenr={pageNumber}&pagesize={pageSize}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        await response.EnsureSuccessStatusCodeWithMessage();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponsePageDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    public async Task<ResponseItemDto<IZoo>> ReadZooAsync(Guid id, bool flat)
    {
        string uri = $"zoo/readitem?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusCodeWithMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }

    public async Task<ResponseItemDto<ZooCuDto>> ReadZooDtoAsync(Guid id, bool flat)
    {
        string uri = $"zoo/readitemdto?id={id}&flat={flat}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.GetAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusCodeWithMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<ZooCuDto>>(s, _jsonSettings);
        return resp;
    }
     public async Task<ResponseItemDto<IZoo>> DeleteZooAsync(Guid id)
    {
        string uri = $"zoo/deleteitem/{id}";

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusCodeWithMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    public async Task<ResponseItemDto<IZoo>> UpdateZooAsync(ZooCuDto item)
    {
        string uri = $"zoo/updateitem/{item.ZooId}";

        //Prepare the request body
        string body = JsonConvert.SerializeObject(item);
        var requestContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PutAsync(uri, requestContent);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusCodeWithMessage();

        //Get the response body
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }
    public async Task<ResponseItemDto<IZoo>> CreateZooAsync(ZooCuDto item)
    {
        string uri = $"zoo/createitem";

        //Prepare the request content
        string body = JsonConvert.SerializeObject(item);
        var requestContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        //Send the HTTP Message and await the repsonse
        HttpResponseMessage response = await _httpClient.PostAsync(uri, requestContent);

        //Throw an exception if the response is not successful
        await response.EnsureSuccessStatusCodeWithMessage();

        //Get the resonse data
        string s = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseItemDto<IZoo>>(s, _jsonSettings);
        return resp;
    }

    public Task<ResponsePageDto<IAnimal>> ReadAnimalsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IAnimal>> ReadAnimalAsync(Guid id, bool flat)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IAnimal>> DeleteAnimalAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IAnimal>> UpdateAnimalAsync(AnimalCuDto item)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IAnimal>> CreateAnimalAsync(AnimalCuDto item)
    {
        throw new NotImplementedException();
    }

    public Task<ResponsePageDto<IEmployee>> ReadEmployeesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IEmployee>> ReadEmployeeAsync(Guid id, bool flat)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IEmployee>> DeleteEmployeeAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IEmployee>> UpdateEmployeeAsync(EmployeeCuDto item)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IEmployee>> CreateEmployeeAsync(EmployeeCuDto item)
    {
        throw new NotImplementedException();
    }

    public Task<ResponsePageDto<ICreditCard>> ReadCreditCardsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<ICreditCard>> ReadCreditCardAsync(Guid id, bool flat)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<ICreditCard>> DeleteCreditCardAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<ICreditCard>> CreateCreditCardAsync(CreditCardCuDto item)
    {
        throw new NotImplementedException();
    }

    public Task<ResponsePageDto<IEmployee>> ReadEmployeesWithCCAsync(bool hasCreditCard, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<ICreditCard>> ReadDecryptedCCAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    #endregion
}