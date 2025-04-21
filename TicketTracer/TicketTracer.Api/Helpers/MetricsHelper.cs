namespace TicketTracer.Api.Helpers;

public static class MetricsHelper
{
    public static KeyValuePair<string, object?> CreateTag(string key, string value)
    {
        return KeyValuePair.Create<string, object?>(key, value);
    }
}