using ExerciseEngine;
using ExerciseEngine.MacroExercise;

// >>> 1.: class ConcreteVariant <<<

sealed class Variant_000_005_S01P04_O_kolik_se_lisi : Variant {
	public readonly int A;
	public readonly int B;

	public Variant_000_005_S01P04_O_kolik_se_lisi(int A, int B) {
		this.A = A;
		this.B = B;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		return IsLegit_0();
	}

	bool IsLegit_0() {
		// Musí vyjít jako kladné nenulové číslo
		return (A * A) > B; 
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex != 0)
			throw new ArgumentException("Index needs to be positive and at most 0, index entered: " + questionIndex.ToString());

		return GetResult_0();
	}

	string GetResult_0() {
		int result = ((A * A) - B);
		return result.ToString();
	}

	public override string GetValueOfVariable(string macroPointer) {
		return macroPointer switch {
			"A" => A.ToString(),
			"B" => (B*B).ToString(), // attention!
			_ => throw new ArgumentException("Variable representation recieved invalid variable name: " + macroPointer + "\n")
		};
	}

	public override VariantRecord ToVariantRecord() {
		Dictionary<string, string> variables = new() {
			["A"] = A.ToString(),
			["B"] = (B*B).ToString() // attention!
		};

		Dictionary<string, string> results = new() {
			["result_0"] = GetResult(0)
		};
		return new(variables, results);
	}
}

// >>> 2.: class ConcreteExercise <<<

sealed class Exercise_000_005_S01P04_O_kolik_se_lisi : Exercise<Variant_000_005_S01P04_O_kolik_se_lisi> {
	// 19 * 19 = 361
	public Exercise_000_005_S01P04_O_kolik_se_lisi() : base(1, 361, "Exercise_000_005_S01P04_O_kolik_se_lisi", 5, Language.cs, ExerciseType.WordProblem) { }

	protected override MacroString BuildDescription() {
		List<MacroText> assignment = new() { };
		return new(assignment);
	}

	protected override List<Macro_Question> BuildQuestions() {
		return new() {
			Question_0()
		};
	}

	static Macro_Question Question_0() {
		// $"Vypočtěte, o kolik se liší druhá mocnina čísla {A}, a druhá odmocnina z čísla {B*B}."
		Text t1 = new("Vypočtěte, o kolik se liší druhá mocnina čísla ");
		Macro m1 = new("A");
		Text t2 = new(", a druhá odmocnina z čísla ");
		Macro m2 = new("B");
		Text t3 = new(".");
		List<MacroText> q = new() { t1, m1, t2, m2, t3 };
		MacroString Question = new(q);

		Macro resultMacro = new("result_0");

		Macro_Question question = new Macro_IntQuestion(Question, resultMacro) {
			SolutionSteps = null,
			ImagePaths = null
		};

		return question;
	}

	public override void FilterLegitVariants() {
		Console.WriteLine($"Initiating nested forloops, expecting to see {expected} variants.");

		for (int A = 2; A <= 20; A++) {
			for (int B = 2; B <= 20; B++) {
				Variant_000_005_S01P04_O_kolik_se_lisi variant = new(A, B);
				Consider(variant);
			}
		}
	}
}