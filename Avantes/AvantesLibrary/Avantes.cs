using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Globalization;



namespace LIBS.Spectrometers
{
    public class Avantes
    {
        #region 事件

        #region 连接成功
        public delegate void AvantesOpenHandler();
        public event AvantesOpenHandler OpenEvent;
        public void OnOpenEvent()
        {
            if (this.OpenEvent != null)
            {
                this.OpenEvent();
            }
        }
        #endregion

        #region 连接失败
        public delegate void AvantesOpenErrorHandler();
        public event AvantesOpenErrorHandler OpenErrorEvent;
        public void OnOpenErrorEvent()
        {
            if (this.OpenErrorEvent != null)
            {
                this.OpenErrorEvent();
            }
        }
        #endregion

        #region 运行错误
        public delegate void AvantesRunErrorHandler(string msg);
        public event AvantesRunErrorHandler RunErrorEvent;
        public void OnRunErrorEvent(string msg)
        {
            if (this.RunErrorEvent != null)
            {
                this.RunErrorEvent(msg);
            }
        }
        #endregion

        #region 执行消息
        public delegate void AvantesStatusHandler(string msg);
        public event AvantesStatusHandler StatusEvent;
        public void OnStatusEvent(string msg)
        {
            if (this.StatusEvent != null)
            {
                this.StatusEvent(msg);
            }
        }
        #endregion

        //#region 采集结束
        //public delegate void AvantesFinishedHandler();
        //public event AvantesFinishedHandler AvantesFinished;
        //public void OnAvantesFinished()
        //{
        //    if (this.AvantesFinished != null)
        //    {
        //        this.AvantesFinished();
        //    }
        //}
        //#endregion

        #endregion

        #region 字段、属性


        public IntPtr m_handle; // 窗体句柄       
        public ushort m_StartPixels = 0;
        public ushort m_StopPixels = 2047;
        public int m_DevNr = 0;//连接的光谱仪数量

        public double m_Minlambda = 1000;//各通道光谱仪中波长最小值（用于确定谱图显示界面中波长范围）
        public double m_Maxlambda;//各通道光谱仪中波长最大值（用于确定谱图显示界面中波长范围）

        //public string[] deviceSerialNr;
        //public string[] deviceStatus;
        //public string[] deviceFullRange;
        //AVS5216.PixelArrayType[] m_Lambda;
        //public ushort[] m_NrPixels;//像素数
        //public uint[] m_Measurements;//采集成功的次数      
        //public uint[] m_Failures;//采集失败的次数
        //public long[] m_DeviceHandle;

        public class MyDevice
        {
            public long avsDeviceHandle = AVS5216.INVALID_AVS_HANDLE_VALUE;
            public AVS5216.AvsIdentityType avsID;
            //public AVS5216.DeviceConfigType avsConfig;
            public uint m_Measurements;
            public uint m_Failures;
            public AVS5216.PixelArrayType lambda;
            //public AVS5216.PixelArrayType spectrum;
            public string lambdarange;
            public bool ready2Read;
        };

        public List<MyDevice> listDevice = new List<MyDevice>();

        public string m_IntergrationTime = "10";
        public uint m_NrAverrages = 1;
        public int m_TriggerMode = 1;//硬件触发
        public byte m_CorDynDark = 1;
        #endregion

        #region 方法
        public int Open(IntPtr handle)
        {
            int count = 0;
            try
            {
                int l_Port = AVS5216.AVS_Init(0);

                AVS5216.AVS_Register(handle);
                if (l_Port > 0)
                {
                    m_handle = handle;
                    UpdateList();
                    count = m_DevNr;
                }
                else
                {
                    AVS5216.AVS_Done();
                    OnOpenErrorEvent();
                }
            }
            catch (Exception ex)
            {
                OnRunErrorEvent("光谱仪:" + ex.ToString());
            }
            return count;
        }

