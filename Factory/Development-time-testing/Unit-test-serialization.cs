namespace ExerciseEngine.Factory;

class UnitTestSerialization {
	readonly List<ExerciseCollection> ecList = new();
	public (string jsonA, string jsonB, string original, string deserialized) Strings { get; set;}
	public bool SaveFiles { get; set;} = true; // disable to not print them..

	public UnitTestSerialization() {
		WriteLine($"   ... constructing collections ...");

		List<ManualCollectionBuilder> collectionBuilders = new();
		// A:
		ManualCollectionBuilderA builderA = new();
		collectionBuilders.Add(builderA);

		foreach (var b in collectionBuilders)
			ecList.Add(b.BuildCollection());
	}

	public void Test_Exercise_Collection() {
		foreach(var ec in ecList) 
			Run_Collection(ec);
	}

	void Run_Collection(ExerciseCollection ec) {
		WriteLine($"   ... serializing collection {ec.UniqueId}...");
		SerializationManager unitTest = new();
		Strings = unitTest.BuildStrings(ec);
		if (SaveFiles)
			_SaveFiles_Collections(ec.UniqueId);
	}

	void _SaveFiles_Collections(ulong ecId) {
		using StreamWriter sw = new($"Collection-id-{ecId}-original.txt");
		sw.WriteLine(Strings.original);

		using StreamWriter sw2 = new($"Collection-id-{ecId}-deserialized.txt");
		sw2.WriteLine(Strings.deserialized);

		using StreamWriter sw3 = new($"Collection-id-{ecId}-A.json");
		sw3.WriteLine(Strings.jsonA);
		
		using StreamWriter sw4 = new($"Collection-id-{ecId}-B.json");
		sw4.WriteLine(Strings.jsonB);
	}


	public void Test_List_Exercise() {
		foreach (var ec in ecList)
			foreach (Language lang in Enum.GetValues(typeof(Language)))
				RunList_Exercise(ec, lang);
	}

	void RunList_Exercise(ExerciseCollection ec, Language lang) {
		WriteLine($"   ... serializing List of exercises: {ec.UniqueId} in {lang} lang...");
		// Get list of localized exercises:
		List<Exercise> localizedExercises = ec.GetLocalizedCollection(lang);
		SerializationManager unitTest = new();
		Strings = unitTest.BuildStrings(localizedExercises);

		if (SaveFiles)
			_SaveFiles_ListEx(ec.UniqueId, lang);
	}

	void _SaveFiles_ListEx(ulong ecId, Language lang) {
		using StreamWriter sw = new($"List-Exercise-id-{ecId}-{lang}-original.txt"); 
		sw.WriteLine(Strings.original);
		
		using StreamWriter sw2 = new($"List-Exercise-id-{ecId}-{lang}-deserialized.txt");
		sw2.WriteLine(Strings.deserialized);
		
		using StreamWriter sw3 = new($"List-Exercise-id-{ecId}-{lang}-A.json");
		sw3.WriteLine(Strings.jsonA);
		
		using StreamWriter sw4 = new($"List-Exercise-id-{ecId}-{lang}-B.json");
		sw4.WriteLine(Strings.jsonB);
	}
}

class SerializationManager {
	readonly JsonSerializerOptions options = new() { WriteIndented = true };

	public SerializationManager() {
		options.Converters.Add(new TextElementConverter());
		options.Converters.Add(new ExerciseConverter());
	}

#pragma warning disable CA1822 // Mark members as static
	public (string jsonA, string jsonB, string original, string deserialized) BuildStrings(ExerciseCollection collection) {
		// 1. to string original
		string original = collection.ToString();
		
		// 2. serialize original
		string jsonA = JsonSerializer.Serialize(collection, options);

		// 3. deserialize serialized original and ToString it
		ExerciseCollection? collectionDeserialized = JsonSerializer.Deserialize<ExerciseCollection>(jsonA, options);
		if (collectionDeserialized == null)
			throw new Exception();
		string deserialized = collectionDeserialized.ToString();

		// 4. serialize last time:
		string jsonB = JsonSerializer.Serialize(deserialized, options);

		return (jsonA, jsonB, original, deserialized);
	}

	public (string jsonA, string jsonB, string original, string deserialized) BuildStrings(List<Exercise> localizedListOfExercises) {
#pragma warning restore CA1822
		// 1. to string original
		StringBuilder original = new();
		foreach (Exercise e in localizedListOfExercises)
			original.Append(e.ToString());

		// 2. serialize original 
		string jsonA = JsonSerializer.Serialize(localizedListOfExercises, options);

		// 3. deserialize serialized original and ToString it:
		List<Exercise>? collectionDeserialized = JsonSerializer.Deserialize<List<Exercise>>(jsonA, options);
		if (collectionDeserialized == null)
			throw new Exception();
		StringBuilder deserialized = new();
		foreach (Exercise e in collectionDeserialized)
			deserialized.Append(e.ToString());

		// 4. serialize last time:
		string jsonB = JsonSerializer.Serialize(collectionDeserialized, options);

		return (jsonA, jsonB, original.ToString(), deserialized.ToString());
	}
}