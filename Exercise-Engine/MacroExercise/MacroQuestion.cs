using ExerciseEngine.API;
using ExerciseEngine.MathEngine;
using System.Text.Json.Serialization;

namespace ExerciseEngine.MacroExercise;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "discriminator")]
[JsonDerivedType(typeof(MacroIntQuestion), "Macro int Question")]
[JsonDerivedType(typeof(MacroDecimalQuestion), "Macro decimal Question")]
[JsonDerivedType(typeof(MacroFractionQuestion), "Macro Fraction Question")]
[JsonDerivedType(typeof(MacroSelectQuestion), "Macro Select Question")]
[JsonDerivedType(typeof(MacroMultiSelectQuestion), "Macro MultiSelect Question")]
public abstract class MacroQuestion {
	public MacroQuestion(MacroString question) { Question = question; }
	public MacroString Question { get; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public List<MacroSolutionStep>? SolutionSteps { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public List<string>? ImagePaths { get; set; }

	public abstract ResultType ResultType { get; }

	public ExerciseQuestion MergeWithVariant(VariantRecord variant) {
		// 1. Prepare MacroString Question in string representation
		// 2. Prepare MacroSolutionSetps in string representation
		// 3. Pass both, along with ImagePaths to abstract MergeWithVariantChildren method that does the job in which the resulting classes differ
		string q = Question.MergeWithVariant(variant);
		List<SolutionStep>? sss = BuildSolutionSteps(variant);
		return MergeWithVariant(q, variant, sss, ImagePaths);
	}

	public List<SolutionStep>? BuildSolutionSteps(VariantRecord variant) {
		List<SolutionStep>? _solutionSteps;
		if (SolutionSteps == null || SolutionSteps.Count == 0) {
			_solutionSteps = null;
		} else {
			_solutionSteps = new();
			foreach (MacroSolutionStep macroStep in SolutionSteps) {
				SolutionStep ss = macroStep.MergeWithVariant(variant);
				_solutionSteps.Add(ss);
			}
		}

		return _solutionSteps;
	}

	protected abstract ExerciseQuestion MergeWithVariant(string _question, VariantRecord variant, List<SolutionStep>? _solutionSteps, List<string>? _imagePaths);
}

public abstract class MacroSingleResultQuestion : MacroQuestion {
	public MacroSingleResultQuestion(MacroString question, Macro result) : base(question) {
		Result = result;
	}

	public Macro Result { get; }
}

sealed public class MacroIntQuestion : MacroSingleResultQuestion {
	public MacroIntQuestion(MacroString question, Macro result)
		: base(question, result) {
	}
										// !!! string type forced by reciever of the web api, stand alone destroys the mototivation for polymorphic tree, obviously.
	public override ResultType ResultType { get => ResultType.Int; }    // sodoma gomora hezkeho navrhu zpusobena pozadavkem web api bezicim v node.js

	protected override IntQuestion MergeWithVariant(string question, VariantRecord variant, List<SolutionStep>? solutionSteps, List<string>? imagePaths) {
		string _result = variant.GetValueOfResult(Result.pointer);

		return new(question, _result) {
			SolutionSteps = solutionSteps,
			ImagePaths = imagePaths
		};
	}
}

public class MacroDecimalQuestion : MacroSingleResultQuestion {
	public MacroDecimalQuestion(MacroString question, Macro result, int precision)
		: base(question, result) {
		Precision = precision;
	}

	protected override DecimalQuestion MergeWithVariant(string question, VariantRecord variant, List<SolutionStep>? solutionSteps, List<string>? imagePaths) {
		string _result = variant.GetValueOfResult(Result.pointer);

		return new(question, _result, Precision) {
			SolutionSteps = solutionSteps,
			ImagePaths = imagePaths
		};
	}

	public int Precision { get; }
	public override ResultType ResultType { get => ResultType.Decimal; }
}

public class MacroFractionQuestion : MacroSingleResultQuestion {
	public MacroFractionQuestion(MacroString question, Macro result) : base(question, result) {	}

	protected override FractionQuestion MergeWithVariant(string question, VariantRecord variant, List<SolutionStep>? solutionSteps, List<string>? imagePaths) {
		string[] numDen = variant.GetValueOfResult(Result.pointer).Split('/'); // in case of fraction, we must pass both numerator and denoimnator so that webserver can compare that strictly to students answer..string[]stringstring[string[string[string[string[string[
		Fraction fraction = new(int.Parse(numDen[0]), int.Parse(numDen[1])); // !!! prasarna, pozdeji vymyslet co s tim !!!

		return new(question, fraction) {
			SolutionSteps = solutionSteps,
			ImagePaths = imagePaths
		};
	}

	public override ResultType ResultType { get => ResultType.Fraction; }
}

public class MacroQuestionOption {
	public MacroQuestionOption(string value, MacroString text) {
		Value = value;
		Text = text;
	}
	public string Value { get; init; } // oznaceni volby						napr.:	"a"
	public MacroString Text { get; init; } // reprezentace moznosti jako takvoe		napr.:	"Velká váza je 5x větší než střední váza."

	public ExerciseQuestionOption MergeWithVariant(VariantRecord variant) {
		return new(Value, Text.MergeWithVariant(variant));
	}
}

public interface IMacroOptionsQuestion  {
	public List<MacroQuestionOption> Options { get; }
}

public class MacroSelectQuestion : MacroSingleResultQuestion, IMacroOptionsQuestion {
	public MacroSelectQuestion(MacroString question, List<MacroQuestionOption> options, Macro result) : base(question, result) {	
		Options = options;	
	}

	protected override SelectQuestion MergeWithVariant(string question, VariantRecord variant, List<SolutionStep>? solutionSteps, List<string>? imagePaths) {
		string _result = variant.GetValueOfVariable(Result.pointer); // in case of fraction, we must pass both numerator and denoimnator so that webserver can compare that strictly to students answer..string[]stringstring[string[string[string[string[string[
		List<ExerciseQuestionOption> _options = new();
		foreach (MacroQuestionOption option in Options) {
			ExerciseQuestionOption _option = option.MergeWithVariant(variant);
			_options.Add(_option);
		}

		return new(question, _options, _result) {
			SolutionSteps = solutionSteps,
			ImagePaths = imagePaths
		};
	}

	public List<MacroQuestionOption> Options { get; }
	public override ResultType ResultType { get => ResultType.Select; }
}

public class MacroMultiSelectQuestion : MacroQuestion, IMacroOptionsQuestion {
	public List<Macro> Results { get; }
	public List<MacroQuestionOption> Options { get; }

	public MacroMultiSelectQuestion(MacroString question, List<MacroQuestionOption> options, List<Macro> results)
		: base(question) {
		Results = results;
		Options = options;
	}

	protected override MultiSelectQuestion MergeWithVariant(string question, VariantRecord variant, List<SolutionStep>? solutionSteps, List<string>? imagePaths) {
		List<string> results = new();
		foreach (Macro macro in Results) {
			string value = variant.GetValueOfResult(macro.pointer);
			results.Add(value);
		}

		List<ExerciseQuestionOption> _options = new();
		foreach (MacroQuestionOption option in Options) {
			ExerciseQuestionOption _option = option.MergeWithVariant(variant);
			_options.Add(_option);
		}

		return new(question, _options, results) {
			SolutionSteps = solutionSteps,
			ImagePaths = imagePaths
		};
	}

	public override ResultType ResultType { get => ResultType.MultiSelect; }
}


public class MacroSolutionStep {
	public MacroSolutionStep(MacroString step, MacroString? didacticComment = null) {
		Step = step;
		DidacticComment = didacticComment;
	}
	/// <summary>
	/// Current state of the exercise.
	/// </summary>
	public MacroString Step { get; init; }

	/// <summary>
	/// Didactic comment describing the current step and
	/// providing instructions for how to get to the next step.
	/// </summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public MacroString? DidacticComment { get; init; }

	public SolutionStep MergeWithVariant(VariantRecord v) {
		string _step = Step.MergeWithVariant(v);
		string? _didacticComment;
		if (DidacticComment == null) {
			_didacticComment = null;
		} else {
			_didacticComment = DidacticComment.MergeWithVariant(v);
		}

		return new(_step, _didacticComment);
	}
}