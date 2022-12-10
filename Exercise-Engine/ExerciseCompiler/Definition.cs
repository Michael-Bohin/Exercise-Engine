using System.Text.Json.Serialization;

namespace ExerciseEngine.ExerciseCompiler;

public class MethodLessDefinition {
    public List<Variable> Variables { get; set; } = new();
    public MetaData MetaData { get; set; } = new();
}

public class Definition : MethodLessDefinition {
	public List<ConstraintMethod> Constraints { get; set; } = new();
	public DefinitionAssignment Assignment { get; set; } = new();
}

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(ConstraintMethod), "constraint_method")]
[JsonDerivedType(typeof(ResultMethod), "result_method")]
abstract public class Method {
	public bool CodeDefined = false;
	public string Code = "";
	public string Comment = "";
}

public class ConstraintMethod : Method { }

public class ResultMethod : Method {
	public ResultType resultType;
}