namespace ExerciseEngine.Factory;

abstract class ManualCollectionBuilder { 
	abstract public ExerciseCollection BuildCollection();
}

#region  BuilderA

class ManualCollectionBuilderA : ManualCollectionBuilder{
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

	override public ExerciseCollection BuildCollection() {
		ulong id = 38;
		var variants = BuildTestVariations();
		var localizations = BuildTestLocalizations();
		ExerciseCollection ec = new(id, variants, localizations);
		return ec;
	}

	List<Variation> BuildTestVariations() {
		List<Variation> variations = new();
		foreach(var v in this.variations)
			variations.Add(BuildVariation(v[0], v[1], v[2]));
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

		public LocalizationDefinition(Language lang, string name, string a1, string a2, string a3, string a4) {
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
		List<Topic> topics = new() { Topic.Percentages, Topic.Arithmetic };
		Groups Groups = new(classes, topics, ExerciseType.WordProblem);

		return new(Lang, Name, Assignment, Questions, Results, SolutionSteps, Groups);
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

