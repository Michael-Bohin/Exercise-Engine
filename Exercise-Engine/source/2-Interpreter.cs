namespace ExerciseEngine;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

public class Interpreter { 
	Definition definition = new();
	int uniqueId = -1;
	bool definitionLoaded = false;
	string translatedCode = "";
	string variantName = "";
	string factoryName = "";
	string exerciseName = "";

	// const strings for simpler interpreter code:
	const string openCurly = "{";
	const string closeCurly = "}";

	public void LoadDefinition(Definition definition, int uniqueId) {
		if(uniqueId < 0)
			throw new ArgumentException("\nId of exercise must be positive integers! recieved: " + uniqueId.ToString());

		this.definition = definition;
		this.uniqueId = uniqueId;
		definitionLoaded = true;
	}

	// !throws erros!, calee should be catching them (especialy validate definition)

	public void Translate() {
		if (!definitionLoaded)
			throw new InvalidOperationException("\nInstance of interpreter does not have an instance of Definition at this point. No definition to interpret.");

		ValidateDefinition();
		FigureOutClassNames();
		string variantCode = TranslateClassVariant();
		string factoryCode = TranslateClassFactory();
		string exerciseCode = TranslateClassExercise();
		
		StringBuilder code = new();
		code.Append(variantCode);
		code.Append('\n');
		code.Append(factoryCode);
		code.Append('\n');
		code.Append(exerciseCode);
		translatedCode = code.ToString();
	}


#region Translate Helper methods

	void ValidateDefinition() {
		// 1. check that at least one variable is present 
		// 2. check that all variable names are unique 
		// 3. check validity of all variable names: they only contain [a-zA-Z0-9_] and dont begin with digit
		// 4. check: the list of questions contains at least one question ... whats the point of solving a problem without a question, right?

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
	void FigureOutClassNames() {
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

		variantName =  $"Variant_{uniqueId}_{title}";
		factoryName = $"Factory_{uniqueId}_{title}";
		exerciseName = $"Exercise_{uniqueId}_{title}";
	}
#pragma warning restore IDE0057 // Use range operator

	string TranslateClassVariant() {
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
		return code.ToString();
	}

	string VariantFields() {
		StringBuilder sb = new();
		sb.Append($"sealed class {variantName} : Variant {openCurly}\n");
		foreach(Variable v in definition.variables) 
			sb.Append($"\tpublic readonly {VariableTypeRepr(v)} {v.Name};\n");

		return sb.ToString();
	}

	// ! move this to variable class (first figure out how it can be done syntactically..)
	string VariableTypeRepr(Variable v) {
		if (v is Range<int> || v is Set<int>) 
			return "int";
		
		if (v is Range<double> || v is Set<double>) 
			return "double";
		
		if (v is Set<Operator>) 
			return "Operator";

		throw new InvalidOperationException($"Interpreter ran into unknown variable type: >>{v.Name}<<!");
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
			sb.Append(VariableTypeRepr(definition.variables[i]));
			sb.Append(' ');
			sb.Append(definition.variables[i].Name);
			if (i != last)
				sb.Append(',');
		}
		sb.Append(") {\n");

		// asign this.var = var lines:
		foreach(Variable v in definition.variables) 
			sb.Append($"\t\tthis.{v.Name} = {v.Name};\n");

		// close ctor and add empty line:
		sb.Append("\t}\n\n");

		return sb.ToString();
	}

	string VariantIsLegit() {
		StringBuilder sb = new();
		sb.Append("\tpublic override bool IsLegit(out int constraintId) {\n");
		sb.Append("\t\tconstraintId = 0;\n");


		for (int i = 0; i < definition.constraints.Count; i++) {
			if(i != 0) 
				sb.Append("\t\tconstraintId++;\n");
			sb.Append($"\t\tif (Constraint_{i}()) \n");
			sb.Append("\t\t\treturn false;\n");
			sb.Append('\n');
		}

		sb.Append("\t\treturn true;\n");
		sb.Append("\t}\n\n");

		for(int i = 0; i < definition.constraints.Count; i++) 
			sb.Append(WriteNthConstraintMethod(definition.constraints[i], i));
		
		return sb.ToString();
	}

	string WriteNthConstraintMethod(ConstraintMethod cm, int index) {
		StringBuilder sb = new();
		
		foreach(string commentLine in cm.comments)
			sb.Append($"\t// {commentLine}\n");

		sb.Append($"\tbool Constraint_{index}() {openCurly}\n");
		if(cm.codeDefined) {
			foreach (string codeLine in cm.code)
				sb.Append($"\t\t{codeLine}\n");
		} else {
			sb.Append($"\t\t// code has not yet been defined\n");
		}

		sb.Append("\t}\n\n");

		return sb.ToString();
	}

