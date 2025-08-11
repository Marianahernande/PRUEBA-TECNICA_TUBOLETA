using ECommerce.Application.Abstractions;
using ECommerce.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories;

public record CreateCategoryCommand(string Name) : IRequest<CategoryDto>;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class CreateCategoryHandler(IAppDbContext db) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand req, CancellationToken ct)
    {
        var exists = await db.Categories.AnyAsync(c => c.Name == req.Name, ct);
        if (exists) throw new InvalidOperationException("La categoría ya existe.");

        var cat = new Category { Name = req.Name.Trim() };
        db.Categories.Add(cat);
        await db.SaveChangesAsync(ct);
        return new CategoryDto(cat.Id, cat.Name);
    }
}