        private void RefreshStatus()
        {
            uint l_Size = 0;
            uint l_RequiredSize = 0;
            int l_NrDevices = 0;

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            l_NrDevices = AVS5216.AVS_GetNrOfDevices();


            AVS5216.AvsIdentityType[] l_Id = new AVS5216.AvsIdentityType[l_NrDevices];
            l_RequiredSize = ((uint)l_NrDevices) * (uint)Marshal.SizeOf(typeof(AVS5216.AvsIdentityType));

            //deviceSerialNr = new string[l_NrDevices];
            //deviceStatus = new string[l_NrDevices];

            if (l_RequiredSize > 0)
            {
                l_Size = l_RequiredSize;
                l_NrDevices = AVS5216.AVS_GetList(l_Size, ref l_RequiredSize, l_Id);

                for (int i = 0; i < l_NrDevices; i++)
                {
                    //deviceSerialNr[i] = l_Id[i].m_SerialNumber.ToString();
                    //deviceStatus[i] = l_Id[i].m_Status.ToString();
                    MyDevice device = new MyDevice();
                   
                        device.avsID.m_SerialNumber = l_Id[i].m_SerialNumber;
                        device.avsID.m_Status = l_Id[i].m_Status;
                    
                        listDevice.Add(device);
                        listDevice.Reverse();//反转 已测试翻转实现
                       
                    
                }
                m_DevNr = l_NrDevices;
            }

        }

        public int GetNumDevice()
        {
            return AVS5216.AVS_GetNrOfDevices();
        }

        public void UpdateList()
        {
            RefreshStatus();

            //deviceFullRange = new string[m_DevNr];
            //m_Measurements = new uint[m_DevNr];
            //m_Failures = new uint[m_DevNr];
            string connectStatusAll = "";

            if (m_DevNr > 0)
            {
                //m_DeviceHandle = new long[m_DevNr];

                for (int i = 0; i < m_DevNr; i++)
                {
                    //m_DeviceHandle[i] = AVS5216.INVALID_AVS_HANDLE_VALUE;
                    listDevice[i].avsDeviceHandle = AVS5216.INVALID_AVS_HANDLE_VALUE;
                    connectStatusAll += Activate(i);
                }
                OnStatusEvent(connectStatusAll);
                //RefreshStatus();
            }
        }

