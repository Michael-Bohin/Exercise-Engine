using System.Text;
using ExerciseEngine.MacroExercise;

namespace ExerciseEngine.ExerciseCompiler;

public class DefinitionAssignment {
	public MacroString Description = new();
	public List<DefinitionQuestion> Questions = new();

	public string CompileResultMethods() {
		StringBuilder sb = new();
		for (int i = 0; i < Questions.Count; i++)
			sb.Append(Questions[i].CompileNthResultMethod(i));
		return sb.ToString();
	}
}