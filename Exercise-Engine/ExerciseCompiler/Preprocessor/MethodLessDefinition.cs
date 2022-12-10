namespace ExerciseEngine.ExerciseCompiler.Preprocessor;

class SimpleMetadata {
	public string Title { get; set; } = ""; 
	public ExerciseType ExerciseType { get; set; } 
	public Language Language { get; set; }
	public int ExerciseId { get; set; }	

	public SimpleMetadata(int exerciseId, ExerciseType exerciseType, string title, Language language) { }
}

class MethodLessDefinition {
	public MethodLessDefinition(List<Variable> variables, MetaData metadata) { 
		Variables = variables;
		MetaData = metadata;
	}

	public List<Variable> Variables {  get; }
	public MetaData MetaData { get; }
}

abstract class MethodLessDefinitionBuilder {
	public MethodLessDefinition BuildMethodLessDefinition() {
		List<Variable> variables = BuildVariables();
		SimpleMetadata simpleMetadata = BuildSimpleMetadata();

		Language lang = simpleMetadata.Language;
		ExerciseType type = simpleMetadata.ExerciseType;
		int exerciseId = 20; // figure out system to assign ids later.
		MetaData metadata = new(exerciseId, lang, type);
		metadata.titles[lang] = simpleMetadata.Title;

		return new MethodLessDefinition(variables, metadata);
	}

	protected abstract List<Variable> BuildVariables();
	protected abstract SimpleMetadata BuildSimpleMetadata();
}
