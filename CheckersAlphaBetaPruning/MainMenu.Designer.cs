namespace CheckersAlphaBetaPruning
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.difficulty = new System.Windows.Forms.ComboBox();
            this.difficulty_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Papyrus", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(527, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "Alpha Beta Pruning Plays Checkers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Perpetua", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(136, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Developed by: Ravi Sinha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Perpetua", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(136, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "CS-UY 4613";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Perpetua", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(136, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 28);
            this.label4.TabIndex = 3;
            this.label4.Text = "Spring 2018";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(47, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(213, 87);
            this.button1.TabIndex = 4;
            this.button1.Text = "Play as Player 1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(321, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(213, 87);
            this.button2.TabIndex = 5;
            this.button2.Text = "Play as Player 2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // difficulty
            // 
            this.difficulty.FormattingEnabled = true;
            this.difficulty.Items.AddRange(new object[] {
            "Hard",
            "Medium",
            "Easy"});
            this.difficulty.Location = new System.Drawing.Point(290, 370);
            this.difficulty.Name = "difficulty";
            this.difficulty.Size = new System.Drawing.Size(121, 21);
            this.difficulty.TabIndex = 6;
            // 
            // difficulty_label
            // 
            this.difficulty_label.AutoSize = true;
            this.difficulty_label.Location = new System.Drawing.Point(198, 373);
            this.difficulty_label.Name = "difficulty_label";
            this.difficulty_label.Size = new System.Drawing.Size(86, 13);
            this.difficulty_label.TabIndex = 7;
            this.difficulty_label.Text = "Select Difficulty: ";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 419);
            this.Controls.Add(this.difficulty_label);
            this.Controls.Add(this.difficulty);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox difficulty;
        private System.Windows.Forms.Label difficulty_label;
    }
}