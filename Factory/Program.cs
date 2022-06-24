global using static System.Console;
global using static System.Math;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

namespace ExerciseEngine.Factory;
using System.Text.Json;

class Program {
	static void Main() {
		WriteLine("Hi!"); 
	
		//UnitTestSerialization ut = new();
		//ut.Test_Exercise_Collection();
		//ut.Test_List_Exercise();
		// serializace pomocí .NET 7 preview 5 featury: 

		JsonSerializerOptions options = new() {
			WriteIndented = false,
			IncludeFields = true
		};

		// options.Converters.Add(new LocalizationMetaDataConverter());
		// options.Converters.Add();
		// options.Converters.Add();
		// options.Converters.Add();
		// options.Converters.Add();
		// options.Converters.Add(new VariantConverter());
		// options.Converters.Add(new TextElementConverter());
		// options.Converters.Add(new MacroTextConverter());
		// options.Converters.Add(new LocalizationMetaDataConverter());
		// options.Converters.Add(new ExerciseLocalizationConverter());
		// options.Converters.Add(new ExerciseCollectionConverter());

		ManualCollectionBuilderA builder = new();
		ExerciseCollection ec = builder.BuildCollection();


		string jsonA = JsonSerializer.Serialize(ec, options);

		ExerciseCollection? deserializedVersion = JsonSerializer.Deserialize<ExerciseCollection>(jsonA, options);

		if (deserializedVersion == null)
			throw new JsonException();

		string jsonB = JsonSerializer.Serialize(deserializedVersion, options);


		using (StreamWriter sw = new("A-NET7.json")) {
			sw.WriteLine(jsonA);
		}

		using (StreamWriter sw = new("B-NET7s.json")) {
			sw.WriteLine(jsonB);
		}


	}
}