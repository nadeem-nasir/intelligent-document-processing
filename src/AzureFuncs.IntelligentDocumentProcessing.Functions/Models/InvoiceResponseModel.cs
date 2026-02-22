namespace AzureFuncs.IntelligentDocumentProcessing.Functions.Models;
public class InvoiceResponseModel
{
    public InvoiceResponseModel()
    {
        Items = [];
    }
    public string VendorName { get; set; } = default!;

    public float VendorNameConfidence { get; set; }

    public string CustomerName { get; set; } = default!;

    public double CustomerNameConfidence { get; set; }

    public List<LineItemResponseModel> Items { get; set; } = default!;

    public CurrencyResponseModel SubTotal { get; set; } = default!;

    public double SubTotalConfidence { get; set; }

    public CurrencyResponseModel TotalTax { get; set; } = default!;

    public double TotalTaxConfidence { get; set; }

    public CurrencyResponseModel InvoiceTotal { get; set; } = default!;

    public double InvoiceTotalConfidence { get; set; }
}

public class LineItemResponseModel
{
    public string Description { get; set; } = default!;

    public double DescriptionConfidence { get; set; }

    public CurrencyResponseModel Amount { get; set; } = default!;

    public double AmountConfidence { get; set; }

    public CurrencyResponseModel UnitPrice { get; set; } = default!;

    public double UnitPriceConfidence { get; set; }

    public double Quantity { get; set; } = default!;

    public double QuantityConfidence { get; set; }

}

public class CurrencyResponseModel
{
    public string Code { get; set; } = default!;

    public double Amount { get; set; }

}