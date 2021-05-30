using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameWithButtons
{
    public partial class GameForm : Form
    {
        public Button[,] Cells;
        public bool player;
        enum CellType { SELECTED, FILLED_BY_FIRST, FILLED_BY_SECOND, CREATED };
        public Tuple<int, int> size;
        private Random random = new Random();

        public GameForm()
        {
            InitializeComponent();
            FillForm(SetFormSize(), 30);
            RollDice();
        }

        private void RollDice()
        {
            int x = random.Next(1, 7);
            int y = random.Next(1, 7);
            MessageBox.Show("Выпал кубик с гранями: {" + x + ";" + y + "}");
            size = Tuple.Create(x, y);
        }

        private void FillForm(int formSize, int cellSize)
        {
            Cells = new Button[formSize, formSize];
            for (int i = 0; i < formSize; i++)
            {
                for (int j = 0; j < formSize; j++)
                {
                    Button cell = new Button
                    {
                        Location = new Point(j * cellSize, i * cellSize),
                        Size = new Size(cellSize, cellSize),
                        BackColor = Color.Gray,
                        Tag = CellType.CREATED
                    };
                    cell.Click += new EventHandler(Cell_clicked);
                    Cells[i, j] = cell;
                    gamePanel.Controls.Add(cell);
                }
            }
        }

        private int SetFormSize()
        {
            int size = 10;
            bool isInteger;
            do
            {
                bool isNum = int.TryParse(Microsoft.VisualBasic.Interaction.InputBox("Введите размерность поля:"), out int result);
                if (isNum && result <= 20 && result > 0)
                {
                    size = result;
                    isInteger = true;
                }
                else
                {
                    MessageBox.Show("Введите число от 1 до 20");
                    isInteger = false;
                }
            }
            while (!isInteger);
            return size;
        }

        private void Cell_clicked(object sender, EventArgs e)
        {
            if (gamePanel.Controls.OfType<Button>().Where(n => n.Tag.Equals(CellType.SELECTED)).Count() > 1)
            {
                MessageBox.Show("Передайте ход");
                return;
            }

            Button selectedCell = sender as Button;

            if (selectedCell.Tag.Equals(CellType.FILLED_BY_FIRST)
                || selectedCell.Tag.Equals(CellType.FILLED_BY_SECOND)
                || selectedCell.Tag.Equals(CellType.SELECTED))
            {
                MessageBox.Show("Данная клетка уже занята");
                return;
            }

            if (size.Equals(Tuple.Create(1, 1)))
            {
                var indexes = GameUtils.CoordinatesOf(Cells, selectedCell);
                if (CanSetSoloFigure(indexes))
                {
                    if (player)
                    {
                        textPlayerMenu.Text = "Ход первого игрока";
                        selectedCell.Tag = CellType.FILLED_BY_SECOND;
                        selectedCell.BackColor = Color.Blue;
                    }
                    else
                    {
                        textPlayerMenu.Text = "Ход второго игрока";
                        selectedCell.Tag = CellType.FILLED_BY_FIRST;
                        selectedCell.BackColor = Color.Red;
                    }
                    player = !player;
                    RollDice();
                    return;
                }
                else
                {
                    ThrowCustomException(new List<Button> { selectedCell }, "Неверная позиция");
                    return;
                }
            }

            selectedCell.Tag = CellType.SELECTED;
            if (!player)
                selectedCell.BackColor = Color.Red;
            else
                selectedCell.BackColor = Color.Blue;
        }

        private bool IsWin()
        {
            var first = GetCellsByType(CellType.FILLED_BY_FIRST);
            var second = GetCellsByType(CellType.FILLED_BY_SECOND);
            var empty = GetCellsByType(CellType.CREATED);
            if (first.Count > second.Count + empty.Count)
                return true;
            else if (second.Count > first.Count + empty.Count)
                return true;
            return false;
        }

        private void NextTurn_Click(object sender, EventArgs e)
        {
            List<Button> selected = GetCellsByType(CellType.SELECTED);
            var selectedCells = Tuple.Create(selected[0], selected[1]);
            var firstIndexes = GameUtils.CoordinatesOf(Cells, selectedCells.Item1);
            var secondIndexes = GameUtils.CoordinatesOf(Cells, selectedCells.Item2);

            if (!GameUtils.IsShapeSizeCorrect(firstIndexes, secondIndexes, size))
            {
                ThrowCustomException(selected, "Неверный размер прямоугольника");
                return;
            }

            if (IsPlayerFirstAction(selectedCells))
            {
                if (WhenFirstActionCorrectSetPlayerType(selectedCells, firstIndexes, secondIndexes))
                {
                    if (!FillCells(firstIndexes, secondIndexes))
                        return;

                    if (player)
                        textPlayerMenu.Text = "Ход первого игрока";
                    else
                        textPlayerMenu.Text = "Ход второго игрока";
                    player = !player;
                    RollDice();
                    return;
                }
                else
                {
                    ThrowCustomException(selected, "Неверная позиция");
                    return;
                }
            }

            if (CanSetFigure(firstIndexes, secondIndexes))
            {
                if (!FillCells(firstIndexes, secondIndexes))
                    return;
                if (IsWin())
                {
                    if (player)
                        MessageBox.Show("Победил второй игрок");
                    else
                        MessageBox.Show("Победил первый игрок");
                    gamePanel.Enabled = false;
                    menuStrip1.Enabled = false;
                }
                if (player)
                    textPlayerMenu.Text = "Ход первого игрока";
                else
                    textPlayerMenu.Text = "Ход второго игрока";
                player = !player;
                RollDice();
                return;
            }
            else
            {
                ThrowCustomException(selected, "Неверная позиция");
                return;
            }
        }

        private bool CanSetFigure(Tuple<int, int> firstIndexes, Tuple<int, int> secondIndexes)
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if ((!player && !Cells[i, j].Tag.Equals(CellType.FILLED_BY_FIRST))
                        || (player && !Cells[i, j].Tag.Equals(CellType.FILLED_BY_SECOND)))
                        continue;

                    if ((Math.Abs(firstIndexes.Item1 - i) == 1 && firstIndexes.Item2 == j) || (Math.Abs(firstIndexes.Item2 - j) == 1 && firstIndexes.Item1 == i)
                        ||
                        (Math.Abs(secondIndexes.Item1 - i) == 1 && secondIndexes.Item2 == j) || (Math.Abs(secondIndexes.Item2 - j) == 1 && secondIndexes.Item1 == i))
                        return true;
                }
            }
            return false;
        }

        private bool CanSetSoloFigure(Tuple<int, int> indexes)
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if ((!player && !Cells[i, j].Tag.Equals(CellType.FILLED_BY_FIRST))
                        || (player && !Cells[i, j].Tag.Equals(CellType.FILLED_BY_SECOND)))
                        continue;
                    if ((Math.Abs(indexes.Item1 - i) == 1 && indexes.Item2 == j)
                        || (Math.Abs(indexes.Item2 - j) == 1 && indexes.Item1 == i))
                        return true;
                }
            }
            return false;
        }

        private bool FillCells(Tuple<int, int> firstIndexes, Tuple<int, int> secondIndexes)
        {
            List<Button> filledButtons = new List<Button>();
            int item1FI = firstIndexes.Item1;
            int item1SI = secondIndexes.Item1;
            int item2FI = firstIndexes.Item2;
            int item2SI = secondIndexes.Item2;
            //костыль, чтобы менять индексы при условии невыполнения цикла
            if (item2FI > item2SI)
            {
                item2FI = secondIndexes.Item2;
                item2SI = firstIndexes.Item2;
            }
            if (item1FI > item1SI)
            {
                item1FI = secondIndexes.Item1;
                item1SI = firstIndexes.Item1;
            }
            for (int i = item1FI; i <= item1SI; i++)
            {
                for (int j = item2FI; j <= item2SI; j++)
                {
                    filledButtons.Add(Cells[i, j]);
                    if (Cells[i, j].Tag.Equals(CellType.FILLED_BY_FIRST) || Cells[i, j].Tag.Equals(CellType.FILLED_BY_SECOND))
                    {
                        var selected = GetCellsByType(CellType.SELECTED);
                        selected.ForEach(s =>
                        {
                            if (!filledButtons.Contains(s))
                                filledButtons.Add(s);
                        });
                        ThrowCustomException(filledButtons, "Клетка уже занята");
                        return false;
                    }

                    if (!player)
                    {
                        Cells[i, j].Tag = CellType.FILLED_BY_FIRST;
                        Cells[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        Cells[i, j].Tag = CellType.FILLED_BY_SECOND;
                        Cells[i, j].BackColor = Color.Blue;
                    }

                }
            }
            return true;
        }

        private static void ThrowCustomException(List<Button> selected, string message)
        {
            MessageBox.Show(message);
            selected.ForEach(n =>
            {
                if (n.Tag.Equals(CellType.CREATED) || n.Tag.Equals(CellType.SELECTED))
                {
                    n.Tag = CellType.CREATED;
                    n.BackColor = Color.Gray;
                }
            });
        }

        private bool IsPlayerFirstAction(Tuple<Button, Button> selectedCells)
        {
            if (!player && GetCellsByType(CellType.FILLED_BY_FIRST).Count() == 0)
                return true;
            else if (player && GetCellsByType(CellType.FILLED_BY_SECOND).Count() == 0)
                return true;
            return false;
        }

        private bool WhenFirstActionCorrectSetPlayerType(Tuple<Button, Button> selectedCells, Tuple<int, int> firstIndexes, Tuple<int, int> secondIndexes)
        {
            if (!player && ((firstIndexes.Item1 == 0 && firstIndexes.Item2 == 0) || (secondIndexes.Item1 == 0 && secondIndexes.Item2 == 0)))
                return true;

            else if (player && ((firstIndexes.Item1 == Cells.GetLength(0) - 1 && firstIndexes.Item2 == Cells.GetLength(0) - 1)
                || (secondIndexes.Item1 == Cells.GetLength(1) - 1 && secondIndexes.Item2 == Cells.GetLength(1) - 1)))
                return true;

            return false;
        }

        private List<Button> GetCellsByType(CellType type)
        {
            return gamePanel.Controls.OfType<Button>().Where(n => n.Tag.Equals(type)).ToList();
        }

        private void ChangePlayer_Click(object sender, EventArgs e)
        {
            var selected = GetCellsByType(CellType.SELECTED);
            selected.ForEach(s =>
            {
                s.Tag = CellType.CREATED;
                s.BackColor = Color.Gray;
            });
            if (player)
                textPlayerMenu.Text = "Ход первого игрока";
            else
                textPlayerMenu.Text = "Ход второго игрока";
            player = !player;
            RollDice();
        }
    }
}
