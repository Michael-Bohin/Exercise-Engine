using ExerciseEngine;
using ExerciseEngine.ExerciseCompiler;
using ExercisePreprocessor.DefinitionBuilder.MethodLessDefinition;

sealed class Variant_000_001_S01P01_First_Exercise : MethodLessDefinitionBuilder {
	protected override SimpleMetadata BuildSimpleMetadata() {
		SimpleMetadata metadata = new() {
			Titles = new Dictionary<Language, string> {
			{ Language.en, "Unit transfers" }
		},

			ExerciseType = ExerciseType.Numerical,
			Grades = new() { Grade.Fourth, Grade.Fifth },
			Topics = new List<Topic>() { Topic.Arithmetic, Topic.BasicUnits }
		};

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
