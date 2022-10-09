namespace ExerciseEngine;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// zatim nemichat jazyky, udelat jen pro cestinu!
// protected Dictionary<Language, Description> translations = new();

public abstract class Variant {
	public abstract bool IsLegit(out int constraintId);

	public abstract string VariableRepresentation(string variableName);

	public abstract string GetResult(int questionIndex);

	static protected char OpToChar(Operator op) {
		switch (op) {
			case Operator.Add:
				return '+';
			case Operator.Sub:
				return '-';
			case Operator.Mul:
				return '*';
			case Operator.Div:
				return '/';
			default:
				throw new ArgumentException("OperatorToChar");
		}
	}
}

// Factory class merged into Exercise class
// public abstract class Factory<V> where V : Variant { }

public abstract class Exercise<V> where V : Variant {
	public readonly string exerciseName;
	// Factory method:
	public readonly int expected;
	public int actual;
	public List<int> constraintLog = new(); // stores number of times the constraint with specific id has been triggered
	public List<V> legit = new();
	public List<List<V>> illegal = new();

	// Builder:
	public readonly Dictionary<Language, MacroRepresentation> babylon = new(); // i am just running out of names I can image at this point tbh. once finnished,  think this one through again.
	public readonly bool monoLingual;

	protected Exercise(bool monoLingual, int constraintCount, int expected, string exerciseName, Language initialLanguage) {
		this.monoLingual = monoLingual;
		this.expected = expected;
		actual = 0;
		for (int i = 0; i < constraintCount; i++) {
			constraintLog.Add(0);
			illegal.Add(new());
		}
		this.exerciseName = exerciseName;

		MacroRepresentation mr = new() {
			assignment = BuildAssignment(),
			questions = BuildQuestions()
		};

		babylon[initialLanguage] = mr;
	}

	/// <summary>
	/// Factory Method -> Create n legit variants
	/// </summary>
	public abstract void FilterLegitVariants();

	/// <summary>
	/// Builder methods for Macro representation used in Exercise ctor.
	/// </summary>
	/// 
	protected abstract List<MacroText> BuildAssignment();

	protected abstract List<MacroQuestion> BuildQuestions();



	public string ReportStatistics() {
		StringBuilder sb = new();
		sb.Append($"Statistics of class {exerciseName}\n");
		sb.Append($"Expected number of variants: {expected}\n");
		sb.Append($"Actual number of variants considered: {actual}\n");
		sb.Append($"Number of constraints: {constraintLog.Count}\n\n");
		sb.Append($"Legit variants found: {legit.Count}, {((double)(legit.Count * 100)/(double)expected):f2} %\n\n");
		sb.Append("Constraints disallowed variants:\n");
		if (constraintLog.Count > 0)
			for (int i = 0; i < constraintLog.Count; i++)
				sb.Append($"Constraint {i}: {constraintLog[i]}, {((double)(constraintLog[i] * 100) / (double)expected):f2} %\n");
		return sb.ToString();
	}

	protected void Consider(V cv) {
		actual++;
		if (cv.IsLegit(out int constraintId)) {
			legit.Add(cv);
		} else {
			constraintLog[constraintId]++;
			if (illegal[constraintId].Count < 50)
				illegal[constraintId].Add(cv);
		}
	}

	/// <summary>
	/// Builder: 
	///		1. Set MacroRepresentation of Language l 
	///		2. Build Representation of exercise given Variant v and Language l 
	/// </summary>
	public bool HasLanguage(Language language) => monoLingual || babylon.ContainsKey(language);

	public bool SetLanguage(MacroRepresentation mr, Language lang) {
		if(monoLingual)
			return false;
		babylon[lang] = mr;
		return true;
	}

	// for monoLingual exercises (numeric at this point) -> return english
	public Representation GetRepresentation(V cv) => GetRepresentation(cv, Language.en);

	public Representation GetRepresentation(V cv, Language lang) {
		MacroRepresentation macroR = babylon[lang];
		string assignment = BuildMacroText(macroR.assignment, cv);

		Representation r = new(assignment);
		int counter = 0;
		foreach (MacroQuestion mq in macroR.questions)
			r.questions.Add(BuildQuestion(mq, cv, counter++) );
		return r;
	}

	static Question BuildQuestion(MacroQuestion macroQuestion, V cv, int counter) {
		Question q = new() {
			resultType = macroQuestion.resultType,
			result = cv.GetResult(counter),
			imagePaths = macroQuestion.imagePaths,

			question = BuildMacroText(macroQuestion.question, cv)
		};

		foreach (var mt in macroQuestion.solutionSteps) 
			q.solutionSteps.Add(BuildMacroText(mt, cv));

		return q;
	}

	static string BuildMacroText(List<MacroText> macroText, V cv) {
		StringBuilder sb = new();
		foreach(MacroText mt in macroText) {
			if (mt is Macro macro) {
				sb.Append(cv.VariableRepresentation(macro.pointer));
			} else if (mt is Text text) {
				sb.Append(text.constText);
			}
		}
		return sb.ToString();
	}

	// serialize representations in bulk:
	public List<Representation> LanguageRepresentation(Language lang) {
		List<Representation> result = new();
		foreach(V variant in legit) {
			Representation r = GetRepresentation(variant, lang);
			result.Add(r);
		}
		return result;
	}

	// this will hardly be an option -> for word problem with 20k variants and 30 languages it is ~ 0.5GB 
	public string SerializedLanguageRepresentation(Language lang, bool indented) {
		List<Representation> languageRepr = LanguageRepresentation(lang);
		JsonSerializerOptions options = new() {	
			WriteIndented = indented,
			IncludeFields = true // important option -> serializes empty object if set to false..	
		};
		string json = JsonSerializer.Serialize<List<Representation>>(languageRepr, options);
		return json;
	}

	public string SerializeSelf(bool indented) {
		JsonSerializerOptions options = new() {
			WriteIndented = indented,
			IncludeFields = true // important option -> serializes empty object if set to false..	
		};
		string json = JsonSerializer.Serialize<Exercise<V>>(this, options);
		return json;
	}
}