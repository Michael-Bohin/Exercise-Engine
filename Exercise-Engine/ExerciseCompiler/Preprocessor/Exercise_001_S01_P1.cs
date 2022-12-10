namespace ExerciseEngine.ExerciseCompiler.Preprocessor;

sealed class MLD_001 : MethodLessDefinitionBuilder {
	protected override SimpleMetadata BuildSimpleMetadata() {
		SimpleMetadata metadata = new(1, ExerciseType.Numerical, "S01 P01 Převod jednotek plochy a objemu - první příklad", Language.cs);

		return metadata;
	}

	protected override List<Variable> BuildVariables() {
		List<Variable> variables = new() {
			new IntRange("A", 1, 9),
			new IntRange("B", 11, 99),
		};

		return variables;
	}
}
