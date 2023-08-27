using System.Text.Json;

namespace RecSum.IntegrationTests.Infrastructure;

public static class Extensions
{
    public static async Task<T?> GetContentAsync<T>(this HttpResponseMessage source)
    {
        var str = await source.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
    }
}