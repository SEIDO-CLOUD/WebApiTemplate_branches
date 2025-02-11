namespace App_simple;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHost _host;

    public Worker(ILogger<Worker> logger, IHost host)
    {
        _logger = logger;
        _host = host;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Hello World again");
        //throw new Exception("Error in AppSimple");

        await _host.StopAsync();
    }
}
