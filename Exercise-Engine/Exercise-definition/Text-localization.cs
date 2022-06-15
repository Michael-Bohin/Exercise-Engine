namespace Exercise_Engine;

enum Language { en, cs, pl, ua } // english, czech, polish, ukrainian
enum JsonTextElement { LocalizedString, Macro }

abstract record TextElement { }

record LocalizedString : TextElement { 
	public string Text { get; }
	public static JsonTextElement JsonId { get => JsonTextElement.LocalizedString; }

	public LocalizedString(string Text) { this.Text = Text; }
}

record Macro : TextElement {
	public string Id { get; }
	public static JsonTextElement JsonId { get => JsonTextElement.Macro; }

	public Macro(string Id) { this.Id = Id; }
}

record Babylon {
	[JsonInclude]
	private readonly Dictionary<Language, List<TextElement>> babylon = new(); // how do we enable private fields to be seializable by json? 

	[JsonConstructor]
	public Babylon() { }

	public Babylon(List<TextElement> text, Language lang) {
		babylon.Add(lang, text);
	}

	public List<TextElement> GetText(Language lang) => babylon[lang];
	public void SetText(Language lang, List<TextElement> text) => babylon[lang] = text; 
	public bool ContainsLanguage(Language lang) => babylon.ContainsKey(lang);
}

// notes:
// macro points to a variable in exercise by its string id value. 
// macro is not aware of any further information about the variable. 
// it doesn't know its type or value. class Exercise knows that. 

//  Alternativní přístup k seriliazaci polymorfního potomka
//	[JsonInclude]
//	public const JsonTextElement _field_JsonId = JsonTextElement.LocalizedString;