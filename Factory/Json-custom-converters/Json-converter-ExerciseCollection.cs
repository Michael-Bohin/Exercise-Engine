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
        writer.WriteEndObject();

        writer.WriteEndObject();
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
        /*reader.Read();
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndObject)
            throw new JsonException();
        */

    /*reader.Read();
    if (reader.TokenType != JsonTokenType.StartArray)
        throw new JsonException();

    List<string> result = new();

    reader.Read();
    while (reader.TokenType == JsonTokenType.String) {
        string? s = reader.GetString();
        if (s == null)
            throw new JsonException();
        result.Add(s);
        reader.Read();
    }

    if (reader.TokenType != JsonTokenType.EndArray)
        throw new JsonException();*/

    static void  ReadLocalizations(ref Utf8JsonReader reader, Dictionary<Language, ExerciseLocalization> localizations, JsonSerializerOptions options) {
        reader.Read();
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
    }
    /*
    static void GiantSwitchStatement(ref Utf8JsonReader reader, Exercise exercise, JsonSerializerOptions options) {
        string? propertyName = reader.GetString();
        reader.Read();
        switch (propertyName) {
            case "Id":
                ReadIdTupleArray(ref reader, exercise, options);
                break;
            case "Name":
                string? name = reader.GetString();
                if (name == null)
                    throw new JsonException();
                exercise.SerializerSetName(name);
                break;
            case "Classes":
                ReadEnumArray(ref reader, exercise.Classes, options);
                break;
            case "Topics":
                ReadEnumArray(ref reader, exercise.Topics, options);
                break;
            case "ExerciseType":
                ExerciseType type = (ExerciseType)reader.GetInt32();
                exercise.SerializerSetExerciseType(type);
                break;
            case "Assignment":
                string? ass = reader.GetString();
                if (ass == null)
                    throw new JsonException();
                exercise.SerializerSetAssignment(ass);
                break;
            case "SolutionSteps":
                ReadStringArray(ref reader, exercise.SolutionSteps, options);
                break;
            case "Questions":
                ReadStringArray(ref reader, ((WordProblem)exercise).Questions, options);
                break;
            case "Results":
                ReadStringArray(ref reader, ((WordProblem)exercise).Results, options);
                break;
            case "Result":
                string? result = reader.GetString();
                if (result == null)
                    throw new JsonException();
                ((NumericalExercise)exercise).SerializerSetResult(result);
                break;
            default:
                throw new JsonException($"\nswitch recieved unknown property name: {propertyName}\n");
        }
    }

    static void ReadIdTupleArray(ref Utf8JsonReader reader, Exercise exercise, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException(); // strict json validation -> must change on change of code repr in model
                                        // expects array of size exactly 3, on any other number fails!

        reader.Read(); if (reader.TokenType != JsonTokenType.Number) throw new JsonException();
        ulong exId = reader.GetUInt64();

        reader.Read(); if (reader.TokenType != JsonTokenType.Number) throw new JsonException();
        Language lang = (Language)reader.GetInt32();

        reader.Read(); if (reader.TokenType != JsonTokenType.Number) throw new JsonException();
        int variationId = reader.GetInt32();

        exercise.SerializerSetId(exId, lang, variationId);

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException();
    }

    static void ReadEnumArray<T>(ref Utf8JsonReader reader, List<T> list, JsonSerializerOptions options) where T : struct, IConvertible {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException(); // strict json validation -> must change on change of code repr in model

        reader.Read();
        while (reader.TokenType == JsonTokenType.Number) {
            IConvertible number = reader.GetInt32();
            list.Add((T)number);
            reader.Read();
        }

        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException();
    }

    static void ReadStringArray(ref Utf8JsonReader reader, List<string> list, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException(); // strict json validation -> must change on change of code repr in model

        reader.Read();
        while (reader.TokenType == JsonTokenType.String) {
            string? s = reader.GetString();
            if (s == null)
                throw new JsonException();
            list.Add(s);
            reader.Read();
        }

        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException();
    }*/
}
#pragma warning restore IDE0060 // Remove unused parameter