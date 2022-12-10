using ExerciseEngine;
using ExerciseEngine.ExerciseCompiler;


namespace ExercisePreprocessor;

abstract class MethodLessDefinitionBuilder {
	public IMethodLessDefinition BuildMethodLessDefinition() {
		List<Variable> variables = BuildVariables();
		SimpleMetadata simpleMetadata = BuildSimpleMetadata();

		MetaData metadata = new() {
			titles = simpleMetadata.Titles,
			exerciseType = simpleMetadata.ExerciseType,
			grades = simpleMetadata.Grades,
			descriptions = simpleMetadata.Descriptions,
			topics = simpleMetadata.Topics,

			exerciseId = GenerateUniqueId(),
			localizations = simpleMetadata.Titles.Keys.ToList()
		};

		return new MethodLessDefinition(variables, metadata);
	}

	protected abstract List<Variable> BuildVariables();
	protected abstract SimpleMetadata BuildSimpleMetadata();

	/// <summary>
	/// TODO: Design deterministic exerciseId generation.
	/// </summary>
	/// <returns></returns>
	private static int GenerateUniqueId () {
		return BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
	}
}
