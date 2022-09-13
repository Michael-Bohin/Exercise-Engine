using ExerciseEngine;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;

WriteLine("Hello");

new ManualDefinitionBuilderA("Exercise-A");
new ManualDefinitionBuilderB("Exercise-B");
abstract class DefinitionFactory {
	public DefinitionFactory(string filePath) {
		Definition d = Define();
		WriteTo(d, filePath);
	}

	protected char SwapAddSub(Operator o) {
		if(o != Operator.Add && o != Operator.Sub)
			throw new ArgumentException();

		if(o == Operator.Sub)
			return '+';

		return '-';
	}

	public virtual Definition Define() {
		Definition d = new();
		d.metaData = GetMetaData();
		d.variables = GetVariables();
		d.assignment = GetAssignment();
		d.questions = GetQuestions();
		d.resultType = GetResultType();
		d.results = GetResults();
		d.constraints = GetConstraints();
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
	public abstract List<MacroText> GetQuestions();
	public abstract ResultType GetResultType();
	public abstract List<ResultMethod> GetResults();
	public abstract List<ConstraintMethod> GetConstraints();
}

class ManualDefinitionBuilderA : DefinitionFactory {

	public ManualDefinitionBuilderA(string filePath) : base(filePath) { }

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
		Range<int> x = new("A", 0, 10, 1);
		Range<int> y = new("B", 0, 10, 1);
		return new() { x, y };
	}

	public override List<MacroText> GetAssignment() {
		Macro el1 = new("A");
		Text el2 = new(" * ");
		Macro el3 = new("B");
		return new() { el1, el2, el3 };
 	}

	// empty since numerical exercise don't qet to ask questions, the assignment is the question already.
	public override List<MacroText> GetQuestions() => new();
	public override ResultType GetResultType() => ResultType.Int;

	/// <summary>
	/// /////////////////// TO DO 
	/// </summary>
	/// <returns></returns>
	public override List<ResultMethod> GetResults() {
		ResultMethod method = new() {
			resultType = GetResultType(),
			codeDefined = true,
			code = new() {
				"return A * B;"
			}
		};
		return new () { method };
	}

	// empty since all combinations are legit 
	public override List<ConstraintMethod> GetConstraints() => new();
}

class ManualDefinitionBuilderB : DefinitionFactory {

	public ManualDefinitionBuilderB(string filePath) : base(filePath) { }

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
		Set<Operator> op1 = new(
			"op1",
			new () { Operator.Add, Operator.Sub, Operator.Mul, Operator.Div } 
		);

		Range<int> leftOperand = new("left", 2, 100, 1);
		Range<int> rightOperand = new("right", 2, 100, 1);

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

	public override List<MacroText> GetQuestions() => new(); // empty

	public override ResultType GetResultType() => ResultType.Int;

	public override List<ResultMethod> GetResults() {
		List<string> localCode = new() {
			"if(op1 == Operator.Add)",
			"	return A + B;",
			"",
			"if (op1 == Operator.Sub)",
			"	return A - B;",
			"",
			"if (op1 == Operator.Mul)",
			"	return A * B;",
			"",
			"return A / B;"
		};

		ResultMethod method = new() {
			resultType = GetResultType(),
			codeDefined = true,
			code = localCode
		};

		return new() { method };
	}

	// constraint defines some condition, which if true, makes the variant not legit. 
	public override List<ConstraintMethod> GetConstraints() {
		ConstraintMethod constraintAdd = new();
		constraintAdd.comments = new() {
			"Sum must not be greater than 100."
		};
		constraintAdd.codeDefined = true;
		constraintAdd.code = new() {
			"return (op1 == Operator.Add) && (A + B) > 100"
		};

		ConstraintMethod constraintSub = new();
		constraintSub.comments = new() {
			"Difference must not be negative number -> (A - B) < 0"
		};
		constraintSub.codeDefined = true;
		constraintSub.code = new() {
			"return (op1 == Operator.Sub) &&(A - B) < 0"
		};

		ConstraintMethod constraintMul = new();
		constraintMul.comments = new() {
			"Both factor can be at most 10 -> (A > 10 || B > 10)"
		};
		constraintMul.codeDefined = true;
		constraintMul.code = new() {
			"return (op1 == Operator.Mul) && (A > 10 || B > 10)"
		};

		ConstraintMethod constraintDiv = new();
		constraintDiv.comments = new() {
			"The division must come without remainder -> A % B != 0"
		};
		constraintDiv.codeDefined = true;
		constraintDiv.code = new() {
			"return (op1 == Operator.Div) && ((A % B) != 0) "
		};

		return new() { constraintAdd, constraintSub, constraintMul, constraintDiv };
	}
}


