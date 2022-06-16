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

// json ctor needs to initiliaze the entire dictionary. 
// The 2nd ctor is used when the exercise is first created,
// before calling the API of translator to fill other languages..
// The 3rd ctor is used in exercise definition property initiliaztion.
record Babylon {
	public Dictionary<Language, List<TextElement>> Text { get; private set; } = new();

	[JsonConstructor]
	public Babylon(Dictionary<Language, List<TextElement>> Text) { 
		this.Text = Text;	
	}

	public Babylon(List<TextElement> text, Language lang) {
		Text.Add(lang, text);
	}

	public Babylon() { }

	public List<TextElement> GetText(Language lang) => Text[lang];
	public void SetText(Language lang, List<TextElement> text) => Text[lang] = text; 
	public bool ContainsLanguage(Language lang) => Text.ContainsKey(lang);
}

// notes:
// macro points to a variable in exercise by its string id value. 
// macro is not aware of any further information about the variable. 
// it doesn't know its type or value. class Exercise knows that. 

//  Alternativní přístup k seriliazaci polymorfního potomka
//	[JsonInclude]
//	public const JsonTextElement _field_JsonId = JsonTextElement.LocalizedString;