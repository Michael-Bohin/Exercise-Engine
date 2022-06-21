namespace ExerciseEngine.Factory;

// Exercise collection:
enum Language { en, pl, ua, cs }

// Groups:
enum Classes { First = 1, Second = 2, Third = 3, Fourth, Fifth, Sixth, Seventh, Eight, Ninth }
enum Topic { Arithmetic, Addition, Subtraction, Multiplication, Division, Modulo, Fractions, Percentages, Combinatorics, BasicUnits } // and many more..
enum ExerciseType { WordProblem, Numerical, Geometric }

// Json polymorphic serialization handlers:
enum VariableDiscriminator { Invariant, Cultural }