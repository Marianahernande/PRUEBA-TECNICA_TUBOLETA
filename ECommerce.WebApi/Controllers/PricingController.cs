using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Pricing;

namespace ECommerce.WebApi.Controllers


{

    /// Pricing dinámico: calcula y aplica precios en función de demanda, inventario,
    /// competencia simulada y estacionalidad.

    [ApiController]
    [Route("api/[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PricingController(IMediator mediator) => _mediator = mediator;

        /// Devuelve el precio calculado sin modificar el producto.
        [HttpGet("preview")]
        public Task<decimal> Preview([FromQuery] int productId, [FromQuery] DateTime? date)
            => _mediator.Send(new GetPricePreviewQuery(productId, date ?? DateTime.UtcNow));

        /// Aplica el precio calculado y guarda PriceHistory (solo Admin).
        [Authorize(Roles = "Admin")]
        [HttpPost("apply")]
        public Task<decimal> Apply([FromBody] ApplyPriceCommand cmd)
            => _mediator.Send(cmd);
            
            [HttpGet("history")]
public Task<List<PricePointDto>> History([FromQuery] int productId, [FromQuery] int days = 30)
    => _mediator.Send(new GetPriceHistoryQuery(productId, days));
    }
}
