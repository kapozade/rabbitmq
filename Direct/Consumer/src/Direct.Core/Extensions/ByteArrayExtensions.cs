using System.Text;
using System.Text.Json;

namespace Direct.Core.Extensions;

public static class ByteArrayExtensions
{
    public static T ToObject<T>(this byte[] exampleAsBytes)
    {
        var data = Encoding.UTF8.GetString(exampleAsBytes);
        return JsonSerializer.Deserialize<T>(data)
            ?? throw new Exception("Deserialization issue!");
    }

    public static byte[] ToBytes<T>(this T obj)
    {
        var data = JsonSerializer.Serialize(obj);
        return Encoding.UTF8.GetBytes(data);
    }
}