namespace Exercise_Engine;

enum Classes { First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eight, Ninth }
enum Topic { Arithmetic, Addition, Subtraction, Multiplication, Division, Modulo, Fractions, Percentages, Combinatorics, BasicUnits } // and many more..

class Groups {
	public List<Classes> classes;
	public List<Topic> topics;
	public Groups() {
		classes = new();
		topics = new();
	}
}
