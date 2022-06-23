namespace ExerciseEngine.Factory;
using System.Text;

#region MetaData

class MetaData {
	public string name;
	public List<Topic> topics;
	public List<Classes> classes;
	public ExerciseType type;

	public MetaData() { // json serializer ctor
		name = default!;
		topics = new();
		classes = new();
	}

	public MetaData(string name, List<Topic> topics, List<Classes> classes, ExerciseType type) {
		this.name = name; this.topics = topics; this.classes = classes; this.type = type;
	}
}

class LocalizationMetaData : MetaData {
	public (int id, Language language) uniqueId;

	public LocalizationMetaData() { } // json serializer ctor

	public LocalizationMetaData((int id, Language language) uniqueId, string name, List<Topic> topics, List<Classes> classes, ExerciseType type) : base(name, topics, classes, type) {
		this.uniqueId = uniqueId; 
	}
}

class ExerciseMetaData : MetaData {
	public (int id, Language language, int variant) uniqueId;

	public ExerciseMetaData() { } // json serializer ctor

	public ExerciseMetaData((int id, Language language, int variant) uniqueId, string name, List<Topic> topics, List<Classes> classes, ExerciseType type) : base(name, topics, classes, type) {
		this.uniqueId = uniqueId;
	}

	public ExerciseMetaData(LocalizationMetaData lmd, int variant): base(lmd.name, lmd.topics, lmd.classes, lmd.type) {
		uniqueId = (lmd.uniqueId.id, lmd.uniqueId.language, variant);
	}
}

#endregion

class ExerciseRepresentation {
	public readonly string assignment;
	public readonly List<string> questions;
	public readonly List<string> results;
	public readonly List<string> solutionSteps;

	public ExerciseRepresentation(string assignment, List<string> questions, List<string> results, List<string> solutionSteps) {
		this.assignment = assignment; this.questions = questions; this.results = results; this.solutionSteps = solutionSteps;
	}
}

class Variant {
	public readonly List<string> invariant; // invariant variables
	public List<Dictionary<Language, string>> cultural; // cultural variables

	public Variant(List<string> invariant, List<Dictionary<Language, string>> cultural) {
		this.invariant = invariant; this.cultural = cultural;	
	}

	public Variant() { 
		invariant =  new();
		cultural = new();
	}

	override public string ToString() {
		StringBuilder sb = new();
		sb.Append("Variant: ");
		foreach(string s in invariant)
			sb.Append(s + ' ');
		sb.Append(", ");
		foreach(var dict in cultural)
			foreach(var kvp in dict)
				sb.Append(kvp.Key.ToString() + "->" + kvp.Value + " ");
		sb.Append('\n');
		return sb.ToString();
	}
}

class MacroText {
	public readonly List<TextElement> elements = new();

	public string ToString(Language lang, Variant v) {
		StringBuilder sb = new();
		for(int i = 0; i <elements.Count; i++) 
			sb.Append(elements[i].GetValue(lang, v));
		return sb.ToString();
	}

	public override string ToString() {
		StringBuilder sb = new();
		foreach(var e in elements) 
			sb.Append(e.ToString() + " >> ");
		return sb.ToString();
	}
}

abstract class TextElement { 
	abstract public string GetValue(Language lang, Variant v);
	override abstract public string ToString();
}

sealed class Macro : TextElement {
	public int pointer;
	public VariableDiscriminator type;

	public Macro() { } // json serializer ctor
	public Macro(int pointer, VariableDiscriminator type) {
		this.pointer = pointer; this.type = type;
	}

	public void SerializerSetPointer(int p) => pointer = p;
	public void SerializerSetDiscriminator(VariableDiscriminator vd) => type = vd;

	override public string GetValue(Language lang, Variant v) {
		if(type == VariableDiscriminator.Invariant)
			return v.invariant[pointer];

		return v.cultural[pointer][lang];
	}

	override public string ToString() => $"Macro: [P:{pointer}, Type:{type}]";
}

sealed class Text : TextElement {
	public string constText = default!;
	public Text() { }
	public Text(string constText) => this.constText = constText;

	public void SerializerSetText(string s) => constText = s;
	override public string GetValue(Language lang, Variant v) => constText;
	override public string ToString() => $"Text: [CT:{constText}]";
}