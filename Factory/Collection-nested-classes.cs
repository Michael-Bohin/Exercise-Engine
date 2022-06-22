namespace ExerciseEngine.Factory;
using System.Text;

class Variation {
	public List<string> Inv { get;} = new(); // invariant variables, shortified name o minimize json
	public List<Dictionary<Language, string>> Cul { get; } = new(); // cultural variables, shortified name to minimize json

	override public string ToString() {
		StringBuilder sb = new();
		sb.Append("Variant: ");
		foreach(string s in Inv)
			sb.Append(s + ' ');
		sb.Append(", ");
		foreach(var dict in Cul)
			foreach(var kvp in dict)
				sb.Append(kvp.Key.ToString() + "->" + kvp.Value + " ");
		sb.Append('\n');
		return sb.ToString();
	}
}

class MacroText {
	public List<TextElement> Elements { get; } = new();

	public string ConstructText(Language lang, Variation v) {
		StringBuilder sb = new();
		for(int i = 0; i <Elements.Count; i++) 
			sb.Append(Elements[i].GetValue(lang, v));
		return sb.ToString();
	}

	public override string ToString() {
		StringBuilder sb = new();
		foreach(var e in Elements) 
			sb.Append(e.ToString() + " >> ");
		return sb.ToString();
	}
}

abstract class TextElement { 
	abstract public string GetValue(Language lang, Variation v);
	override abstract public string ToString();
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

	override public string ToString() => $"Macro: [P:{Pointer}, Type:{Type}]";
}

sealed class Text : TextElement {
	public string ConstText { get; private set;} = default!;
	public Text() { }
	public Text(string ConstText) {
		this.ConstText = ConstText;
	}

	public void SerializerSetText(string t) => ConstText = t;
	override public string GetValue(Language lang, Variation v) => ConstText;
	override public string ToString() => $"Text: [CT:{ConstText}]";
}