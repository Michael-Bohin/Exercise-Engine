namespace Exercise_Engine;

class Constraint {
	public List<string> code; // constraint defined in C#
	public bool codeDefined; // the human that created the exercise signals, whether he c++ code is finnished, or needs programming finalization
	public List<string> comments; // human language explanation of the constraint

	public Constraint(List<string> code, bool codeDefined, List<string> comments) {
		this.code = code;
		this.comments = comments;
		this.codeDefined = codeDefined;
	}
}

