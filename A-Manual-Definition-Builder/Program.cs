using ExerciseEngine;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;

WriteLine("Hello");

ManualDefinitionBuilderA defBuilderA = new();
ManualDefinitionBuilderB defBuilderB = new();
ManualDefinitionBuilderD defBuilderD = new(); // 

Definition defB = defBuilderB.Define("Exercise-B");
Definition defD = defBuilderD.Define("Exercise-D");

Compiler compiler = new();
/*compiler.LoadDefinition(defB, 381_199);
compiler.Translate();
compiler.SaveAsCsFile();*/
//compiler.ExecuteTheCode();

compiler.LoadDefinition(defD, 381_200);
compiler.Translate();
compiler.SaveAsCsFile();
//compiler.ExecuteTheCode();



abstract class DefinitionFactory {
	public DefinitionFactory() { }

	protected char SwapAddSub(Operator o) {
		if(o != Operator.Add && o != Operator.Sub)
			throw new ArgumentException();

		if(o == Operator.Sub)
			return '+';

		return '-';
	}

	public virtual Definition Define(string filePath) {
		Definition d = new();
		d.metaData = GetMetaData();
		d.variables = GetVariables();
		d.assignment = GetAssignment();
		d.constraints = GetConstraints();

		d.questions = GetQuestions();

		WriteTo(d, filePath);
		return d;
	}

	void WriteTo(Definition d, string filePath) {
		var options = new JsonSerializerOptions {
			WriteIndented = true,
			IncludeFields = true
		};
		string json = JsonSerializer.Serialize(d, options);

		using StreamWriter sw = new("Definition-" + filePath + ".json");
		sw.WriteLine(json);
	}

	protected char OperatorToChar(Operator o) {
		if(o == Operator.Add)
			return '+';
		if(o == Operator.Sub)
			return '-';
		if(o == Operator.Mul)
			return '*';

		return '/';
	}

	public abstract Definition_MetaData GetMetaData();
	public abstract List<Variable> GetVariables();
	public abstract List<MacroText> GetAssignment();
	public abstract List<ConstraintMethod> GetConstraints();

	public abstract List<Definition_Question> GetQuestions();
}

class ManualDefinitionBuilderA : DefinitionFactory {

	public ManualDefinitionBuilderA()  { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.Numerical;
		md.title = "Multiplication up to 100";
		md.description = "121 multiplication operations with both factors ranging from 0 to 10. Since multiplication with 1 and 0 is trivial, and the probability of one of those factors occuring in the problem is 40/121 => each third time, which is too often for the trivial case, the probability of either zero or one occcuring in either factor has been decreased to 5% -> each twentieth exercise. See info image to better understand how we modified the uniform event space.";
		md.topics = new() { Topic.Multiplication };
		md.grades = new() { Grade.Third };
		return md;
	}

	public override List<Variable> GetVariables() {
		IntRange x = new("A", 0, 10, 1);
		IntRange y = new("B", 0, 10, 1);
		return new() { x, y };
	}

	public override List<MacroText> GetAssignment() {
		Macro el1 = new("A");
		Text el2 = new(" * ");
		Macro el3 = new("B");
		return new() { el1, el2, el3 };
 	}

	// empty since numerical exercise don't qet to ask questions, the assignment is the question already.
	public override List<Definition_Question> GetQuestions() {
		List<Definition_Question> questions = new();
		Definition_Question q = new();
		q.question = new(); // empty for numeric exercise
		
		ResultMethod method = new() {
			resultType = ResultType.Int,
			codeDefined = true,
			code = new() {
				"return A * B;"
			}
		};
		q.result = method;
		q.resultType = ResultType.Int;
		q.imagePaths = new();

		questions.Add(q);
		return questions;
	}

	// empty since all combinations are legit 
	public override List<ConstraintMethod> GetConstraints() => new();
}

class ManualDefinitionBuilderB : DefinitionFactory {

	public ManualDefinitionBuilderB()  { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.Numerical;
		md.title = "Arithmetic up to 100 - without two digit multiplication and dividing by two digit numbers.";
		md.description = "Mix of addition, subtraction, multiplication and division. Multiplication is limited only to both factors being ten or smaller. Similarily division has limited divisor to be at most ten.";
		md.topics = new() { Topic.Arithmetic, Topic.Addition, Topic.Multiplication, Topic.Subtraction, Topic.Multiplication };
		md.grades = new() { Grade.Second, Grade.Third, Grade.Fourth };
		return md;
	}

