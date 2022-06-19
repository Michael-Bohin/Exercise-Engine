Dialog d = new();
d.TalkWithUser();

class Dialog {
	private SbirkaPrikladu sp = new();

	public void TalkWithUser() {
		sp = new();

		while(true) {
			int usersChoice = WhichExercise();
			if(usersChoice < 0 || sp.Count <= usersChoice) {

			} else {
				Exercise e = sp.priklady[usersChoice];
				Console.WriteLine(e.GetAssignment());
				string answer = Console.ReadLine()!;
				string expected = e.GetResult();
				if(answer == expected) {
					Console.WriteLine("Correct!");
				} else {
					Console.WriteLine("Bad answer!");
				}
			}

			Console.WriteLine("Quit or continue? (Type 'q' to quit other wise anything else.)");
			string command = Console.ReadLine()!;
			if(command == "q")
				break;
		}
		Console.WriteLine("Good bye!");
	}

	private static int WhichExercise() {
		Console.WriteLine("Which exercise number would you like to do?");
		string answer = Console.ReadLine()!;
		return int.Parse(answer);
	}
}

class SbirkaPrikladu {
	public  List<Exercise> priklady = new();

	public int Count { get { return priklady.Count;} }

	public SbirkaPrikladu() {
		priklady.Add(new Nasobeni());
		priklady.Add(new Scitani());
	}
}

abstract class Exercise { 
	public abstract string GetResult();
	public abstract string GetAssignment();
}

class Nasobeni : Exercise {
	public int A = 2;
	public int B = 6;

	override public string GetResult() { return $"{A * B}";}
	override public string GetAssignment() { return $"Kolik je {A} * {B} = ?";}
}

class Scitani : Exercise {
	public int A = 2;
	public int B = 3;

	override public string GetResult() { return $"{A + B}"; }
	override public string GetAssignment() { return $"Kolik je {A} + {B} = ?"; }
}



