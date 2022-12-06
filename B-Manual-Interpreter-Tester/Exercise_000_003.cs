using ExerciseEngine;
using ExerciseEngine.MacroExercise;

// >>> 1.: class ConcreteVariant <<<

sealed class Variant_000_003_S01P02_Prevod_jednotek_casu : Variant {
	public readonly int A;
	public readonly int B;
	public readonly int C;

	public Variant_000_003_S01P02_Prevod_jednotek_casu(int A, int B, int C) {
		this.A = A;
		this.B = B;
		this.C = C;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		return IsLegit_0();
	}

	bool IsLegit_0() {
		// počet zadaných minut musí být beze zbytku dělitelný velikostí minutového intervalu
		int celekMinut = (60 * A) + (6 * B);
		return celekMinut % C == 0;
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex != 0)
			throw new ArgumentException("Index needs to be positive and at most 0, index entered: " + questionIndex.ToString());

		return GetResult_0();
	}

	string GetResult_0() {
		int celekMinut = (60 * A) + (6 * B);
		int result = celekMinut / C;
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
			["B"] = B.ToString(),
			["C"] = C.ToString()
		};

		Dictionary<string, string> results = new() {
			["result_0"] = GetResult(0)
		};
		return new(variables, results);
	}
}

// >>> 2.: class ConcreteExercise <<<

sealed class Exercise_000_003_S01P02_Prevod_jednotek_casu : Exercise<Variant_000_003_S01P02_Prevod_jednotek_casu> {
	// expected space: 5 * 9 * 9 = 405
	public Exercise_000_003_S01P02_Prevod_jednotek_casu() : base(1, 405, "Exercise_000_003_S01P02_Prevod_jednotek_casu", 3, Language.cs, ExerciseType.WordProblem) { }

	protected override MacroString BuildDescription() {
		List<MacroText> assignment = new() { };
		return new(assignment);
	}

	protected override List<MacroQuestion> BuildQuestions() {
		return new() {
			Question_0()
		};
	}

	static MacroQuestion Question_0() {
		// $"Určete, na kolik {C}minutových intervalů lze rozdělit {A},{B} hodiny.";
		Text t1 = new("Určete, na kolik ");
		Macro m1 = new("C");
		Text t2 = new(" minutových intervalů lze rozdělit ");
		Macro m2 = new("A");
		Text t3 = new(",");
		Macro m3 = new("B");
		Text t4 = new(" hodiny.");
		List<MacroText> q = new() { t1, m1, t2, m2, t3, m3, t4 };
		MacroString Question = new(q);

		Macro resultMacro = new("result_0");

		MacroQuestion question = new MacroIntQuestion(Question, resultMacro) {
			SolutionSteps = null,
			ImagePaths = null
		};

		return question;
	}

	public override void FilterLegitVariants() {
		Console.WriteLine($"Initiating nested forloops, expecting to see {expected} variants.");

		for (int A = 1; A <= 5; A++) {
			for (int B = 1; B <= 9; B++) {
				for(int C = 11; C <= 19; C++ ) {
					Variant_000_003_S01P02_Prevod_jednotek_casu variant = new(A, B, C);
					Consider(variant);
				}
			}
		}
	}
}