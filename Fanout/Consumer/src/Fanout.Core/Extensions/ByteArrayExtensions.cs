using System.Text;
using System.Text.Json;

namespace Fanout.Core.Extensions;

public static class ByteArrayExtensions
{
    public static byte[] ToBytes<T>(this T obj)
    {
        var data = JsonSerializer.Serialize(obj);
        return Encoding.UTF8.GetBytes(data);
    }

    public static T ToObject<T>(this byte[] bytes)
    {
        var data = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(data)
               ?? throw new Exception("Deserialization issue");
    }
}