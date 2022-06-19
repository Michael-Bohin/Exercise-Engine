namespace ExerciseEngine;

abstract class Exercise {
	public ulong Id { get; }
	public Babylon Name { get; } = new();


	abstract public string GetAssignment(Language lang);
	abstract public List<string> GetQuestions(Language lang);
	abstract public List<string> GetAnswers();
	public 
}

abstract class WordProblem : Exercise {

}