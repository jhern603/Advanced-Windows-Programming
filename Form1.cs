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
            Button button = sender as Button;
            var v = new object();
            String calculator_contents ="";
            String sqrt = "";
            try
            {
                foreach(char c in calculator_textbox.Text)
                { 
                    if (c == '√' || c == '(')
                        continue;
                    else if(c == ')')
                        calculator_contents += Math.Sqrt(Double.Parse(sqrt));
                    else
                        sqrt += c;
                }
                
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
            if (button.Name == "squareroot")
                calculator_textbox.Text = string.Format("√({0})", calculator_textbox.Text);
            else if(button.Name == "inverse")
                calculator_textbox.Text = string.Format("1/({0})", calculator_textbox.Text);
            else if(button.Name == "square")
                calculator_textbox.Text = string.Format("{0}^2", calculator_textbox.Text);
            else
                calculator_textbox.Text += button.Text;
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
