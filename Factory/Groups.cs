namespace ExerciseEngine.Factory;

// Record Groups contains information for teachers to filter exercises by topic, class or exercise
// type. Creating enum for polymorphic structure is antipattern for sure, however here it is done
// on purpose for the needs of Json deserialization process. The deserializer uses the value in
// the enum to tell which instance of Exercise to make.

record Groups {
	public List<Classes> Classes { get; } 
	public List<Topic> Topics { get; } 
	public ExerciseType ExerciseType { get; } 
	public Groups(List<Classes> Classes, List<Topic> Topics, ExerciseType ExerciseType) {
		this.Classes = Classes;	this.Topics = Topics; this.ExerciseType = ExerciseType;
	}
}