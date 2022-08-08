namespace ExerciseEngine.Editor;

public enum JsonVariable { IntRange, IntSet, OperatorSet, StringSet, DoubleSet/*, FractionSet*/ } // more types are likelly to come in future
public enum Operator { Add, Sub, Mul, Div }

abstract public class Variable {
	public string Id { get; } = default!;
	protected Variable(string Id) { this.Id = Id; }

	// more abstract methods will be added as I see the needs for the interpreter to fluently write the necessary code.
	abstract public string GetValueType();
	abstract public string PrintForeachDef();
}

public class IntRange : Variable {
	public int Min { get; }
	public int Max { get; }
	public int Increment { get; }

	public IntRange(string Id, int Min, int Max, int Increment) : base(Id) {
		this.Min = Min;
		this.Max = Max;
		this.Increment = Increment;
	}

	public static JsonVariable JsonId { get => JsonVariable.IntRange; }
	public override string GetValueType() => "int";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

abstract public class SetVariable<T> : Variable {
	public List<T> Elements { get; }
	public SetVariable(string Id, List<T> Elements) : base(Id) { this.Elements = Elements; }
}

public class IntSet : SetVariable<int> {
	public IntSet(string Id, List<int> Elements) : base(Id, Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.IntSet; }
	public override string GetValueType() => "int";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

public class OperatorSet : SetVariable<Operator> {
	public OperatorSet(string Id, List<Operator> Elements) : base(Id, Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.OperatorSet; }
	public override string GetValueType() => "Operator";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

public class StringSet : SetVariable<string> {
	public StringSet(string Id, List<string> Elements) : base(Id, Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.StringSet; }
	public override string GetValueType() => "string";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

public class DoubleSet : SetVariable<double> {
	public DoubleSet(string Id, List<double> Elements) : base(Id, Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.DoubleSet; }
	public override string GetValueType() => "double";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

// !!! fraction must be added...
/*
class Fraction {
	public Fraction() {
		throw new NotImplementedException();
	}
}

class FractionSet : SetVariable<Fraction> {
	public FractionSet(string Id, List<Fraction> Elements) : base(Id, Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.FractionSet; }
	public override string GetValueType() => "Fraction";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}*/