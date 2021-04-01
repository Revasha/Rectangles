using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDoubleClick
{
    public partial class PlayerForm : Form
    {
        public PlayerForm()
        {
            InitializeComponent();
        }

        private Pen pen = new Pen(Color.Black, 2);
        private Brush redBrush = Brushes.Red;
        private Brush blueBrush = Brushes.DarkBlue;
        private Brush white;

        private int click = 1; //1 - первый клик, 2 - второй, 3 - запрещено

        private bool indikator = true; //можно заносить или нет
        private bool player = true; //красный
        private bool counter = true; //0 раз для красного

        private int x, y = 0; //координаты мыши
        private int x2, y2 = 0; //абсолютное значение координат

        private int width = 3, height = 2; //ширина и высота
        private int lenghtW = 0, lenghtH = 0; // диапазон

        private int xRect, yRect = 1; //для зарисовки и добавления др. клеток
        private int widthplus = 0;
        private int heightplus = 0;
        private int regen = 0, regenBlue = 0;

        private List<int> X = new List<int>() { };
        private List<int> Y = new List<int>() { };

        private List<int> Xblue = new List<int>() { };
        private List<int> Yblue = new List<int>() { };

        private Random r = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            width = r.Next(1, 4);
            height = r.Next(1, 4);
            label3.Text = width.ToString();
            label4.Text = height.ToString();

            white = new SolidBrush(this.BackColor);
        }

        private void Draw(int size, int dx, int dy, int count_X, int count_Y)
        {
            Graphics g = panel1.CreateGraphics();

            for (int i = 0; i <= count_X; i++)
            {
                int x = i * size + dx;
                g.DrawLine(pen, x, dy, x, (count_X - 1) * size + dy);
            }

            for (int i = 0; i <= count_Y; i++)
            {
                int y = i * size + dy;
                g.DrawLine(pen, dx, y, (count_Y + 1) * size + dx, y);
            }
        }

        public int FindX(int x) // x,y - координаты мыши
        {
            x2 = x / 50;
            return x2 * 50;
        }

        public int FindY(int y) // x,y - координаты мыши
        {
            y2 = y / 50;
            return y2 * 50;
        }

        public void DrawRectangleRed(bool indikator)
        {
            Graphics g = panel1.CreateGraphics();
            for (int i = 0; i < X.Count; i++)
            {
                if (x2 == X[i] && y2 == Y[i])
                {
                    indikator = false;
                    break;
                }

            }
            if (indikator == true)
            {
                X.Add(x2);
                Y.Add(y2);
                g.FillRectangle(redBrush, x2, y2, 50, 50);
            }
        }

        public bool SameClick(int x2, int y2)
        {
            for (int i = 0; i < X.Count; i++)
            {
                if (x2 == X[i] && y2 == Y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void DrawRectangleBlue(bool indikator)
        {
            Graphics g = panel1.CreateGraphics();
            for (int i = 0; i < Xblue.Count; i++)
            {
                if (x2 == Xblue[i] && y2 == Yblue[i])
                {
                    indikator = false;
                    break;
                }

            }
            if (indikator == true)
            {
                Xblue.Add(x2);
                Yblue.Add(y2);
                g.FillRectangle(blueBrush, x2, y2, 50, 50);
            }
        }

        public bool SameClickBlue(int x2, int y2)
        {
            for (int i = 0; i < Xblue.Count; i++)
            {
                if (x2 == Xblue[i] && y2 == Yblue[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            lenghtW = 50 * (width - 1);  //диапазон по условию
            lenghtH = 50 * (height - 1);
            if (X.Count + Xblue.Count != 90)
            {
                if (player == true) //красный игрок
                {
                    if (click == 3)
                    {
                        if ((X[X.Count - 1] - X[X.Count - 2] == lenghtW) && (Y[Y.Count - 1] - Y[Y.Count - 2] == lenghtH)) //диапазон по факту
                        {
                            player = !player;
                            click = 1;
                            widthplus = 0;
                            heightplus = 0;

                            if (counter == false) //если не первый прямоугольник
                            {
                                widthplus = (X[X.Count - 2]) / 50; //для других прямоугольников на панели
                                heightplus = (Y[Y.Count - 2]) / 50;
                            }

                            for (int i = 0 + heightplus; i < height + heightplus; i++) //проходим по строкам
                            {
                                for (int j = 0 + widthplus; j < width + widthplus; j++) //по столбцам
                                {
                                    xRect = xRect * 50 * j; //
                                    yRect = yRect * 50 * i;

                                    if (xRect == X[X.Count - 2 - regen] && yRect == Y[Y.Count - 2 - regen]) //соотв. началу
                                    {
                                        xRect = 1; 
                                        yRect = 1;
                                    }
                                    else //все ок
                                    {
                                        if (xRect == X[X.Count - 1 - regen] && yRect == Y[Y.Count - 1 - regen]) //соотв. концу
                                        {
                                            xRect = 1;
                                            yRect = 1;
                                        }
                                        else //не уже имеющаяся точка конца
                                        {
                                            X.Add(xRect);
                                            Y.Add(yRect);
                                            g.FillRectangle(redBrush, xRect, yRect, 50, 50);
                                            xRect = 1;
                                            yRect = 1;
                                            regen++;
                                        }
                                    }
                                }
                            }
                            regen = 0;
                            counter = false;
                            width = r.Next(1, 4);
                            height = r.Next(1, 4);
                            label3.Text = width.ToString();
                            label4.Text = height.ToString();

                            textBox1.Text = "Синий игрок";
                            label1.Text = X.Count.ToString();
                        }
                        else //диапазон не совпал
                        {
                            for (int i = 1; i <= 2; i++)
                            {
                                g.FillRectangle(white, X[X.Count - i], Y[Y.Count - i], 50, 50);
                            }
                            X.RemoveRange(X.Count - 2, 2);
                            Y.RemoveRange(Y.Count - 2, 2);
                            click = 1;
                            MessageBox.Show("Прямоугольник неправильной формы. Попробуйте еще раз");
                        }
                    }

                    else if (click == 2 && (width == 1 && height == 1))
                    {
                        width = r.Next(1, 4);
                        height = r.Next(1, 4);
                        label3.Text = width.ToString();
                        label4.Text = height.ToString();
                        player = !player;
                        click = 1;

                        textBox1.Text = "Синий игрок";
                        label1.Text = X.Count.ToString();
                    }


                    else if (click == 2)
                    {
                        g.FillRectangle(white, X[X.Count - 1], Y[Y.Count - 1], 50, 50);
                        X.RemoveAt(X.Count - 1);
                        Y.RemoveAt(Y.Count - 1);
                        click = 1;
                        MessageBox.Show("Требуется две точки. Попробуйте еще раз");
                    }

                    else if (click == 1)
                    {
                        width = r.Next(1, 4);
                        height = r.Next(1, 4);
                        label3.Text = width.ToString();
                        label4.Text = height.ToString();
                        player = !player;

                        textBox1.Text = "Синий игрок";
                        label1.Text = X.Count.ToString();
                    }
                }
                else //синий игрок
                {
                    if (click == 3)
                    {
                        if ((Xblue[Xblue.Count - 2] - Xblue[Xblue.Count - 1] == lenghtW) && (Yblue[Yblue.Count - 2] - Yblue[Yblue.Count - 1] == lenghtH)) //диапазон по факту
                        {
                            player = !player;
                            click = 1;

                            for (int i = (Yblue[Yblue.Count - 1 - regenBlue]) / 50; i <= (Yblue[Yblue.Count - 2 - regenBlue]) / 50; i++)
                            {
                                for (int j = (Xblue[Xblue.Count - 1 - regenBlue]) / 50; j <= (Xblue[Xblue.Count - 2 - regenBlue]) / 50; j++)
                                {
                                    xRect = xRect * 50 * j; //
                                    yRect = yRect * 50 * i;

                                    if (xRect == Xblue[Xblue.Count - 1 - regenBlue] && yRect == Yblue[Yblue.Count - 1 - regenBlue]) //соотв. началу
                                    {
                                        xRect = 1; //
                                        yRect = 1;
                                    }
                                    else //все ок
                                    {
                                        if (xRect == Xblue[Xblue.Count - 2 - regenBlue] && yRect == Yblue[Yblue.Count - 2 - regenBlue]) //соотв. концу
                                        {
                                            xRect = 1;
                                            yRect = 1;
                                        }
                                        else //не уже имеющаяся точка конца
                                        {
                                            Xblue.Add(xRect);
                                            Yblue.Add(yRect);
                                            g.FillRectangle(blueBrush, xRect, yRect, 50, 50);
                                            xRect = 1;
                                            yRect = 1;
                                            regenBlue++;
                                        }
                                    }
                                }
                            }
                            regenBlue = 0;
                            width = r.Next(1, 4);
                            height = r.Next(1, 4);
                            label3.Text = width.ToString();
                            label4.Text = height.ToString();

                            textBox1.Text = "Красный игрок";
                            label2.Text = Xblue.Count.ToString();
                        }
                        else //неправильный диапазон
                        {
                            for (int i = 1; i <= 2; i++)
                            {
                                g.FillRectangle(white, Xblue[Xblue.Count - i], Yblue[Yblue.Count - i], 50, 50);
                            }
                            Xblue.RemoveRange(Xblue.Count - 2, 2);
                            Yblue.RemoveRange(Yblue.Count - 2, 2);
                            click = 1;
                            MessageBox.Show("Прямоугольник неправильной формы. Попробуйте еще раз");
                        }
                    }

                    else if (click == 2 && (width == 1 && height == 1))
                    {
                        width = r.Next(1, 4);
                        height = r.Next(1, 4);
                        label3.Text = width.ToString();
                        label4.Text = height.ToString();
                        click = 1;
                        player = !player;

                        textBox1.Text = "Красный игрок";
                        label2.Text = Xblue.Count.ToString();
                    }

                    else if (click == 2)
                    {
                        g.FillRectangle(white, Xblue[Xblue.Count - 1], Yblue[Yblue.Count - 1], 50, 50);
                        Xblue.RemoveAt(Xblue.Count - 1);
                        Yblue.RemoveAt(Yblue.Count - 1);
                        click = 1;
                        MessageBox.Show("Требуется две точки. Попробуйте еще раз");
                    }

                    else if (click == 1)
                    {
                        width = r.Next(1, 4);
                        height = r.Next(1, 4);
                        label3.Text = width.ToString();
                        label4.Text = height.ToString();
                        player = !player;

                        textBox1.Text = "Красный игрок";
                        label2.Text = Xblue.Count.ToString();
                    }
                }
            }
            else //конец игры
            {
                if(X.Count > Xblue.Count)
                {
                    MessageBox.Show("Победил красный игрок");
                    Environment.Exit(1);
                }

                else
                {
                    MessageBox.Show("Победил синий игрок");
                    Environment.Exit(2);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Draw(50, 0, 0, 10, 9);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            x = e.Location.X;
            y = e.Location.Y;
            indikator = true;
            Graphics g = panel1.CreateGraphics();

            if (player == true) //красный
            {
                if (click == 1) //1 клик
                {
                    x2 = FindX(x);
                    y2 = FindY(y);
                    if ((x2 == 0 && y2 == 0) || ((X.Contains(0) && Y.Contains(0))))
                    {
                        if (X.Count != 0)
                        {
                            if (SameClick(x2, y2) && SameClickBlue(x2,y2))
                            {
                                if ((X.Contains(x2 - 50) && Y.Contains(y2)) || (X.Contains(x2) && Y.Contains(y2 - 50)) || (X.Contains(x2 + 50) && Y.Contains(y2)) || (X.Contains(x2) && Y.Contains(y2 + 50)))
                                {
                                    DrawRectangleRed(indikator);
                                    click = 2;
                                }

                                x2 = 0; //Переприсовоение переменных для отрисовки заново
                                y2 = 0;
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Такая точка уже имеется, выберите другую");
                                x2 = 0; //Переприсовоение переменных для отрисовки заново
                                y2 = 0;
                                return;
                            }
                        }
                        else //если 0 элементов
                        {
                            DrawRectangleRed(indikator);
                            click = 2;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Выберите точку в начале координат");
                    }
                }
                else if (click == 2) //2 клик
                {
                    x2 = FindX(x);
                    y2 = FindY(y);

                    if (SameClick(x2, y2) && SameClickBlue(x2, y2))
                    {
                        DrawRectangleRed(indikator);
                        click = 3;
                    }

                    else
                    {
                        MessageBox.Show("Требуется две точки. Попробуйте еще раз");
                    }
                }

                else //если попытка сделать три клика
                {
                    MessageBox.Show("Передайте ход");
                }
            }

            else //синий
            {
                if (click == 1) //1 клик
                {
                    x2 = FindX(x);
                    y2 = FindY(y);
                    if ((x2 == 450 && y2 == 400) || ((Xblue.Contains(450) && Yblue.Contains(400)))) // поле 12x11 => (12-1)*50
                    {
                        if (Xblue.Count != 0)
                        {
                            if (SameClickBlue(x2, y2) && SameClick(x2, y2))
                            {
                                if ((Xblue.Contains(x2 - 50) && Yblue.Contains(y2)) || (Xblue.Contains(x2) && Yblue.Contains(y2 - 50)) || (Xblue.Contains(x2 + 50) && Yblue.Contains(y2)) || (Xblue.Contains(x2) && Yblue.Contains(y2 + 50)))
                                {
                                    DrawRectangleBlue(indikator);
                                    click = 2;
                                }

                                x2 = 0; //Переприсовоение переменных для отрисовки заново
                                y2 = 0;
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Такая точка уже имеется, выберите другую");
                                x2 = 0; //Переприсовоение переменных для отрисовки заново
                                y2 = 0;
                                return;
                            }
                        }
                        else //если 0 элементов
                        {
                            DrawRectangleBlue(indikator);
                            click = 2;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Требуется две точки. Попробуйте еще раз");
                    }
                }
                else if (click == 2) //2 клик
                {
                    x2 = FindX(x);
                    y2 = FindY(y);

                    if (SameClickBlue(x2, y2) && (SameClick(x2, y2)))
                    {
                        DrawRectangleBlue(indikator);
                        click = 3;
                    }

                    else
                    {
                        MessageBox.Show("Требуется две точки. Попробуйте еще раз");
                    }
                }

                else //try to 3
                {
                    MessageBox.Show("Передайте ход");
                }
            }
        }
    }
}
