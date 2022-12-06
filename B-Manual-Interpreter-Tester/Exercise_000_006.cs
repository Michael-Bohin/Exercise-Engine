using ExerciseEngine;
using ExerciseEngine.MacroExercise;
using System.Globalization;

// >>> 1.: class ConcreteVariant <<<

sealed class Variant_000_006_S01P05_Pocitani_se_zlomky : Variant {
	public readonly int Citatel;
	public readonly int Jmenovatel;
	public readonly double C;

	public Variant_000_006_S01P05_Pocitani_se_zlomky(int Citatel, int Jmenovatel, double C) {
		this.Citatel = Citatel;
		this.Jmenovatel = Jmenovatel;
		this.C = C;
	}

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		return IsLegit_0();
	}

	bool IsLegit_0() {
		// Dělení ve zlomku musí vyjít jako celé číslo beze zbytku
		return (Citatel * 10) % Jmenovatel == 0; 
	}

	public override string GetResult(int questionIndex) {
		if (questionIndex != 0)
			throw new ArgumentException("Index needs to be positive and at most 0, index entered: " + questionIndex.ToString());

		return GetResult_0();
	}

	string GetResult_0() {
		int levyZlomek = (Citatel * 10) / Jmenovatel;
		int praveNasobeni = (int)(1.0 / C);
		int result = levyZlomek * praveNasobeni;
		return result.ToString();
	}

	public override string GetValueOfVariable(string macroPointer) {
		return macroPointer switch {
			"Citatel" => Citatel.ToString(),
			"Jmenovatel" => Jmenovatel.ToString(),
			"C" => C.ToString(CultureInfo.CreateSpecificCulture("eu-CS")),
			_ => throw new ArgumentException("Variable representation recieved invalid variable name: " + macroPointer + "\n")
		};
	}

	public override VariantRecord ToVariantRecord() {
		Dictionary<string, string> variables = new() {
			["Citatel"] = Citatel.ToString(),
			["Jmenovatel"] = Jmenovatel.ToString(),
			["C"] = C.ToString(CultureInfo.CreateSpecificCulture("eu-CS"))
		};

		Dictionary<string, string> results = new() {
			["result_0"] = GetResult(0)
		};
		return new(variables, results);
	}
}

// >>> 2.: class ConcreteExercise <<<

sealed class Exercise_000_006_S01P05_Pocitani_se_zlomky : Exercise<Variant_000_006_S01P05_Pocitani_se_zlomky> {
	// 41 * 41 * 4 = 6 724
	public Exercise_000_006_S01P05_Pocitani_se_zlomky() : base(1, 6_724, "Exercise_000_006_S01P05_Pocitani_se_zlomky", 6, Language.cs, ExerciseType.Numerical) { }

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
		// $"(0,{Citatel} / 0,0{Jmenovatel}) : {C}"
		Text t1 = new("(0,");
		Macro m1 = new("Citatel");
		Text t2 = new("/ 0,0");
		Macro m2 = new("Jmenovatel");
		Text t3 = new(") : ");
		Macro m3 = new("C");
		List<MacroText> q = new() { t1, m1, t2, m2, t3, m3 };
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

		List<double> set_C = new() { 0.1, 0.2, 0.25, 0.5 };

		for (int Citatel = 10; Citatel <= 50; Citatel++) {
			for (int Jmenovatel = 10; Jmenovatel <= 50; Jmenovatel++) {
				foreach(double C in  set_C) { 
					Variant_000_006_S01P05_Pocitani_se_zlomky variant = new(Citatel, Jmenovatel, C);
					Consider(variant);
				}
			}
		}
	}
}