        private string Activate(int list_select)
        {
            //AVS5216.AvsIdentityType l_Active = new AVS5216.AvsIdentityType();
            //AVS5216.PixelArrayType m_Lambda = new AVS5216.PixelArrayType();
            long l_hDevice = 0;
            string connectStatus = "";
            //l_Active.m_SerialNumber = deviceSerialNr[list_select];

            if (listDevice[list_select].avsID.m_Status.ToString() == "AVAILABLE")
            {
                l_hDevice = (long)AVS5216.AVS_Activate(ref listDevice[list_select].avsID);
                if (AVS5216.INVALID_AVS_HANDLE_VALUE == l_hDevice)
                {
                    connectStatus = listDevice[list_select].avsID.m_SerialNumber + " Activate Error\n";
                }
                else
                {
                    //m_DeviceHandle[list_select] = l_hDevice;
                    listDevice[list_select].avsDeviceHandle = l_hDevice;
                    connectStatus = listDevice[list_select].avsID.m_SerialNumber + " Activated Success\n";

                    if (AVS5216.ERR_SUCCESS == (int)AVS5216.AVS_GetLambda((IntPtr)listDevice[list_select].avsDeviceHandle, ref listDevice[list_select].lambda))
                    {

                        string wavelengthmin = string.Format("{0:F2}", listDevice[list_select].lambda.Value[m_StartPixels]);
                        string wavelengthmmax = string.Format("{0:F2}", listDevice[list_select].lambda.Value[m_StopPixels]);
                        listDevice[list_select].lambdarange = wavelengthmin + "-" + wavelengthmmax;
                        m_Minlambda = listDevice[list_select].lambda.Value[m_StartPixels] < m_Minlambda ? listDevice[list_select].lambda.Value[m_StartPixels] : m_Minlambda;
                        m_Maxlambda = listDevice[list_select].lambda.Value[m_StopPixels] > m_Maxlambda ? listDevice[list_select].lambda.Value[m_StopPixels] : m_Maxlambda;
                    }

                    if (list_select == 0)
                    {
                        AVS5216.AVS_SetSyncMode((IntPtr)listDevice[list_select].avsDeviceHandle, 1);
                        //ReadMeasFromEEProm(list_select);
                    }
                    else
                    {
                        AVS5216.AVS_SetSyncMode((IntPtr)listDevice[list_select].avsDeviceHandle, 0);
                    }
                }
            }
            //else
            //{
            //    if (deviceStatus[list_select] == "UNKNOWN")
            //    {
            //        connectStatus = l_Active + " Activate Error\n";
            //    }

            //    if (deviceStatus[list_select] == "IN_USE_BY_APPLICATION" || deviceStatus[list_select] == "IN_USE_BY_OTHER")
            //    {
            //        DeactivateDevice(list_select);
            //        l_hDevice = (long)AVS5216.AVS_Activate(ref l_Active);
            //        if (AVS5216.INVALID_AVS_HANDLE_VALUE == l_hDevice)
            //        {
            //            connectStatus = l_Active.m_SerialNumber + " Activate Error\n";
            //        }
            //        else
            //        {
            //            m_DeviceHandle[list_select] = l_hDevice;

            //            if (AVS5216.ERR_SUCCESS == (int)AVS5216.AVS_GetLambda((IntPtr)m_DeviceHandle[list_select], ref m_Lambda))
            //            {
            //                string wavelengthmin = string.Format("{0:F2}", m_Lambda.Value[m_StartPixels]);
            //                string wavelengthmmax = string.Format("{0:F2}", m_Lambda.Value[m_StopPixels]);
            //                deviceFullRange[list_select] = wavelengthmin + "-" + wavelengthmmax;
            //            }


            //            connectStatus = l_Active.m_SerialNumber + " Activated Success\n";
            //            if (list_select == 0)
            //            {
            //                AVS5216.AVS_SetSyncMode((IntPtr)m_DeviceHandle[list_select], 1);
            //                //ReadMeasFromEEProm(list_select);
            //            }
            //            else
            //            {
            //                AVS5216.AVS_SetSyncMode((IntPtr)m_DeviceHandle[list_select], 0);
            //            }
            //        }
            //    }

            //}
            return connectStatus;
        }
        //-------------------------------------------------------------------------
        public void DeactivateDevice(int list_select)
        {
            AVS5216.AVS_Deactivate((IntPtr)listDevice[list_select].avsDeviceHandle);
            listDevice[list_select].avsDeviceHandle = AVS5216.INVALID_AVS_HANDLE_VALUE;

        }
        //-------------------------------------------------------------------------
        public void StartMeasurement()
        {
            ////Prepare Measurement  
            //int l_Res;
            //AVS5216.MeasConfigType[] l_PrepareMeasData = new AVS5216.MeasConfigType[m_DevNr];
            //for (int i = 0; i < m_DevNr; i++)
            //{
            //    l_PrepareMeasData[i].m_StartPixel = System.Convert.ToUInt16(0);
            //    l_PrepareMeasData[i].m_StopPixel = System.Convert.ToUInt16(2047);
            //    l_PrepareMeasData[i].m_IntegrationTime = (float)System.Convert.ToDouble(m_IntergrationTime);
            //    double l_NanoSec = -21;
            //    uint l_FPGAClkCycles = (uint)(6.0 * (l_NanoSec + 20.84) / 125.0);
            //    l_PrepareMeasData[i].m_IntegrationDelay = l_FPGAClkCycles;
            //    l_PrepareMeasData[i].m_NrAverages = System.Convert.ToUInt32(m_NrAverrages);
            //    l_PrepareMeasData[i].m_Trigger.m_Mode = (byte)m_TriggerMode;
            //    l_PrepareMeasData[i].m_Trigger.m_Source = (byte)1;
            //    l_PrepareMeasData[i].m_Trigger.m_SourceType = 0;
            //    l_PrepareMeasData[i].m_SaturationDetection = System.Convert.ToByte(1);
            //    l_PrepareMeasData[i].m_CorDynDark.m_Enable = (byte)m_CorDynDark;
            //    l_PrepareMeasData[i].m_CorDynDark.m_ForgetPercentage = System.Convert.ToByte("100");
            //    l_PrepareMeasData[i].m_Smoothing.m_SmoothPix = System.Convert.ToUInt16("6");
            //    l_PrepareMeasData[i].m_Smoothing.m_SmoothModel = System.Convert.ToByte("0");
            //    l_PrepareMeasData[i].m_Control.m_StrobeControl = 0;
            //    l_PrepareMeasData[i].m_Control.m_StoreToRam = 0;

            //    //l_FPGAClkCycles = (uint)(6.0 * l_NanoSec / 125.0);
            //    l_PrepareMeasData[i].m_Control.m_LaserDelay = 0;

            //    //l_FPGAClkCycles = (uint)(6.0 * l_NanoSec / 125.0);
            //    l_PrepareMeasData[i].m_Control.m_LaserWidth = 0;

            //    l_PrepareMeasData[i].m_Control.m_LaserWaveLength = 0;

            //    if (i == 0)
            //    {
            //        l_PrepareMeasData[i].m_Trigger.m_Source = (byte)0;
            //    }

            //    l_Res = (int)AVS5216.AVS_PrepareMeasure((IntPtr)m_DeviceHandle[i], ref l_PrepareMeasData[i]);
            //    if (AVS5216.ERR_SUCCESS != l_Res)
            //    {
            //        OnRunErrorEvent("Error " + l_Res.ToString());
            //    }

            //    //if ((l_PrepareMeasData[i].m_Control.m_StoreToRam > 0) && true)
            //    //{

            //    //    MessageBox.Show(l_PrepareMeasData[i].m_Control.m_StoreToRam.ToString() +
            //    //                    " scans will be stored to RAM. " +
            //    //                    "The number of measurements (a_Nmsr in AVS_Measure) has been set to 1",
            //    //                    "Avantes",
            //    //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    //}

            //}
            ////Start Measurement

            //m_Measurements = new uint[m_DevNr];
            //m_Failures = new uint[m_DevNr];

            //m_StartPixels = l_PrepareMeasData.m_StartPixel;
            //m_StopPixels = l_PrepareMeasData.m_StopPixel;

            //主通道最后采集
            //for (int i = 1; i < m_DevNr; i++)
            //{

            //    l_Res = (int)AVS5216.AVS_Measure((IntPtr)m_DeviceHandle[i], m_handle, meas_count);
            //    if (AVS5216.ERR_SUCCESS != l_Res)
            //    {
            //        switch (l_Res)
            //        {
            //            case AVS5216.ERR_INVALID_PARAMETER:
            //                OnRunErrorEvent("AVS5216.ERR_INVALID_PARAMETER");
            //                break;
            //            default:
            //                OnRunErrorEvent("Meas.Status: start failed, code: " + l_Res.ToString());
            //                break;
            //        }
            //    }

            //}

            //l_Res = (int)AVS5216.AVS_Measure((IntPtr)m_DeviceHandle[0], m_handle, meas_count);
            //if (AVS5216.ERR_SUCCESS != l_Res)
            //{
            //    switch (l_Res)
            //    {
            //        case AVS5216.ERR_INVALID_PARAMETER:
            //            OnRunErrorEvent("MEASURE:AVS5216.ERR_INVALID_PARAMETER");
            //            break;
            //        default:
            //            OnRunErrorEvent("Meas.Status: start failed, code: " + l_Res.ToString());
            //            break;
            //    }
            //}
            prepareDate();

            Measure();
        }

