using ECommerce.Application.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories;

public record UpdateCategoryCommand(int Id, string Name) : IRequest<CategoryDto>;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class UpdateCategoryHandler(IAppDbContext db) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand req, CancellationToken ct)
    {
        var cat = await db.Categories.FirstOrDefaultAsync(c => c.Id == req.Id, ct)
                  ?? throw new KeyNotFoundException("Categoría no encontrada.");

        var nameTaken = await db.Categories.AnyAsync(c => c.Id != req.Id && c.Name == req.Name, ct);
        if (nameTaken) throw new InvalidOperationException("El nombre ya existe.");

        cat.Name = req.Name.Trim();
        await db.SaveChangesAsync(ct);

        return new CategoryDto(cat.Id, cat.Name);
    }
}
