using ExerciseEngine;
using static System.Console;

namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class ExerciseLocalizationConverter : JsonConverter<ExerciseLocalization> {

    public override bool CanConvert(Type typeToConvert) => typeof(ExerciseLocalization).IsAssignableFrom(typeToConvert);
    //
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, ExerciseLocalization localization, JsonSerializerOptions options) {
		writer.WriteStartObject();

		WriteFieldWithPropertyName(writer, "meta", localization.metaData, options);
		writer.WritePropertyName("as");
		string json = JsonSerializer.Serialize(localization.assignment, options);
		writer.WriteRawValue(json);

		Write_List_MacroText_FieldWithPropertyName(writer, "qs", localization.questions, options);
		Write_List_MacroText_FieldWithPropertyName(writer, "rs", localization.results, options);
		Write_List_MacroText_FieldWithPropertyName(writer, "ss", localization.solutionSteps, options);

		writer.WriteEndObject();
	}

	static void WriteFieldWithPropertyName<T>(Utf8JsonWriter writer, string properyName, T t, JsonSerializerOptions options) {
		writer.WritePropertyName(properyName);
		string json = JsonSerializer.Serialize(t, options);
		writer.WriteRawValue(json);
	}

	static void Write_List_MacroText_FieldWithPropertyName(Utf8JsonWriter writer, string properyName, List<MacroText> list, JsonSerializerOptions options) {
		writer.WritePropertyName(properyName);
		writer.WriteStartArray(); // start of list macro text
		foreach(var mt in list) {
			string json = JsonSerializer.Serialize(mt, options);
			writer.WriteRawValue(json);
		}
			
		writer.WriteEndArray(); // end of list macro text
	}

	//
	//      >>> Deserialization <<<
	//
	public override ExerciseLocalization Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		ShiftToMetaData(ref reader, options);
		var metaData = JsonSerializer.Deserialize<LocalizationMetaData>(ref reader, options);
		if(metaData == null)
			throw new JsonException();
		
		reader.Read();
		AssertTokenIsPropertyWithName(reader.TokenType, reader.GetString(), "as");
		MacroText? assignment = JsonSerializer.Deserialize<MacroText>(ref reader, options);
		if(assignment == null)
			throw new JsonException();

		reader.Read();
		AssertTokenIsPropertyWithName(reader.TokenType, reader.GetString(), "qs");
		reader.Read();
		List<MacroText> questions = DeserializeListMacroText(ref reader, options);
		
		reader.Read();
		AssertTokenIsPropertyWithName(reader.TokenType, reader.GetString(), "rs");
		reader.Read();
		List<MacroText> results = DeserializeListMacroText(ref reader, options);
		reader.Read();
		AssertTokenIsPropertyWithName(reader.TokenType, reader.GetString(), "ss");
		reader.Read();
		List<MacroText> solutionsSteps = DeserializeListMacroText(ref reader, options);
		
		reader.Read();

		ExerciseLocalization localization = new(metaData, assignment, questions, results, solutionsSteps);
		return localization;
    }

	static void AssertTokenIsPropertyWithName(JsonTokenType type, string? actualValue, string expectedValue) {
		if(type != JsonTokenType.PropertyName || actualValue != expectedValue)
			throw new JsonException();
	}

	static void ShiftToMetaData(ref Utf8JsonReader reader, JsonSerializerOptions options) {
		if (reader.TokenType != JsonTokenType.StartObject)
			throw new JsonException();

		// meta property... and on
		reader.Read();
		if (reader.TokenType != JsonTokenType.PropertyName)
			throw new JsonException();
		string? propertyName = reader.GetString();
		if (propertyName != "meta")
			throw new JsonException();

		reader.Read();
		if (reader.TokenType != JsonTokenType.StartObject)
			throw new JsonException();
	}

	static List<MacroText> DeserializeListMacroText(ref Utf8JsonReader reader, JsonSerializerOptions options) {
		// !!
		// do last check what if there are more macro texts in the list, do I hold?
		
		List<MacroText> result = new();
		
		WriteLine(reader.TokenType);
		WriteLine(reader.CurrentDepth);
		//reader.Skip();
		
		if(reader.TokenType != JsonTokenType.StartArray)
			throw new JsonException();

		WriteLine("while init");
		while (reader.Read()) {// odhod initial array start..
			WriteLine(reader.TokenType);
			if (reader.TokenType == JsonTokenType.StartObject) {
				// read next macro text:
				MacroText? macroText = JsonSerializer.Deserialize<MacroText>(ref reader, options);
				if (macroText == null)
					throw new JsonException();
				result.Add(macroText);
			} else if (reader.TokenType == JsonTokenType.EndArray) {
				// List<MacroText> is complete, return to calee..
				break;
			} else {
				throw new JsonException();
			}
		}

		WriteLine(reader.TokenType);
		WriteLine(reader.CurrentDepth);
		return result;
	}
}
#pragma warning restore IDE0060 // Remove unused parameter
//WriteLine(reader.TokenType);
//reader.Read();
//WriteLine(reader.TokenType);
//reader.Read();
//WriteLine(reader.TokenType);
//reader.Read();
//WriteLine(reader.TokenType);
//WriteLine(reader.GetInt32());
//reader.Read();
//WriteLine(reader.TokenType);
//WriteLine(reader.GetString());
//reader.Read();
// reader.Skip();

