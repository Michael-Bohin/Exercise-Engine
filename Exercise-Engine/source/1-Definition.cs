namespace ExerciseEngine;

#region Definition

class Definition_MetaData { 
	public Language		initialLanguage;
	public ExerciseType type;
	public string		title = "";
	public string		description = "";
	public List<Topic>	topcis = new();
	public List<Grade>	grades = new();
	public bool			autoGenerateThumbnail = true;
	public string		thumbnailPath = "";
}

class Definition
{
    public Definition_MetaData metaData = new();

    public List<Variable>			variables = new();
	public List<MacroText>			assignment = new();
	public List<MacroText>			questions = new();
	public ResultType				resultType = new();
	public List<ResultMethod>		results = new();
    public List<ConstraintMethod>	constraints = new();
	public List<MacroText>			solutionSteps = new();
	public List<string>				imagePaths = new();
}

#endregion

#region Variable

abstract class Variable {
	public string Id { get; } = default!;
	public Variable(string Id) { this.Id = Id; }
}

class IntRange : Variable {
	public int Min { get; }
	public int Max { get; }
	public int Increment { get; }

	public IntRange(string Id, int Min, int Max, int Increment) : base(Id) {
		this.Min = Min;
		this.Max = Max;
		this.Increment = Increment;
	}
}

abstract class SetVariable<T> : Variable {
	public List<T> Elements { get; } = new();
	public SetVariable(string Id, List<T> Elements) : base(Id) { this.Elements = Elements; }
}

class IntSet : SetVariable<int> {
	public IntSet(string Id, List<int> Elements) : base(Id, Elements) { }
}

class OperatorSet : SetVariable<Operator> {
	public OperatorSet(string Id, List<Operator> Elements) : base(Id, Elements) { }
}

// for now skip string variables for simplicity 
// each string variable would need different translations!!
// 
// internal class StringSet : SetVariable<string> {
//	  public StringSet(string Id, List<string> Elements) : base(Id, Elements) { }
// }

class DoubleSet : SetVariable<double> {
	public DoubleSet(string Id, List<double> Elements) : base(Id, Elements) { }
}

#endregion

#region MacroText

abstract class MacroText { }

sealed class Macro : MacroText {
	public string pointer;

	public Macro(string pointer) {
		this.pointer = pointer;
	}
}

sealed class Text : MacroText {
	public string constText = default!;

	public Text(string constText) => this.constText = constText;
}

#endregion

#region Methods

abstract class Method {
	public bool codeDefined = new();
	public List<string> code = new();
	public List<string> comments = new();
}

class ConstraintMethod : Method { }

class ResultMethod {
	public ResultType resultType;
}

#endregion