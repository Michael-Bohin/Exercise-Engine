namespace ExerciseEngine.Factory;

class ExerciseConverterWithTypeDiscriminator : JsonConverter<Exercise> {
    enum TypeDiscriminator {
        WordProblem = 1,
        NumericalExercise = 2
    }

    public override bool CanConvert(Type typeToConvert) => typeof(Exercise).IsAssignableFrom(typeToConvert);

    // Deserialization:
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
                    case "MetaData":
                        int value = reader.GetInt32();
                        ((Macro)exercise).SerializerSetPointer(value);
                        break;
                    case "Type":
                        VariableDiscriminator type = (VariableDiscriminator)reader.GetInt32();
                        ((Macro)exercise).SerializerSetDiscriminator(type);
                        break;
                    case "ConstText":
                        string? constText = reader.GetString();
                        if(constText == null)
                            throw new JsonException();
                        ((Text)exercise).SerializerSetText(constText);
                        break;
                }
            }
        }
         
        throw new JsonException();
    }

    // Serialization:
    public override void Write(
        Utf8JsonWriter writer, Exercise exercise, JsonSerializerOptions options) {
        writer.WriteStartObject();

        if (exercise is Macro macro) {
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Macro);
            writer.WriteNumber("Pointer", macro.Pointer);
            writer.WriteNumber("VariableDiscriminator", (int)macro.Type);
        } else if (exercise is Text text) {
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Text);
            writer.WriteString("ConstText", text.ConstText);
        } else {
            throw new JsonException();
        }

        // writer.WriteString("Name", person.Name);

        writer.WriteEndObject();
    }
}
