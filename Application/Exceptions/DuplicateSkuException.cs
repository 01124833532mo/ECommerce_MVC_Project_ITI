namespace EcommerceIti.Application.Exceptions;

public class DuplicateSkuException : Exception
{
    public DuplicateSkuException(string sku)
        : base($"A product with SKU '{sku}' already exists.")
    {
        Sku = sku;
    }

    public string Sku { get; }
}