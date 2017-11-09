namespace HiveTools
{
    partial class FormCustomConsole
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
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.buttonDiag = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConsole.Location = new System.Drawing.Point(13, 13);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConsole.Size = new System.Drawing.Size(446, 376);
            this.textBoxConsole.TabIndex = 0;
            this.textBoxConsole.TabStop = false;
            this.textBoxConsole.TextChanged += new System.EventHandler(this.textBoxConsole_TextChanged);
            // 
            // buttonDiag
            // 
            this.buttonDiag.Location = new System.Drawing.Point(94, 395);
            this.buttonDiag.Name = "buttonDiag";
            this.buttonDiag.Size = new System.Drawing.Size(75, 23);
            this.buttonDiag.TabIndex = 1;
            this.buttonDiag.Text = "Diag";
            this.buttonDiag.UseVisualStyleBackColor = true;
            this.buttonDiag.Click += new System.EventHandler(this.buttonDiag_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(13, 395);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // FormCustomConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 430);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonDiag);
            this.Controls.Add(this.textBoxConsole);
            this.Name = "FormCustomConsole";
            this.Text = "FormCustomConsole";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCustomConsole_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.Button buttonDiag;
        private System.Windows.Forms.Button buttonClear;
    }
}