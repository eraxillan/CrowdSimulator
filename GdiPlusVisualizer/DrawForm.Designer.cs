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
            this.pbVisualizator = new System.Windows.Forms.PictureBox();
            this.lblFloor = new System.Windows.Forms.Label();
            this.cmbFloor = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pbVisualizator)).BeginInit();
            this.SuspendLayout();
            // 
            // pbVisualizator
            // 
            this.pbVisualizator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVisualizator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbVisualizator.Location = new System.Drawing.Point(12, 12);
            this.pbVisualizator.Name = "pbVisualizator";
            this.pbVisualizator.Size = new System.Drawing.Size(756, 420);
            this.pbVisualizator.TabIndex = 0;
            this.pbVisualizator.TabStop = false;
            this.pbVisualizator.Paint += new System.Windows.Forms.PaintEventHandler(this.pbVisualizator_Paint);
            this.pbVisualizator.MouseEnter += new System.EventHandler(this.pbVisualizator_MouseEnter);
            // 
            // lblFloor
            // 
            this.lblFloor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFloor.AutoSize = true;
            this.lblFloor.Location = new System.Drawing.Point(782, 12);
            this.lblFloor.Name = "lblFloor";
            this.lblFloor.Size = new System.Drawing.Size(113, 13);
            this.lblFloor.TabIndex = 1;
            this.lblFloor.Text = "Select floor to display: ";
            // 
            // cmbFloor
            // 
            this.cmbFloor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFloor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFloor.FormattingEnabled = true;
            this.cmbFloor.Location = new System.Drawing.Point(774, 28);
            this.cmbFloor.Name = "cmbFloor";
            this.cmbFloor.Size = new System.Drawing.Size(121, 21);
            this.cmbFloor.TabIndex = 2;
            this.cmbFloor.SelectedIndexChanged += new System.EventHandler(this.cmbFloor_SelectedIndexChanged);
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 444);
            this.Controls.Add(this.cmbFloor);
            this.Controls.Add(this.lblFloor);
            this.Controls.Add(this.pbVisualizator);
            this.DoubleBuffered = true;
            this.Name = "DrawForm";
            this.Text = "Building schema";
            this.Resize += new System.EventHandler(this.DrawForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbVisualizator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVisualizator;
        private System.Windows.Forms.Label lblFloor;
        private System.Windows.Forms.ComboBox cmbFloor;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

