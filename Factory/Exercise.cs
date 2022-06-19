namespace ExerciseEngine.Factory;

enum Language { en, cs, pl, ua }

abstract class Exercise {
	// public abstract string Result { get; }
	// public abstract string Assignment { get; }
}


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

// string does not contain macros: pointer to variables 
// Text is contains List of text elements: either string or macro

class WordProblem : Exercise {
	public ulong UniqueId { get; }								// computer name
	public Dictionary<Language, string> Name { get; } = new();	// human name in different languages
	public Groups Groups { get; } = new();

	public Dictionary<Language, Text> Assignment { get; } = new();  // zadani v ruznych jazycich
	public Dictionary<Language, List<Text>> Questions { get; } = new();  // otazky v ruznych jayzcich

	public List<WP_ExerciseVariation> Variations { get; } = new(); // informace nutne pro vsechny obmeny tohoto typu prikladu
	// jak ukladat relevnatni obrazky?  ... solve later ...
}

record WP_ExerciseVariation { // obmena typu prikladu s konkretnimi hodnotami promenne
	public List<string> Variables { get;} = new();
	public List<string> Results { get; } = new();
	public Dictionary<Language, string> MultiCulturalVariables { get; } = new();

}

// public List<(int pointer, bool multiCultural)> VarPointers { get; } = new();
// public Dictionary<Language, List<string>> Results { get; } = new(); // vysledky

// string representations of variables indexed by their sorted order.
// Their order is given by sorting prios inside interpreter: first prio type order, second prio alphabetic order.(names must be unique, hence order is unambiguous)
// Macro.Id is pointing at index  of this list: 'Variables[SomeMacro.Id]'

// ++ what to do when the variable is a string that is represented differently in different languages?
// ++ possibly images?
// ++ List<Variations....> how?? 

class SlovniUloha_012542 : WordProblem {
	public int A, B, C;
	public SlovniUloha_012542(int A, int B, int C) { 
		this.A = A; 
		Questions[Language.cs] = new List<string>();
		Questions[Language.cs].Add("Kolik jablek nakoupil pepicek?");
	}
}

abstract class NumericalExercise : Exercise {

}

abstract class GeometricExercise : Exercise {
	protected GeometricExercise() { 
		throw new NotImplementedException("System.Drawing will be definitelly utilized, but first attention of kids must be monetized using word problems and numerical exercises.");	
	}
}

