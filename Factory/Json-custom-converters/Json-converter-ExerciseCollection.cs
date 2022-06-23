using System.Collections.Immutable;

namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter



class ExerciseCollectionConverter : JsonConverter<ExerciseCollection> {

    public override bool CanConvert(Type typeToConvert) => typeof(ExerciseCollection).IsAssignableFrom(typeToConvert);

    private static readonly Dictionary<string, Language> strToLang = new() {
        { "cs", Language.cs },
        { "pl", Language.pl },
        { "ua", Language.ua },
        { "en", Language.en }
    };
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, ExerciseCollection collection, JsonSerializerOptions options) {
        writer.WriteStartObject();

        // id:
        writer.WriteNumber("id", collection.uniqueId);
        
        // variations:
        writer.WritePropertyName("va");
        writer.WriteStartArray();
        WriteVariants(writer, collection.variants, options);
        writer.WriteEndArray();

        // localizations:
        writer.WritePropertyName("lo");
        writer.WriteStartObject();
        WriteLocalizations(writer, collection.localizations, options);
        writer.WriteEndObject(); // end localizations

        writer.WriteEndObject(); // end json
    }

    static void WriteVariants(Utf8JsonWriter writer, List<Variant> variations, JsonSerializerOptions options) {
        foreach(var variation in variations) {
            writer.WriteStartObject();
            
            writer.WritePropertyName("inv");
            writer.WriteStartArray();
            foreach(string s in variation.invariant) 
                writer.WriteStringValue(s);
            writer.WriteEndArray();

            writer.WritePropertyName("cul");
            writer.WriteStartArray();

            foreach (var dict in variation.cultural) {
                writer.WriteStartObject();
                
                foreach(var kvp in dict) {
                    writer.WriteString(kvp.Key.ToString(), kvp.Value);
				}

                writer.WriteEndObject();
			}

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
	}

    static void WriteLocalizations(Utf8JsonWriter writer, Dictionary<Language, ExerciseLocalization> localizations, JsonSerializerOptions options) {
        foreach(var kvp in localizations) {
            writer.WritePropertyName(kvp.Key.ToString());
            WriteLocalization(writer, kvp.Value, options);
		}
    }

    static void WriteLocalization(Utf8JsonWriter writer, ExerciseLocalization localization, JsonSerializerOptions options) {
        writer.WriteStartObject();
        WriteMetaData(writer, "meta", localization.metaData, options);
		writer.WritePropertyName("as"); 
        WriteMacroText(writer, localization.assignment, options);
        writer.WritePropertyName("qs");
        Write_List_MacroText(writer, localization.questions, options);
        writer.WritePropertyName("rs");
        Write_List_MacroText(writer, localization.results, options);
        writer.WritePropertyName("ss");
        Write_List_MacroText(writer, localization.solutionSteps, options);
        writer.WriteEndObject();
	}

    static void WriteMetaData(Utf8JsonWriter writer, string properyName, LocalizationMetaData metaData, JsonSerializerOptions options) {
        writer.WritePropertyName(properyName);
        writer.WriteStartObject();

        writer.WriteNumber("id", metaData.uniqueId.id);
        writer.WriteNumber("lang", (int)metaData.uniqueId.language);
        writer.WriteString("name", metaData.name);
        writer.WriteNumber("type", (int)metaData.type);

        WriteEnumArray(writer, "topics", metaData.topics, options);
        WriteEnumArray(writer, "classes", metaData.classes, options);

        writer.WriteEndObject();
    }

    static void Write_List_MacroText(Utf8JsonWriter writer, List<MacroText> list, JsonSerializerOptions options) {
        writer.WriteStartArray();
        foreach (MacroText mt in list)
            WriteMacroText(writer, mt, options);
        writer.WriteEndArray();
    }

    static void WriteMacroText(Utf8JsonWriter writer, MacroText macroText, JsonSerializerOptions options) {
        writer.WriteStartArray();
        foreach (var textElement in macroText.elements) 
            WriteTextElement(writer, textElement, options);
        writer.WriteEndArray();
    }

    static void WriteTextElement(Utf8JsonWriter writer, TextElement textElement, JsonSerializerOptions options) {
        writer.WriteStartArray();
        // expects to write discriminator and based on value appropriate number of following elements..
        // on Macro follows int Point and VariableDiscriminator type (not this, but the of the variable the pointer is pointing to...)
        // on ConsText follows string different from null
        if(textElement is Macro m) {
            writer.WriteNumberValue((int)TextElementDiscriminator.Macro);
            writer.WriteNumberValue(m.pointer);
            writer.WriteNumberValue((int)m.type);
		} else if(textElement is Text t) {
            writer.WriteNumberValue((int)TextElementDiscriminator.Text);
            writer.WriteStringValue(t.constText);
		} else {
            throw new JsonException("WriteTextElement hit unknown TextElement child");
		}

        writer.WriteEndArray();
    }

    

    


    /*
    static void WriteCollectionProperties(Utf8JsonWriter writer, ExerciseCollection exercise, JsonSerializerOptions options) {
        writer.WritePropertyName("Id");
        writer.WriteStartArray();
        writer.WriteNumberValue(exercise.Id.exerciseId);
        writer.WriteNumberValue((int)exercise.Id.language);
        writer.WriteNumberValue(exercise.Id.variationId);
        writer.WriteEndArray();
        writer.WriteString("Name", exercise.Name);
        WriteEnumArray(writer, "Classes", exercise.Classes, options); // List<Classes> Classes
        WriteEnumArray(writer, "Topics", exercise.Topics, options); // List<Topics> Topics
        writer.WriteNumber("ExerciseType", (int)exercise.ExerciseType);
        writer.WriteString("Assignment", exercise.Assignment);
        WriteStringArray(writer, "SolutionSteps", exercise.SolutionSteps, options); // List<string> SolutionSteps
    }*/

    static void WriteEnumArray<T>(Utf8JsonWriter writer, string propertyName, List<T> list, JsonSerializerOptions options) where T : struct, IConvertible {
        writer.WriteStartArray(propertyName);
        foreach (IConvertible t in list)
            writer.WriteNumberValue((int)t);
        writer.WriteEndArray();
    }

    static void WriteStringArray(Utf8JsonWriter writer, string propertyName, List<string> list, JsonSerializerOptions options) {
        writer.WriteStartArray(propertyName);
        foreach (string s in list)
            writer.WriteStringValue(s);
        writer.WriteEndArray();
    }

    //
    //      >>> Deserialization <<<
    //
    public override ExerciseCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        ExerciseCollection collection = new(0, new(), new());

        while (reader.Read()) {
            WriteLine(reader.TokenType);
            if (reader.TokenType == JsonTokenType.EndObject)
                break;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            string? propertyName = reader.GetString();

            switch (propertyName) {
                case "id":
                    reader.Read();
                    if(reader.TokenType != JsonTokenType.Number)
                        throw new JsonException();
                    int id = reader.GetInt32();
                    collection.uniqueId = id;
                    break;
                case "va":
                    ReadVariants(ref reader, collection.variants, options);
                    break;
                case "lo":
                    ReadLocalizations(ref reader, collection.localizations, options);
                    break;
                default:
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

        // nasleduje n objektu, kazdy ma dve property: "inv" a "cul" (array of strings a array of dictionaries)
        
        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            Variant v = DeserializeVariant(ref reader, options);
            variants.Add(v);
        }
    }

    static Variant DeserializeVariant(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        reader.Read();
        if(reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();

        string? propertyName = reader.GetString();
        if(propertyName != "inv")
            throw new JsonException();

        // property name is "inv" :
        List<string> invariant = DeserializeStringArray(ref reader, options);

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        propertyName = reader.GetString();
        if (propertyName != "cul")
            throw new JsonException();
        // property name is "cul" : !! follows list <dict string string> !!

        List<Dictionary<Language, string>> cultural = Deserialize_Array_Dictionary_StringString(ref reader, options);

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndObject)
            throw new JsonException();

        return new(invariant, cultural);
    }

    static List<string> DeserializeStringArray(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        reader.Read();
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        List<string> result = new();

        while (reader.Read()) {
            if(reader.TokenType == JsonTokenType.EndArray)
                break;
            if(reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            string? s = reader.GetString();
            if (s == null)
                throw new JsonException();
            result.Add(s);
        }

        return result;
    }

    static List<Dictionary<Language, string>> Deserialize_Array_Dictionary_StringString(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        reader.Read();
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        List<Dictionary<Language, string>> array = new();

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            Dictionary<Language, string> dict = Deseriliaze_Dictionary_StringString(ref reader, options);
            array.Add(dict);
        }
        
        return array;
    } 

    static Dictionary<Language, string> Deseriliaze_Dictionary_StringString(ref Utf8JsonReader reader, JsonSerializerOptions options) {
        Dictionary<Language, string> dict = new();

        while (reader.Read()) {
            if(reader.TokenType == JsonTokenType.EndObject)
                break;
            if(reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string? propertyName = reader.GetString()!;
            Language language = strToLang[propertyName];
            
            reader.Read();
            if(reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            string? value = reader.GetString();
            if(value == null)
                throw new JsonException();
            dict.Add(language, value);
		}
        
        return dict;
	}

    static void  ReadLocalizations(ref Utf8JsonReader reader, Dictionary<Language, ExerciseLocalization> localizations, JsonSerializerOptions options) {
        reader.Skip();
        /*reader.Read();
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();*/
    }
}
#pragma warning restore IDE0060 // Remove unused parameter