using ExerciseEngine;
using ExerciseEngine.MacroExercise;
using System.Globalization;

// >>> 1.: class ConcreteVariant <<<

sealed class Variant_000_002_S01P01_Druhy_priklad : Variant {
	public readonly int A;
	public readonly int B;

	public Variant_000_002_S01P01_Druhy_priklad(int A, int B) {
		this.A = A;
		this.B = B;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0; // dummy
		return true;
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex != 0)
			throw new ArgumentException("Index needs to be positive and at most 0, index entered: " + questionIndex.ToString());

		return GetResult_0();
	}

	string GetResult_0() {
		double result = (double)(A * 10) + ((double)B / 1000.0);
		return result.ToString("F2");
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

sealed class Exercise_000_002_S01P01_Prevod_jednotek_plochy_objemu_druhy : Exercise<Variant_000_002_S01P01_Druhy_priklad> {
	public Exercise_000_002_S01P01_Prevod_jednotek_plochy_objemu_druhy() : base(0, 801, "Exercise_000_002_S01P01_Druhy_priklad", 2, Language.cs, ExerciseType.Numerical) { }

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
		Text t1 = new("x dm^3 – 0,0");
		Macro m1 = new("A");
		Text t2 = new(" m^3 = ");
		Macro m2 = new("B");
		Text t3 = new(" cm^3");
		List<MacroText> q = new() { t1, m1, t2, m2, t3 };
		MacroString Question = new(q);

		Macro resultMacro = new("result_0");

		MacroQuestion question = new MacroDecimalQuestion(Question, resultMacro, 2) {
			SolutionSteps = null,
			ImagePaths = null
		};

		return question;
	}

	public override void FilterLegitVariants() {
		Console.WriteLine($"Initiating nested forloops, expecting to see {expected} variants.");

		for (int A = 1; A <= 9; A++) {
			for (int B = 110; B <= 990; B += 10) {
				Variant_000_002_S01P01_Druhy_priklad variant = new(A, B);
				Consider(variant);
			}
		}
	}
}