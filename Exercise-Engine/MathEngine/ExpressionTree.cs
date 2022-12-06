namespace ExerciseEngine.MathEngine;

public abstract class Expression {
	public override abstract string ToString(); // override object.ToString so that compiler forces own implementation 
	public abstract string ToHTML();            // using predefined css tricks for fractions
	public abstract Expression DeepCopy();
}

public abstract class BinaryExpression : Expression {
	public Expression leftOperand;
	public Expression rightOperand;
	protected abstract string SignRepresentation { get; }

	public BinaryExpression(int a, int b) {
		leftOperand = new Integer(a);
		rightOperand = new Integer(b);
	}

	public BinaryExpression(double a, double b) {
		leftOperand = new RealNumber(a);
		rightOperand = new RealNumber(b);
	}

	public BinaryExpression(double a, Expression b) : this(new RealNumber(a), b) { }

	public BinaryExpression(Expression a, Expression b) {
		leftOperand = a;
		rightOperand = b;
	}
	public override string ToString() {
		if (this is Addition || this is Subtraction) // assert infix notation doesnt change due to priority of operators
			return $"({leftOperand}{SignRepresentation}{rightOperand})";
		return $"{leftOperand}{SignRepresentation}{rightOperand}";
	}

	public override string ToHTML() => $"{leftOperand.ToHTML()} {SignRepresentation} {rightOperand.ToHTML()}";

	public override abstract BinaryExpression DeepCopy();
}
public abstract class UnaryExpression : Expression {
	public Expression operand;
	protected abstract string SignRepresentation { get; }
	public override string ToString() => $"({SignRepresentation}{operand})";

	public override string ToHTML() => $"{SignRepresentation}{operand.ToHTML()}";

	public override UnaryExpression DeepCopy() {
		UnaryExpression other = (UnaryExpression)MemberwiseClone();
		other.operand = operand.DeepCopy();
		return other;
	}
}

public abstract class Value : Expression { }

public abstract class Constant : Value {
	public ValueType number;
}

public class Addition : BinaryExpression {
	public Addition(int a, int b) : base(a, b) { }
	public Addition(double a, double b) : base(a, b) { }
	public Addition(Expression a, Expression b) : base(a, b) { }
	public Addition(double a, Expression b) : base(a, b) { }

	protected override string SignRepresentation => "+";

	public Expression LeftSummand { // levy scitanec
		get => leftOperand;
		set => leftOperand = value;
	}

	public Expression RightSummand {
		get => rightOperand;
		set => rightOperand = value;
	}
	// not yet defined Sum : soucet

	public override Addition DeepCopy() {
		Addition other = (Addition)MemberwiseClone();
		other.leftOperand = leftOperand.DeepCopy();
		if (rightOperand == null) {
			other.rightOperand = null;
		} else {
			other.rightOperand = rightOperand.DeepCopy();
		}
		return other;
	}
}

public class Subtraction : BinaryExpression {
	public Subtraction(int a, int b) : base(a, b) { }
	public Subtraction(double a, double b) : base(a, b) { }
	public Subtraction(Expression a, Expression b) : base(a, b) { }
	public Subtraction(double a, Expression b) : base(a, b) { }


	protected override string SignRepresentation => "−";

	public Expression Minuend { // mensenec
		get => leftOperand;
		set => leftOperand = value;
	}

	public Expression Subtrahend { // mensitel 
		get => rightOperand;
		set => rightOperand = value;
	}
	// not yet defined Difference : rozdil
	public override Subtraction DeepCopy() {
		Subtraction other = (Subtraction)MemberwiseClone();
		other.leftOperand = leftOperand.DeepCopy();
		if (rightOperand == null) {
			other.rightOperand = null;
		} else {
			other.rightOperand = rightOperand.DeepCopy();
		}

		return other;
	}
}

