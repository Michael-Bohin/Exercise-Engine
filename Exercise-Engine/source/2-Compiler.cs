namespace ExerciseEngine;
using System.Text;
using System.Text.RegularExpressions;

public class Compiler { 
	Definition definition = new();
	public string fileName = "";
	UsageCompiler usageCompiler = new();
	VariantCompiler variantCompiler = new();
	ExerciseCompiler exerciseCompiler = new();
	bool definitionLoaded = false; 

	// !throws erros!, calee should be catching them (especialy validate definition)
	public void LoadDefinition(Definition definition, int uniqueId) {
		if(uniqueId < 0)
			throw new ArgumentException("\nId of exercise must be positive integers! recieved: " + uniqueId.ToString());
		this.definition = definition;
		definitionLoaded = true;

		ValidateDefinition();
		(string variantName, string exerciseName) = FigureOutClassNames(uniqueId);

		fileName = exerciseName;
		usageCompiler = new(this.definition, exerciseName);
		variantCompiler = new(this.definition, variantName);
		exerciseCompiler = new(this.definition, variantName, exerciseName);
	}

	public string Translate() {
		if (!definitionLoaded)
			throw new InvalidOperationException("\nInstance of interpreter does not have an instance of Definition at this point. No definition to interpret.");
		
		StringBuilder code = new();
		code.Append(usageCompiler.Translate() + '\n');
		code.Append(variantCompiler.Translate() + '\n');
		code.Append(exerciseCompiler.Translate());
		return code.ToString();
	}

	// 1. check: that at least one variable is present 
	// 2. check: that all variable names are unique 
	// 3. check: validity of all variable names: they only contain [a-zA-Z0-9_] and dont begin with digit
	// 4. check: the list of questions contains at least one question ... whats the point of solving a problem without a question, right?
	void ValidateDefinition() {
		if(definition.variables.Count == 0)
			throw new InvalidOperationException("Interpreter cannot translate definition that does not have any variables.");

		List<string> names = new();
		foreach(Variable variable in definition.variables) {
			if(names.Contains(variable.Name))
				throw new InvalidOperationException($"Interpreter detected at least one occurence of variables sharing the same name: >>{variable.Name}<<!");
			names.Add(variable.Name);
		} 

		foreach(Variable variable in definition.variables) {
			// notice the not operator before regex match --> all names must fall into the definition of the regex below 
			if ( ! Regex.Match(variable.Name, "^[a-zA-Z_][a-zA-Z_0-9]*$").Success)
				throw new InvalidOperationException($"Invalid Id name: '{variable.Name}' ");
		}

		if(definition.questions.Count == 0)
			throw new InvalidOperationException("Create only exercises with at least one question. Provided definition of exercise contains 0 questions.");
	}

#pragma warning disable IDE0057 // Use range operator
	(string variantName, string exerciseName) FigureOutClassNames(int uniqueId) {
		string title = definition.metaData.title;
		// 1. take only first 10 chars
		// 2. substitute all whitespace characters in title with underscore
		// 3. remove all chars that are not from: [_a-zA-z0-9]
		// -> can start with number, since it will be suffix of class name
		// this rules out any middle European special characters like ěščřžýáíéúůóťďň

		if (title.Length > 10)
			title = title.Substring(0,10);
		title = title.Replace(' ', '_');
		title = Regex.Replace(title, @"[^0-9a-zA-Z_]", "");

		string v = $"Variant_{uniqueId}_{title}";
		string e = $"Exercise_{uniqueId}_{title}";
		return (v , e);
	}
#pragma warning restore IDE0057 // Use range operator
}

abstract class CompilerHelper {
	protected const char quotes = '"', openCurly = '{', closeCurly = '}';
	protected Definition definition = new();
	protected string variantName = "";
	protected string exerciseName = "";
}

class UsageCompiler : CompilerHelper {
	const string usageHeadline = "// >>> 1.: Intended usage <<<";
	public UsageCompiler() { }

	public UsageCompiler(Definition d, string exerciseName) {
		definition = d;
		this.exerciseName = exerciseName;
	}

