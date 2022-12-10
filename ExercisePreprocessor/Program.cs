using ExerciseEngine.ExerciseCompiler;

namespace ExercisePreprocessor;


List<DefinitionWithBuilder> methodLessDefinitions = BuildAllMethodLessDefinitions();


List<DefinitionWithBuilder> BuildAllMethodLessDefinitions() {
	MethodLessDefinitionBuilder[] methodLessDefinitionBuilders = {
		new Variant_000_001_S01P01_First_Exercise()
	};

	List<DefinitionWithBuilder> defsWithBuilders = new();

	foreach (var definitionBuilder in methodLessDefinitionBuilders) {
		DefinitionWithBuilder defWithBuilder = new(
			definitionBuilder, 
			definitionBuilder.BuildMethodLessDefinition()
		);

		defsWithBuilders.Add(defWithBuilder);
	}

	return defsWithBuilders;
}

void GenerateExerciseSkeletons(List<DefinitionWithBuilder> defsWithBuilders) {
	string outDir = $"./out";

	if (!Directory.Exists(outDir)) {
		Directory.CreateDirectory(outDir);
	}

	foreach (var defWithBuilder in defsWithBuilders) {
		string filename = nameof(defWithBuilder.Builder);
		string skeleton = BuildExerciseSkeleton(defWithBuilder.Definition);

		using StreamWriter sw = new($"{outDir}/Definition{filename}.cs");
		sw.Write(skeleton);
	}
}

string BuildExerciseSkeleton(IMethodLessDefinition definition) {
	
}

class DefinitionWithBuilder {
	public DefinitionWithBuilder(MethodLessDefinitionBuilder builder, IMethodLessDefinition definition) {
		Definition = definition;
		Builder = builder;
	}

	public IMethodLessDefinition Definition { get; }
	public MethodLessDefinitionBuilder Builder { get; }
}