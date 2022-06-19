namespace ExerciseEngine;

enum ParameterType { Int, Operator, String, Double/*, Fraction*/ }

record Parameter {
	public ParameterType Type { get; }
	public string Id { get; } = default!;

	public Parameter(ParameterType Type, string Id) {
		this.Type = Type;
		this.Id = Id;
	}
}

class Constraint {
	public List<string> SharedCode { get; }		// variables shared by more constrainst executed in IsLegit method
	public List<string> Parameters { get; }		// name and type of variables passed into the constraint function
	public List<string> Code { get; }			// constraint defined in C#
	public bool CodeDefined { get; }			// the human that created the exercise signals, whether he c++ code is finnished, or needs programming finalization
	public List<string> Comments { get; }		// human language explanation of the constraint

	public Constraint(List<string> SharedCode, List<string> Parameters, List<string> Code, bool CodeDefined, List<string> Comments) {
		this.SharedCode = SharedCode;
		this.Parameters = Parameters;
		this.Code = Code;
		this.Comments = Comments;
		this.CodeDefined = CodeDefined;
	}
}

