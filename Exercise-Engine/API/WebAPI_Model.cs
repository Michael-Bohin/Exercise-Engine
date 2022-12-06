using System.Text;
using System.Text.Json.Serialization;
using ExerciseEngine.MathRepresentation;

namespace ExerciseEngine.API;

//  living space of classes that are exposed through Api/webApi as a result of exercise engine
//  The only other namespace that sees this namespace is ExerciseEngine.MacroRepresentation
//  Other namespaces should not worry about structure of classes being sent to external world!

public class Assignment
{
    public int exerciseId;
    public Language language;
    public string description = ""; // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HODNOTA STRINGU SE BUDE LISIT PRES PREKLADY PRIKLADU
    public List<ExerciseQuestion> questions = new();

    public Assignment(int exerciseId, Language language, string description, List<ExerciseQuestion> questions)
    {
        this.exerciseId = exerciseId;
        this.language = language;
        this.description = description;
        this.questions = questions;
    }

    public string PrettyPrint() {
        StringBuilder sb = new();
        sb.Append($"id: {exerciseId}, lang: {language}\n");
        sb.Append($"description: {description}\n");
        foreach(ExerciseQuestion question in questions) 
            sb.Append(question.PrettyPrint());   
        sb.Append("\n\n____________________\n\n");
        return sb.ToString();
    }
}

[JsonDerivedType(typeof(IntQuestion))]
[JsonDerivedType(typeof(DecimalQuestion))]
[JsonDerivedType(typeof(FractionQuestion))]
[JsonDerivedType(typeof(SelectQuestion))]
[JsonDerivedType(typeof(MultiSelectQuestion))]
public abstract class ExerciseQuestion
{
    public ExerciseQuestion(string question) { Question = question; }
    public string Question { get; } // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HODNOTA STRINGU SE BUDE LISIT PRES PREKLADY PRIKLADU

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<SolutionStep>? SolutionSteps { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? ImagePaths { get; set; }

    public abstract ResultType ResultType { get; }

    public abstract string PrettyPrintDifferences();

    public string PrettyPrint() {
        StringBuilder sb = new();
        sb.Append($"question: {Question}\n");
        string differences = PrettyPrintDifferences();
		sb.Append(differences + '\n');
        if(SolutionSteps != null ) 
            foreach(SolutionStep ss in SolutionSteps) 
                sb.Append(ss.PrettyPrint());
            
        if(ImagePaths != null)
            foreach(string path in ImagePaths)  
                sb.Append($"img: {path}\n");

        return sb.ToString();
    }
}

public class SolutionStep
{
    public SolutionStep(string step, string? didacticComment = null)
    {
        Step = step;
        DidacticComment = didacticComment;
    }
    /// <summary>
    /// Current state of the exercise.
    /// </summary>
    public string Step { get; init; } // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HODNOTA STRINGU SE BUDE LISIT PRES PREKLADY PRIKLADU

    /// <summary>
    /// Didactic comment describing the current step and
    /// providing instructions for how to get to the next step.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DidacticComment { get; init; } // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HODNOTA STRINGU SE BUDE LISIT PRES PREKLADY PRIKLADU

    public string PrettyPrint() {
        string comment = ">> absent <<";
        if(DidacticComment!= null)
            comment = DidacticComment;

        return $"step: {Step}, \ncomment: {comment}\n";
	}
}

sealed public class IntQuestion : ExerciseQuestion
{
    public IntQuestion(string question, string result)
        : base(question)
    {
        Result = result;
    }

    public string Result { get; } // !!! string type forced by reciever of the web api, stand alone destroys the mototivation for polymorphic tree, obviously. 
    public override ResultType ResultType { get => ResultType.Int; }

    public override string PrettyPrintDifferences() {
        return $"Int result: {Result}";
    }
}

public class DecimalQuestion : ExerciseQuestion
{
    public DecimalQuestion(string question, string result, int precision)
        : base(question)
    {
        Result = result;
        Precision = precision;
    }

    public int Precision { get; set; }
    public string Result { get; }
    public override ResultType ResultType { get => ResultType.Decimal; }

	public override string PrettyPrintDifferences() {
		return $"Decimal result: {Result}, precision: {Precision}";
	}
}

public class FractionQuestion : ExerciseQuestion
{
    public FractionQuestion(string question, Fraction result)
        : base(question)
    {
        Result = result;
    }

    public Fraction Result { get; }
    public override ResultType ResultType { get => ResultType.Fraction; }

	public override string PrettyPrintDifferences() {
		return $"Fraction result: {Result.Numerator} / {Result.Denominator}";
	}
}

public class ExerciseQuestionOption
{
    public ExerciseQuestionOption(string value, string text)
    {
        Value = value;
        Text = text;    // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> HODNOTA STRINGU SE BUDE LISIT PRES PREKLADY PRIKLADU
    }
    public string Value { get; init; } // oznaceni volby						napr.:	"a"
    public string Text { get; init; } // reprezentace moznosti jako takvoe		napr.:	"Velká váza je 5x větší než střední váza."

    public string PrettyPrint() {
        return $"Option, value: {Value}, text: {Text}";
    }
}

public abstract class OptionsExerciseQuestion : ExerciseQuestion
{
    public OptionsExerciseQuestion(string question, List<ExerciseQuestionOption> options)
        : base(question)
    {
        Options = options;
    }

    public List<ExerciseQuestionOption> Options { get; }
}

public class SelectQuestion : OptionsExerciseQuestion
{
    public SelectQuestion(string question, List<ExerciseQuestionOption> options, string result)
        : base(question, options)
    {
        Result = result;
    }
    public string Result { get; }

    public override ResultType ResultType { get => ResultType.Select; }

	public override string PrettyPrintDifferences() {
        StringBuilder sb = new();
        foreach (ExerciseQuestionOption option in Options)
            sb.Append(option.PrettyPrint() + '\n');

        sb.Append($"Select result: {Result}\n");
		return sb.ToString();
	}
}

public class MultiSelectQuestion : OptionsExerciseQuestion
{
    public MultiSelectQuestion(string question, List<ExerciseQuestionOption> options, List<string> result)
        : base(question, options)
    {
        Result = result;
    }
    public List<string> Result { get; }
    public override ResultType ResultType { get => ResultType.MultiSelect; }

	public override string PrettyPrintDifferences() {
		StringBuilder sb = new();
		foreach (ExerciseQuestionOption option in Options)
			sb.Append(option.PrettyPrint() + '\n');

        sb.Append("Correct multiselect results: ");
        foreach(string res in Result)
            sb.Append(res + " ; ");
        sb.Append('\n');
		return sb.ToString();
	}
}