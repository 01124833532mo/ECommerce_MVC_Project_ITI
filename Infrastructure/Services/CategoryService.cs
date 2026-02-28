using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIti.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly MazadContext _context;

    public CategoryService(MazadContext context)
    {
        _context = context;
    }

    public async Task<IList<CategoryVm>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryVm
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentName = c.ParentCategory != null ? c.ParentCategory.Name : null
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CategoryVm?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Where(c => c.CategoryId == id)
            .Select(c => new CategoryVm
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentName = c.ParentCategory != null ? c.ParentCategory.Name : null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(CategoryVm vm)
    {
        var category = new Domain.Entities.Category
        {
            Name = vm.Name,
            ParentCategoryId = vm.ParentCategoryId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category.CategoryId;
    }

    public async Task UpdateAsync(CategoryVm vm)
    {
        var category = await _context.Categories.FindAsync(vm.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        category.Name = vm.Name;
        category.ParentCategoryId = vm.ParentCategoryId;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
