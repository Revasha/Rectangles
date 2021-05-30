namespace GameWithButtons
{
    partial class GameForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nextTurn = new System.Windows.Forms.ToolStripMenuItem();
            this.textPlayerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.gamePanel = new System.Windows.Forms.Panel();
            this.changePlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextTurn,
            this.textPlayerMenu,
            this.changePlayer});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(788, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nextTurn
            // 
            this.nextTurn.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.nextTurn.Name = "nextTurn";
            this.nextTurn.Size = new System.Drawing.Size(100, 24);
            this.nextTurn.Text = "Передача хода";
            this.nextTurn.Click += new System.EventHandler(this.NextTurn_Click);
            // 
            // textPlayerMenu
            // 
            this.textPlayerMenu.Enabled = false;
            this.textPlayerMenu.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.textPlayerMenu.Name = "textPlayerMenu";
            this.textPlayerMenu.Size = new System.Drawing.Size(128, 24);
            this.textPlayerMenu.Text = "Ход первого игрока";
            // 
            // gamePanel
            // 
            this.gamePanel.AutoSize = true;
            this.gamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamePanel.Location = new System.Drawing.Point(0, 28);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new System.Drawing.Size(788, 571);
            this.gamePanel.TabIndex = 1;
            // 
            // changePlayer
            // 
            this.changePlayer.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.changePlayer.Name = "changePlayer";
            this.changePlayer.Size = new System.Drawing.Size(96, 24);
            this.changePlayer.Text = "Смена игрока";
            this.changePlayer.Click += new System.EventHandler(this.ChangePlayer_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(788, 599);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Игра";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nextTurn;
        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.ToolStripMenuItem textPlayerMenu;
        private System.Windows.Forms.ToolStripMenuItem changePlayer;
    }
}

