/*using ExerciseEngine;
using static System.Console;

WriteLine("Hello, World! First exercise");

// instantiate one instance of exercise and factory
E_1_AritemtikaDoStovky exercise_1 = new(); 
F_1_AritmetikaDoStovky factory_1 = new();

// factory creates n instances of variants
factory_1.FilterLegitVariants();

// foreach variant send instance of it to exercise to get concrete representation of it 
List<string> result = new();
foreach (var variant in factory_1.legit) {
	Representation r = exercise_1.GetRepresentation(variant);
	result.Add(r.assignment + "   " + r.questions[0].result);
}

// save the result:
using (StreamWriter sw = new("logA.txt")) {
	foreach (var line in result)
		sw.WriteLine(line);
}

using (StreamWriter sw = new("statsA.txt")) 
	sw.WriteLine(factory_1.ReportStatistics());

// expected to be written by interpreter based on definition:
sealed class V_1_AritmetikaDoStovky : Variant {
	public readonly int left;
	public readonly int right;
	public readonly Operator op1;
	public V_1_AritmetikaDoStovky(int left, int right, Operator op1) {
		this.left = left;
		this.right = right;
		this.op1 = op1;
    }

	public override bool IsLegit(out int constraintId) {
		constraintId = 0;
		if(op1 == Operator.Add && (left + right) > 100)
			return false; // sum should not be over 100

		constraintId++;
		if(op1 == Operator.Sub && right > left)
			return false; // subtraction should end in negative number

		constraintId++; // 2 
		if(op1 == Operator.Mul && (left>10 || right > 10) )
			return false; // both factors can be at most 10

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

sealed class F_1_AritmetikaDoStovky : Factory<V_1_AritmetikaDoStovky> {
	// 4 podminky na legit a ocekavame zkontrolovat 39 204 variant
	public F_1_AritmetikaDoStovky() : base(4, 39_204) { }

	public override void FilterLegitVariants() {
		List<Operator> op1Set = new() { Operator.Add, Operator.Sub, Operator.Mul, Operator.Div };
		WriteLine($"Initiating for loop, expecting to see {expected} variants.");
		for(int left = 2; left <= 100; left++) {
			for(int right = 2; right <= 100; right++) {
				foreach(Operator op1 in op1Set) {
					V_1_AritmetikaDoStovky variant = new(left, right, op1);
					Consider(variant);
				}
            }
        }
	}
}

sealed class E_1_AritemtikaDoStovky : Exercise<V_1_AritmetikaDoStovky> { 
	public E_1_AritemtikaDoStovky():base(true) {
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

		babylon[Language.en] = mr;
	}
}*/