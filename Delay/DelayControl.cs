using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace LIBS.Delay
{
    public class DelayControl
    {
        #region 事件
        #region 事件: 开启/关闭
        public delegate void DelayOpenMessageHandler();
        public event DelayOpenMessageHandler DelayOpen;
        public void OnDelayOpen()
        {
            if (this.DelayOpen != null)
            {
                this.DelayOpen();
            }
        }

        public delegate void DelayCloseMessageHandler(string msg);
        public event DelayCloseMessageHandler DelayClose;
        public void OnDelayClose(string msg)
        {
            if (this.DelayClose != null)
            {
                this.DelayClose(msg);
            }
        }
        #endregion

        #region 事件: 出错
        public delegate void DelayErrorMessageHandler(string msg);
        public event DelayErrorMessageHandler DelayError;
        public void OnDelayError(string msg)
        {
            if (this.DelayError != null)
            {
                this.DelayError(msg);
            }
        }
        #endregion

        #region 已获取当前状态
        public delegate void DelayParaMessageHandler();
        public event DelayParaMessageHandler DelayPara;
        public void OnDelayPara()
        {
            if (this.DelayPara != null)
            {
                this.DelayPara();
            }
        }
        #endregion
        #endregion

        #region 字段/属性
        private SerialPort DelayCom = new SerialPort();
        private bool ClosingSerial = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke  
        private string buff="";
        private bool connected=false;

        private string delayTimeA;
        public string DelayTimeA
        {
            get { return delayTimeA; }
            set { delayTimeA = value; }
        }

        private string delayTimeB;
        public string DelayTimeB
        {
            get { return delayTimeB; }
            set { delayTimeB = value; }
        }

        private char polarA;
        public char PolarA
        {
            get { return polarA; }
            set { polarA = value; }
        }

        private char polarB;
        public char PolarB
        {
            get { return polarB; }
            set { polarB = value; }
        }

        private char polarE;
        public char PolarE
        {
            get { return polarE; }
            set { polarE = value; }
        }

        private char polarT;
        public char PolarT
        {
            get { return polarT; }
            set { polarT = value; }
        }

        #endregion
    
        #region 方法

        public void Open(string DelayPort)
        {
            try
            {

                DelayCom.PortName = DelayPort;
                DelayCom.BaudRate = 115200;
                DelayCom.Parity = Parity.None;
                DelayCom.StopBits = StopBits.One;
                DelayCom.DataBits = 8;


                //初始化SerialPort对象  
                DelayCom.RtsEnable = true;//根据实际情况吧。  
                //添加事件注册  
                DelayCom.DataReceived += CommDataReceived;               
                DelayCom.Open();

                GetAllPara();
            }

            catch (Exception ex)
            {
                //捕获到异常信息，创建一个新的comm对象，之前的不能用了。  
                DelayCom = new SerialPort();
                //现实异常信息给客户。
                OnDelayError("延时器："+ex.Message);
            }
        }
        
        private void DelayCmdWrite(string cmd)
        {
            try
            {
                DelayCom.Write(cmd);
            }
            catch (Exception ex)
            {
                OnDelayError("延时器："+ex.ToString());
            }
        }

        public void SetDelayPolar(char channel,char polar)
        {
            string cmd = "~p" + channel + polar + "#";
            DelayCmdWrite(cmd);        
        }

        public void SetDelayTime(char channel, string delaytime)
        {
            string cmd = "~s" + channel+delaytime + "#";
            DelayCmdWrite(cmd);               
        }

        public void GetAllPara()
        {
            DelayCmdWrite("~i#"); 
        }

        private void getTriggerTimes()
        {
            DelayCmdWrite("~t#");             
        }

        private void getPulseWidth()
        {
            DelayCmdWrite("~r#");
        }

        private void setPulseWidth(string pulsewidth)
        {
            DelayCmdWrite("~l"+pulsewidth+"#");
        }

        public void Close()
        {
            ClosingSerial = true;
            DelayCom.Close();
            ClosingSerial = false;
            OnDelayClose("DelayComClose");
        }

        private void CommDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (ClosingSerial) return;//如果正在关闭，忽略操作，直接返回，尽快的完成串口监听线程的一次循环  
                buff = "";
                buff = DelayCom.ReadTo("#");
                DataClassify(buff);
            }
            catch(Exception ex)
            {

            }
        }
     
        private void DataClassify(string str)
        {
            if (str.StartsWith("~")|| str.StartsWith("YanShiQi start!"))
            {
                if (connected == false)
                {
                    connected = true;
                    OnDelayOpen();
                }              
            }

            if (str.StartsWith("~I") || str.StartsWith("YanShiQi start!"))
            {
                string[] strSplit = str.Split('|');

                delayTimeA = "";
                char[] chararrA = strSplit[0].ToCharArray();
                for (int i =chararrA.Length-11; i < chararrA.Length; i++)
                {
                    delayTimeA += chararrA[i];
                }

                delayTimeB = "";
                char[] chararrB=strSplit[1].ToCharArray();
                for (int i = 0; i < 11; i++)
                {
                    delayTimeB += chararrB[i];
                }
                polarE = chararrB[11];
                polarT = chararrB[12];
                polarA = chararrB[13];
                polarB = chararrB[14];
                OnDelayPara();
            }
        }



        #endregion


    }
}
