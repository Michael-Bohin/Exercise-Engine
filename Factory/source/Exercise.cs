namespace ExerciseEngine.source;

abstract class Exercise
{
    public ExerciseMetaData metaData;
    public string assignment;
    public List<string> solutionSteps;
    // List<Picture> pictures;

    protected static readonly string endl = Environment.NewLine;
    protected static readonly string endl2x = endl + endl;

    public Exercise()
    { // json seriliazer ctor
        metaData = new(); assignment = default!; solutionSteps = new();
    }

    protected Exercise(ExerciseMetaData metaData, string assignment, List<string> solutionSteps)
    {
        this.metaData = metaData;
        this.assignment = assignment;
        this.solutionSteps = solutionSteps;
    }

    public void SerializerSetId(int exId, Language l, int varId) => metaData.id = new(exId, l, varId);
    public void SerializerSetName(string n) { metaData.name = n; }
    public void SerializerSetExerciseType(ExerciseType et) { metaData.type = et; }
    public void SerializerSetAssignment(string s) => assignment = s;
    public void SerializerAddSolutionStep(string ss) => solutionSteps.Add(ss);

    override public string ToString()
    {
        StringBuilder sb = new();
        AppendDiscriminator(sb);
        sb.Append($"{endl}    >>> Meta data of exercise <<<{endl}");
        sb.Append($"Name: {metaData.name}{endl}");
        sb.Append($"Id: {metaData.id.id}, Language: {metaData.id.language}, Variation id: {metaData.id.variant}{endl}");
        sb.Append($"Classes: ");
        foreach (var c in metaData.classes)
            sb.Append($"{c} ");
        sb.Append(endl);
        sb.Append($"Topics: ");
        foreach (var t in metaData.topics)
            sb.Append($"{t} ");
        sb.Append(endl);
        sb.Append($"ExerciseType: {metaData.type}{endl}");

        sb.Append($"{endl}    >>> Representation of exercise <<<{endl}");
        sb.Append($"Assignment: {assignment}{endl}");
        AppendListString("Solution steps", solutionSteps, sb);

        AppendPolymorphicProperties(sb);
        return sb.ToString();
    }

    abstract protected void AppendDiscriminator(StringBuilder sb);
    abstract protected void AppendPolymorphicProperties(StringBuilder sb);

    protected void AppendListString(string propertyName, List<string> list, StringBuilder sb)
    {
        sb.Append(propertyName + ":\n");
        foreach (var item in list)
            sb.Append(item + '\n');
        sb.Append('\n');
    }
}

class WordProblem : Exercise
{
    public List<string> questions;
    public List<string> results;

    public WordProblem()
    { // json seriliazer ctor
        questions = new();
        results = new();
    }

    public WordProblem(ExerciseMetaData metaData, string assignment, List<string> questions, List<string> results, List<string> solutionSteps) : base(metaData, assignment, solutionSteps)
    {
        this.questions = questions;
        this.results = results;
    }

    override protected void AppendDiscriminator(StringBuilder sb) => sb.Append("Type discriminator: 1\n");
    override protected void AppendPolymorphicProperties(StringBuilder sb)
    {
        AppendListString("Questions", questions, sb);
        AppendListString("Results", results, sb);
    }
}

class NumericalExercise : Exercise
{
    public string result;

    public NumericalExercise()
    { // json seriliazer ctor
        result = default!;
    }

    public NumericalExercise(ExerciseMetaData metaData, string assignment, string result, List<string> solutionSteps) : base(metaData, assignment, solutionSteps)
    {
        this.result = result;
    }

    public void SerializerSetResult(string s) => result = s;

    override protected void AppendDiscriminator(StringBuilder sb) => sb.Append($"Type discriminator: 2{endl}");
    override protected void AppendPolymorphicProperties(StringBuilder sb) => sb.Append($"Result: {result}{endl}");
}

// reminder for future, geometric exercise also need to be developed..
class GeometricExercise : Exercise
{
    public GeometricExercise(ExerciseMetaData metaData, string Assignment, List<string> SolutionSteps) : base(metaData, Assignment, SolutionSteps)
    {
        throw new NotImplementedException("No attention = no geometric exercises.");
    }

    protected override void AppendDiscriminator(StringBuilder sb) => throw new NotImplementedException();
    protected override void AppendPolymorphicProperties(StringBuilder sb) => throw new NotImplementedException();
}