//namespace ExerciseEngine.Factory;

//class PersonConverterWithTypeDiscriminator : JsonConverter<Variable> {
//    enum TypeDiscriminator {
//        InvariantVar = 1,
//        CulturalVar = 2
//    }

//    public override bool CanConvert(Type typeToConvert) => typeof(Variable).IsAssignableFrom(typeToConvert);

//    public override Variable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
//        if (reader.TokenType != JsonTokenType.StartObject) 
//            throw new JsonException();

//        reader.Read();
//        if (reader.TokenType != JsonTokenType.PropertyName) 
//            throw new JsonException();

//        string? propertyName = reader.GetString();
//        if (propertyName != "TypeDiscriminator") 
//            throw new JsonException();

//        reader.Read();
//        if (reader.TokenType != JsonTokenType.Number) 
//            throw new JsonException();

//        TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
//        Variable variable = typeDiscriminator switch {
//            TypeDiscriminator.InvariantVar => new InvariantVariable(),
//            TypeDiscriminator.CulturalVar => new CulturalVariable(),
//            _ => throw new JsonException()
//        };

//        while (reader.Read()) {
//            if (reader.TokenType == JsonTokenType.EndObject)
//                return variable;
            
//            if (reader.TokenType == JsonTokenType.PropertyName) {
//                propertyName = reader.GetString();
//                reader.Read();
//                switch (propertyName) {
//                    case "Value":
//                        string? value = reader.GetString();
//                        if(value == null)
//                            throw new JsonException();
//                        ((InvariantVariable)variable).Value = value!;
//                        break;
//                    case "Dict":
//                        string? officeNumber = reader.Get;
//                        ((CulturalVariable)variable).OfficeNumber = officeNumber;
//                        break;
//                }
//            }
//        }
         
//        throw new JsonException();
//    }

//    public override void Write(
//        Utf8JsonWriter writer, Variable person, JsonSerializerOptions options) {
//        writer.WriteStartObject();

//        if (person is Customer customer) {
//            writer.WriteNumber("TypeDiscriminator", (int)JsonPolyId.InvariantVar);
//            writer.WriteNumber("CreditLimit", customer.CreditLimit);
//        } else if (person is Employee employee) {
//            writer.WriteNumber("TypeDiscriminator", (int)JsonPolyId.CulturalVar);
//            writer.WriteString("OfficeNumber", employee.OfficeNumber);
//        }

//        // writer.WriteString("Name", person.Name);

//        writer.WriteEndObject();
//    }
//}
