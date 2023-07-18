using System;
using System.Drawing;
using System.Windows.Forms;
using LIBS.Windows.Properties;
using System.Globalization;
using System.IO;
using LIBS.Spectrometers;
using System.Linq;
using OxyPlot;
using System.Text;

using System.IO.Ports;

using System.Collections.Generic;
using System.Diagnostics;
using LIBS.Delay;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Newimage;

using MVSDK;//使用MindVision .net SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.Runtime.InteropServices;
using LIBS.Windows;
using ThridLibray;



namespace LIBS
{
    public partial class MainForm : Form
    {
        #region 字段及属性
        #region Oxy
        LineSeries[] _lines = null;

        LineSeries[] _lines_load = new LineSeries[10];
        PlotModel model = new PlotModel///初始化光谱
        {
            //LegendPlacement = LegendPlacement.Outside,
            PlotMargins = new OxyThickness(2),
            Axes = {
                    new LinearAxis(AxisPosition.Left)
                    {

                        MaximumPadding = 0.05,/* 小数表示百分比 */
						MinimumPadding = 0.05,
                        Minimum = -500,
                        Maximum = 65000,
						//AbsoluteMinimum = 0,
						MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        MajorGridlineColor = OxyColor.FromAColor(40, OxyColors.DarkBlue),
                        MinorGridlineColor = OxyColor.FromAColor(20, OxyColors.DarkBlue)
                    },
                    new LinearAxis(AxisPosition.Bottom, "波长")
                    {
                        Unit = "nm",
                        Maximum=920,
                        Minimum=200,
                        MaximumPadding = 0.05,
                        MinimumPadding = 0.05,
						//AbsoluteMinimum = 0,
						MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        MajorGridlineColor = OxyColor.FromAColor(40, OxyColors.DarkBlue),
                        MinorGridlineColor = OxyColor.FromAColor(20, OxyColors.DarkBlue)
                    }
                },
            Culture = CultureInfo.InvariantCulture
        };
        #endregion

        #region 光谱仪数据获取事件
        public delegate void SpectrumDataHandler(double[] x, double[] y, int index);
        public event SpectrumDataHandler DataEvent;
        public void OnDataEvent(double[] x, double[] y, int index)
        {
            if (this.DataEvent != null)
            {
                this.DataEvent(x, y, index);
            }
        }
        #endregion
        /// <summary>
        /// listview控件属性
        /// </summary>
        List<double>[] wave = new List<double>[10];
        List<double>[] count = new List<double>[10];
        int[] markPosition;//标识峰的位置
        string[] eleTypeList;//元素的谱线类型
        string[] eleNameList;//元素的元素名
        double[] eleIntensity;//峰线的峰值
        #region 相机变量
        protected CameraHandle m_hCamera = 0;             // 句柄
        protected IntPtr m_ImageBuffer;             // 预览通道RGB图像缓存
        protected IntPtr m_ImageBufferSnapshot;     // 抓拍通道RGB图像缓存
        protected tSdkCameraCapbility tCameraCapability;  // 相机特性描述
        protected int m_iDisplayedFrames = 0;    //已经显示的总帧数
        protected IntPtr m_iCaptureCallbackCtx;     //图像回调函数的上下文参数
        protected Thread m_tCaptureThread;          //图像抓取线程
        protected bool m_bExitCaptureThread = false;//采用线程采集时，让线程退出的标志
        protected IntPtr m_iSettingPageMsgCallbackCtx; //相机配置界面消息回调函数的上下文参数   
        protected tSdkFrameHead m_tFrameHead;
        //protected bool          m_bEraseBk = false;
        #endregion

        #region 相机变量2
        List<IGrabbedRawData> m_frameList = new List<IGrabbedRawData>();        // 图像缓存列表 | frame data list 
        Thread renderThread = null;         // 显示线程 | image display thread 
        bool m_bShowLoop = true;            // 线程控制变量 | thread looping flag 
        Mutex m_mutex = new Mutex();        // 锁，保证多线程安全 | mutex 
        private Graphics _g = null;
        bool m_bShowByGDI;
        Stopwatch m_stopWatch = new Stopwatch();
        #endregion


        List<double[]>[] dataRom = null;//LIBS模式下存储各通道每个脉冲所产生的的波长强度数据，以便后续求平均值

        int dataReadyCount;//存储时，是否各通道数都全部获取
        OpenFileDialog ofd = new OpenFileDialog();
        int LIBScount = 0;  //LIBS模式下，平均次数
        int xuanzewj = 1;
        System.Windows.Forms.Timer forShow = new System.Windows.Forms.Timer();//这个timer和下面的MainForm_Shown时间 都是为了在窗口完全显示后 连接设备；可考虑splash
        SaveFileDialog sfdSpectrum = new SaveFileDialog();//用于保存图像或谱图
        OpenFileDialog ofdSpectrum = new OpenFileDialog();
        string SpectrumSavePath = "C:\\";
        string SpectrumSaveName = "";
        int OpenCount = 0;//用于记录载入谱图的数量
        int SaveCount = 0;//用于记录同一路径下存储谱图的数量
        string ypcs = "名称（化学式）:" + Environment.NewLine + "编号:" + Environment.NewLine + "成分:" + Environment.NewLine + "采集地点:" + Environment.NewLine + "样品图像:" + Environment.NewLine + "属性（质量、密度和形态）:" + Environment.NewLine + "备注:";
        string sycs = "";
        bool stopFlag = false;


        bool PeakLineExist = false;
        LineAnnotation PeakLine = new LineAnnotation { Type = LineAnnotationType.Vertical };

        Avantes avantes = new Avantes();
        DelayControl delayController = new DelayControl();

        #endregion

        #region 用户界面
        public MainForm()
        {
            InitializeComponent();
            plotSpectrum.Model = MouseEvents();//可选区域放大
            tlpSpectrometer.Enabled = false;//光谱仪设置区域隐藏

            DataEvent += new SpectrumDataHandler(AvantesDrawSpectum);//画光谱
            lbxMsg.DrawItem += new DrawItemEventHandler(lbxMsg_DrawItem);

            for (int i = 0; i < wave.Length; i++)
            {
                wave[i] = new List<double>();  // 初始化每个元素为一个新的List<double>
            }
            for (int i = 0; i < count.Length; i++)
            {
                count[i] = new List<double>();  // 初始化每个元素为一个新的List<double>
            }
            forShow.Interval = 1000;
            forShow.Tick += new EventHandler(forShow_Tick);
            //ConnectSpectrometers();

            //ConnectDelay();           
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            btnPauseCam.Enabled = false;
            if (null == renderThread)
            {
                renderThread = new Thread(new ThreadStart(ShowThread));
                renderThread.Start();
            }
            m_stopWatch.Start();
            forShow.Start();

        }

        void forShow_Tick(object sender, EventArgs e)
        {
            forShow.Stop();
            ConnectSpectrometers();
            ConnectDelay();
            //if (avantes.m_DevNr == 0)
            //{
            //    forShow.Start();
            //}
            //ConnectCamera();

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (m_hCamera > 0)
            {
#if !USE_CALL_BACK //使用回调函数的方式则不需要停止线程
                m_bExitCaptureThread = true;
                while (m_tCaptureThread.IsAlive)
                {
                    Thread.Sleep(10);
                }
#endif
                MvApi.CameraUnInit(m_hCamera);
                Marshal.FreeHGlobal(m_ImageBuffer);
                Marshal.FreeHGlobal(m_ImageBufferSnapshot);
                m_hCamera = 0;
            }


            if (btnStart.Enabled == false)
            {

                avantes.StopMeasurement();

            }

            avantes.DisConnect();
            delayController.Close();



            System.Environment.Exit(System.Environment.ExitCode);

            this.Close();

        }

        private void StartAcqUI()
        {
            stopFlag = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            plotSpectrum.Focus();
            tlpSpectrometer.Enabled = false;
            ListItemColor lic = new ListItemColor("开始采集");
            lbxMsg.Items.Insert(0, lic);
        }

        /// <summary>
        /// 停止采集, 可用于初始化状态, 当仪器就绪后isReady=true; 未就绪则为false;
        /// </summary>
        /// <param name="isReady">仪器是否就绪</param>
        private void StopAcqUI(bool isReady = true)
        {
            this.UIThread(delegate
            {

                //xPoints = new double[avantes.m_DevNr * 2048];
                //yPoints = new double[avantes.m_DevNr * 2048];
                stopFlag = true;
                //dataIsReady = new bool[avantes.m_DevNr];
                avantes.listDevice.ForEach(d => d.ready2Read = false);
                LIBScount = 0;

                //dataRom = new List<double[]>[avantes.m_DevNr];
                //for (int i = 0; i < avantes.m_DevNr; i++)
                //{
                //    dataRom[i] = new List<double[]>();
                //}

                btnStart.Enabled = isReady;

                btnStop.Enabled = false;

                if (!isReady)
                {
                    tlpSpectrometer.Enabled = false;
                }

                // TODO: 调试
                ListItemColor lic = new ListItemColor("停止采集");
                lbxMsg.Items.Insert(0, lic);

                plotSpectrum.Focus();
                tlpSpectrometer.Enabled = true;
            });
        }

