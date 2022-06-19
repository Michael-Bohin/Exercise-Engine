namespace ExerciseEngine.Factory;

// some theory front up:
//
// slovni uloha (v 'z' jazycich) je:
// 1 uniqueId					ulong
// 2 name						Dict Lang -> string
// 3 zadani						Dict Lang -> string 
// 4 otazky						Dict Lang -> List<string>
// 5 spravne odpovedi			Dict Lang -> List<string>
// 6 komentovane kroky reseni	Dict Lang -> List<string>
// 7 obrazky					????? (will solve later)
// 8 skupiny					Groups
//   (relevnatni tridy, tema, typ prikladu)
//
// Dobre, tohle je v pripade jedne slovni ulohy S PRAVE JEDNOU variantou. 
// Slovni uloha v teto podobe jiz obsahuje ruzne preklady do 'z' jazyku. 
// Co kdybychom ale zaroven chteli mit 'y' variant? Tj. stejna uloha, jine vstupni cisla a vysledek.
// Pak musime navrh vyse rozsirit o:
//		1. Kazdy string bude v bode promennych obsahovat makra -> promenne, ktere se s kazdou 
//		   variatnou prikladu meni. 
//		2. Dodat seznam variant prikladu ('varianta prikladu' = 'variation of exercise')
//		3. Vyporadat se s cornercasem, kdy promenna je typu string, ktera ma v ruznych jazycich ruznou podobu. 
//		   (Nebo ruzny pristup k destinne carce/tecce nebo znaku pro deleni napric ruznymi kulturami)
//
// Tj. chceme mit sbirku prikladu, ktera bude mit: 
// 'x' slovnich uloh, kazda uloha v 'y' ruznych variantach prelozenych do 'z' ruznych jazyku.
// 
//
// Kolekce slovni ulohy s 'y' variantami a 'z' ruznych jazyku je:
// 1 uniqueId					ulong
// 2 name						Dict Lang -> string
// 3 seznam variant				List<Variation> // .. vnitrek class Variation nize ..
// 4 zadani						Dict Lang -> MacroText 
// 5 otazky						Dict Lang -> List<MacroText>
// 6 komentovane kroky reseni	Dict Lang -> List<MacroText>
// 7 obrazky					????? (will solve later)
// 8 skupiny					Groups
//   (relevnatni tridy, tema, typ prikladu)
//
// Trida Variation je:
// 1 seznam promennych				List<Variable>				// promenne se stejnou i ruznou string reprezentaci pres ruzne kultury
// 2 seznam spravnych odpovedi      List<string>				// za predpokladu, ze forma odpovedi bude ve vsech kulturach stejna (cislo)
//
// Trida Variable je abstraktni trida s dvema potomky:
// InvariantVariable: string
// LocalizedVariable: Dict Lang -> string
// 
// Trida MacroText drzi List<TextElement>, kde TextElement je abstraktni trida s dvema potmky:
// Macro: ulong pointer, bool multiCultural
// Text: string
//
//
// Ok, posledni dil do skladanky, jak bude vypadat konkretni slovni uloha? 
// Tj. Slovni uloha 'x' ve variaci 'y' a jazyce 'z' ?
// 
// Slovni uloha (x , y, z) je:
// 1 uniqueId				ulong 
// 2 name					string 
// 3 zadani					string 
// 4 otazky					List<string>
// 5 odpovedi				List<string>
// 6 kroky reseni			List<string>
// 7 obrazky				????
// 8 skupiny				Groups



enum Language { en, cs, pl, ua }

// string does not contain macros: pointer to variables 
// Text is contains List of text elements: either string or macro
abstract record TextElement { }

record Macro : TextElement { 
	public int Pointer { get; }
	public bool MultiCultural { get; }
	public Macro(int Pointer, bool MultiCultural) { 
		this.Pointer = Pointer; 
		this.MultiCultural = MultiCultural; 
	}
}

record String : TextElement {
	public string Msg { get; }
	public String(string Msg) { this.Msg = Msg; }
}

class Text {
	public List<TextElement> List { get; } = new();
}

abstract class ExerciseCollection { }

// For exercise with large number of variations (tens or hundred of thousands), this approach will likely 
// lead to bad use of memory. However at this step, the code is solving the problem to generate the 
// large collection of exercises. Once it will be done figure out the optimal usage of memory at that point.

class WordProblem : ExerciseCollection {
	public ulong UniqueId { get; }								// computer name
	public Dictionary<Language, string> Name { get; } = new();	// human name in different languages
	public Groups Groups { get; } = new();						// school classes, topics and exercise type

	public Dictionary<Language, Text> Assignment { get; } = new();			// zadani v ruznych jazycich
	public Dictionary<Language, List<Text>> Questions { get; } = new();     // otazky v ruznych jayzcich
	public List<Macro> Results { get; } = new();							// spravne odpovedi variace
	public Dictionary<Language, List<Text>> SolutionSteps { get; } = new(); // komentovane kroky reseni v ruznych jazycich
	public List<WP_ExerciseVariation> Variations { get; } = new();			// specificke informace pro vsechny obmeny tohoto prikladu
	// jak ukladat relevnatni obrazky?  ... solve later ...
}

record WP_ExerciseVariation { // obmena typu prikladu s konkretnimi hodnotami promenne
	public List<string> Variables { get;} = new();
	public Dictionary<Language, List<string>> MultiCulturalVariables { get; } = new();
}

// public List<(int pointer, bool multiCultural)> VarPointers { get; } = new();
// public Dictionary<Language, List<string>> Results { get; } = new(); // vysledky

// string representations of variables indexed by their sorted order.
// Their order is given by sorting prios inside interpreter: first prio type order, second prio alphabetic order.(names must be unique, hence order is unambiguous)
// Macro.Id is pointing at index  of this list: 'Variables[SomeMacro.Id]'

// ++ what to do when the variable is a string that is represented differently in different languages?
// ++ possibly images?
// ++ List<Variations....> how?? 

class NumericalExerciseCollection : ExerciseCollection {

}

class GeometricExerciseCollection : ExerciseCollection {
	protected GeometricExerciseCollection() { 
		throw new NotImplementedException("System.Drawing will be definitelly utilized, but first attention of kids must be monetized using word problems and numerical exercises.");	
	}
}

