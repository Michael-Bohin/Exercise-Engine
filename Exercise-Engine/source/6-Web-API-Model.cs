namespace ExerciseEngine;
// these classes will be only used inside web api application, for now they are here

internal class Exercise_MetaData {
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

internal class Exercise_Representation {
	public string			assignment = "";
	public List<string>		questions = new();
	public List<string>		results = new();
	public ResultType		resultType;
	public List<string>		solutionSteps = new();
	public List<string>		imagePaths = new();
}
