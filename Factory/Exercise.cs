namespace ExerciseEngine.Factory;

class MetaData {
	public ulong UniqueId { get; }
	public string Name { get; } 
	public Language Lang { get; }
	public int VariationId { get; }
	public Groups Groups { get; }

	public MetaData(ulong UniqueId, string Name, Language Lang, int VariationId, Groups Groups) {
		this.UniqueId = UniqueId; this.Name = Name;	this.Lang = Lang; this.VariationId = VariationId; this.Groups = Groups;
	}
}

abstract class Exercise {
	// meta data of exercise:
	public ulong UniqueId { get; private set; }
	public string Name { get; private set; }
	public Language Lang { get; private set; }
	public int VariationId { get; private set; }
	public List<Classes> Classes { get; }
	public List<Topic> Topics { get; }
	public ExerciseType ExerciseType { get; private set; }

	// representation of exercise:
	public string Assignment { get; private set;}
	public List<string> SolutionSteps { get; }
	// List<Picture> pictures;

	public Exercise() { // json seriliazer ctor
		Name = default!; Classes = new(); Topics = new(); Assignment = default!; SolutionSteps = new();
	}

	protected Exercise(MetaData md, string Assignment, List<string> SolutionSteps) {
		UniqueId = md.UniqueId; Name = md.Name;
		Lang = md.Lang; VariationId = md.VariationId;
		Classes = md.Groups.Classes;
		Topics = md.Groups.Topics;
		ExerciseType = md.Groups.ExerciseType;
		this.Assignment = Assignment;
		this.SolutionSteps = SolutionSteps;
	}

	public void SerializerSetUniqueId(ulong ui) { UniqueId = ui; }
	public void SerializerSetName(string n) { Name = n; }
	public void SerializerSetLang(Language l) { Lang = l; }
	public void SerializerSetVariationId(int vi) { VariationId = vi; }
	public void SerializerSetExerciseType(ExerciseType et) { ExerciseType = et; }

	public void SerializerSetAssignment(string s) => Assignment = s;
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