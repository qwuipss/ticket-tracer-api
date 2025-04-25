namespace TicketTracer.Api.Utilities;

internal class GuidFactory : IGuidFactory
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}