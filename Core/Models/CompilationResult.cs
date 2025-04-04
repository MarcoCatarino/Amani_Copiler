using System.Collections.Generic;

namespace CompilerProject.Models
{
    public class CompilationResult
    {
        public List<CompilationError> Errors { get; set; } = new List<CompilationError>();
        public List<Token> Tokens { get; set; } = new List<Token>();
        public string OptimizedCode { get; set; } = string.Empty;
        public bool Success => Errors.Count == 0;
    }
}
