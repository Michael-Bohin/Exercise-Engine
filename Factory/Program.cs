global using static System.Console;
global using static System.Math;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

namespace ExerciseEngine.Factory;
using System.Text.Json;
using ExerciseEngine.source;

class Program {
	static void Main() {
		WriteLine("Hi!"); 
	
		JsonSerializerOptions options = new() {
			WriteIndented = false,
			IncludeFields = true
		};

		ManualCollectionBuilderA builder = new();
		ExerciseCollection ec = builder.BuildCollection();

		string jsonA = JsonSerializer.Serialize(ec, options);

		ExerciseCollection? deserializedVersion = JsonSerializer.Deserialize<ExerciseCollection>(jsonA, options);

		if (deserializedVersion == null)
			throw new JsonException();

		string jsonB = JsonSerializer.Serialize(deserializedVersion, options);

		using (StreamWriter sw = new("A-NET7.json")) 
			sw.WriteLine(jsonA);

		using (StreamWriter sw = new("B-NET7.json")) 
			sw.WriteLine(jsonB);
		WriteLine("Bye!");
	}
}