using ExerciseEngine;
using System.Text.Json.Serialization; // incase json min size is needed

// >>> 1.: Intended usage <<<

Exercise_381200_Balonky ex = new();
ex.FilterLegitVariants();

string stats = ex.ReportStatistics();
string json = ex.SerializeSelf(true);

using StreamWriter sw1 = new("stats_Exercise_381200_Balonky.txt");
sw1.Write(stats);

using StreamWriter sw2 = new("json_Exercise_381200_Balonky.json");
sw2.Write(json);

// >>> 2.: class ConcreteVariant <<<

sealed class Variant_381200_Balonky : Variant {
	public readonly int cervene;
	public readonly int zelene;
	public readonly int modre;
	public Variant_381200_Balonky(int cervene, int zelene, int modre) {
		this.cervene = cervene;
		this.zelene = zelene;
		this.modre = modre;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		if (Constraint_0())
			return false;

		constraintId++;
		if (Constraint_1())
			return false;

		return true;
	}

	// vysledek otazky B ma nanejvys 2 desetinna mista
	bool Constraint_0() {
		string result = GetResult(1);
		int index = result.IndexOf('.');
		int length = result.Length;
		length -= index;
		length--;
		return length > 2;
	}

	// maximum je prave jedno
	bool Constraint_1() {
		int max = Math.Max(cervene, zelene);
		max = Math.Max(max, modre);
		int counter = 0;
		if (max == cervene) counter++;
		if (max == zelene) counter++;
		if (max == modre) counter++;
		return counter > 1;
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex < 0 || questionIndex > 2)
			throw new ArgumentException("Index needs to be positive and at most 2, index entered: " + questionIndex.ToString());

		if (questionIndex == 0)
			return GetResult_0();

		if (questionIndex == 1)
			return GetResult_1();

		return GetResult_2();
	}

	string GetResult_0() => (cervene + zelene + modre).ToString();

	string GetResult_1() {
		double Px = modre / (double)(cervene + zelene + modre);
		return Px.ToString();
	}

	string GetResult_2() {
		int max = Math.Max(cervene, zelene);
		max = Math.Max(max, modre);
		if (max == modre)
			return "b";
		
		if (max == zelene)
			return "c";
		
		return "a";
	}

	public override string VariableRepresentation(string variableName) {
		return variableName switch {
			"cervene" => cervene.ToString(),
			"zelene" => zelene.ToString(),
			"modre" => modre.ToString(),
			_ => throw new ArgumentException("Variable representation recieved invalid variable name: " + variableName + "\n")
		};
	}
}

// >>> 3.: class ConcreteExercise <<<

sealed class Exercise_381200_Balonky : Exercise<Variant_381200_Balonky> {
	public Exercise_381200_Balonky() : base(false, 2, 24389, "Exercise_381200_Balonky", Language.cs) { }

	protected override List<MacroText> BuildAssignment() {
		Text el0 = new("Jakub nosí batoh a v něm má balónky s různými barvami. Dneska ráno si do batohu dal ");
		Macro el1 = new("cervene");
		Text el2 = new(" červených, ");
		Macro el3 = new("zelene");
		Text el4 = new(" zelených a ");
		Macro el5 = new("modre");
		Text el6 = new(" modrých balonků.");
		List<MacroText> assignment = new() { el0, el1, el2, el3, el4, el5, el6 };
		return assignment;
	}

	protected override List<MacroQuestion> BuildQuestions() {
		return new() {
			Question_0(),
			Question_1(),
			Question_2()
		};
	}

	static MacroQuestion Question_0() {
		Text el0 = new("Kolik balónků si dal Kuba do batohu dohromady?");
		MacroQuestion question = new() {
			resultType = ResultType.Int,
			question = new() { el0 }
		};
		return question;
	}

	static MacroQuestion Question_1() {
		Text el0 = new("Jaká je pravděpodobnost, že si Kuba při náhodném výběru vytáhne z batohu modrý balónek?");
		MacroQuestion question = new() {
			resultType = ResultType.Double,
			question = new() { el0 }
		};
		return question;
	}

	static MacroQuestion Question_2() {
		Text el0 = new("Kterých balonků má Kuba v batohu nejvíce? a) červených, b) modrých nebo c) zelených?");
		MacroQuestion question = new() {
			resultType = ResultType.Select,
			question = new() { el0 }
		};
		return question;
	}

	public override void FilterLegitVariants() {
		Console.WriteLine($"Initiating nested forloops, expecting to see {expected} variants.");

		for (int cervene = 2; cervene <= 30; cervene++) {
			for (int zelene = 2; zelene <= 30; zelene++) {
				for (int modre = 2; modre <= 30; modre++) {
					Variant_381200_Balonky variant = new(cervene, zelene, modre);
					Consider(variant);
				}
			}
		}
	}
}