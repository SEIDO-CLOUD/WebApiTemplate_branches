using Newtonsoft.Json;

namespace AppWebApiTest;

public interface ITestEndpointAccess {
    public Task ExecuteTestsAsync();
}