using ExerciseEngine;
using static System.Console;

// the goal here is to test the implementation of exercise engine interpreter: 
// There is target cs code, that it should generate given the definition it recieves..
// 

WriteLine("Hello, World!");

F_S02_AritmetikaDoStovky f1 = new();
f1.FilterLegitVariants();

List<string> result = new List<string>();

foreach(var legitExercise in f1.legit) {
	Representation r = legitExercise.GetRepresentation();
	string save = r.assignment + "    " + r.questions[0].result;
	result.Add(save);
}

using (StreamWriter sw = new("log.txt")) {
	foreach (var line in result)
		sw.WriteLine(line);
}

using (StreamWriter sw = new("stats.txt")) {	
	sw.WriteLine(f1.ReportStatistics());
}





// expected to be written by interpreter based on definition:
class S02_AritmetikaDoStovky : Exercise {
	public readonly int left;
	public readonly int right;
	public readonly Operator op1;
	public S02_AritmetikaDoStovky(int left, int right, Operator op1) {
		this.left = left;
		this.right = right;
		this.op1 = op1;
    }

    // provide is legit 
    // provide representation (build the class accordingly.. ) 
    // first in one language, later based on laguage as parameter..

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		if(op1 == Operator.Add && (left + right) > 101)
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

    public override Representation GetRepresentation() {
		string assignment = $"{left} {OpToChar(op1)} {right} = ?";
		int result = 0;

		if (op1 == Operator.Add)
			result = left + right;

		if (op1 == Operator.Sub)
			result = left - right;

		if (op1 == Operator.Mul)
			result = left * right;

		if (op1 == Operator.Div)
			result = left / right;

		Question q = new($"{result}", ResultType.Int);
		
		Representation r = new(assignment);
		r.questions.Add(q);
		return r;
    }
}

class F_S02_AritmetikaDoStovky : Factory {
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
	public F_S02_AritmetikaDoStovky() : base(4, 39204) { 
		for(int i = 0; i < constraintCount; i++) {
			illegal.Add(new());
		}
	}

	public List<S02_AritmetikaDoStovky> legit = new();
	public List<List<S02_AritmetikaDoStovky>> illegal = new();

	public override void FilterLegitVariants() {
		List<Operator> op1Set = new() { Operator.Add, Operator.Sub, Operator.Mul, Operator.Div };
		WriteLine($"Initiating for loop, expecting to see {expected} variants.");
		for(int left = 2; left <= 100; left++) {
			WriteLine($"{actual}");
			for(int right = 2; right <= 100; right++) {
				foreach(Operator op1 in op1Set) {
					// 0. increment actual, 1. make instance of exercise, 2. check if legit, 3. on legit add to legit, 4. on illegal, add to illegal of not 50 and increment coresponding ilegal log
					S02_AritmetikaDoStovky ex = new(left, right, op1);
					actual++;
					if (ex.IsLegit(out int constraintId)) {
						legit.Add(ex);
					} else {
						constraintLog[constraintId]++;
						if (illegal[constraintId].Count < 50)
							illegal[constraintId].Add(ex);
					}
				}
            }
        }
    }
}