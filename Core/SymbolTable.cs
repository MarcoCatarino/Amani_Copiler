using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SymbolTable
    {
        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        // Asumimos que 'Variable' es una clase que tiene 'Name' y 'DeclarationToken'
        private List<Variable> variables = new List<Variable>();

        public List<Variable> GetUnusedVariables()
        {
            // Lógica para obtener variables no usadas
            return variables.Where(v => !v.IsUsed).ToList();  // Suponiendo que hay una propiedad 'IsUsed'
        }
        public bool AddSymbol(string name, string type)
        {
            if (!symbols.ContainsKey(name))
            {
                symbols.Add(name, new Symbol(name, type));
                return true;
            }
            return false;
        }

        public Symbol GetSymbol(string name)
        {
            if (symbols.ContainsKey(name))
                return symbols[name];
            return null;
        }

        public bool SymbolExists(string name)
        {
            return symbols.ContainsKey(name);
        }

        public void MarkAsUsed(string name)
        {
            if (symbols.ContainsKey(name))
                symbols[name].IsUsed = true;
        }

        public List<Symbol> GetUnusedSymbols()
        {
            return symbols.Values.Where(s => !s.IsUsed).ToList();
        }

        public void Clear()
        {
            symbols.Clear();
        }
    }
}
