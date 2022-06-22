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
	public (ulong exerciseId, Language language, int variationId) Id { get; private set; }
	public string Name { get; private set; }
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
		Id = (md.UniqueId, md.Lang, md.VariationId);
		Name = md.Name;
		Classes = md.Groups.Classes;
		Topics = md.Groups.Topics;
		ExerciseType = md.Groups.ExerciseType;
		this.Assignment = Assignment;
		this.SolutionSteps = SolutionSteps;
	}

	public void SerializerSetId(ulong exId, Language l, int varId) => Id = (exId, l, varId);
	public void SerializerSetName(string n) { Name = n; }
	public void SerializerSetExerciseType(ExerciseType et) { ExerciseType = et; }

	public void SerializerSetAssignment(string s) => Assignment = s;

	override public string ToString() {
		StringBuilder sb = new();
		AddDiscriminator(sb);
		sb.Append("\n    >>> Meta data of exercise <<<\n");
		sb.Append($"Name: {Name}\n");
		sb.Append($"Id: {Id.exerciseId}, Language: {Id.language}, Variation id: {Id.variationId}\n");
		sb.Append($"Classes: ");
		foreach(var c in Classes)
			sb.Append($"{c} ");
		sb.Append('\n');
		sb.Append($"Topics: ");
		foreach (var t in Topics)
			sb.Append($"{t} ");
		sb.Append('\n');
		sb.Append($"ExerciseType: {ExerciseType}\n");

		sb.Append("\n    >>> Representation of exercise <<<\n");
		sb.Append($"Assignment: {Assignment}\n");
		AppendListString("Solution steps", SolutionSteps, sb);

		AddPolymorphicProperties(sb);
		return sb.ToString();
	}

	abstract protected void AddDiscriminator(StringBuilder sb);
	abstract protected void AddPolymorphicProperties(StringBuilder sb);

	protected void AppendListString(string propertyName, List<string> list, StringBuilder sb) {
		sb.Append(propertyName + ":\n");
		foreach (var item in list)
			sb.Append(item + '\n');
		sb.Append('\n');
	}
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

	override protected void AddDiscriminator(StringBuilder sb) => sb.Append("Type discriminator: 1\n");
	override protected void AddPolymorphicProperties(StringBuilder sb) {
		AppendListString("Questions", Questions, sb);
		AppendListString("Results", Results, sb);
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

	override protected void AddDiscriminator(StringBuilder sb) => sb.Append("Type discriminator: 2\n");
	override protected void AddPolymorphicProperties(StringBuilder sb) => sb.Append($"Result: {Result}\n");
}

class GeometricExercise : Exercise {	
	public GeometricExercise(
		MetaData MetaData, string Assignment, List<string> SolutionSteps) 
		:base(MetaData, Assignment, SolutionSteps) {
		throw new NotImplementedException("No attention = no geometric exercises.");
	}

	protected override void AddDiscriminator(StringBuilder sb) {
		throw new NotImplementedException();
	}

	protected override void AddPolymorphicProperties(StringBuilder sb) {
		throw new NotImplementedException();
	}
}