using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Domain.Entities;
using EcommerceIti.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIti.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly MazadContext _context;

    public ProductService(MazadContext context)
    {
        _context = context;
    }

    public async Task<ProductListVm> GetPagedAsync(ProductListQuery query)
    {
        var baseQuery = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!query.IncludeInactive)
        {
            baseQuery = baseQuery.Where(p => p.IsActive);
        }

        if (query.CategoryId.HasValue)
        {
            baseQuery = baseQuery.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            baseQuery = baseQuery.Where(p => p.Name.Contains(term) || p.SKU.Contains(term));
        }

        baseQuery = query.Sort switch
        {
            "price_desc" => baseQuery.OrderByDescending(p => p.Price),
            "price" => baseQuery.OrderBy(p => p.Price),
            "newest" => baseQuery.OrderByDescending(p => p.CreatedAt),
            _ => baseQuery.OrderBy(p => p.Name)
        };

        var total = await baseQuery.CountAsync();
        var items = await baseQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new ProductListItemVm(p.ProductId, p.Name, p.SKU, p.Price, p.StockQuantity, p.IsActive, p.Category.Name))
            .ToListAsync();

        return new ProductListVm
        {
            Items = items,
            CurrentPage = query.Page,
            PageSize = query.PageSize,
            TotalCount = total,
            CategoryId = query.CategoryId,
            Search = query.Search,
            Sort = query.Sort
        };
    }

    public async Task<ProductDetailsVm?> GetDetailsAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.ProductId == id)
            .Select(p => new ProductDetailsVm
            {
                ProductId = p.ProductId,
                Name = p.Name,
                SKU = p.SKU,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                CategoryName = p.Category.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductEditVm?> GetForEditAsync(int id)
    {
        return await _context.Products
            .Where(p => p.ProductId == id)
            .Select(p => new ProductEditVm
            {
                ProductId = p.ProductId,
                CategoryId = p.CategoryId,
                Name = p.Name,
                SKU = p.SKU,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(ProductEditVm vm)
    {
        var product = new Product
        {
            CategoryId = vm.CategoryId,
            Name = vm.Name,
            SKU = vm.SKU,
            Price = vm.Price,
            StockQuantity = vm.StockQuantity,
            IsActive = vm.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.ProductId;
    }

    public async Task UpdateAsync(ProductEditVm vm)
    {
        var product = await _context.Products.FindAsync(vm.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }

        product.CategoryId = vm.CategoryId;
        product.Name = vm.Name;
        product.SKU = vm.SKU;
        product.Price = vm.Price;
        product.StockQuantity = vm.StockQuantity;
        product.IsActive = vm.IsActive;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
