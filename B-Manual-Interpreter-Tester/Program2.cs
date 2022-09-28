using ExerciseEngine;
using static System.Console;

E_2_Balonky exercise_2 = new();
F_2_Balonky factory_2 = new();
factory_2.FilterLegitVariants();

List<string> result = new();
foreach (var variant in factory_2.legit) {
	Representation r = exercise_2.GetRepresentation(variant, Language.cs);
	result.Add(r.assignment + "   " + r.questions[0].result + "  " + r.questions[1].result + "  " + r.questions[2].result);
}

// save the result:
using (StreamWriter sw = new("logB.txt")) {
	foreach (var line in result)
		sw.WriteLine(line);
}

using (StreamWriter sw = new("statsB.txt")) 
	sw.WriteLine(factory_2.ReportStatistics());

// expected to be written by interpreter based on definition:
sealed class V_2_Balonky : Variant {
	public readonly int cervene;
	public readonly int zelene;
	public readonly int modre;
	public V_2_Balonky(int cervene, int zelene, int modre) {
		this.cervene = cervene;
		this.zelene = zelene;
		this.modre = modre;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		if (Constraint_01())
			return false;

		constraintId++;
		if (Constraint_02())
			return false;

		return true;
	}

	bool Constraint_01() {
		string result = GetResult(1);
		int index = result.IndexOf('.');
		int length = result.Length;
		length -= index;
		length--;
		if (length > 2)
			return true;
		return false;
	}

	bool Constraint_02() {
		int max = Math.Max(cervene, zelene);
		max = Math.Max(max, modre);
		int counter = 0;
		if (max == cervene) counter++;
		if (max == zelene) counter++;
		if (max == modre) counter++;
		if (counter > 1)
			return true;
		return false;
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex < 0 || questionIndex > 2)
			throw new ArgumentException("Index needs to be positive and at most 2, index entered: " + questionIndex.ToString());

		if (questionIndex == 0)
			return GetResult_1();

		if (questionIndex == 1)
			return GetResult_2();

		return GetResult_3();
	}

	string GetResult_1() => (cervene + zelene + modre).ToString();

	string GetResult_2() {
		double Px = modre / (double)(cervene + zelene + modre);
		return Px.ToString();
	}

	string GetResult_3() {
		int max = Math.Max(cervene, zelene);
		max = Math.Max(max, modre);
		if (max == modre)
			return "b";

		if (max == zelene)
			return "c";

		return "a";
	}

	public override string VariableRepresentation(string variableName) {
		switch (variableName) {
			case "cervene":
				return cervene.ToString();
			case "zelene":
				return zelene.ToString();
			case "modre":
				return modre.ToString();

			default:
				throw new ArgumentException("Variable representation recieved invalid variable name: " + variableName + "\n");
		}
	}
}

sealed class F_2_Balonky : Factory<V_2_Balonky> {
	public F_2_Balonky() : base(2, 24_389) { }

	public override void FilterLegitVariants() {
		WriteLine($"Initiating for loop, expecting to see {expected} variants.");
		for (int cervene = 2; cervene <= 30; cervene++) {
			for (int zelene = 2; zelene <= 30; zelene++) {
				for (int modre = 2; modre <= 30; modre++) {
					V_2_Balonky variant = new(cervene, zelene, modre);
					Consider(variant);
				}
			}
		}
	}
}

sealed class E_2_Balonky : Exercise<V_2_Balonky> {
	public E_2_Balonky() : base(false) {
		Text el1 = new("Jakub nosí batoh a v něm má balónky s různými barvami. Dneska ráno si do batohu dal ");
		Macro el2 = new("cervene");
		Text el3 = new(" červených, ");
		Macro el4 = new("zelene");
		Text el5 = new(" zelených a ");
		Macro el6 = new("modre");
		Text el7 = new(" modrých balonků.");
		List<MacroText> assignment = new() { el1, el2, el3, el4, el5, el6, el7 };

		MacroRepresentation mr = new();
		mr.assignment = assignment;

		Text q1 = new("Kolik balónků si dal Kuba do batohu dohromady?");
		Text q2 = new("Jaká je pravděpodobnost, že si Kuba při náhodném výběru vytáhne z batohu modrý balónek?");
		Text q3 = new("Kterých balonků má Kuba v batohu nejvíce? a) červených, b) modrých nebo c) zelených?");

		MacroQuestion mQ1 = new() {
			resultType = ResultType.Int,
			question = new() { q1 }
		};

		MacroQuestion mQ2 = new() {
			resultType = ResultType.Double,
			question = new() { q2 }
		};

		MacroQuestion mQ3 = new() {
			resultType = ResultType.Select,
			question = new() { q3 }
		};

		mr.questions.Add(mQ1);
		mr.questions.Add(mQ2);
		mr.questions.Add(mQ3);

		babylon[Language.cs] = mr;
	}
}