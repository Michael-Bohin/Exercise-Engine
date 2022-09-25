namespace ExerciseEngine;
// these classes will be only used inside web api application, for now they are here

public class Exercise_MetaData {
	public int							uniqueId;
	public List<Language>				localizations = new();
	public int							variantsCount;
	public ExerciseType					exerciseType;
	public Dictionary<Language, string>	titles = new();
	public Dictionary<Language, string> descriptions = new();
	public List<Topic>					topics = new(); // possibly here dict of topics, or dict at place of webserver
	public List<Grade>					grades = new();
	public string						thumbnailPath = "";
	public int							metaVersion;
}

public class Representation {
	// exercise content:
	public string			assignment = "";
	public List<Question>   questions = new();

	public Representation(string assignment) {
		this.assignment = assignment;
	}
}

public class Question {
	public string		question = ""; // for empty cases, shared assignment is enough.
	public string		result = "";
	public ResultType	resultType;
	public List<string> solutionSteps = new();
	public List<string> imagePaths = new();

	public Question() { }
	public Question(string result, ResultType resultType) {
		this.result = result;
		this.resultType = resultType;
	}
}

