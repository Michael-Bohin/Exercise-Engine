using System.Data;
using System.Text.RegularExpressions;
using ExerciseEngine;

namespace ExerciseEngine.Editor;


abstract record ExerciseDefinition {
	public DefinitionMetaData metaData = new();

	public List<Variable> Variables { get; set; } = new();
	public List<Constraint> Constraints { get; set; } = new();
	// ----------------------------
	public List<MacroText> SolutionSteps { get; set; } = new(); // this is actually 3.5D : 1st D List<>, 2nd D Dictionary with keys Lang, 3rd D List<TextElement>, 4th D partialy Macros pointing to some variable
	// public ExerciseDefinition() { }
}

abstract record WordProblemDefinition : ExerciseDefinition {
	public MacroText Assignment { get; set; } = new();
	public List<MacroText> Questions { get; set; } = new();
	public List<string> Answers { get; set; } = new(); // once finnished consider types of answers..  how about having answers of concrete type? 
	// public ImageDataTypeToBeDecided Image { get; set; } = new(); // btw with dragon rocket this will be list of images.. (from vid, 1 per second of vid)
	// public WordProblemDefinition() { }
}

abstract record NumericalExerciseDefinition : ExerciseDefinition {
	public MacroText Question { get; set; } = new(); // expression/equation to solve
	public string Answer { get; set; } = "";
	// public NumericalExerciseDefinition() : base() { }
}

abstract record GeometricExerciseDefinition : ExerciseDefinition {
	public GeometricExerciseDefinition() : base() {
		throw new NotImplementedException("Will be implemented only if people give enough attention to word problems and numerical exercises...");
	}
}