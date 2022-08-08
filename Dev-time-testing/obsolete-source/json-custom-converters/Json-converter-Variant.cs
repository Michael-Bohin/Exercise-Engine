using ExerciseEngine;

namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class VariantConverter : JsonConverter<Variant> {

    public override bool CanConvert(Type typeToConvert) => typeof(Variant).IsAssignableFrom(typeToConvert);
    //
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, Variant variant, JsonSerializerOptions options) {
        writer.WriteStartArray();

        string jsonInvariant = JsonSerializer.Serialize(variant.invariant, options);
        writer.WriteRawValue(jsonInvariant);
        
        foreach(var v in variant.cultural) { // save one depth
            string jsonCultural = JsonSerializer.Serialize(v, options);
            writer.WriteRawValue(jsonCultural);
        }

        writer.WriteEndArray();
    }

    //
    //      >>> Deserialization <<<
    //
    public override Variant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

		if (reader.TokenType != JsonTokenType.StartArray)
			throw new JsonException();
        reader.Read();
        Variant variant = new();
        ReadStringArray(ref reader,  variant.invariant, options);

        reader.Read();

        while(reader.TokenType == JsonTokenType.StartObject) {
            // next dictionary to be added to variant.cultural..
            Dictionary<Language, string>? dict = JsonSerializer.Deserialize<Dictionary<Language, string>>(ref reader, options);
            if(dict == null)
                throw new JsonException();
            variant.cultural.Add(dict);
            reader.Read();
		}

        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException();
       
        return variant;
    }

    static void ReadStringArray(ref Utf8JsonReader reader, List<string> list, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException(); // strict json validation -> must change on change of code repr in model
        
        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.String) {
                string? s = reader.GetString();
                if (s == null)
                    throw new JsonException();
                list.Add(s);
            } else if (reader.TokenType == JsonTokenType.EndArray) {
                break;
            } else {
                throw new JsonException();
            }   
        }   
    }
}

#pragma warning restore IDE0060 // Remove unused parameter