using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text;

namespace ExerciseEngine.ExerciseCompiler;

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
[JsonDerivedType(typeof(DoubleRange), "double_rng")]
[JsonDerivedType(typeof(IntRange), "int_rng")]
[JsonDerivedType(typeof(IntSet), "int_set")]
[JsonDerivedType(typeof(DoubleSet), "double_set")]
[JsonDerivedType(typeof(OperatorSet), "Operator_set")]
abstract public class Variable {
	public string Name { get; set; } = default!;
	public Variable(string Name) {
		if (!Regex.Match(Name, "^[a-zA-Z_][a-zA-Z_0-9]*$").Success)
			throw new ArgumentException($"Invalid Id name: '{Name}' ");
		this.Name = Name;
	}

	abstract public int GetCardinality();

	abstract public string GetCodeSetInstantionLine();

	abstract public string GetForLoopLine();

	abstract public string TypeRepresentation();

	abstract public bool IsSet();
}

public abstract class Range<T> : Variable where T : struct, IComparable {
	public Range(string Name, T Min, T Max, T Inc) : base(Name) {
		if (Max.CompareTo(Min) < 0)
			throw new ArgumentException($"Range variable must have maximum equal or greater to minimum. Recieved values: Max: {Max}, Min: {Min} ");
		if (Inc.CompareTo(default(T)) < 0)
			throw new ArgumentException("Increment value of range variable must be greater than 0.");
		this.Min = Min;
		this.Max = Max;
		Increment = Inc;
	}

	public T Min { get; }
	public T Max { get; }
	public T Increment { get; }

	public override string GetCodeSetInstantionLine() => ""; // ranges don't need this -> returning empty string make the code inside interpreter simpler, as interpreter just calls the method on all variables.

	public override string GetForLoopLine() {
		StringBuilder sb = new();
		sb.Append($"for ({TypeRepresentation()} {Name} = {Min}; {Name} <= {Max}; ");
		if (Increment.CompareTo(1) == 0) {
			sb.Append($"{Name}++");
		} else {
			sb.Append($" {Name} = {Name} + {Increment} ");
		}
		sb.Append(") {");
		return sb.ToString();
	}

	public override bool IsSet() => false;
}

sealed public class IntRange : Range<int> {
	public IntRange(string Name, int Min, int Max, int Inc = 1) : base(Name, Min, Max, Inc) { }

	public override int GetCardinality() {
		int rozsah = Max - Min;
		return rozsah / Increment + 1;
	}

	public override string TypeRepresentation() => "int";
}

sealed public class DoubleRange : Range<double> {
	public DoubleRange(string Name, double Min, double Max, double Inc) : base(Name, Min, Max, Inc) { }

	public override int GetCardinality() {
		double rozsah = Max - Min;
		double exclusiveCardinality = rozsah / Increment;
		return (int)exclusiveCardinality + 1;
	}

	public override string TypeRepresentation() => "double";
}

// once we will start using strings as variable, struct constraint will have to be removed 
public abstract class Set<T> : Variable where T : struct, IComparable {
	public List<T> Elements { get; } = new();
	public Set(string Name, List<T> Elements) : base(Name) {
		if (Elements.Count == 0)
			throw new ArgumentException("Set variable must have at least one element in its set. Why consider variable as empty set??? ");
		this.Elements = Elements;
	}

	public override int GetCardinality() => Elements.Count;

	public override string GetForLoopLine() => $"foreach({TypeRepresentation()} {Name} in Elements_{Name}) " + '{';

	public override string GetCodeSetInstantionLine() {
		StringBuilder sb = new();
		sb.Append($"List<{TypeRepresentation()}> Elements_{Name} = new() " + '{' + ' ');

		for (int i = 0; i < Elements.Count; i++) {
			if (i != 0)
				sb.Append(", ");
			sb.Append(GetElementRepr(Elements[i]));
		}

		sb.Append("};");
		sb.ToString();
		return sb.ToString();
	}

	public abstract string GetElementRepr(T element);

	public override bool IsSet() => true;
}

sealed public class IntSet : Set<int> {
	public IntSet(string Name, List<int> Elements) : base(Name, Elements) { }

	public override string TypeRepresentation() => "int";

	public override string GetElementRepr(int element) => element.ToString();
}

sealed public class DoubleSet : Set<double> {
	public DoubleSet(string Name, List<double> Elements) : base(Name, Elements) { }

	public override string TypeRepresentation() => "double";

	public override string GetElementRepr(double element) => element.ToString();
}

sealed public class OperatorSet : Set<Operator> {
	public OperatorSet(string Name, List<Operator> Elements) : base(Name, Elements) { }

	public override string TypeRepresentation() => "Operator";

	public override string GetElementRepr(Operator element) => $"Operator.{element}";
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
		if (setRange == SetRange.Range)
			return CastToRange();

		return CastToSet();
	}

	Variable CastToRange() {
		if (dataType == DataType.Operator)
			throw new Exception("Blazor bindable not polymorphic variable atempted to instantiate range operator, which does not make much senese.");

		if (dataType == DataType.Int)
			return CastToIntRange();

		return CastToDoubleRange();
	}

	IntRange CastToIntRange() => new(name, intMin, intMax, intIncrement);
	DoubleRange CastToDoubleRange() => new(name, doubleMin, doubleMax, doubleIncrement);

	Variable CastToSet() {
		if (dataType == DataType.Operator)
			return CastToOperatorSet();

		if (dataType == DataType.Int)
			return CastToIntSet();

		return CastToDoubleSet();
	}

	OperatorSet CastToOperatorSet() => new(name, ParseOperatorElements());
	IntSet CastToIntSet() => new(name, ParseIntElements());
	DoubleSet CastToDoubleSet() => new(name, ParseDoubleElements());

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
