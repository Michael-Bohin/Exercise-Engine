using System.Text.Json.Serialization;
using System.Text;

namespace ExerciseEngine.MacroExercise;

public class MacroString {
	public List<MacroText> macroTexts = new();

	public MacroString() { }

	[JsonConstructor]
	public MacroString(List<MacroText> macroTexts) {
		this.macroTexts = macroTexts;
	}

	public string MergeWithVariant(VariantRecord variant) {
		StringBuilder sb = new();
		foreach (MacroText mt in macroTexts) {
			if (mt is Text text) {
				sb.Append(text.constText);
			} else if (mt is Macro macro) {
				string value = variant.GetValueOfVariable(macro.pointer);
				sb.Append(value);
			} else {
				throw new InvalidOperationException("Error in matrix detected!");
			}
		}

		return sb.ToString();
	}

	public string MergeWithVariant(Variant v) {
		StringBuilder sb = new();
		foreach (MacroText mt in macroTexts) {
			if (mt is Text text) {
				sb.Append(text.constText);
			} else if (mt is Macro macro) {
				string value = v.GetValueOfVariable(macro.pointer);
				sb.Append(value);
			} else {
				throw new InvalidOperationException("Error in matrix detected!");
			}
		}

		return sb.ToString();
	}
}

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(Macro), "macro")]
[JsonDerivedType(typeof(Text), "text")]
abstract public class MacroText {
	const char openCurly = '{', closeCurly = '}', quotes = '"';

	public string TranslateInstantiation(int order) {
		return $"{ChildType()} el{order} = new({quotes}{Representation()}{quotes});";
	}

	abstract protected string ChildType();
	abstract protected string Representation();
}

sealed public class Macro : MacroText {
	public string pointer;
	public Macro(string pointer) => this.pointer = pointer;

	override protected string ChildType() => "Macro";
	override protected string Representation() => pointer;
}

sealed public class Text : MacroText {
	public string constText = default!;

	public Text(string constText) => this.constText = constText;

	override protected string ChildType() => "Text";
	override protected string Representation() => constText;
}
