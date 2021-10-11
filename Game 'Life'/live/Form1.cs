using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



// Create by SKIF4A -> https://github.com/evilSKIF4A


namespace live
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Graphics graphics; // глобально объявляем, чтобы отрисовывать графику
        public bool[,] life; // хранит всю информацию о клетках. То есть живая клетка, пустая и т.д.
        public int stroki; // нужны чтобы определить размер для массива 
        public int stolbci;

        public int CountNeigth(int x, int y)
        {
            int count = 0;
            // с помощью двух циклов, можно получить информацию о соседях. То есть отняв от x - 1. получим информацию о соседе слево, а если добавить единицу то информацию о соседе справо
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    // получим всю информацию о соседях вокруг
                    int x0 = (x + i + stroki) % stroki;
                    int y0 = (y + j + stolbci) % stolbci; // Как будто шар, то есть если координаты 0,0, то надо посмотреть уже с другой стороны карты.

                    bool etokletka = x0 == x && y0 == y; // чтобы мы не считали саму клетку.
                    bool estsosedkletka = life[x0, y0]; // если сосед живая клетка
                    if(estsosedkletka && !etokletka)
                    {
                        count = count + 1; // количество соседей
                    }
                }
            }

            return count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            graphics.Clear(Color.Black); // создаем поле для игры. Оно будет черным

            bool[,] newlife = new bool[stroki, stolbci]; // нужна для того, чтобы создавать новые клетки


            for (int x = 0; x < stroki; x++)
            {
                for (int y = 0; y < stolbci; y++)
                {
                    int neigthCount = CountNeigth(x, y); // количество соседей 
                    bool nlife = life[x, y]; // есть ли живая клетка по текущим координатам

                    //правила генерирования следующего поколения
                    if (!nlife && neigthCount == 3)
                    {
                        newlife[x, y] = true; // зараждается клетка

                    }else if(nlife && (neigthCount < 2 || neigthCount > 3))
                    {
                        newlife[x, y] = false; // клетки не будет
                    }
                    else
                    {
                        newlife[x, y] = life[x, y]; // если ничего не выполнилось, то все клетки остаются там где нужно
                    }

                    if (nlife)
                    {
                        graphics.FillRectangle(Brushes.Green, x*4, y*4, 4, 4); // отрисовывает все клетки. Размер 4х4. Координаты смещаются в 4 раза,чтобы клетки отрисовывались по всему pictureBox.
                    }
                }
            }


            life = newlife; // после всего что выполнится, мы сохраняем все в массив life;
            pictureBox1.Refresh(); // обновляем pictureBox
        }

        // старт
        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled) return; // если таймер включен, то выйти из метода

            stroki = pictureBox1.Width / 4; // делим на 4, чтобы увеличить масштаб 
            stolbci = pictureBox1.Height / 4;
            life = new bool[stroki, stolbci];

            Random ran = new Random(); // рандомно создаем первое поколение клеток
            for(int x = 0; x < stroki; x++)
            {
                for(int y = 0; y < stolbci; y++)
                {
                    life[x, y] = ran.Next(10) == 0; // рандомно генерируются клетки. Если рандомное число будет 0, то будет true и клетка создаться.
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Enabled = true;
        }

        // стоп
        private void button2_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                return; // если таймер выключен, то ничего не делать 
            }
            else
            {
                timer1.Enabled = false;
                graphics.Clear(Color.Black); // очищаем поле
                pictureBox1.Refresh(); // обновляем pictureBox
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled) return; // если таймер выключен, чтобы ничего нельзя было делать

            if(e.Button == MouseButtons.Left) // Если левая кнопка нажата, то он создает клетку
            {
                int x = e.Location.X / 4; // делим на 4, потому что если не делить, то таких координат не будет в массие life
                int y = e.Location.Y / 4;
                if(x >= 0 && y >= 0 && x < stroki && y < stolbci) // условие, чтобы не выходило за границы
                {
                    life[x, y] = true;
                }
            }
            if(e.Button == MouseButtons.Right) // Если правая кнопка нажата, то он удаляет клетку
            {
                int x = e.Location.X / 4; // делим на 4, потому что если не делить, то таких координат не будет в массие life
                int y = e.Location.Y / 4;
                if (x >= 0 && y >= 0 && x < stroki && y < stolbci) // условие, чтобы не выходило за границы
                {
                    life[x, y] = false;
                }
            }
        }
    }
}
