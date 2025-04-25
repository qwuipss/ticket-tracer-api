namespace TicketTracer.Api.Utilities;

internal interface ISentry
{
    public Task ReportAsync(Exception exc);
}