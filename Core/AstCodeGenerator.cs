using Core.Models;
using System.Text;

namespace Core
{
    public class AstCodeGenerator
    {
        private readonly StringBuilder _output = new StringBuilder();
        private int _indentLevel = 0;

        public string Generate(AstNode node)
        {
            _output.Clear();
            _indentLevel = 0;
            GenerateNode(node);
            return _output.ToString().Trim();
        }

        private void GenerateNode(AstNode node)
        {
            switch (node)
            {
                case BlockNode block:
                    GenerateBlock(block);
                    break;
                case VariableDeclarationNode decl:
                    GenerateVariableDeclaration(decl);
                    break;
                case AssignmentNode assign:
                    GenerateAssignment(assign);
                    break;
                case BinaryExpressionNode expr:
                    GenerateBinaryExpression(expr);
                    break;
                case LiteralNode literal:
                    GenerateLiteral(literal);
                    break;
                case VariableUsageNode varUsage:
                    GenerateVariableUsage(varUsage);
                    break;
                default:
                    throw new NotSupportedException($"Tipo de nodo no soportado: {node.GetType().Name}");
            }
        }

        private void GenerateBlock(BlockNode block)
        {
            _indentLevel++;
            foreach (var statement in block.Statements)
            {
                GenerateNode(statement);
                _output.AppendLine();
            }
            _indentLevel--;
        }

        private void GenerateVariableDeclaration(VariableDeclarationNode decl)
        {
            AddIndent();
            _output.Append($"{decl.Type} {decl.Name}");

            if (decl.Value != null)
            {
                _output.Append(" = ");
                GenerateValue(decl.Value);
            }

            _output.AppendLine(";");
        }

        private void GenerateAssignment(AssignmentNode assign)
        {
            AddIndent();
            _output.Append($"{assign.VariableName} = ");
            GenerateValue(assign.Value);
            _output.AppendLine(";");
        }

        private void GenerateValue(object value)
        {
            switch (value)
            {
                case AstNode node:
                    GenerateNode(node);
                    break;
                case Token token:
                    _output.Append(token.Value);
                    break;
                default:
                    _output.Append(value.ToString());
                    break;
            }
        }

        private void GenerateBinaryExpression(BinaryExpressionNode expr)
        {
            _output.Append("(");
            GenerateNode(expr.Left);
            _output.Append($" {expr.Operator} ");
            GenerateNode(expr.Right);
            _output.Append(")");
        }

        private void GenerateLiteral(LiteralNode literal)
        {
            _output.Append(literal.Value.Value);
        }

        private void GenerateVariableUsage(VariableUsageNode varUsage)
        {
            _output.Append(varUsage.VariableName);
        }

        private void AddIndent()
        {
            _output.Append(new string(' ', _indentLevel * 4));
        }
    }
}