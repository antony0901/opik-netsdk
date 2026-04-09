using OpikSimplSdk.Http.Infrastructure;

namespace OpikSimplSdk.Http.Clients;

internal abstract class ClientBase
{
    protected ClientBase(IOpikHttpTransport transport)
    {
        Transport = transport;
    }

    protected IOpikHttpTransport Transport { get; }

    protected static string WithQuery(string path, params (string Key, object? Value)[] query)
    {
        var parts = query
            .Where(q => q.Value is not null)
            .Select(q => $"{Uri.EscapeDataString(q.Key)}={Uri.EscapeDataString(q.Value!.ToString()!)}")
            .ToArray();

        if (parts.Length == 0)
        {
            return path;
        }

        var separator = path.Contains('?') ? "&" : "?";
        return path + separator + string.Join("&", parts);
    }
}
