using System.Text;
using System.Text.Json.Serialization;
using ExerciseEngine.MathEngine;
using ExerciseEngine.API;
using System.Text.Json;
using MudBlazor;

namespace ExerciseEngine.MacroExercise;


// what about metadata? where is the optimal place for them? 
public class Macro_Exercise
{
    public Macro_Exercise(MetaData metaData, List<VariantRecord> variants, Dictionary<Language, Macro_Assignment> macroAssignments)
    {
        MetaData = metaData;
        Variants = variants;
        MacroAssignments = macroAssignments;
    }

    public MetaData MetaData { get; set; }
	public Dictionary<Language, Macro_Assignment> MacroAssignments { get; init; } // Babylon
	public List<VariantRecord> Variants { get; init; }

    public Assignment GetAssignment(VariantRecord variant) => GetAssignment(variant, Language.en);

    public Assignment GetAssignment(VariantRecord variant, Language lang)
    {
        // Fetch macro assignment in requested language
        Macro_Assignment macroA = MacroAssignments[lang];

        // Build variables for assignment ctor
        int id = macroA.exerciseId;
        string descripton = macroA.description.MergeWithVariant(variant);
        List<ExerciseQuestion> questions = new();
        int counter = 0;
        foreach (Macro_Question mq in macroA.questions)
            questions.Add(BuildQuestion(mq, variant, counter++));

        // return built assignment:
        return new(id, lang, descripton, questions);
    }

    static ExerciseQuestion BuildQuestion(Macro_Question macroQuestion, VariantRecord variant, int counter)
    {
        /// !!! this has changed in major way --> exercise question is now abstract root of many different seald children..
        ExerciseQuestion q = macroQuestion.MergeWithVariant(variant);
        return q;
    }

    // serialize representations in bulk:
    public List<Assignment> LanguageRepresentation(Language lang)
    {
        List<Assignment> assignments = new();
        foreach (VariantRecord variant in Variants)
        {
            Assignment a = GetAssignment(variant, lang);
            assignments.Add(a);
        }
        return assignments;
    }

    // this will hardly be an option -> for word problem with 20k variants and 30 languages it is ~ 0.5GB 
    public string SerializedLanguageRepresentation(Language lang, bool indented)
    {
        List<Assignment> languageRepr = LanguageRepresentation(lang);
        JsonSerializerOptions options = new()
        {
            WriteIndented = indented,
            IncludeFields = true // important option -> serializes empty object if set to false..	
        };
        string json = JsonSerializer.Serialize(languageRepr, options);
        return json;
    }

    public string SerializeAssignments(Language lang, bool indented) {
        List<Assignment> assignments = new();
        foreach(VariantRecord variant in Variants) {
            Assignment a = GetAssignment(variant, lang);
            assignments.Add(a);
        }

        JsonSerializerOptions options = new() { WriteIndented = indented, IncludeFields = true };
        return JsonSerializer.Serialize(assignments, options);
    }

	public string SerializeSelf(bool indented) {
		JsonSerializerOptions options = new() {
			WriteIndented = indented,
			IncludeFields = true
		};
		return JsonSerializer.Serialize(this, options);
	}

    public string PrettyPrint(Language lang, int max) {
		Macro_Assignment macroAss = MacroAssignments[lang];
        StringBuilder sb = new();
        int counter = 0;
        foreach(VariantRecord variant in Variants) {
            if(++counter > max)
                break;
            Assignment ass = macroAss.MergeWithVariant(variant);
            string prettyAssignment = ass.PrettyPrint();
            sb.Append(prettyAssignment);
        }
        return sb.ToString();
    }
}