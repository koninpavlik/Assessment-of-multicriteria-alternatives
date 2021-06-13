using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LabX
{
    public partial class Form1 : Form
    {
        int[,] matrix = new int[4, 3]; //Матрица альтернатив
        int[] w = new int[3]; //Матрица весов
        double[] L = new double[3]; //Матрица длин шкал
        double[,] Consent_matrix = new double[4, 4]; //Матрица согласия
        double[,] Disagreement_matrix = new double[4, 4]; //Матрица несогласия

        public Form1()
        {
            InitializeComponent();
            InitializeGrids();
        }

        private void InitializeGrids()
        {
            dataGridView1.RowCount = 4;
            dataGridView1.ColumnCount = 3;
            dataGridView2.RowCount = 4;
            dataGridView2.ColumnCount = 4;
            dataGridView3.RowCount = 4;
            dataGridView3.ColumnCount = 4;

            List<List<double>> initList = new List<List<double>>
            {
                new List<double> {280, 50, 10},
                new List<double> {200, 50, 60},
                new List<double> {180, 80, 20},
                new List<double> {150, 120, 50},
            };
            for (int i = 0; i < initList.Count; i++)
            {
                for (int j = 0; j < initList[i].Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = initList[i][j];
                }
            }
        }
        public void compareMc(int[,] mtr)
        {
            int c1 = 0;
            int c2 = 0;
            int n = 0; //Номер строки
            double sum_w = w[0] + w[1] + w[2]; //Сумма весов
            for (int i = 0; i < 4; i++)
            {
                for (int i1 = 0; i1 < 4; i1++)
                {
                    if ((i1 == 0) & (i == 0) & (n == 0)) continue;
                    if (((i1 == 1) & (i == 1) & (n == 1))) continue;
                    if (((i1 == 2) & (i == 2) & (n == 2))) continue;
                    if ((i1 == 3) & (i == 3) & (n == 3)) continue;
                    int sum_row = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        if (mtr[i, j] <= mtr[i1, j])
                        {
                            sum_row += w[j];
                        }
                    }
                    if (c2 > 3)
                    {
                        c1 += 1;
                        c2 = 0;
                    }

                    if (c1 == c2)
                    {
                        Consent_matrix[c1, c2] = 0;
                        c2 += 1;
                    }

                    Consent_matrix[c1, c2] = sum_row / sum_w;
                    c2 += 1;
                }

                n += 1;
            }
        }

        public void compareMd(int[,] mtr)
        {
            int c1 = 0;
            int c2 = 0;
            int n = 0; //Номер строки
            double[] d = new double[3]; //Индекс несогласия
            double res;
            for (int i = 0; i < 4; i++)
            {
                for (int i1 = 0; i1 < 4; i1++)
                {
                    if ((i1 == 0) & (i == 0) & (n == 0)) continue;
                    if (((i1 == 1) & (i == 1) & (n == 1))) continue;
                    if (((i1 == 2) & (i == 2) & (n == 2))) continue;
                    if ((i1 == 3) & (i == 3) & (n == 3)) continue;
                    Array.Clear(d, 0, 2);
                    for (int j = 0; j < 3; j++)
                    {
                        if (mtr[i, j] > mtr[i1, j])
                        {
                            res = (mtr[i, j] - mtr[i1, j]) / L[j];
                            d[j] = res;
                        }
                    }

                    if (c2 > 3)
                    {
                        c1 += 1;
                        c2 = 0;
                    }

                    if (c1 == c2)
                    {
                        Disagreement_matrix[c1, c2] = 0;
                        c2 += 1;
                    }

                    Disagreement_matrix[c1, c2] = d.Max();
                    c2 += 1;
                }

                n += 1;
            }
        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            //Задание уровней согласия и несогласия
            double a = Convert.ToDouble(numericUpDown8.Value);
            double l = Convert.ToDouble(numericUpDown7.Value);

            int[] sum_a = new int[4];
            int[] sum_l = new int[4];

            var n = dataGridView2.RowCount;
            var m = dataGridView2.ColumnCount;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (Convert.ToDouble(dataGridView2[j, i].Value) == a)
                    {
                        sum_a[i] += 1;
                    }
                    if (Convert.ToDouble(dataGridView3[j, i].Value) == l)
                    {
                        sum_l[i] += 1;
                    }
                }
            }

            int max1 = sum_a[0];
            int max2 = sum_l[0];
            int maxind1 = 0;
            int maxind2 = 0;
            for (int i = 0; i < 4; i++)
            {
                if (sum_a[i] > max1)
                {
                    max1 = sum_a[i];
                    maxind1 = i;
                }
                if (sum_l[i] > max2)
                {
                    max2 = sum_l[i];
                    maxind2 = i;
                }
            }

            if (maxind1 == maxind2)
            {
                richTextBox1.Text = "";
                if (maxind1 == 0) richTextBox1.Text += "Наилучшая альтернатива - A";
                if (maxind1 == 1) richTextBox1.Text += "Наилучшая альтернатива - B";
                if (maxind1 == 2) richTextBox1.Text += "Наилучшая альтернатива - C";
                if (maxind1 == 3) richTextBox1.Text += "Наилучшая альтернатива - D";
            }
            else richTextBox1.Text = "Необходимо продолжить вычисления...";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Заполнение матрицы из таблицы
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value);
                }
            }

            //Заполнение матрицы весов критериев
            w[0] = Convert.ToInt32(numericUpDown1.Value);
            w[1] = Convert.ToInt32(numericUpDown2.Value);
            w[2] = Convert.ToInt32(numericUpDown3.Value);

            //Заполнение матрицы длин шкал
            L[0] = Convert.ToInt32(numericUpDown4.Value);
            L[1] = Convert.ToInt32(numericUpDown5.Value);
            L[2] = Convert.ToInt32(numericUpDown6.Value);

            compareMc(matrix);
            compareMd(matrix);

            var n = dataGridView2.RowCount;
            var m = dataGridView2.ColumnCount;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    dataGridView2[j, i].Value = Math.Round(Consent_matrix[i, j], 2).ToString();
                    dataGridView3[j, i].Value = Math.Round(Disagreement_matrix[i, j], 2).ToString();
                }
            }
        }
    }

}