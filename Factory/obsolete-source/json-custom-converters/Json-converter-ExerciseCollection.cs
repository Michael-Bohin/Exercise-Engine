using ExerciseEngine.source;

namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class ExerciseCollectionConverter : JsonConverter<ExerciseCollection> {

    public override bool CanConvert(Type typeToConvert) => typeof(ExerciseCollection).IsAssignableFrom(typeToConvert);

    static readonly Dictionary<string, Language> StringToLanguage = new() {
        { "pl", Language.pl },
        { "ua", Language.ua },
        { "en", Language.en },
        { "cs", Language.cs }
    };

    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, ExerciseCollection collection, JsonSerializerOptions options) {
        writer.WriteStartObject();

        writer.WriteNumber("id", collection.uniqueId);

        writer.WritePropertyName("va");
        string jsonVar = JsonSerializer.Serialize(collection.variants, options);
        writer.WriteRawValue(jsonVar);

        writer.WritePropertyName("lo");
        string jsonLoc = JsonSerializer.Serialize(collection.localizations, options);
        writer.WriteRawValue(jsonLoc);
        
        writer.WriteEndObject();
    }

    //
    //      >>> Deserialization <<<
    //
    public override ExerciseCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        ExerciseCollection collection = new(0, new(), new());

        while (reader.Read()) {
            if(reader.TokenType == JsonTokenType.PropertyName) {
                string? propertyName = reader.GetString();
                if(propertyName == "id") {
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.Number)
                        throw new JsonException();
                    collection.uniqueId = reader.GetInt32();
                } else if(propertyName == "va") {
                    ReadVariants(ref reader, collection.variants, options);
                } else if(propertyName == "lo") {
                    ReadLocalizations(ref reader, collection.localizations, options);
                } else {
                    throw new JsonException();
                }

            } else if (reader.TokenType == JsonTokenType.EndObject) {
                break;
            } else {
                throw new JsonException();
            }
        }
        reader.Read();
        return collection;
    }

    static void ReadVariants(ref Utf8JsonReader reader, List<Variant> variants, JsonSerializerOptions options) {
        reader.Read();
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();
            Variant? v = JsonSerializer.Deserialize<Variant>(ref reader, options);
            if(v == null)
                throw new JsonException();
            variants.Add(v);
        }
    }

    static void  ReadLocalizations(ref Utf8JsonReader reader, Dictionary<Language, ExerciseLocalization> localizations, JsonSerializerOptions options) {
        reader.Read();// init dict object opening
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read()) {
            if(reader.TokenType == JsonTokenType.PropertyName) {
                // lang
                string? propertyName = reader.GetString();
                if(propertyName == null)
                    throw new JsonException();
                Language lang = StringToLanguage[propertyName];
                // localization
                reader.Read();
                ExerciseLocalization? loc = JsonSerializer.Deserialize<ExerciseLocalization>(ref reader, options);
                if (loc == null)
                    throw new JsonException();
                localizations.Add(lang, loc);

			} else if(reader.TokenType == JsonTokenType.EndObject) {
                break;
			} else { 
                throw new JsonException();    
            }
		}
        reader.Read();
    }
}
#pragma warning restore IDE0060 // Remove unused parameter