namespace ExerciseEngine.Factory;

// Record Groups contains information for teachers to filter exercises by topic, class or exercise
// type. Creating enum for polymorphic structure is antipattern for sure, however here it is done
// on purpose for the needs of Json deserialization process. The deserializer uses the value in
// the enum to tell which instance of Exercise to make.

enum Classes { First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eight, Ninth }
enum Topic { Arithmetic, Addition, Subtraction, Multiplication, Division, Modulo, Fractions, Percentages, Combinatorics, BasicUnits } // and many more..
enum ExerciseType { WordProblem, Numerical, Geometric }

record Groups {
	public List<Classes> Classes { get; } = new();
	public List<Topic> Topics { get; } = new();
	public ExerciseType ExerciseType { get; }
	public Groups(List<Classes> Classes, List<Topic> Topics, ExerciseType ExerciseType) {
		this.Classes = Classes;
		this.Topics = Topics;
		this.ExerciseType = ExerciseType;
	}

	public Groups() { }
}