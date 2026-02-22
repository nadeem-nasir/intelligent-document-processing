namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Providers;
public interface ITemplateProvider
{
    Task<byte[]> GetTemplate(string templateName);
    string GetTemplatePath(string templateName);
}


public class TemplateProvider : ITemplateProvider
{
    public async Task<byte[]> GetTemplate(string templateName)
    {
        // Get the app directory from the context
        var rootDir = Directory.GetCurrentDirectory();

        // Combine the app directory and the template name to get the full path of the template file
        var templatePath = Path.Combine(rootDir, "Template", templateName);

        // Read and return the content of the template file as a byte array
        return await File.ReadAllBytesAsync(templatePath);
    }

    public string GetTemplatePath(string templateName)
    {
        // Get the app directory from the context
        var rootDir = Directory.GetCurrentDirectory();

        // Combine the app directory and the template name to get the full path of the template file
        var templatePath = Path.Combine(rootDir, "Template", templateName);

        return templatePath;
    }
}
