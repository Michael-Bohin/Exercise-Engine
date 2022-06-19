namespace ExerciseEngine;

enum Classes { First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eight, Ninth }
enum Topic { Arithmetic, Addition, Subtraction, Multiplication, Division, Modulo, Fractions, Percentages, Combinatorics, BasicUnits } // and many more..

record Groups {
	public List<Classes> Classes { get; } = new();
	public List<Topic> Topics { get; } = new();
	public Groups(List<Classes> Classes, List<Topic> Topics) {
		this.Classes = Classes;
		this.Topics = Topics;
	}

	public Groups() { }
}
