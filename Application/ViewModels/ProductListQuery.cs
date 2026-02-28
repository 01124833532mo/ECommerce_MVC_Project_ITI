namespace EcommerceIti.Application.ViewModels;

public record ProductListQuery(int? CategoryId, string? Search, string? Sort, int Page = 1, int PageSize = 12, bool IncludeInactive = false);
