global using static System.Console;
global using static System.Math;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

namespace ExerciseEngine.Factory;
using System.Text.Json;

class Program {
	static void Main() {
		WriteLine("Hi!"); 
	
		UnitTestSerialization ut = new();
		ut.Test_Exercise_Collection();
		ut.Test_List_Exercise();
		
	}
}