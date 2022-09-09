using ExerciseEngine;

public class AppState {



    // initial lang:
    private Language _initialLangugage = Language.en;
    public Language InitialLanguage {
        get => _initialLangugage;
        set {
            _initialLangugage = value;
            NotifyStateChanged();
        }
    }

    Dictionary<Language, string> dict = new() { { Language.en, "English" }, { Language.pl, "Polish" }, { Language.ua, "Ukrainian" }, { Language.cs, "Czech" } };
    public string EnumToEnglish() => dict[_initialLangugage];
    public string EnumToEnglish(Language l) => dict[l];

    // exercise type:
    private ExerciseType _exerciseType = ExerciseType.Numerical;
    public ExerciseType ExerciseType {
        get => _exerciseType;
        set {
            _exerciseType = value;
            NotifyStateChanged();
        }
    }

    string _title = "";
    string _description = "";
    string _thumbnailFileName = "";

    public string Title {
        get => _title;
        set {
            _title = value;
            NotifyStateChanged();
        }
    }

    public string Description {
        get => _description;
        set {
            _description = value;
            NotifyStateChanged();
        }
    }

    public string ThumbnailFileName {
        get => _thumbnailFileName;
        set {
            _thumbnailFileName = value;
            NotifyStateChanged();
        }
    }

    // string message behaviour:
    private string _message = "";
    public string Message {
        get => _message;
        set {
            _message = value;
            NotifyStateChanged();
        }
    }

    // int your number behaviour:
    private int _creationPhase = 1;
    public int CreationPhase {
        get => _creationPhase;
        set {
            _creationPhase = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}