using ECommerce.Application.Abstractions;
using MediatR;

namespace ECommerce.Application.Categories;

public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

public class DeleteCategoryHandler(IAppDbContext db) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryCommand req, CancellationToken ct)
    {
        var cat = await db.Categories.FindAsync(new object?[] { req.Id }, ct)
                  ?? throw new KeyNotFoundException("Categoría no encontrada.");

        db.Categories.Remove(cat);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
