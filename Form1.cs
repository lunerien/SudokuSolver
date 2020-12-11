using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        bool solved = false;
         
        public Form1()
        {
            InitializeComponent();
            string[] fileEntries = Directory.GetFiles("../../Sudoku");
            
            foreach (string fileName in fileEntries)
                comboBox1.Items.Add(fileName.Split('\\')[1]);
            comboBox1.SelectedIndex = 0;

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
            var max_prob = 100;
            while (!is_solved() && max_prob-- >= 0 )
            {
                jedna_brakujaca();
                wyklucz();
            }
            if(max_prob <= 1)
            {
                label1.Text = "Nie potrafię tego rozwiązać";
            }
        }

        public bool is_solved()
        {
            var counter = 0;
            var dest = 81;
            for(int i = 0; i < 9; i++)
            {
                for(int ii = 0; ii < 9; ii++)
                {
                    if (arr[i, ii].isSet)
                    {
                        counter++;
                    }
                    else
                    {
                        return false;

                    }
                }
            }
            label1.Text = "Wygrana";
            return true;

        }

        private void jedna_brakujaca()
        {
            for (int i = 0; i < 9; i++)
            {
                brakujacaWiersz(i);
                brakujacaKolumna(i);
                for (int ii = 0; ii < 9; ii++)
                {
                    brakujacaKwadrat(i, ii);
                }
            }
        }

        private int[] zwrocBrakujace(int[] numArr)
        {
            HashSet<int> myRange = new HashSet<int>(Enumerable.Range(1, 9));
            myRange.ExceptWith(numArr);
           
            return myRange.ToArray();
        }

        private int[] brakujacaWiersz(int x)
        {
            int counter = 0;
            int puste_pole = 0;
            int[] numArr = new int[9];
            NumField nf;
            for (int i = 0; i < 9; i++)
            {
                nf = arr[i, x];
                if (nf.isSet)
                {
                    numArr[counter++] = nf.value;
                }
                else
                {
                    puste_pole = i;
                }




            }
            int[] myRange = zwrocBrakujace(numArr);

            if (counter == 8)
            {

                nf = arr[puste_pole, x];
                nf.setText(myRange[0].ToString());
                //https://stackoverflow.com/questions/3700448/how-can-i-print-the-contents-of-an-array-horizontally
            }
            return myRange;
        }

        private int[] brakujacaKolumna(int y)
        {
            int counter = 0;
            int puste_pole = 0;
            int[] numArr = new int[9];
            NumField nf;
            for (int i = 0; i < 9; i++)
            {
                nf = arr[y, i];
                if (nf.isSet)
                {
                    numArr[counter++] = nf.value;
                }
                else
                {
                    puste_pole = i;
                }
            }
            int[] myRange = zwrocBrakujace(numArr);

            if (counter == 8)
            {

                nf = arr[y, puste_pole];
                nf.setText(myRange[0].ToString());
                //https://stackoverflow.com/questions/3700448/how-can-i-print-the-contents-of-an-array-horizontally
            }
            return myRange;
        }

        private int[] brakujacaKwadrat(int y, int x)
        {
            int[] numArr = new int[9];
            NumField nf;
            int counter = 0;
            int brakujaca_pole_x = 0, brakujaca_pole_y = 0;
            int xk = ((int)(x / 3)) * 3;
            int yk = ((int)(y / 3)) * 3;
            for (int i = xk; i < xk + 3; i++)
            {
                for (int ii = yk; ii < yk + 3; ii++)
                {
                    nf = arr[i, ii];
                    if (nf.isSet)
                    {
                        numArr[counter++] = nf.value;
                    }
                    else
                    {
                        brakujaca_pole_x = i;
                        brakujaca_pole_y = ii;
                    }
                }
            }
            int[] myRange = zwrocBrakujace(numArr);

            if (counter == 8)
            {

                nf = arr[brakujaca_pole_x, brakujaca_pole_y];
                nf.setText(myRange[0].ToString());
            }
            return myRange; 
        }

        int[] zamien_braukjace_na_te_ktore_sa(int[] brakujce)
        {
            int[] liczby = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            foreach(int el in brakujce)
            {
                liczby = liczby.Where(val => val != el).ToArray();
            }
            return liczby;
        }

        private void wyklucz()
        {
            
            for(int i = 0; i < 9; i += 3)
            {
                for(int ii = 0; ii < 9; ii +=3)
                {
                    wykluczKwadrat(i, ii);
                }
            }
        }

        private int[,,] wykluczKwadrat(int x, int y)
        {
            int[,,] brakujace = new int[3,3,9];
            NumField nf;

            int xk = ((int)(x / 3)) * 3;
            int yk = ((int)(y / 3)) * 3;

            int[] brakujace_w_kwadracie = zamien_braukjace_na_te_ktore_sa(brakujacaKwadrat(x, y));

            int ly = 0;
            int lx = 0;

            int[] brakujace_1;

            for(int i = xk; i < xk+3; i++)
            {
                ly = 0;
                for (int ii = yk; ii < yk+3; ii++)
                {

                    nf = arr[ii, i];
                    if (!nf.isSet)
                    {
                        int[] cyfry = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                        foreach(int el in brakujace_w_kwadracie)
                        {
                            cyfry = cyfry.Where(val => val != el).ToArray();
                        }

                        //sprawdzamy wiersze jeżeli jest w damyn jest to usuwamy z brakujacych
                        brakujace_1 = brakujacaWiersz(i);
                        brakujace_1 = zamien_braukjace_na_te_ktore_sa(brakujace_1);
                        foreach (int el in brakujace_1)
                        {
                            cyfry = cyfry.Where(val => val != el).ToArray();
                        }

                        //sprawdzamy kolumny jeżeli jest to usuwamy z brakujacych
                        brakujace_1 = brakujacaKolumna(ii);
                        brakujace_1 = zamien_braukjace_na_te_ktore_sa(brakujace_1);
                        foreach (int el in brakujace_1)
                        {
                            cyfry = cyfry.Where(val => val != el).ToArray();
                        }
                        //cyfry = cyfry.Where(val => val != 0).ToArray();
                        for (int iii = 0; iii< cyfry.Length;iii++)
                        {
                            brakujace[lx, ly, iii] = cyfry[iii];
                           
                        }
                        
                        //jeżeli jest 1 to wpisz w puste
                        if(cyfry.Length == 1)
                        {
                            nf.setValue(cyfry[0]);
                        }
                    }
                    ly++;
                }
                lx++;
            }
            
            return brakujace;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using(var reader = new StreamReader($"../../sudoku/{comboBox1.Text}"))
            {
                int counter = 0;
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    for(int i = 0; i<9; i++)
                    {
                        arr[i,counter].setText(values[i]);
                    }
                    counter++;
                }
            }
        }
    }
}