        private void prepareDate()
        {
            //Prepare Measurement  
            int l_Res;
            AVS5216.MeasConfigType[] l_PrepareMeasData = new AVS5216.MeasConfigType[m_DevNr];
            for (int i = 0; i < m_DevNr; i++)
            {
                l_PrepareMeasData[i].m_StartPixel = System.Convert.ToUInt16(0);
                l_PrepareMeasData[i].m_StopPixel = System.Convert.ToUInt16(2047);
                l_PrepareMeasData[i].m_IntegrationTime = (float)System.Convert.ToDouble(m_IntergrationTime);
                double l_NanoSec = -21;
                uint l_FPGAClkCycles = (uint)(6.0 * (l_NanoSec + 20.84) / 125.0);
                l_PrepareMeasData[i].m_IntegrationDelay = l_FPGAClkCycles;
                l_PrepareMeasData[i].m_NrAverages = 1;
                l_PrepareMeasData[i].m_Trigger.m_Mode = (byte)m_TriggerMode;
                l_PrepareMeasData[i].m_Trigger.m_Source = (byte)1;
                l_PrepareMeasData[i].m_Trigger.m_SourceType = 0;
                l_PrepareMeasData[i].m_SaturationDetection = System.Convert.ToByte(0);
                l_PrepareMeasData[i].m_CorDynDark.m_Enable = (byte)m_CorDynDark;
                l_PrepareMeasData[i].m_CorDynDark.m_ForgetPercentage = System.Convert.ToByte("100");
                l_PrepareMeasData[i].m_Smoothing.m_SmoothPix = System.Convert.ToUInt16("0");
                l_PrepareMeasData[i].m_Smoothing.m_SmoothModel = System.Convert.ToByte("0");
                l_PrepareMeasData[i].m_Control.m_StrobeControl = 0;
                l_PrepareMeasData[i].m_Control.m_StoreToRam = 0;

                //l_FPGAClkCycles = (uint)(6.0 * l_NanoSec / 125.0);
                l_PrepareMeasData[i].m_Control.m_LaserDelay = 0;

                //l_FPGAClkCycles = (uint)(6.0 * l_NanoSec / 125.0);
                l_PrepareMeasData[i].m_Control.m_LaserWidth = 0;

                l_PrepareMeasData[i].m_Control.m_LaserWaveLength = 0;

                if (i == 0)
                {
                    l_PrepareMeasData[i].m_Trigger.m_Source = (byte)0;
                }

                AVS5216.AVS_UseHighResAdc((IntPtr)listDevice[i].avsDeviceHandle, true);

                l_Res = (int)AVS5216.AVS_PrepareMeasure((IntPtr)listDevice[i].avsDeviceHandle, ref l_PrepareMeasData[i]);
                if (AVS5216.ERR_SUCCESS != l_Res)
                {
                    OnRunErrorEvent("Error " + l_Res.ToString());
                }

                //if ((l_PrepareMeasData[i].m_Control.m_StoreToRam > 0) && true)
                //{

                //    MessageBox.Show(l_PrepareMeasData[i].m_Control.m_StoreToRam.ToString() +
                //                    " scans will be stored to RAM. " +
                //                    "The number of measurements (a_Nmsr in AVS_Measure) has been set to 1",
                //                    "Avantes",
                //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}

            }
            //Start Measurement

            //m_Measurements = new uint[m_DevNr];
            //m_Failures = new uint[m_DevNr];

        }

