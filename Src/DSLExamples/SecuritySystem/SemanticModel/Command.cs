namespace DSLExamples.SecuritySystem.SemanticModel
{
	public class Command : AbstractEvent
	{
		public Command(string name, string code) 
			: base(name, code)
		{
		}
	}
}