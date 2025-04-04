using CompilerProject.Models;
using Core;

public class MainCompiler
{
    private readonly SymbolTable _symbolTable = new SymbolTable();
    private readonly ErrorHandler _errorHandler = new ErrorHandler();

    public class CompilationResult
    {
        public bool Success { get; set; }
        public List<CompilationError> Errors { get; set; } = new List<CompilationError>(); // Cambiar a CompilationError
        public List<CompilationError> Warnings { get; set; } = new List<CompilationError>(); // Cambiar a CompilationError
        public string OptimizedCode { get; set; } = string.Empty;
        public List<Token> Tokens { get; set; } = new List<Token>();
    }

    public CompilationResult Compile(string code)
    {
        var result = new CompilationResult();

        try
        {
            // 1. Tokenización
            var lexer = new Lexer();
            result.Tokens = lexer.Tokenize(code);

            // 2. Análisis sintáctico y semántico
            var parser = new Parser(_symbolTable);
            var parserErrors = parser.Parse(result.Tokens, code);
            foreach (var error in parserErrors)
            {
                result.Errors.Add(ParseError(error, code));
            }

            // 3. Detección de variables no usadas (Advertencias)
            var unusedVars = _symbolTable.GetUnusedVariables();  // Asegúrate de que esto devuelva objetos con 'DeclarationToken'
            foreach (var variable in unusedVars)
            {
                var warning = new CompilationError
                {
                    Type = "Advertencia",
                    Line = variable.DeclarationToken.Line,  // Asegúrate de que variable sea del tipo correcto
                    Description = $"Variable '{variable.Name}' declarada pero no usada"
                };
                result.Warnings.Add(warning);
            }


            // 4. Optimización (solo si no hay errores críticos)
            if (!result.Errors.Any(e => e.Type == "Error" || e.Type == "Error de Tipo"))
            {
                var optimizer = new Optimizer(_symbolTable);
                List<Token> optimizedTokens = optimizer.Optimize(result.Tokens);
                result.OptimizedCode = string.Join(" ", optimizedTokens.Select(t => t.Value));
                result.Success = true;
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add(new CompilationError
            {
                Type = "Error Crítico",
                Line = 0, // Línea desconocida
                Description = ex.Message
            });
        }

        return result;
    }

    // Método para convertir un error a un objeto CompilationError
    private CompilationError ParseError(string error, string code)
    {
        // Aquí debes hacer el parsing del error y extraer la línea, el tipo y la descripción.
        // Este es un ejemplo general, que dependerá de cómo se estructuren los errores en tu parser.

        var lineNumber = ExtractLineNumber(error, code);
        return new CompilationError
        {
            Type = "Error",
            Line = lineNumber,
            Description = error
        };
    }

    private int ExtractLineNumber(string error, string code)
    {
        // Aquí puedes extraer el número de línea del error basado en el formato de tu error string
        // Si el error tiene "Línea 4:", extrae la línea
        // Este es un ejemplo simplificado
        var match = System.Text.RegularExpressions.Regex.Match(error, @"Línea (\d+):");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        return 0; // Si no se encuentra la línea, devolver 0
    }
}
