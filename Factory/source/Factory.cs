namespace ExerciseEngine.source;

// ukol factories:
// 1. vygenervat mnozinu vsech variant
// 2. ulozit ve forme exercise collection s jednou localization
// 3 . deserializace do json podoby -> priklad s vsemi jeho variantami je ulozeny v 'databazi' ve forme jsonu
// 
// dusledek: logika vypoctu prikladu konci ve factory - to ulozi hotove vysledky a postupy. Kolekce uz nevi nic o matice, jen ma ulozene spocitane vysledky ve forme stringu.

// ukol exercise collection:
// 1. po vygenerovani facotory zacit komunikovat s API nejakeho  prekladace a naplnit prazdne lokalizace aproximaci prekladu
// 2. korektorumprekladu nabidnout komunkacni API na opravu aproximace prekladu 
// 3. schvalene preklady od korektoru nabidnout webserveru k pouzivani pod metodou:
//		Exercise GetExercise(Language lang, int variation)

// ! do renaming ?
// 'x' prikladu. Kazdy v 'y' jazycich a 'z' variantach.
// ExerciseEngine:
// Exercise GetExercise(ExerciseUniqueId id)
//
// 1 priklad. Kazdy v 'y' jazycich a 'z' variantach.
// ExerciseCollection: 
// Exercise GetExercise(Language lang, int variant)

// meaning at the storage of exercise collection we will be doing the translations development.. 
// there will be some specialized class that will be using 
// exercise collection as input and ouput place...

// first approach:

class KonkretniPriklad : WordProblem { }

class JinyKonkretniPriklad : WordProblem { }


interface IExerciseCollection__
{
    // those already provided:
    Exercise GetExercise(Language lang, int variant);
}

// exercise factory vyprdne jedno jazykovou mnozinu variant stejneho prikladu:
class MonolingualExerciseVariants
{

}

// takes MonolingualExerciseVariants at input and outputs ExerciseCollectionPrecursor
class LocalizationsApproximator
{


}

// contains all localizations approximated, they still need to be corrected by individual language correctors..
// alternativne ApproximatedExerciseCollection
class ExerciseCollectionPrecursor
{

}

// provides all APIs of server nesseccary  to finalize translation aproximations into finnished product: ExerciseCollection
class LocalizationsCorrector
{

}

// class ExerciseCollesiton { }

// 1. ExerciseEditor : null -> ExerciseDefinition
// 2. Interpreter : ExerciseDefinition -> Factory
// 3. Factory -> MonolingualExerciseVariants
// 4. LocalizationsApproximator : MonolingualExerciseVariants -> ApproximatedExerciseCollection     (or ApproximatedExerciseCollection)
// 5. LocalizationsCorrector : ApproximatedExerciseCollection -> ExerciseCollection
// 6. ExerciseCollection -> json of self, that can be used by webserver to run EC instances and make them server exercises to users.


// The entire pipeline:       (Interpreter)											(LocalizationsApproximator)                                (LocalizationsCorrector)
// null -> ExerciseDefinition     --->      Factory -> MonolingualExerciseVariants            --->              ApproximatedExerciseCollection     --->                 ExerciseCollection
// (1 creator of exercise)             1. ExerciseEditor creates ExerciseDefinition (from teacher input)
// (program, almost instant)           2. Interpreter takes the definition and writes the code of factory 
// (program, almost instant)           3. Factory outputs MonolingualExerciseVariants
// (program, almost instant)           4. LocalizationApproximator fills Mono with translations from some AI, yielding 'ApproximatedExerciseCollection'
// ('n' correctors as native speakers) 5. LocalizationsCorrector mediates the correcting process from teachers around the world, once done, saving the resulting ExerciseCollection













interface IFactory
{
    ExerciseCollection CreateExerciseCollection();
}

// class 9, exercise number 1, factory

class C09_E001_Factory : IFactory
{

    // based on exercise definition write code that will enumarate all legit assignments 
    // factory can have a lot of shared code and so it will be practical to 
    public ExerciseCollection CreateExerciseCollection()
    {
        int uniqueId = 1;
        List<Variant> variants = new();
        Dictionary<Language, ExerciseLocalization> localizations = new();
        return new(uniqueId, variants, localizations);
    }
}