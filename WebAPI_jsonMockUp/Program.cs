using ExerciseEngine;
using System.Text.Json;
using System.Text.Json.Serialization;

var options = new JsonSerializerOptions {
	WriteIndented = true,
	IncludeFields = true
};

// create list of all exercises:
Exercise_MetaData prvniPriklad = new() { 
	uniqueId = 1,
	localizations = new() { Language.en, Language.pl, Language.cs, Language.ua},
	variantsCount = 121,
	exerciseType = ExerciseType.Numerical, 
	titles = new() {
		{ Language.en, "Multiplication up to 100"},
		{ Language.cs, "Násobení do 100"},
		{ Language.pl, "Polské násobení do 100"},
		{ Language.ua, "Ukrajinské násobení navíc v azbuce 100"}
	}, 
	descriptions = new() {
		{ Language.en, "121 multiplication operations with both factors ranging from 0 to 10. Since multiplication with 1 and 0 is trivial, and the probability of one of those factors occuring in the problem is 40/121 => each third time, which is too often for the trivial case, the probability of either zero or one occcuring in either factor has been decreased to 5% -> each twentieth exercise. See info image to better understand how we modified the uniform event space."},
		{ Language.cs, "Hezky cesky popisek"},
		{ Language.pl, "Hezky polsky popisek"},
		{ Language.ua, "Hezky UA popisek navíc v azbuce"}
	}, 
	topics = new() {
		Topic.Multiplication
	},
	grades = new() {
		Grade.Second, Grade.Third
	}, 
	thumbnailPath = "cestaKSouboru.jpg", 
	metaVersion = 1
};

Exercise_MetaData druhyPriklad = new() {
	uniqueId = 2,
	localizations = new() { Language.en, Language.pl, Language.cs, Language.ua },
	variantsCount = 8500,
	exerciseType = ExerciseType.Numerical,
	titles = new() {
		{ Language.en, "Arithmetic up to 100 - without two digit multiplication and dividing by two digit numbers."},
		{ Language.cs, "Aritmetika do 100"},
		{ Language.pl, "Polská aritmetika do 100"},
		{ Language.ua, "Ukrajinská aritmetika navíc v azbuce 100"}
	},
	descriptions = new() {
		{ Language.en, "Mix of addition, subtraction, multiplication and division. Multiplication is limited only to both factors being ten or smaller. Similarily division has limited divisor to be at most ten."},
		{ Language.cs, "Hezky cesky popisek o aritemtice"},
		{ Language.pl, "Hezky polsky popisek o aritemtice"},
		{ Language.ua, "Hezky UA popisek navíc v azbuce o aritemtice"}
	},
	topics = new() {
		Topic.Addition, Topic.Subtraction, Topic.Multiplication, Topic.Division, Topic.Arithmetic
	},
	grades = new() {
		Grade.Second, Grade.Third, Grade.Fourth
	},
	thumbnailPath = "jinaCestaKSouboru.jpg",
	metaVersion = 1
};

List<Exercise_MetaData> exerciseCollection = new();
exerciseCollection.Add(prvniPriklad);
exerciseCollection.Add(druhyPriklad);
string json = JsonSerializer.Serialize(exerciseCollection, options);
using StreamWriter swX = new("ExerciseCollectionInfo.json");
swX.WriteLine(json);

// get prikaldy:

Exercise_Representation rep1 = new() {
	assignment = "5 * 8 = ?", 
	questions = new(),
	results = new() { "40" },
	solutionSteps = new(), 
	imagePaths = new()
};

Exercise_Representation rep2 = new() {
	assignment = "3 * 4 = ?",
	questions = new(),
	results = new() { "12" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation rep3 = new() {
	assignment = "9 * 7 = ?",
	questions = new(),
	results = new() { "63" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation rep4 = new() {
	assignment = "8 * 8 = ?",
	questions = new(),
	results = new() { "64" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation rep5 = new() {
	assignment = "6 * 2 = ?",
	questions = new(),
	results = new() { "12" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation rep6 = new() {
	assignment = "5 * 6 = ?",
	questions = new(),
	results = new() { "30" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation rep7 = new() {
	assignment = "2 * 9 = ?",
	questions = new(),
	results = new() { "18" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation rep8 = new() {
	assignment = "7 * 7 = ?",
	questions = new(),
	results = new() { "49" },
	solutionSteps = new(),
	imagePaths = new()
};

List<Exercise_Representation> GetExercisesA = new() {
	rep1, rep2, rep3, rep4, rep5, rep6, rep7, rep8
};

json = JsonSerializer.Serialize(GetExercisesA, options);
using StreamWriter getA = new("GetExercise_1_en_8.json");
getA.WriteLine(json);

// get prikaldy podruhe:

Exercise_Representation r1 = new() {
	assignment = "34 + 28 = ?",
	questions = new(),
	results = new() { "62" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r2 = new() {
	assignment = "72 / 8 = ?",
	questions = new(),
	results = new() { "9" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r3 = new() {
	assignment = "5 * 7 = ?",
	questions = new(),
	results = new() { "35" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r4 = new() {
	assignment = "90 - 38 = ?",
	questions = new(),
	results = new() { "52" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r5 = new() {
	assignment = "46 + 11 = ?",
	questions = new(),
	results = new() { "57" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r6 = new() {
	assignment = "44 + 27 = ?",
	questions = new(),
	results = new() { "61" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r7 = new() {
	assignment = "27 / 9 = ?",
	questions = new(),
	results = new() { "3" },
	solutionSteps = new(),
	imagePaths = new()
};

Exercise_Representation r8 = new() {
	assignment = "18 - 7 = ?",
	questions = new(),
	results = new() { "11" },
	solutionSteps = new(),
	imagePaths = new()
};

List<Exercise_Representation> GetExercisesB = new() {
	r1, r2, r3, r4, r5, r6, r7, r8
};

json = JsonSerializer.Serialize(GetExercisesB, options);
using StreamWriter getB = new("GetExercise_2_cs_8.json");
getB.WriteLine(json);



EnumList el = new();
json = JsonSerializer.Serialize(el, options);
using StreamWriter sw = new("EnumList.json");
sw.WriteLine(json);