public class Multiplication : BinaryExpression {
	public Multiplication(int a, int b) : base(a, b) { }
	public Multiplication(double a, double b) : base(a, b) { }
	public Multiplication(Expression a, Expression b) : base(a, b) { }
	public Multiplication(double a, Expression b) : base(a, b) { }

	protected override string SignRepresentation => "∙";

	public Expression LeftFactor { // levy cinitel
		get => leftOperand;
		set => leftOperand = value;
	}

	public Expression RightFactor {
		get => rightOperand;
		set => rightOperand = value;
	}
	// not yet defined Product : soucin
	public override Multiplication DeepCopy() {
		Multiplication other = (Multiplication)MemberwiseClone();
		other.leftOperand = leftOperand.DeepCopy();
		other.rightOperand = rightOperand.DeepCopy();
		return other;
	}
}

public class Division : BinaryExpression {
	public Division(int a, int b) : base(a, b) { }
	public Division(double a, double b) : base(a, b) { }
	public Division(Expression a, Expression b) : base(a, b) { }
	public Division(double a, Expression b) : base(a, b) { }

	protected override string SignRepresentation => ":";

	public Expression Dividend { // delenec
		get => leftOperand;
		set => leftOperand = value;
	}

	public Expression Divisor { // delitel
		get => rightOperand;
		set => rightOperand = value;
	}
	// not yet defined Quotient : podil
	public override Division DeepCopy() {
		Division other = (Division)MemberwiseClone();
		other.leftOperand = leftOperand.DeepCopy();
		other.rightOperand = rightOperand.DeepCopy();
		return other;
	}
}

interface IFraction {
	void PrimeFactorization(); // change numerator and denomintor to consist of prime factors
	bool NumAndDenAreIntegers(); // are numerator and denominator integer expressions? 
	bool IsSimplestForm(); // if they are integers, is it in simplest form? 
	void Reduce(); // gcd = GCD(num, den); num = num / gcd; den = den / gcd;
	List<int> GetPrimeFactors(int i); // given integer, return list of its prime factors 
										// (disabled beacuse I need it static) int EuclidsGCD(int a, int b); // given two integers, return their GCD
}

class MathRepresentationException : InvalidOperationException {
	public MathRepresentationException() { }
	public MathRepresentationException(string error) : base(error) { }
}


