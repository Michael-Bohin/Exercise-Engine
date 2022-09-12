using ExerciseEngine;
using static System.Console;
using System.Text.Json;
using System.Text.Json.Serialization;

WriteLine("Hello");

ManualDefinitionBuilderA builderA = new();
Definition d = builderA.Define();

var options = new JsonSerializerOptions { 
	WriteIndented = true, 
	IncludeFields = true
};
string s = JsonSerializer.Serialize(d, options);
WriteLine(s);



abstract class DefinitionFactory {
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

	public abstract Definition_MetaData GetMetaData();
	public abstract List<Variable> GetVariables();
	public abstract List<MacroText> GetAssignment();
	public abstract List<MacroText> GetQuestions();
	public abstract ResultType GetResultType();
	public abstract List<ResultMethod> GetResults();
	public abstract List<ConstraintMethod> GetConstraints();
}

class ManualDefinitionBuilderA : DefinitionFactory {

	public ManualDefinitionBuilderA() { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.Numerical;
		md.title = "Multiplication up to 100";
		md.description = "121 multiplication operations with both factors ranging from 0 to 10. Since multiplication with 1 and 0 is trivial, and the probability of one of those factors occuring in the problem is 40/121 => each third time, which is too often for the trivial case, the probability of either zero or one occcuring in either factor has been decreased to 5% -> each twentieth exercise. See info image for all probabilites.";
		md.topics = new() { Topic.Multiplication };
		md.grades = new() { Grade.Third };
		return md;
	}

	public override List<Variable> GetVariables() {
		RangeInt x = new("A", 0, 10, 1);
		RangeInt y = new("B", 0, 10, 1);
		return new() { x, y};
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
		ResultMethod method = new();
		method.resultType = GetResultType();
		method.codeDefined = true;
		List<string> c = new();
		c.Add("return A * B;");
		method.code = c;
		return new () { method };
	}

	// empty since all combinations are legit 
	public override List<ConstraintMethod> GetConstraints() => new();
}

class ManualDefinitionBuilderB : DefinitionFactory {

	public ManualDefinitionBuilderB() { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.Numerical;
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


class ManualDefinitionBuilderC : DefinitionFactory {

	public ManualDefinitionBuilderC() { }

	public override Definition_MetaData GetMetaData() {
		Definition_MetaData md = new();
		md.initialLanguage = Language.en;
		md.type = ExerciseType.Numerical;
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


class ManualDefinitionBuilderD : DefinitionFactory {

	public ManualDefinitionBuilderD() { }

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

	public ManualDefinitionBuilderG() { }

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




