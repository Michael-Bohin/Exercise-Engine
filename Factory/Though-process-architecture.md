# Some theory front up:

## Slovní úloha v 'y' jazycích je uspořádaná osmice:

1. počítačové jméno
2. lidské jméno
3. zadání
4. otázky
5. výsledky
6. komentované kroky řešení
7. obrázky
8. skupiny

Body [2,6] a 8 jsou ve slovníku, kde klíče budou různé jazykové lokalizace. S otypováním by třída slovní úloha mohla vypadat následovně:


```c#
enum Language { en, pl, ua, cs, ... }

class WordProblem {
	ulong uniqueId;
	Dictionary<Language, string> name;
	Dictionary<Language, string> assignment;
	Dictionary<Language, List<string>> questions;
	Dictionary<Language, List<string>> results;
	Dictionary<Language, List<string>> solutionSteps;
	// ??? pictures; dev in future, today type is unknown
	Dictionary<Language, Groups> groups;
}
```

Dobře, tohle je v případě jedné slovní ulohy **S PRÁVĚ JEDNOU VARIANTOU**. Slovní úloha v této podobě, obsahuje překlady do **'y'** různých jazyků. Co kdybychom ale zároveň chtěli mít **'z'** variant? Tj. stejná úloha, ale jiná vstupní čísla a výsledek. Pak musíme návrh výše rozšířit o:

1. Každý string bude v bodě proměnných obsahovat makra -> proměnné, které se s každou variantou příkladu mění.
2. Dodat seznam variant příkladu.
3. Vypořádat se s cornercasem, kdy proměnná je typu string, která má v různych jazycích různou podobu. (Nebo ruzný přístup k desetinné čárce/tečce nebo znaku pro dělení napříč různými kulturami.)

Tedy chceme mít sbírku příkladů, která bude mít **'x'** slovních úloh a každá úloha bude mít **'y'** různých překladů do **'z'** různých variant. Říkejme takovému objektu 'Kolekce slovních úloh'.

## Kolekce slovních úloh s 'y' překlady a 'z' variantami je uspořádaná n-tice:

1. počítačové jméno
2. lidské jméno
3. seznam **'z'** variant
4. zadání
5. otázky
6. komentované kroky řešení
7. obrázky
8. skupiny

Je třeba zadefinovat tři nové objekty: Varianta, abstraktní proměnná a MacroText. 

### Definice 'Varianta':

1. seznam proměnných 
2. seznam odpovědí

### Proměnná

Třída Variable je abstraktní třída s n potomky:

1. ```InvariantVariable```: ```string```
2. ```LocalizedVariable```: ```Dict Lang -> string```
3. ```Picture```: ```???```
4. ```Link```:```string```
5. ```Animace```:```???```

### Proměnné v textu: 

Třída ```MacroText``` drží ```List<TextElement>```, kde ```TextElement``` je abstraktní třída s potomky:

1. ```Macro```: ```int``` pointer
2. ```Text```:```string``` constText

Logika uložené kolekce je minimalizování uložených dat, tedy veškeré texty v různých jazycích jsou v kolekci uloženy právě jednou. Proměnné ve kterých se varianty příkladu liší jsou uložené v samostatném seznamu variant. V textu zadání, otázek atd. se na ně odkazujeme 'pointerem' -> číslo indexu pod kterým je najdeme v seznamu nějaké konkrétní varianty.

Ok, jak bude vypadat implementace konkrétní slovní úlohy? 

## Implementace kolekce slovní úlohy s 'y' překlady a 'z' variantami:

```c#
class WordProblemCollection {
	ulong uniqueId;
	Dictionary<Language, string> name;
	List<Variation> variants;
	Dictionary<Language, MacroText> assignment;
	Dictionary<Language, List<MacroText>> questions;
	Dictionary<Language, List<MacroText>> solutionSteps;
	// ??? pictures; dev in future, today type is unknown
	Dictionary<Language, Groups> groups;
}

class Variation {
	List<Variable> variables;
	List<MacroText> results;
}

// ___________________________________________ //

abstract class Variable { }

class InvariantVariable : Variable { 
	string value;
}

class CulturalVariable : Variable {
	Dictionary<Language, string> dict;
}

// ___________________________________________ //

class MacroText {
	List<TextElement> elements;
}

abstract class TextElement {}

class Macro : TextElement { 
	int pointer;
}

class Text : TextElement {
	string constText;
}
```


## Rozdíl mezi slovní a početní úlohou:

1. Početní úloha nemá otázky, jen zadaní. (Např. vzoreček, výraz, rovnici, nerovnici atd.) 
2. Početní úloha má místo seznamu odpovědí jednu odpověď (plural -> singular)

Důsledky pro polymorfní stromeček:

1. Abstraktní předek slovní úlohy i početního příkladu bude mít vše kromě otázek.
2. Třída ```Variation``` bude také abstraktní.  V případě Variation slovní úlohy bude mít seznam odpovědí, v připadě Variation početního příkladu bude právě jedna odpověď.

## Geometrické úlohy 

Pro ně zatím vytvořím jejich třídy, ale do ctoru příjde vyhození NotYetImplemented výjimky. S použitím System.Drawing určitě půjdou implementovat! Ale jen za předpokladu předchozího zájmu o slovní a početní úlohy, který ufinancuje vývoj geometrických úloh. 

## Reprezentace příkladů při konkrétním zavolání varianty y a jazyku z. 

Poslední díl do skládačky, jak typ bude kolekce vracet pokud jí volající zavolá a řekne chci tuhle variantu v tomhle jazyce?

```c#
abstract class Exercise {
	// metadata o uloze:
	ulong uniqueId;
	string name;
	Language lang;
	int variationId;
	Groups groups;

	// vlastni uloha:
	string assignment;
	List<string> solutionSteps;
	// List<Picture> pictures;

}

class WordProblem : Exercise {
	// vlastni uloha:
	List<string> questions;
	List<string> results;
}

class NumericalExercise : Exercise {
	// vlastni uloha:
	string result;
}
```

TODO: 

1. Projit not minimized vypis vsech variant a prikladu nejake slovni a nejake pocetni ulohy. Whats the file size? 
2. Komprimovat JSON serializaci kolekce. How much has the file size dropped?
3. Ukazat priklady jak se z komprimovane kolekce postavi cely priklad pri volani konkretni varianty a konkretniho jazyka. 
4. Napsat factory, ktera vygeneruje ExerciseCollection z bodu 2. 