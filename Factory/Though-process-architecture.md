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

1. Každý string bude v bodě proměnných obsahovat makra -> proměnné, které se s každou variatnou příkladu mění.
2. Dodat seznam variant příkladu.
3. Vypořádat se s cornercasem, kdy proměnná je typu string, která má v různych jazycích různou podobu. (Nebo ruzný přístup k desetinné čárce/tečce nebo znaku pro dělení napříč různými kulturami.)

Tedy chceme mít sbírku příkladů, která bude mít **'x'** slovních úloh a každá úloha bude mít **'y'** různých překladů do **'z'** různých variant. Říkejme takovému objektu 'Kolekce slovních úloh'.

## Kolekce slovní úlohy s 'y' překlady a 'z' variantami je uspořádaná n-tice:

1. počítačové jméno
2. lidské jméno
3. seznam **'z'** variant
4. zadání
5. otázky
6. komentované kroky řešení
7. obrázky
8. skupiny

Je třeba zadefinovat tři nové objekty: MacroText, Varianta a abstraktní proměnná. 

### Definice 'Varianta':

1. seznam proměnných 
2. seznam odpovědí

### Proměnné v textu: 

Třída ```MacroText``` drží ```List<TextElement>```, kde ```TextElement``` je abstraktní třída s potomky:

1. ```Macro```: ```int``` pointer
2. ```Text```:```string``` constText

### Proměnná

Třída Variable je abstraktní třída s n potomky:

1. ```InvariantVariable```: ```string```
2. ```LocalizedVariable```: ```Dict Lang -> string```
3. ```Picture```: ```???```
4. ```Link```:```string```
5. ```Animace```:```???```

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

// 

class Variation {
	List<Variable> variables;
	List<MacroText> results;
}

// 

abstract class Variable { }

class InvariantVariable : Variable { 
	string value;
}

class CulturalVariable : Variable {
	Dictionary<Language, string> dict;
}

//

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



1. uniqueId                 ulong
 2 name Dict Lang -> string
 3 seznam variant               List<Variation> // .. vnitrek class Variation nize ..
 4 zadani Dict Lang -> MacroText
 5 otazky Dict Lang -> List<MacroText>
 6 komentovane kroky reseni Dict Lang -> List<MacroText>
 7 obrazky                  ????? (will solve later)
 8 skupiny Groups
   (relevnatni tridy, tema, typ prikladu)

 Trida Variation je:
 1 seznam promennych                List<Variable>				// promenne se stejnou i ruznou string reprezentaci pres ruzne kultury
 2 seznam spravnych odpovedi List<string>               // za predpokladu, ze forma odpovedi bude ve vsech kulturach stejna (cislo)







 Slovni uloha (x, y, z) je:
 1 uniqueId             ulong
 2 name                 string
 3 zadani                   string
 4 otazky List<string>
 5 odpovedi List<string>
 6 kroky reseni         List<string>
 7 obrazky              ????
 8 skupiny Groups

 poznamka do budoucnosti: Groups bude take nejspis potrebovat slovnik pres kultury, protoze napr. cesko, polsko a nemecko maji jine osnovy a tedy stejne priklady muzou a asi i spadnou do ruznych trid


 Rozdil mezi slovni a pocetni ulohou:
 1. Pocetni uloha nema otazky 
 2. pocetni uloha ma misto seznamu odpovedi jednu odpoved (plural -> singular)
 Tzn.
 1. abstraktni predek obou bude mit vse krome otazek
 2. variation bude take abstraktni v pripade slovni ulohy bude list odpovedi, v pripade pocetni jedna odpoved