	string VariantGetResults() {
		StringBuilder sb = new();

		sb.Append($"\tpublic override string GetResult(int questionIndex) {openCurly}\n");
		if(definition.questions.Count != 1) { 
			sb.Append($"\t\tif (questionIndex < 0 || questionIndex > {definition.questions.Count - 1})\n"); 
		} else {
			sb.Append($"\t\tif (questionIndex != 0)\n");
		}

		
		sb.Append($"\t\t\tthrow new ArgumentException(" + '"' + $"Index needs to be positive and at most {definition.questions.Count-1}, index entered: " +  '"' + " + questionIndex.ToString());\n");
		sb.Append("\t\t\n");

		// i-2 since the last can be without if statement
		for(int i = 0; i < definition.questions.Count-1; i++) {
			sb.Append($"\t\tif (questionIndex == {i})\n");
			sb.Append($"\t\t\treturn GetResult_{i}();\n");
		}
		sb.Append("\t\t\n");
		sb.Append($"\t\treturn GetResult_{definition.questions.Count - 1}();\n");

		sb.Append("\t}\n\n");

		for (int i = 0; i < definition.questions.Count; i++)
			sb.Append(WriteNthResultMethod(definition.questions[i], i));

		return sb.ToString();
	}

	string WriteNthResultMethod(Definition_Question question, int index) {
		StringBuilder sb = new();

		foreach (string commentLine in question.result.comments)
			sb.Append($"\t// {commentLine}\n");

		sb.Append($"\tbool GetResult_{index}() {openCurly}\n");
		if (question.result.codeDefined) {
			foreach (string codeLine in question.result.code)
				sb.Append($"\t\t{codeLine}\n");
		} else {
			sb.Append($"\t\t// result code has not yet been defined\n");
		}

		sb.Append("\t}\n\n");

		return sb.ToString();
	}


	// I need to find a fix how to handle 
	string VariantVarRep() {
		StringBuilder sb = new();

		sb.Append($"\tpublic override string VariableRepresentation(string variableName) {openCurly}\n");
		sb.Append($"\t\tswitch (variableName) {openCurly}\n");
		foreach(Variable variable in definition.variables) {
			sb.Append($"\t\t\tcase " + '"' + $"{variable.Name}" + '"' + ":\n");
			sb.Append($"\t\t\t\treturn {variable.Name}.ToString();\n");	
		}

		sb.Append("\t\t\tdefault:\n");
		sb.Append($"\t\t\t\tthrow new ArgumentException(" + '"' + "Variable representation recieved invalid variable name: " + '"' + " + variableName);\n");
		sb.Append("\t\t}\n");
		sb.Append("\t}\n");
		sb.Append("}\n\n"); // end the entire sealed class Variant

		return sb.ToString();
	}



	string TranslateClassFactory() {
		string ctor = FactoryCtor();
		string filterLegit = FactoryFilterLegit();

		StringBuilder code = new();
		code.Append(ctor);
		code.Append(filterLegit);
		return code.ToString();
	}

	
	string FactoryCtor() {
		int expectedEventSpace = CountEventSpaceCardinality(definition.variables);

		StringBuilder sb = new();

		string classDeclaration = $"sealed class {factoryName} : Factory<{variantName}> {openCurly}\n";
		string ctorDeclaration = $"\tpublic {factoryName}() : base({definition.constraints.Count}, {expectedEventSpace}) {openCurly} {closeCurly}";




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
	}

	string FactoryFilterLegit() {
		StringBuilder sb = new();



		return sb.ToString();
	}

	string TranslateClassExercise() {
		// think through the decomposition here...
		string ctorDeclaration = DeclareClass_Ctor_MR();
		string assignment = Build_Assign_Assignment();
		List<string> questions = new();
		for(int i = 0; i < definition.questions.Count; i++) 
			questions.Add(Build_Add_Question(definition.questions[i], i));
		
		string closeClass = Assign_MR_Close_Declaration();
			
		StringBuilder code = new();
		code.Append(ctorDeclaration);
		code.Append(assignment);
		foreach(string question in questions) 
			code.Append(question);
		code.Append(closeClass);
		return code.ToString();
	}

	string DeclareClass_Ctor_MR() {
		StringBuilder sb = new();

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



#endregion

	public void ExecuteTheCode() {
		// Load exercise engine classes required for the code be compilable 
		// Load translated code 
		// Load instructions what to do with the code:
		//		1. generate variants
		//		2. save log
		//		3. save all legit variants 
		//		4. save illgal variants 
		//      5. save the translated code
		WriteLine(translatedCode);
	}

#region Execute the code helper methods

#endregion

}

