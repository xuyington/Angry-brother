


namespace LIBS
{
	partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lbxMsg = new System.Windows.Forms.ListBox();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.tpStop = new System.Windows.Forms.ToolTip(this.components);
            this.tpStart = new System.Windows.Forms.ToolTip(this.components);
            this.panelSpectrum = new BSE.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.plotSpectrum = new OxyPlot.WindowsForms.Plot();
            this.toolStripPanel = new System.Windows.Forms.ToolStrip();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveSpectrum = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLoadSpectrum = new System.Windows.Forms.ToolStripButton();
            this.btnZoom = new System.Windows.Forms.ToolStripButton();
            this.btnNarrow = new System.Windows.Forms.ToolStripButton();
            this.btnZoomRst = new System.Windows.Forms.ToolStripButton();
            this.btnYZoom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPeakLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.tbPeakPosition = new System.Windows.Forms.ToolStripTextBox();
            this.btnSaveAsExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.滤噪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.基线校正ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter2 = new BSE.Windows.Forms.Splitter();
            this.splitter1 = new BSE.Windows.Forms.Splitter();
            this.splitter4 = new BSE.Windows.Forms.Splitter();
            this.panelSetting = new BSE.Windows.Forms.Panel();
            this.btnAnalysis = new System.Windows.Forms.Button();
            this.tbnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPauseCam = new System.Windows.Forms.Button();
            this.btnStartCam = new System.Windows.Forms.Button();
            this.pbxImage = new System.Windows.Forms.PictureBox();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Start = new System.Windows.Forms.Button();
            this.tlpLibsMode = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveSpec = new System.Windows.Forms.Button();
            this.保存名称及路径 = new System.Windows.Forms.Label();
            this.ckbAutoSaveSpectrum = new System.Windows.Forms.CheckBox();
            this.ckbLibsModel = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LIBS模式 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbRange = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tlpSpectrometer = new System.Windows.Forms.TableLayoutPanel();
            this.numDelayTime = new System.Windows.Forms.NumericUpDown();
            this.lblIntegrationTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudScansToAverage = new System.Windows.Forms.NumericUpDown();
            this.lblScansToAverage = new System.Windows.Forms.Label();
            this.lblExternalTriggerMode = new System.Windows.Forms.Label();
            this.cbxExternalTriggerMode = new System.Windows.Forms.ComboBox();
            this.nudIntegrationTime = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelSpectrum.SuspendLayout();
            this.toolStripPanel.SuspendLayout();
            this.panelSetting.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImage)).BeginInit();
            this.tlpLibsMode.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlpSpectrometer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScansToAverage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntegrationTime)).BeginInit();
            this.SuspendLayout();
            // 
            // lbxMsg
            // 
            this.lbxMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbxMsg.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxMsg.ItemHeight = 18;
            this.lbxMsg.Location = new System.Drawing.Point(400, 606);
            this.lbxMsg.Margin = new System.Windows.Forms.Padding(4);
            this.lbxMsg.Name = "lbxMsg";
            this.lbxMsg.Size = new System.Drawing.Size(859, 94);
            this.lbxMsg.TabIndex = 10;
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(189, 39);
            this.miniToolStrip.TabIndex = 15;
            // 
            // panelSpectrum
            // 
            this.panelSpectrum.AssociatedSplitter = null;
            this.panelSpectrum.BackColor = System.Drawing.Color.Transparent;
            this.panelSpectrum.CaptionFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelSpectrum.CaptionHeight = 27;
            this.panelSpectrum.Controls.Add(this.listView1);
            this.panelSpectrum.Controls.Add(this.plotSpectrum);
            this.panelSpectrum.Controls.Add(this.toolStripPanel);
            this.panelSpectrum.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(65)))), ((int)(((byte)(118)))));
            this.panelSpectrum.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panelSpectrum.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panelSpectrum.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.panelSpectrum.CustomColors.CaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))));
            this.panelSpectrum.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.panelSpectrum.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(222)))));
            this.panelSpectrum.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(203)))), ((int)(((byte)(136)))));
            this.panelSpectrum.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panelSpectrum.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panelSpectrum.CustomColors.ContentGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this.panelSpectrum.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.panelSpectrum.CustomColors.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelSpectrum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpectrum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelSpectrum.Image = null;
            this.panelSpectrum.Location = new System.Drawing.Point(413, 10);
            this.panelSpectrum.Margin = new System.Windows.Forms.Padding(4);
            this.panelSpectrum.MinimumSize = new System.Drawing.Size(27, 27);
            this.panelSpectrum.Name = "panelSpectrum";
            this.panelSpectrum.ShowCaptionbar = false;
            this.panelSpectrum.Size = new System.Drawing.Size(846, 576);
            this.panelSpectrum.TabIndex = 23;
            this.panelSpectrum.Text = "谱线面板";
            this.panelSpectrum.ToolTipTextCloseIcon = null;
            this.panelSpectrum.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelSpectrum.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.SystemColors.Info;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(622, 38);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(212, 512);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "查找谱线波长";
            this.columnHeader1.Width = 108;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "强度值";
            this.columnHeader2.Width = 86;
            // 
            // plotSpectrum
            // 
            this.plotSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotSpectrum.KeyboardPanHorizontalStep = 0.1D;
            this.plotSpectrum.KeyboardPanVerticalStep = 0.1D;
            this.plotSpectrum.Location = new System.Drawing.Point(0, 27);
            this.plotSpectrum.Margin = new System.Windows.Forms.Padding(4);
            this.plotSpectrum.Name = "plotSpectrum";
            this.plotSpectrum.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotSpectrum.Size = new System.Drawing.Size(615, 549);
            this.plotSpectrum.TabIndex = 3;
            this.plotSpectrum.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotSpectrum.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotSpectrum.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // toolStripPanel
            // 
            this.toolStripPanel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart,
            this.btnStop,
            this.toolStripSeparator3,
            this.btnSaveSpectrum,
            this.toolStripSeparator1,
            this.btnLoadSpectrum,
            this.btnZoom,
            this.btnNarrow,
            this.btnZoomRst,
            this.btnYZoom,
            this.toolStripSeparator2,
            this.btnPeakLine,
            this.toolStripButton4,
            this.tbPeakPosition,
            this.btnSaveAsExcel,
            this.toolStripButton1,
            this.toolStripSeparator4,
            this.toolStripButton3,
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.toolStripTextBox1,
            this.toolStripButton2,
            this.toolStripSeparator5,
            this.toolStripDropDownButton1});
            this.toolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.toolStripPanel.Name = "toolStripPanel";
            this.toolStripPanel.Size = new System.Drawing.Size(846, 27);
            this.toolStripPanel.TabIndex = 1;
            this.toolStripPanel.Text = "toolStrip1";
            // 
            // btnStart
            // 
            this.btnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(29, 24);
            this.btnStart.Text = "开始";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(29, 24);
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // btnSaveSpectrum
            // 
            this.btnSaveSpectrum.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveSpectrum.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveSpectrum.Image")));
            this.btnSaveSpectrum.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveSpectrum.Name = "btnSaveSpectrum";
            this.btnSaveSpectrum.Size = new System.Drawing.Size(29, 24);
            this.btnSaveSpectrum.Text = "设置光谱存储路径（默认为C:）";
            this.btnSaveSpectrum.Click += new System.EventHandler(this.btnSaveSpectrum_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnLoadSpectrum
            // 
            this.btnLoadSpectrum.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadSpectrum.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadSpectrum.Image")));
            this.btnLoadSpectrum.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadSpectrum.Name = "btnLoadSpectrum";
            this.btnLoadSpectrum.Size = new System.Drawing.Size(29, 24);
            this.btnLoadSpectrum.Text = "载入谱图";
            this.btnLoadSpectrum.Click += new System.EventHandler(this.btnLoadSpectrum_Click);
            // 
            // btnZoom
            // 
            this.btnZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoom.Image = ((System.Drawing.Image)(resources.GetObject("btnZoom.Image")));
            this.btnZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Size = new System.Drawing.Size(29, 24);
            this.btnZoom.Text = "放大";
            this.btnZoom.Click += new System.EventHandler(this.btnZoom_Click);
            // 
            // btnNarrow
            // 
            this.btnNarrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNarrow.Image = ((System.Drawing.Image)(resources.GetObject("btnNarrow.Image")));
            this.btnNarrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNarrow.Name = "btnNarrow";
            this.btnNarrow.Size = new System.Drawing.Size(29, 24);
            this.btnNarrow.Text = "缩小";
            this.btnNarrow.Click += new System.EventHandler(this.btnNarrow_Click);
            // 
            // btnZoomRst
            // 
            this.btnZoomRst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomRst.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomRst.Image")));
            this.btnZoomRst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomRst.Name = "btnZoomRst";
            this.btnZoomRst.Size = new System.Drawing.Size(29, 24);
            this.btnZoomRst.Text = "1:1显示";
            this.btnZoomRst.Click += new System.EventHandler(this.btnZoomRst_Click);
            // 
            // btnYZoom
            // 
            this.btnYZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnYZoom.Image = ((System.Drawing.Image)(resources.GetObject("btnYZoom.Image")));
            this.btnYZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnYZoom.Name = "btnYZoom";
            this.btnYZoom.Size = new System.Drawing.Size(29, 24);
            this.btnYZoom.Text = "Y轴放大";
            this.btnYZoom.Click += new System.EventHandler(this.btnYZoom_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnPeakLine
            // 
            this.btnPeakLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPeakLine.Image = ((System.Drawing.Image)(resources.GetObject("btnPeakLine.Image")));
            this.btnPeakLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeakLine.Name = "btnPeakLine";
            this.btnPeakLine.Size = new System.Drawing.Size(29, 24);
            this.btnPeakLine.Text = "标线";
            this.btnPeakLine.Click += new System.EventHandler(this.btnPeakLine_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton4.Text = "清空图表";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // tbPeakPosition
            // 
            this.tbPeakPosition.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tbPeakPosition.Name = "tbPeakPosition";
            this.tbPeakPosition.Size = new System.Drawing.Size(79, 27);
            this.tbPeakPosition.ToolTipText = "x轴坐标";
            // 
            // btnSaveAsExcel
            // 
            this.btnSaveAsExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAsExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAsExcel.Image")));
            this.btnSaveAsExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAsExcel.Name = "btnSaveAsExcel";
            this.btnSaveAsExcel.Size = new System.Drawing.Size(29, 24);
            this.btnSaveAsExcel.Text = "另存为excel";
            this.btnSaveAsExcel.Click += new System.EventHandler(this.btnSaveAsExcel_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton1.Text = "分析软件";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton3.Text = "定性分析";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(69, 24);
            this.toolStripLabel1.Text = "输入元素";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 27);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton2.Text = "查找";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.滤噪ToolStripMenuItem,
            this.基线校正ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(34, 24);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // 滤噪ToolStripMenuItem
            // 
            this.滤噪ToolStripMenuItem.Name = "滤噪ToolStripMenuItem";
            this.滤噪ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.滤噪ToolStripMenuItem.Text = "滤噪";
            this.滤噪ToolStripMenuItem.Click += new System.EventHandler(this.滤噪ToolStripMenuItem_Click);
            // 
            // 基线校正ToolStripMenuItem
            // 
            this.基线校正ToolStripMenuItem.Name = "基线校正ToolStripMenuItem";
            this.基线校正ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.基线校正ToolStripMenuItem.Text = "基线校正";
            this.基线校正ToolStripMenuItem.Click += new System.EventHandler(this.基线校正ToolStripMenuItem_Click);
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.Transparent;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(413, 0);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(846, 10);
            this.splitter2.TabIndex = 19;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Transparent;
            this.splitter1.Enabled = false;
            this.splitter1.Location = new System.Drawing.Point(400, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(13, 586);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // splitter4
            // 
            this.splitter4.BackColor = System.Drawing.Color.Transparent;
            this.splitter4.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(400, 586);
            this.splitter4.Margin = new System.Windows.Forms.Padding(4);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(859, 20);
            this.splitter4.TabIndex = 11;
            this.splitter4.TabStop = false;
            // 
            // panelSetting
            // 
            this.panelSetting.AssociatedSplitter = null;
            this.panelSetting.BackColor = System.Drawing.Color.Transparent;
            this.panelSetting.CaptionFont = new System.Drawing.Font("微软雅黑", 11.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelSetting.CaptionHeight = 20;
            this.panelSetting.Controls.Add(this.btnAnalysis);
            this.panelSetting.Controls.Add(this.tbnExit);
            this.panelSetting.Controls.Add(this.panel2);
            this.panelSetting.Controls.Add(this.btn_Stop);
            this.panelSetting.Controls.Add(this.btn_Start);
            this.panelSetting.Controls.Add(this.tlpLibsMode);
            this.panelSetting.Controls.Add(this.tableLayoutPanel1);
            this.panelSetting.Controls.Add(this.tlpSpectrometer);
            this.panelSetting.CustomColors.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(65)))), ((int)(((byte)(118)))));
            this.panelSetting.CustomColors.CaptionCloseIcon = System.Drawing.SystemColors.ControlText;
            this.panelSetting.CustomColors.CaptionExpandIcon = System.Drawing.SystemColors.ControlText;
            this.panelSetting.CustomColors.CaptionGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.panelSetting.CustomColors.CaptionGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(164)))), ((int)(((byte)(224)))));
            this.panelSetting.CustomColors.CaptionGradientMiddle = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.panelSetting.CustomColors.CaptionSelectedGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(222)))));
            this.panelSetting.CustomColors.CaptionSelectedGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(203)))), ((int)(((byte)(136)))));
            this.panelSetting.CustomColors.CaptionText = System.Drawing.SystemColors.ControlText;
            this.panelSetting.CustomColors.CollapsedCaptionText = System.Drawing.SystemColors.ControlText;
            this.panelSetting.CustomColors.ContentGradientBegin = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(190)))), ((int)(((byte)(245)))));
            this.panelSetting.CustomColors.ContentGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.panelSetting.CustomColors.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelSetting.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSetting.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelSetting.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelSetting.Image = null;
            this.panelSetting.Location = new System.Drawing.Point(0, 0);
            this.panelSetting.Margin = new System.Windows.Forms.Padding(4);
            this.panelSetting.MinimumSize = new System.Drawing.Size(20, 20);
            this.panelSetting.Name = "panelSetting";
            this.panelSetting.ShowExpandIcon = true;
            this.panelSetting.ShowTransparentBackground = false;
            this.panelSetting.Size = new System.Drawing.Size(400, 700);
            this.panelSetting.TabIndex = 1;
            this.panelSetting.Text = "设置面板";
            this.panelSetting.ToolTipTextCloseIcon = null;
            this.panelSetting.ToolTipTextExpandIconPanelCollapsed = null;
            this.panelSetting.ToolTipTextExpandIconPanelExpanded = null;
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnAnalysis.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAnalysis.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalysis.Image")));
            this.btnAnalysis.Location = new System.Drawing.Point(289, 216);
            this.btnAnalysis.Margin = new System.Windows.Forms.Padding(4);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(91, 61);
            this.btnAnalysis.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnAnalysis, "数据分析");
            this.btnAnalysis.UseVisualStyleBackColor = false;
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // tbnExit
            // 
            this.tbnExit.BackColor = System.Drawing.Color.MediumTurquoise;
            this.tbnExit.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbnExit.ForeColor = System.Drawing.Color.Maroon;
            this.tbnExit.Location = new System.Drawing.Point(289, 302);
            this.tbnExit.Margin = new System.Windows.Forms.Padding(4);
            this.tbnExit.Name = "tbnExit";
            this.tbnExit.Size = new System.Drawing.Size(91, 62);
            this.tbnExit.TabIndex = 6;
            this.tbnExit.Text = "EXIT";
            this.tbnExit.UseVisualStyleBackColor = false;
            this.tbnExit.Click += new System.EventHandler(this.tbnExit_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnPauseCam);
            this.panel2.Controls.Add(this.btnStartCam);
            this.panel2.Controls.Add(this.pbxImage);
            this.panel2.Location = new System.Drawing.Point(0, 381);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(553, 434);
            this.panel2.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(261, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "放大";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPauseCam
            // 
            this.btnPauseCam.Location = new System.Drawing.Point(133, 4);
            this.btnPauseCam.Margin = new System.Windows.Forms.Padding(4);
            this.btnPauseCam.Name = "btnPauseCam";
            this.btnPauseCam.Size = new System.Drawing.Size(113, 29);
            this.btnPauseCam.TabIndex = 2;
            this.btnPauseCam.Text = "停止";
            this.btnPauseCam.UseVisualStyleBackColor = true;
            this.btnPauseCam.Click += new System.EventHandler(this.btnPauseCam_Click);
            // 
            // btnStartCam
            // 
            this.btnStartCam.Location = new System.Drawing.Point(-4, 4);
            this.btnStartCam.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartCam.Name = "btnStartCam";
            this.btnStartCam.Size = new System.Drawing.Size(119, 29);
            this.btnStartCam.TabIndex = 1;
            this.btnStartCam.Text = "开始";
            this.btnStartCam.UseVisualStyleBackColor = true;
            this.btnStartCam.Click += new System.EventHandler(this.btnStartCam_Click);
            // 
            // pbxImage
            // 
            this.pbxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxImage.BackColor = System.Drawing.Color.Silver;
            this.pbxImage.Location = new System.Drawing.Point(7, 46);
            this.pbxImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbxImage.Name = "pbxImage";
            this.pbxImage.Size = new System.Drawing.Size(382, 260);
            this.pbxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxImage.TabIndex = 0;
            this.pbxImage.TabStop = false;
            this.pbxImage.SizeChanged += new System.EventHandler(this.pbxImage_SizeChanged);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Image = ((System.Drawing.Image)(resources.GetObject("btn_Stop.Image")));
            this.btn_Stop.Location = new System.Drawing.Point(289, 136);
            this.btn_Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(91, 62);
            this.btn_Stop.TabIndex = 4;
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Start
            // 
            this.btn_Start.Image = ((System.Drawing.Image)(resources.GetObject("btn_Start.Image")));
            this.btn_Start.Location = new System.Drawing.Point(289, 49);
            this.btn_Start.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(91, 62);
            this.btn_Start.TabIndex = 3;
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // tlpLibsMode
            // 
            this.tlpLibsMode.ColumnCount = 2;
            this.tlpLibsMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLibsMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLibsMode.Controls.Add(this.panel1, 1, 2);
            this.tlpLibsMode.Controls.Add(this.保存名称及路径, 0, 2);
            this.tlpLibsMode.Controls.Add(this.ckbAutoSaveSpectrum, 1, 1);
            this.tlpLibsMode.Controls.Add(this.ckbLibsModel, 1, 0);
            this.tlpLibsMode.Controls.Add(this.label1, 0, 1);
            this.tlpLibsMode.Controls.Add(this.LIBS模式, 0, 0);
            this.tlpLibsMode.Location = new System.Drawing.Point(3, 171);
            this.tlpLibsMode.Margin = new System.Windows.Forms.Padding(4);
            this.tlpLibsMode.Name = "tlpLibsMode";
            this.tlpLibsMode.RowCount = 3;
            this.tlpLibsMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpLibsMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpLibsMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpLibsMode.Size = new System.Drawing.Size(253, 125);
            this.tlpLibsMode.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveSpec);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(130, 86);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(119, 35);
            this.panel1.TabIndex = 21;
            // 
            // btnSaveSpec
            // 
            this.btnSaveSpec.BackColor = System.Drawing.Color.DarkGray;
            this.btnSaveSpec.Enabled = false;
            this.btnSaveSpec.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveSpec.Image")));
            this.btnSaveSpec.Location = new System.Drawing.Point(37, -1);
            this.btnSaveSpec.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveSpec.Name = "btnSaveSpec";
            this.btnSaveSpec.Size = new System.Drawing.Size(39, 32);
            this.btnSaveSpec.TabIndex = 20;
            this.btnSaveSpec.UseVisualStyleBackColor = false;
            this.btnSaveSpec.Click += new System.EventHandler(this.btnSaveSpectrum_Click);
            // 
            // 保存名称及路径
            // 
            this.保存名称及路径.AutoSize = true;
            this.保存名称及路径.Dock = System.Windows.Forms.DockStyle.Fill;
            this.保存名称及路径.Location = new System.Drawing.Point(4, 82);
            this.保存名称及路径.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.保存名称及路径.Name = "保存名称及路径";
            this.保存名称及路径.Size = new System.Drawing.Size(118, 43);
            this.保存名称及路径.TabIndex = 19;
            this.保存名称及路径.Text = "保存名称及路径";
            this.保存名称及路径.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ckbAutoSaveSpectrum
            // 
            this.ckbAutoSaveSpectrum.AutoSize = true;
            this.ckbAutoSaveSpectrum.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbAutoSaveSpectrum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckbAutoSaveSpectrum.Enabled = false;
            this.ckbAutoSaveSpectrum.Location = new System.Drawing.Point(130, 45);
            this.ckbAutoSaveSpectrum.Margin = new System.Windows.Forms.Padding(4);
            this.ckbAutoSaveSpectrum.Name = "ckbAutoSaveSpectrum";
            this.ckbAutoSaveSpectrum.Size = new System.Drawing.Size(119, 33);
            this.ckbAutoSaveSpectrum.TabIndex = 18;
            this.ckbAutoSaveSpectrum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbAutoSaveSpectrum.UseVisualStyleBackColor = true;
            this.ckbAutoSaveSpectrum.CheckedChanged += new System.EventHandler(this.ckbAutoSaveSpectrum_CheckedChanged);
            // 
            // ckbLibsModel
            // 
            this.ckbLibsModel.AutoSize = true;
            this.ckbLibsModel.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbLibsModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckbLibsModel.Location = new System.Drawing.Point(130, 4);
            this.ckbLibsModel.Margin = new System.Windows.Forms.Padding(4);
            this.ckbLibsModel.Name = "ckbLibsModel";
            this.ckbLibsModel.Size = new System.Drawing.Size(119, 33);
            this.ckbLibsModel.TabIndex = 23;
            this.ckbLibsModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbLibsModel.UseVisualStyleBackColor = true;
            this.ckbLibsModel.CheckedChanged += new System.EventHandler(this.ckbLibsModel_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 41);
            this.label1.TabIndex = 17;
            this.label1.Text = "自动保存光谱";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LIBS模式
            // 
            this.LIBS模式.AutoSize = true;
            this.LIBS模式.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LIBS模式.Location = new System.Drawing.Point(4, 0);
            this.LIBS模式.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LIBS模式.Name = "LIBS模式";
            this.LIBS模式.Size = new System.Drawing.Size(118, 41);
            this.LIBS模式.TabIndex = 22;
            this.LIBS模式.Text = "LIBS模式";
            this.LIBS模式.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.54974F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.45026F));
            this.tableLayoutPanel1.Controls.Add(this.lbRange, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 302);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(255, 71);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lbRange
            // 
            this.lbRange.AutoSize = true;
            this.lbRange.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbRange.Location = new System.Drawing.Point(120, 56);
            this.lbRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRange.Name = "lbRange";
            this.lbRange.Size = new System.Drawing.Size(131, 15);
            this.lbRange.TabIndex = 2;
            this.lbRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 71);
            this.label2.TabIndex = 0;
            this.label2.Text = "光谱范围";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpSpectrometer
            // 
            this.tlpSpectrometer.ColumnCount = 2;
            this.tlpSpectrometer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSpectrometer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSpectrometer.Controls.Add(this.numDelayTime, 1, 3);
            this.tlpSpectrometer.Controls.Add(this.lblIntegrationTime, 0, 0);
            this.tlpSpectrometer.Controls.Add(this.label3, 0, 3);
            this.tlpSpectrometer.Controls.Add(this.nudScansToAverage, 1, 1);
            this.tlpSpectrometer.Controls.Add(this.lblScansToAverage, 0, 1);
            this.tlpSpectrometer.Controls.Add(this.lblExternalTriggerMode, 0, 2);
            this.tlpSpectrometer.Controls.Add(this.cbxExternalTriggerMode, 1, 2);
            this.tlpSpectrometer.Controls.Add(this.nudIntegrationTime, 1, 0);
            this.tlpSpectrometer.Location = new System.Drawing.Point(3, 25);
            this.tlpSpectrometer.Margin = new System.Windows.Forms.Padding(4);
            this.tlpSpectrometer.Name = "tlpSpectrometer";
            this.tlpSpectrometer.RowCount = 4;
            this.tlpSpectrometer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSpectrometer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSpectrometer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSpectrometer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSpectrometer.Size = new System.Drawing.Size(253, 150);
            this.tlpSpectrometer.TabIndex = 0;
            // 
            // numDelayTime
            // 
            this.numDelayTime.AutoSize = true;
            this.numDelayTime.DecimalPlaces = 2;
            this.numDelayTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numDelayTime.Enabled = false;
            this.numDelayTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numDelayTime.Location = new System.Drawing.Point(130, 118);
            this.numDelayTime.Margin = new System.Windows.Forms.Padding(4);
            this.numDelayTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numDelayTime.Name = "numDelayTime";
            this.numDelayTime.Size = new System.Drawing.Size(119, 25);
            this.numDelayTime.TabIndex = 17;
            this.numDelayTime.ValueChanged += new System.EventHandler(this.numDelayTime_ValueChanged);
            // 
            // lblIntegrationTime
            // 
            this.lblIntegrationTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIntegrationTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIntegrationTime.Location = new System.Drawing.Point(1, 1);
            this.lblIntegrationTime.Margin = new System.Windows.Forms.Padding(1);
            this.lblIntegrationTime.Name = "lblIntegrationTime";
            this.lblIntegrationTime.Size = new System.Drawing.Size(124, 36);
            this.lblIntegrationTime.TabIndex = 3;
            this.lblIntegrationTime.Text = "积分时间(ms)";
            this.lblIntegrationTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(4, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 31);
            this.label3.TabIndex = 5;
            this.label3.Text = "延时时间(us)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudScansToAverage
            // 
            this.nudScansToAverage.AutoSize = true;
            this.nudScansToAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudScansToAverage.Location = new System.Drawing.Point(130, 42);
            this.nudScansToAverage.Margin = new System.Windows.Forms.Padding(4);
            this.nudScansToAverage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScansToAverage.Name = "nudScansToAverage";
            this.nudScansToAverage.Size = new System.Drawing.Size(119, 25);
            this.nudScansToAverage.TabIndex = 15;
            this.nudScansToAverage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblScansToAverage
            // 
            this.lblScansToAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScansToAverage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblScansToAverage.Location = new System.Drawing.Point(4, 42);
            this.lblScansToAverage.Margin = new System.Windows.Forms.Padding(4);
            this.lblScansToAverage.Name = "lblScansToAverage";
            this.lblScansToAverage.Size = new System.Drawing.Size(118, 31);
            this.lblScansToAverage.TabIndex = 4;
            this.lblScansToAverage.Text = "平均次数";
            this.lblScansToAverage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExternalTriggerMode
            // 
            this.lblExternalTriggerMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExternalTriggerMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblExternalTriggerMode.Location = new System.Drawing.Point(4, 81);
            this.lblExternalTriggerMode.Margin = new System.Windows.Forms.Padding(4);
            this.lblExternalTriggerMode.Name = "lblExternalTriggerMode";
            this.lblExternalTriggerMode.Size = new System.Drawing.Size(118, 29);
            this.lblExternalTriggerMode.TabIndex = 6;
            this.lblExternalTriggerMode.Text = "触发模式";
            this.lblExternalTriggerMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbxExternalTriggerMode
            // 
            this.cbxExternalTriggerMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxExternalTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxExternalTriggerMode.FormattingEnabled = true;
            this.cbxExternalTriggerMode.Items.AddRange(new object[] {
            "外触发"});
            this.cbxExternalTriggerMode.Location = new System.Drawing.Point(130, 81);
            this.cbxExternalTriggerMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxExternalTriggerMode.Name = "cbxExternalTriggerMode";
            this.cbxExternalTriggerMode.Size = new System.Drawing.Size(119, 23);
            this.cbxExternalTriggerMode.TabIndex = 12;
            this.cbxExternalTriggerMode.SelectedIndexChanged += new System.EventHandler(this.cbxExternalTriggerMode_SelectedIndexChanged);
            // 
            // nudIntegrationTime
            // 
            this.nudIntegrationTime.AutoSize = true;
            this.nudIntegrationTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudIntegrationTime.Location = new System.Drawing.Point(130, 4);
            this.nudIntegrationTime.Margin = new System.Windows.Forms.Padding(4);
            this.nudIntegrationTime.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudIntegrationTime.Minimum = new decimal(new int[] {
            105,
            0,
            0,
            131072});
            this.nudIntegrationTime.Name = "nudIntegrationTime";
            this.nudIntegrationTime.Size = new System.Drawing.Size(119, 25);
            this.nudIntegrationTime.TabIndex = 14;
            this.nudIntegrationTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudIntegrationTime.ValueChanged += new System.EventHandler(this.nudIntegrationTime_ValueChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 700);
            this.Controls.Add(this.panelSpectrum);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.splitter4);
            this.Controls.Add(this.lbxMsg);
            this.Controls.Add(this.panelSetting);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "LIBS2.4";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.panelSpectrum.ResumeLayout(false);
            this.panelSpectrum.PerformLayout();
            this.toolStripPanel.ResumeLayout(false);
            this.toolStripPanel.PerformLayout();
            this.panelSetting.ResumeLayout(false);
            this.panelSetting.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxImage)).EndInit();
            this.tlpLibsMode.ResumeLayout(false);
            this.tlpLibsMode.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tlpSpectrometer.ResumeLayout(false);
            this.tlpSpectrometer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScansToAverage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntegrationTime)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblIntegrationTime;
        private System.Windows.Forms.Label lblScansToAverage;
        private System.Windows.Forms.Label lblExternalTriggerMode;
		private System.Windows.Forms.ComboBox cbxExternalTriggerMode;
		private System.Windows.Forms.NumericUpDown nudIntegrationTime;
        private System.Windows.Forms.NumericUpDown nudScansToAverage;
        private System.Windows.Forms.ToolStrip toolStripPanel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnLoadSpectrum;
		private System.Windows.Forms.ToolStripButton btnZoom;
		private System.Windows.Forms.ToolStripButton btnNarrow;
		private System.Windows.Forms.ToolStripButton btnZoomRst;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnPeakLine;
		private System.Windows.Forms.ToolStripTextBox tbPeakPosition;
		private System.Windows.Forms.ToolStripButton btnSaveAsExcel;
		private OxyPlot.WindowsForms.Plot plotSpectrum;
		private System.Windows.Forms.ListBox lbxMsg;
		private BSE.Windows.Forms.Splitter splitter4;
        private BSE.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStrip miniToolStrip;
        private BSE.Windows.Forms.Panel panelSetting;
		private BSE.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.ToolTip tpStop;
        private System.Windows.Forms.ToolTip tpStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numDelayTime;
        private BSE.Windows.Forms.Panel panelSpectrum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbAutoSaveSpectrum;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TableLayoutPanel tlpSpectrometer;
        private System.Windows.Forms.ToolStripButton btnSaveSpectrum;
        private System.Windows.Forms.Label 保存名称及路径;
        private System.Windows.Forms.Button btnSaveSpec;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tlpLibsMode;
        private System.Windows.Forms.CheckBox ckbLibsModel;
        private System.Windows.Forms.Label LIBS模式;
        private System.Windows.Forms.ToolStripButton btnYZoom;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPauseCam;
        private System.Windows.Forms.Button btnStartCam;
        private System.Windows.Forms.PictureBox pbxImage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button tbnExit;
        private System.Windows.Forms.Button btnAnalysis;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 滤噪ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 基线校正ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
    }
}

