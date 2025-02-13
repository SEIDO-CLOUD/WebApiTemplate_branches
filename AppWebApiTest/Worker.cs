namespace AppWebApiTest;

public class Worker : BackgroundService
{

    private readonly ILogger<Worker> _logger;
    private readonly IHost _host;
    ITestEndpointAccess _testEndpointAccess;

    public Worker(ILogger<Worker> logger, IHost host, ITestEndpointAccess testEndpointAccess)
    {
        _logger = logger;
        _host = host;
        _testEndpointAccess = testEndpointAccess;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //dotnet run -- 10
        //sets nrOfIterations to 10 
        var args = Environment.GetCommandLineArgs();
        int nrOfIterations = 1;
        if (args.Length >= 2 && int.TryParse(args[1], out int argNrIterations))
        {
            nrOfIterations = argNrIterations;
        }

        bool testFauilure = false;
        _logger.LogInformation("JWT CRUD test suite started");
        try
        {
            await _testEndpointAccess.ExecuteTestsAsync(nrOfIterations);

            _logger.LogInformation("JWT CRUD test suite successfull");
            _logger.LogInformation("TestEndpointAccess suite ended");
        }
        catch (Exception ex)
        {
            testFauilure = true;
            _logger.LogError($"{ex.Message}.{ex.InnerException?.Message}");
            _logger.LogError("JWT CRUD test suite failed");
        }
        finally 
        {
            _logger.LogInformation("JWT CRUD test suite ended");
            await _host.StopAsync();

            //used to stop a DevOps CI/CD pipeline
            if (testFauilure)
            {
                Environment.Exit(1);
            }
        }

    }
}
