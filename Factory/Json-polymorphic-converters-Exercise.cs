namespace ExerciseEngine.Factory;

class ExerciseConverterWithTypeDiscriminator : JsonConverter<Exercise> {
    enum TypeDiscriminator {
        WordProblem = 1,
        NumericalExercise = 2
    }

    public override bool CanConvert(Type typeToConvert) => typeof(Exercise).IsAssignableFrom(typeToConvert);

    //
    //      >>> Deserialization <<<
    //
    public override Exercise Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartObject) 
            throw new JsonException();

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

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndObject)
                return exercise;
            
            if (reader.TokenType == JsonTokenType.PropertyName) {
                propertyName = reader.GetString();
                reader.Read();
                switch (propertyName) {
                    case "UniqueId":
                        ulong id = reader.GetUInt64();
                        exercise.SerializerSetUniqueId(id);
                        break;
                    case "Name":
                        string? name = reader.GetString();
                        if (name == null)
                            throw new JsonException();
                        exercise.SerializerSetName(name);
                        break;
                    case "Lang":
                        Language lang = (Language)reader.GetInt32();
                        exercise.SerializerSetLang(lang);
                        break; 
                    case "VariationId":
                        int variation = reader.GetInt32();
                        exercise.SerializerSetVariationId(variation);
                        break;
                    case "Classes":
                        JsonTokenType token = reader.TokenType;
                        if(token != JsonTokenType.StartArray)
                            throw new JsonException(); // strict json validation -> must change on change of code repr in model
                        WriteLine(token);

                        reader.Read();
                        token = reader.TokenType;
                        WriteLine(token);

                        int number = reader.GetInt32();
                        WriteLine(number);
                        exercise.Classes.Add((Classes)number);

                        reader.Read();
                        token = reader.TokenType;
                        WriteLine(token);



                        // i dont know how to define List<T> deserialization yet..
                        break;
                    case "Topics":
                        // i dont know how to define List<T> deserialization yet..
                        token = reader.TokenType;
                        WriteLine(token);
                        if (token != JsonTokenType.StartArray)
                            throw new JsonException(); // strict json validation -> must change on change of code repr in model

                        reader.Read();
                        token = reader.TokenType;
                        WriteLine(token);
                        number = reader.GetInt32();
                        WriteLine(number);
                        exercise.Topics.Add((Topic)number);

                        reader.Read();
                        token = reader.TokenType;
                        WriteLine(token);
                        number = reader.GetInt32();
                        WriteLine(number);
                        exercise.Topics.Add((Topic)number);

                        reader.Read();
                        token = reader.TokenType;
                        WriteLine(token);
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
                        // i dont know how to define List<T> deserialization yet..
                        break;

                    case "Questions":
                        // i dont know how to define List<T> deserialization yet..
                        break;

                    case "Results":
                        // i dont know how to define List<T> deserialization yet..
                        break;
                    case "Result":
                        string? result = reader.GetString();
                        if (result == null)
                            throw new JsonException();
                        ((NumericalExercise)exercise).SerializerSetResult(result);
                        break;
                }
            }
        }
         
        throw new JsonException();
    }

    //
    //      >>> Serialization <<<
    //
    public override void Write(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        writer.WriteStartObject();

        WriteTypeDiscriminator(writer, exercise, options);
        WriteExerciseProperties(writer, exercise, options);
        WritePolymorphicChildrenProperties(writer, exercise, options);

        writer.WriteEndObject();
    }

    #pragma warning disable IDE0060 // Remove unused parameter
    static void WriteTypeDiscriminator(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        if (exercise is WordProblem)
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.WordProblem);
        else if (exercise is NumericalExercise)
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.NumericalExercise);
        else
            throw new JsonException();
    }

    static void WriteExerciseProperties(Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        writer.WriteNumber("UniqueId", exercise.UniqueId);
        writer.WriteString("Name", exercise.Name);
        writer.WriteNumber("Lang", (int)exercise.Lang);
        writer.WriteNumber("VariationId", exercise.VariationId);
		WriteEnumArray(writer, "Classes", exercise.Classes, options); // List<Classes> Classes
        WriteEnumArray(writer, "Topics", exercise.Topics, options); // List<Topics> Topics
        writer.WriteNumber("ExerciseType", (int)exercise.ExerciseType);
        writer.WriteString("Assignment", exercise.Name);
        WriteStringArray(writer, "SolutionSteps", exercise.SolutionSteps, options); // List<string> SolutionSteps
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
            WriteStringArray(writer, "Questions", wp.Questions, options); // List<string> Questions
            WriteStringArray(writer, "Results", wp.Results, options); // List<string> Results
        } else if (exercise is NumericalExercise ne) {
            writer.WriteString("Result", ne.Result);
        } else {
            throw new JsonException();
        }
    }
    #pragma warning restore IDE0060 // Remove unused parameter
}
