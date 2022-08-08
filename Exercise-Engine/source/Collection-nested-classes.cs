namespace ExerciseEngine;

public class ExerciseRepresentation
{
    public readonly string assignment;
    public readonly List<string> questions;
    public readonly List<string> results;
    public readonly List<string> solutionSteps;

    public ExerciseRepresentation(string assignment, List<string> questions, List<string> results, List<string> solutionSteps)
    {
        this.assignment = assignment; this.questions = questions; this.results = results; this.solutionSteps = solutionSteps;
    }
}

public class Variant
{
    [JsonPropertyName("inv")]
    public List<string> invariant; // invariant variables
    [JsonPropertyName("cul")]
    public List<Dictionary<Language, string>> cultural; // cultural variables

    public Variant(List<string> invariant, List<Dictionary<Language, string>> cultural)
    {
        this.invariant = invariant; this.cultural = cultural;
    }

    public Variant()
    {
        invariant = new();
        cultural = new();
    }

    override public string ToString()
    {
        StringBuilder sb = new();
        sb.Append("Variant: ");
        foreach (string s in invariant)
            sb.Append(s + ' ');
        sb.Append(", ");
        foreach (var dict in cultural)
            foreach (var kvp in dict)
                sb.Append(kvp.Key.ToString() + "->" + kvp.Value + " ");
        sb.Append('\n');
        return sb.ToString();
    }
}

public class MacroText
{
    public MacroText() { elements = new(); }
    [JsonPropertyName("elements")]
    public List<TextElement> elements;

    public string ToString(Language lang, Variant v)
    {
        StringBuilder sb = new();
        for (int i = 0; i < elements.Count; i++)
            sb.Append(elements[i].GetValue(lang, v));
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        foreach (var e in elements)
            sb.Append(e.ToString() + " >> ");
        return sb.ToString();
    }
}

[JsonPolymorphic(
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,
    TypeDiscriminatorPropertyName = "$type")
]
[JsonDerivedType(typeof(TextElement), typeDiscriminator: 0)]
[JsonDerivedType(typeof(Macro), typeDiscriminator: 1)]
[JsonDerivedType(typeof(Text), typeDiscriminator: 2)]
abstract public class TextElement
{
    abstract public string GetValue(Language lang, Variant v);
}

sealed public class Macro : TextElement
{
    [JsonPropertyName("pointer")]
    public int pointer;
    [JsonPropertyName("type")]
    public VariableDiscriminator type;

    [JsonConstructor]
    public Macro() { } // json serializer ctor
    public Macro(int pointer, VariableDiscriminator type)
    {
        this.pointer = pointer; this.type = type;
    }

    public void SerializerSetPointer(int p) => pointer = p;
    public void SerializerSetDiscriminator(VariableDiscriminator vd) => type = vd;

    override public string GetValue(Language lang, Variant v)
    {
        if (type == VariableDiscriminator.Invariant)
            return v.invariant[pointer];

        return v.cultural[pointer][lang];
    }

    override public string ToString() => $"Macro: [P:{pointer}, Type:{type}]";
}

sealed public class Text : TextElement
{
    [JsonPropertyName("text")]
    public string constText = default!;
    [JsonConstructor]
    public Text() { }
    public Text(string constText) => this.constText = constText;

    public void SerializerSetText(string s) => constText = s;
    override public string GetValue(Language lang, Variant v) => constText;
    override public string ToString() => $"Text: [CT:{constText}]";
}