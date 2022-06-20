namespace ExerciseEngine.Factory;
using System.Text;

class Variation {
	public List<string> Inv { get;} = new(); // invariant variables, shortified to minimize json
	public List<Dictionary<Language, string>> Cul { get; } = new(); // cultural variables, shortified to minimize json
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
	public int Pointer { get; private set;}
	public VariableDiscriminator Type { get; private set; }
	public Macro() { } // json serializer ctor
	public Macro(int Pointer, VariableDiscriminator Type) {
		this.Pointer = Pointer; this.Type = Type;
	}

	public void SerializerSetPointer(int p) => Pointer = p;
	public void SerializerSetDiscriminator(VariableDiscriminator vd) => Type = vd;

	override public string GetValue(Language lang, Variation v) {
		if(Type == VariableDiscriminator.Invariant)
			return v.Inv[Pointer];

		return v.Cul[Pointer][lang];
	}
}

sealed class Text : TextElement {
	public string ConstText { get; private set;} = default!;
	public Text() { }
	public Text(string ConstText) {
		this.ConstText = ConstText;
	}

	public void SerializerSetText(string t) => ConstText = t;
	override public string GetValue(Language lang, Variation v) => ConstText;
}