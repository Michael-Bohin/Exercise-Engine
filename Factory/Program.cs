global using static System.Console;
global using static System.Math;
global using System.Text.Json;
global using System.Text.Json.Serialization;
namespace ExerciseEngine.Factory;
using System.Text.Json;

class Program {
	static void Main() {
		JsonSerializerOptions options = new() { WriteIndented = true };
		options.Converters.Add(new TextElementConverterWithTypeDiscriminator());
		options.Converters.Add(new ExerciseConverterWithTypeDiscriminator());
		
		WriteLine("Hi!"); 
		WriteLine("   ... constructing excercise collection...");

		ulong id = 38;
		var variants = BuildTestVariations();
		var localizations = BuildTestLocalizations();

		ExerciseCollection ec = new(id, variants, localizations);
		WriteLine("   ... serializing excericse collection and writting it to output file...");
		var json = JsonSerializer.Serialize(ec, options);
		using (StreamWriter sw = new("devTestOut.json")) {
			sw.WriteLine(json);
		}
		
		List<Exercise> czech = ec.GetEntireCollection(Language.cs);

		json = JsonSerializer.Serialize(czech, options);
		using (StreamWriter sw = new("czechCollectionTestOut.json")) {
			sw.WriteLine(json);
		}

		List<Exercise> czechDeserialized = JsonSerializer.Deserialize<List<Exercise>>(json, options)!;

		WriteLine(czechDeserialized[1].Assignment);

		WordProblem wp = (WordProblem)czechDeserialized[1];
		WriteLine("classes");
		foreach(var c in wp.Classes)
			WriteLine(c);

		WriteLine("topics");
		foreach (var t in wp.Topics)
			WriteLine(t);

		// unit test by comparing serialized output...
		// provide word problem and numerical exercise some elaborate tostring method and also compare that ... 

		// 1. make instance of excercise collection
		// 2. foreach language do:
		//		i.		GetEntireCollection(lang) to listA
		//		ii.		serialize List<exericse> to jsonA
		//		iii.	deserialize into List<exe> to listB
		//		iv.		serialize listB List<exercise> to jsonB
		//		v.		assert listA == listB
		//		vi.		assert jsonA == jsonB
		//
		// notes: 
		// 1. do this for some elaborate word problem and numerical exercise as well so that both structures are tested well enough 
		// 2. create instance with different lengths of List<T> properties..
		// 3. test serialization and deserialization of exercise collection in same approach..

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

	class LocalizationDefinition {
		public Language lang;
		public string name, a1, a2, a3, a4;
		
		public LocalizationDefinition(Language lang, string name, string a1, string a2, string a3, string a4 ) {
			this.lang = lang; this.name = name;
			this.a1 = a1; this.a2 = a2;
			this.a3 = a3; this.a4 = a4;
		}
	}

	static Dictionary<Language, ExerciseLocalization> BuildTestLocalizations() {
		Dictionary<Language, ExerciseLocalization> localizations = new();

		LocalizationDefinition cs = new(Language.cs, "Velice hezké a kreativní jméno české slovní úlohy.", "Obdélník má šířku ", " cm a obsah ", "dm ^ 2.", "Vypočtěte, o kolik cm se liší délka a šířka obdélníku.");
		LocalizationDefinition pl = new(Language.pl, "Bardzo ładna i pomysłowa nazwa dla polskiego problemu słownego.", "Prostokąt ma szerokość ", " cm i zawartość ", "dm ^ 2.", "Oblicz, o ile cm różnią się długości i szerokości prostokąta.");
		LocalizationDefinition en = new(Language.en, "Very nice and creative English word problem name.", "The rectangle has width ", " cm and content ", "dm^2.", "Calculate by how many cm the length and width of the rectangle differ.");
		LocalizationDefinition ua = new(Language.ua, "Очень красивое и креативное название для украинского слова роль.", "Прямоугольник имеет ширину ", " см и площадь ", " дм^2.", "Вычислите, на сколько см отличаются длина и ширина прямоугольника.");

		localizations.Add(Language.cs, BuildLocalization(cs));
		localizations.Add(Language.pl, BuildLocalization(pl));
		localizations.Add(Language.en, BuildLocalization(en));
		localizations.Add(Language.ua, BuildLocalization(ua));
		return localizations;
	}

	static ExerciseLocalization BuildLocalization(LocalizationDefinition locDef) {
		Language Lang = locDef.lang;
		string Name = locDef.name;

		Text el1 = new(locDef.a1);
		Macro el2 = new(0, VariableDiscriminator.Invariant);
		Text el3 = new(locDef.a2);
		Macro el4 = new(1, VariableDiscriminator.Invariant);
		Text el5 = new(locDef.a3);
		MacroText Assignment = new();
		Assignment.Elements.Add(el1);
		Assignment.Elements.Add(el2);
		Assignment.Elements.Add(el3);
		Assignment.Elements.Add(el4);
		Assignment.Elements.Add(el5);

		List<MacroText> Questions = new();
		MacroText question = new();
		question.Elements.Add(new Text(locDef.a4));
		Questions.Add(question);

		List<MacroText> Results = new();
		MacroText result = new();
		result.Elements.Add(new Macro(2, VariableDiscriminator.Invariant));
		Results.Add(result);

		List<MacroText> SolutionSteps = new();

		List<Classes> classes = new() { Classes.Ninth };
		List<Topic> topics = new() { Topic.Percentages, Topic.Arithmetic};
		Groups Groups = new(classes, topics, ExerciseType.WordProblem);

		return new(Lang, Name, Assignment, Questions, Results, SolutionSteps, Groups);
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