	public override List<Variable> GetVariables() {
		OperatorSet op1 = new(
			"op1",
			new () { Operator.Add, Operator.Sub, Operator.Mul, Operator.Div } 
		);

		IntRange leftOperand = new("left", 2, 100, 1);
		IntRange rightOperand = new("right", 2, 100, 1);

		return new() { op1, leftOperand, rightOperand };
	}

	public override List<MacroText> GetAssignment() {
		// Make a simple parser for this!!! It will get string and parse the apropriate macro text representation! 
		// fore example: "{left} {op1} {right} = ?" , will be parsed into:
		Macro el1 = new("left");
		Text el2 = new(" ");
		Macro el3 = new("op1");
		Text el4 = new(" ");
		Macro el5 = new("right");
		Text el6 = new(" = ?");
		return new() { el1, el2, el3, el4, el5, el6 };
	}

	// empty since numerical exercise don't qet to ask questions, the assignment is the question already.
	public override List<Definition_Question> GetQuestions() {
		List<Definition_Question> questions = new();
		Definition_Question q = new();
		q.question = new(); // empty for numeric exercise

		List<string> localCode = new() {
			"if(op1 == Operator.Add)",
			"	return left + right;",
			"",
			"if (op1 == Operator.Sub)",
			"	return left - right;",
			"",
			"if (op1 == Operator.Mul)",
			"	return left * right;",
			"",
			"return left / right;"
		};

		ResultMethod method = new() {
			resultType = ResultType.Int,
			codeDefined = true,
			code = localCode
		};

		q.result = method;
		q.resultType = ResultType.Int;
		q.imagePaths = new();

		questions.Add(q);
		return questions;
	}

	// constraint defines some condition, which if true, makes the variant not legit. 
	public override List<ConstraintMethod> GetConstraints() {
		ConstraintMethod constraintAdd = new();
		constraintAdd.comments = new() {
			"Sum must not be greater than 100."
		};
		constraintAdd.codeDefined = true;
		constraintAdd.code = new() {
			"return (op1 == Operator.Add) && (A + B) > 100;"
		};

		ConstraintMethod constraintSub = new();
		constraintSub.comments = new() {
			"Difference must not be negative number -> (A - B) < 0"
		};
		constraintSub.codeDefined = true;
		constraintSub.code = new() {
			"return (op1 == Operator.Sub) && (A - B) < 0;"
		};

		ConstraintMethod constraintMul = new();
		constraintMul.comments = new() {
			"Both factor can be at most 10 -> (A > 10 || B > 10)"
		};
		constraintMul.codeDefined = true;
		constraintMul.code = new() {
			"return (op1 == Operator.Mul) && (A > 10 || B > 10);"
		};

		ConstraintMethod constraintDiv = new();
		constraintDiv.comments = new() {
			"The division must come without remainder -> A % B != 0"
		};
		constraintDiv.codeDefined = true;
		constraintDiv.code = new() {
			"return (op1 == Operator.Div) && ((A % B) != 0);"
		};

		return new() { constraintAdd, constraintSub, constraintMul, constraintDiv };
	}
}

class ManualDefinitionBuilderD : DefinitionFactory {

	public ManualDefinitionBuilderD()  { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.cs;
		md.type = ExerciseType.WordProblem;
		md.title = "Balonky";
		md.description = "Slovní úloha na procvičení sčítání, pravděpodobnosti a maximum.";
		md.topics = new() { Topic.Arithmetic };
		md.grades = new() { Grade.First, Grade.Third, Grade.Seventh };
		return md;
	}

	public override List<Variable> GetVariables() {
		IntRange pocetCervenych = new("cervene", 2, 30, 1);
		IntRange pocetZelenych = new("zelene", 2, 30, 1);
		IntRange pocetModrych = new("modre", 2, 30, 1);
		return new() { pocetCervenych, pocetZelenych, pocetModrych };
	}

	public override List<MacroText> GetAssignment() {
		Text e1 = new("Jakub nosí batoh a v něm má balónky s různými barvami. Dneska ráno si do batohu dal ");
		Macro e2 = new("cervene");
		Text e3 = new(" červených, ");
		Macro e4 = new("zelene");
		Text e5 = new(" zelených a ");
		Macro e6 = new("modre");
		Text e7 = new(" modrých balonků.");
		return new() { e1, e2, e3, e4, e5, e6, e7 };
	}

