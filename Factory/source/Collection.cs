using static System.Environment;
namespace ExerciseEngine;

interface IExerciseCollection1D {
    Exercise GetExercise(int index);
	Exercise GetRandomExercise();
	List<Exercise> GetAllVariants();
}

interface IExerciseCollection2D
{
    Exercise GetExercise(Language lang, int index);
    Exercise GetRandomExercise(Language lang);
    List<Exercise> GetLocalizedExercises(Language lang);
}

interface IExerciseLocalization
{
    ExerciseRepresentation ConstructVariant(Variant v);
}

abstract class ExerciseCollection {
	protected static Exercise CreateExerciseInstance(ExerciseMetaData md, ExerciseRepresentation er) {
		ExerciseType type = md.type;
		if (type == ExerciseType.WordProblem) {
			WordProblem wp = new(md, er.assignment, er.questions, er.results, er.solutionSteps);
			return wp;
		}

		NumericalExercise ne = new(md, er.assignment, er.results[0], er.solutionSteps);
		return ne;
	}
}

class ExerciseCollection1D : ExerciseCollection, IExerciseCollection1D {
	public int uniqueId;
	public List<Variant> variants;
	public ExerciseLocalization originalLanguage;

	readonly Random rand = new();
	static readonly string endl = NewLine;
	static readonly string endl2x = endl + endl;

	public ExerciseCollection1D(int uniqueId, List<Variant> variants, ExerciseLocalization originalLanguage) {
		this.uniqueId = uniqueId; this.variants = variants; this.originalLanguage = originalLanguage;
	}

	public Exercise GetExercise(int index) {
		ExerciseMetaData emd = new(originalLanguage.metaData, index);
		ExerciseRepresentation exRepr = originalLanguage.ConstructVariant(variants[index]);
		return CreateExerciseInstance(emd, exRepr);
	}

	public Exercise GetRandomExercise() {
		int pick = rand.Next(0, variants.Count);
		return GetExercise(pick);
	}

	public List<Exercise> GetAllVariants() {
		List<Exercise> collection = new();
		for (int i = 0; i < variants.Count; i++)
			collection.Add(GetExercise(i));
		return collection;
	}

	override public string ToString() {
		StringBuilder sb = new();
		sb.Append($"Unique id: {uniqueId}{endl2x}");
		sb.Append($"Variations:{endl}");
		foreach (var v in variants)
			sb.Append(v.ToString());
		sb.Append($"{endl2x}Original language:{endl}");
		sb.Append($"{endl}   >>> Excercise localization: {originalLanguage.metaData.id.language} <<< {endl2x}");
		sb.Append(originalLanguage.ToString());
		sb.Append($"{endl2x}");
		return sb.ToString();
	}
}

class ExerciseCollection2D : ExerciseCollection, IExerciseCollection2D {
    public int uniqueId;
    public List<Variant> variants;
    public Dictionary<Language, ExerciseLocalization> localizations;

    readonly Random rand = new();
    static readonly string endl = NewLine;
    static readonly string endl2x = endl + endl;

    public ExerciseCollection2D(int uniqueId, List<Variant> variants, Dictionary<Language, ExerciseLocalization> localizations)
    {
        this.uniqueId = uniqueId; this.variants = variants; this.localizations = localizations;
    }

    public Exercise GetExercise(Language lang, int index)
    {
        ExerciseMetaData emd = new(localizations[lang].metaData, index);
        ExerciseRepresentation exRepr = localizations[lang].ConstructVariant(variants[index]);
        return CreateExerciseInstance(emd, exRepr);
    }

    public Exercise GetRandomExercise(Language lang)
    {
        int pick = rand.Next(0, variants.Count);
        return GetExercise(lang, pick);
    }

    public List<Exercise> GetLocalizedExercises(Language lang)
    {
        List<Exercise> collection = new();
        for (int i = 0; i < variants.Count; i++)
            collection.Add(GetExercise(lang, i));
        return collection;
    }

    override public string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"Unique id: {uniqueId}{endl2x}");
        sb.Append($"Variations:{endl}");
        foreach (var v in variants)
            sb.Append(v.ToString());
        sb.Append($"{endl2x}Localizations:{endl}");
        foreach (var localization in localizations)
        {
            sb.Append($"{endl}   >>> Excercise localization: {localization.Key} <<< {endl2x}");
            sb.Append(localization.Value.ToString());
        }
        sb.Append($"{endl2x}");
        return sb.ToString();
    }

    /// ?? Is this still nesecary with the new inbuilt json serializer option??
    public void SerializerSetId(int id) => uniqueId = id;
    public void SerializerSetVariations(List<Variant> vs) => variants = vs;
}

class ExerciseLocalization : IExerciseLocalization
{
    [JsonPropertyName("meta")]
    public LocalizationMetaData metaData;
    [JsonPropertyName("ass")]
    public MacroText assignment;
    [JsonPropertyName("qs")]
    public List<MacroText> questions;
    [JsonPropertyName("res")]
    public List<MacroText> results;
    [JsonPropertyName("ss")]
    public List<MacroText> solutionSteps;

    static readonly string endl = NewLine;
    static readonly string endl2x = endl + endl;

    public ExerciseLocalization(LocalizationMetaData metaData, MacroText assignment, List<MacroText> questions, List<MacroText> results, List<MacroText> solutionSteps)
    {
        this.metaData = metaData; this.assignment = assignment; this.questions = questions; this.results = results; this.solutionSteps = solutionSteps;
    }

    public ExerciseLocalization()
    {
        metaData = new();
        assignment = new();
        questions = new();
        results = new();
        solutionSteps = new();
    }

    public ExerciseRepresentation ConstructVariant(Variant v)
    {
        Language l = metaData.id.language;
        string _assignment = assignment.ToString(l, v);
        List<string> _questions = Construct2DText(questions, l, v);
        List<string> _results = Construct2DText(results, l, v);
        List<string> _solutionSteps = Construct2DText(solutionSteps, l, v);
        return new(_assignment, _questions, _results, _solutionSteps);
    }

    private static List<string> Construct2DText(List<MacroText> template, Language l, Variant v)
    {
        List<string> result = new();
        foreach (var step in template)
            result.Add(step.ToString(l, v));
        return result;
    }

    // more readable version for humans relative to json:
    override public string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"Name              -> {metaData.name}{endl}");
        sb.Append($"Assignment        -> {assignment}{endl}");
        StringifyList(sb, questions, "Questions", 17);
        StringifyList(sb, results, "Results", 17);
        StringifyList(sb, solutionSteps, "SolutionSteps", 17);
        return sb.ToString();
    }

    static void StringifyList(StringBuilder sb, List<MacroText> list, string name, int colAllign)
    {
        sb.Append(name);
        colAllign -= name.Length;
        for (int i = colAllign; i > -1; i--)
            sb.Append(' ');
        sb.Append("-> ");
        foreach (var item in list)
            sb.Append(item.ToString() + ' ');
        sb.Append(endl);
    }
}

