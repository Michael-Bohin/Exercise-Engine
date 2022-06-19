namespace ExerciseEngine.Factory;



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

