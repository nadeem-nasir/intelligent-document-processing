namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Mapper;
public static class InvoiceResponseModelMapper
{
    /// <summary>
    /// Maps the document analysis result to a list of invoices
    /// </summary>
    /// <param name="documentAnalysisResult"></param>
    /// <returns></returns>
    public static List<InvoiceResponseModel> MapToInvoiceResponseModel(AnalyzeResult documentAnalysisResult)
    {
        // Create a list to store the invoices
        List<InvoiceResponseModel> invoices = [];

        for (int i = 0; i < documentAnalysisResult.Documents.Count; i++)
        {

            AnalyzedDocument document = documentAnalysisResult.Documents[i];

            var invoice = MapToInvoiceResponseModel(document);

            invoices.Add(invoice);
        }

        return invoices;

    }

    /// <summary>
    /// Maps the analyzed document to an invoice
    /// </summary>
    /// <param name="analyzedDocument"></param>
    /// <returns></returns>
    private static InvoiceResponseModel MapToInvoiceResponseModel(AnalyzedDocument analyzedDocument)
    {
        // Create a new invoice object
        InvoiceResponseModel invoice = new();

        // Get the document fields
        IReadOnlyDictionary<string, DocumentField> fields = analyzedDocument.Fields;
        (List<DocumentField> itemsField, double _) = GetFieldValueAndConfidence<List<DocumentField>>(fields, "Items");

        (invoice.VendorName, invoice.VendorNameConfidence) = GetFieldValueAndConfidence<string>(fields, "VendorName");

        (invoice.CustomerName, invoice.CustomerNameConfidence) = GetFieldValueAndConfidence<string>(fields, "CustomerName");


        (invoice.SubTotal, invoice.SubTotalConfidence) = GetFieldValueAndConfidence<CurrencyResponseModel>(fields, "SubTotal");

        (invoice.TotalTax, invoice.TotalTaxConfidence) = GetFieldValueAndConfidence<CurrencyResponseModel>(fields, "TotalTax");

        (invoice.InvoiceTotal, invoice.InvoiceTotalConfidence) = GetFieldValueAndConfidence<CurrencyResponseModel>(fields, "InvoiceTotal");

        invoice.Items = MapInvoiceLineItems(itemsField);

        return invoice;

    }

    /// <summary>
    /// Maps the invoice line items
    /// </summary>
    /// <param name="itemsField"></param>
    /// <returns></returns>
    private static List<LineItemResponseModel> MapInvoiceLineItems(List<DocumentField> itemsField)
    {
        List<LineItemResponseModel> lineItems = [];

        foreach (var documentItemField in itemsField)
        {
            var itemField = documentItemField.Value.AsDictionary();

            var lineItem = new LineItemResponseModel();

            (lineItem.Description, lineItem.DescriptionConfidence) = GetFieldValueAndConfidence<string>(itemField, "Description");

            (lineItem.Amount, lineItem.AmountConfidence) = GetFieldValueAndConfidence<CurrencyResponseModel>(itemField, "Amount");

            (lineItem.UnitPrice, lineItem.UnitPriceConfidence) = GetFieldValueAndConfidence<CurrencyResponseModel>(itemField, "UnitPrice");

            (lineItem.Quantity, lineItem.QuantityConfidence) = GetFieldValueAndConfidence<double>(itemField, "Quantity");


            lineItems.Add(lineItem);
        }

        return lineItems;

    }

    /// <summary>
    /// maps the field value and confidence
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fields"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    private static (T, float) GetFieldValueAndConfidence<T>(IReadOnlyDictionary<string, DocumentField> fields, string fieldName)
    {
        ArgumentNullException.ThrowIfNull(fields, nameof(fields));

        ArgumentException.ThrowIfNullOrEmpty(fieldName, nameof(fieldName));

        var documentField = GetDocumentField(fields, fieldName);

        T value = ConvertFieldValue<T>(documentField);


        return (value, documentField.Confidence ?? 0.0f);

    }

    private static DocumentField GetDocumentField(IReadOnlyDictionary<string, DocumentField> fields, string fieldName)
    {
        ArgumentNullException.ThrowIfNull(fields, nameof(fields));

        ArgumentException.ThrowIfNullOrEmpty(fieldName, nameof(fieldName));

        // Use the null-coalescing operator to return the default value if the key is not found
        return fields.TryGetValue(fieldName, out DocumentField? field) ? field : default!;

    }

    /// <summary>
    /// Converts the field value to the specified type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="field"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static T ConvertFieldValue<T>(DocumentField field)
    {
        ArgumentNullException.ThrowIfNull(field, nameof(field));

        return field.FieldType switch {

            DocumentFieldType.String => (T)(object)field.Value.AsString(),

            DocumentFieldType.Date => (T)(object)field.Value.AsDate(),

            DocumentFieldType.Time => (T)(object)field.Value.AsTime(),

            DocumentFieldType.PhoneNumber => (T)(object)field.Value.AsPhoneNumber(),

            DocumentFieldType.Double => (T)(object)field.Value.AsDouble(),

            DocumentFieldType.Int64 => (T)(object)field.Value.AsInt64(),

            DocumentFieldType.SelectionMark => (T)(object)field.Value.AsSelectionMarkState(),

            DocumentFieldType.Signature => (T)(object)field.Value.AsSignatureType(),

            DocumentFieldType.CountryRegion => (T)(object)field.Value.AsCountryRegion(),

            DocumentFieldType.List => (T)(object)field.Value.AsList(),

            DocumentFieldType.Dictionary => (T)(object)field.Value.AsDictionary(),

            DocumentFieldType.Currency => MapToCurrencyResponseModel<T>(field.Value),

            DocumentFieldType.Address => (T)(object)field.Value.AsAddress(),

            DocumentFieldType.Unknown => default!,

            _ => throw new ArgumentOutOfRangeException(nameof(field), $"Unexpected field type {field.FieldType}"),

        };
    }

    private static T MapToCurrencyResponseModel<T>(DocumentFieldValue fieldValue)
    {
        ArgumentNullException.ThrowIfNull(fieldValue, nameof(fieldValue));

        var currency = fieldValue.AsCurrency();

        return (T)(object)new CurrencyResponseModel {
            Code = currency.Symbol,
            Amount = currency.Amount
        };
    }
}
