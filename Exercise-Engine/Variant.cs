using ExerciseEngine.MacroExercise;

namespace ExerciseEngine;

public abstract class Variant {
	public abstract bool IsLegit(out int constraintId);

	public abstract string GetValueOfVariable(string macroPointer);

	public abstract string GetResult(int questionIndex);

	public abstract VariantRecord ToVariantRecord();

	static protected char OpToChar(Operator op) {
		switch (op) {
			case Operator.Add:
				return '+';
			case Operator.Sub:
				return '-';
			case Operator.Mul:
				return '*';
			case Operator.Div:
				return '/';
			default:
				throw new ArgumentException("OperatorToChar");
		}
	}
}