﻿using System.Text.Json.Serialization;

namespace ExerciseEngine;

public enum Operator { 
	Add,
	Sub,
	Mul,
	Div
}

public enum Language { 
	en, 
	pl, 
	ua, 
	cs
}

public enum Grade { 
	First = 1, 
	Second = 2, 
	Third = 3,
	Fourth = 4,
	Fifth = 5,
	Sixth = 6,
	Seventh = 7,
	Eighth = 8,
	Ninth = 9
}

public enum Topic { 
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
 
public enum ExerciseType { 
	WordProblem,
	Numerical,
	ADS,
	Geometric 
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ResultType {
	Int,
	String,
	Double,
	Fraction,
	Decimal,
	Select, // from options: a, b, c, -> always one answer from given options is correct
	MultiSelect
}

public enum SetRange { 
	Set, 
	Range
}

public enum DataType {
	Int, 
	Double,
	Operator
}