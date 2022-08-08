namespace ExerciseEngine;

struct ExerciseUniqueId {
	[JsonPropertyName("id")]
	public int id;
	[JsonPropertyName("lang")]
	public Language language;
	[JsonPropertyName("variant")]
	public int variant;

	public ExerciseUniqueId(LocalizationUniqueId lui, int variant) {
		id = lui.id;
		language = lui.language;
		this.variant = variant;
	}

	[JsonConstructor]
	public ExerciseUniqueId(int id, Language language, int variant) {
		this.id = id;
		this.language = language;
		this.variant = variant;
	}
}

struct LocalizationUniqueId {
	[JsonPropertyName("id")]
	public int id;
	[JsonPropertyName("lang")]
	public Language language;

	public LocalizationUniqueId(int id, Language language) {
		this.id = id;
		this.language = language;
	}
}

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(MetaData), typeDiscriminator: 0)]
[JsonDerivedType(typeof(DefinitionMetaData), typeDiscriminator: 1)]
[JsonDerivedType(typeof(LocalizationMetaData), typeDiscriminator: 2)]
[JsonDerivedType(typeof(ExerciseMetaData), typeDiscriminator: 3)]
class MetaData {
	[JsonPropertyName("name")]
	public string name;
	[JsonPropertyName("topics")]
	public List<Topic> topics;
	[JsonPropertyName("classes")]
	public List<Classes> classes;
	[JsonPropertyName("type")]
	public ExerciseType type;

	[JsonConstructor]
	public MetaData() { // json serializer ctor
		name = default!;
		topics = new();
		classes = new();
	}

	public MetaData(string name, List<Topic> topics, List<Classes> classes, ExerciseType type) {
		this.name = name; this.topics = topics; this.classes = classes; this.type = type;
	}
}

class DefinitionMetaData : MetaData {
	[JsonPropertyName("originalLang")]
	public Language originalLanguage;

	[JsonConstructor]
	public DefinitionMetaData() { } // json serializer ctor

	public DefinitionMetaData(string name, List<Topic> topics, List<Classes> classes, ExerciseType type, Language originalLanguage) : base(name, topics, classes, type) {
		this.originalLanguage = originalLanguage;
	}
}

class LocalizationMetaData : MetaData {
	[JsonPropertyName("id")]
	public LocalizationUniqueId id;

	[JsonConstructor]
	public LocalizationMetaData() { } // json serializer ctor

	public LocalizationMetaData(LocalizationUniqueId id, string name, List<Topic> topics, List<Classes> classes, ExerciseType type) : base(name, topics, classes, type) {
		this.id = id;
	}
}

class ExerciseMetaData : MetaData {
	[JsonPropertyName("id")]
	public ExerciseUniqueId id;

	// does json seriliazer realy need this ctor? 
	public ExerciseMetaData() { } // json serializer ctor

	[JsonConstructor]
	public ExerciseMetaData(ExerciseUniqueId id, string name, List<Topic> topics, List<Classes> classes, ExerciseType type) : base(name, topics, classes, type) {
		this.id = id;
	}

	public ExerciseMetaData(LocalizationMetaData lmd, int variant) : base(lmd.name, lmd.topics, lmd.classes, lmd.type) {
		id = new(lmd.id, variant);
	}
}