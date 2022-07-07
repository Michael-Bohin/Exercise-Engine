using ExerciseEngine.source;

namespace ExerciseEngine.Factory;

abstract class ManualCollection2DBuilder { 
	abstract public ExerciseCollection2D BuildCollection();
}

#region  BuilderA

class ManualCollectionBuilderA : ManualCollection2DBuilder {
	readonly List<List<int>> variations = new() {
		new() { 5, 2, 35 },
		new() { 5, 3, 55 },
		new() { 5, 4, 75 },
		new() { 5, 5, 95 },
		new() { 5, 6, 115 },
		new() { 5, 7, 135 },
		new() { 5, 8, 155 },
		new() { 5, 9, 175 },
		new() { 5, 10, 195 },
		new() { 6, 3, 44 },
		new() { 6, 6, 94 },
		new() { 6, 9, 144 },
		new() { 6, 12, 194 },
		new() { 7, 7, 93 },
		new() { 7, 14, 193 }
	};

	override public ExerciseCollection2D BuildCollection() {
		int id = 38;
		var variants = BuildTestVariations();
		var localizations = BuildTestLocalizations();
		ExerciseCollection2D ec = new(id, variants, localizations);
		return ec;
	}

	List<Variant> BuildTestVariations() {
		List<Variant> variations = new();
		foreach(var v in this.variations)
			variations.Add(BuildVariation(v[0], v[1], v[2]));

		Dictionary<Language, string> culturalVariable = new();
		culturalVariable[Language.cs] = "ceskaPromenna";
		culturalVariable[Language.pl] = "polskaPremenna";
		culturalVariable[Language.en] = "englishVariable";

		Dictionary<Language, string> culturalVariableB = new();
		culturalVariableB[Language.en] = "englishVariable apple";
		culturalVariableB[Language.ua] = "английское переменное яблоко ";

		variations[0].cultural.Add(culturalVariable);
		variations[0].cultural.Add(culturalVariableB);
		variations[1].cultural.Add(culturalVariableB);
		variations[1].cultural.Add(culturalVariable);
		variations[1].cultural.Add(culturalVariableB);
		variations[2].cultural.Add(culturalVariableB);
		return variations;
	}

	static Variant BuildVariation(int A, int B, int Result) {
		Variant variation = new();
		variation.invariant.Add(A.ToString());
		variation.invariant.Add(B.ToString());
		variation.invariant.Add(Result.ToString());
		return variation;
	}

	class LocalizationDefinition {
		public (int id, Language lang) uniqueId;
		public string name, a1, a2, a3, a4;

		public LocalizationDefinition(int id, Language lang, string name, string a1, string a2, string a3, string a4) {
			uniqueId = (id, lang);
			this.name = name;
			this.a1 = a1; this.a2 = a2;
			this.a3 = a3; this.a4 = a4;
		}
	}

	static Dictionary<Language, ExerciseLocalization> BuildTestLocalizations() {
		Dictionary<Language, ExerciseLocalization> localizations = new();
		int id = 38;

		LocalizationDefinition cs = new(id, Language.cs, "Velice hezké a kreativní jméno české slovní úlohy.", "Obdélník má šířku ", " cm a obsah ", "dm ^ 2.", "Vypočtěte, o kolik cm se liší délka a šířka obdélníku.");
		LocalizationDefinition pl = new(id, Language.pl, "Bardzo ładna i pomysłowa nazwa dla polskiego problemu słownego.", "Prostokąt ma szerokość ", " cm i zawartość ", "dm ^ 2.", "Oblicz, o ile cm różnią się długości i szerokości prostokąta.");
		LocalizationDefinition en = new(id, Language.en, "Very nice and creative English word problem name.", "The rectangle has width ", " cm and content ", "dm^2.", "Calculate by how many cm the length and width of the rectangle differ.");
		LocalizationDefinition ua = new(id, Language.ua, "Очень красивое и креативное название для украинского слова роль.", "Прямоугольник имеет ширину ", " см и площадь ", " дм^2.", "Вычислите, на сколько см отличаются длина и ширина прямоугольника.");

		localizations.Add(Language.cs, BuildLocalization(cs));
		localizations.Add(Language.pl, BuildLocalization(pl));
		localizations.Add(Language.en, BuildLocalization(en));
		localizations.Add(Language.ua, BuildLocalization(ua));

		MacroText mt = new();
		mt.elements.Add(new Text("Je to sport. Ve předu útočíš. Vzádu bráníš."));
		mt.elements.Add(new Text("Na středu hřiště si přihráváš."));
		mt.elements.Add(new Text("Je to sport."));

		MacroText mtPl = new();
		mtPl.elements.Add(new Text("To jest sport. Atakujesz z przodu.Z tyłu bronisz.Podajesz piłkę na środek boiska.To jest sport."));
		
		localizations[Language.cs].solutionSteps.Add(mt);
		localizations[Language.pl].solutionSteps.Add(mtPl);
		return localizations;
	}
	
	static ExerciseLocalization BuildLocalization(LocalizationDefinition locDef) {
		Text el1 = new(locDef.a1);
		Macro el2 = new(0, VariableDiscriminator.Invariant);
		Text el3 = new(locDef.a2);
		Macro el4 = new(1, VariableDiscriminator.Invariant);
		Text el5 = new(locDef.a3);
		MacroText assignment = new();
		assignment.elements.Add(el1);
		assignment.elements.Add(el2);
		assignment.elements.Add(el3);
		assignment.elements.Add(el4);
		assignment.elements.Add(el5);

		List<MacroText> questions = new();
		MacroText question = new();
		question.elements.Add(new Text(locDef.a4));
		questions.Add(question);

		List<MacroText> results = new();
		MacroText result = new();
		result.elements.Add(new Macro(2, VariableDiscriminator.Invariant));
		results.Add(result);

		List<MacroText> solutionSteps = new();

		List<Classes> classes = new() { Classes.Ninth };
		List<Topic> topics = new() { Topic.Percentages, Topic.Arithmetic };

		LocalizationUniqueId id = new(locDef.uniqueId.id, locDef.uniqueId.lang);
		LocalizationMetaData metaData = new(id, locDef.name, topics, classes, ExerciseType.WordProblem);

		return new(metaData, assignment, questions, results, solutionSteps);
	}
}

#endregion


#region BuilderB

#endregion

#region BuilderC

#endregion

#region BuilderD

#endregion

#region BuilderE

#endregion

