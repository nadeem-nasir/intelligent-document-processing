namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Providers;

public class AzureAIServicesClientProvider
{
    private readonly FormRecognizerSettings _formRecognizerconnectionString;
    public AzureAIServicesClientProvider(FormRecognizerSettings formRecognizerconnectionString)
    {
        ArgumentNullException.ThrowIfNull(formRecognizerconnectionString, nameof(formRecognizerconnectionString));

        _formRecognizerconnectionString = formRecognizerconnectionString;

    }
    public DocumentAnalysisClient GetFormRecognizerClient()
    {
        var credential = new AzureKeyCredential(_formRecognizerconnectionString.KEY1);

        var endpoint = new Uri(_formRecognizerconnectionString.Endpoint);

        return new DocumentAnalysisClient(endpoint, credential);
    }
}

