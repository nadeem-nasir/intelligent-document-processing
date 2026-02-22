namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Functions;
public class PingInvoiceFunction(
    IIntelligentDocumentProcessingService intelligentDocumentProcessingService,
    ITemplateProvider templateProvider,
    ILogger<PingInvoiceFunction> logger)
{

    /// <summary>
    /// Converts the invoice from the URI to a JSON response
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function(nameof(PingInvoiceFromUri))]
    public async Task<HttpResponseData> PingInvoiceFromUri([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            var rawResponse = await intelligentDocumentProcessingService.AnalyzePrebuiltInvoicesFromUriAsync("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-invoice.pdf");

            var invoiceResponse = InvoiceResponseModelMapper.MapToInvoiceResponseModel(rawResponse);

            response.WriteString(JsonSerializer.Serialize(invoiceResponse), System.Text.Encoding.UTF8);

            return response;

        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred");

            throw;
        }
    }

    /// <summary>
    /// Converts the invoice from the file to a JSON response
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function(nameof(PingInvoiceFromFile))]
    public async Task<HttpResponseData> PingInvoiceFromFile([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json");

            var sampleInvoice = await templateProvider.GetTemplate("sample-invoice.pdf");
            var ms = new MemoryStream(sampleInvoice);
            var rawResponse = await intelligentDocumentProcessingService.AnalyzePrebuiltInvoicesFromFileAsync(ms);

            var invoiceResponse = InvoiceResponseModelMapper.MapToInvoiceResponseModel(rawResponse);

            response.WriteString(JsonSerializer.Serialize(invoiceResponse), System.Text.Encoding.UTF8);

            return response;

        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred");

            throw;
        }
    }
}