//static MacroText DeserializeMacroText(ref Utf8JsonReader reader, JsonSerializerOptions options) {
//	MacroText result = new();
//	reader.Read();
//	if (reader.TokenType != JsonTokenType.StartArray) // initilize entire MacroText as Array
//		throw new JsonException();

//	// follows array of text elements..
//	while (reader.Read()) {
//		if (reader.TokenType == JsonTokenType.StartArray) {
//			// read next text element..
//			TextElement? te = JsonSerializer.Deserialize<TextElement>(ref reader, options); // ReadTextElement(ref reader , options);
//			if (te == null)
//				throw new JsonException();
//			result.elements.Add(te);
//		} else if (reader.TokenType == JsonTokenType.EndArray) {
//			// do nothing 
//			// carful! might skip more depths... for security count the depth...

//		} else if (reader.TokenType == JsonTokenType.PropertyName) {
//			break;
//		} else {
//			throw new JsonException();
//		}
//	}

//	return result;
//}


//static void WriteTextElement(Utf8JsonWriter writer, TextElement textElement, JsonSerializerOptions options) {
//	writer.WriteStartArray();
//	if (textElement is Macro m) {
//		writer.WriteNumberValue((int)TextElementDiscriminator.Macro);
//		writer.WriteNumberValue(m.pointer);
//		writer.WriteNumberValue((int)m.type);
//	} else if (textElement is Text t) {
//		writer.WriteNumberValue((int)TextElementDiscriminator.Text);
//		writer.WriteStringValue(t.constText);
//	} else {
//		throw new JsonException("WriteTextElement hit unknown TextElement child");
//	}
//	writer.WriteEndArray();
//}

//static TextElement ReadTextElement(ref Utf8JsonReader reader, JsonSerializerOptions options) {
//	// first item is type discriminator, follow two or one based on Macro/Text instance
//	// WriteLine(reader.TokenType);
//	if (reader.TokenType != JsonTokenType.StartArray)
//		throw new JsonException();
//	reader.Read();
//	var typeDisc = (TextElementDiscriminator)reader.GetInt32();
//	TextElement result;

//	if (typeDisc == TextElementDiscriminator.Macro) {
//		reader.Read();
//		if (reader.TokenType != JsonTokenType.Number)
//			throw new JsonException();
//		int pointer = reader.GetInt32();
//		reader.Read();
//		if (reader.TokenType != JsonTokenType.Number)
//			throw new JsonException();
//		var varDisc = (VariableDiscriminator)reader.GetInt32();
//		result = new Macro(pointer, varDisc);

//	} else if (typeDisc == TextElementDiscriminator.Text) {
//		reader.Read();
//		if (reader.TokenType != JsonTokenType.String)
//			throw new JsonException();
//		string? constText = reader.GetString();
//		if (constText == null)
//			throw new JsonException();
//		result = new Text(constText);

//	} else {
//		throw new JsonException();
//	}
//	reader.Read();
//	if (reader.TokenType != JsonTokenType.EndArray)
//		throw new JsonException();


//	return result;
//}