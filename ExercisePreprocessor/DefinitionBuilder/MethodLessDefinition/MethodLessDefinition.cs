using ExerciseEngine;
using ExerciseEngine.ExerciseCompiler;

namespace ExercisePreprocessor;

class MethodLessDefinition : IMethodLessDefinition {
	public MethodLessDefinition(List<Variable> variables, MetaData metadata) { 
		Variables = variables;
		MetaData = metadata;
	}

	public List<Variable> Variables {  get; }
	public MetaData MetaData { get; }
}

