namespace ExerciseEngine.MathRepresentation;

// namespace InfiniteEngine {

using M = MathAlgorithms;
using Q = Fraction;
// using EQ = Educational_RationalNumber;
// implement N, Z, and R in future! :) 

/// I can see several approaches to doing Rational arithemtic 
/// Beacuse of kids, all will need to have its representation in code 
/// For now, the one leading to the result in reduced form will be in place. 
/// Different approaches: (Addition and subtraction)
/// A: ( most steps, but guaranteed smallest product of multiplication )
///     1. Reduce operands 
///     2. Find the least common multiple of denominators 
///     3. Expand operands to LCM denominator 
///     4. Add/Subtract numerators after joining the Ration numbers 
///     5. Reduce result to simplest form 
/// 
/// B: 
///     1. Use equation: (ad+-cb)/bd and dont reduce anything
///     
/// C:  
///     1. B + reduce operands before and result after 
///     
/// D:  1. Find the least common multiple of denominators 
///     2. Expand operands to LCM denominator 
///     3. Add/Subtract numerators after joining the Ration numbers 
///     4. don't reduce anything 

/// IComparable<RationalNumber>
/// https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1?view=net-5.0
/// 
/// defines: 
/// bool CompareTo(Q other); -> negative: this < other, zero: this == equal, positive: this > other. 
/// op_GreaterThan, op_GreaterThanOrEqual
/// op_LessThan, op_LessThanOrEqual
/// 
/// IEquatable<RationalNumber>
/// https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1?view=net-5.0
/// 
/// define: 
/// bool Equals(Q other);
/// override bool Equals(Object obj)
/// override int GetHashCode();
/// static bool operator ==(Q q1, Q q2);
/// static bool opeartor !=(Q q1, Q q2);
///
/// math note:
/// In this code I will consider two different expansions of same simplest form different. 
/// bool IsEquivalent() covers the other comparison semantic where 2/4 is equal to 1/2

/// RationalNumber:
/// Fraction with the constraint of holding some integer as numerator and denominator
/// Denominator can not be zero. Therefore, it is the set Q. 
/// With usefull methods, that are frequently used in primary school math. 
/// Because it is Q, unlike Fractions (which can hold nested infix expression),
/// it inherits from value as real number or integer does.
/// Simplest form definition:
/// 1. denominator is from natural numbers
/// 2. the only divisor of num and den is 1.

interface IRationalNumber {
	double ToDouble();

	bool IsSimplestForm();
	bool IsInteger();

	void Reduce();
	Q GetSimplestForm();

	void Expand(int i); // expand the rational number, throws InvalidOperationException if i == 0
	Q GetExpandedForm(int i);

	Q Copy();

	void Inverse();  // throws MathHellException, if numerator is 0
	Q GetInverse();

	bool IsEquivalent(Q other); // comparison based on comparing simplest forms of two rational numbers

	int[] NumeratorPrimeFactors();
	int[] DenominatorPrimeFactors();
}

interface IRationalArithmetic {
	Q Add(Q addend);
	Q Subtract(Q subtrahend);
	Q Multiply(Q multiplier);
	Q Divide(Q divisor);
	Q Operate(Q other, Operator o);
}

interface ITeacher { }

interface IRationalAtomicOperations : ITeacher {
	// 'Least common denominator'
	(Q, Q) ExpandToLCD(Q other);

	// does all the arithemtic operations without conversion 
	// of the result into simplest form (or the input)
	Q AtomicOperate(Q other, Operator o);
}

