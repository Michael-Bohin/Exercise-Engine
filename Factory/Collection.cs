namespace ExerciseEngine.Factory;

abstract class ExerciseCollection {
	public ulong UniqueId { get; }
	public Dictionary<Language, string> Name { get; } = new();
	public List<Variation> Variants { get; } = new();
	public Dictionary<Language, MacroText> Assignment { get; } = new();
	public Dictionary<Language, List<MacroText>> SolutionSteps { get; } = new();
	// ??? pictures; dev in future, today type is unknown
	public Dictionary<Language, Groups> Groups { get; } = new();
	
	// private fields:
	static Random rand = new();

	public Exercise GetRandomExercise(Language lang) { 
		int pick = rand.Next(0,Variants.Count);
		Variation v = Variants[pick];

		MetaData metaData = new(UniqueId, Name[lang], lang, pick, Groups[lang]);

		string ass = Assignment[lang].ConstructText(lang, v);
		
		List<string> solSteps = new();
		foreach(var step in SolutionSteps[lang]) 
			solSteps.Add(step.ConstructText(lang, v));

		return MakeExercise(metaData, v, ass, solSteps);
	}

	abstract protected Exercise MakeExercise(MetaData metaData, Variation v, string ass, List<string> solSteps);
}

class WordProblemCollection : ExerciseCollection {
	public Dictionary<Language, List<MacroText>> Questions { get; } = new();

	override protected Exercise MakeExercise(MetaData metaData, Variation v, string ass, List<string> solSteps) {
		throw new NotImplementedException();
	}
}

class NumericalExerciseCollection : ExerciseCollection {
	override protected Exercise MakeExercise(MetaData metaData, Variation v, string ass, List<string> solSteps) {
		throw new NotImplementedException();
	}
}

class GeometricExerciseCollection : ExerciseCollection {
	protected GeometricExerciseCollection() {
		throw new NotImplementedException("System.Drawing will be definitelly utilized, but first attention of kids must be monetized using word problems and numerical exercises.");
	}

	override protected Exercise MakeExercise(MetaData metaData, Variation v, string ass, List<string> solSteps) {
		throw new NotImplementedException();
	}
}