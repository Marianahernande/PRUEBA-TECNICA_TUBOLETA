using ECommerce.Application.Abstractions;
using MediatR;

namespace ECommerce.Application.Products;

public record DeleteProductCommand(int Id) : IRequest<Unit>;

public class DeleteProductHandler(IAppDbContext db) : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand req, CancellationToken ct)
    {
        var p = await db.Products.FindAsync(new object?[] { req.Id }, ct)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

        db.Products.Remove(p);
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
