namespace ExerciseEngine;

using System.Text;

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
	// Factory method:
	protected int expected, actual;
	protected List<int> constraintLog = new(); // stores number of times the constraint with specific id has been triggered
	public List<V> legit = new();
	public List<List<V>> illegal = new();
	
	// Builder:
	readonly protected Dictionary<Language, MacroRepresentation> babylon = new(); // i am just running out of names I can image at this point tbh. once finnished,  think this one through again.
	readonly protected bool monoLingual;

	protected Exercise(bool monoLingual, int constraintCount, int expected) {
		this.monoLingual = monoLingual;
		this.expected = expected;
		actual = 0;
		for (int i = 0; i < constraintCount; i++) {
			constraintLog.Add(0);
			illegal.Add(new());
		}
	}

	/// <summary>
	/// Factory Method -> Create n legit variants
	/// </summary>
	public abstract void FilterLegitVariants();

	public string ReportStatistics() {
		StringBuilder sr = new();
		sr.Append($"Expected variants count: {expected}\n");
		sr.Append($"Actual variants instantiated: {actual}\n");
		sr.Append($"Number of constraints: {constraintLog.Count}\n");
		if (constraintLog.Count > 0)
			for (int i = 0; i < constraintLog.Count; i++)
				sr.Append($"{i}, occurences: {constraintLog[i]}\n");
		return sr.ToString();
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
	public bool HasLanguage(Language language) {
		return monoLingual || babylon.ContainsKey(language);
	}

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
}