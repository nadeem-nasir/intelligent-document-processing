var host = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) => {
    })
    .ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder workerApplication) => {

    })
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddTemplateProvider();
        services.AddAzureAIServicesClientProvider();
        services.AddServices();
    })
    .Build();

await host.RunAsync();

