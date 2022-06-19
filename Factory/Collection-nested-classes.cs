namespace ExerciseEngine.Factory;
using System.Text;

abstract class Variation {
	public List<Variable> Variables { get; } = new();
}

sealed class WordProblemVariation : Variation {
	public List<MacroText> Results { get; } = new();
}

sealed class NumericalExerciseVariation : Variation {
	public MacroText Result { get; } = new();
}

abstract class Variable { 
	abstract public string GetValue(Language lang);	
}

sealed class InvariantVariable : Variable {
	public string Value { get;} = default!;

	override public string GetValue(Language lang) => Value;
}

sealed class CulturalVariable : Variable {
	public Dictionary<Language, string> Dict { get; } = new();

	override public string GetValue(Language lang) => Dict[lang];
}

class MacroText {
	public List<TextElement> Elements { get; } = new();

	public string ConstructText(Language lang, Variation v) {
		StringBuilder sb = new();
		for(int i = 0; i <Elements.Count; i++) 
			sb.Append(Elements[i].GetValue(lang, v));

		return sb.ToString();
	}
}

abstract class TextElement { 
	abstract public string GetValue(Language lang, Variation v);	
}

sealed class Macro : TextElement {
	public int Pointer { get; }
	override public string GetValue(Language lang, Variation v) {
		return v.Variables[Pointer].GetValue(lang);
	}
}

sealed class Text : TextElement {
	public string ConstText { get; } = default!;
	override public string GetValue(Language lang, Variation v) => ConstText; // ignoring language here is actually correct 

}