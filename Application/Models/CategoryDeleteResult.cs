namespace EcommerceIti.Application.Models;

public enum CategoryDeleteResult
{
    NotFound = 0,
    Deleted = 1,
    HasChildCategories = 2,
    HasOrderedProducts = 3
}