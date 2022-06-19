# Some theory front up:

## Slovní úloha v 'z' jazycích je osmice:


1. počítačové jméno
2. lidské jméno
3. zadání
4. otázky
5. výsledky
6. komentované kroky řešení
7. obrázky
8. skupiny

Přičemž body [2,6] jsou ve slovníku, kde klíče budou různé jazykové lokalizace. S otypováním by třída slovní úloha mohla vypadat následnovně:


```c#
enum Language { en, pl, ua, cs }

class WordProblem {
	ulong uniqueId;
	Dictionary<Language, string> name;
	Dictionary<Language, string> assignment;
	Dictionary<Language, List<string>> questions;
	Dictionary<Language, List<string>> results;
	Dictionary<Language, List<string>> solutionSteps;
	??? pictures;
	Groups groups;
}
```


 Dobre, tohle je v pripade jedne slovni ulohy S PRAVE JEDNOU variantou.
 Slovni uloha v teto podobe jiz obsahuje ruzne preklady do 'z' jazyku.
 Co kdybychom ale zaroven chteli mit 'y' variant? Tj.stejna uloha, jine vstupni cisla a vysledek.
 Pak musime navrh vyse rozsirit o:
		1. Kazdy string bude v bode promennych obsahovat makra -> promenne, ktere se s kazdou
		   variatnou prikladu meni.
		2. Dodat seznam variant prikladu ('varianta prikladu' = 'variation of exercise')
		3. Vyporadat se s cornercasem, kdy promenna je typu string, ktera ma v ruznych jazycich ruznou podobu.
		   (Nebo ruzny pristup k destinne carce/tecce nebo znaku pro deleni napric ruznymi kulturami)

 Tj.chceme mit sbirku prikladu, ktera bude mit: 
 'x' slovnich uloh, kazda uloha v 'y' ruznych variantach prelozenych do 'z' ruznych jazyku.


 Kolekce slovni ulohy s 'y' variantami a 'z' ruznych jazyku je:
 1 uniqueId                 ulong
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

 Trida MacroText drzi List<TextElement>, kde TextElement je abstraktni trida s dvema potmky:
 Macro: ulong pointer, bool multiCultural
 Text: string


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
