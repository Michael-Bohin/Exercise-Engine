using ExerciseEngine.MacroExercise;
using ExerciseEngine;
using System.IO;
using System.Net.Http.Headers;

Exercise_000_001_S01P01_Prevod_jednotek_plochy_objemu_prvni		e001 = new();
Exercise_000_002_S01P01_Prevod_jednotek_plochy_objemu_druhy		e002 = new();
Exercise_000_003_S01P02_Prevod_jednotek_casu					e003 = new();
Exercise_000_004_S01P03_Prevod_jednotek_uhlu					e004 = new();
Exercise_000_005_S01P04_O_kolik_se_lisi							e005 = new();
Exercise_000_006_S01P05_Pocitani_se_zlomky						e006 = new();

CompilerOutput cout = new();

cout.Add(e001);
cout.Add(e002);
cout.Add(e003);
cout.Add(e004);	
cout.Add(e005);
cout.Add(e006);

cout.RunAll();

class CompilerOutput {
	readonly List<IExercise> exercises = new();

	public void Add(IExercise e) =>	exercises.Add(e);

	public void RunAll() {
		foreach(IExercise exercise in exercises)
			RunSingle(exercise);
	}

	static void RunSingle(IExercise ex) {

		ex.FilterLegitVariants();
		string stats = ex.ReportStatistics();
		string JsonExercise = ex.SerializeSelf(true);
		
		MacroExercise me = ex.BuildMacroExercise();
		string JsonMacroExercise = me.SerializeSelf(true);
		string JsonAssignments = me.SerializeAssignments(Language.cs, true);
		string PrettyPrint = me.PrettyPrint(Language.cs, 1_000);

		string name = ex.GetName();

		string folder = $"./out/{name}/";

		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);

		using StreamWriter sw1 = new($"{folder}stats.txt");
		sw1.Write(stats);

		using StreamWriter sw2 = new($"{folder}Exercise.json");
		sw2.Write(JsonExercise);

		using StreamWriter sw3 = new($"{folder}MacroExercise.json");
		sw3.Write(JsonMacroExercise);

		using StreamWriter sw4 = new($"{folder}Assignments.json");
		sw4.Write(JsonAssignments);

		using StreamWriter sw5 = new($"{folder}PrettyPrint.txt");
		sw5.Write(PrettyPrint);

	}	
}