        private void DrawSpectumUI(double[] x, double[] y, int index)
        {
            if (x == null || y == null || x.Length != y.Length)
            {
                lbxMsg.Items.Insert(0, new ListItemColor("光谱数据不正确", Color.Red));
            }

            _lines[index].Title = (index + 1).ToString();

            _lines[index].StrokeThickness = 0.01;
            //_lines[index].Tag = 0;

            _lines[index].Points.Clear();

            int len = x.Length;

            for (int i = 0; i < AVS5216.MAX_NR_PIXELS_PER_CHANNEL; ++i)
            {
                _lines[index].Points.Add(new DataPoint(x[i], y[i]));
            }

            plotSpectrum.RefreshPlot(true);
            //plotSpectrum.InvalidatePlot(false);
            //model.RefreshPlot(true);
            System.Windows.Forms.Application.DoEvents();

        }

        private void DrawLoadSpectumUI(double[] x, double[] y, int index)
        {
            if (x == null || y == null || x.Length != y.Length)
            {
                lbxMsg.Items.Insert(0, new ListItemColor("光谱数据不正确", Color.Red));
            }

            _lines_load[index] = new LineSeries(index.ToString());


            model.Axes[1].Maximum = 800;
            model.Axes[1].Minimum = 190;
            _lines_load[index].StrokeThickness = 0.01;
            model.Series.Add(_lines_load[index]);
            //_lines[index].Points.Clear();

            int len = x.Length;


            for (int i = 0; i < len; ++i)
            {
                _lines_load[index].Points.Add(new DataPoint(x[i], y[i]));
            }

            plotSpectrum.RefreshPlot(true);
            //plotSpectrum.InvalidatePlot(false);
            System.Windows.Forms.Application.DoEvents();
        }

        #region 光谱仪

        private void InitializeSpectrometers()
        {
            avantes.RunErrorEvent += new Avantes.AvantesRunErrorHandler(avantes_RunErrorEvent);
            avantes.OpenErrorEvent += new Avantes.AvantesOpenErrorHandler(avantes_OpenErrorEvent);
            avantes.StatusEvent += new Avantes.AvantesStatusHandler(avantes_StatusEvent);
            //avantes.AvantesFinished += new Avantes.AvantesFinishedHandler(avantes_AvantesFinished);
        }

        private void ConnectSpectrometers()
        {
            InitializeSpectrometers();

            int count = avantes.Open(this.Handle);

            if (count > 0)
            {

                //此处需要初始化光谱仪几个参数的选项
                avantes.listDevice.ForEach(d => d.ready2Read = false);

                dataRom = new List<double[]>[count];//实际验证，泛型数组在建立时，需将每个泛型元素都各自初始化
                for (int i = 0; i < count; i++)
                {
                    dataRom[i] = new List<double[]>();
                }

                xPoints = new double[count * 2048];
                yPoints = new double[count * 2048];

                model.Axes[1].Minimum = avantes.m_Minlambda - 20;
                model.Axes[1].Maximum = avantes.m_Maxlambda + 20;

                model.Series.Clear();

                plotSpectrum.Model = model;

                _lines = new LineSeries[count];

                for (int i = 0; i < count; ++i)
                {
                    _lines[i] = new LineSeries();
                    plotSpectrum.Model.Series.Add(_lines[i]);
                }

                // cbxExternalTriggerMode.SelectedIndex = 0;
                //avantes.m_TriggerMode = cbxExternalTriggerMode.SelectedIndex;

                // TODO: 使能光谱仪控制区域
                tlpSpectrometer.Enabled = true;
            }

        }

        private void StopSpectrometers()
        {
            avantes.StopMeasurement();
            tlpSpectrometer.Enabled = true;
        }

        #endregion

        #region 延时器
        private void InitializeDelay()
        {
            delayController.DelayOpen += DelayOpened;
            delayController.DelayPara += DelayParaReady;
            delayController.DelayError += DelayErr;
        }

        private void ConnectDelay()
        {
            InitializeDelay();
            delayController.Open("COM5");
        }
        #endregion

        #region 相机
        //private void ConnectCamera()
        //{
        //    if (InitCamera() == true)
        //    {
        //        btnStartCam.Enabled = true;
        //        btnPauseCam.Enabled = false;
        //    }
        //    else
        //    {
        //        btnStartCam.Enabled = false;
        //        btnPauseCam.Enabled = false;
        //        lbxMsg.Items.Insert(0, new ListItemColor("照相机：连接失败", Color.Red));
        //       // MessageBox.Show("照相机连接失败");
        //    }
        //}

        #endregion

        #endregion

        #region 用户操作
        #region 采集命令
        private void btnStart_Click(object sender, EventArgs e)
        {
            sycs = "延时时间:" + numDelayTime.Value + Environment.NewLine + "积分时间:" + nudIntegrationTime.Value + Environment.NewLine + "平均次数:" + nudScansToAverage.Value + Environment.NewLine + "泵浦灯能量:" + Environment.NewLine + "脉冲频率:";
            StartAcqUI();
            if (ckbLibsModel.Checked)
            {
                LIBScount = (int)nudScansToAverage.Value - 1;
            }
            avantes.StartMeasurement();
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            avantes.StopMeasurement();
            StopAcqUI();
            dataRom = new List<double[]>[avantes.m_DevNr];
            for (int i = 0; i < avantes.m_DevNr; i++)
            {
                dataRom[i] = new List<double[]>();
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            btnStart_Click(this, e);
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            btnStop_Click(this, e);
        }

        private void btn_Analyse_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(this, e);
        }


        #endregion
        #region 光谱仪
        private void nudIntegrationTime_ValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.NumericUpDown nud = sender as System.Windows.Forms.NumericUpDown;
            decimal nVal = nud.Value;
            avantes.m_IntergrationTime = ((int)nVal).ToString();
        }

        //private void nudScansToAverage_ValueChanged(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.NumericUpDown nud = sender as System.Windows.Forms.NumericUpDown;
        //    decimal nVal = nud.Value;
        //    //hr2000.ScansToAverageChange((int)nVal);
        //    avantes.m_NrAverrages = (uint)nVal;
        //}