	// empty since numerical exercise don't qet to ask questions, the assignment is the question already.
	public override List<Definition_Question> GetQuestions() {
		List<Definition_Question> questions = new() {
			Question_1(), 
			Question_2(), 
			Question_3()
		};

		return questions;
	}

	public Definition_Question Question_1() {
		// question 1 kolik si dal kuba balonku do batohu dohromady
		Definition_Question q = new();
		Text q1_1 = new("Kolik balónků si dal Kuba do batohu dohromady?"); // no constraint... 
		q.question = new() { q1_1 };

		List<string> localCode = new() {
			"(cervene + zelene + modre).ToString();"
		};

		ResultMethod method = new() {
			resultType = ResultType.Int,
			codeDefined = true,
			code = localCode
		};

		q.result = method;
		q.resultType = ResultType.Int;
		q.imagePaths = new();
		return q;
	}

	public Definition_Question Question_2() {
		// question 2 jake je pravdepodobnost, ze si pri nahodnem vyberu vytahne modry balonek? 
		Definition_Question q = new();
		// pravdepodobnost by mela vyjit rozumne, tj. desetine cislo obsahuje nanejvys 2 desetinna mista
		Text q1_1 = new("Jaká je pravděpodobnost, že si Kuba při náhodném výběru vytáhne z batohu modrý balónek?"); 
		q.question = new() { q1_1 };

		List<string> localCode = new() {
			"double Px = modre / (double)(cervene + zelene + modre);",
			"return Px.ToString();" 
		};

		ResultMethod method = new() {
			resultType = ResultType.Double,
			codeDefined = true,
			code = localCode
		};

		q.result = method;
		q.resultType = ResultType.Double;
		q.imagePaths = new();
		return q;
	}

	public Definition_Question Question_3() {
		// question 1 kolik si dal kuba balonku do batohu dohromady
		Definition_Question q = new();
		// pro jednoduchost chceme jednoznačně maximum -> maximum je prave jedno.
		Text q1_1 = new("Kterých balonků má Kuba v batohu nejvíce? a) červených, b) modrých nebo c) zelených?");
		q.question = new() { q1_1 };

		List<string> localCode = new() {
			"int max = Math.Max(cervene, zelene);",
			"max = Math.Max(max, modre);", 
			"if (max == modre)", 
			"\treturn " + '"' + 'b' + '"' + ';',
			"",
			"if (max == zelene)",
			"\treturn " + '"' + 'c' + '"' + ';',
			"",
			"return " + '"' + 'a' + '"' + ';'
		};

		ResultMethod method = new() {
			resultType = ResultType.Select,
			codeDefined = true,
			code = localCode
		};

		q.result = method;
		q.resultType = ResultType.Select;
		q.imagePaths = new();
		return q;
	}

	public override List<ConstraintMethod> GetConstraints() {
		ConstraintMethod constraintOtazkaB = new();
		constraintOtazkaB.comments = new() {
			"vysledek otazky B ma nanejvys 2 desetinna mista"
		};
		constraintOtazkaB.codeDefined = true;
		constraintOtazkaB.code = new() {
			"string result = GetResult(1);", 
			"int index = result.IndexOf('.');", 
			"int length = result.Length;",
			"length -= index;",
			"length--;",
			"return length > 2;"
		};

		ConstraintMethod constraintJednoMaximum = new();
		constraintJednoMaximum.comments = new() {
			"maximum je prave jedno"
		};
		constraintJednoMaximum.codeDefined = true;
		constraintJednoMaximum.code = new() {
			"int max = Math.Max(cervene, zelene);",
			"max = Math.Max(max, modre);",
			"int counter = 0;",
			"if (max == cervene) counter++;",
			"if (max == zelene) counter++;",
			"if (max == modre) counter++;",
			"return counter > 1;" // if counter is different from 1, that means that there is more than one maximum, which too bad for this variant
		};

		return new() { constraintOtazkaB , constraintJednoMaximum };

	}
}


/*class ManualDefinitionBuilderE : DefinitionFactory {

	public ManualDefinitionBuilderE() { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.WordProblem;
		md.title = "";
		md.description = "";
		md.topics = new() { };
		md.grades = new() { };
		return md;
	}

	public override List<Variable> GetVariables() {
		throw new NotImplementedException();
	}

	public override List<MacroText> GetAssignment() {
		throw new NotImplementedException();
	}

	public override List<Definition_Question> GetQuestions() {
		throw new NotImplementedException();
	}

	public override List<ConstraintMethod> GetConstraints() {
		throw new NotImplementedException();
	}
}*/