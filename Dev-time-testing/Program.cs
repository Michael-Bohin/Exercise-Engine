using ExerciseEngine;
using static System.Console;
using System.Text.Json;

// namespace DevTimeAccess;

class Program {
	static void Main() {
		WriteLine("Hi!");

		JsonSerializerOptions options = new() {
			WriteIndented = false,
			IncludeFields = true
		};

		ManualCollectionBuilderA builder = new();
		ExerciseCollection2D ec = builder.BuildCollection();

		string jsonA = JsonSerializer.Serialize(ec, options);

		ExerciseCollection2D? deserializedVersion = JsonSerializer.Deserialize<ExerciseCollection2D>(jsonA, options);

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