namespace ExerciseEngine;

abstract internal class Exercise {
	public Factory? Factory = null;
	public Exercise_MetaData MetaData = new();
	public Dictionary<Language, Translation> Translations = new();
	// variants will be written in children by interpretes: List<tuple> variants
	// the tuple type and number of parameters will be different for all
}

abstract internal class Factory {

}

class Translation { }