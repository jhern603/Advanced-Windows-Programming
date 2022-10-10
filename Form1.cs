using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//TODO: implement power button on main calculator form
namespace assignment_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Calculator
        private void calculate_result(object sender, EventArgs e)
        {
            //TODO: fix inverse, SIN, COS, TAN, LOG
            //Perhaps a sliding window?
            Button button = sender as Button;
            var v = new object();
            this.calculator_contents = "";
            String operator_value = "";
            String op = "";
            try
            {
                get_special_operator(ref calculator_contents, ref operator_value, ref op);
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
                v = new DataTable().Compute(calculator_contents, "");
                //this.calculator_history.Add(calculator_contents, v.ToString());
                calculator_textbox.Text = v.ToString();
                calculator_contents = v.ToString();
            }
            catch (Exception ex)
            {
                v = "NaN";
            }
        }

        private void get_special_operator(ref string calculator_contents, ref string operator_value, ref string op)
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

        private void operator_handler(object sender, EventArgs e)
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
        private void Toolstrip_Operator_Handler(object sender, EventArgs e)
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
        private void digit_handler(object sender, EventArgs e)
        {
            if (calculator_textbox.Text == "0" || calculator_textbox.Text == "NaN" || calculator_textbox.Text == "∞")
                calculator_textbox.Clear();
            Button button = sender as Button;
            calculator_textbox.Text += button.Text;
        }
        private void handle_clear(object sender, EventArgs e)
        {
            calculator_textbox.Text = 0.ToString();
            this.calculator_contents = 0.ToString();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            calculator_textbox.Text = 0.ToString();
            this.calculator_contents = calculator_textbox.Text;
            //calculator_history.Clear();
        }
        //Day Difference Calculator
        private void calculate_date_difference(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).ContainsFocus)
                dayCounter.Value = (toDate.Value - fromDate.Value).Days + 1;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (dayCounter.ContainsFocus)
            {
                decimal count = dayCounter.Value;
                if (count > 0)
                    toDate.Value = toDate.Value.AddDays((double)count);
                else if (count < 0)
                    fromDate.Value = fromDate.Value.AddDays((double)count);
                else
                    toDate.Value = fromDate.Value;
            }
        }

        //Bottom StatusStrip
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"Hello! Today is {DateTime.Today.ToLongDateString()}. The time is: {DateTime.Now.ToLongTimeString()}";
        }
        //Lookup<String, String> calculator_history;
        String calculator_contents;

        private void print_calculator_history(object sender, EventArgs e)
        {
            PrintDialog PrintDialog1 = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            PrintDialog1.ShowDialog();
        }

        //Appearance Settings
        private void change_background_color(object sender, EventArgs e)
        {
            ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
            ColorDialog color_dialog = new ColorDialog();
            if (color_dialog.ShowDialog() == DialogResult.OK)
                switch (menu_item.Name)
                {
                    case "calculator_background_picker":
                        horizontal_container.Panel1.BackColor = color_dialog.Color;
                        break;
                    case "day_counter_background_picker":
                        horizontal_container.Panel2.BackColor = color_dialog.Color;
                        break;
                    case "graph_background_picker":
                        splitContainer1.Panel2.BackColor = color_dialog.Color;
                        break;
                    case "form_background_picker":
                        this.BackColor = color_dialog.Color;
                        break;
                }
        }

        private void change_calculator_font(object sender, EventArgs e)
        {
            FontDialog font_dialog = new FontDialog();
            if (font_dialog.ShowDialog() == DialogResult.OK)
                foreach (Control control_item in tableLayoutPanel1.Controls)
                {
                    control_item.Font = font_dialog.Font;
                }

        }
    }
}
