namespace ExerciseEngine.Factory;

class PersonConverterWithTypeDiscriminator : JsonConverter<TextElement> {
    enum TypeDiscriminator {
        Macro = 1,
        Text = 2
    }

    public override bool CanConvert(Type typeToConvert) => typeof(TextElement).IsAssignableFrom(typeToConvert);

    // Deserialization:
    public override TextElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
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
        TextElement textElement = typeDiscriminator switch {
            TypeDiscriminator.Macro => new Macro(),
            TypeDiscriminator.Text => new Text(),
            _ => throw new JsonException()
        };

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndObject)
                return textElement;
            
            if (reader.TokenType == JsonTokenType.PropertyName) {
                propertyName = reader.GetString();
                reader.Read();
                switch (propertyName) {
                    case "Pointer":
                        int value = reader.GetInt32();
                        ((Macro)textElement).SerializerSetPointer(value);
                        break;
                    case "Type":
                        VariableDiscriminator type = (VariableDiscriminator)reader.GetInt32();
                        ((Macro)textElement).SerializerSetDiscriminator(type);
                        break;
                    case "ConstText":
                        string? constText = reader.GetString();
                        if(constText == null)
                            throw new JsonException();
                        ((Text)textElement).SerializerSetText(constText);
                        break;
                }
            }
        }
         
        throw new JsonException();
    }

    // Serialization:
    public override void Write(
        Utf8JsonWriter writer, TextElement textElement, JsonSerializerOptions options) {
        writer.WriteStartObject();

        if (textElement is Macro macro) {
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Macro);
            writer.WriteNumber("Pointer", macro.Pointer);
            writer.WriteNumber("VariableDiscriminator", (int)macro.Type);
        } else if (textElement is Text text) {
            writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Text);
            writer.WriteString("ConstText", text.ConstText);
        } else {
            throw new JsonException();
        }

        // writer.WriteString("Name", person.Name);

        writer.WriteEndObject();
    }
}
