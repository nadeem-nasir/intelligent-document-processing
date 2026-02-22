using System.Text.Json.Serialization;

namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Extensions;

internal static class WorkerConfigurationExtensions
{
    /// <summary>
    /// Calling ConfigureFunctionsWorkerDefaults() configures the Functions Worker to use System.Text.Json for all JSON
    /// serialization and sets JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    /// This method uses DI to modify the JsonSerializerOptions. Call /api/HttpFunction to see the changes.
    /// </summary>
    public static IFunctionsWorkerApplicationBuilder ConfigureSystemTextJson(this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.Services.Configure<JsonSerializerOptions>(jsonSerializerOptions => {
            jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            jsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            jsonSerializerOptions.WriteIndented = true;
            // override the default value
            jsonSerializerOptions.PropertyNameCaseInsensitive = false;
        });

        return builder;
    }

}