        public void Measure()
        {
            int l_Res;

            for (int i = 1; i < m_DevNr; i++)
            {

                l_Res = (int)AVS5216.AVS_Measure((IntPtr)listDevice[i].avsDeviceHandle, m_handle, 1);
                if (AVS5216.ERR_SUCCESS != l_Res)
                {
                    switch (l_Res)
                    {
                        case AVS5216.ERR_INVALID_PARAMETER:
                            OnRunErrorEvent("AVS5216.ERR_INVALID_PARAMETER");
                            break;
                        default:
                            OnRunErrorEvent("Meas.Status: start failed, code: " + l_Res.ToString());
                            break;
                    }
                }

            }

            l_Res = (int)AVS5216.AVS_Measure((IntPtr)listDevice[0].avsDeviceHandle, m_handle, 1);
            if (AVS5216.ERR_SUCCESS != l_Res)
            {
                switch (l_Res)
                {
                    case AVS5216.ERR_INVALID_PARAMETER:
                        OnRunErrorEvent("MEASURE:AVS5216.ERR_INVALID_PARAMETER");
                        break;
                    default:
                        OnRunErrorEvent("Meas.Status: start failed, code: " + l_Res.ToString());
                        break;
                }
            }
        }

        public void StopMeasurement()
        {
            for (int i = 0; i < m_DevNr; i++)
            {
                int l_Res = (int)AVS5216.AVS_StopMeasure((IntPtr)listDevice[i].avsDeviceHandle);

                if (AVS5216.ERR_SUCCESS != l_Res)
                {
                    OnRunErrorEvent("Error " + l_Res.ToString());
                }
                //OnAvantesFinished();
            }

        }

