using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.calculator_history = new Dictionary<string, string>();
        }

        //Calculator
        private void calculate_result(object sender, EventArgs e)
        {
            //TODO: fix inverse, SIN, COS, TAN, LOG
            //Perhaps a sliding window?
            Button button = sender as Button;
            var v = new object();
            String calculator_contents = "";
            String sqrt = "";
            try
            {
                for (int i = 0; i < calculator_textbox.Text.Length; i++)
                {
                    char c = calculator_textbox.Text[i];
                    if (c == '√' || c == '(')
                        continue;
                    else if (c == ')')
                        calculator_contents += Math.Sqrt(Double.Parse(sqrt));
                    else
                        sqrt += c;
                }
                calculator_contents += sqrt;
                v = new DataTable().Compute(calculator_contents, "");
                this.calculator_history.Add(calculator_contents, v.ToString());
                calculator_textbox.Text = v.ToString();
            }
            catch (Exception ex)
            {
                v = "NaN";
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
                    calculator_textbox.Text = string.Format("√({0})", calculator_textbox.Text);
                    break;
                case "inverse":
                    calculator_textbox.Text = string.Format("1/({0})", calculator_textbox.Text);
                    break;
                case "square":
                    calculator_textbox.Text = string.Format("{0}^2", calculator_textbox.Text);
                    break;
                case "ERASE":
                    calculator_textbox.Text = calculator_textbox.Text.Remove(calculator_textbox.Text.Length - 1, 1);
                    break;
                case "negate":
                    if (calculator_textbox.Text.Substring(0, 1) == "-")
                        calculator_textbox.Text = calculator_textbox.Text.Remove(0, 1);
                    else
                        calculator_textbox.Text = string.Format("-{0}", calculator_textbox.Text);
                    break;
                default:
                    calculator_textbox.Text += button.Text;
                    break;
            }
        }
        private void Toolstrip_Operator_Handler(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            switch (button.Text)
            {
                case ("LOG"):
                    calculator_textbox.Text = string.Format("LOG({0})", calculator_textbox.Text);
                    break;
                case ("Square"):
                    calculator_textbox.Text = string.Format("{0}^2", calculator_textbox.Text);
                    break;
                case ("SIN"):
                    calculator_textbox.Text = string.Format("SIN({0})", calculator_textbox.Text);
                    break;
                case ("COS"):
                    calculator_textbox.Text = string.Format("COS({0})", calculator_textbox.Text);
                    break;
                case ("TAN"):
                    calculator_textbox.Text = string.Format("TAN({0})", calculator_textbox.Text);
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
        }
        private void button2_Click(object sender, EventArgs e)
        {
            calculator_textbox.Text = 0.ToString();
            calculator_history.Clear();
        }
        //Day Difference Calculator
        private void calculate_date_difference(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).ContainsFocus)
            {
                DateTime to = toDate.Value;
                DateTime from = fromDate.Value;
                dayCounter.Value = (to - from).Days + 1;
            }
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
            toolStripStatusLabel1.Text = String.Format("Hello! Today is {0}. The time is: {1}", DateTime.Today.ToLongDateString(), DateTime.Now.ToLongTimeString());
        }
        Dictionary<String, String> calculator_history;
    }
}
