namespace ExerciseEngine;

internal enum Operator { 
	Add,
	Sub,
	Mul,
	Div
}

internal enum Language { 
	en, 
	pl, 
	ua, 
	cs
}

internal enum Grade { 
	First = 1, 
	Second = 2, 
	Third = 3,
	Fourth = 4,
	Fifth = 5,
	Sixth = 6,
	Seventh = 7,
	Eight = 8,
	Ninth = 9
}

internal enum Topic { 
	Arithmetic, 
	Addition, 
	Subtraction, 
	Multiplication, 
	Division, 
	Modulo, 
	Fractions, 
	Percentages, 
	Combinatorics, 
	BasicUnits
} // and many more..

internal enum ExerciseType { 
	WordProblem,
	Numerical,
	ADS,
	Geometric 
}

internal enum ResultType {
	Int, 
	String, 
	Double, 
	Fraction
}