class ManualDefinitionBuilderC : DefinitionFactory {

	public ManualDefinitionBuilderC(string filePath) : base(filePath) { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.Numerical;
		md.title = "Linear equation with one variable, with addition and subtraction only.";
		md.description = "Solving linear equation with i) one variable only, b) on addition and subtraction, c) on real numbers with at most one digit after decimal dot. That is on real numbers that are multiples of 0.1. Real numbers are from range <0.1, 4.9>, excluding all real numbers which also belong to the set of whole numbers.";
		md.topics = new() { Topic.Addition, Topic.Subtraction };
		md.grades = new() { Grade.Ninth };
		return md;
	}

	public override List<Variable> GetVariables() {
		Range<double> A = new("A", 0.1, 4.9, 0.1);
		Range<double> B = new("B", 0.1, 4.9, 0.1);
		Range<double> C = new("C", 0.1, 4.9, 0.1);
		Range<double> D = new("D", 0.1, 4.9, 0.1);
		Range<double> E = new("E", 0.1, 4.9, 0.1);
		Set<Operator> op1 = new("op1", new() {Operator.Add, Operator.Sub });
		Set<Operator> op2 = new("op1", new() {Operator.Add, Operator.Sub });
		Set<Operator> op3 = new("op1", new() {Operator.Add, Operator.Sub });
		return new() { A, B, C, D, E, op1, op2, op3 };
	}

	public override List<MacroText> GetAssignment() {
		Macro e1  = new("A");
		Text  e2  = new(" ");
		Macro e3  = new("op1");
		Text  e4  = new(" ");
		Macro e5  = new("B");
		Text  e6  = new("x ");
		Macro e7  = new("op2");
		Text  e8  = new(" ");
		Macro e9  = new("C");
		Text  e10 = new(" = ");
		Macro e11 = new("D");
		Text  e12 = new(" ");
		Macro e13 = new("op3");
		Text  e14 = new(" ");
		Macro e15 = new("E");
		Text  e16 = new("x");
		return new() {
			e1,
			e2,
			e3,
			e4,
			e5,
			e6,
			e7,
			e8,
			e9,
			e10,
			e11,
			e12,
			e13,
			e14,
			e15,
			e16
		};
	}

	public override List<MacroText> GetQuestions() => new(); // numerical exercise -> empty 

	public override ResultType GetResultType() => ResultType.Double;

	public override List<ResultMethod> GetResults() {
		ResultMethod md = new();
		
		string line1 = "char char_op2 = SwapAddSub(op2);";
		string line2 = "string citatel = D - A + char_op2 C;"; // think this through again, there is an error like



		// .... figure out after the code is written...


	}

	public override List<ConstraintMethod> GetConstraints() {
		throw new NotImplementedException();
	}
}


class ManualDefinitionBuilderD : DefinitionFactory {

	public ManualDefinitionBuilderD(string filePath) : base(filePath) { }

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

	public override List<MacroText> GetQuestions() {
		throw new NotImplementedException();
	}

	public override ResultType GetResultType() {
		throw new NotImplementedException();
	}

	public override List<ResultMethod> GetResults() {
		throw new NotImplementedException();
	}

	public override List<ConstraintMethod> GetConstraints() {
		throw new NotImplementedException();
	}
}


class ManualDefinitionBuilderE : DefinitionFactory {

	public ManualDefinitionBuilderE(string filePath) : base(filePath) { }

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

	public override List<MacroText> GetQuestions() {
		throw new NotImplementedException();
	}

	public override ResultType GetResultType() {
		throw new NotImplementedException();
	}

	public override List<ResultMethod> GetResults() {
		throw new NotImplementedException();
	}

	public override List<ConstraintMethod> GetConstraints() {
		throw new NotImplementedException();
	}
}

// F is visually too close to E
class ManualDefinitionBuilderG : DefinitionFactory {

	public ManualDefinitionBuilderG(string filePath) : base(filePath) { }

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

	public override List<MacroText> GetQuestions() {
		throw new NotImplementedException();
	}

	public override ResultType GetResultType() {
		throw new NotImplementedException();
	}

	public override List<ResultMethod> GetResults() {
		throw new NotImplementedException();
	}

	public override List<ConstraintMethod> GetConstraints() {
		throw new NotImplementedException();
	}
}