public class Fraction : Value, IRationalNumber, IRationalArithmetic,
	IEquatable<Fraction>, IComparable<Fraction>, IRationalAtomicOperations {
	int _num;
	public int Numerator { get => _num; set => _num = value; }

	int _den;
	public int Denominator {
		get => _den;
		set {
			if (value == 0)
				throw new MathRepresentationException(mathHellAlert);
			_den = value;
		}
	}
	const string mathHellAlert = "Math hell alert! 😈🙀 Rational number can not have denominator equal to zero.";

	public Fraction(int numerator, int denominator) {
		if (denominator == 0)
			throw new MathRepresentationException(mathHellAlert);
		_num = numerator;
		_den = denominator;
	}

	public Fraction(int integer) {
		_num = integer;
		_den = 1;
	}

	public static explicit operator Q(int integer) => new(integer, 1);

	/// inherited abstract methods overrides ///
	// Katex to string method:
	public override string ToString() {
		return $"$\\frac{{{_num}}}{{{_den}}}$";
	}
	// public override string ToString() => ToHTML();
	public override Q DeepCopy() => (Q)MemberwiseClone();
	public override string ToHTML() => @"<div class=""frac""><span>" + _num.ToString() + @"</span><span class=""symbol"">/</span><span class=""bottom"">" + _den.ToString() + @"</span></div>";

	/// interface IRationalNumber ///
	public double ToDouble() => (double)_num / (double)_den;

	public bool IsSimplestForm() => !(_den < 0) && M.EuclidsGCD(Math.Abs(_num), _den) == 1;

	public bool IsInteger() => Math.Abs(_num) % Math.Abs(_den) == 0;

	public void Reduce() {
		if (_den < 0) { // if denominator is negative , multiply both num and den by -1
			_den = Math.Abs(_den);
			_num *= -1;
		}

		if (IsSimplestForm())
			return;
		int GCD = M.EuclidsGCD(_num, _den);
		_num /= GCD;
		_den /= GCD;
	}

	public Q GetSimplestForm() {
		Q other = new(_num, _den);
		other.Reduce();
		return other;
	}

	const string invalidExpandException = "Expanding rational number using 0, not only doesn't preserve same simplest form, but also leads to Math Hell Exception!";
	public void Expand(int i) {
		if (i == 0)
			throw new InvalidOperationException(invalidExpandException);
		_num *= i;
		_den *= i;
	}

	public Q GetExpandedForm(int i) {
		if (i == 0)
			throw new InvalidOperationException(invalidExpandException);
		return new(_num * i, _den * i);
	}

	public Q Copy() => new(_num, _den);

	public void Inverse() {
		if (_num == 0)
			throw new InvalidOperationException(invalidExpandException);
		int temp = _num;
		_num = _den;
		_den = temp;
	}

	public Q GetInverse() {
		if (_num == 0)
			throw new MathRepresentationException(mathHellAlert);
		Q other = Copy();
		other.Inverse();
		return other;
	}

	public bool IsEquivalent(Q other) {
		Q a = Copy();
		Q b = other.Copy();
		a.Reduce();
		b.Reduce();
		return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
	}

	// ! treats numerator as absolute value
	public int[] NumeratorPrimeFactors() => GetPrimeFactors(_num);
	public int[] DenominatorPrimeFactors() => GetPrimeFactors(_den);

	static int[] GetPrimeFactors(int number) {
		List<int> primeFactors = new();
		if (number == 0)
			return primeFactors.ToArray();

		number = Math.Abs(number);

		if (number == 1) {
			primeFactors.Add(1);
			return primeFactors.ToArray();
		}

		int divisor = 2;
		while (number > 1) {
			if (number % divisor == 0) {
				primeFactors.Add(divisor);
				number /= divisor;
			} else {
				divisor++; // can be sped up by only using primes
			}
		}
		return primeFactors.ToArray();
	}

	/*public Fraction GetPrimeFactorization() {
		int[] numPF = NumeratorPrimeFactors();
		int[] denPF = DenominatorPrimeFactors();
		Expression num = Fraction.CreatePrimeProduct(new List<int>(numPF));
		Expression den = Fraction.CreatePrimeProduct(new List<int>(denPF));
		return new Fraction(num, den);
	}*/

	///
	/// interface IRationalArithmetic ///
	///

	public Q Add(Q addend) {
		// a/b + c/d = (ad+bc)/bd -> can be shortcuted using finding LCM of simplest forms 
		(Q a, Q b) = PrepareAddSub(addend);
		a.Numerator += b.Numerator;
		a.Reduce();
		return a;
	}

	(Q a, Q b) PrepareAddSub(Q rightOperand) {
		Q a = GetSimplestForm();
		Q b = rightOperand.GetSimplestForm();

		int LCM = M.EuclidsLCM(a.Denominator, b.Denominator);
		if (LCM != a.Denominator)
			a.Expand(LCM / a.Denominator);

		if (LCM != b.Denominator)
			b.Expand(LCM / b.Denominator);

		return (a, b);
	}

	public Q Subtract(Q subtrahend) {
		(Q a, Q b) = PrepareAddSub(subtrahend);
		a.Numerator -= b.Numerator;
		a.Reduce();
		return a;
	}

	public Q Multiply(Q multiplier) {
		Q a = GetSimplestForm();
		Q b = multiplier.GetSimplestForm();

		a.Numerator *= b.Numerator;
		a.Denominator *= b.Denominator;
		a.Reduce();
		return a;
	}

	public Q Divide(Q divisor) {
		if (divisor.Numerator == 0)
			throw new MathRepresentationException(mathHellAlert);

		return Multiply(divisor.GetInverse());
	}

	public static Q operator -(Q a) => new(-a._num, a._den);
	public static Q operator +(Q a, Q b) => a.Add(b);
	public static Q operator -(Q a, Q b) => a.Subtract(b);
	public static Q operator *(Q a, Q b) => a.Multiply(b);
	public static Q operator /(Q a, Q b) {
		if (b.Numerator == 0)
			throw new DivideByZeroException();
		return a.Divide(b);
	}

	public Q Operate(Q other, Operator o) {
		if (o == Operator.Add) {
			return this + other;
		} else if (o == Operator.Sub) {
			return this - other;
		} else if (o == Operator.Mul) {
			return this * other;
		} else {
			return this / other;
		}
	}

	///
	/// interface IEquatable<RationalNumber> ///
	///

	// avoid inverse fractions returning same product 
	public override int GetHashCode() => _num.GetHashCode() * _den.GetHashCode() + _num.GetHashCode();

	public bool Equals(Q other) {
		if (other == null)
			return false;

		return _num == other.Numerator && _den == other.Denominator;
	}

	public override bool Equals(object obj) {
		if (obj == null)
			return false;

		Q fractionObj = obj as Q;
		if (fractionObj == null)
			return false;
		else
			return Equals(fractionObj);
	}

	public static bool operator ==(Q person1, Q person2) {
		if (((object)person1) == null || ((object)person2) == null)
			return Object.Equals(person1, person2);

		return person1.Equals(person2);
	}

	public static bool operator !=(Q person1, Q person2) {
		if (((object)person1) == null || ((object)person2) == null)
			return !Object.Equals(person1, person2);

		return !(person1.Equals(person2));
	}

	public int CompareTo(Q other) {
		// If other is not a valid object reference, this instance is greater.
		if (other == null) return 1;

		// reduce both to simplest form 
		// find least common multiple of both denominators
		// extend both fraction to it 
		// compare only numerators :) 
		Q a = GetSimplestForm();
		Q b = other.GetSimplestForm();
		int LCM = M.EuclidsLCM(a.Denominator, b.Denominator);
		if (LCM != a.Denominator)
			a.Expand(LCM / a.Denominator);
		if (LCM != b.Denominator)
			b.Expand(LCM / b.Denominator);

		return a.Numerator.CompareTo(b.Numerator);
	}

	public static bool operator >(Q operand1, Q operand2) => operand1.CompareTo(operand2) > 0;
	public static bool operator <(Q operand1, Q operand2) => operand1.CompareTo(operand2) < 0;
	public static bool operator >=(Q operand1, Q operand2) => operand1.CompareTo(operand2) >= 0;
	public static bool operator <=(Q operand1, Q operand2) => operand1.CompareTo(operand2) <= 0;

	///
	/// interface IRationalAtomicOperations ///
	///

	// does all the arithemtic operations without conversion 
	// of the result into simplest form (or the input)
	public Q AtomicOperate(Q other, Operator o) {
		if (o == Operator.Add) {
			if (Denominator != other.Denominator) {
				(Q a, Q b) = ExpandToLCD(other);
				return new(a.Numerator + b.Numerator, a.Denominator);
			}
			return new(Numerator + other.Numerator, Denominator);
		}

		if (o == Operator.Sub) {
			if (Denominator != other.Denominator) {
				(Q a, Q b) = ExpandToLCD(other);
				return new(a.Numerator - b.Numerator, a.Denominator);
			}
			return new(Numerator - other.Numerator, Denominator);
		}
		return o == Operator.Mul ? new Q(Numerator * other.Numerator, Denominator * other.Denominator) : new Q(Numerator * other.Denominator, Denominator * other.Numerator);
	}

	public (Q, Q) ExpandToLCD(Q other) {
		int lcd = M.EuclidsLCM(Denominator, other.Denominator);
		Q a = Copy();
		Q b = other.Copy();
		a.Expand(lcd / a.Denominator);
		b.Expand(lcd / b.Denominator);
		return (a, b);
	}
}

// }

/// https://docs.microsoft.com/en-us/dotnet/api/system.iformattable?view=net-5.0
/// Study IFormattable in depth and if usefull implement it in future
///
/// https://github.com/microsoft/referencesource/blob/master/mscorlib/system/double.cs
/// IArithmetic<Double> template at the end 