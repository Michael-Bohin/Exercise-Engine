namespace ExerciseEngine;

class Representation {
	public ExerciseType EType { get; }
	public ResultType RType { get; }
	public string Assignment { get; }
	public List<string> Questions { get; }
	public List<string> Results { get; }
	public List<string> SolutionSteps { get; }

	public Representation(string Assignment, List<string> Questions, List<string> Results, List<string> SolutionSteps) {
		this.Assignment = Assignment; this.Questions = Questions; this.Results = Results; this.SolutionSteps = SolutionSteps;
	}
}