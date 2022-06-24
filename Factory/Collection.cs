namespace ExerciseEngine.Factory;

class ExerciseCollection {
	public int uniqueId;
	public List<Variant> variants;
	public Dictionary<Language, ExerciseLocalization> localizations;

	readonly Random rand = new();
	static readonly string endl = Environment.NewLine;
	static readonly string endl2x = endl + endl;

	public ExerciseCollection(int uniqueId, List<Variant> variants, Dictionary<Language, ExerciseLocalization> localizations) {
		this.uniqueId = uniqueId; this.variants = variants; this.localizations = localizations;
	}

	public Exercise GetExercise(Language lang, int index) {
		ExerciseMetaData emd = new(localizations[lang].metaData, index);
		ExerciseRepresentation exRepr = localizations[lang].ConstructVariation(variants[index]);
		return CreateExerciseInstance(emd, exRepr);
	}

	public Exercise GetRandomExercise(Language lang) {
		int pick = rand.Next(0, variants.Count);
		return GetExercise(lang, pick);
	}

	public List<Exercise> GetLocalizedCollection(Language lang) {
		List<Exercise> collection = new();
		for (int i = 0; i < variants.Count; i++)
			collection.Add(GetExercise(lang, i));
		return collection;
	}

	private static Exercise CreateExerciseInstance(ExerciseMetaData md, ExerciseRepresentation er) {
		ExerciseType type = md.type;
		if (type == ExerciseType.WordProblem) {
			WordProblem wp = new(md, er.assignment, er.questions, er.results, er.solutionSteps);
			return wp;
		}

		NumericalExercise ne = new(md, er.assignment, er.results[0], er.solutionSteps);
		return ne;
	}

	override public string ToString() {
		StringBuilder sb = new();
		sb.Append($"Unique id: {uniqueId}{endl2x}");
		sb.Append($"Variations:{endl}");
		foreach (var v in variants)
			sb.Append(v.ToString());
		sb.Append($"{endl2x}Localizations:{endl}");
		foreach (var localization in localizations) {
			sb.Append($"{endl}   >>> Excercise localization: {localization.Key} <<< {endl2x}");
			sb.Append(localization.Value.ToString());
		}
		sb.Append($"{endl2x}");
		return sb.ToString();
	}

	public void SerializerSetId(int id) => uniqueId = id;
	public void SerializerSetVariations(List<Variant> vs) => variants = vs;
}

class ExerciseLocalization {
	[JsonPropertyName("meta")]
	public LocalizationMetaData metaData;
	[JsonPropertyName("a")]
	public MacroText assignment;
	[JsonPropertyName("q")]
	public List<MacroText> questions;
	[JsonPropertyName("r")]
	public List<MacroText> results;
	[JsonPropertyName("s")]
	public List<MacroText> solutionSteps;
	
	static readonly string endl = Environment.NewLine;
	static readonly string endl2x = endl + endl;

	public ExerciseLocalization(LocalizationMetaData metaData, MacroText assignment, List<MacroText> questions, List<MacroText> results, List<MacroText> solutionSteps) {
		this.metaData = metaData; this.assignment = assignment; this.questions = questions; this.results = results; this.solutionSteps = solutionSteps;
	}

	public ExerciseLocalization() {
		metaData = new();
		assignment = new();
		questions = new();
		results = new();
		solutionSteps = new();
	}

	public ExerciseRepresentation ConstructVariation(Variant v) {
		Language l = metaData.uniqueId.language;
		string _assignment = assignment.ToString(l, v);
		List<string> _questions = Construct2DText(questions, l, v);
		List<string> _results = Construct2DText(results, l, v);
		List<string> _solutionSteps = Construct2DText(solutionSteps, l, v);
		return new(_assignment, _questions, _results, _solutionSteps);
	}

	private static List<string> Construct2DText(List<MacroText> template, Language l, Variant v) {
		List<string> result = new();
		foreach (var step in template)
			result.Add(step.ToString(l, v));
		return result;
	}

	// more readable version for humans relative to json:
	override public string ToString() { 
		StringBuilder sb = new();
		sb.Append($"Name              -> {metaData.name}{endl}");
		sb.Append($"Assignment        -> {assignment}{endl}");
		StringifyList(sb, questions, "Questions", 17);
		StringifyList(sb, results, "Results", 17);
		StringifyList(sb, solutionSteps, "SolutionSteps", 17);
		return sb.ToString();
	}

	static void StringifyList(StringBuilder sb, List<MacroText> list, string name, int colAllign) {
		sb.Append(name);
		colAllign -= name.Length;
		for(int i = colAllign; i > -1; i--) 
			sb.Append(' ');
		sb.Append("-> ");
		foreach(var item in list)
			sb.Append(item.ToString() + ' ');
		sb.Append(endl);
	}
}

