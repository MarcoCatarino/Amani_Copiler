using System.Collections.Generic;

namespace Core.Models
{
    public abstract class AstNode
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }

    public class BlockNode : AstNode
    {
        public List<AstNode> Statements { get; } = new List<AstNode>();
    }

    public class VariableDeclarationNode : AstNode
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }  // Puede ser Token o AstNode
    }

    public class AssignmentNode : AstNode
    {
        public string VariableName { get; set; }
        public object Value { get; set; }  // Puede ser Token o AstNode
    }

    public class BinaryExpressionNode : AstNode
    {
        public AstNode Left { get; set; }
        public string Operator { get; set; }
        public AstNode Right { get; set; }
    }

    public class LiteralNode : AstNode
    {
        public Token Value { get; set; }
    }

    public class VariableUsageNode : AstNode
    {
        public string VariableName { get; set; }
    }
}