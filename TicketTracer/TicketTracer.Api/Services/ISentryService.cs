namespace TicketTracer.Api.Services;

public interface ISentryService
{
    public Task ReportAsync(Exception exc);
}