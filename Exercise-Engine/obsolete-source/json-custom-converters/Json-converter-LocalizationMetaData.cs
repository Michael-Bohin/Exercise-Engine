using ExerciseEngine;

namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class LocalizationMetaDataConverter : JsonConverter<LocalizationMetaData> {

    public override bool CanConvert(Type typeToConvert) => typeof(LocalizationMetaData).IsAssignableFrom(typeToConvert);
    //
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, LocalizationMetaData metaData, JsonSerializerOptions options) {
        writer.WriteStartObject();

        writer.WritePropertyName("ids");
        writer.WriteStartArray();
        writer.WriteNumberValue( metaData.id.id);
        writer.WriteNumberValue( (int)metaData.id.language);
        writer.WriteNumberValue( (int)metaData.type);
        writer.WriteStringValue( metaData.name);
        writer.WriteEndArray();
       
        WriteEnumArray(writer, "top", metaData.topics, options);
        WriteEnumArray(writer, "cla", metaData.classes, options);

        writer.WriteEndObject();
    }

    static void WriteEnumArray<T>(Utf8JsonWriter writer, string propertyName, List<T> list, JsonSerializerOptions options) where T : struct, IConvertible {
		writer.WriteStartArray(propertyName);
		foreach (IConvertible t in list)
			writer.WriteNumberValue((int)t);
		writer.WriteEndArray();
	}

    //
    //      >>> Deserialization <<<
    //
    public override LocalizationMetaData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		LocalizationMetaData metaData = new();
		
		reader.Read();
		if (reader.TokenType != JsonTokenType.PropertyName)
			throw new JsonException();
		string? propertyName = reader.GetString();
		if (propertyName != "ids")
			throw new JsonException();

		reader.Read();
		if (reader.TokenType != JsonTokenType.StartArray)
			throw new JsonException();

		reader.Read();
		if (reader.TokenType != JsonTokenType.Number)
			throw new JsonException();


		int id = reader.GetInt32();

		reader.Read();
		if (reader.TokenType != JsonTokenType.Number)
			throw new JsonException();
		metaData.id = new(id, (Language)reader.GetInt32());

		reader.Read();
		if (reader.TokenType != JsonTokenType.Number)
			throw new JsonException();
		metaData.type = (ExerciseType)reader.GetInt32();

		reader.Read();
		if (reader.TokenType != JsonTokenType.String)
			throw new JsonException();

		string? s = reader.GetString();
		if (s == null)
			throw new JsonException();
		metaData.name = s;


		reader.Read();
		if (reader.TokenType != JsonTokenType.EndArray)
			throw new JsonException();
		
		reader.Read();
		if (reader.TokenType != JsonTokenType.PropertyName)
			throw new JsonException();
		

		propertyName = reader.GetString();
		if(propertyName == null) 
			throw new JsonException();

		metaData.topics = DeserializeEnumArray<Topic>(ref reader, options);
		metaData.classes = DeserializeEnumArray<Classes>(ref reader, options);
		
		if (reader.TokenType != JsonTokenType.EndObject)
			throw new JsonException();
		reader.Read();
		
		return metaData;

	}

	static List<T> DeserializeEnumArray<T>(ref Utf8JsonReader reader, JsonSerializerOptions options) where T : struct, IConvertible {
		List<T> result = new();
		reader.Read();
		if (reader.TokenType != JsonTokenType.StartArray)
			throw new JsonException();

		while(reader.Read()) {
			if(reader.TokenType == JsonTokenType.Number) {
				IConvertible i = reader.GetInt32();
				result.Add((T)i);
			} else if (reader.TokenType == JsonTokenType.EndArray) {
				reader.Read(); // calee can start reading next token..
				return result;
			} else {
				throw new JsonException();
			}
		}

		throw new JsonException();
	}
}

#pragma warning restore IDE0060 // Remove unused parameter