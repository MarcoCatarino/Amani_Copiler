using Core.Models;
using System;
using System.Text.RegularExpressions;

namespace Core
{
    public class Parser
    {
        private readonly SymbolTable symbolTable;
        private readonly ErrorHandlers errorHandler;

        public Parser(SymbolTable table, ErrorHandlers errors)
        {
            symbolTable = table;
            errorHandler = errors;
        }

        public void AnalyzeLine(string line)
        {
            line = line.Trim();

            if (Regex.IsMatch(line, @"^(entero|decimal|cadena)\s+\w+\s*=")) // declaración
            {
                var match = Regex.Match(line, @"^(entero|decimal|cadena)\s+(\w+)\s*=\s*(.+);?$");
                if (match.Success)
                {
                    string tipo = match.Groups[1].Value;
                    string nombre = match.Groups[2].Value;
                    string valor = match.Groups[3].Value;

                    if (!symbolTable.AddSymbol(nombre, tipo))
                    {
                        errorHandler.AddError($"[ERROR] La variable '{nombre}' ya está declarada.");
                        return;
                    }

                    if (!EsTipoCompatible(tipo, valor))
                    {
                        errorHandler.AddError($"[ERROR] Tipo incompatible en la asignación de '{nombre}'. Esperado: {tipo}.");
                    }
                }
            }
            else if (Regex.IsMatch(line, @"^\w+\s*=")) // reasignación
            {
                var match = Regex.Match(line, @"^(\w+)\s*=\s*(.+);?$");
                if (match.Success)
                {
                    string nombre = match.Groups[1].Value;
                    string valor = match.Groups[2].Value;

                    var simbolo = symbolTable.GetSymbol(nombre);
                    if (simbolo == null)
                    {
                        errorHandler.AddError($"[ERROR] Variable '{nombre}' no declarada.");
                        return;
                    }

                    if (!EsTipoCompatible(simbolo.Type, valor))
                    {
                        errorHandler.AddError($"[ERROR] Asignación inválida para '{nombre}'. Tipo esperado: {simbolo.Type}.");
                    }

                    symbolTable.MarkAsUsed(nombre);
                }
            }
            else if (Regex.IsMatch(line, @"^\w+\s*\(.*\);?$")) // función, ignorar
            {
                return;
            }
            else // posible uso de variable
            {
                foreach (var palabra in line.Split(new[] { ' ', ';', '+', '-', '*', '/', '(', ')' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var simbolo = symbolTable.GetSymbol(palabra);
                    if (simbolo != null)
                        symbolTable.MarkAsUsed(palabra);
                }
            }
        }

        public void FinalizeAnalysis()
        {
            foreach (var symbol in symbolTable.GetUnusedSymbols())
            {
                errorHandler.AddWarning($"[ADVERTENCIA] La variable '{symbol.Name}' fue declarada pero no utilizada.");
            }
        }

        private bool EsTipoCompatible(string tipo, string valor)
        {
            valor = valor.Trim();

            switch (tipo)
            {
                case "entero":
                    return int.TryParse(valor, out _);
                case "decimal":
                    return decimal.TryParse(valor, out _);
                case "cadena":
                    return valor.StartsWith("\"") && valor.EndsWith("\"");
                default:
                    return false;
            }
        }
    }
}