// think this through, should I place rational number in here?
// disable for now for namespace safty..
/*

public class Fraction : BinaryExpression, IFraction {
	public Fraction(int a, int b) : base(a, b) {
		if (b == 0)
			throw new MathRepresentationException("Attempted to create fraction with zero denominator. Division by zero detected.");
	}
	public Fraction(double a, double b) : base(a, b) {
		if (b == 0)
			throw new MathRepresentationException("Attempted to create fraction with zero denominator. Division by zero detected.");
	}
	public Fraction(Expression a, Expression b) : base(a, b) {
		if ((b is Integer i && i.number == 0) || (b is RealNumber r && r.number == 0))
			throw new MathRepresentationException("Attempted to create fraction with zero denominator. Division by zero detected.");
	}
	public Fraction(double a, Expression b) : base(a, b) { }
	public Fraction(string a, string b) : base(new Variable(1, a), new Variable(1, b)) { }

	protected override string SignRepresentation => "/";
	public override string ToString() => $"({Numerator})/({Denominator})";
	// it nessecary to throw in some tricks here, since drawing proper Fractions in html is not trivial or inbuilt into web browsers
	public override string ToHTML() => @"<div class=""frac""><span>" + Numerator.ToHTML() + @"</span><span class=""symbol"">/</span><span class=""bottom"">" + Denominator.ToHTML() + @"</span></div>";
	public static string ToHTML(string left, string right) => @"<div class=""frac""><span>" + left + @"</span><span class=""symbol"">/</span><span class=""bottom"">" + right + @"</span></div>";

	public double ToDouble() {
		if (Numerator is Integer i && Denominator is Integer j)
			return (double)i.number / (double)j.number;
		throw new NotImplementedException("Converting other classes from object tree other than Integers is not yet supported.");
	}

	public Expression Numerator { // citatel
		get => leftOperand;
		set => leftOperand = value;
	}

	public Expression Denominator { // jmenovatel
		get => rightOperand;
		set => rightOperand = value;
	}
	// not yet defined Quotient : podil
	public override Fraction DeepCopy() {
		Fraction other = (Fraction)MemberwiseClone();
		other.leftOperand = leftOperand.DeepCopy();
		other.rightOperand = rightOperand.DeepCopy();
		return other;
	}

	/* Prime factorization 

	// this function assumes that both numerator and denominator are integers!
	// so first check it does whether num and den are Integers, if not returns doing nothing 
	// if both are integers, it gets prime factorization 
	// and than creates multiplication expressions that represent it
	// for integers 0 and 1  2  3 it keeps the integer intact

	public void PrimeFactorization() {
		if (Numerator is Integer num && Denominator is Integer den) {
			int n = num.number;
			if (n == 0)
				return;  // -> making prim factors of zero does not make much sense
			int d = den.number;
			List<int> primesOfNum = GetPrimeFactors(n);
			List<int> primesOfDen = GetPrimeFactors(d);
			// create chained multiplication expressions representing primes 
			Numerator = CreatePrimeProduct(primesOfNum);
			Denominator = CreatePrimeProduct(primesOfDen);
		}
	}

	public List<int> GetPrimeFactors(int number) {
		List<int> primeFactors = new();
		if (number == 0)
			return primeFactors;

		if (number < 0)
			number = Math.Abs(number);

		if (number == 1) {
			primeFactors.Add(1);
			return primeFactors;
		}

		int divisor = 2;
		while (number > 1) {
			if (number % divisor == 0) {
				primeFactors.Add(divisor);
				number /= divisor;
			} else {
				divisor++; // can be sped up by only using primes, for 9th grade, you can allocate them 
			}
		}
		return primeFactors;
	}

	public static Expression CreatePrimeProduct(List<int> primeFactors) {
		if (primeFactors.Count == 0)
			throw new InvalidOperationException("Cannot create expression from empty list of numbers. :(");
		if (primeFactors.Count == 1)
			return new Integer(primeFactors[0]);

		Multiplication expr = new(primeFactors[^2], primeFactors[^1]);

		for (int i = primeFactors.Count - 3; i > -1; i--) {
			Multiplication temp = expr;
			expr = new(primeFactors[i], temp);
		}
		return expr;
	}

	public bool NumAndDenAreIntegers() => Numerator is Integer && Denominator is Integer;

	public bool IsSimplestForm() => Numerator is Integer num && Denominator is Integer den && EuclidsGCD(Math.Abs(num.number), Math.Abs(den.number)) == 1;

	public static int EuclidsGCD(int a, int b) { // euclids algorithm for greatest common divisor 
		if (b == 0)
			return a;
		return EuclidsGCD(b, a % b);
	}

	// least common multiple -> multiply the two numbers and divide the result by GCD
	public static int EuclidsLCM(int a, int b) => a * b / EuclidsGCD(a, b);

	public void Reduce() {
		// if both num and den are prime 
		// get gcd using euclids algorithm
		// if it is greater than one 
		// use it to create new instances of integers that represent num and den 
		if (Numerator is Integer num && Denominator is Integer den) {
			int n = num.number;
			int d = den.number;
			int GCD = EuclidsGCD(n, d);
			Numerator = new Integer(n / GCD);
			Denominator = new Integer(d / GCD);
		}
	}


	/* Equality and arithmetic operators 
	public static bool operator ==(Fraction a, Fraction b) {
		if (a.Numerator is Integer aNum && a.Denominator is Integer aDen && b.Numerator is Integer bNum && b.Denominator is Integer bDen) {
			return aNum.number == bNum.number && bDen.number == aDen.number;
		}
		return false;
	}

	public static bool operator !=(Fraction a, Fraction b) {
		if (a.Numerator is Integer aNum && a.Denominator is Integer aDen && b.Numerator is Integer bNum && b.Denominator is Integer bDen) {
			return !(aNum.number == bNum.number && bDen.number == aDen.number);
		}
		return false;
	}

	public override bool Equals(object obj) {
		if (obj == null)
			return false;
		return obj is Fraction f && f == this;
	}

	public bool Equals(Fraction other) => this == other;

	public static Fraction operator +(Fraction a, Fraction b) {
		if (a.Numerator is Integer NumeratorA && a.Denominator is Integer DenominatorA && b.Numerator is Integer NumeratorB && b.Denominator is Integer DenominatorB) {
			// ? How do I treat fractions in not simplest form? Say 50/100 + 4/8? Do I convert them to 200/400 + 200 /400 ? Or Do I first reduce them to simplest form? 
			// find least common multiple
			// divide all members with it (if it is different from 1 and 0)
			// add up current numerators
			// create new fraction

			int Anum = NumeratorA.number;
			int Aden = DenominatorA.number;
			int Bnum = NumeratorB.number;
			int Bden = DenominatorB.number;

			if (Anum == 0)
				return b.DeepCopy();
			if (Bnum == 0)
				return a.DeepCopy();

			int LCM = EuclidsLCM(Aden, Bden);

			if (Aden != LCM)
				Anum *= (LCM / Aden);
			if (Bden != LCM)
				Bnum *= (LCM / Bden);

			return new Fraction(Anum + Bnum, LCM);
		}
		throw new InvalidOperationException("Attempted yet undefined arithemtic of fractions with other than integer members!");
	}

	public override int GetHashCode() => Numerator.GetHashCode() * Denominator.GetHashCode() + Denominator.GetHashCode(); // to plus tam je pro odliseni 'inverznich' zlomku
}*/

