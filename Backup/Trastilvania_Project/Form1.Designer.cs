namespace Trastilvania_Project
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutTrastilvaniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Reset_button = new System.Windows.Forms.Button();
            this.Run_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_Lambda_Trastes = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TB_Lambda_Huecos = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TB_Time = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TB_Exponential = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TB_Cobertura = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TB_Muros_V = new System.Windows.Forms.TextBox();
            this.TB_Muros_H = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TB_Muros = new System.Windows.Forms.TextBox();
            this.TB_Wobots = new System.Windows.Forms.TextBox();
            this.TB_Huecos = new System.Windows.Forms.TextBox();
            this.TB_Trastes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Drawing_Area = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.RoyalBlue;
            this.menuStrip1.Font = new System.Drawing.Font("Haettenschweiler", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(837, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Font = new System.Drawing.Font("Haettenschweiler", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(47, 25);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutTrastilvaniaToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutTrastilvaniaToolStripMenuItem
            // 
            this.aboutTrastilvaniaToolStripMenuItem.Name = "aboutTrastilvaniaToolStripMenuItem";
            this.aboutTrastilvaniaToolStripMenuItem.Size = new System.Drawing.Size(198, 26);
            this.aboutTrastilvaniaToolStripMenuItem.Text = "About Trastilvania...";
            // 
            // Reset_button
            // 
            this.Reset_button.Location = new System.Drawing.Point(634, 337);
            this.Reset_button.Name = "Reset_button";
            this.Reset_button.Size = new System.Drawing.Size(82, 27);
            this.Reset_button.TabIndex = 2;
            this.Reset_button.Text = "Reset";
            this.Reset_button.UseVisualStyleBackColor = true;
            this.Reset_button.Click += new System.EventHandler(this.Reset_button_Click);
            // 
            // Run_button
            // 
            this.Run_button.Enabled = false;
            this.Run_button.Location = new System.Drawing.Point(723, 337);
            this.Run_button.Name = "Run_button";
            this.Run_button.Size = new System.Drawing.Size(87, 27);
            this.Run_button.TabIndex = 3;
            this.Run_button.Text = "Run";
            this.Run_button.UseVisualStyleBackColor = true;
            this.Run_button.Click += new System.EventHandler(this.Run_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TB_Lambda_Trastes);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.TB_Lambda_Huecos);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.TB_Time);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.TB_Exponential);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TB_Cobertura);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.TB_Muros_V);
            this.groupBox1.Controls.Add(this.TB_Muros_H);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TB_Muros);
            this.groupBox1.Controls.Add(this.TB_Wobots);
            this.groupBox1.Controls.Add(this.TB_Huecos);
            this.groupBox1.Controls.Add(this.TB_Trastes);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(634, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 290);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parámetros de la simulación";
            // 
            // TB_Lambda_Trastes
            // 
            this.TB_Lambda_Trastes.Location = new System.Drawing.Point(110, 222);
            this.TB_Lambda_Trastes.MaxLength = 3;
            this.TB_Lambda_Trastes.Name = "TB_Lambda_Trastes";
            this.TB_Lambda_Trastes.Size = new System.Drawing.Size(44, 20);
            this.TB_Lambda_Trastes.TabIndex = 21;
            this.TB_Lambda_Trastes.Text = "10";
            this.TB_Lambda_Trastes.TextChanged += new System.EventHandler(this.TB_Lambda_Trastes_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 228);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Lambda Trastes:";
            // 
            // TB_Lambda_Huecos
            // 
            this.TB_Lambda_Huecos.Location = new System.Drawing.Point(110, 199);
            this.TB_Lambda_Huecos.MaxLength = 3;
            this.TB_Lambda_Huecos.Name = "TB_Lambda_Huecos";
            this.TB_Lambda_Huecos.Size = new System.Drawing.Size(44, 20);
            this.TB_Lambda_Huecos.TabIndex = 19;
            this.TB_Lambda_Huecos.Text = "1";
            this.TB_Lambda_Huecos.TextChanged += new System.EventHandler(this.TB_Lambda_Huecos_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 206);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Lambda Huecos:";
            // 
            // TB_Time
            // 
            this.TB_Time.Location = new System.Drawing.Point(141, 266);
            this.TB_Time.MaxLength = 3;
            this.TB_Time.Name = "TB_Time";
            this.TB_Time.Size = new System.Drawing.Size(44, 20);
            this.TB_Time.TabIndex = 17;
            this.TB_Time.Text = "30";
            this.TB_Time.TextChanged += new System.EventHandler(this.TB_Time_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 271);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Tiempo de Simulación:";
            // 
            // TB_Exponential
            // 
            this.TB_Exponential.Location = new System.Drawing.Point(92, 176);
            this.TB_Exponential.MaxLength = 3;
            this.TB_Exponential.Name = "TB_Exponential";
            this.TB_Exponential.Size = new System.Drawing.Size(44, 20);
            this.TB_Exponential.TabIndex = 15;
            this.TB_Exponential.Text = "1";
            this.TB_Exponential.TextChanged += new System.EventHandler(this.TB_Exponential_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Lambda Exp:";
            // 
            // TB_Cobertura
            // 
            this.TB_Cobertura.Location = new System.Drawing.Point(97, 244);
            this.TB_Cobertura.MaxLength = 3;
            this.TB_Cobertura.Name = "TB_Cobertura";
            this.TB_Cobertura.Size = new System.Drawing.Size(44, 20);
            this.TB_Cobertura.TabIndex = 13;
            this.TB_Cobertura.Text = "10";
            this.TB_Cobertura.TextChanged += new System.EventHandler(this.TB_Cobertura_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Cobertura_(D):";
            // 
            // TB_Muros_V
            // 
            this.TB_Muros_V.Location = new System.Drawing.Point(121, 151);
            this.TB_Muros_V.MaxLength = 3;
            this.TB_Muros_V.Name = "TB_Muros_V";
            this.TB_Muros_V.Size = new System.Drawing.Size(44, 20);
            this.TB_Muros_V.TabIndex = 11;
            this.TB_Muros_V.Text = "1";
            this.TB_Muros_V.TextChanged += new System.EventHandler(this.TB_Muros_V_TextChanged);
            // 
            // TB_Muros_H
            // 
            this.TB_Muros_H.Location = new System.Drawing.Point(121, 127);
            this.TB_Muros_H.MaxLength = 3;
            this.TB_Muros_H.Name = "TB_Muros_H";
            this.TB_Muros_H.Size = new System.Drawing.Size(44, 20);
            this.TB_Muros_H.TabIndex = 10;
            this.TB_Muros_H.Text = "1";
            this.TB_Muros_H.TextChanged += new System.EventHandler(this.TB_Muros_H_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Lambda Muros_V:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Lambda Muros_H:";
            // 
            // TB_Muros
            // 
            this.TB_Muros.Location = new System.Drawing.Point(89, 105);
            this.TB_Muros.MaxLength = 3;
            this.TB_Muros.Name = "TB_Muros";
            this.TB_Muros.Size = new System.Drawing.Size(44, 20);
            this.TB_Muros.TabIndex = 7;
            this.TB_Muros.Text = "10";
            this.TB_Muros.TextChanged += new System.EventHandler(this.TB_Muros_TextChanged);
            // 
            // TB_Wobots
            // 
            this.TB_Wobots.Location = new System.Drawing.Point(89, 81);
            this.TB_Wobots.MaxLength = 3;
            this.TB_Wobots.Name = "TB_Wobots";
            this.TB_Wobots.Size = new System.Drawing.Size(44, 20);
            this.TB_Wobots.TabIndex = 6;
            this.TB_Wobots.Text = "10";
            this.TB_Wobots.TextChanged += new System.EventHandler(this.TB_Wobots_TextChanged);
            // 
            // TB_Huecos
            // 
            this.TB_Huecos.Location = new System.Drawing.Point(89, 56);
            this.TB_Huecos.MaxLength = 3;
            this.TB_Huecos.Name = "TB_Huecos";
            this.TB_Huecos.Size = new System.Drawing.Size(44, 20);
            this.TB_Huecos.TabIndex = 5;
            this.TB_Huecos.Text = "10";
            this.TB_Huecos.TextChanged += new System.EventHandler(this.TB_Huecos_TextChanged);
            // 
            // TB_Trastes
            // 
            this.TB_Trastes.Location = new System.Drawing.Point(89, 28);
            this.TB_Trastes.MaxLength = 3;
            this.TB_Trastes.Name = "TB_Trastes";
            this.TB_Trastes.Size = new System.Drawing.Size(44, 20);
            this.TB_Trastes.TabIndex = 4;
            this.TB_Trastes.Text = "10";
            this.TB_Trastes.TextChanged += new System.EventHandler(this.TB_Trastes_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "No. Muros:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "No. Wobots:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "No. Huecos:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "No. Trastes:";
            // 
            // Drawing_Area
            // 
            this.Drawing_Area.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Drawing_Area.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Drawing_Area.BackgroundImage")));
            this.Drawing_Area.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Drawing_Area.Location = new System.Drawing.Point(0, 30);
            this.Drawing_Area.Name = "Drawing_Area";
            this.Drawing_Area.Size = new System.Drawing.Size(628, 335);
            this.Drawing_Area.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 365);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Run_button);
            this.Controls.Add(this.Reset_button);
            this.Controls.Add(this.Drawing_Area);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trastilvania & CyberSolutions CS Company contribution. ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutTrastilvaniaToolStripMenuItem;
        private System.Windows.Forms.Panel Drawing_Area;
        private System.Windows.Forms.Button Reset_button;
        private System.Windows.Forms.Button Run_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TB_Trastes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_Muros;
        private System.Windows.Forms.TextBox TB_Wobots;
        private System.Windows.Forms.TextBox TB_Huecos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TB_Muros_V;
        private System.Windows.Forms.TextBox TB_Muros_H;
        private System.Windows.Forms.TextBox TB_Cobertura;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TB_Time;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TB_Exponential;
        private System.Windows.Forms.TextBox TB_Lambda_Huecos;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TB_Lambda_Trastes;
        private System.Windows.Forms.Label label11;
    }
}

