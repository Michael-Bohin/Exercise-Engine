namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class TextElementConverter : JsonConverter<TextElement> {

    public override bool CanConvert(Type typeToConvert) => typeof(TextElement).IsAssignableFrom(typeToConvert);
    //
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, TextElement textElement, JsonSerializerOptions options) {
        writer.WriteStartArray();
        if (textElement is Macro m) {
            writer.WriteNumberValue((int)TextElementDiscriminator.Macro);
            writer.WriteNumberValue(m.pointer);
            writer.WriteNumberValue((int)m.type);
        } else if (textElement is Text t) {
            writer.WriteNumberValue((int)TextElementDiscriminator.Text);
            writer.WriteStringValue(t.constText);
        } else {
            throw new JsonException("WriteTextElement hit unknown TextElement child");
        }
        writer.WriteEndArray();
    }

    //
    //      >>> Deserialization <<<
    //
    public override TextElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		// first item is type discriminator, follow two or one based on Macro/Text instance

		if (reader.TokenType != JsonTokenType.StartArray)
			throw new JsonException();
		reader.Read();
		var typeDisc = (TextElementDiscriminator)reader.GetInt32();
		TextElement result;

		if (typeDisc == TextElementDiscriminator.Macro) {
			reader.Read();
			if (reader.TokenType != JsonTokenType.Number)
				throw new JsonException();
			int pointer = reader.GetInt32();
			reader.Read();
			if (reader.TokenType != JsonTokenType.Number)
				throw new JsonException();
			var varDisc = (VariableDiscriminator)reader.GetInt32();
			result = new Macro(pointer, varDisc);

		} else if (typeDisc == TextElementDiscriminator.Text) {
			reader.Read();
			if (reader.TokenType != JsonTokenType.String)
				throw new JsonException();
			string? constText = reader.GetString();
			if (constText == null)
				throw new JsonException();
			result = new Text(constText);

		} else {
			throw new JsonException();
		}
		reader.Read();
		if (reader.TokenType != JsonTokenType.EndArray)
			throw new JsonException();

		return result;
	}
}

#pragma warning restore IDE0060 // Remove unused parameter