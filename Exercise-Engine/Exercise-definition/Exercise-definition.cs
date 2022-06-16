namespace Exercise_Engine;

enum JSONId_ExerciseDefinition { WordProblem, Numerical, Geometric }

abstract record ExerciseDefinition {
	public ulong id;
	public Babylon name = new();
	public List<Variable> variables = new();
	public List<Constraint> constraints = new();
	// ----------------------------
	public List<Babylon> solutionSteps = new(); // this is actually 3.5D : 1st D List<>, 2nd D Dictionary with keys Lang, 3rd D List<TextElement>, 4th D partialy Macros pointing to some variable
	public Groups groups = new();

	public ExerciseDefinition() { }
}

abstract record WordProblemDefinition : ExerciseDefinition {
	public Babylon assignment = new();
	public List<Babylon> questions = new();
	public List<string> answers = new(); // once finnished consider types of answers..  how about having answers of concrete type? 
	public ImageDataTypeToBeDecided image = new(); // btw with dragon rocket this will be list of images.. (from vid, 1 per second of vid)

	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.WordProblem;
	public WordProblemDefinition() { }
}

abstract record NumericalExerciseDefinition : ExerciseDefinition {
	public Babylon equation = new();
	public string answer = "";

	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.Numerical;
	public NumericalExerciseDefinition() { }
}

abstract record GeometricExerciseDefinition : ExerciseDefinition {
	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.Geometric;
	public GeometricExerciseDefinition() {
		throw new NotImplementedException("Only if people give enough attention to word probemls and numerical exercises...");
	}
}