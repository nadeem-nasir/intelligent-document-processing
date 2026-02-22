namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Extensions;
internal static class StartupExtensions
{

    /// <summary>    
    /// Adds the Azure AI Services client provider to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>

    public static IServiceCollection AddAzureAIServicesClientProvider(this IServiceCollection services)
    {
        var endpoint = Environment.GetEnvironmentVariable("FormRecognizerEndpoint") ?? throw new ArgumentNullException("FormRecognizerEndpoint", "FormRecognizerEndpoint");

        var key = Environment.GetEnvironmentVariable("FormRecognizerKey1") ?? throw new ArgumentNullException("FormRecognizerKey1", "FormRecognizerKey1");

        services.AddSingleton(c => new AzureAIServicesClientProvider(new FormRecognizerSettings() {
            KEY1 = key,
            Endpoint = endpoint
        }));

        return services;
    }

    /// <summary>
    /// Adds the services to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IIntelligentDocumentProcessingService, IntelligentDocumentProcessingService>();

        return services;
    }

    public static IServiceCollection AddTemplateProvider(this IServiceCollection services)
    {
        services.AddSingleton<ITemplateProvider, TemplateProvider>();

        return services;
    }
}
