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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Geometry", 1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Apertures", 1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Furniture", 1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("People", 1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawForm));
            this.pbVisualizator = new System.Windows.Forms.PictureBox();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.lblFloor = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPan = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCursorPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblBuildingExtent = new System.Windows.Forms.ToolStripStatusLabel();
            this.grdProps = new System.Windows.Forms.PropertyGrid();
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLoadData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReload = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUnloadData = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropsBuilding = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropsFloor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropsRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropsBox = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropsAperture = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropsStairway = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVisualization = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVisCurrentFloor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCmbCurrentFloor = new System.Windows.Forms.ToolStripComboBox();
            this.dlgDataDir = new System.Windows.Forms.FolderBrowserDialog();
            this.lstDataFiles = new System.Windows.Forms.ListView();
            this.clnFileType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imlDataStatus = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbVisualizator)).BeginInit();
            this.stsMain.SuspendLayout();
            this.mnsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbVisualizator
            // 
            this.pbVisualizator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVisualizator.BackColor = System.Drawing.Color.White;
            this.pbVisualizator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbVisualizator.Location = new System.Drawing.Point(12, 27);
            this.pbVisualizator.Name = "pbVisualizator";
            this.pbVisualizator.Size = new System.Drawing.Size(776, 423);
            this.pbVisualizator.TabIndex = 0;
            this.pbVisualizator.TabStop = false;
            this.pbVisualizator.Paint += new System.Windows.Forms.PaintEventHandler(this.pbVisualizator_Paint);
            this.pbVisualizator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbVisualizator_MouseDown);
            this.pbVisualizator.MouseEnter += new System.EventHandler(this.pbVisualizator_MouseEnter);
            this.pbVisualizator.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbVisualizator_MouseMove);
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFloor,
            this.lblScale,
            this.lblPan,
            this.lblCursorPos,
            this.lblBuildingExtent});
            this.stsMain.Location = new System.Drawing.Point(0, 453);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(1032, 22);
            this.stsMain.TabIndex = 3;
            this.stsMain.Text = "statusStrip1";
            // 
            // lblFloor
            // 
            this.lblFloor.Name = "lblFloor";
            this.lblFloor.Size = new System.Drawing.Size(128, 17);
            this.lblFloor.Text = "Floor number: <none>";
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
            this.grdProps.Location = new System.Drawing.Point(794, 133);
            this.grdProps.Name = "grdProps";
            this.grdProps.Size = new System.Drawing.Size(235, 317);
            this.grdProps.TabIndex = 4;
            // 
            // mnsMain
            // 
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuProperties,
            this.mnuVisualization});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.ShowItemToolTips = true;
            this.mnsMain.Size = new System.Drawing.Size(1032, 24);
            this.mnsMain.TabIndex = 5;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLoadData,
            this.mnuReload,
            this.mnuUnloadData,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuLoadData
            // 
            this.mnuLoadData.Name = "mnuLoadData";
            this.mnuLoadData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuLoadData.Size = new System.Drawing.Size(257, 22);
            this.mnuLoadData.Text = "Load data from directory...";
            this.mnuLoadData.Click += new System.EventHandler(this.mnuLoadData_Click);
            // 
            // mnuReload
            // 
            this.mnuReload.Name = "mnuReload";
            this.mnuReload.Size = new System.Drawing.Size(257, 22);
            this.mnuReload.Text = "Reload data";
            this.mnuReload.Click += new System.EventHandler(this.mnuReload_Click);
            // 
            // mnuUnloadData
            // 
            this.mnuUnloadData.Enabled = false;
            this.mnuUnloadData.Name = "mnuUnloadData";
            this.mnuUnloadData.Size = new System.Drawing.Size(257, 22);
            this.mnuUnloadData.Text = "Unload data and clean screen";
            this.mnuUnloadData.Click += new System.EventHandler(this.mnuUnloadData_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mnuExit.Size = new System.Drawing.Size(257, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.ToolTipText = "Exit from the application";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuProperties
            // 
            this.mnuProperties.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPropsBuilding,
            this.mnuPropsFloor,
            this.mnuPropsRoom,
            this.mnuPropsBox,
            this.mnuPropsAperture,
            this.mnuPropsStairway});
            this.mnuProperties.Enabled = false;
            this.mnuProperties.Name = "mnuProperties";
            this.mnuProperties.Size = new System.Drawing.Size(72, 20);
            this.mnuProperties.Text = "Properties";
            // 
            // mnuPropsBuilding
            // 
            this.mnuPropsBuilding.Checked = true;
            this.mnuPropsBuilding.CheckOnClick = true;
            this.mnuPropsBuilding.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuPropsBuilding.Name = "mnuPropsBuilding";
            this.mnuPropsBuilding.Size = new System.Drawing.Size(120, 22);
            this.mnuPropsBuilding.Text = "Building";
            // 
            // mnuPropsFloor
            // 
            this.mnuPropsFloor.CheckOnClick = true;
            this.mnuPropsFloor.Name = "mnuPropsFloor";
            this.mnuPropsFloor.Size = new System.Drawing.Size(120, 22);
            this.mnuPropsFloor.Text = "Floor";
            // 
            // mnuPropsRoom
            // 
            this.mnuPropsRoom.CheckOnClick = true;
            this.mnuPropsRoom.Name = "mnuPropsRoom";
            this.mnuPropsRoom.Size = new System.Drawing.Size(120, 22);
            this.mnuPropsRoom.Text = "Room";
            // 
            // mnuPropsBox
            // 
            this.mnuPropsBox.CheckOnClick = true;
            this.mnuPropsBox.Name = "mnuPropsBox";
            this.mnuPropsBox.Size = new System.Drawing.Size(120, 22);
            this.mnuPropsBox.Text = "Box";
            // 
            // mnuPropsAperture
            // 
            this.mnuPropsAperture.CheckOnClick = true;
            this.mnuPropsAperture.Enabled = false;
            this.mnuPropsAperture.Name = "mnuPropsAperture";
            this.mnuPropsAperture.Size = new System.Drawing.Size(120, 22);
            this.mnuPropsAperture.Text = "Aperture";
            // 
            // mnuPropsStairway
            // 
            this.mnuPropsStairway.CheckOnClick = true;
            this.mnuPropsStairway.Enabled = false;
            this.mnuPropsStairway.Name = "mnuPropsStairway";
            this.mnuPropsStairway.Size = new System.Drawing.Size(120, 22);
            this.mnuPropsStairway.Text = "Stairway";
            // 
            // mnuVisualization
            // 
            this.mnuVisualization.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVisCurrentFloor});
            this.mnuVisualization.Enabled = false;
            this.mnuVisualization.Name = "mnuVisualization";
            this.mnuVisualization.Size = new System.Drawing.Size(85, 20);
            this.mnuVisualization.Text = "Visualization";
            // 
            // mnuVisCurrentFloor
            // 
            this.mnuVisCurrentFloor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCmbCurrentFloor});
            this.mnuVisCurrentFloor.Name = "mnuVisCurrentFloor";
            this.mnuVisCurrentFloor.Size = new System.Drawing.Size(142, 22);
            this.mnuVisCurrentFloor.Text = "Current floor";
            // 
            // mnuCmbCurrentFloor
            // 
            this.mnuCmbCurrentFloor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mnuCmbCurrentFloor.Name = "mnuCmbCurrentFloor";
            this.mnuCmbCurrentFloor.Size = new System.Drawing.Size(121, 23);
            this.mnuCmbCurrentFloor.SelectedIndexChanged += new System.EventHandler(this.mnuCmbCurrentFloor_SelectedIndexChanged);
            // 
            // dlgDataDir
            // 
            this.dlgDataDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // lstDataFiles
            // 
            this.lstDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDataFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clnFileType,
            this.clnName});
            this.lstDataFiles.FullRowSelect = true;
            this.lstDataFiles.GridLines = true;
            this.lstDataFiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.lstDataFiles.Location = new System.Drawing.Point(794, 27);
            this.lstDataFiles.MultiSelect = false;
            this.lstDataFiles.Name = "lstDataFiles";
            this.lstDataFiles.ShowItemToolTips = true;
            this.lstDataFiles.Size = new System.Drawing.Size(235, 100);
            this.lstDataFiles.SmallImageList = this.imlDataStatus;
            this.lstDataFiles.TabIndex = 6;
            this.lstDataFiles.UseCompatibleStateImageBehavior = false;
            this.lstDataFiles.View = System.Windows.Forms.View.Details;
            // 
            // clnFileType
            // 
            this.clnFileType.Text = "File type";
            this.clnFileType.Width = 85;
            // 
            // clnName
            // 
            this.clnName.Text = "File name";
            this.clnName.Width = 139;
            // 
            // imlDataStatus
            // 
            this.imlDataStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlDataStatus.ImageStream")));
            this.imlDataStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imlDataStatus.Images.SetKeyName(0, "imgOk");
            this.imlDataStatus.Images.SetKeyName(1, "imgError");
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 475);
            this.Controls.Add(this.lstDataFiles);
            this.Controls.Add(this.grdProps);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.mnsMain);
            this.Controls.Add(this.pbVisualizator);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mnsMain;
            this.Name = "DrawForm";
            this.Text = "Building schema";
            this.Resize += new System.EventHandler(this.DrawForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbVisualizator)).EndInit();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVisualizator;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel lblScale;
        private System.Windows.Forms.ToolStripStatusLabel lblBuildingExtent;
        private System.Windows.Forms.ToolStripStatusLabel lblPan;
        private System.Windows.Forms.ToolStripStatusLabel lblCursorPos;
        private System.Windows.Forms.PropertyGrid grdProps;
        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuLoadData;
        private System.Windows.Forms.ToolStripMenuItem mnuUnloadData;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuPropsBuilding;
        private System.Windows.Forms.ToolStripMenuItem mnuPropsFloor;
        private System.Windows.Forms.ToolStripMenuItem mnuPropsRoom;
        private System.Windows.Forms.ToolStripMenuItem mnuPropsBox;
        private System.Windows.Forms.ToolStripMenuItem mnuPropsAperture;
        private System.Windows.Forms.ToolStripMenuItem mnuPropsStairway;
        private System.Windows.Forms.ToolStripMenuItem mnuVisualization;
        private System.Windows.Forms.ToolStripMenuItem mnuVisCurrentFloor;
        private System.Windows.Forms.ToolStripComboBox mnuCmbCurrentFloor;
        private System.Windows.Forms.FolderBrowserDialog dlgDataDir;
        private System.Windows.Forms.ToolStripMenuItem mnuReload;
        private System.Windows.Forms.ListView lstDataFiles;
        private System.Windows.Forms.ColumnHeader clnFileType;
        private System.Windows.Forms.ColumnHeader clnName;
        private System.Windows.Forms.ImageList imlDataStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblFloor;
    }
}

