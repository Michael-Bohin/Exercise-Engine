namespace ExerciseEngine;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

#region Definition

public class Definition_MetaData { 
	public Language		initialLanguage;
	public ExerciseType type;
	public string		title = "";
	public string		description = "";
	public List<Topic>	topics = new();
	public List<Grade>	grades = new();
	public bool			autoGenerateThumbnail = true;
	public string		thumbnailPath = "";
}

public class Definition
{
    public Definition_MetaData			metaData = new();

    public List<Variable>				variables = new();
	public List<MacroText>				assignment = new();
    public List<ConstraintMethod>		constraints = new();
	public List<string>					imagePaths = new();

	public List<Definition_Question>	questions = new();
}

public class Definition_Question {
	public List<MacroText>				question = new();
	public ResultType					resultType = new();
	public ResultMethod					result = new();
	public List<MacroText>				solutionSteps = new();
	public List<string>					imagePaths = new();
}

#endregion

#region Variable

// Variables:
// 1. Three ways they can throw exceptions:
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

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(Range<double>), "double_rng")]
[JsonDerivedType(typeof(Range<int>), "int_rng")]
[JsonDerivedType(typeof(Set<int>), "int_set")]
[JsonDerivedType(typeof(Set<double>), "double_set")]
[JsonDerivedType(typeof(Set<Operator>), "Operator_set")]
abstract public class Variable {
	public string Name { get; set; } = default!;
	public Variable(string Name) {
		if (!Regex.Match(Name, "^[a-zA-Z_][a-zA-Z_0-9]*$").Success) 
			throw new ArgumentException($"Invalid Id name: '{Name}' ");
		this.Name = Name; 
	}

	// abstract string GetTypeRepr(); // used by interpreter
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

// definitelly longest class name:
// this is required on blazor side as user changes types, this seems to be the only solution
public class Bindable_NotPolymorphic_Variable {
	public string name = default!;
	public SetRange setRange;
	public DataType dataType;
	public int intMax, intMin, intIncrement;
	public double doubleMax, doubleMin, doubleIncrement;
	public string elements = ""; // will be parsed from string for all three int, double, Operator types, so it doesnt make sense to make individual strings for all..

	public Variable CastToPolymorphicVersion() {
		if(setRange == SetRange.Range) 
			return CastToRange();

		return CastToSet();
    }

	Variable CastToRange() {
		if(dataType == DataType.Operator)
			throw new Exception("Blazor bindable not polymorphic variable atempted to instantiate range operator, which does not make much senese.");

		if(dataType == DataType.Int)
			return CastToIntRange();

		return CastToDoubleRange();
    }

	Range<int> CastToIntRange() => new (name, intMin, intMax, intIncrement);
	Range<double> CastToDoubleRange() => new(name, doubleMin, doubleMax, doubleIncrement);

	Variable CastToSet() {
		if (dataType == DataType.Operator)
			return CastToOperatorSet();

		if (dataType == DataType.Int)
			return CastToIntSet();

		return CastToDoubleSet();
	}
	
	Set<Operator> CastToOperatorSet() => new(name, ParseOperatorElements());
	Set<int> CastToIntSet() => new(name, ParseIntElements());
	Set<double> CastToDoubleSet() => new(name, ParseDoubleElements());

	// do both methods for each: validator reuturning bool and parser -> validator will be used repeatedly during UI process, parses once at the end. 

	List<int> ParseIntElements() {
		throw new NotImplementedException("implement parsing!");
	}

	List<double> ParseDoubleElements() {
		throw new NotImplementedException("implement parsing!");
	}

	List<Operator> ParseOperatorElements() {
		throw new NotImplementedException("implement parsing!");
	}
}

#endregion

#region MacroText

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(Macro), "macro")]
[JsonDerivedType(typeof(Text), "text")]
abstract public class MacroText { }

sealed public class Macro : MacroText {
	public string pointer;

	public Macro(string pointer) {
		this.pointer = pointer;
	}
}

sealed public class Text : MacroText {
	public string constText = default!;

	public Text(string constText) => this.constText = constText;
}

#endregion

#region Methods

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(ConstraintMethod), "constraint_method")]
[JsonDerivedType(typeof(ResultMethod), "result_method")]
abstract public class Method {
	public bool codeDefined = new();
	public List<string> code = new();
	public List<string> comments = new();
}

// if constraint method returns true, the variant of exercise is not legit.
public class ConstraintMethod : Method { }

public class ResultMethod : Method {
	public ResultType resultType;
}

#endregion