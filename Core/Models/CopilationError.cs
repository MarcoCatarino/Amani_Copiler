namespace CompilerProject.Models
{
    public class CompilationError
    {
        public string Type { get; set; } = "Error";
        public int Line { get; set; }
        public string Description { get; set; } = "";
    }
}
