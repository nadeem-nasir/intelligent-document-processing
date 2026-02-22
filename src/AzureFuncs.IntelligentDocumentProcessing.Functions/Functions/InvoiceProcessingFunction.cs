namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Functions;

/// <summary>
/// Azure Function to process the invoice from the blob storage
/// Using Primary constructor
/// </summary>
/// <param name="logger"></param>
/// <param name="intelligentDocumentProcessingService"></param>
public class InvoiceProcessingFunction(ILogger<InvoiceProcessingFunction> logger, IIntelligentDocumentProcessingService intelligentDocumentProcessingService)
{
    /// <summary>
    /// Read the invoice from the blob storage and extract the information from the invoice and save it to a JSON file in the blob storage
    /// </summary>
    /// <param name="blobContent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    [Function(nameof(InvoiceProcessingFunction))]
    [BlobOutput("json-customer-invoice-email-attachments/{name}.json", Connection = "AzureWebJobsStorage")]
    public async Task<List<InvoiceResponseModel>> InvoiceFromBlob([BlobTrigger("raw-customer-invoice-email-attachments/{name}", Connection = "AzureWebJobsStorage")] byte[] blobContent, string name)
    {
        logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");

        var ms = new MemoryStream(blobContent);
        var rawResponse = await intelligentDocumentProcessingService.AnalyzePrebuiltInvoicesFromFileAsync(ms);

        return InvoiceResponseModelMapper.MapToInvoiceResponseModel(rawResponse);
    }
}
