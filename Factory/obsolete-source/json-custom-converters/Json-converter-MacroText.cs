using ExerciseEngine.source;

namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class MacroTextConverter : JsonConverter<MacroText> {

	public override bool CanConvert(Type typeToConvert) => typeof(MacroText).IsAssignableFrom(typeToConvert);
	//
	//      >>> Serialization <<<
	//
	public override void Write(Utf8JsonWriter writer, MacroText macroText, JsonSerializerOptions options) {
		// WriteLine(">>>>>>>>>>>>>Did we get here??<<<<<<<<<<<<<<");
		writer.WriteStartObject(); // start macro text object

		writer.WritePropertyName("els");
		writer.WriteStartArray(); // start array of elements

		foreach (var el in macroText.elements)
			writer.WriteRawValue(JsonSerializer.Serialize(el, options));

		writer.WriteEndArray(); // end array of elements 
		writer.WriteEndObject(); // end macro text object
	}

	//
	//      >>> Deserialization <<<
	//
	public override MacroText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		//WriteLine(reader.CurrentDepth);
		MacroText result = new();
		//WriteLine(reader.TokenType);
		// reader.Skip();
		if (reader.TokenType != JsonTokenType.StartObject) 
			throw new JsonException();
		reader.Read();
		//WriteLine(reader.TokenType);
		if(reader.TokenType != JsonTokenType.PropertyName)
			throw new JsonException();
		string? propertyName = reader.GetString();
		if(propertyName != "els")
			throw new JsonException();
		//WriteLine(propertyName);

		reader.Read();
		//WriteLine(reader.TokenType); // startof an array  : List<TextElement>
		if (reader.TokenType != JsonTokenType.StartArray)
			throw new JsonException();

		reader.Read();
		//WriteLine(reader.TokenType);

		//WriteLine("\n\nWhile loop init:\n\n");
		while (true) { 
 			TextElement? textElement = JsonSerializer.Deserialize<TextElement>(ref reader, options);
			if (textElement == null)
				throw new JsonException();
			result.elements.Add(textElement);

			// WriteLine(); WriteLine(reader.TokenType);
			JsonTokenType first = reader.TokenType;
			reader.Read();
			JsonTokenType second = reader.TokenType;

			if(first != JsonTokenType.EndObject) // change back to end arraqy!
				throw new JsonException();

			if(second == JsonTokenType.EndArray) {
				break;
			} else if(second == JsonTokenType.StartObject) { // change back to start arraqy!
				// continue with next iteration of while loop
			} else {
				throw new JsonException();
			}
			
		}
		reader.Read();
		//WriteLine(reader.CurrentDepth);
		return result;
	}
}

#pragma warning restore IDE0060 // Remove unused parameter

//// follows array of text elements..
//while (reader.Read()) {
//	if (reader.TokenType == JsonTokenType.StartArray) {
//		// read next text element..
//		TextElement? te = JsonSerializer.Deserialize<TextElement>(ref reader, options); // ReadTextElement(ref reader , options);
//		if (te == null)
//			throw new JsonException();
//		result.elements.Add(te);
//	} else if (reader.TokenType == JsonTokenType.EndArray) {
//		// do nothing 
//		// carful! might skip more depths... for security count the depth...

//	} else if (reader.TokenType == JsonTokenType.PropertyName) {
//		break;
//	} else {
//		throw new JsonException();
//	}
//}