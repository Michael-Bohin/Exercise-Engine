using ExerciseEngine;
using static System.Console;

// the goal here is to test the implementation of exercise engine interpreter: 
// There is target cs code, that it should generate given the definition it recieves..
// 

WriteLine("Hello, World!");

// instantiate one instance of exercise: (figure out how to generate following code: using interpreter or some automated builder? )
// posble apporach, give required fields from definition to ctor... :) 
Macro el1 = new("left");
Text el2 = new(" ");
Macro el3 = new("op1");
Text el4 = new(" ");
Macro el5 = new("right");
Text el6 = new(" = ?");
List<MacroText> assignment = new() { el1, el2, el3, el4, el5, el6 };

MacroRepresentation mr = new();
mr.assignment = assignment;

MacroQuestion mQ = new();
mQ.resultType = ResultType.Int;
mr.questions.Add(mQ);

Exercise<V_1_AritmetikaDoStovky> exercise = new(mr); // monolingual exercise instantiated

// instantiate one instance of factory
F_1_AritmetikaDoStovky f1 = new();

// factory create n instances of variants
f1.FilterLegitVariants();

List<string> result = new();
// foreach variant send instance of it to exercise to get concrete representation of it 
foreach (var variant in f1.legit) {
	Representation r = exercise.GetRepresentation(variant);
	result.Add(r.assignment + "   " + r.questions[0].result);
}

// save the result"
using (StreamWriter sw = new("log.txt")) {
	foreach (var line in result)
		sw.WriteLine(line);
}

using (StreamWriter sw = new("stats.txt")) {	
	sw.WriteLine(f1.ReportStatistics());
}

// expected to be written by interpreter based on definition:
class V_1_AritmetikaDoStovky : Variant {
	public readonly int left;
	public readonly int right;
	public readonly Operator op1;
	public V_1_AritmetikaDoStovky(int left, int right, Operator op1) {
		this.left = left;
		this.right = right;
		this.op1 = op1;
    }

    // provide is legit 
    // provide representation (build the class accordingly.. ) 
    // first in one language, later based on laguage as parameter..
	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		if(op1 == Operator.Add && (left + right) > 100)
			return false; // sum should not be over 100

		constraintId++; // 1
		if(op1 == Operator.Sub && right > left)
			return false; // subtraction should end in negative number

		constraintId++; // 2 
		if(op1 == Operator.Mul && (left>10 || right > 10) )
			return false; // both fastors can be at most 10

		constraintId++; // 3
		if(op1 == Operator.Div && (left % right) != 0) 
			return false;

		return true;
    }

	public override string GetResult(int questionIndex) {
		if(questionIndex != 0)
			throw new ArgumentException("numerical exercise always has one result at index 0");

		int result = 0;
		if (op1 == Operator.Add)
			result = left + right;
		else if (op1 == Operator.Sub)
			result = left - right;
		else if (op1 == Operator.Mul)
			result = left * right;
		else if (op1 == Operator.Div)
			result = left / right;
		return result.ToString();
	}

	public override string VariableRepresentation(string variableName) {
		switch(variableName) {
			case "left": 
				return left.ToString();
			case "right": 
				return right.ToString();	
			case "op1": 
				return OpToChar(op1).ToString();
			default:
				throw new ArgumentException("Variable representation recieved invalid variable name: " + variableName + "\n");
		}
	}
}

class F_1_AritmetikaDoStovky : Factory<V_1_AritmetikaDoStovky> {
	// should provide method that loops over all options
	// internaly
	//	1. builds list of correct exercises
	//  2. keeps incorrect up to first 100 exercises
	//  3. keeps track of statistics, including number of times different constraint have been trigered
	// 	
	// method that returns legit exercises 
	// method that returns illegal exercises

	// constraint count a expected bude psat interpreter jako vysledek vlastniho vypoctu
	// 4 podminky na legit a ocekavame zkontrolovat 39 204 variant
	public F_1_AritmetikaDoStovky() : base(4, 39204) { }

	// figure out good way to measure time for first loop of the most outer loop and print that info:
	// loop lasted: x ms, and there will be y many of them, estimated time:
	public override void FilterLegitVariants() {
		List<Operator> op1Set = new() { Operator.Add, Operator.Sub, Operator.Mul, Operator.Div };
		WriteLine($"Initiating for loop, expecting to see {expected} variants.");
		for(int left = 2; left <= 100; left++) {
			for(int right = 2; right <= 100; right++) {
				foreach(Operator op1 in op1Set) {
					// 0. increment actual, 1. make instance of exercise, 2. check if legit, 3. on legit add to legit, 4. on illegal, add to illegal of not 50 and increment coresponding ilegal log
					V_1_AritmetikaDoStovky variant = new(left, right, op1);
					Consider(variant);
				}
            }
        }
    }
}