	public string Translate() {
		StringBuilder sb = new();
		sb.Append("using ExerciseEngine;\n");
		sb.Append("using System.Text.Json.Serialization; // incase json min size is needed\n\n");

		sb.Append($"{usageHeadline}\n\n");

		sb.Append($"{exerciseName} ex = new();\n");
		sb.Append("ex.FilterLegitVariants();\n\n");

		sb.Append("string stats = ex.ReportStatistics();\n");
		sb.Append("string json = ex.SerializeSelf(true);\n\n");

		sb.Append($"using StreamWriter sw1 = new({quotes}stats_{exerciseName}.txt{quotes});\n");
		sb.Append("sw1.Write(stats);\n\n");

		sb.Append($"using StreamWriter sw2 = new({quotes}json_{exerciseName}.json{quotes});\n");
		sb.Append("sw2.Write(json);\n\n");

		return sb.ToString();
	}
}

class VariantCompiler : CompilerHelper {
	const string variantHeadline = "// >>> 2.: class ConcreteVariant <<<";
	public VariantCompiler() { }

	public VariantCompiler(Definition d, string variantName) {
		definition = d;
		this.variantName = variantName;
	}

	public string Translate() {
		string fields = VariantFields();
		string ctor = VariantCtor();
		string isLegit = VariantIsLegit();
		string getResults = VariantGetResults();
		string varRepresentation = VariantVarRep();

		StringBuilder code = new();
		code.Append(fields);
		code.Append(ctor);
		code.Append(isLegit);
		code.Append(getResults);
		code.Append(varRepresentation);
		code.Append("}\n"); // end the entire sealed class Variant
		return code.ToString();
	}

	string VariantFields() {
		StringBuilder sb = new();
		sb.Append($"{variantHeadline}\n\n");
		sb.Append($"sealed class {variantName} : Variant {openCurly}\n");
		foreach (Variable v in definition.variables)
			sb.Append($"\tpublic readonly {v.TypeRepresentation()} {v.Name};\n");

		return sb.ToString();
	}

	string VariantCtor() {
		StringBuilder sb = new();
		// ctor declaration line:
		sb.Append($"\tpublic {variantName}(");
		int count = definition.variables.Count;
		int last = count - 1;
		for (int i = 0; i < count; i++) {
			if (i != 0)
				sb.Append(' ');
			sb.Append(definition.variables[i].TypeRepresentation());
			sb.Append(' ');
			sb.Append(definition.variables[i].Name);
			if (i != last)
				sb.Append(',');
		}
		sb.Append(") {\n");

		foreach (Variable v in definition.variables)
			sb.Append($"\t\tthis.{v.Name} = {v.Name};\n");

		sb.Append("\t}\n\n"); // close ctor and add empty line:
		return sb.ToString();
	}

	string VariantIsLegit() {
		StringBuilder sb = new();
		sb.Append("\tpublic override bool IsLegit(out int constraintId) {\n");
		sb.Append("\t\tconstraintId = 0;\n");

		for (int i = 0; i < definition.constraints.Count; i++) {
			if (i != 0)
				sb.Append("\t\tconstraintId++;\n");
			sb.Append($"\t\tif (Constraint_{i}())\n");
			sb.Append("\t\t\treturn false;\n");
			sb.Append('\n');
		}

		sb.Append("\t\treturn true;\n");
		sb.Append("\t}\n\n");

		for (int i = 0; i < definition.constraints.Count; i++)
			sb.Append(WriteNthConstraintMethod(definition.constraints[i], i));

		return sb.ToString();
	}

	static string WriteNthConstraintMethod(ConstraintMethod cm, int index) {
		StringBuilder sb = new();
		foreach (string commentLine in cm.comments)
			sb.Append($"\t// {commentLine}\n");
		sb.Append($"\tbool Constraint_{index}() ");

		if (cm.codeDefined) {
			sb.Append(MethodBody(cm.code));
		} else {
			sb.Append($"{openCurly}\n\t\t// code has not yet been defined\n\t{closeCurly}\n\n");
		}

		return sb.ToString();
	}

