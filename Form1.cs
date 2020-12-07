using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuDoKuSePuKu
{
    public partial class Form1 : Form
    {
        NumField[,] arr = new NumField[9, 9];
        bool isCorrect = true;
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 9; i++)
            {
                for (int ii = 0; ii < 9; ii++)
                {
                    arr[i, ii] = new NumField(i, ii, panel1, this);
                }
            }
        }

        public bool sprawdzWszystekie()
        {
            bool state = true;
            bool tempState;
            for (int i = 0; i < 9; i++)
            {
                for (int ii = 0; ii < 9; ii++)
                {
                    tempState = sprawdz(i, ii);
                    if (tempState == false)
                    {
                        state = false;
                    }
                }
            }
            if (state == false)
            {
                label1.Text = "Błąd";
                isCorrect = false;
            }
            else
            {
                label1.Text = "";
                isCorrect = true;
            }
            return state;
        }
        private bool sprawdz(int x, int y)
        {
            NumField nf = arr[x, y];
            bool state = true;
            if (!(sprawdzWiersz(y) && sprawdzKolumne(x) && sprawdzKwadrat(x, y)))
            {
                if (nf.defaultColor == Color.LightGray)
                    nf.setColor(Color.Red);
                else
                    nf.setColor(Color.HotPink);
                state = false;
            }
            else
            {
                nf.setColor(nf.defaultColor);
            }
            return state;
        }
        private bool sprawdzWiersz(int x)
        {
            int[] numArr = new int[9];
            int iter = 0;
            NumField nf;

            for (int i = 0; i < 9; i++)
            {
                nf = arr[i, x];
                if (nf.isSet && numArr.Contains(nf.value))
                {
                    return false;
                }
                numArr[iter++] = nf.value;
            }
            return true;
        }
        private bool sprawdzKolumne(int y)
        {
            int[] numArr = new int[9];
            int iter = 0;
            NumField nf;

            for (int i = 0; i < 9; i++)
            {
                nf = arr[y, i];
                if (nf.isSet && numArr.Contains(nf.value))
                {
                    return false;
                }
                numArr[iter++] = nf.value;
            }
            return true;
        }
        private bool sprawdzKwadrat(int x, int y)
        {
            int[] numArr = new int[9];
            int iter = 0;
            NumField nf;

            int xk = ((int)(x / 3)) * 3;
            int yk = ((int)(y / 3)) * 3;

            for (int i = xk; i < xk + 3; i++)
            {
                for (int ii = yk; ii < yk + 3; ii++)
                {
                    nf = arr[i, ii];
                    if (nf.isSet && numArr.Contains(nf.value))
                    {
                        return false;
                    }
                    numArr[iter++] = nf.value;
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void jedna_brakujaca()
        {

        }

        private void brakujacaWiersz(int x)
        {
            int counter = 0;
            int[] numArr = new int[9];
            NumField nf;
            for(int i = 0; i < 9; i++)
            {
                nf = arr[i, x];
                if (nf.isSet)
                {
                    numArr[counter++] = nf.value;
                }
            }

        }
    }
}
