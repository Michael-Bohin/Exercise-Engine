﻿namespace ExerciseEngine.Factory;

class ExerciseRepresentation {
	public string Assignment { get; } = default!;
	public List<string> Questions { get; }
	public List<string> Results { get; }
	public List<string> SolutionSteps { get; }

	public ExerciseRepresentation(string Assignment, List<string> Questions, List<string> Results, List<string> SolutionSteps) {
		this.Assignment = Assignment; this.Questions = Questions; this.Results = Results; this.SolutionSteps = SolutionSteps;
	}
}

class ExerciseLocalization {
	public Language Lang { get; }
	public string Name { get; } 
	public MacroText Assignment { get; }
	public List<MacroText> Questions { get; }
	public List<MacroText> Results { get; } 
	public List<MacroText> SolutionSteps { get; } 
	public Groups Groups { get; }

	static readonly string endl = Environment.NewLine;
	static readonly string endl2x = endl + endl;

	public ExerciseLocalization(Language Lang, string Name, MacroText Assignment, List<MacroText> Questions, List<MacroText> Results, List<MacroText> SolutionSteps, Groups Groups) {
		this.Lang = Lang; this.Name = Name;	this.Assignment = Assignment; this.Questions=Questions; this.Results=Results; this.SolutionSteps=SolutionSteps;	this.Groups=Groups;
	}

	public ExerciseRepresentation ConstructVariation(Variation v) {
		string _assignment = Assignment.ConstructText(Lang, v);
		var _questions = Construct2DText(Questions, Lang, v);
		var _results = Construct2DText(Results, Lang, v);
		var _solutionSteps = Construct2DText(SolutionSteps, Lang, v);
		return new(_assignment, _questions, _results, _solutionSteps);
	}

	private static List<string> Construct2DText(List<MacroText> template, Language lang, Variation v) {
		List<string> result = new();
		foreach (var step in template)
			result.Add(step.ConstructText(lang, v));
		return result;
	}

	override public string ToString() {
		StringBuilder sb = new();
		sb.Append($"Name              -> {Name}{endl}");
		sb.Append($"Assignment        -> {Assignment}{endl}");
		StringifyList(sb, Questions, "Questions", 17);
		StringifyList(sb, Results, "Results", 17);
		StringifyList(sb, SolutionSteps, "SolutionSteps", 17);
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

class ExerciseCollection {
	public ulong UniqueId { get; }
	public List<Variation> Variants { get; } = new();
	public Dictionary<Language, ExerciseLocalization> Localizations { get; } = new();
	
	readonly Random rand = new();
	static readonly string endl = Environment.NewLine;
	static readonly string endl2x = endl + endl;

	public ExerciseCollection(  ulong UniqueId, List<Variation> Variants, Dictionary<Language, ExerciseLocalization> Localizations) {
		this.UniqueId = UniqueId; this.Variants = Variants;	this.Localizations = Localizations;
	}
	
	public Exercise GetRandomExercise(Language lang) {
		int pick = rand.Next(0,Variants.Count); 
		return GetExercise(lang, pick);
	}

	public Exercise GetExercise(Language lang, int index) {
		Variation v = Variants[index];
		// gather metaData and exercise representation:
		MetaData metaData = new(UniqueId, Localizations[lang].Name, lang, index, Localizations[lang].Groups);
		ExerciseRepresentation exRepr = Localizations[lang].ConstructVariation(v);
		return CreateExerciseInstance(metaData, exRepr);
	}

	// this implementation will likely be doing a lot of instructions repetitively
	// likely may be sped up by identifying those parts and doing them just once
	public List<Exercise> GetLocalizedCollection(Language lang) {
		List<Exercise> collection = new();
		for(int i= 0; i < Variants.Count;i++)
			collection.Add(GetExercise(lang, i));
		return collection;
	}

	private static Exercise CreateExerciseInstance(MetaData metaData, ExerciseRepresentation er) {
		ExerciseType et = metaData.Groups.ExerciseType;
		if (et == ExerciseType.WordProblem) {
			WordProblem wp = new(metaData, er.Assignment, er.Questions, er.Results, er.SolutionSteps);
			return wp;
		}

		NumericalExercise ne = new(metaData, er.Assignment, er.Results[0], er.SolutionSteps);
		return ne;
	}

	override public string ToString() {
		
		StringBuilder sb = new();
		sb.Append($"Unique id: {UniqueId}{endl2x}");
		sb.Append($"Variations:{endl}");
		foreach(var v in Variants)
			sb.Append(v.ToString());
		sb.Append($"{endl2x}Localizations:{endl}");
		foreach(var localization in Localizations) {
			sb.Append($"{endl}   >>> Excercise localization: {localization.Key} <<< {endl2x}");
			sb.Append(localization.Value.ToString());
		}
		sb.Append($"{endl2x}");
		return sb.ToString();
	}
}