	static string MethodBody(List<string> code) {
		StringBuilder sb = new();
		if (code.Count > 1) {
			sb.Append($"{openCurly}\n");
			foreach (string codeLine in code)
				sb.Append($"\t\t{codeLine}\n");
			sb.Append("\t}\n\n");
		} else if (code.Count == 1) {
			sb.Append($"=> {code[0]}\n\n");
		} else {
			throw new InvalidOperationException("Compiler and its MethodBody method ran into defined code containing 0 lines.");
		}
		return sb.ToString();
	}

	string VariantGetResults() {
		StringBuilder sb = new();
		sb.Append($"\tpublic override string GetResult(int questionIndex) {openCurly}\n");
		if (definition.questions.Count != 1) {
			sb.Append($"\t\tif (questionIndex < 0 || questionIndex > {definition.questions.Count - 1})\n");
		} else {
			sb.Append($"\t\tif (questionIndex != 0)\n");
		}

		sb.Append($"\t\t\tthrow new ArgumentException(" + '"' + $"Index needs to be positive and at most {definition.questions.Count - 1}, index entered: " + '"' + " + questionIndex.ToString());\n\n");

		for (int i = 0; i < definition.questions.Count - 1; i++) {
			sb.Append($"\t\tif (questionIndex == {i})\n");
			sb.Append($"\t\t\treturn GetResult_{i}();\n\n");
		}

		sb.Append($"\t\treturn GetResult_{definition.questions.Count - 1}();\n\t{closeCurly}\n\n");
		for (int i = 0; i < definition.questions.Count; i++)
			sb.Append(WriteNthResultMethod(definition.questions[i], i));

		return sb.ToString();
	}

	static string WriteNthResultMethod(Definition_Question question, int index) {
		StringBuilder sb = new();
		foreach (string commentLine in question.result.comments)
			sb.Append($"\t// {commentLine}\n");

		sb.Append($"\tstring GetResult_{index}() ");
		if (question.result.codeDefined) {
			sb.Append(MethodBody(question.result.code));
		} else {
			sb.Append($"{openCurly}\n\t\t// result code has not yet been defined\n");
		}

		return sb.ToString();
	}

	// I need to find a fix how to handle 
	string VariantVarRep() {
		StringBuilder sb = new();
		sb.Append($"\tpublic override string VariableRepresentation(string variableName) {openCurly}\n");
		sb.Append($"\t\treturn variableName switch {openCurly}\n");

		foreach (Variable variable in definition.variables)
			sb.Append($"\t\t\t{quotes}{variable.Name}{quotes} => {variable.Name}.ToString(),\n");

		sb.Append($"\t\t\t_ => throw new ArgumentException({quotes}Variable representation recieved invalid variable name: {quotes} + variableName + {quotes}\\n{quotes})\n");
		sb.Append("\t\t};\n");
		sb.Append("\t}\n");
		return sb.ToString();
	}
}

class ExerciseCompiler : CompilerHelper {
	const string exerciseHeadline = "// >>> 3.: class ConcreteExercise <<<";
	public ExerciseCompiler() { }

	public ExerciseCompiler(Definition d, string variantName, string exerciseName) {
		definition = d;
		this.variantName = variantName;
		this.exerciseName = exerciseName;
	}

	public string Translate() {
		StringBuilder sb = new();
		sb.Append(DeclareClass_Ctor());
		sb.Append(FactoryFilterLegit());
		sb.Append('}'); // close class ConcreteExercise
		return sb.ToString();
	}

	string DeclareClass_Ctor() {
		string assignment = Build_Assign_Assignment();
		List<string> questions = new();
		for (int i = 0; i < definition.questions.Count; i++)
			questions.Add(Build_Add_Question(definition.questions[i], i));
		string closeClass = Assign_MR_Close_Declaration();

		StringBuilder sb = new();
		sb.Append(exerciseHeadline + '\n' + '\n');
		sb.Append($"sealed class {exerciseName} : Exercise<{variantName}> {openCurly}\n");
		sb.Append(assignment);
		foreach (string question in questions)
			sb.Append(question);
		sb.Append(closeClass);

		return sb.ToString();
	}

	string Build_Assign_Assignment() {
		StringBuilder sb = new();

		return sb.ToString();
	}

