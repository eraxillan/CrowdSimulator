namespace GdiPlusVisualizer
{
    partial class DrawForm
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
            this.SuspendLayout();
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 444);
            this.Name = "DrawForm";
            this.Text = "Building schema";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DrawForm_KeyPress);
            this.Resize += new System.EventHandler(this.DrawForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

