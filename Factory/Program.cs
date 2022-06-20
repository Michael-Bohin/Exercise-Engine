global using static System.Console;
global using static System.Math;
global using System.Text.Json;
global using System.Text.Json.Serialization;
namespace ExerciseEngine.Factory;
using System.Text.Json;

class Program {
	static void Main() { 
		WriteLine("Hi!"); 
		WriteLine("   ... constructing excercise collection...");

		ulong id = 38;
		var variants = BuildTestVariations();
		var localizations = BuildTestLocalizations();

		ExerciseCollection ec = new(id, variants, localizations);
		WriteLine("   ... serializing excericse collection and writting it to output file...");
		JsonSerializerOptions options = new() { WriteIndented = true};
		var json = JsonSerializer.Serialize(ec, options);
		using StreamWriter sw = new ("devTestOut.json");
		sw.WriteLine(json);
	}

	static List<Variation> BuildTestVariations() {
		List<Variation> variations = new();
		variations.Add(BuildVariation(5, 2, 35));
		variations.Add(BuildVariation(5, 3, 55));
		variations.Add(BuildVariation(5, 4, 75));
		variations.Add(BuildVariation(5, 5, 95));
		variations.Add(BuildVariation(5, 6, 115));
		variations.Add(BuildVariation(5, 7, 135));
		variations.Add(BuildVariation(5, 8, 155));
		variations.Add(BuildVariation(5, 9, 175));
		variations.Add(BuildVariation(5, 10, 195));
		variations.Add(BuildVariation(6, 3, 44));
		variations.Add(BuildVariation(6, 6, 94));
		variations.Add(BuildVariation(6, 9, 144));
		variations.Add(BuildVariation(6, 12, 194));
		variations.Add(BuildVariation(7, 7, 93));
		variations.Add(BuildVariation(7, 14, 193));
		return variations;
	}

	static Variation BuildVariation(int A, int B, int Result) {
		Variation variation = new();
		variation.Inv.Add(A.ToString());
		variation.Inv.Add(B.ToString());
		variation.Inv.Add(Result.ToString());
		return variation;
	}

	static Dictionary<Language, ExerciseLocalization> BuildTestLocalizations() {
		Dictionary<Language, ExerciseLocalization> localizations = new();
		return localizations;
	}
}






//Language cs = Language.cs;
//Language ua = Language.ua;
//Language en = Language.en;
//Language pl = Language.pl;

//Exercise randomCsExercise = ec.GetRandomExercise(cs);
//Exercise randomUaExercise = ec.GetRandomExercise(ua);
//Exercise randomEnExercise = ec.GetRandomExercise(en);
//Exercise randomPlExercise = ec.GetRandomExercise(pl);
//List<Exercise> allEnExercises = ec.GetEntireCollection(en);
//List<Exercise> allCsExercises = ec.GetEntireCollection(cs);
//List<Exercise> allUaExercises = ec.GetEntireCollection(ua);
//List<Exercise> allPlExercises = ec.GetEntireCollection(pl);

// than:
// 1. all in pretty print of json!
// 2. serialize the exercise collection to some file 
// 3. than all other variables
// 4. observe and repair bugs
// 5. write unit tests
// 6. observe how you can improve the code
// 7. measure how long does it take to build one random exercises given concrete languge. (that will be typical usecase for server)
// 8. how long does it take when exrcise collection instance is in memory? 
// 9. how long does it take when we need to actually deserilize the collection after IO operation? 
// --- we should be far under 1ms so that we know we are free to do not overoptimized solutions ---
