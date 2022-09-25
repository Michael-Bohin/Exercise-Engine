using System.Text;

namespace ExerciseEngine;

// zatim nemichat jazyky, udelat jen pro cestinu!
// protected Dictionary<Language, Description> translations = new();

abstract public class Exercise {
	// exercise shouldnt be aware of its metadata! but where do I put them??
	protected Exercise_MetaData metaData = new();	
	// variants will be written in children by interpreters: List<tuple> variants
	// the tuple type and number of parameters will be different for all
	public Representation GetRandom(Language lang) {
		Representation repr = new("ahoj");

		return repr;
	}

	public int Count { 
		get => metaData.variantsCount;
	}
	
	protected char OpToChar(Operator op) {
		switch (op) {
			case Operator.Add: 
				return '+'; 
			case Operator.Sub:
				return '-';
			case Operator.Mul:
				return '*';
			case Operator.Div:
				return '/';
			default:
				throw new ArgumentException("OperatorToChar");
		}
	}

	public abstract bool IsLegit(out int constraintId);
	public abstract Representation GetRepresentation();
}

abstract public class Factory {
	protected int constraintCount, expected, actual;
	protected List<int> constraintLog = new(); // stores number of times the constraint with specific id has been triggered
	// public List<List<Exercise>> illegal = new();

	public Factory(int constraintCount, int expected) {
		this.constraintCount = constraintCount;
		this.expected = expected;
		actual = 0;
		for(int i = 0; i < constraintCount; i++) 
			constraintLog.Add(0);
    }

	public abstract void FilterLegitVariants();

	public string ReportStatistics() {
		StringBuilder sr = new();
		sr.Append($"Expected variants count: {expected}\n");
		sr.Append($"Actual variants instantiated: {actual}\n");
		sr.Append($"Number of constraints: {constraintLog.Count}\n");
		if(constraintLog.Count > 0) 
			for(int i =0 ; i < constraintLog.Count; i++) 
				sr.Append($"{i}, occurences: {constraintLog[i]}\n");
		return sr.ToString();
    }
}






/// <summary>
/// //////////////////////////////////////////////////////////////////
/// </summary>
// -> macro text pointing at variables in variants for: 
// ->	i.	 assignment
// ->	ii.  questions
//		iii. results
//		iv.  resultTypes
//		v.   solutionsSteps
//		vi.	 imagePaths

// notice: no constraints and no results mehtods..
public class Description {

	
}