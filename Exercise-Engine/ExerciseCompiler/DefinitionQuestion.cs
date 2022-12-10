using ExerciseEngine.MacroExercise;
using System.Text;
using System;

namespace ExerciseEngine.ExerciseCompiler;

public abstract class DefinitionQuestion {
	public DefinitionQuestion(MacroString question, ResultMethod resultCode) {
		Question = question;
		ResultCode = resultCode;
	}

	const string openCurly = "{";
	public MacroString Question { get; }
	public ResultMethod ResultCode { get; }
	// solution steps are now being ignored -> implement them later 
	public List<string> ImagePaths = new();
	public abstract ResultType ResultType { get; }

	public string CompileNthResultMethod(int order) {
		StringBuilder sb = new();
		sb.Append($"\t// {ResultCode.Comment}\n");
		sb.Append($"\tstring GetResult_{order}() ");
		if (ResultCode.CodeDefined) {
			StringReader reader = new(ResultCode.Code);
			sb.Append($"{openCurly}\n");
			while (reader.ReadLine() is string line) {
				sb.Append($"\t\t{line}\n");
			}
			sb.Append("\t}\n\n");
		} else {
			sb.Append($"{openCurly}\n\t\t//  TO DO -> result code has not yet been defined\n");
		}

		return sb.ToString();
	}
}

public sealed class DefinitionIntQuestion : DefinitionQuestion {
	public DefinitionIntQuestion(MacroString question, ResultMethod resultCode)
		: base(question, resultCode) {
	}
	public override ResultType ResultType { get => ResultType.Int; }
}

public sealed class DefinitionDecimalQuestion : DefinitionQuestion {
	public DefinitionDecimalQuestion(MacroString question, ResultMethod resultCode, int precision)
		: base(question, resultCode) {
		Precision = precision;
	}

	public int Precision { get; }
	public override ResultType ResultType { get => ResultType.Decimal; }
}

public sealed class DefinitionFractionQuestion : DefinitionQuestion {
	public DefinitionFractionQuestion(MacroString question, ResultMethod resultCode) : base(question, resultCode) { }
	public override ResultType ResultType { get => ResultType.Fraction; }
}

public class DefinitionQuestionOption {
	public DefinitionQuestionOption(string value, MacroString text) {
		Value = value;
		Text = text;
	}
	public string Value { get; }		// oznaceni volby napr.: "a"
	public MacroString Text { get; }	// reprezentace moznosti jako takvoe napr.:	"Velká váza je 5x větší než střední váza."
}

public abstract class DefinitionOptionsQuestion : DefinitionQuestion {
	public DefinitionOptionsQuestion(MacroString question, List<DefinitionQuestionOption> options, ResultMethod resultCode) :base(question, resultCode) {
		Options = options;
	}
	public List<DefinitionQuestionOption> Options { get; }
}

public class DefinitionSelectQuestion : DefinitionOptionsQuestion {
	public DefinitionSelectQuestion(MacroString question, List<DefinitionQuestionOption> options, ResultMethod resultCode) : base(question,options, resultCode) { }
	public override ResultType ResultType { get => ResultType.Select; }
}

public class DefinitionMultiSelectQuestion : DefinitionOptionsQuestion {
	public DefinitionMultiSelectQuestion(MacroString question, List<DefinitionQuestionOption> options, ResultMethod resultCode) : base(question, options, resultCode) { }
	public override ResultType ResultType { get => ResultType.MultiSelect; }
}