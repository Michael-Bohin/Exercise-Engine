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

Body [2,6] jsou ve slovníku, kde klíče budou různé jazykové lokalizace. S otypováním by třída slovní úloha mohla vypadat následovně:


```c#
enum Language { en, pl, ua, cs, ... }

class WordProblem {
	ulong uniqueId;
	Dictionary<Language, string> name;
	Dictionary<Language, string> assignment;
	Dictionary<Language, List<string>> questions;
	Dictionary<Language, List<string>> results;
	Dictionary<Language, List<string>> solutionSteps;
	??? pictures; // dev in future, today type is unknown
	Groups groups;
}
```

Dobře, tohle je v případě jedné slovní ulohy **S PRÁVĚ JEDNOU variantou**. Slovní úloha v této podobě, obsahuje překlady do **'y'** různých jazyků. Co kdybychom ale zároveň chtěli mít **'z'** variant? Tj. stejná úloha, ale jiná vstupní čísla a výsledek. Pak musíme návrh výše rozšířit o:

1. Kazdy string bude v bode promennych obsahovat makra -> promenne, ktere se s kazdou variatnou prikladu meni.
2. Dodat seznam variant prikladu ('varianta prikladu' = 'variation of exercise')
3. Vyporadat se s cornercasem, kdy promenna je typu string, ktera ma v ruznych jazycich ruznou podobu. (Nebo ruzny pristup k destinne carce/tecce nebo znaku pro deleni napric ruznymi kulturami)

Tedy chceme mít sbírku příkladů, která bude mít **'x'** slovnich uloh a každá úloha bude mít **'y'** různých překladů do **'z'** různých variant. Říkejme takovému objektu 'Kolekce slovních úloh'.

## Kolekce slovní úlohy s 'y' překlady a 'z' variantami je uspořádaná n-tice:

1. počítačové jméno
2. lidské jméno
3. seznam **'z'** variant
4. zadání
5. otázky
6. komentované kroky řešení
7. obrázky
8. skupiny

Je třeba zadefinovat tři nové objekty: Varianta a abstraktní MacroText a abstraktní proměnná. 

### Definice 'Varianta':

1. seznam proměnných 
2. seznam odpovědí

### Proměnné v textu: 

Třída ```MacroText``` drží ```List<TextElement>```, kde ```TextElement``` je abstraktni třída s dvěma potomky. 
```Macro```: ```ulong``` pointer, ```bool``` multiCultural
```Text```:```string```


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

 Trida Variable je abstraktni trida s dvema potomky:
 InvariantVariable: string
 LocalizedVariable: Dict Lang -> string


 Ok, posledni dil do skladanky, jak bude vypadat konkretni slovni uloha? 
 Tj.Slovni uloha 'x' ve variaci 'y' a jazyce 'z' ?


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
