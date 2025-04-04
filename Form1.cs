using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Core;  // Si 'SymbolTable' está en Core


namespace CompilerProject
{
    public partial class Form1 : Form
    {
        private readonly MainCompiler _compiler = new MainCompiler();

        public Form1()
        {
            InitializeComponent();
            ConfigureEditor();
            ConfigureErrorView();
            AddDemoCode();
        }

        private void ConfigureEditor()
        {
            txtSourceCode.BackColor = Color.FromArgb(30, 30, 30);
            txtSourceCode.ForeColor = Color.White;
            txtSourceCode.Font = new Font("Consolas", 11);
            txtSourceCode.AcceptsTab = true;
        }

        private void ConfigureErrorView()
        {
            lvErrors.View = View.Details;
            lvErrors.Columns.Add("Tipo", 100);
            lvErrors.Columns.Add("Línea", 70);
            lvErrors.Columns.Add("Descripción", 500);
            lvErrors.FullRowSelect = true;
        }

        private void AddDemoCode()
        {
            txtSourceCode.Text = @"// Escribe tu código aquí
$C{
    // Ejemplo con errores:
    ENT variableNoUsada = 10;
    DEC precio = 20.5;
    precio = ""texto"";  // Error de tipo
    
    Function START() {
        Write(""Hola Mundo"");
    }
}$C";
        }

        private void ShowError(string message)
        {
            var item = new ListViewItem("Error");
            item.SubItems.Add("");
            item.SubItems.Add(message);
            item.BackColor = Color.LightCoral;
            lvErrors.Items.Add(item);
            UpdateStatus(message);
        }

        private void ClearHighlights()
        {
            txtSourceCode.SelectAll();
            txtSourceCode.SelectionBackColor = txtSourceCode.BackColor;
            txtSourceCode.SelectionLength = 0;
        }

        private void ShowSuccess(string message)
        {
            var item = new ListViewItem("Éxito");
            item.SubItems.Add("");
            item.SubItems.Add(message);
            item.BackColor = Color.FromArgb(50, 120, 50);
            lvErrors.Items.Add(item);
            tabControl1.SelectedTab = tabOptimized;
            UpdateStatus(message);
        }

        private void ShowErrors(List<string> errors)
        {
            foreach (var error in errors)
            {
                var item = new ListViewItem(GetErrorType(error));
                item.SubItems.Add(GetLineNumber(error));
                item.SubItems.Add(GetErrorDescription(error));
                item.BackColor = GetErrorColor(error);
                lvErrors.Items.Add(item);

                HighlightErrorLine(error);
            }
            tabControl1.SelectedTab = tabErrors;
            UpdateStatus($"Errores encontrados: {errors.Count}");
        }

        private string GetErrorType(string error)
        {
            if (error.Contains("no usada") || error.Contains("no utilizada")) return "Advertencia";
            if (error.Contains("tipo") || error.Contains("asignación")) return "Error de Tipo";
            return "Error";
        }

        private string GetLineNumber(string error)
        {
            var parts = error.Split(new[] { "Línea" }, StringSplitOptions.None);
            if (parts.Length > 1)
            {
                var linePart = parts[1].Split(',')[0];
                if (int.TryParse(linePart.Trim(), out _))
                    return linePart.Trim();
            }
            return "0";
        }

        private string GetErrorDescription(string error)
        {
            var colonIndex = error.IndexOf(':');
            return colonIndex > 0 ? error.Substring(colonIndex + 1).Trim() : error;
        }

        private Color GetErrorColor(string error)
        {
            if (error.Contains("no usada")) return Color.FromArgb(70, 130, 180);
            if (error.Contains("tipo")) return Color.FromArgb(220, 20, 60);
            return Color.FromArgb(255, 140, 0);
        }

        private void HighlightErrorLine(string error)
        {
            if (int.TryParse(GetLineNumber(error), out int lineNum) && lineNum > 0)
            {
                int start = txtSourceCode.GetFirstCharIndexFromLine(lineNum - 1);
                int end = lineNum < txtSourceCode.Lines.Length ?
                         txtSourceCode.GetFirstCharIndexFromLine(lineNum) - 1 :
                         txtSourceCode.TextLength;

                if (start >= 0)
                {
                    txtSourceCode.Select(start, end - start);
                    txtSourceCode.SelectionBackColor = GetErrorColor(error);
                    txtSourceCode.SelectionLength = 0;
                }
            }
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = message;
        }

        private void lvErrors_DoubleClick(object sender, EventArgs e)
        {
            if (lvErrors.SelectedItems.Count > 0)
            {
                string lineStr = lvErrors.SelectedItems[0].SubItems[1].Text;
                if (int.TryParse(lineStr, out int lineNum) && lineNum > 0)
                {
                    int start = txtSourceCode.GetFirstCharIndexFromLine(lineNum - 1);
                    if (start >= 0)
                    {
                        txtSourceCode.Select(start, 0);
                        txtSourceCode.ScrollToCaret();
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnCompile.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnCompile_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Limpiar resultados anteriores
                lvErrors.Items.Clear();
                ClearHighlights();
                txtOptimizedCode.Clear();

                // Obtener código fuente
                string userCode = txtSourceCode.Text;

                // Validar código vacío
                if (string.IsNullOrWhiteSpace(userCode))
                {
                    ShowError("No hay código para compilar");
                    return;
                }

                // Compilar
                var result = _compiler.Compile(userCode);

                // Debug: Mostrar tokens en consola
                Console.WriteLine("=== TOKENS ===");
                foreach (var token in result.Tokens)
                {
                    Console.WriteLine($"{token.Value} [{token.Category}] (Línea {token.Line})");
                }

                // Mostrar resultados
                if (result.Success)
                {
                    ShowSuccess("¡Compilación exitosa!");
                    txtOptimizedCode.Text = result.OptimizedCode;
                }
                else
                {
                    ShowErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error inesperado: " + ex.Message);
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            txtSourceCode.Clear();
            lvErrors.Items.Clear();
            txtOptimizedCode.Clear();
            ClearHighlights();
            UpdateStatus("Editor listo");
        }
    }
}
