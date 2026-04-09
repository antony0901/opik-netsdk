using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpikSimplSdk.Core.Common;

[JsonConverter(typeof(OptionalJsonConverterFactory))]
public readonly struct Optional<T>
{
    private readonly T? _value;

    public Optional(T? value)
    {
        _value = value;
        IsSet = true;
    }

    public bool IsSet { get; }
    public T? Value => _value;

    public static Optional<T> Unset => default;

    public static implicit operator Optional<T>(T? value) => new(value);
}

public sealed class OptionalJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(OptionalJsonConverter<>).MakeGenericType(valueType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private sealed class OptionalJsonConverter<TValue> : JsonConverter<Optional<TValue>>
    {
        public override Optional<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = JsonSerializer.Deserialize<TValue>(ref reader, options);
            return new Optional<TValue>(value);
        }

        public override void Write(Utf8JsonWriter writer, Optional<TValue> value, JsonSerializerOptions options)
        {
            if (!value.IsSet)
            {
                writer.WriteNullValue();
                return;
            }

            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