public class Minus : UnaryExpression {
	public Minus(int i) { operand = new Integer(i); }
	public Minus(double d) { operand = new RealNumber(d); }
	public Minus(Expression e) { operand = e; }
	protected override string SignRepresentation => "− ";
}

public class Integer : Constant {
	public readonly new int number;
	public Integer(int i) {
		number = i;
	}

	public override string ToString() => number.ToString();
	public override string ToHTML() => number.ToString();

	public override Integer DeepCopy() => (Integer)MemberwiseClone();
	public override int GetHashCode() => number.GetHashCode();
}

public class RealNumber : Constant {
	public readonly new double number;
	public RealNumber(double d) {
		number = d;
	}

	public override string ToString() => number.ToString();
	public override string ToHTML() => number.ToString();
	public override RealNumber DeepCopy() => (RealNumber)MemberwiseClone();
}

public class Variable : Value {
	public readonly string variableName;
	public readonly Constant constant;

	public Variable(Constant c, string s) {
		constant = c;
		variableName = s;
	}
	public Variable(int i, string s) {
		constant = new Integer(i);
		variableName = s;
	}
	public Variable(double r, string s) {
		constant = new RealNumber(r);
		variableName = s;
	}

	public override string ToString() => $"{constant}{variableName}";

	public override string ToHTML() {
		if (constant.ToString() == "1")
			return variableName;

		return $"{constant.ToHTML()}{variableName}";
	}

	public override Variable DeepCopy() {
		// using Microsoft's docs recomended approach:
		// https://docs.microsoft.com/en-us/dotnet/api/system.string.copy?view=net-5.0
		string otherName = variableName[..];
		if (constant is Integer i) {
			Integer otherConstant = new(i.number);
			return new Variable(otherConstant, otherName);
		} else if (constant is RealNumber r) {
			RealNumber otherConstant = new(r.number);
			return new Variable(otherConstant, otherName);
		}
		// Waiting exception in case someone in future adds constants child and doesnt update code here
		// I will want to change the architecture in such a way that this is not nesseccary. 
		throw new Exception("DeepCopy method of Variable class encountered unknown Constant's child.");
	}
}