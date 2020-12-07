using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SuDoKuSePuKu
{
    class NumField
    {
        public int x { get; }
        public int y { get; }
        public int value { get; private set; }
        public bool isSet { get; private set; }
        private TextBox tb { get; }
        private Form1 form1;
        public Color color { get; private set; }
        public Color defaultColor { get; }

        public NumField(int x, int y, Panel p, Form1 form1)
        {
            this.form1 = form1;
            this.x = x;
            this.y = y;
            value = 0;

            if ((x < 3 && y < 3) || (x > 5 && y < 3) || (x > 5 && y > 5) || (x < 3 && y > 5) || (x > 2 && x < 6 && y > 2 && y < 6))
                defaultColor = Color.LightGray;
            else
                defaultColor = Color.White;

            tb = new TextBox();
            tb.Size = new Size(20, 20);
            tb.Location = new Point(25 * x, 25 * y);
            tb.KeyPress += textBox_KeyPress;
            tb.TextChanged += textBox_textChanged;
            setColor(defaultColor);

            p.Controls.Add(tb);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            var regex = new Regex(@"0");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void textBox_textChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            setText(tb.Text);

        }

        private void setText(String text)
        {

            if (text.Length > 1)
            {
                text = text[0].ToString();
            }

            isSet = Int32.TryParse(text, out int value);
            if (isSet)
            {
                this.value = value;
                tb.Text = text;
            }

            form1.sprawdzWszystekie();
        }

        public void setColor(Color color)
        {
            this.tb.BackColor = color;
            this.color = color;
        }
    }
}
