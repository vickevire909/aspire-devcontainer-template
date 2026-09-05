using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloAspire.Shared;

#region Abstractions
public interface IEntityId<TSelf> : IParsable<TSelf>
    where TSelf : struct, IEntityId<TSelf>
{
    Guid Value { get; }

    static abstract TSelf FromValue(Guid Value);

    static virtual TSelf New() => TSelf.FromValue(Guid.CreateVersion7());

    static TSelf IParsable<TSelf>.Parse(string s, IFormatProvider? provider) =>
        TSelf.FromValue(Guid.Parse(s));

    static bool IParsable<TSelf>.TryParse(string? s, IFormatProvider? provider, out TSelf result)
    {
        if (Guid.TryParse(s, out var res))
        {
            result = TSelf.FromValue(res);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }
}

public interface IAuditable
{
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public abstract class Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public TEntityId Id { get; protected set; }

    protected Entity()
    {
        Id = TEntityId.New();
    }

    protected Entity(TEntityId id)
    {
        Id = id;
    }
}
#endregion Abstractions

#region Json
public sealed class EntityIdJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsValueType
            && typeToConvert
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityId<>));
    }

    public override JsonConverter? CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var converterType = typeof(EntityIdJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

public sealed class EntityIdJsonConverter<TEntityId> : JsonConverter<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public override TEntityId Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.Null)
            throw new JsonException("Null is not valid for entity ID.");

        var text = reader.GetString();
        if (string.IsNullOrWhiteSpace(text))
            throw new JsonException("Entity ID cannot be empty.");

        return TEntityId.Parse(text, CultureInfo.InvariantCulture);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TEntityId value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}
#endregion Json