	string Build_Add_Question(Definition_Question defQ, int qCounter) {
		StringBuilder sb = new();

		return sb.ToString();
	}

	string Assign_MR_Close_Declaration() {
		StringBuilder sb = new();

		return sb.ToString();
	}

	string FactoryFilterLegit() {
		string SetVariablesListInitialization = InstantiateSetLists(definition.variables);
		string forLoop = BuildForLoop(definition.variables);

		StringBuilder sb = new();
		sb.Append($"\tpublic override void FilterLegitVariants() {openCurly}\n");
		sb.Append(SetVariablesListInitialization);
		sb.Append("\t\tConsole.WriteLine($" + '"' + $"Initiating nested forloops, expecting to see {openCurly}expected{closeCurly} variants." + '"' + ");\n");
		sb.Append(forLoop);
		sb.Append("\t}\n");
		return sb.ToString();
	}

	static string InstantiateSetLists(List<Variable> variables) {
		StringBuilder sb = new();
		foreach (Variable variable in variables)
			if (variable.IsSet()) {
				sb.Append("\t\t");
				sb.Append(variable.GetCodeSetInstantionLine());
				sb.Append('\n');
			}
		return sb.ToString();
	}

	string BuildForLoop(List<Variable> variables) {
		StringBuilder sb = new();
		sb.Append('\n');
		sb.Append(OpenForLoops(variables, out int indentCounter));
		sb.Append(BuildForLoopBody(variables, indentCounter));
		sb.Append(CloseForLoops(indentCounter));
		return sb.ToString();
	}

	static string OpenForLoops(List<Variable> variables, out int indentCounter) {
		indentCounter = 2;
		StringBuilder sb = new();
		foreach (Variable variable in variables) {
			for (int i = 0; i < indentCounter; i++)
				sb.Append('\t');
			sb.Append(variable.GetForLoopLine());
			sb.Append('\n');
			indentCounter++;
		}

		return sb.ToString();
	}

	string BuildForLoopBody(List<Variable> variables, int indentCounter) {
		StringBuilder sb = new();
		for (int i = 0; i < indentCounter; i++)
			sb.Append('\t');

		sb.Append($"{variantName} variant = new(");
		for (int i = 0; i < variables.Count; i++) {
			if (i != 0)
				sb.Append(", ");
			sb.Append(variables[i].Name);
		}

		sb.Append(");\n");
		for (int i = 0; i < indentCounter; i++)
			sb.Append('\t');

		sb.Append("Consider(variant);\n");
		return sb.ToString();
	}

	static string CloseForLoops(int indentCounter) {
		StringBuilder sb = new();
		indentCounter--;
		while (indentCounter > 1) {
			for (int i = 0; i < indentCounter; i++)
				sb.Append('\t');
			sb.Append("}\n");
			indentCounter--;
		}
		return sb.ToString();
	}

#region Translate - class ConcreteFactory
	/*
	string TranslateClassFactory() {
		string ctor = FactoryCtor();
		string filterLegit = FactoryFilterLegit();

		StringBuilder sb = new();
		sb.Append(ctor);
		sb.Append(filterLegit);
		sb.Append("}\n\n"); // end class sealed factory
		return sb.ToString();
	}

	string FactoryCtor() {
		int expectedEventSpace = CountEventSpaceCardinality(definition.variables);
		string classDeclaration = $"sealed class {exerciseName} : Factory<{variantName}> {openCurly}\n";
		string ctorDeclaration = $"\tpublic {exerciseName}() : base({definition.constraints.Count}, {expectedEventSpace}) {openCurly} {closeCurly}\n";

		StringBuilder sb = new();
		sb.Append(classDeclaration);
		sb.Append(ctorDeclaration);
		return sb.ToString();
	}

	static int CountEventSpaceCardinality(List<Variable> variables) {
		// if is set variable: get List<T>.Count
		// else, count occurences from min max inc
		// finally, calculate product of count of all variables. 
		int product = 1;
		foreach(Variable variable in variables)
			product *= variable.GetCardinality();
		return product;
	}*/
#endregion

}