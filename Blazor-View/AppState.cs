using ExerciseEngine;

public class AppState {
    public AppState() {
        InitializeVariables();
    }

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

    string _title = "Very nice math exercise";
    string _description = "Lets children explore the depths of their imagination";
    string _thumbnailFileName = "";
    bool _autoGenerateThumbnail = true;

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

    public bool AutoGenerateThumbnail {
        get => _autoGenerateThumbnail;
        set {
            _autoGenerateThumbnail = value;
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

    public IEnumerable<string> topicOptions = new HashSet<string>() { "Addition", "Multiplication", "Modulo"};
    public IEnumerable<string> gradeOptions = new HashSet<string>() { "Ninth"};

    private List<Bindable_NotPolymorphic_Variable> _variables = new();
    
    void InitializeVariables() {
        Bindable_NotPolymorphic_Variable x, y, z, op1, op2;
        x = new() { name = "A", setRange = SetRange.Range, dataType = DataType.Int, intMin = 1, intMax = 11, intIncrement = 2 };
        y = new() { name = "B", setRange = SetRange.Range, dataType = DataType.Int, intMin = 20, intMax = 50, intIncrement = 1 };
        z = new() { name = "C", setRange = SetRange.Range, dataType = DataType.Double, doubleMin = 2.0, doubleMax = 8.2, doubleIncrement = 0.1 };
        _variables.Add(x);
        _variables.Add(y);
        _variables.Add(z);

        op1 = new() { name = "op1", setRange = SetRange.Set, dataType = DataType.Operator, opElements = new() { Operator.Add, Operator.Sub, Operator.Mul, Operator.Div} };
        op2 = new() { name = "op2", setRange = SetRange.Set, dataType = DataType.Operator, opElements = new() { Operator.Add, Operator.Sub } };
        _variables.Add(op1);
        _variables.Add(op2);
    }

    // !! notify change will not get trigered on normal operations on accessed elements !!
    public List<Bindable_NotPolymorphic_Variable> Variables {
        get => _variables;
        set {
            _variables = value;
            NotifyStateChanged();
		}
	}

	public event Action? OnChange;

    public void NotifyStateChanged() => OnChange?.Invoke();
}