        public void DisConnect()
        {
            for (int i = 0; i < m_DevNr; i++)
            {
                if (listDevice[i].avsDeviceHandle != AVS5216.INVALID_AVS_HANDLE_VALUE)
                {
                    AVS5216.AVS_Deactivate((IntPtr)listDevice[i].avsDeviceHandle);
                }
            }
            AVS5216.AVS_Done();
        }


        /*
               private void ReadMeasFromEEProm(int list_select)
               {
                   AVS5216.DeviceConfigType l_pDeviceData = new AVS5216.DeviceConfigType();
                   uint l_Size;

                   l_Size = (uint)Marshal.SizeOf(typeof(AVS5216.DeviceConfigType));
                   int l_Res = (int)AVS5216.AVS_GetParameter((IntPtr)m_DeviceHandle[0], l_Size, ref l_Size, ref l_pDeviceData);

                   if (l_Res == AVS5216.ERR_INVALID_SIZE)
                   {
                       l_Res = (int)AVS5216.AVS_GetParameter((IntPtr)m_DeviceHandle[0], l_Size, ref l_Size, ref l_pDeviceData);
                   }

                   if (AVS5216.ERR_SUCCESS != l_Res)
                   {
                       MessageBox.Show("Error ", "Avantes",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                       return;
                   }

                   //show measurement settings
                   //edtStartPixel.Text = l_pDeviceData.m_StandAlone.m_Meas.m_StartPixel.ToString();
                   //edtStopPixel.Text = l_pDeviceData.m_StandAlone.m_Meas.m_StopPixel.ToString();
                   edtIntegrationTime.Text = string.Format("{0:F}", l_pDeviceData.m_StandAlone.m_Meas.m_IntegrationTime);
                   uint l_FPGAClkCycles = l_pDeviceData.m_StandAlone.m_Meas.m_IntegrationDelay;
                   double l_NanoSec = 125.0 * (l_FPGAClkCycles - 1.0) / 6.0;
                   //edtIntegrationDelay.Text = string.Format("{0:F}", l_NanoSec);
                   edtNrOfAverages.Text = l_pDeviceData.m_StandAlone.m_Meas.m_NrAverages.ToString();
                   //edtSaturationLevel.Text = l_pDeviceData.m_StandAlone.m_Meas.m_SaturationDetection.ToString();
                   chkTrigModeHw.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_Trigger.m_Mode == 1);
                   chkTrigModeSw.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_Trigger.m_Mode == 0);
                   chkTrigSourceExtHw.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_Trigger.m_Source == 0);
                   chkTrigSourceSync.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_Trigger.m_Source == 1);
                   chkTrigTypeEdge.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_Trigger.m_SourceType == 0);
                   chkTrigTypeLevel.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_Trigger.m_SourceType == 1);
                   chkEnableDarkCorrection.Checked = (l_pDeviceData.m_StandAlone.m_Meas.m_CorDynDark.m_Enable == 1);
                   //edtDarkHist.Text = l_pDeviceData.m_StandAlone.m_Meas.m_CorDynDark.m_ForgetPercentage.ToString();
                   //edtSmoothModel.Text = l_pDeviceData.m_StandAlone.m_Meas.m_Smoothing.m_SmoothModel.ToString();
                   //edtSmoothPix.Text = l_pDeviceData.m_StandAlone.m_Meas.m_Smoothing.m_SmoothPix.ToString();

                   l_FPGAClkCycles = l_pDeviceData.m_StandAlone.m_Meas.m_Control.m_LaserDelay;
                   l_NanoSec = 125.0 * (l_FPGAClkCycles) / 6.0;

                   l_FPGAClkCycles = l_pDeviceData.m_StandAlone.m_Meas.m_Control.m_LaserWidth;
                   l_NanoSec = 125.0 * (l_FPGAClkCycles) / 6.0;
               }
       */

        #endregion
    }
}


