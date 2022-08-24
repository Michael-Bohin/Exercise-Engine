using MudBlazor;

namespace ExerciseEngine;

#region Definition

internal class Definition
{
    public MetaData metaData = new();
	public Language initialLanguage;
    public List<Variable> Variables = new();
    public List<Constraint> Constraints = new();
	public MacroText Assignment = new();
	public List<MacroText> SolutionSteps = new();
}

internal class WordProblemDefinition : Definition
{
    public List<MacroText> Questions = new();
    public List<Result> Results = new(); // once finnished consider types of answers..  how about having answers of concrete type?                        
}

internal class NumericalExerciseDefinition : Definition
{
    public MacroText Question = new();
    public Result Result = new();
}

internal class GeometricExerciseDefinition : Definition
{
    public GeometricExerciseDefinition() : base()
    {
        throw new NotImplementedException("Will be implemented only if people give enough attention to word problems and numerical exercises...");
    }
}

#endregion

#region Variable

abstract internal class Variable {
	public string Id { get; } = default!;
	public Variable(string Id) { this.Id = Id; }
}

internal class IntRange : Variable {
	public int Min { get; }
	public int Max { get; }
	public int Increment { get; }

	public IntRange(string Id, int Min, int Max, int Increment) : base(Id) {
		this.Min = Min;
		this.Max = Max;
		this.Increment = Increment;
	}
}

abstract internal class SetVariable<T> : Variable {
	public List<T> Elements { get; } = new();
	public SetVariable(string Id, List<T> Elements) : base(Id) { this.Elements = Elements; }
}

internal class IntSet : SetVariable<int> {
	public IntSet(string Id, List<int> Elements) : base(Id, Elements) { }
}

internal class OperatorSet : SetVariable<Operator> {
	public OperatorSet(string Id, List<Operator> Elements) : base(Id, Elements) { }
}

// for now skip string variables for simplicity 
// each string variable would need different translations!!
// 
// internal class StringSet : SetVariable<string> {
//	  public StringSet(string Id, List<string> Elements) : base(Id, Elements) { }
// }

internal class DoubleSet : SetVariable<double> {
	public DoubleSet(string Id, List<double> Elements) : base(Id, Elements) { }
}

#endregion

// think about how to save lines of code for interpreter and roslyn analyzer...
#region Constraint

internal class Constraint { }

#endregion

#region Result 

internal class Result { }

#endregion

#region MetaData 

internal class MetaData {
	public string name = default!;
	public List<Topic> topics = new();
	public List<Classes> classes = new();
	public ExerciseType type = ExerciseType.Numerical;

	public MetaData() { }

	public MetaData(string name, List<Topic> topics, List<Classes> classes, ExerciseType type) {
		this.name = name; this.topics = topics; this.classes = classes; this.type = type;
	}
}

#endregion

#region MacroText

internal class MacroText {
	public MacroText() { elements = new(); }
	public List<TextElement> elements;
}

abstract internal class TextElement { }

sealed internal class Macro : TextElement {
	public int pointer;
	
	public Macro(int pointer) {
		this.pointer = pointer;
	}
}

sealed internal class Text : TextElement {
	public string constText = default!;
	
	public Text(string constText) => this.constText = constText;
}

#endregion