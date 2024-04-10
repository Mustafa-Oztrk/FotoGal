namespace FotoGal
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panelKlasorler = new System.Windows.Forms.Panel();
            this.panelResimler = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_cp = new System.Windows.Forms.Button();
            this.btn_tkvm = new System.Windows.Forms.Button();
            this.btn_vsklik = new System.Windows.Forms.Button();
            this.textBoxArama = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dosya = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelKlasorler
            // 
            this.panelKlasorler.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelKlasorler.Location = new System.Drawing.Point(0, 78);
            this.panelKlasorler.Name = "panelKlasorler";
            this.panelKlasorler.Size = new System.Drawing.Size(221, 1065);
            this.panelKlasorler.TabIndex = 0;
            // 
            // panelResimler
            // 
            this.panelResimler.Location = new System.Drawing.Point(227, 84);
            this.panelResimler.Name = "panelResimler";
            this.panelResimler.Size = new System.Drawing.Size(1491, 1048);
            this.panelResimler.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.btn_cp);
            this.panel2.Controls.Add(this.btn_tkvm);
            this.panel2.Controls.Add(this.btn_vsklik);
            this.panel2.Controls.Add(this.textBoxArama);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(0, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1762, 84);
            this.panel2.TabIndex = 2;
            // 
            // btn_cp
            // 
            this.btn_cp.Location = new System.Drawing.Point(1134, 31);
            this.btn_cp.Name = "btn_cp";
            this.btn_cp.Size = new System.Drawing.Size(112, 40);
            this.btn_cp.TabIndex = 4;
            this.btn_cp.Text = "Çöp Kutusu";
            this.btn_cp.UseVisualStyleBackColor = true;
            this.btn_cp.Click += new System.EventHandler(this.btn_cp_Click);
            // 
            // btn_tkvm
            // 
            this.btn_tkvm.Location = new System.Drawing.Point(985, 31);
            this.btn_tkvm.Name = "btn_tkvm";
            this.btn_tkvm.Size = new System.Drawing.Size(112, 40);
            this.btn_tkvm.TabIndex = 3;
            this.btn_tkvm.Text = "Takvim";
            this.btn_tkvm.UseVisualStyleBackColor = true;
            this.btn_tkvm.Click += new System.EventHandler(this.btn_tkvm_Click);
            // 
            // btn_vsklik
            // 
            this.btn_vsklik.Location = new System.Drawing.Point(851, 31);
            this.btn_vsklik.Name = "btn_vsklik";
            this.btn_vsklik.Size = new System.Drawing.Size(112, 40);
            this.btn_vsklik.TabIndex = 2;
            this.btn_vsklik.Text = "Vesikalık";
            this.btn_vsklik.UseVisualStyleBackColor = true;
            this.btn_vsklik.Click += new System.EventHandler(this.btn_vsklik_Click);
            // 
            // textBoxArama
            // 
            this.textBoxArama.Location = new System.Drawing.Point(553, 31);
            this.textBoxArama.Name = "textBoxArama";
            this.textBoxArama.Size = new System.Drawing.Size(244, 20);
            this.textBoxArama.TabIndex = 1;
            this.textBoxArama.TextChanged += new System.EventHandler(this.textBoxArama_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dosya);
            this.panel1.Location = new System.Drawing.Point(13, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 57);
            this.panel1.TabIndex = 0;
            // 
            // dosya
            // 
            this.dosya.Location = new System.Drawing.Point(52, 11);
            this.dosya.Name = "dosya";
            this.dosya.Size = new System.Drawing.Size(112, 40);
            this.dosya.TabIndex = 0;
            this.dosya.Text = "Dosya Aç";
            this.dosya.UseVisualStyleBackColor = true;
            this.dosya.Click += new System.EventHandler(this.dosya_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1770, 1142);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelResimler);
            this.Controls.Add(this.panelKlasorler);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "FotoGal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelKlasorler;
        private System.Windows.Forms.Panel panelResimler;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button dosya;
        private System.Windows.Forms.TextBox textBoxArama;
        private System.Windows.Forms.Button btn_cp;
        private System.Windows.Forms.Button btn_tkvm;
        private System.Windows.Forms.Button btn_vsklik;
    }
}

