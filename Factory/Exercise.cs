namespace ExerciseEngine.Factory;

class MetaData {
	public ulong UniqueId { get; private set;}
	public string Name { get; private set; } 
	public Language Lang { get; private set; }
	public int VariationId { get; private set; }
	public Groups Groups { get; }

	public MetaData() { // json serilalizer ctor
		Name = default!;
		Groups = new();
	}

	public void SerializerSetUniqueId(ulong ui) { UniqueId = ui;}
	public void SerializerSetName(string n) { Name = n;}
	public void SerializerSetLang(Language l) { Lang = l;}
	public void SerializerSetVariationId(int vi) { VariationId = vi; }
	// public void SerializerSetGroups() { }

	public MetaData(ulong UniqueId, string Name, Language Lang, int VariationId, Groups Groups) {
		this.UniqueId = UniqueId; this.Name = Name;	this.Lang = Lang; this.VariationId = VariationId; this.Groups = Groups;
	}
}

abstract class Exercise {
	public MetaData MetaData { get; }
	public string Assignment { get; private set;}
	public List<string> SolutionSteps { get; }
	// List<Picture> pictures;

	public Exercise() { // json seriliazer ctor
		MetaData = new(); Assignment = default!; SolutionSteps = new();
	}

	protected Exercise(MetaData MetaData, string Assignment, List<string> SolutionSteps) {
		this.MetaData = MetaData;
		this.Assignment = Assignment;
		this.SolutionSteps = SolutionSteps;	
	}

	public void SerializerSetAssignment(string s) => Assignment = s;
	// public void SerializerSet
}

class WordProblem : Exercise {
	public List<string> Questions { get; }
	public List<string> Results { get; }

	public WordProblem() { // json seriliazer ctor
		Questions = new();
		Results = new();
	}  

	public WordProblem(MetaData MetaData, string Assignment, List<string> Questions, List<string> Results, List<string> SolutionSteps)
		: base(MetaData, Assignment, SolutionSteps) {
		this.Questions = Questions;
		this.Results = Results;
	}
}

class NumericalExercise : Exercise {
	public string Result { get; private set; }

	public NumericalExercise() {// json seriliazer ctor
		Result = default!;
	}

	public NumericalExercise(MetaData MetaData, string Assignment, string Result, List<string> SolutionSteps) 
		: base(MetaData, Assignment, SolutionSteps) {
		this.Result = Result;
	}

	public void SerializerSetResult(string s) => Result = s;
}

class GeometricExercise : Exercise {	
	public GeometricExercise(
		MetaData MetaData, string Assignment, List<string> SolutionSteps) 
		:base(MetaData, Assignment, SolutionSteps) {
		throw new NotImplementedException("No attention = no geometric exercises.");
	}	
}