        private void cbxExternalTriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // System.Windows.Forms.ComboBox cbx = sender as System.Windows.Forms.ComboBox;
            // avantes.m_TriggerMode = cbx.SelectedIndex;//
        }

        private void ckbLibsModel_CheckedChanged(object sender, EventArgs e)
        {
            ckbAutoSaveSpectrum.Enabled = ckbLibsModel.Checked;
        }

        private void ckbAutoSaveSpectrum_CheckedChanged(object sender, EventArgs e)
        {
            btnSaveSpec.Enabled = ckbAutoSaveSpectrum.Checked;
            numDelayTime.Enabled = !ckbAutoSaveSpectrum.Checked;
            nudScansToAverage.Enabled = !ckbAutoSaveSpectrum.Checked; ;
            nudIntegrationTime.Enabled = !ckbAutoSaveSpectrum.Checked;
            cbxExternalTriggerMode.Enabled = !ckbAutoSaveSpectrum.Checked;
            if (ckbAutoSaveSpectrum.Checked)
            {
                btnSaveSpectrum_Click(this, e);
            }
        }




        #endregion
        #region 延时器
        private void numDelayTime_ValueChanged(object sender, EventArgs e)
        {
            Decimal delaytime = numDelayTime.Value;
            delaytime *= 10000;
            delayController.SetDelayTime('a', String.Format("{0:00000000000}", delaytime));
        }
        #endregion
        #region 保存谱图、谱图放大、谱图缩小
        private void btnZoom_Click(object sender, EventArgs e)
        {
            plotSpectrum.ZoomAllAxes(1.25);
        }

        private void btnNarrow_Click(object sender, EventArgs e)
        {
            plotSpectrum.ZoomAllAxes(0.8);
        }

        private void btnZoomRst_Click(object sender, EventArgs e)
        {

            model.Axes[0].Maximum = model.Axes[0].DataMaximum * 1.1;
            model.Axes[0].Minimum = model.Axes[0].DataMinimum - 0.1 * System.Math.Abs(model.Axes[0].DataMinimum);
            model.Axes[1].Maximum = model.Axes[1].DataMaximum + 20;
            model.Axes[1].Minimum = model.Axes[1].DataMinimum - 20;

            //model.Axes[0].Maximum = _lines_load[1].MaxY;
            //model.Axes[1].Minimum = _lines_load[1].MinX;
            //model.Axes[1].Maximum = _lines_load[1].MaxX;
            plotSpectrum.Reset(model.Axes[0]);
            plotSpectrum.Reset(model.Axes[1]);
            plotSpectrum.InvalidatePlot(false);

        }

        /// <summary>
        /// Y轴局部放大，即获取屏幕显示区域所有点的最大值和最小值（y坐标），根据这两个值缩放y轴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYZoom_Click(object sender, EventArgs e)
        {


            List<double> maxima = new List<double>();
            List<double> minima = new List<double>();
            if (model.Series.Count != 0)//判断是否有数据
            {
                foreach (LineSeries ls in model.Series.OfType<LineSeries>())
                {
                    double max = ls.Points.Where(p => p.X > model.Axes[1].ActualMinimum && p.X < model.Axes[1].ActualMaximum).Select(p => p.Y).Max();
                    double min = ls.Points.Where(p => p.X > model.Axes[1].ActualMinimum && p.X < model.Axes[1].ActualMaximum).Select(p => p.Y).Min();
                    maxima.Add(max);
                    minima.Add(min);
                }

                model.Axes[0].Maximum = maxima.Max() + 0.1 * System.Math.Abs(maxima.Max());
                model.Axes[0].Minimum = minima.Min() - 0.1 * System.Math.Abs(minima.Min());

                plotSpectrum.Reset(model.Axes[0]);
                plotSpectrum.InvalidatePlot(false);
            }
        }

        private void btnSaveSpectrum_Click(object sender, EventArgs e)
        {

            sfdSpectrum.Title = "选择光谱自动保存路径";

            //设置文件类型 
            sfdSpectrum.Filter = "数据文件（*.txt）|*.txt|所有文件（*.*）|*.*";

            //设置默认文件类型显示顺序 
            sfdSpectrum.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录 
            sfdSpectrum.RestoreDirectory = true;

            sfdSpectrum.DefaultExt = "txt";
            //点了保存按钮进入 
            if (sfdSpectrum.ShowDialog() == DialogResult.OK)
            {

                SaveCount = 0;
                string localFilePath = sfdSpectrum.FileName.ToString(); //获得文件路径,包含文件名称及扩展名 

                string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1); //获取文件名，不带路径

                string filePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));//获取文件路径，不带文件名 

                //newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt;//给文件名前加上时间 

                //saveFileDialog1.FileName.Insert(1,"dameng");//在文件名里加字符

                //System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();//输出文件
                /*不用保存参数*/
                //System.IO.FileStream fs;
                // fs = System.IO.File.Create(localFilePath);
                // AddText(fs, "积分时间(ms)：" + nudIntegrationTime.Value.ToString() + "\r\n");
                // AddText(fs, "平均次数：" + nudScansToAverage.Value.ToString() + "\r\n");
                // AddText(fs, "延时时间：" + numDelayTime.Value.ToString() + "\r\n");
                //fs.Close();
                SpectrumSavePath = filePath + "\\";
                SpectrumSaveName = fileNameExt.Replace(".txt", "");

            }
        }

        //private static void AddText(FileStream fs, string value)
        //{
        //    byte[] info = new UTF8Encoding(true).GetBytes(value);
        //    fs.Write(info, 0, info.Length);
        //}

        private void btnLoadSpectrum_Click(object sender, EventArgs ee)
        {
            //ofd.InitialDirectory = SpectrumSavePath;
            ofdSpectrum.Filter = "文本文档|*.txt|所有文件 (*.*)|*.*";
            ofdSpectrum.FilterIndex = 1;
            ofdSpectrum.RestoreDirectory = true;
            ofdSpectrum.Title = "载入光谱";

            string strLine;
            string[] strArray;
            char charArray = ';';


            double[] x = new double[61440];
            double[] y = new double[61440];
            int i = 0;
            //ofd.ValidateNames = true;
            //ofd.CheckFileExists = true;
            //ofd.CheckFileExists = true;
            if (ofdSpectrum.ShowDialog() == DialogResult.OK)
            {

                OpenCount++;
                try
                {
                    FileStream aFile = new FileStream(ofdSpectrum.FileName, FileMode.Open);
                    StreamReader sr = new StreamReader(aFile);
                    strLine = sr.ReadLine();

                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    strLine = sr.ReadLine();
                    //strLine = sr.ReadLine();
                    while (strLine != null)
                    {
                        strArray = strLine.Split(charArray);
                        x[i] = Double.Parse(strArray[0]);
                        //list1.Add(x);
                        y[i++] = Double.Parse(strArray[1]);
                        //list2.Add(y);
                        //MessageBox.Show(i.ToString());
                        strLine = sr.ReadLine();
                    }
                    sr.Close();
                    aFile.Close();
                }
                catch (IOException ex)
                {
                    lbxMsg.Items.Insert(0, new ListItemColor(ex.ToString(), Color.Red));
                }

                this.UIThread(() => DrawLoadSpectumUI(x.Take(i).ToArray(), y.Take(i).ToArray(), OpenCount));
                //wave.Clear();
                //count.Clear();

                foreach (double element in x.Take(i).ToArray())
                {
                    wave[avantes.m_DevNr + OpenCount - 1].Add(element);
                }
                foreach (double element in y.Take(i).ToArray())
                {
                    count[avantes.m_DevNr + OpenCount - 1].Add(element);
                }

                /*是否需要载入时按1：1显示*/
                //plotSpectrum.Reset(model.Axes[0]);
                //plotSpectrum.Reset(model.Axes[1]);
                //model.RefreshPlot(true);

                EnableRightClickToDeleteLine(OpenCount);
            }
        }

        private void btnPeakLine_Click(object sender, EventArgs ee)
        {
            if (PeakLineExist == false)
            {
                PeakLine.X = (model.Axes[1].ActualMaximum + model.Axes[1].ActualMinimum) / 2;

                PeakLine.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton != OxyMouseButton.Left)
                    {
                        return;
                    }

                    //PeakLine.StrokeThickness *= 5;
                    model.RefreshPlot(false);
                    e.Handled = true;
                };

                // Handle mouse movements (note: this is only called when the mousedown event was handled)
                PeakLine.MouseMove += (s, e) =>
                {

                    PeakLine.X = PeakLine.InverseTransform(e.Position).X;
                    model.RefreshPlot(false);
                    e.Handled = true;

                    tbPeakPosition.Text = "X = " + PeakLine.X.ToString("F3");
                };
                PeakLine.MouseUp += (s, e) =>
                {
                    //PeakLine.StrokeThickness /= 5;
                    model.RefreshPlot(false);
                    e.Handled = true;
                };
                model.Annotations.Add(PeakLine);
                model.RefreshPlot(false);
                PeakLineExist = true;
            }
            else
            {
                model.Annotations.Remove(PeakLine);
                model.RefreshPlot(false);
                PeakLineExist = false;
            }
        }

        private PlotModel MouseEvents()//谱图可选定区域放大
        {
            RectangleAnnotation range = null;
            double startx = double.NaN;
            double starty = double.NaN;
            model.MouseDown += (s, ee) =>
            {
                if (ee.ChangedButton == OxyMouseButton.Left)
                {
                    if (range == null)
                    {
                        range = new RectangleAnnotation { Fill = OxyColors.White.ChangeAlpha(255) };
                        model.Annotations.Add(range);
                        model.RefreshPlot(true);
                    }
                    range.Fill = OxyColors.White.ChangeAlpha(255);
                    startx = range.InverseTransform(ee.Position).X;
                    starty = range.InverseTransform(ee.Position).Y;

                    range.MaximumY = starty;
                    range.MinimumY = starty;
                    range.MinimumX = startx;
                    range.MaximumX = startx;
                    model.RefreshPlot(true);
                    ee.Handled = true;
                }
                ee.Handled = false;
            };
            model.MouseMove += (s, ee) =>
            {
                if (ee.ChangedButton == OxyMouseButton.Left && !double.IsNaN(startx))
                {
                    var x = range.InverseTransform(ee.Position).X;
                    var y = range.InverseTransform(ee.Position).Y;
                    range.MinimumX = Math.Min(x, startx);
                    range.MaximumX = Math.Max(x, startx);
                    range.MinimumY = Math.Min(y, starty);
                    range.MaximumY = Math.Max(y, starty);
                    //model.Subtitle = string.Format("X from {0:0.00} to {1:0.00};Y from {2:0.00} to {3:0.00}", range.MinimumX, range.MaximumX, range.MinimumY, range.MaximumY);
                    model.RefreshPlot(true);
                    ee.Handled = true;
                }
            };
            model.MouseUp += (s, ee) =>
            {
                if (s != plotSpectrum)
                {

                }
                else
                {
                    startx = double.NaN;
                    starty = double.NaN;
                    if (range != null && ee.ChangedButton == OxyMouseButton.Left && range.MinimumX != range.MaximumX && range.MaximumY != range.MinimumY)
                    {
                        range.Fill = null;

                        plotSpectrum.Zoom(model.Axes[0], range.MinimumY, range.MaximumY);
                        plotSpectrum.Zoom(model.Axes[1], range.MinimumX, range.MaximumX);
                        model.RefreshPlot(true);

                    }
                }
            };
            return model;
        }

        /**************右键取消添加的谱线*************/
        private void EnableRightClickToDeleteLine(int index)
        {

            _lines_load[index].MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Right)
                {
                    return;
                }
                //model.RefreshPlot(false);
                e.Handled = true;
            };

            //// Handle mouse movements (note: this is only called when the mousedown event was handled)
            //_lines[OpenCount].MouseMove += (s, e) =>
            //{

            //};
            _lines_load[index].MouseUp += (s, e) =>
            {
                model.Series.Remove(_lines_load[index]);
                //plotSpectrum.Model.Series.Remove(_lines[OpenCount]);
                OpenCount--;

                //PeakLine.StrokeThickness /= 5;
                model.RefreshPlot(false);
                e.Handled = true;
            };
        }

        /**************自动保存光谱数据****************/
        private void SaveDate()
        {
            StringBuilder dataToWrite = new StringBuilder();
            //Array.Sort(xPoints, yPoints);2.29
            //dataToWrite.Append(ypcs + Environment.NewLine);
            //dataToWrite.Append(sycs + Environment.NewLine);
            for (int i = 0; i < AVS5216.MAX_NR_PIXELS_PER_CHANNEL * avantes.m_DevNr; i++)
            {
                dataToWrite.Append(xPoints[i].ToString("f6") + ";" + yPoints[i].ToString() + Environment.NewLine);
            }
            //加f6让输出波长小数点由原来的12位保留小数点后6位有效数字

            //if (!File.Exists(path))
            //{
            // Create a file to write to.

            string fname = SpectrumSavePath + SpectrumSaveName + (++SaveCount).ToString("#000") + ".txt";
            //SpectrumSavePath += fname;
            File.WriteAllText(fname, dataToWrite.ToString(), Encoding.Unicode);

            // xPoints = new double[avantes.m_DevNr * 2048];//2.29调试看看会不会清零
            //yPoints = new double[avantes.m_DevNr * 2048];
            //dataReadyCount = 0;

            //}
        }
        #region   保存数据采用SveData测试满足条件
        /*2.29修改  保存数据采用SveData测试满足条件  为什么采用SaveData1呢？*/
        //private void SaveDate1()
        // {
        //     StringBuilder dataToWrite = new StringBuilder();
        //     Array.Sort(xPoints, yPoints);
        //    // dataToWrite.Append(ypcs + Environment.NewLine);
        //     //dataToWrite.Append(sycs + Environment.NewLine);
        //     for (int i = 0; i < AVS5216.MAX_NR_PIXELS_PER_CHANNEL * avantes.m_DevNr; i++)
        //     {
        //         dataToWrite.Append(xPoints[i].ToString() + ";" + yPoints[i].ToString() + Environment.NewLine);
        //     }

        //     //if (!File.Exists(path))
        //     //{
        //     // Create a file to write to.

        //     string fname = "d://" + SpectrumSaveName + "000" + ".txt";
        //     //SpectrumSavePath += fname;
        //     File.WriteAllText(fname, dataToWrite.ToString(), Encoding.Unicode);

        //     xPoints = new double[avantes.m_DevNr * 2048];
        //     yPoints = new double[avantes.m_DevNr * 2048];
        //     dataReadyCount = 0;

        //     //}
        // }
        #endregion
        #endregion
        #region 另存为excel
        List<string> resultFile = new List<string>();
        int N;
        private void btnSaveAsExcel_Click(object sender, EventArgs e)
        {

            string strLine;
            string[] strArray;
            char[] charArray = new char[] { ';' };
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Title = "Open Album";
            dlg.Filter = "Album files(*.txt)|*.txt|" + "All files(*.*)|*.*";
            dlg.InitialDirectory = "c:\\";
            dlg.RestoreDirectory = true;
            System.Data.DataTable dt = new System.Data.DataTable();
            DataColumn dc;
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            // dc.ColumnName = "波长";
            dc.Unique = false;
            dt.Columns.Add(dc);
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            // dc.ColumnName = "光谱强度";
            dc.Unique = false;
            dt.Columns.Add(dc);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                N = dlg.FileNames.Length;
                for (int i = 0; i < N; i++)
                {
                    resultFile.Add(dlg.FileNames[i]);
                }
            }
            //try
            //{


            for (int i = 0; i < N; i++)
            {
                dt.Clear();
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook;
                Microsoft.Office.Interop.Excel.Worksheet worksheet;
                excel.Visible = true;                            //是Excel可见
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Worksheet)workbook.ActiveSheet;
                FileStream aFile = new FileStream(resultFile[i], FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                //strLine = sr.ReadLine();
                for (int i1 = 0; i1 < 12; i1++)
                {
                    strLine = sr.ReadLine();
                    strArray = strLine.Split(':');
                    string xx = strArray[0];
                    string yy = strArray[1];
                    DataRow dr = dt.NewRow();
                    dr[0] = xx;
                    dr[1] = yy;
                    dt.Rows.Add(dr);
                }
                DataRow dr1 = dt.NewRow();
                dr1[0] = "波长";
                dr1[1] = "强度";
                dt.Rows.Add(dr1);
                strLine = sr.ReadLine();
                while (strLine != null)
                {
                    strArray = strLine.Split(charArray);
                    double x = Double.Parse(strArray[0]);
                    double y = Double.Parse(strArray[1]);
                    dt.Rows.Add(x, y);
                    strLine = sr.ReadLine();
                }

                sr.Close();
                DataTableToExcel(dt, worksheet);
                workbook.Saved = true;
                string name = System.IO.Path.GetFileNameWithoutExtension(dlg.FileNames[i]);
                string name1 = System.IO.Path.GetDirectoryName(dlg.FileNames[i]);
                workbook.SaveCopyAs(name1 + "\\" + name + ".xlsx");
                excel.Quit();

            }

        }
        public void DataTableToExcel(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet excelSheet)
        {
            int rowCount = dt.Rows.Count;
            int colCount = dt.Columns.Count;
            object[,] dataArray = new object[rowCount + 1, colCount];
            for (int k = 0; k < colCount; k++)
            {
                dataArray[0, k] = dt.Columns[k].ColumnName;
            }
            for (int i = 1; i < rowCount + 1; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    dataArray[i, j] = dt.Rows[i - 1][j];
                }
            }
            Microsoft.Office.Interop.Excel.Range r = excelSheet.Range["A1", excelSheet.Cells[rowCount + 1, colCount]];
            r.Value2 = dataArray;
        }
        #endregion
        #endregion

        #region 事件响应

        private void lbxMsg_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            System.Windows.Forms.ListBox lb = (System.Windows.Forms.ListBox)sender;
            g.DrawString(((ListItemColor)lb.Items[e.Index]).Text, e.Font, new SolidBrush(((ListItemColor)lb.Items[e.Index]).Color), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        #region 光谱仪

        private void avantes_RunErrorEvent(string msg)
        {
            this.UIThread(delegate
            {
                lbxMsg.Items.Insert(0, new ListItemColor(msg, Color.Red));
            });
        }

        private void avantes_StatusEvent(string msg)
        {
            this.UIThread(delegate
            {
                lbxMsg.Items.Insert(0, new ListItemColor(msg));
                for (int i = 0; i < AVS5216.AVS_GetNrOfDevices(); i++)
                {
                    lbRange.Text += avantes.listDevice[i].lambdarange + "nm";//注：此处只考虑一个通道，多通道时请重新编写
                    //lbRange.Text = "";
                }
            });
        }

        /*    private void avantes_AvantesFinished()
            {
                xPoints = new double[6144];
                yPoints = new double[6144];

                stopFlag = true;
                StopAcqUI();

            }
    */
        private void avantes_OpenErrorEvent()
        {
            this.UIThread(delegate
            {
                //lbxMsg.Items.Insert(0, new ListItemColor("光谱仪：连接失败", Color.Red));
                lbRange.Text = "未连接";
            });
        }

        static double[] xPoints;
        static double[] yPoints;


        public void AvantesDrawSpectum(double[] x, double[] y, int index)
        {


            this.UIThread(() => DrawSpectumUI(x, y, index));
            int j = 0;
            //来一次就存到数组里，直到数组满
            for (int i = index * 2048; i < index * 2048 + AVS5216.MAX_NR_PIXELS_PER_CHANNEL; i++, j++)
            {
                xPoints[i] = x[j];
                yPoints[i] = y[j];
            }
            if (ckbAutoSaveSpectrum.Checked == true)
            {
                /*2016年修改2.29
                //dataReadyCount++;
               // x.CopyTo(xPoints, index * 2048);
               // y.CopyTo(yPoints, index * 2048);
                */
                //int j = 0;
                ////来一次就存到数组里，直到数组满
                //for (int i = index * 2048; i < index * 2048 + AVS5216.MAX_NR_PIXELS_PER_CHANNEL; i++, j++)
                //{                    
                //    xPoints[i] = x[j];
                //    yPoints[i] = y[j];
                //}

                // if (dataReadyCount == avantes.m_DevNr&&)  //dataReadyCount代表已获取几个通道的数据
                if (x[0] != 0)
                {

                    SaveDate();

                    //SaveDate(SpectrumSavePath, xPoints, yPoints);

                }
                //if (x[0] != 0)
                //{
                //    SaveDate(SpectrumSavePath, x, y);
                //}

            }
        }


        private const int WM_USER = 0x400;
        private const int WM_MEAS_READY = WM_USER + 1;
        private const int WM_DBG_INFOAs = WM_USER + 2;
        private const int WM_DEVICE_RESET = WM_USER + 3;
        private const int WM_DEVICECHANGE = 0x219;

        private const int DBT_DEVICEARRIVAL = 0x8000;          // system detected a new device 
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;          // device is gone 

        private double[] YPoint = new double[2048];
        /// <summary>
        /// 数据采集完毕，光谱仪向窗体传递消息
        /// </summary>
        /// <param name="a_WinMess"></param>       
        protected override void WndProc(ref System.Windows.Forms.Message a_WinMess)
        {
            bool[] isReady = { true, true, true };
            //ulong l_Ticks = 0;
            int deviceIndex = 0;
            bool l_Found = false;

            #region 获取光谱数据
            if (a_WinMess.Msg == WM_MEAS_READY)
            {
                if (AVS5216.ERR_SUCCESS == (int)a_WinMess.WParam) // normal measurements。 a_WinMess.WParam>0时为StoreToRam模式，未考虑。
                {
                    for (int i = 0; i < avantes.m_DevNr; i++)
                    {

                        if (avantes.listDevice[i].avsDeviceHandle == (int)a_WinMess.LParam)
                        {
                            //lbxMsg.Items.Insert(0, new ListItemColor("deviceIndex:" + deviceIndex.ToString()));
                            deviceIndex = i;
                            avantes.listDevice[deviceIndex].ready2Read = true;
                            //lbxMsg.Items.Insert(0, new ListItemColor("ready2" + avantes.listDevice[deviceIndex].ready2Read.ToString()));
                            l_Found = true;
                            break;
                        }
                    }
                    if (l_Found == true)
                    {
                        AVS5216.PixelArrayType l_pSpectrum = new AVS5216.PixelArrayType();
                        //AVS5216.PixelArrayType m_Lambda = new AVS5216.PixelArrayType();
                        //AVS5216.AVS_GetLambda((IntPtr)avantes.listDevice[deviceIndex].avsDeviceHandle, ref avantes.listDevice[deviceIndex].lambda);

                        uint l_Time = 0;

                        if (AVS5216.ERR_SUCCESS == (int)AVS5216.AVS_GetScopeData((IntPtr)avantes.listDevice[deviceIndex].avsDeviceHandle, ref l_Time, ref l_pSpectrum))
                        {

                            avantes.listDevice[deviceIndex].m_Measurements++;
                            if (ckbLibsModel.Checked)
                            {
                                //lbxMsg.Items.Insert(0, new ListItemColor("采集了一次"));
                                /*
                                 * ckbLibsModel.Checked==true表示处于LIBS模式下，在该模式下，
                                 * 每次采集的强度数据l_pSpectrum存储在dataRom中，直到采集完毕后才处理数据。
                                 * 当最后一次采集完毕时，把dataRom的数据按各个通道独自加和求平均值。
                                 * 非LIBS模式下的数据直接显示。
                                 */
                                dataRom[deviceIndex].Add(l_pSpectrum.Value);
                                //lbxMsg.Items.Insert(0, new ListItemColor("dataRom"+ dataRom[deviceIndex].Count));
                                //MessageBox.Show("dataRom" + dataRom[deviceIndex].Count);
                                //MessageBox.Show("deviceIndex" +deviceIndex.ToString()+ ":count" + LIBScount.ToString());
                                if (LIBScount == 0 && isAllReady())
                                {
                                    //lbxMsg.Items.Insert(0, new ListItemColor("画光谱"));
                                    LIBScount = (int)nudScansToAverage.Value - 1;
                                    //MessageBox.Show("deviceIndex" + deviceIndex.ToString() + ":count" + LIBScount.ToString());
                                    for (int i = 0; i < avantes.m_DevNr; i++)
                                    {
                                        for (int j = 0; j < nudScansToAverage.Value; j++)
                                        {
                                            YPoint = YPoint.Zip(dataRom[i][j], (a, b) => a + (b / (double)nudScansToAverage.Value)).ToArray();
                                        }
                                        OnDataEvent(avantes.listDevice[i].lambda.Value, YPoint, i);
                                        wave[i].Clear();
                                        count[i].Clear();
                                        foreach (double element in avantes.listDevice[i].lambda.Value)
                                        {
                                            wave[i].Add(element);
                                        }
                                        foreach (double element in YPoint)
                                        {
                                            count[i].Add(element);
                                        }
                                        YPoint = new double[2048];
                                    }
                                    avantes.StopMeasurement();
                                    stopFlag = false;
                                    dataRom = new List<double[]>[avantes.m_DevNr];
                                    for (int i = 0; i < avantes.m_DevNr; i++)
                                    {
                                        dataRom[i] = new List<double[]>();
                                        //dataRom[i].Clear();
                                    }
                                    //avantes.listDevice.ForEach(d => d.ready2Read = false);
                                    //StartAcqUI();
                                }
                                else
                                {
                                    if (LIBScount > 0 && isAllReady())
                                    {
                                        LIBScount--;
                                    }
                                }
                                if (isAllReady())
                                {
                                    avantes.StopMeasurement();
                                    stopFlag = false;
                                    //StopAcqUI();                                  
                                    avantes.listDevice.ForEach(d => d.ready2Read = false);
                                    //StartAcqUI();
                                    avantes.Measure();//开始下一次测量

                                }

                            }
                            else
                            {

                                OnDataEvent(avantes.listDevice[deviceIndex].lambda.Value, l_pSpectrum.Value, deviceIndex);
                                foreach (double element in avantes.listDevice[deviceIndex].lambda.Value)
                                {
                                    wave[deviceIndex].Add(element);
                                }
                                foreach (double element in l_pSpectrum.Value)
                                {
                                    count[deviceIndex].Add(element);
                                }
                            }

                        }
                        else
                        {
                            lbxMsg.Items.Insert(0, new ListItemColor("Meas.Status: no data avail."));
                        }
                    }
                }
                else // message.WParam < 0 indicates error 
                {
                    MessageBox.Show("Meas.Status: failed. Error " + a_WinMess.WParam.ToString());
                    avantes.listDevice[deviceIndex].m_Failures++;
                }

                /*采集的数据接收完毕后，需要考虑下一次采集：
                 * 根据光谱仪控制的原理，AVS_Measure（）中的采集次数是指每次执行该函数是采集的次数
                 * 在外触发模式下，每个外触发获取后就执行一遍AVS_Measure，直到外触发次数到达，故执行次数应始终为1。
                 * 内触发模式下，采集停止是根据停止键是否按下判断的，故每次获取采集数据后，只要停止键未按下，就执行AVS_Measure准备下一次测量。
                 * 多通道情况下，要综合考虑每个通道的数据是否都接受完毕，故加入dataIsReady.Equals(isReady)
                 */

                //&& isAllReady()
                //if (stopFlag == false && isAllReady())//stopFlag==true时表示停止键已被按下
                //{
                //    //MessageBox.Show(isAllReady().ToString());
                //    if (ckbLibsModel.Checked)
                //    {

                //        avantes.Measure();//开始下一次测量
                //        StopAcqUI();
                //        //if (LIBScount >=0)
                //        //{
                //        //    LIBScount--;
                //        //    //dataIsReady = new bool[avantes.m_DevNr];
                //        //    //avantes.listDevice.ForEach(d => d.ready2Read = false);
                //        //    //MessageBox.Show("if");
                //        //    avantes.Measure();//开始下一次测量
                //        //}
                //    }
                //    else
                //    {
                //        //dataIsReady = new bool[avantes.m_DevNr];
                //        avantes.listDevice.ForEach(d => d.ready2Read = false);
                //        avantes.Measure();//开始下一次测量                       
                //    }

                //}

            }
            #endregion

            #region 设备移除或接入
            else
            {
                if (a_WinMess.Msg == WM_DEVICECHANGE)
                {
                    //if ((int)a_WinMess.WParam == DBT_DEVICEARRIVAL)//检测接入设备
                    //{

                    //    if (avantes.m_DevNr != avantes.GetNumDevice())
                    //    {
                    //        lbRange.Text = "连接中.......";
                    //        System.Windows.Forms.Application.DoEvents();
                    //        //MessageBox.Show("here");
                    //        for (int i = 0; i < avantes.m_DevNr; i++)
                    //        {
                    //            avantes.DeactivateDevice(i);
                    //        }
                    //        avantes.UpdateList();
                    //    }
                    //}
                    if ((int)a_WinMess.WParam == DBT_DEVICEREMOVECOMPLETE)
                    {
                        if (avantes.m_DevNr != avantes.GetNumDevice())
                        {
                            for (int i = 0; i < avantes.m_DevNr; i++)
                            {
                                avantes.DeactivateDevice(i);
                            }
                            //avantes.DisConnect();
                            avantes.m_DevNr = 0;
                            MessageBox.Show("光谱仪被移除");
                            lbRange.Text = "未连接";
                        }
                    }
                }
                else
                {
                    base.WndProc(ref a_WinMess);
                }
            }
            #endregion
        }

        bool isAllReady()
        {
            bool ready = true;
            for (int i = 0; i < avantes.m_DevNr; i++)
            {
                ready = ready && avantes.listDevice[i].ready2Read;
                //MessageBox.Show("read"+avantes.listDevice[i].ready2Read.ToString());
            }
            return ready;
        }
        #endregion

        #region 延时器
        private void DelayOpened()
        {
            this.UIThreadInvoke(delegate
            {
                numDelayTime.Enabled = true;
            });
        }

        private void DelayParaReady()
        {
            this.UIThreadInvoke(delegate
            {
                if (delayController.PolarE == '-')
                {
                    delayController.SetDelayPolar('e', '+');
                }

                if (delayController.PolarA == '-')
                {
                    delayController.SetDelayPolar('a', '+');
                }

                Decimal delaytime = Convert.ToDecimal(delayController.DelayTimeA);
                delaytime /= 10000;
                numDelayTime.Value = delaytime;
            });

        }

        private void DelayErr(string msg)
        {

            this.UIThreadInvoke(delegate
            {
                //lbxMsg.Items.Insert(0, new ListItemColor(msg, Color.Red));
            });

        }
        #endregion

        //        #region 相机
        //        private void btnStartCam_Click_1(object sender, EventArgs e)
        //        {
        //            if (m_hCamera < 1)//还未初始化相机
        //            {
        //                if (InitCamera() == true)
        //                {
        //                    MvApi.CameraPlay(m_hCamera);
        //                    btnStartCam.Enabled = false;
        //                    btnPauseCam.Enabled = true;
        //                }
        //            }
        //            else//已经初始化
        //            {
        //                MvApi.CameraPlay(m_hCamera);
        //                btnStartCam.Enabled = false;
        //                btnPauseCam.Enabled = true;
        //            }

        //        }

        //        private void btnPauseCam_Click_1(object sender, EventArgs e)
        //        {
        //            MvApi.CameraPause(m_hCamera);

        //            btnStartCam.Enabled = true;
        //            btnPauseCam.Enabled = false;
        //        }

        //        private void btnCamConfig_Click(object sender, EventArgs e)
        //        {
        //            if (m_hCamera > 0)
        //            {
        //                MvApi.CameraShowSettingPage(m_hCamera, 1);//1 show ; 0 hide
        //            }
        //        }

        //#if USE_CALL_BACK
        //        public void ImageCaptureCallback(CameraHandle hCamera, uint pFrameBuffer, ref tSdkFrameHead pFrameHead, uint pContext)
        //        {
        //            //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
        //            MvApi.CameraImageProcess(hCamera, pFrameBuffer, (IntPtr)((int)m_ImageBuffer&(~0xf)), ref pFrameHead);
        //            //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。   
        //            MvApi.CameraImageOverlay(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
        //            //调用SDK封装好的接口，显示预览图像
        //            MvApi.CameraDisplayRGB24(hCamera, (IntPtr)((int)m_ImageBuffer & (~0xf)), ref pFrameHead);
        //            m_tFrameHead = pFrameHead;
        //            m_iDisplayedFrames++;

        //            if (pFrameHead.iWidth != m_tFrameHead.iWidth || pFrameHead.iHeight != m_tFrameHead.iHeight)
        //            {
        //                timer2.Enabled = true;
        //                timer2.Start();
        //                m_tFrameHead = pFrameHead;
        //            }

        //        }
        //#else
        //        public void CaptureThreadProc()
        //        {
        //            CameraSdkStatus eStatus;
        //            tSdkFrameHead FrameHead;
        //            uint uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数

        //            while (m_bExitCaptureThread == false)
        //            {
        //                //500毫秒超时,图像没捕获到前，线程会被挂起,释放CPU，所以该线程中无需调用sleep
        //                eStatus = MvApi.CameraGetImageBuffer(m_hCamera, out FrameHead, out uRawBuffer, 500);

        //                if (eStatus == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//如果是触发模式，则有可能超时
        //                {
        //                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
        //                    MvApi.CameraImageProcess(m_hCamera, uRawBuffer, m_ImageBuffer, ref FrameHead);
        //                    //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。    
        //                    MvApi.CameraImageOverlay(m_hCamera, m_ImageBuffer, ref FrameHead);
        //                    //调用SDK封装好的接口，显示预览图像
        //                    MvApi.CameraDisplayRGB24(m_hCamera, m_ImageBuffer, ref FrameHead);
        //                    //成功调用CameraGetImageBuffer后必须释放，下次才能继续调用CameraGetImageBuffer捕获图像。
        //                    MvApi.CameraReleaseImageBuffer(m_hCamera, uRawBuffer);

        //                    if (FrameHead.iWidth != m_tFrameHead.iWidth || FrameHead.iHeight != m_tFrameHead.iHeight)
        //                    {
        //                        m_tFrameHead = FrameHead;
        //                        //分辨率改变时，需要刷新picturebox，此处忽略
        //                    }
        //                    m_iDisplayedFrames++;
        //                }

        //            }

        //        }
        //#endif

        //        /*相机配置窗口的消息回调函数
        //        hCamera:当前相机的句柄
        //        MSG:消息类型，
        //	    SHEET_MSG_LOAD_PARAM_DEFAULT	= 0,//加载默认参数的按钮被点击，加载默认参数完成后触发该消息,
        //	    SHEET_MSG_LOAD_PARAM_GROUP		= 1,//切换参数组完成后触发该消息,
        //	    SHEET_MSG_LOAD_PARAM_FROMFILE	= 2,//加载参数按钮被点击，已从文件中加载相机参数后触发该消息
        //	    SHEET_MSG_SAVE_PARAM_GROUP		= 3//保存参数按钮被点击，参数保存后触发该消息
        //	    具体参见CameraDefine.h中emSdkPropSheetMsg类型

        //        uParam:消息附带的参数，不同的消息，参数意义不同。
        //	    当 MSG 为 SHEET_MSG_LOAD_PARAM_DEFAULT时，uParam表示被加载成默认参数组的索引号，从0开始，分别对应A,B,C,D四组
        //	    当 MSG 为 SHEET_MSG_LOAD_PARAM_GROUP时，uParam表示切换后的参数组的索引号，从0开始，分别对应A,B,C,D四组
        //	    当 MSG 为 SHEET_MSG_LOAD_PARAM_FROMFILE时，uParam表示被文件中参数覆盖的参数组的索引号，从0开始，分别对应A,B,C,D四组
        //	    当 MSG 为 SHEET_MSG_SAVE_PARAM_GROUP时，uParam表示当前保存的参数组的索引号，从0开始，分别对应A,B,C,D四组
        //        */
        //        public void SettingPageMsgCalBack(CameraHandle hCamera, uint MSG, uint uParam, uint pContext)
        //        {

        //        }

        //        private bool InitCamera()
        //        {
        //            tSdkCameraDevInfo[] tCameraDevInfoList = new tSdkCameraDevInfo[12];
        //            IntPtr ptr;
        //            int i;
        //#if USE_CALL_BACK
        //            CAMERA_SNAP_PROC pCaptureCallOld = null;
        //#endif
        //            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraDevInfo()) * 12);
        //            int iCameraCounts = 12;//如果有多个相机时，表示最大只获取最多12个相机的信息列表。该变量必须初始化，并且大于1
        //            if (m_hCamera > 0)
        //            {
        //                //已经初始化过，直接返回 true

        //                return true;
        //            }
        //            if (MvApi.CameraEnumerateDevice(ptr, ref iCameraCounts) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
        //            {
        //                for (i = 0; i < 12; i++)
        //                {
        //                    tCameraDevInfoList[i] = (tSdkCameraDevInfo)Marshal.PtrToStructure((IntPtr)((int)ptr + i * Marshal.SizeOf(new tSdkCameraDevInfo())), typeof(tSdkCameraDevInfo));
        //                }
        //                Marshal.FreeHGlobal(ptr);

        //                if (iCameraCounts >= 1)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
        //                {
        //                    if (MvApi.CameraInit(ref tCameraDevInfoList[0], -1, -1, ref m_hCamera) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
        //                    {
        //                        //获得相机特性描述
        //                        ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraCapbility()));
        //                        MvApi.CameraGetCapability(m_hCamera, ptr);
        //                        tCameraCapability = (tSdkCameraCapbility)Marshal.PtrToStructure(ptr, typeof(tSdkCameraCapbility));
        //                        Marshal.FreeHGlobal(ptr);
        //                        Marshal.FreeHGlobal(m_ImageBuffer);
        //                        m_ImageBuffer = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
        //                        m_ImageBufferSnapshot = Marshal.AllocHGlobal(tCameraCapability.sResolutionRange.iWidthMax * tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);

        //                        //初始化显示模块，使用SDK内部封装好的显示接口
        //                        MvApi.CameraDisplayInit(m_hCamera, pbxImage.Handle);
        //                        MvApi.CameraSetDisplaySize(m_hCamera, pbxImage.Width, pbxImage.Height);

        //                        //设置抓拍通道的分辨率。
        //                        tSdkImageResolution tResolution;
        //                        tResolution.fZoomScale = 1.0F;
        //                        tResolution.iVOffset = 0;
        //                        tResolution.iHOffset = 0;
        //                        tResolution.uBinMode = 0;
        //                        tResolution.uSkipMode = 0;
        //                        tResolution.iWidth = tCameraCapability.sResolutionRange.iWidthMax;
        //                        tResolution.iHeight = tCameraCapability.sResolutionRange.iHeightMax;
        //                        //tResolution.iIndex = 0xff;表示自定义分辨率,如果tResolution.iWidth和tResolution.iHeight
        //                        //定义为0，则表示跟随预览通道的分辨率进行抓拍。抓拍通道的分辨率可以动态更改。
        //                        //本例中将抓拍分辨率固定为最大分辨率。
        //                        tResolution.iIndex = 0xff;
        //                        tResolution.acDescription = new byte[32];//描述信息可以不设置
        //                        MvApi.CameraSetResolutionForSnap(m_hCamera, ref tResolution);

        //                        //让SDK来根据相机的型号动态创建该相机的配置窗口。
        //                        MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null, 0);

        //                        //两种方式来获得预览图像，设置回调函数或者使用定时器或者独立线程的方式，
        //                        //主动调用CameraGetImageBuffer接口来抓图。
        //                        //本例中仅演示了两种的方式,注意，两种方式也可以同时使用，但是在回调函数中，
        //                        //不要使用CameraGetImageBuffer，否则会造成死锁现象。
        //#if USE_CALL_BACK
        //                        MvApi.CameraSetCallbackFunction(m_hCamera, ImageCaptureCallback, m_iCaptureCallbackCtx, ref pCaptureCallOld);
        //#else //如果需要采用多线程，使用下面的方式
        //                        m_bExitCaptureThread = false;
        //                        m_tCaptureThread = new Thread(new ThreadStart(CaptureThreadProc));
        //                        m_tCaptureThread.Start();

        //#endif
        //                        //MvApi.CameraReadSN 和 MvApi.CameraWriteSN 用于从相机中读写用户自定义的序列号或者其他数据，32个字节
        //                        //MvApi.CameraSaveUserData 和 MvApi.CameraLoadUserData用于从相机中读取自定义数据，512个字节
        //                        return true;

        //                    }
        //                    else
        //                    {
        //                        m_hCamera = 0;

        //                        return false;
        //                    }


        //                }
        //            }

        //            return false;

        //        }


        //        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (xPoints != null)
            {
                //SaveDate1();2.29
            }
            if (xuanzewj == 1)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start(ofd.FileName);
                    xuanzewj = 0;
                }
            }
            else
            {
                System.Diagnostics.Process.Start(ofd.FileName);
            }
        }





        #endregion

        private void btnStartCam_Click(object sender, EventArgs e)
        {
            //if (m_hCamera < 1)//还未初始化相机
            //{
            //    if (InitCamera() == true)
            //    {
            //        MvApi.CameraPlay(m_hCamera);
            //        btnStartCam.Enabled = false;
            //        btnPauseCam.Enabled = true;
            //    }
            //}
            //else//已经初始化
            //{
            //    MvApi.CameraPlay(m_hCamera);
            //    btnStartCam.Enabled = false;
            //    btnPauseCam.Enabled = true;
            //}
            try
            {
                // 设备搜索 
                // device search 
                List<IDeviceInfo> li = Enumerator.EnumerateDevices();
                if (li.Count > 0)
                {
                    // 获取搜索到的第一个设备 
                    // get the first searched device 
                    m_dev = Enumerator.GetDeviceByIndex(0);

                    // 注册链接事件 
                    // register event callback 
                    m_dev.CameraOpened += OnCameraOpen;
                    m_dev.ConnectionLost += OnConnectLoss;
                    m_dev.CameraClosed += OnCameraClose;

                    // 打开设备 
                    // open device 
                    if (!m_dev.Open())
                    {
                        MessageBox.Show("Open camera failed");
                        return;
                    }
                    // 设置图像格式 
                    // set PixelFormat 
                    using (IEnumParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ImagePixelFormat])
                    {
                        p.SetValue("Mono8");
                    }

                    // 设置曝光 
                    // set ExposureTime 
                    using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.ExposureTime])
                    {
                        p.SetValue(52000);
                    }

                    // 设置增益 
                    // set Gain 
                    using (IFloatParameter p = m_dev.ParameterCollection[ParametrizeNameSet.GainRaw])
                    {
                        p.SetValue(1.2);
                    }

                    // 设置缓存个数为8（默认值为16） 
                    // set buffer count to 8 (default 16) 
                    m_dev.StreamGrabber.SetBufferCount(8);

                    // 注册码流回调事件 
                    // register grab event callback 
                    m_dev.StreamGrabber.ImageGrabbed += OnImageGrabbed;

                    // 开启码流 
                    // start grabbing 
                    if (!m_dev.GrabUsingGrabLoopThread())
                    {
                        MessageBox.Show(@"Start grabbing failed");
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Catcher.Show(exception);
            }
        }
        // 码流数据回调 
        // grab callback function 
        private void OnImageGrabbed(Object sender, GrabbedEventArgs e)
        {
            m_mutex.WaitOne();
            m_frameList.Add(e.GrabResult.Clone());
            m_mutex.ReleaseMutex();
        }

        // 停止码流 
        // stop grabbing 
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dev == null)
                {
                    throw new InvalidOperationException("Device is invalid");
                }

                m_dev.StreamGrabber.ImageGrabbed -= OnImageGrabbed;         // 反注册回调 | unregister grab event callback 
                m_dev.ShutdownGrab();                                       // 停止码流 | stop grabbing 
                m_dev.Close();                                              // 关闭相机 | close camera 
            }
            catch (Exception exception)
            {
                Catcher.Show(exception);
            }
        }

        private void btnPauseCam_Click(object sender, EventArgs e)
        {
            //MvApi.CameraPause(m_hCamera);
            //btnStartCam.Enabled = true;
            //btnPauseCam.Enabled = false;

            try
            {
                if (m_dev == null)
                {
                    throw new InvalidOperationException("Device is invalid");
                }

                m_dev.StreamGrabber.ImageGrabbed -= OnImageGrabbed;         // 反注册回调 | unregister grab event callback 
                m_dev.ShutdownGrab();                                       // 停止码流 | stop grabbing 
                m_dev.Close();                                              // 关闭相机 | close camera 
            }
            catch (Exception exception)
            {
                Catcher.Show(exception);
            }
        }

        // 窗口关闭 
        // Window Closed 
        protected override void OnClosed(EventArgs e)
        {
            if (m_dev != null)
            {
                m_dev.Dispose();
                m_dev = null;
            }

            m_bShowLoop = false;
            renderThread.Join();
            if (_g != null)
            {
                _g.Dispose();
                _g = null;
            }
            base.OnClosed(e);
        }

        /// <summary>
        /// 点击放大图片
        /// </summary>还不好使
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //if (pbxImage.Image == null)
            //{
            //    MessageBox.Show("img is nul");
            //    return;
            //}
            //BigImgFrm  frm = new BigImgFrm (this.pbxImage.Image);//传参
            //frm.Show();
            Bitmap b = new Bitmap(1000, 1000);
            pbxImage.DrawToBitmap(b, new System.Drawing.Rectangle(0, 0, 1000, 1000));
            b.Save("c:/1.jpg");
            BigImgFrm f = new BigImgFrm(b);
            f.Show();
            //BigImgFrm frm = new BigImgFrm(this.pbxImage.Image);
            //frm.ShowDialog();

            //原始图片的宽、高 
            // int initWidth = initImage.Width;
            // int initHeight = initImage.Height;
        }




        private void tbnExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"LIBS数据预处理与分析\WindowsFormsApplication1.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据处理软件调用失败，请及时保存数据" + ex.ToString());
            }
        }

        private void ShowThread()
        {
            while (m_bShowLoop)
            {
                if (m_frameList.Count == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                // 图像队列取最新帧 
                // always get the latest frame in list 
                m_mutex.WaitOne();
                IGrabbedRawData frame = m_frameList.ElementAt(m_frameList.Count - 1);
                m_frameList.Clear();
                m_mutex.ReleaseMutex();

                // 主动调用回收垃圾 
                // call garbage collection 
                GC.Collect();

                // 控制显示最高帧率为25FPS 
                // control frame display rate to be 25 FPS 
                if (false == isTimeToDisplay())
                {
                    continue;
                }

                try
                {
                    // 图像转码成bitmap图像 
                    // raw frame data converted to bitmap 
                    var bitmap = frame.ToBitmap(false);
                    m_bShowByGDI = true;
                    if (m_bShowByGDI)
                    {
                        // 使用GDI绘图 
                        // create graphic object 
                        if (_g == null)
                        {
                            _g = pbxImage.CreateGraphics();
                        }
                        _g.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, pbxImage.Width, pbxImage.Height),
                        new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                        bitmap.Dispose();
                    }
                    else
                    {
                        // 使用控件绘图,会造成主界面卡顿 
                        // assign bitmap to image control will cause main window reflect slowly  
                        if (InvokeRequired)
                        {
                            BeginInvoke(new MethodInvoker(() =>
                            {
                                try
                                {
                                    pbxImage.Image = bitmap;
                                }
                                catch (Exception exception)
                                {
                                    Catcher.Show(exception);
                                }
                            }));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Catcher.Show(exception);
                }
            }
        }
        const int DEFAULT_INTERVAL = 40;
        private bool isTimeToDisplay()
        {
            m_stopWatch.Stop();
            long m_lDisplayInterval = m_stopWatch.ElapsedMilliseconds;
            if (m_lDisplayInterval <= DEFAULT_INTERVAL)
            {
                m_stopWatch.Start();
                return false;
            }
            else
            {
                m_stopWatch.Reset();
                m_stopWatch.Start();
                return true;
            }
        }
        private IDevice m_dev;
        // 相机打开回调 
        // camera open event callback 
        private void OnCameraOpen(object sender, EventArgs e)
        {

            lbxMsg.Items.Insert(0, new ListItemColor("相机已连接"));
            this.Invoke(new System.Action(() =>
            {
                btnStartCam.Enabled = false;
                btnPauseCam.Enabled = true;
            }));
        }

        // 相机关闭回调 
        // camera close event callback 
        private void OnCameraClose(object sender, EventArgs e)
        {
            lbxMsg.Items.Insert(0, new ListItemColor("相机已关闭"));
            this.Invoke(new System.Action(() =>
            {
                btnStartCam.Enabled = true;
                btnPauseCam.Enabled = false;
            }));
        }
        // 相机丢失回调 
        // camera disconnect event callback 
        private void OnConnectLoss(object sender, EventArgs e)
        {
            lbxMsg.Items.Insert(0, new ListItemColor("相机丢失"));
            m_dev.ShutdownGrab();
            m_dev.Dispose();
            m_dev = null;

            this.Invoke(new System.Action(() =>
            {
                btnStartCam.Enabled = true;
                btnPauseCam.Enabled = false;
            }));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string element_name = toolStripTextBox1.Text;
                string[] rows = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "element.txt", Encoding.Default);
                string[] str_count = rows[0].Split(';');
                string[,] NIST = new string[rows.Length, 3];//元素谱线类型,波长,元素名
                List<int> location = new List<int>();
                for (int i = 0; i < rows.Length; i++)
                {
                    string[] s = new string[str_count.Length];
                    s = rows[i].Split(';');
                    NIST[i, 0] = s[0];
                    NIST[i, 1] = s[1];
                    NIST[i, 2] = s[2];
                }
                int[] indexes = Enumerable.Range(0, NIST.GetLength(0)).Where(i => NIST[i, 2] == element_name).ToArray();
                double temp = 0.0;
                for (int i = 0; i < indexes.Length; i++)
                {
                    if (NIST[indexes[i], 1] != "NaN" & NIST[i, 1] != "")
                    {
                        temp = Convert.ToDouble(NIST[indexes[i], 1]);
                        if (OpenCount > 0)
                        {
                            if (temp >= wave[avantes.m_DevNr + OpenCount - 1][0] && temp <= wave[avantes.m_DevNr + OpenCount - 1][wave[avantes.m_DevNr + OpenCount - 1].Count - 1])
                            {
                                for (int j = 0; j < wave[avantes.m_DevNr + OpenCount - 1].Count - 1; j++)
                                {
                                    if (wave[avantes.m_DevNr + OpenCount - 1][j] < temp) continue;
                                    else
                                    {
                                        int closest = Math.Abs(wave[avantes.m_DevNr + OpenCount - 1][j - 1] - temp) < Math.Abs(wave[avantes.m_DevNr + OpenCount - 1][j] - temp) ? j - 1 : j;
                                        location.Add(closest);
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
                //将强度数组显示出来
                List<ColumnHeader> columns = new List<ColumnHeader>();
                foreach (ColumnHeader column in listView1.Columns)
                {
                    columns.Add(column);
                }
                listView1.Clear();
                foreach (ColumnHeader column in columns)
                {
                    listView1.Columns.Add(column);
                }
                listView1.Columns[0].Text = element_name + "元素的波长";
                listView1.Columns[1].Text = "强度值";
                for (int i = 0; i < location.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem(new string[] { wave[avantes.m_DevNr + OpenCount - 1][location[i]].ToString(), count[avantes.m_DevNr + OpenCount - 1][location[i]].ToString() });
                    listView1.Items.Add(lvi);
                }
                //在谱线上将标记箭头
                var plotModel = plotSpectrum.Model;

                // 检查并删除所有的箭头注释
                for (int i = plotModel.Annotations.Count - 1; i >= 0; i--)
                {
                    if (plotModel.Annotations[i] is ArrowAnnotation)
                    {
                        plotModel.Annotations.RemoveAt(i);
                    }
                }

                //// 刷新绘图视图以显示改动
                //plotSpectrum.InvalidatePlot(true);
                var points = new List<DataPoint>();
                for (int i = 0; i < location.Count; i++)
                {
                    points.Add(new DataPoint(wave[avantes.m_DevNr + OpenCount - 1][location[i]], count[avantes.m_DevNr + OpenCount - 1][location[i]]));
                }
                foreach (var point in points)
                {
                    var arrowAnnotation = new ArrowAnnotation
                    {
                        StartPoint = new DataPoint(point.X, point.Y + 6000), // 要标记的点的坐标
                        EndPoint = point, // 箭头指向的坐标
                        Color = OxyColors.Green,
                        StrokeThickness = 1
                    };
                    plotSpectrum.Model.Annotations.Add(arrowAnnotation);
                }

                plotSpectrum.RefreshPlot(true);
            }
            catch
            {
                MessageBox.Show("查找元素失败");

            }


        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="wave"></param>
        /// <param name="count"></param>
        public void Writetxt(List<double> wave,List<double> count)
        {
            using (StreamWriter sw=new StreamWriter("滤噪后.txt"))
            {
                for (int i=0;i<wave.Count;i++)
                {
                    sw.WriteLine(wave[i].ToString()+";"+count[i].ToString("F2"));
                }
            }
        }



        private void 滤噪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateProcess dateProcess = new DateProcess();
                if (OpenCount > 0)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        double[] result = dateProcess.IDWT(count[avantes.m_DevNr + OpenCount - 1].ToArray(), "db5");
                        for (int m = 0; m < result.Length; m++)
                        {
                            count[avantes.m_DevNr + OpenCount - 1][m] = result[m];
                        }
                    }
                    //double[] result = new double[count[avantes.m_DevNr + OpenCount - 1].Count];
                    //MessageBox.Show(count[avantes.m_DevNr + OpenCount - 1].Count.ToString() + "+" + wave[avantes.m_DevNr + OpenCount - 1].Count.ToString());
                    this.UIThread(() => DrawLoadSpectumUI(wave[avantes.m_DevNr + OpenCount - 1].ToArray(), count[avantes.m_DevNr + OpenCount - 1].ToArray(), OpenCount));
                    Task task = new Task(()=>Writetxt(wave[avantes.m_DevNr + OpenCount - 1], count[avantes.m_DevNr + OpenCount - 1]));
                    task.Start();
                }
                else
                {
                    if (avantes.m_DevNr > 0)
                    {
                        for (int i = 0; i < avantes.m_DevNr; i++)
                        {
                            for (int j = 0; j < 15; j++)
                            {
                                double[] result = dateProcess.IDWT(count[i].ToArray(), "db5");
                                for (int m = 0; m < result.Length; m++)
                                {
                                    count[i][m] = result[m];
                                }
                            }
                            this.UIThread(() => DrawSpectumUI(wave[i].ToArray(), count[i].ToArray(), i));
                        }
                        lbxMsg.Items.Insert(0, new ListItemColor("滤噪成功", Color.Blue));
                    }
                    else
                    {
                        MessageBox.Show("请输入光谱数据");
                    }
                }
            }
            catch
            {
                MessageBox.Show("数据存在问题");
            }
        }

        private void 基线校正ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateProcess dateProcess = new DateProcess();
                double[] cy, result;
                if (OpenCount > 0)
                {
                    result = dateProcess.BaselineCorrect2(wave[avantes.m_DevNr + OpenCount - 1].ToArray(),count[avantes.m_DevNr + OpenCount - 1].ToArray(),5,21,5);
                    //dateProcess.baselineCorrection(wave[avantes.m_DevNr + OpenCount - 1].ToArray(), count[avantes.m_DevNr + OpenCount - 1].ToArray(), 5, 3, 2, 11, out cy, out result);
                    //this.UIThread(() => DrawLoadSpectumUI(wave[avantes.m_DevNr + OpenCount - 1].ToArray(), cy, OpenCount));
                    this.UIThread(() => DrawLoadSpectumUI(wave[avantes.m_DevNr + OpenCount - 1].ToArray(), result, OpenCount));
                }
                else
                {
                    if (avantes.m_DevNr > 0)
                    {
                        for (int i = 0; i < avantes.m_DevNr; i++)
                        {
                            dateProcess.baselineCorrection(wave[i].ToArray(), count[i].ToArray(), 5, 3, 2, 7, out cy, out result);
                            this.UIThread(() => DrawSpectumUI(wave[i].ToArray(), result, i));
                            lbxMsg.Items.Insert(0, new ListItemColor("基线校正成功", Color.Red));
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入光谱数据");
                    }
                }
            }
            catch
            {
                MessageBox.Show("数据存在问题");
            }
        }

        private void pbxImage_SizeChanged(object sender, EventArgs e)
        {
            _g = null;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string[] rows = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "element.txt", Encoding.Default);
            string[] str_count = rows[0].Split(';');
            string[,] NIST = new string[rows.Length, 3];//元素谱线类型,波长,元素名
            for (int i = 0; i < rows.Length; i++)
            {
                string[] s = new string[str_count.Length];
                s = rows[i].Split(';');
                NIST[i, 0] = s[0];
                NIST[i, 1] = s[1];
                NIST[i, 2] = s[2];
            }
            DateProcess dateProcess = new DateProcess();
            dateProcess.qualitativeAnalysis(wave[avantes.m_DevNr + OpenCount - 1].ToArray(), count[avantes.m_DevNr + OpenCount - 1].ToArray(), 200, 800, 100, 15, NIST, out markPosition, out eleTypeList, out eleNameList, out eleIntensity);
            string[] singleNameList = eleNameList.Distinct().ToArray();
            List<ColumnHeader> columns = new List<ColumnHeader>();
            foreach (ColumnHeader column in listView1.Columns)
            {
                columns.Add(column);
            }
            listView1.Clear();
            foreach (ColumnHeader column in columns)
            {
                listView1.Columns.Add(column);
            }
            listView1.Columns[0].Text = "编号";
            listView1.Columns[1].Text = "可能的元素";
            for (int i = 0; i < singleNameList.Length; i++)
            {
                ListViewItem lvi = new ListViewItem(new string[] { Convert.ToString(i + 1), singleNameList[i].ToString() });
                listView1.Items.Add(lvi);
            }


        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.UIThread(delegate
            {
                //OpenCount = 0;
                var plotModel = plotSpectrum.Model;

                // 检查并删除所有的箭头注释
                for (int i = plotModel.Annotations.Count - 1; i >= 0; i--)
                {
                    if (plotModel.Annotations[i] is ArrowAnnotation)
                    {
                        plotModel.Annotations.RemoveAt(i);
                    }
                }
                foreach (LineSeries series in model.Series)
                {
                    series.Points.Clear(); // 清除所有数据点
                    series.Title = null;   // 清除标题
                }
                plotSpectrum.RefreshPlot(true);
            });
        }
    }
    static class ControlExtensions
    {
        //如果需要在不同的线程上执行，它将使用 Control.BeginInvoke 函数将操作安排在创建控件的线程上异步执行。
        //一旦操作被安排执行，控制权就会立即返回，所以这种方式不会阻塞当前线程。
        static public void UIThread(this Control control, System.Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }
        //与 UIThread 扩展方法比较，UIThreadInvoke 扩展方法将阻塞当前线程，直到UI线程上的操作完成。
        static public void UIThreadInvoke(this Control control, System.Action code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }

    class ListItemColor
    {
        string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public ListItemColor(string _text)
        {
            text = _text;
            color = Color.Black;
        }

        public ListItemColor(string _text, Color _color)
        {
            text = _text;
            color = _color;
        }
    }


}
