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
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPan = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCursorPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBuildingExtent = new System.Windows.Forms.ToolStripStatusLabel();
            this.grdProps = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.pbVisualizator)).BeginInit();
            this.stsMain.SuspendLayout();
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
            this.pbVisualizator.Size = new System.Drawing.Size(776, 407);
            this.pbVisualizator.TabIndex = 0;
            this.pbVisualizator.TabStop = false;
            this.pbVisualizator.Paint += new System.Windows.Forms.PaintEventHandler(this.pbVisualizator_Paint);
            this.pbVisualizator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbVisualizator_MouseDown);
            this.pbVisualizator.MouseEnter += new System.EventHandler(this.pbVisualizator_MouseEnter);
            this.pbVisualizator.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbVisualizator_MouseMove);
            // 
            // lblFloor
            // 
            this.lblFloor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFloor.AutoSize = true;
            this.lblFloor.Location = new System.Drawing.Point(794, 12);
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
            this.cmbFloor.Location = new System.Drawing.Point(794, 28);
            this.cmbFloor.Name = "cmbFloor";
            this.cmbFloor.Size = new System.Drawing.Size(113, 21);
            this.cmbFloor.TabIndex = 2;
            this.cmbFloor.SelectedIndexChanged += new System.EventHandler(this.cmbFloor_SelectedIndexChanged);
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblScale,
            this.lblPan,
            this.lblCursorPos,
            this.lblBuildingExtent});
            this.stsMain.Location = new System.Drawing.Point(0, 422);
            this.stsMain.Name = "stsMain";
            this.stsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.stsMain.Size = new System.Drawing.Size(1032, 22);
            this.stsMain.TabIndex = 3;
            this.stsMain.Text = "statusStrip1";
            // 
            // lblScale
            // 
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(68, 17);
            this.lblScale.Text = "Scale: 100%";
            // 
            // lblPan
            // 
            this.lblPan.Name = "lblPan";
            this.lblPan.Size = new System.Drawing.Size(99, 17);
            this.lblPan.Text = "Pan: <unknown>";
            // 
            // lblCursorPos
            // 
            this.lblCursorPos.Name = "lblCursorPos";
            this.lblCursorPos.Size = new System.Drawing.Size(205, 17);
            this.lblCursorPos.Text = "Cursor position (device): <unknown>";
            // 
            // lblBuildingExtent
            // 
            this.lblBuildingExtent.Name = "lblBuildingExtent";
            this.lblBuildingExtent.Size = new System.Drawing.Size(161, 17);
            this.lblBuildingExtent.Text = "Building extent:  <unknown>";
            // 
            // grdProps
            // 
            this.grdProps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdProps.Location = new System.Drawing.Point(794, 55);
            this.grdProps.Name = "grdProps";
            this.grdProps.Size = new System.Drawing.Size(235, 364);
            this.grdProps.TabIndex = 4;
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 444);
            this.Controls.Add(this.grdProps);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.cmbFloor);
            this.Controls.Add(this.lblFloor);
            this.Controls.Add(this.pbVisualizator);
            this.DoubleBuffered = true;
            this.Name = "DrawForm";
            this.Text = "Building schema";
            this.Resize += new System.EventHandler(this.DrawForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbVisualizator)).EndInit();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVisualizator;
        private System.Windows.Forms.Label lblFloor;
        private System.Windows.Forms.ComboBox cmbFloor;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private System.Windows.Forms.ToolStripStatusLabel lblBuildingExtent;
        private System.Windows.Forms.ToolStripStatusLabel lblPan;
        private System.Windows.Forms.ToolStripStatusLabel lblCursorPos;
        private System.Windows.Forms.PropertyGrid grdProps;
    }
}

