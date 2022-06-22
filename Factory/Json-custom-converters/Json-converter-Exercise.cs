namespace ExerciseEngine.Factory;
#pragma warning disable IDE0060 // Remove unused parameter

class ExerciseConverter : JsonConverter<Exercise> {
    enum TypeDiscriminator {
        WordProblem = 1,
        NumericalExercise = 2
    }

    public override bool CanConvert(Type typeToConvert) => typeof(Exercise).IsAssignableFrom(typeToConvert);
    //
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        // WriteLine("Called Exercise serializer");
        writer.WriteStartObject();

        WriteTypeDiscriminator(writer, exercise, options);
        WriteExerciseProperties(writer, exercise, options);
        WritePolymorphicChildrenProperties(writer, exercise, options);

        writer.WriteEndObject();
    }


    static void WriteTypeDiscriminator(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        if (exercise is WordProblem)
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.WordProblem);
        else if (exercise is NumericalExercise)
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.NumericalExercise);
        else
            throw new JsonException();
    }

    static void WriteExerciseProperties(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        writer.WritePropertyName("id");
        writer.WriteStartArray();
        writer.WriteNumberValue(exercise.metaData.uniqueId.id);
        writer.WriteNumberValue((int)exercise.metaData.uniqueId.language);
        writer.WriteNumberValue(exercise.metaData.uniqueId.variant);
        writer.WriteEndArray();
        writer.WriteString("na", exercise.metaData.name);
        WriteEnumArray(writer, "cl", exercise.metaData.classes, options); // List<Classes> Classes
        WriteEnumArray(writer, "to", exercise.metaData.topics, options); // List<Topics> Topics
        writer.WriteNumber("exT", (int)exercise.metaData.type);
        writer.WriteString("as", exercise.assignment);
        WriteStringArray(writer, "ss", exercise.solutionSteps, options); // List<string> SolutionSteps
    }

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

    static void WritePolymorphicChildrenProperties(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        if (exercise is WordProblem wp) {
            WriteStringArray(writer, "qs", wp.questions, options); // List<string> Questions
            WriteStringArray(writer, "rs", wp.results, options); // List<string> Results
        } else if (exercise is NumericalExercise ne) {
            writer.WriteString("re", ne.result);
        } else {
            throw new JsonException();
        }
    }

    //
    //      >>> Deserialization <<<
    //
    public override Exercise Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // WriteLine("Called Exercise DESERILIZER");
        if (reader.TokenType != JsonTokenType.StartObject) 
            throw new JsonException();

        Exercise exercise = InstantiateChild(ref reader);

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndObject)
                return exercise;
            if (reader.TokenType == JsonTokenType.PropertyName)     
                GiantSwitchStatement(ref reader, exercise, options);
        }
         
        throw new JsonException();
    }

    static Exercise InstantiateChild(ref Utf8JsonReader reader) {
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();

        string? propertyName = reader.GetString();
        if (propertyName != "TypeDiscriminator")
            throw new JsonException();

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
        Exercise exercise = typeDiscriminator switch {
            TypeDiscriminator.WordProblem => new WordProblem(),
            TypeDiscriminator.NumericalExercise => new NumericalExercise(),
            _ => throw new JsonException()
        };
        return exercise;
    }

    static void GiantSwitchStatement(ref Utf8JsonReader reader, Exercise exercise, JsonSerializerOptions options) {
        string? propertyName = reader.GetString();
        reader.Read();
        switch (propertyName) {
            case "id":
                ReadIdTupleArray(ref reader, exercise, options);
                break;
            case "na":
                string? name = reader.GetString();
                if (name == null)
                    throw new JsonException();
                exercise.SerializerSetName(name);
                break;
            case "cl":
                ReadEnumArray(ref reader, exercise.metaData.classes, options);
                break;
            case "to":
                ReadEnumArray(ref reader, exercise.metaData.topics, options);
                break;
            case "exT":
                ExerciseType type = (ExerciseType)reader.GetInt32();
                exercise.SerializerSetExerciseType(type);
                break;
            case "as":
                string? ass = reader.GetString();
                if (ass == null)
                    throw new JsonException();
                exercise.SerializerSetAssignment(ass);
                break;
            case "ss":
                ReadStringArray(ref reader, exercise.solutionSteps, options);
                break;
            case "qs":
                ReadStringArray(ref reader, ((WordProblem)exercise).questions, options);
                break;
            case "rs":
                ReadStringArray(ref reader, ((WordProblem)exercise).results, options);
                break;
            case "re":
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
        int exId = reader.GetInt32();

        reader.Read(); if (reader.TokenType != JsonTokenType.Number) throw new JsonException();
        Language lang = (Language)reader.GetInt32();

        reader.Read(); if (reader.TokenType != JsonTokenType.Number) throw new JsonException();
        int variantId = reader.GetInt32();
        
        exercise.SerializerSetId(exId, lang, variantId);

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
            if(s == null)
                throw new JsonException();
            list.Add(s);
            reader.Read();
        }

        if (reader.TokenType != JsonTokenType.EndArray)
            throw new JsonException();
    }

    
}
#pragma warning restore IDE0060 // Remove unused parameter
