namespace Exercise_Engine;

enum JSONId_ExerciseDefinition { WordProblem, Numerical, Geometric }

abstract record ExerciseDefinition {
	public ulong id;
	public Babylon name; 
	public List<Variable> variables;
	public List<Constraint> constraints;
	// ----------------------------
	public List<Babylon> solutionSteps;
	public Groups groups;

	public ExerciseDefinition() {
		name = new();
		variables = new();
		constraints = new();
		solutionSteps = new();
		groups = new();
	}
}

abstract record WordProblemDefinition : ExerciseDefinition {
	public Babylon assignment;
	public List<Babylon> questions;
	public List<string> answers; // once finnished consider types of answers..  how about having answers of concrete type? 
	public ImageDataTypeToBeDecided image; // btw with dragon rocket this will be list of images.. (from vid, 1 per second of vid)
	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.WordProblem;
	public WordProblemDefinition() {
		assignment = new();
		questions = new();
		answers = new();
		image = new();
	}
}

abstract record NumericalExerciseDefinition : ExerciseDefinition {
	public Text equation;
	public string answer;
	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.Numerical;

	public NumericalExerciseDefinition() {
		equation = new();
		answer = "";
	}
}

abstract record GeometricExerciseDefinition : ExerciseDefinition {
	public const JSONId_ExerciseDefinition PolyJSONid = JSONId_ExerciseDefinition.Geometric;
	public GeometricExerciseDefinition() {
		throw new NotImplementedException("Only if people give enough attention to word probemls and numerical exercises...");
	}
}