namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Configuration;

/// <summary>
/// Form Recognizer Settings
/// </summary>
public class FormRecognizerSettings
{
    public required string Endpoint { get; set; }

    public required string KEY1 { get; set; }

    public string KEY2 { get; set; } = default!;

}