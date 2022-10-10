using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment_3
{
    public class Calculator_Logic
    {
        public static void insert_nonbinary_function(object sender, TextBox calculator_textbox)
        {
            ToolStripButton button = sender as ToolStripButton;
            switch (button.Name)
            {
                case ("LOG"):
                    calculator_textbox.Text = $"LOG({calculator_textbox.Text})";
                    break;
                case ("POW"):
                    calculator_textbox.Text = $"POW({calculator_textbox.Text})";
                    break;
                case ("SIN"):
                    calculator_textbox.Text = $"SIN({calculator_textbox.Text})";
                    break;
                case ("COS"):
                    calculator_textbox.Text = $"COS({calculator_textbox.Text})";
                    break;
                case ("TAN"):
                    calculator_textbox.Text = $"TAN({calculator_textbox.Text})";
                    break;
                case ("ERASE"):
                    calculator_textbox.Text = calculator_textbox.Text.Remove(calculator_textbox.Text.Length - 1, 1);
                    if (calculator_textbox.Text.Length == 0)
                        calculator_textbox.Text = 0.ToString();
                    break;
            }
        }

        public static void insert_operator(object sender, TextBox calculator_textbox)
        {
            if (calculator_textbox.Text == "NaN" || calculator_textbox.Text == "∞")
                calculator_textbox.Clear();
            Button button = sender as Button;
            switch (button.Name)
            {
                case "squareroot":
                    calculator_textbox.Text = $"√({calculator_textbox.Text})";
                    break;
                case "inverse":
                    calculator_textbox.Text = $"1/({calculator_textbox.Text})";
                    break;
                case "square":
                    calculator_textbox.Text = $"POW({calculator_textbox.Text})";
                    break;
                case "ERASE":
                    calculator_textbox.Text = calculator_textbox.Text.Remove(calculator_textbox.Text.Length - 1, 1);
                    break;
                case "negate":
                    if (calculator_textbox.Text.Substring(0, 1) == "-")
                        calculator_textbox.Text = calculator_textbox.Text.Remove(0, 1);
                    else
                        calculator_textbox.Text = $"-{calculator_textbox.Text}";
                    break;
                default:
                    calculator_textbox.Text += button.Text;
                    break;
            }
        }

        public static void get_special_operator(ref string calculator_contents, ref string operator_value, ref string op, ref TextBox calculator_textbox)
        {
            for (int i = 0; i < calculator_textbox.Text.Length; i++)
            {
                char c = calculator_textbox.Text[i];
                if (!Char.IsDigit(c) && op.Length < 3)
                {
                    switch (c)
                    {
                        case '√':
                        case 'S':
                        case 'C':
                        case 'T':
                        case 'L':
                        case 'P':
                            for (int j = i + 1; j < calculator_textbox.Text.Length; j++)
                            {
                                op = calculator_textbox.Text.Substring(i, j);
                                if (calculator_textbox.Text[j] == '(')
                                    break;
                            }
                            break;
                    }
                }
                if (Char.IsDigit(c))
                    operator_value += c;
            }
        }

        public static String calculate_result(object sender, TextBox calculator_textbox, String calculator_contents)
        {
            Button button = sender as Button;
            var v = new object();
            calculator_contents = "";
            String operator_value = "";
            String op = "";
            try
            {
                Calculator_Logic.get_special_operator(ref calculator_contents, ref operator_value, ref op, ref calculator_textbox);
                if (op.Equals("√"))
                    calculator_contents += Math.Sqrt(Double.Parse(operator_value));
                else if (op.Equals("COS"))
                    calculator_contents += Math.Cos(Double.Parse(operator_value));
                else if (op.Equals("SIN"))
                    calculator_contents += Math.Sin(Double.Parse(operator_value));
                else if (op.Equals("TAN"))
                    calculator_contents += Math.Tan(Double.Parse(operator_value));
                else if (op.Equals("LOG"))
                    calculator_contents += Math.Log(Double.Parse(operator_value));
                else if (op.Equals("POW"))
                    calculator_contents += Math.Pow(Double.Parse(operator_value), 2);
                else
                    calculator_contents = calculator_textbox.Text;
                v = new DataTable().Compute(calculator_contents, "");
                //this.calculator_history.Add(calculator_contents, v.ToString());
                return v.ToString();
            }
            catch (Exception ex)
            {
                return "NaN";
                v = "NaN";
            }
        }
    }
}
