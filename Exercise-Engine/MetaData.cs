namespace ExerciseEngine;

public class MetaData
{
    public int exerciseId;
    public List<Language> localizations = new();
    public int variantsCount;
    public ExerciseType exerciseType;
    public Dictionary<Language, string> titles = new();
    public Dictionary<Language, string> descriptions = new();
    public List<Topic> topics = new(); // possibly here dict of topics, or dict at place of webserver
    public List<Grade> grades = new();
    public string thumbnailPath = "";
    public int metaVersion;

    public MetaData() { }

    public MetaData(int exerciseId, Language initialLanguage, ExerciseType exerciseType) {
        this.exerciseId = exerciseId;
        localizations.Add(initialLanguage);
        this.exerciseType = exerciseType; 
    }
}
