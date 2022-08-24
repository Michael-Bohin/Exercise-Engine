namespace ExerciseEngine;

// 1 exercise, 1 language, 1 variant
// return types to be thought through later
interface IRepresentation {
	string GetType();
	string GetAssignment();
    List<string> GetQuestions();
    List<string> GetResults();
    List<string> GetSolutionSteps();
}

// 1 exercise, X languages, Y variants
interface IExercise
{
	Representation GetExercise(Language lang, int index);
	Representation GetExercise(Language lang); // gets random variant in given language
    List<Representation> GetAllVariants(Language lang);
    List<Representation> GetSomeVariants(Language lang, int count);
}

// X exercises, Y languages, Z variants
interface IExerciseCollection {
	Exercise GetExercise(int unigueId);
}

interface IExerciseEngineAPI { 
    string GetAllTranslations(Language lang);
    string GetVariants(int exerciseUniqueId, int count); 
    string GetTopics();
}
// this interface serves the http request from web server 
// Throws erros if:
// 1. Language does not exist 
// 2. Exercise unique id does not exist (negative number or greater than greatest id)
// 3. Count is smaller than one (must return at least one exercise)
// 4. Count is greater than count of all variants for given exercise (all exercise will have different number of variants)
// 5. Count is greater than maximum number of exercise held in memory at one time. Say 1000 for begining. 
// In all cases above the http request returns with code 5xx. 
// 
// Always returns serialized classes to JSON, because the webserver will run in node.js language 
//
// Always returns random sequence of variants such that all variants occur at most once.
// That is variants do not repeat. 
