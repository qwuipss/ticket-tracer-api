using System.Security.Claims;
using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Tickets;
using TicketTracer.Api.Models.Response.Boards;

namespace TicketTracer.Api.Services;

internal interface ITicketsService
{
    Task<TicketModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<TicketModel>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken);

    Task<List<TicketModel>?> GetAllByBoardIdAsync(Guid boardId, int offset, int limit, CancellationToken cancellationToken);

    Task<OneOf<TicketModel, BoardNotFoundModel>> CreateAsync(CreateTicketModel model, ClaimsPrincipal user, CancellationToken cancellationToken);
}