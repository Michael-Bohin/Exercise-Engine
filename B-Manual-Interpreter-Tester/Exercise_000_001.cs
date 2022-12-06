using ExerciseEngine;
using ExerciseEngine.MacroExercise;

// >>> 1.: class ConcreteVariant <<<

sealed class Variant_000_001_S01P01_Prvni_priklad : Variant {
	public readonly int A;
	public readonly int B;

	public Variant_000_001_S01P01_Prvni_priklad(int A, int B) {
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
		return ((A*1000)-B).ToString(); 
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

sealed class Exercise_000_001_S01P01_Prevod_jednotek_plochy_objemu_prvni : Exercise<Variant_000_001_S01P01_Prvni_priklad> {
	public Exercise_000_001_S01P01_Prevod_jednotek_plochy_objemu_prvni() : base(0, 801, "Exercise_000_001_S01P01_Prvni_priklad", 1, Language.cs, ExerciseType.Numerical ) { }

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
		Text t1 = new("0,");
		Macro m1 = new("A");
		Text t2 = new(" m^2 – ");
		Macro m2 = new("B");
		Text t3 = new(" cm^2 = x cm^2");
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

		for (int A = 1; A <= 9; A++) {
			for (int B = 11; B <= 99; B++) {
				Variant_000_001_S01P01_Prvni_priklad variant = new(A, B);
				Consider(variant);
			}
		}
	}
}