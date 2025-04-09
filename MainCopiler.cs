using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class MainCompiler
{
    private readonly SymbolTable _symbolTable = new SymbolTable();
    private readonly ErrorHandler _errorHandler = new ErrorHandler();

    public class CompilationResult
    {
        public bool Success { get; set; }
        public List<CompilationError> Errors { get; set; } = new List<CompilationError>();
        public List<CompilationError> Warnings { get; set; } = new List<CompilationError>();
        public string OriginalCode { get; set; }
        public string OptimizedCode { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();
        public AstNode Ast { get; set; }
        public AstNode OptimizedAst { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }

    public CompilationResult Compile(string code)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new CompilationResult
        {
            OriginalCode = code
        };

        try
        {
            // Fase 1: Tokenización
            var lexer = new Lexer();
            result.Tokens = lexer.Tokenize(code);

            // Fase 2: Análisis sintáctico y generación de AST
            var parser = new Parser(_symbolTable, _errorHandler);
            result.Ast = parser.Parse(result.Tokens);

            // Recoger errores y advertencias
            result.Errors = _errorHandler.Errors;
            result.Warnings = _errorHandler.Warnings;
            result.Success = !_errorHandler.HasErrors;

            // Fase 3: Optimización (solo si no hay errores)
            if (result.Success)
            {
                // Optimización a nivel de AST
                var astOptimizer = new AstOptimizer();
                result.OptimizedAst = astOptimizer.Optimize(result.Ast);

                // Generación de código optimizado
                var codeGenerator = new AstCodeGenerator();
                result.OptimizedCode = codeGenerator.Generate(result.OptimizedAst);
            }
        }
        catch (ParseError ex)
        {
            _errorHandler.AddError("Error Sintáctico", ex.Line ?? 0, ex.Message);
            result.Success = false;
        }
        catch (SemanticError ex)
        {
            _errorHandler.AddError("Error Semántico", ex.Line ?? 0, ex.Message);
            result.Success = false;
        }
        catch (Exception ex)
        {
            _errorHandler.AddError("Error del Compilador", 0,
                $"Error interno: {ex.GetType().Name} - {ex.Message}");
            result.Success = false;
        }
        finally
        {
            stopwatch.Stop();
            result.ElapsedTime = stopwatch.Elapsed;

            // Limpiar estado para la próxima compilación
            _symbolTable.Clear();
            _errorHandler.Clear();
        }

        return result;
    }
}

// Nuevas clases auxiliares (deben ir en sus propios archivos)

public class SemanticError : Exception
{
    public int? Line { get; }

    public SemanticError(string message, int? line = null) : base(message)
    {
        Line = line;
    }
}

public class AstOptimizer
{
    public AstNode Optimize(AstNode node)
    {
        return node switch
        {
            BlockNode block => OptimizeBlock(block),
            VariableDeclarationNode decl => OptimizeDeclaration(decl),
            AssignmentNode assign => OptimizeAssignment(assign),
            _ => node
        };
    }

    private BlockNode OptimizeBlock(BlockNode block)
    {
        var optimized = new BlockNode();
        foreach (var statement in block.Statements)
        {
            optimized.Statements.Add(Optimize(statement));
        }
        return optimized;
    }

    private VariableDeclarationNode OptimizeDeclaration(VariableDeclarationNode decl)
    {
        // Aquí irían optimizaciones como constant folding
        return decl;
    }

    private AssignmentNode OptimizeAssignment(AssignmentNode assign)
    {
        // Optimizaciones de expresiones
        return assign;
    }
}

public class AstCodeGenerator
{
    public string Generate(AstNode node)
    {
        return node switch
        {
            BlockNode block => GenerateBlock(block),
            VariableDeclarationNode decl => GenerateDeclaration(decl),
            AssignmentNode assign => GenerateAssignment(assign),
            _ => string.Empty
        };
    }

    private string GenerateBlock(BlockNode block)
    {
        return string.Join("\n", block.Statements.Select(Generate));
    }

    private string GenerateDeclaration(VariableDeclarationNode decl)
    {
        return decl.Value != null
            ? $"{decl.Type} {decl.Name} = {decl.Value}"
            : $"{decl.Type} {decl.Name}";
    }

    private string GenerateAssignment(AssignmentNode assign)
    {
        return $"{assign.VariableName} = {assign.Value}";
    }
}