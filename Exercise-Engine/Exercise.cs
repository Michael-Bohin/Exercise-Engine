using System.Text;
using System.Text.Json;
using ExerciseEngine.MacroExercise;

namespace ExerciseEngine;

public interface IExercise { 
	void FilterLegitVariants();
	MacroExercise.MacroExercise BuildMacroExercise();

	string ReportStatistics();
	string SerializeSelf(bool writeIndented);
	string GetName();
}

public abstract class Exercise<V> : IExercise where V : Variant {
	public readonly string exerciseName; // compiler assigned name -> not the title name for humans
	public MetaData metaData;

	// Builder:
	public readonly Dictionary<Language, MacroAssignment> MacroAssignments = new();

	// Factory method:
	public readonly int expected;
	public int actual;
	public int legitCount; // so that this information gets serialized
	public int illegalCount; // so that this information gets serialized
	public List<int> constraintLog = new(); // stores number of times the constraint with specific id has been triggered
	public List<V> legit = new();
	public List<List<V>> illegal = new();

	protected Exercise(int constraintCount, int expected, string exerciseName, int exerciseId, Language initialLanguage, ExerciseType exerciseType) {
		this.expected = expected;
		actual = 0;
		for (int i = 0; i < constraintCount; i++) {
			constraintLog.Add(0);
			illegal.Add(new());
		}
		this.exerciseName = exerciseName;
		metaData = new(exerciseId, initialLanguage, exerciseType);
		MacroAssignment ma = new(exerciseId, initialLanguage, BuildDescription(), BuildQuestions());
		MacroAssignments[initialLanguage] = ma;
	}

	public string GetName() => exerciseName;

	public abstract void FilterLegitVariants();

	protected abstract MacroString BuildDescription();

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
	
	public string SerializeSelf(bool indented) {
		legitCount = legit.Count;
		illegalCount = illegal.Count;

		JsonSerializerOptions options = new() {
			WriteIndented = indented,
			IncludeFields = true // important option -> serializes empty object if set to false..	
		};
		string json = JsonSerializer.Serialize<Exercise<V>>(this, options);
		return json;
	}

	public MacroExercise.MacroExercise BuildMacroExercise() {
		List<VariantRecord> variants = new();
		foreach (V variant in legit)
			variants.Add(variant.ToVariantRecord());

		metaData.variantsCount = variants.Count;
		MacroExercise.MacroExercise me = new(metaData, variants, MacroAssignments);
		return me;
	}
}