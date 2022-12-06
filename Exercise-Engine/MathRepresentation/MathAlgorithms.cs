namespace ExerciseEngine.MathRepresentation;

using Q = Fraction;

class MathAlgorithms {
	// Contains general math algorithms, that are usefull across different topics of math.
	// There are either multiple steps algorithms, or relatively simple equations 
	// that given a long name of function improve code readability of generators. 

	// find greatest common divisor of two integers:
	public static int EuclidsGCD(int a, int b) {
		a = Math.Abs(a);
		b = Math.Abs(b);

		if (a == 0)
			return b;
		if (b == 0)
			return a;

		int t;
		while (a % b != 0) {
			t = b;
			b = a % b;
			a = t;
		}
		return b;
	}

	// least common multiple -> multiply the two numbers and divide the result by GCD
	public static int EuclidsLCM(int a, int b) => a * b / EuclidsGCD(a, b);

	// je reduced vysledek operace cele cislo? 
	public static bool AritmetikaVedeNaNeceleCislo(Q A, Q B, Operator op) => op == Operator.Add ? !(A + B).IsInteger() : !(A - B).IsInteger();

	// existuje k z prirozenych cisel t.ž.: 
	// k >= 2 && (k * y) == x && x neni nula 
	public static bool XJeKNasobekY(int x, int y) => x % y == 0 && x != y && x != 0;
}