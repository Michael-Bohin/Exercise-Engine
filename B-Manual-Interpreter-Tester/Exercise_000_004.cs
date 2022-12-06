using ExerciseEngine;
using ExerciseEngine.MacroExercise;

// >>> 1.: class ConcreteVariant <<<

sealed class Variant_000_004_S01P03_Prevod_jednotek_uhlu : Variant {
	public readonly int A;
	public readonly int B;

	public Variant_000_004_S01P03_Prevod_jednotek_uhlu(int A, int B) {
		this.A = A;
		this.B = B;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		return IsLegit_0();
	}

	bool IsLegit_0() {
		// 60 minut děleno B musí vyjít beze zbytku. (zakázání pravě jenom B==25)
		return 60 % B == 0; 
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex != 0)
			throw new ArgumentException("Index needs to be positive and at most 0, index entered: " + questionIndex.ToString());

		return GetResult_0();
	}

	string GetResult_0() { 
		int result = A * (60 / B);
		return result.ToString();
	} 
	
	public override string GetValueOfVariable(string macroPointer) {
		return macroPointer switch {
			"A" => A.ToString(),
			"B" => B.ToString(),
			_ => throw new ArgumentException("Variable representation recieved invalid variable name: " + macroPointer + "\n")
		};
	}

	public override VariantRecord ToVariantRecord() {
		Dictionary<string, string> variables = new() {
			["A"] = A.ToString(),
			["B"] = B.ToString()
		};

		Dictionary<string, string> results = new() {
			["result_0"] = GetResult(0)
		};
		return new(variables, results);
	}
}

// >>> 2.: class ConcreteExercise <<<

sealed class Exercise_000_004_S01P03_Prevod_jednotek_uhlu : Exercise<Variant_000_004_S01P03_Prevod_jednotek_uhlu> {
	// 18 * 6 = 102
	public Exercise_000_004_S01P03_Prevod_jednotek_uhlu() : base(1, 108, "Exercise_000_004_S01P03_Prevod_jednotek_uhlu", 4, Language.cs, ExerciseType.WordProblem) { }

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
		// $"Vypočtěte, kolikrát je úhel o velikosti {A}° větší než úhel o vleikosti 0°{B}’."
		Text t1 = new("Vypočtěte, kolikrát je úhel o velikosti ");
		Macro m1 = new("A");
		Text t2 = new("° větší než úhel o vleikosti 0°");
		Macro m2 = new("B");
		Text t3 = new("’.");
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

		for (int A = 3; A <= 20; A++) {
			for (int B = 5; B <= 30; B += 5) {
				Variant_000_004_S01P03_Prevod_jednotek_uhlu variant = new(A, B);
				Consider(variant);
			}
		}
	}
}