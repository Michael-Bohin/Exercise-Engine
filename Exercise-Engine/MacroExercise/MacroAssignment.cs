using Exercise_Engine.API;
using ExerciseEngine.API;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExerciseEngine.MacroExercise;

public class MacroAssignment : IBsonItem {
	public int exerciseId;
	public Language language;
	public MacroString description;
	public List<MacroQuestion> questions = new();

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public MacroAssignment(int exerciseId, Language language, MacroString description, List<MacroQuestion> questions) {
		this.exerciseId = exerciseId;
		this.language = language;
		this.description = description;
		this.questions = questions;
	}

	public Assignment MergeWithVariant(VariantRecord variant) {
		string _description = description.MergeWithVariant(variant);
		List<ExerciseQuestion> _questions = new();
		foreach (MacroQuestion question in questions) {
			ExerciseQuestion _question = question.MergeWithVariant(variant);
			_questions.Add(_question);
		}
		Assignment ass = new(exerciseId, language, _description, _questions);
		return ass;
	}
	// add some methods from other regions here MacroAssignment should be more clever!
}
