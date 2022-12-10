using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Exercise_Engine.API;

namespace ExerciseEngine.MacroExercise;

public class VariantRecord : IBsonItem {
	public VariantRecord(Dictionary<string, string> variables, Dictionary<string, string> results) {
		Variables = variables;
		Results = results;
	}

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	[JsonPropertyName("v")]
	public Dictionary<string, string> Variables { get; set; }
	[JsonPropertyName("r")]
	public Dictionary<string, string> Results { get; set; }

	public string GetValueOfVariable(string macroPointer) { return Variables[macroPointer]; }
	public string GetValueOfResult(string macroPointer) { return Results[macroPointer]; }
}