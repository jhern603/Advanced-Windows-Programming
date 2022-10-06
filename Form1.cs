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
           
            try
            {
                v = new DataTable().Compute(textbox.Text, "");
                this.calculator_history.Add(textbox.Text, v.ToString());
                textbox.Text = v.ToString();
            }
            catch (Exception ex)
            {
                v = "NaN";
            }
        }
        private void operator_handler(object sender, EventArgs e)
        {
            if (textbox.Text == "NaN" || textbox.Text == "∞")
                textbox.Clear();
            Button button = sender as Button;
            textbox.Text += button.Text;
        }
        private void digit_handler(object sender, EventArgs e)
        {
            if (textbox.Text == "0" || textbox.Text == "NaN" || textbox.Text == "∞")
                textbox.Clear();
            Button button = sender as Button;
            textbox.Text += button.Text;
        }
        private void handle_clear(object sender, EventArgs e)
        {
            textbox.Text = 0.ToString();
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
