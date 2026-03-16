using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.Models;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Application.Exceptions;
using EcommerceIti.Domain.Entities;
using EcommerceIti.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
            .Select(p => new ProductListItemVm(p.ProductId, p.Name, p.SKU, p.Price, p.ImageUrl, p.StockQuantity, p.IsActive, p.Category.Name))
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
                ImageUrl = p.ImageUrl,
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
                ImageUrl = p.ImageUrl,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsSkuInUseAsync(string sku, int? productId = null)
    {
        var normalizedSku = sku.Trim();

        return await _context.Products.AnyAsync(p =>
            p.SKU == normalizedSku &&
            (!productId.HasValue || p.ProductId != productId.Value));
    }

    public async Task<int> CreateAsync(ProductEditVm vm)
    {
        vm.SKU = vm.SKU.Trim();

        if (await IsSkuInUseAsync(vm.SKU))
        {
            throw new DuplicateSkuException(vm.SKU);
        }

        var product = new Product
        {
            CategoryId = vm.CategoryId,
            Name = vm.Name,
            SKU = vm.SKU,
            Price = vm.Price,
            ImageUrl = vm.ImageUrl,
            StockQuantity = vm.StockQuantity,
            IsActive = vm.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsDuplicateSkuViolation(ex))
        {
            throw new DuplicateSkuException(vm.SKU);
        }

        return product.ProductId;
    }

    public async Task UpdateAsync(ProductEditVm vm)
    {
        vm.SKU = vm.SKU.Trim();

        if (await IsSkuInUseAsync(vm.SKU, vm.ProductId))
        {
            throw new DuplicateSkuException(vm.SKU);
        }

        var product = await _context.Products.FindAsync(vm.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }

        product.CategoryId = vm.CategoryId;
        product.Name = vm.Name;
        product.SKU = vm.SKU;
        product.Price = vm.Price;
        product.ImageUrl = vm.ImageUrl;
        product.StockQuantity = vm.StockQuantity;
        product.IsActive = vm.IsActive;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsDuplicateSkuViolation(ex))
        {
            throw new DuplicateSkuException(vm.SKU);
        }
    }

    public async Task<ProductDeleteResult> DeleteAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.OrderItems)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
        {
            return ProductDeleteResult.NotFound;
        }

        if (product.OrderItems.Count > 0)
        {
            product.IsActive = false;
            product.StockQuantity = 0;
            await _context.SaveChangesAsync();
            return ProductDeleteResult.Deactivated;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return ProductDeleteResult.Deleted;
    }

    private static bool IsDuplicateSkuViolation(DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException
            && (sqlException.Number == 2601 || sqlException.Number == 2627)
            && sqlException.Message.Contains("IX_Products_SKU", StringComparison.OrdinalIgnoreCase);
    }
}
