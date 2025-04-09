using Core.Models;

namespace Core
{
    public class ErrorHandler
    {
        public List<CompilationError> Errors { get; } = new List<CompilationError>();
        public List<CompilationError> Warnings { get; } = new List<CompilationError>();

        public void AddError(string type, int line, string description)
        {
            Errors.Add(new CompilationError { Type = type, Line = line, Description = description });
        }

        public void AddWarning(string type, int line, string description)
        {
            Warnings.Add(new CompilationError { Type = type, Line = line, Description = description });
        }

        public void Clear()
        {
            Errors.Clear();
            Warnings.Clear();
        }

        public bool HasErrors => Errors.Count > 0;
    }
}
