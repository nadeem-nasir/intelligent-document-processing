namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Services;

public interface IIntelligentDocumentProcessingService
{
    /// <summary>
    /// This method analyzes a prebuilt invoice from a given file URI using the Form 
    /// </summary>
    /// <param name="fileUri"></param>
    /// <returns></returns>
    Task<AnalyzeResult> AnalyzePrebuiltInvoicesFromUriAsync(string fileUri);
    Task<AnalyzeResult> AnalyzePrebuiltInvoicesFromFileAsync(Stream document);

}

public class IntelligentDocumentProcessingService : IIntelligentDocumentProcessingService
{

    private readonly AzureAIServicesClientProvider _azureAIServicesClientProvider;

    public IntelligentDocumentProcessingService(AzureAIServicesClientProvider azureAIServicesClientProvider)
    {
        ArgumentNullException.ThrowIfNull(azureAIServicesClientProvider, nameof(azureAIServicesClientProvider));

        _azureAIServicesClientProvider = azureAIServicesClientProvider;
    }

    /// <summary>
    /// This method analyzes a prebuilt invoice from a given file URI using the Form Recognizer client
    /// </summary>
    /// <param name="fileUri"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<AnalyzeResult> AnalyzePrebuiltInvoicesFromUriAsync(string fileUri)
    {
        // Check if the fileUri is null or invalid
        ArgumentNullException.ThrowIfNull(fileUri, nameof(fileUri));

        if (!Uri.IsWellFormedUriString(fileUri, UriKind.Absolute))
        {
            throw new ArgumentException("Invalid fileUri", nameof(fileUri));
        }

        // Get the Form Recognizer client from the provider
        var client = _azureAIServicesClientProvider.GetFormRecognizerClient();

        // Analyze the document from the file URI using the prebuilt-invoice model
        var operationResponse = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-invoice", new Uri(fileUri));

        // Return the analysis result

        return operationResponse.Value;

    }

    public async Task<AnalyzeResult> AnalyzePrebuiltInvoicesFromFileAsync(Stream document)
    {
        // Check if the fileUri is null or invalid
        ArgumentNullException.ThrowIfNull(document, nameof(document));

        // Get the Form Recognizer client from the provider
        var client = _azureAIServicesClientProvider.GetFormRecognizerClient();

        // Analyze the document from the stream using the prebuilt-invoice model
        var operationResponse = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", document);

        // Return the analysis result

        return operationResponse.Value;

    }

}
