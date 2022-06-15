namespace Exercise_Engine;

enum JsonVariable { IntRange, IntSet, OperatorSet, StringSet, DoubleSet, FractionSet } // more types are likelly to come in future
enum Operator { Add, Sub, Mul, Div }

abstract class Variable {
	public string Id { get; } = default!;
	// protected JsonVariable _JsonId;
	// abstract public List<T> GetElements();

	// more abstract methods will be added as I see the needs for the interpreter to fluently write the necessary code.
	abstract public string GetValueType();
	abstract public string PrintForeachDef();
}

class IntRange : Variable {
	public int Min { get; }
	public int Max { get; }
	public int Increment { get; }
	
	public IntRange(int Min, int Max, int Increment) {
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

abstract class SetVariable<T> : Variable {
	public List<T> Elements { get; }
	public SetVariable(List<T> Elements) { this.Elements = Elements; }
}

class IntSet : SetVariable<int> {
	public IntSet(List<int> Elements): base(Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.IntSet; }
	public override string GetValueType() => "int";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

class OperatorSet : SetVariable<Operator> {
	public OperatorSet(List<Operator> Elements) : base(Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.OperatorSet; }
	public override string GetValueType() => "Operator";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

class StringSet : SetVariable<string> {
	public StringSet(List<string> Elements) : base(Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.StringSet; }
	public override string GetValueType() => "string";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}

class DoubleSet : SetVariable<double> {
	public DoubleSet(List<double> Elements) : base(Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.DoubleSet; }
	public override string GetValueType() => "double";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}


// !!! fraction must be added...
class Fraction {
	public Fraction() {
		throw new NotImplementedException();
	}
}

class FractionSet : SetVariable<Fraction> {
	public FractionSet(List<Fraction> Elements) : base(Elements) { }

	public static JsonVariable JsonId { get => JsonVariable.FractionSet; }
	public override string GetValueType() => "Fraction";
	public override string PrintForeachDef() {
		throw new NotImplementedException();
	}
}



