namespace ExerciseEngine;

// Exercise collection:
public enum Language { en, pl, ua, cs }

// Groups:
public enum Classes { First = 1, Second = 2, Third = 3, Fourth, Fifth, Sixth, Seventh, Eight, Ninth }
public enum Topic { Arithmetic, Addition, Subtraction, Multiplication, Division, Modulo, Fractions, Percentages, Combinatorics, BasicUnits } // and many more..
public enum ExerciseType { WordProblem, Numerical, Geometric }

// Json polymorphic serialization handlers:
public enum VariableDiscriminator { Invariant, Cultural }
public enum TextElementDiscriminator { Macro, Text }