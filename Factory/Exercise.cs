namespace ExerciseEngine.Factory;

record MetaData {
	public ulong UniqueId { get; }
	public string Name { get;} 
	public Language Lang { get; }
	public int VariationId { get; }
	public Groups Groups { get; }

	public MetaData(ulong UniqueId, string Name, Language Lang, int VariationId, Groups Groups) {
		this.UniqueId = UniqueId;
		this.Name = Name;
		this.Lang = Lang;
		this.VariationId = VariationId;
		this.Groups = Groups;
	}
}

abstract record Exercise {
	// metadata o uloze:
	public MetaData MetaData { get; }

	// vlastni uloha: 
	public string Assignment { get; }
	public List<string> SolutionSteps { get; }
	// List<Picture> pictures;

	protected Exercise(MetaData MetaData, string Assignment, List<string> SolutionSteps) {
		this.MetaData = MetaData;
		this.Assignment = Assignment;
		this.SolutionSteps = SolutionSteps;	
	}
}

record WordProblem : Exercise {
	// vlastni uloha:
	public List<string> Questions { get; }
	public List<string> Results { get; }

	public WordProblem(	
		MetaData MetaData, string Assignment, List<string> Questions, List<string> Results, List<string> SolutionSteps)
		: base(MetaData, Assignment, SolutionSteps) {
		this.Questions = Questions;
		this.Results = Results;
	}
}

record NumericalExercise : Exercise {
	// vlastni uloha:
	public string Result { get; }

	public NumericalExercise(
		MetaData MetaData, string Assignment, string Result, List<string> SolutionSteps)
		:base(MetaData, Assignment, SolutionSteps) {
		this.Result = Result;
	}
}

record GeometricExercise : Exercise {	
	public GeometricExercise(
		MetaData MetaData, string Assignment, List<string> SolutionSteps) 
		:base(MetaData, Assignment, SolutionSteps) {
		throw new NotImplementedException("No attention = no geometric exercises.");
	}	
}