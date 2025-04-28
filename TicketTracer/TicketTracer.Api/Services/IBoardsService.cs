using OneOf;
using TicketTracer.Api.Models.Common;
using TicketTracer.Api.Models.Request.Boards;
using TicketTracer.Api.Models.Response.Boards;
using TicketTracer.Api.Models.Response.Projects;

namespace TicketTracer.Api.Services;

internal interface IBoardsService
{
    Task<BoardModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<BoardModel>?> GetAllAsync(int offset, int limit, CancellationToken cancellationToken);

    Task<List<BoardModel>?> GetAllByProjectIdAsync(Guid projectId, int offset, int limit, CancellationToken cancellationToken);

    Task<OneOf<BoardModel, BoardExistsModel, ProjectNotFoundModel>> CreateAsync(CreateBoardModel model, CancellationToken cancellationToken);
}