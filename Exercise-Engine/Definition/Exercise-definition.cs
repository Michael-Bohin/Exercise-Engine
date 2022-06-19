namespace ExerciseEngine;

enum JSONId_ExerciseDefinition { WordProblem, Numerical, Geometric }

abstract record ExerciseDefinition {
	public ulong Id { get; }
	public Babylon Name { get; set; } = new();
	public List<Variable> Variables { get; set; } = new();
	public List<Constraint> Constraints { get; set; } = new();
	// ----------------------------
	public List<Babylon> SolutionSteps { get; set; } = new(); // this is actually 3.5D : 1st D List<>, 2nd D Dictionary with keys Lang, 3rd D List<TextElement>, 4th D partialy Macros pointing to some variable
	public Groups Groups { get; set; } = new();

	public ExerciseDefinition(ulong Id) { 
		this.Id = Id; // simplify for now, later it will be interpreters job to know which id is up next	
	}
}

abstract record WordProblemDefinition : ExerciseDefinition {
	public Babylon Assignment { get; set; } = new();
	public List<Babylon> Questions { get; set; } = new();
	public List<string> Answers { get; set; } = new(); // once finnished consider types of answers..  how about having answers of concrete type? 
	public ImageDataTypeToBeDecided Image { get; set; } = new(); // btw with dragon rocket this will be list of images.. (from vid, 1 per second of vid)

	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.WordProblem;
	public WordProblemDefinition(ulong Id):base(Id) { }
}

abstract record NumericalExerciseDefinition : ExerciseDefinition {
	public Babylon Question { get; set; } = new(); // expression/equation to solve
	public string Answer { get; set; } = "";

	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.Numerical;
	public NumericalExerciseDefinition(ulong Id) : base(Id) { }
}

abstract record GeometricExerciseDefinition : ExerciseDefinition {
	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.Geometric;
	public GeometricExerciseDefinition(ulong Id) : base(Id) {
		throw new NotImplementedException("Only if people give enough attention to word probemls and numerical exercises...");
	}
}