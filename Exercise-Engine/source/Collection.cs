global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

namespace ExerciseEngine;
using static System.Environment;

// ! have variants loaded in RAM ! at http reachable service :) 
// that service than constructs those serialized random variants calling them from RAM, not JSON
// If there are going to be too many variants, it can periodically swap those loaded in memory in such count, that it is OK 
// -> this evades the nessecity to recalculate variants over and over agian (for each call from 1 person) 
// additionaly the server can call System.Random AOT, having the sequence ready before the request arrives at the server
// upon recieving the request it simply dequeues next sequence members and after it serves the request with response 
// it refills the sequence
/*
// 1 exercise, 1 language, X variants
interface IExerciseCollection1D {
    Exercise GetExercise(int index); // concrete exercise
	Exercise GetExercise(); // random exercise
	List<Exercise> GetAllVariants(); // all variants in order they sit in internal sequence
    List<Exercise> GetSomeVariants(int count); // randomly chosen
}*/


// 1 exercise, 1 language, 1 variant
// return types to be thought through later
interface IExerciseRepresentation {
	string GetType();
	string GetAssignment();
    List<string> GetQuestions();
    List<string> GetResults();
    List<string> GetSolutionSteps();
}

// 1 exercise, X languages, Y variants
interface IExercise
{
	ExerciseRepresentation GetExercise(Language lang, int index);
	ExerciseRepresentation GetExercise(Language lang); // gets random variant in given language
    List<ExerciseRepresentation> GetAllVariants(Language lang);
    List<ExerciseRepresentation> GetSomeVariants(Language lang, int count);


}

// X exercises, Y languages, Z variants
interface IExerciseCollection {
	Exercise GetExercise(int unigueId);
}

interface IExerciseEngineAPI { 
    string GetAllTranslations(Language lang);
    string GetVariants(int exerciseUniqueId, int count); 
    string GetTopics();
}
// this interface serves the http request from web server 
// Throws erros if:
// 1. Language does not exist 
// 2. Exercise unique id does not exist (negative number or greater than greatest id)
// 3. Count is smaller than one (must return at least one exercise)
// 4. Count is greater than count of all variants for given exercise (all exercise will have different number of variants)
// 5. Count is greater than maximum number of exercise held in memory at one time. Say 1000 for begining. 
// In all cases above the http request returns with code 5xx. 
// 
// Always returns serialized classes to JSON, because the webserver will run in node.js language 
//
// Always returns random sequence of variants such that all variants occur at most once.
// That is variants do not repeat. 

class ExerciseRepresentation {

}

class Translation {

}

class Variant {

}

class Exercise {
    private Dictionary<Language, Translation> translations;
    private List<Variant> variants;
    private MetaData metaData;
}

interface IExerciseLocalization
{
    ExerciseRepresentation ConstructVariant(Variant v);
}


// why? 
abstract public class ExerciseCollection {
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

public class ExerciseCollection1D : ExerciseCollection, IExerciseCollection1D {
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

public class ExerciseCollection2D : ExerciseCollection, IExerciseCollection2D {
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

public class ExerciseLocalization : IExerciseLocalization
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

