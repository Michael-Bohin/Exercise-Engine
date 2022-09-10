namespace ExerciseEngine;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

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

// Major question: Will System.Text.Json be able to handle polymorphic generic classes? 

// Variables:
// 1. Their properties are get only. Assignable only inside constructor. 
// 2. Three ways they can throw exceptions:
//		a. string id is in bad format:
//			i.		must contain only alphanumeric characters or underscore: [a-zA-Z_0-9]
//			ii.		first character can not be a digit
//			iii.	must contain at least one character
//			leading to regex definition: ^[a-zA-Z_][a-zA-Z_0-9]*$
//			iv.		variables must have unique names, however that needs to be checked in one context higher, where all living Variables are accessible
//		b.  For Range:
//			i.	Max must be at least min 
//			ii. Increment must be greater than 0 (default(T))
//		c.  For set:
//			i.  Count of elements must be greater than 0

[JsonPolymorphic]
[JsonDerivedType(typeof(Variable), "var")]
[JsonDerivedType(typeof(Range<int>), "int_rng")]
[JsonDerivedType(typeof(Range<double>), "double_rng")]
[JsonDerivedType(typeof(Set<int>), "int_set")]
[JsonDerivedType(typeof(Set<int>), "double_set")]
[JsonDerivedType(typeof(Set<int>), "Operator_set")]
abstract public class Variable {
	public string Name { get; set; } = default!;
	public Variable(string Name) {
		if (!Regex.Match(Name, "^[a-zA-Z_][a-zA-Z_0-9]*$").Success) 
			throw new ArgumentException($"Invalid Id name: '{Name}' ");
		this.Name = Name; 
	}
}

sealed public class Range<T> : Variable where T : struct, IComparable {
	public Range(string Name, T Min, T Max, T Inc) : base(Name) {
		if(Max.CompareTo(Min) < 0)
			throw new ArgumentException($"Range variable must have maximum equal or greater to minimum. Recieved values: Max: {Max}, Min: {Min} ");
		if(Inc.CompareTo(default(T)) < 0)
			throw new ArgumentException("Increment value of range variable must be greater than 0.");
		this.Min = Min;
		this.Max = Max;
		Increment = Inc;
	}

	public T Min { get; }
	public T Max { get; }
	public T Increment { get; }	
}

// once we will start using strings as variable, struct constraint will have to be removed 
sealed public class Set<T> : Variable where T : struct, IComparable {
	public List<T> Elements { get; } = new();
	public Set(string Name, List<T> Elements) : base(Name) { 
		if(Elements.Count == 0)
			throw new ArgumentException("Set variable must have at least one element in its set. Why consider variable as empty set??? ");
		this.Elements = Elements; 
	}
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