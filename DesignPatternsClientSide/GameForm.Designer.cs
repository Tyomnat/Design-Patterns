
namespace DesignPatternsClientSide
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrMoving = new System.Windows.Forms.Timer(this.components);
            this.lives = new System.Windows.Forms.Label();
            this.gameOver = new System.Windows.Forms.Label();
            this.Win = new System.Windows.Forms.Label();
            this.pausedLabel = new System.Windows.Forms.Label();
            this.chatArea = new System.Windows.Forms.Label();
            this.chatInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tmrMoving
            // 
            this.tmrMoving.Enabled = true;
            this.tmrMoving.Tick += new System.EventHandler(this.tmrMoving_Tick_1);
            // 
            // lives
            // 
            this.lives.AutoSize = true;
            this.lives.BackColor = System.Drawing.Color.Transparent;
            this.lives.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lives.Location = new System.Drawing.Point(13, 13);
            this.lives.Name = "lives";
            this.lives.Size = new System.Drawing.Size(0, 21);
            this.lives.TabIndex = 0;
            // 
            // gameOver
            // 
            this.gameOver.BackColor = System.Drawing.Color.Transparent;
            this.gameOver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameOver.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameOver.ForeColor = System.Drawing.Color.Red;
            this.gameOver.Location = new System.Drawing.Point(0, 0);
            this.gameOver.Name = "gameOver";
            this.gameOver.Size = new System.Drawing.Size(544, 684);
            this.gameOver.TabIndex = 1;
            this.gameOver.Text = "GAME OVER";
            this.gameOver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.gameOver.Visible = false;
            // 
            // Win
            // 
            this.Win.BackColor = System.Drawing.Color.Transparent;
            this.Win.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Win.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Win.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.Win.Location = new System.Drawing.Point(0, 0);
            this.Win.Name = "Win";
            this.Win.Size = new System.Drawing.Size(544, 684);
            this.Win.TabIndex = 2;
            this.Win.Text = "YOU WIN";
            this.Win.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Win.Visible = false;
            // 
            // pausedLabel
            // 
            this.pausedLabel.BackColor = System.Drawing.Color.Transparent;
            this.pausedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pausedLabel.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pausedLabel.ForeColor = System.Drawing.Color.Orange;
            this.pausedLabel.Location = new System.Drawing.Point(0, 0);
            this.pausedLabel.Name = "pausedLabel";
            this.pausedLabel.Size = new System.Drawing.Size(544, 684);
            this.pausedLabel.TabIndex = 3;
            this.pausedLabel.Text = "PAUSED";
            this.pausedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pausedLabel.Visible = false;
            // 
            // chatArea
            // 
            this.chatArea.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chatArea.Location = new System.Drawing.Point(13, 540);
            this.chatArea.Name = "chatArea";
            this.chatArea.Size = new System.Drawing.Size(519, 97);
            this.chatArea.TabIndex = 4;
            // 
            // chatInput
            // 
            this.chatInput.Enabled = false;
            this.chatInput.Location = new System.Drawing.Point(12, 649);
            this.chatInput.Multiline = true;
            this.chatInput.Name = "chatInput";
            this.chatInput.Size = new System.Drawing.Size(520, 23);
            this.chatInput.TabIndex = 5;
            this.chatInput.TabStop = false;
            this.chatInput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.chatInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatInput_KeyDown);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 684);
            this.Controls.Add(this.chatInput);
            this.Controls.Add(this.chatArea);
            this.Controls.Add(this.pausedLabel);
            this.Controls.Add(this.Win);
            this.Controls.Add(this.gameOver);
            this.Controls.Add(this.lives);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmrMoving;
        private System.Windows.Forms.Label lives;
        private System.Windows.Forms.Label gameOver;
        private System.Windows.Forms.Label Win;
        private System.Windows.Forms.Label pausedLabel;
        private System.Windows.Forms.Label chatArea;
        private System.Windows.Forms.TextBox chatInput;
    }
}

