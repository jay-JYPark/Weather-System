//#define TotalVer
//#define debug

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using WeatherRSystem.Properties;
using ADEng.Control.MountainValley;
using ADEng.Library.MountainValley;
using ADEng.Control.WeatherSystem;
using ADEng.Library.WeatherSystem;
using Adeng.Framework.Log;
using Adeng.Framework.Net;
using Adeng.Framework.Util;

namespace ADEng.Module.WeatherSystem
{
    public partial class MainForm : Form
    {
        #region delegate
        private delegate void invokeSetWord(string _str);
        private delegate void invokeSetButton(byte _num, bool _stat);
        private delegate void InvokeSplshclose();
        private delegate void InvokerSplashClose();
        #endregion

        #region instance
        private WeatherForm weatherForm = null;
        private WeatherCtrForm weatherCtrForm = null;
        private SMSMainForm weatherSmsForm = null;
        private WeatherOptionForm weatherOptionForm = null;
        private WeatherTcpServer server = null;
        private AdengSvrSocket ethernetServer = null;
        private ToolStripStatusLabel serverToolstripLB = null;
        private ToolStripStatusLabel serverToolstrip_E_LB = null;
        private WeatherTcpClient client_1 = null;
        private WeatherTcpClient client_2 = null;
        private WeatherSerial serial_1 = null;
        private WeatherSerial serial_2 = null;
        private AlarmWindow alarmWindow = null;
        private WDeviceAlarmWindow deviceAlarm = null;
        private AlarmStrt aStruct;
        private DeviceMng deviceMng = null;
        private WeatherDataMng dataMng = null;
        private WeatherSplash splashDlg = null;
        private WeatherLogin weatherLogin = null;
        private ParsingWeatherDiviceProtocol parsing = null;
        private CDMAComm cdmaComm = null;
        private WeatherAboutForm aboutForm = null;
        private RecordsForm weatherRecord = null;
#if TotalVer
        private StatsForm weatherState = null;
#endif
        private WeatherSelfForm weatherSelfForm = null;
        private List<StateChecker> CheckerList = new List<StateChecker>();
        private List<EthernetChecker> EthernetCheckerList = new List<EthernetChecker>();
        private DeviceAlarmStrt daStruct;
        #endregion

        #region var
	//2013년 7월 22일 버전으로 하나 추가해야 함. 툴이 없어서 현재 주석으로 처리하고 있음. 수정 내용은 수위 임계치 데이터 단위를 m -> cm로 변경했음. ver 2.2로 하면 됨.
        private const string wRSName = "기상 연계 서버시스템";  //프로그램 정보에서 쓰이는 프로그램 이름
        private const string wRSVer_0 = "Ver 2.1 <2011/09/17>"; //현재 가장 최신 버전, 우량기로 IP자동 전송 기능 추가, 수위계 단위 변경(m -> cm)
        private const string wRSVer_1 = "Ver 2.0 <2011/05/30>"; //구 버전
        private const string wRSVer_2 = "Ver 1.2 <2011/01/07>"; //구 버전
        private const string wRSVer_3 = "Ver 1.1 <2010/09/13>"; //구 버전
        private const string wRSVer_4 = "Ver 1.0 <2010/08/30>"; //구 버전
        private int WDeviceChkTimeInterval = 30;                //측기 통신 체크간격
        private double EthernetPollingInterval = 0.15;          //이더넷 통신 폴링 체크간격
        private List<string> wRSVerList = new List<string>();   //지금까지의 버전 관리 리스트
        private Thread AlarmPopTD;                              //팝업 창이 On인 경우의 프로세스를 멈추지 않기 위한 쓰레드
        private Thread deviceAlarmTD;                           //알람 항목을 위한 쓰레드
        private bool flagLogin = true;
        private Thread splashThread = null;
        private string userId = string.Empty;
        private string userPass = string.Empty;
        private bool dbConState = true;
        #endregion

        #region enum's
        //tcp 클라이언트 enum's
        private enum CType
        {
            client_1 = 1,
            client_2 = 2
        }

        //serial 클라이언트 enum's
        private enum SType
        {
            client_1 = 1,
            client_2 = 2
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        #region 생성자
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //재실행 방지 코드
            bool executeProc;
            executeProc = Adeng.Framework.Util.AdengUtil.CheckAppOverlap(Application.ProductName);

            if (executeProc)
            {
                MessageBox.Show("기상 연계 서버시스템이 이미 실행중입니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose(true);
                return;
            }

            this.Text = string.Format("{0} {1}", this.Text, wRSVer_0);
            this.dataMng = WeatherDataMng.getInstance();

            while (flagLogin)
            {
                if (this.splashDlg == null)
                {
                    this.splashDlg = new WeatherSplash(this.Text);
                    this.splashDlg.BackgroundImage = Resources.Weather_Loading;
                }
                else
                {
                    if (this.splashDlg.InvokeRequired)
                    {
                        this.Invoke(new InvokeSplshclose(this.closesplash));
                    }
                    else
                    {
                        this.closesplash();
                    }
                }

                // 로그인 데이터 로드
                if (this.LoadEvnData())
                {
                    this.dataMng.Dispose();
                    this.Dispose(true);
                    return;
                }

                if (!this.dataMng.DataBaseConnect(Settings.Default.DbIp, Settings.Default.DbPort, Settings.Default.DbId, Settings.Default.DbPw, Settings.Default.DbSid))
                {
                    Settings.Default.AutoLogin = false;
                    Settings.Default.Save();
                    MessageBox.Show("DB에 연결하지 못했습니다.\n환경 설정의 DB 항목을 확인해 주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.flagLogin = false;
                    this.dbConState = false;
                }
                else
                {
                    this.dbConState = true;

                    if (!this.dataMng.SetInitData())
                    {
                        Settings.Default.AutoLogin = false;
                        Settings.Default.Save();
                        MessageBox.Show("DB에 연결하지 못했습니다.\n환경 설정의 DB 항목을 확인해 주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.flagLogin = false;
                        this.dbConState = false;
                    }
                }

                if (!this.Userconfirm() && this.dbConState)
                {
                    Settings.Default.AutoLogin = false;
                    Settings.Default.Save();
                    MessageBox.Show("사용자 정보가 일치하지 않습니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.dataMng.SetRemoveData();
                    continue;
                }

                this.flagLogin = false;
            }

            this.dataMng.onSendSmsMsgEvt += new EventHandler<SendSmsMsgEventArgs>(dataMng_onSendSmsMsgEvt);
            this.dataMng.onWOURequestEvt += new EventHandler<WOUWDeviceRequestEventArgs>(dataMng_onWOURequestEvt);
            this.dataMng.onWOUAlarmControlEvt += new EventHandler<WOUWDeviceControlAlarmEventArgs>(dataMng_onWOUAlarmControlEvt);
            this.dataMng.onWOUFTimeControlEvt += new EventHandler<WOUWDeviceControlFTimeEventArgs>(dataMng_onWOUFTimeControlEvt);
            this.dataMng.onSendSmsUserMsgEvt += new EventHandler<SendSmsMsgEventArgs>(dataMng_onSendSmsUserMsgEvt);
            this.dataMng.onAddRainDataEvt += new EventHandler<AddRainDataEventArgs>(dataMng_onAddRainDataEvt);
            this.dataMng.onAddWaterLevelDataEvt += new EventHandler<AddWaterLevelEventArgs>(dataMng_onAddWaterLevelDataEvt);
            this.dataMng.onAddFlowSpeedDataEvt += new EventHandler<AddFlowSpeedEventArgs>(dataMng_onAddFlowSpeedDataEvt);
            this.dataMng.onAddWDeviceEvt += new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt += new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
            this.dataMng.onEthernetClientSendEvt += new EventHandler<EthernetClientSendEventArgs>(dataMng_onEthernetClientSendEvt);
            this.weatherForm = new WeatherForm();
            this.weatherCtrForm = new WeatherCtrForm();
            this.weatherSmsForm = new SMSMainForm();
            this.server = new WeatherTcpServer();
            this.ethernetServer = new AdengSvrSocket();
            this.serverToolstripLB = new ToolStripStatusLabel();
            this.serverToolstrip_E_LB = new ToolStripStatusLabel();
            this.client_1 = new WeatherTcpClient();
            this.client_2 = new WeatherTcpClient();
            this.client_1.onConnectResultEvt += new EventHandler<ConnectResult>(client_1_onConnectResultEvt);
            this.client_1.onBroadcastRequestEvt += new EventHandler<BroadcastRequest>(client_1_onBroadcastRequestEvt);
            this.client_2.onConnectResultEvt += new EventHandler<ConnectResult>(client_2_onConnectResultEvt);
            this.client_2.onBroadcastRequestEvt += new EventHandler<BroadcastRequest>(client_2_onBroadcastRequestEvt);
            this.aStruct = new AlarmStrt();
            this.parsing = new ParsingWeatherDiviceProtocol();
            this.parsing.onWOU_RcvDataEvt += new EventHandler<WOU_RcvData>(parsing_onWOU_RcvDataEvt);
            this.parsing.onRainMachineRcvEvt += new EventHandler<RainMachineRcv>(parsing_onRainMachineRcvEvt);
            this.weatherRecord = new RecordsForm();
#if TotalVer
            this.weatherState = new StatsForm();
#endif

            if (Settings.Default.CdmaPort == string.Empty)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 모듈의 Serial 통신 연결을 실패했습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("CDMA 모듈의 Serial 통신 연결을 실패했습니다."));
                }

                this.cdmaComm = new CDMAComm("COM", 115200, Parity.None, 8, StopBits.One);
            }
            else
            {
                this.cdmaComm = new CDMAComm(Settings.Default.CdmaPort, 115200, Parity.None, 8, StopBits.One);
            }

            this.cdmaComm.onSerialConnectEvt += new EventHandler<SerialConnect>(cdmaComm_onSerialConnectEvt);
            this.cdmaComm.onSerialDisConnectEvt += new EventHandler<SerialDisConnect>(cdmaComm_onSerialDisConnectEvt);
            this.cdmaComm.onMyNumberRcvEvt += new EventHandler<MyNumberRcvEvtArgs>(cdmaComm_onMyNumberRcvEvt);
            this.cdmaComm.onCDMA_SendResultEvt += new EventHandler<CDMA_SendResultEvtArgs>(cdmaComm_onCDMA_SendResultEvt);
            this.cdmaComm.SerialCon();

            this.weatherForm.MdiParent = this;
            this.weatherCtrForm.MdiParent = this;
            this.weatherSmsForm.MdiParent = this;
            this.weatherRecord.MdiParent = this;
#if TotalVer
            this.weatherState.MdiParent = this;
#endif

            this.weatherForm.Dock = DockStyle.Fill;
            this.weatherCtrForm.Dock = DockStyle.Fill;
            this.weatherSmsForm.Dock = DockStyle.Fill;
            this.weatherRecord.Dock = DockStyle.Fill;
#if TotalVer
            this.weatherState.Dock = DockStyle.Fill;
#endif

            this.init();

            if (this.splashDlg != null)
            {
                this.CloseSplash();
            }

            //버전 관리(현재 2차 버전)
            this.wRSVerList.Add(wRSVer_0);
            this.wRSVerList.Add(wRSVer_1);
            this.wRSVerList.Add(wRSVer_2);
            this.wRSVerList.Add(wRSVer_3);
            this.wRSVerList.Add(wRSVer_4);

            //CDMA TCP 서버 시작
            ServerStartEventArgs tmpServerStartEventArgs = new ServerStartEventArgs(Settings.Default.ServerIp.ToString(), int.Parse(Settings.Default.ServerPort));
            this.weatherOptionForm_ServerConEvt(this, tmpServerStartEventArgs);

            //이더넷 TCP 서버 시작
            ServerStartEventArgs tmpEthernetServerStartEventArgs = new ServerStartEventArgs(Settings.Default.EthernetIp.ToString(), int.Parse(Settings.Default.EthernetPort));
            this.weatherOptionForm_OnEthernetServerConEvt(this, tmpEthernetServerStartEventArgs);

            //TCP 클라이언트 시작
            if (Settings.Default.Tcp1IsUse)
            {
                TcpClientConEventArgs tmpTcpClientConEventArgs = new TcpClientConEventArgs(
                    (byte)CType.client_1, Settings.Default.Tcp1Ip, int.Parse(Settings.Default.Tcp1Port));
                this.weatherOptionForm_ClientConEvt(this, tmpTcpClientConEventArgs);
            }

            if (Settings.Default.Tcp2IsUse)
            {
                TcpClientConEventArgs tmpTcpClientConEventArgs = new TcpClientConEventArgs(
                    (byte)CType.client_2, Settings.Default.Tcp2Ip, int.Parse(Settings.Default.Tcp2Port));
                this.weatherOptionForm_ClientConEvt(this, tmpTcpClientConEventArgs);
            }

            //시리얼 통신 시작
            if (Settings.Default.Serial1IsUse)
            {
                SerialConEventArgs tmpSerialConEventArgs = new SerialConEventArgs(
                    (byte)SType.client_1,
                    Settings.Default.S1Com,
                    Settings.Default.S1Rate,
                    Settings.Default.S1DataBit,
                    Settings.Default.S1Parity,
                    Settings.Default.S1StopBit);
                this.weatherOptionForm_SerialConEvt(this, tmpSerialConEventArgs);
            }

            if (Settings.Default.Serial2IsUse)
            {
                SerialConEventArgs tmpSerialConEventArgs = new SerialConEventArgs(
                    (byte)SType.client_2,
                    Settings.Default.S2Com,
                    Settings.Default.S2Rate,
                    Settings.Default.S2DataBit,
                    Settings.Default.S2Parity,
                    Settings.Default.S2StopBit);
                this.weatherOptionForm_SerialConEvt(this, tmpSerialConEventArgs);
            }

            //측기 통신 체크
            this.WDeviceChkTimeInterval = int.Parse(Settings.Default.WDeviceChkTime.ToString());
            this.GetWDeviceState();
            EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.Warning, wRSName + " 시작");

            //우량기로 IP 전송
            string localIp = AdengUtil.GetIpV4();

            if (Settings.Default.LocalIp != localIp)
            {
                for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
                {
                    WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(this.dataMng.DeviceList[i].TypeDevice);

                    if (wTypeTmp.Name == "RAT")
                    {
                        //DB 저장
                        WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.IP;
                        WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                        WDeviceRequest tmp = new WDeviceRequest(0, this.dataMng.DeviceList[i].PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, localIp);
                        this.dataMng.AddDeviceRequest(tmp);
                        Thread.Sleep(20);

                        wiTypeAll = WeatherDataMng.WIType.PORT;
                        AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                        tmp = new WDeviceRequest(0, this.dataMng.DeviceList[i].PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, "9001");
                        this.dataMng.AddDeviceRequest(tmp);
                        Thread.Sleep(20);

                        CProto03 cProto03 = CProtoMng.GetProtoObj("03") as CProto03;
                        cProto03.Header = "[";
                        cProto03.Length = "040";
                        cProto03.ID = this.dataMng.DeviceList[i].ID;
                        cProto03.MainCode = "1";
                        cProto03.SubCode = "g";
                        cProto03.RecvType = "1";
                        string[] tmpIpStr = localIp.Split('.');
                        cProto03.Data = string.Format("{0}.{1}.{2}.{3}9001",
                            tmpIpStr[0].PadLeft(3, '0'),
                            tmpIpStr[1].PadLeft(3, '0'),
                            tmpIpStr[2].PadLeft(3, '0'),
                            tmpIpStr[3].PadLeft(3, '0'));
                        cProto03.CRC = "00000";
                        cProto03.Tail = "]";

                        byte[] buff = cProto03.MakeProto();
                        this.dataMng.SendSmsMsg(this.dataMng.DeviceList[i].CellNumber, buff);
                    }
                }

                Settings.Default.LocalIp = localIp;
                Settings.Default.Save();
            }
        }
        #endregion

        /// <summary>
        /// 이더넷 클라이언트 데이터 전송 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onEthernetClientSendEvt(object sender, EthernetClientSendEventArgs e)
        {
            EthernetClient tmp = this.dataMng.getEthernetClient(e.ID);

            if (tmp.Client == null || !tmp.Client.Connected)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} 는 이더넷이 정상적이지 않아 CDMA로 전송합니다.", e.ID) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("{0} 는 이더넷이 정상적이지 않아 CDMA로 전송합니다.", e.ID));
                }

                WDevice tmpDevice = this.dataMng.GetRatDevice(e.ID);
                this.dataMng.SendSmsMsg(tmpDevice.CellNumber, e.Buff);
            }
            else
            {
                tmp.Client.Send(e.Buff, e.Buff.Length);
                
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} 로 이더넷 데이터를 전송하였습니다.", e.ID) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("{0} 로 이더넷 데이터를 전송하였습니다.", e.ID));
                }
            }
        }

        //측기 삭제 이벤트
        private void dataMng_onDeleteWDeviceEvt(object sender, DeleteWDeviceEventArgs e)
        {
            List<StateChecker> tmpChecker = new List<StateChecker>();

            for (int i = 0; i < e.WDList.Count; i++)
            {
                for (int k = 0; k < this.CheckerList.Count; k++)
                {
                    if (e.WDList[i].PKID == this.CheckerList[k].WDevicePKID)
                    {
                        this.CheckerList[k].Flag = false;
                        tmpChecker.Add(this.CheckerList[k]);
                    }
                }
            }

            for (int i = 0; i < tmpChecker.Count; i++)
            {
                this.CheckerList.Remove(this.getCheckerComparer(tmpChecker[i]));
            }
        }

        /// <summary>
        /// 인자로 받은 StateChecker를 리스트에서 검색해 반환한다.
        /// </summary>
        /// <param name="_sc"></param>
        /// <returns></returns>
        private StateChecker getCheckerComparer(StateChecker _sc)
        {
            try
            {
                StateChecker rst = new StateChecker(byte.MinValue, byte.MinValue);

                for (int i = 0; i < this.CheckerList.Count; i++)
                {
                    if (this.CheckerList[i].WDevicePKID == _sc.WDevicePKID)
                    {
                        rst = this.CheckerList[i];
                        break;
                    }
                }

                return rst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.getCheckerComparer(StateChecker _sc) - {0}", ex.Message));
                return new StateChecker(byte.MinValue, byte.MinValue);
            }
        }

        //측기 등록 이벤트
        private void dataMng_onAddWDeviceEvt(object sender, AddWDeviceEventArgs e)
        {
            StateChecker wDeviceChecker = new StateChecker(this.WDeviceChkTimeInterval * 3, e.WD.PKID);
            wDeviceChecker.onWDeviceStateEvt += new EventHandler<WDeviceStateEventArgs>(wDeviceChecker_onWDeviceStateEvt);
            wDeviceChecker.Start();
            this.CheckerList.Add(wDeviceChecker);
        }

        //유속 데이터 들어오는 이벤트
        private void dataMng_onAddFlowSpeedDataEvt(object sender, AddFlowSpeedEventArgs e)
        {
            for (int i = 0; i < this.CheckerList.Count; i++)
            {
                if (this.CheckerList[i].WDevicePKID == e.WFS.FKDevice)
                {
                    this.CheckerList[i].CheckTime = DateTime.Now;
                    this.CheckerList[i].IsFirst = true;
                }
            }
        }

        //수위 데이터 들어오는 이벤트
        private void dataMng_onAddWaterLevelDataEvt(object sender, AddWaterLevelEventArgs e)
        {
            for (int i = 0; i < this.CheckerList.Count; i++)
            {
                if (this.CheckerList[i].WDevicePKID == e.WWL.FKDevice)
                {
                    this.CheckerList[i].CheckTime = DateTime.Now;
                    this.CheckerList[i].IsFirst = true;
                }
            }
        }

        //강수 데이터 들어오는 이벤트
        private void dataMng_onAddRainDataEvt(object sender, AddRainDataEventArgs e)
        {
            for (int i = 0; i < this.CheckerList.Count; i++)
            {
                if (this.CheckerList[i].WDevicePKID == e.WR.FKDevice)
                {
                    this.CheckerList[i].CheckTime = DateTime.Now;
                    this.CheckerList[i].IsFirst = true;
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("프로그램을 종료하시겠습니까?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.Warning, wRSName + " 종료");

                try
                {
                    this.dataMng.Dispose();

                    if (this.server.bServerListen)
                    {
                        this.server.NetworkListenEnd();
                    }

                    if (this.ethernetServer.IsRunning)
                    {
                        this.ethernetServer.CloseAllClient();
                        this.ethernetServer.Stop();
                    }

                    if (this.client_1.TcpConn)
                    {
                        this.client_1.TcpClose();
                    }

                    if (this.client_2.TcpConn)
                    {
                        this.client_2.TcpClose();
                    }

                    if (this.serial_1 != null)
                    {
                        if (this.serial_1.IsOpenState())
                        {
                            this.serial_1.Close();
                        }
                    }

                    if (this.serial_2 != null)
                    {
                        if (this.serial_2.IsOpenState())
                        {
                            this.serial_2.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("MainForm.OnClosing - {0}", ex.Message));
                }
            }
            else
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// 각종 설정값 및 UI 초기화 메소드
        /// </summary>
        private void init()
        {
            this.weatherForm.Show();
            this.WeatherToolStripBtn.Checked = true;
            this.우량기상태WToolStripMenuItem.Checked = true;

            //서버상태 아이콘 셋팅
            this.serverToolstripLB.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            this.serverToolstripLB.Image = Resources.DMB_systemRed;
            this.serverToolstripLB.ImageAlign = ContentAlignment.MiddleRight;
            this.serverToolstripLB.Spring = true;
            this.serverToolstripLB.Text = "TCP서버 (CDMA)";
            this.serverToolstripLB.Name = "Server";
            this.serverToolstripLB.TextAlign = ContentAlignment.MiddleRight;
            this.serverToolstripLB.TextImageRelation = TextImageRelation.TextBeforeImage;
            this.serverToolstripLB.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            this.MainStatus.Items.Add(this.serverToolstripLB);

            this.serverToolstrip_E_LB.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            this.serverToolstrip_E_LB.Image = Resources.DMB_systemRed;
            this.serverToolstrip_E_LB.ImageAlign = ContentAlignment.MiddleRight;
            this.serverToolstrip_E_LB.Text = "          TCP서버 (이더넷)";
            this.serverToolstrip_E_LB.Name = "Server_e";
            this.serverToolstrip_E_LB.TextAlign = ContentAlignment.MiddleRight;
            this.serverToolstrip_E_LB.TextImageRelation = TextImageRelation.TextBeforeImage;
            this.serverToolstrip_E_LB.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            this.MainStatus.Items.Add(this.serverToolstrip_E_LB);
        }

        #region UI Click Event
        //Tcp 클라이언트 2번에 연결된 경보대에서 발생시키는 발령 클래스 이벤트
        private void client_2_onBroadcastRequestEvt(object sender, BroadcastRequest e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP Client_2 발령 데이터가 수신되었습니다.") });
            }
            else
            {
                this.SetRWeatherData(string.Format("TCP Client_2 발령 데이터가 수신되었습니다."));
            }

            Proto900 p900 = ProtoMng.GetProtoObj("900") as Proto900;
            byte[] buff = p900.MakeOrderProto(e);
            this.SendFromWRS(buff);
        }

        //Tcp 클라이언트 1번에 연결된 경보대에서 발생시키는 발령 클래스 이벤트
        private void client_1_onBroadcastRequestEvt(object sender, BroadcastRequest e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP Client_1 발령 데이터가 수신되었습니다.") });
            }
            else
            {
                this.SetRWeatherData(string.Format("TCP Client_1 발령 데이터가 수신되었습니다."));
            }

            Proto900 p900 = ProtoMng.GetProtoObj("900") as Proto900;
            byte[] buff = p900.MakeOrderProto(e);
            this.SendFromWRS(buff);
        }

        //측기 상태 버튼 클릭
        private void WeatherToolStripBtn_Click(object sender, EventArgs e)
        {
            this.weatherForm.Show();
            this.weatherCtrForm.Hide();
            this.weatherSmsForm.Hide();
            this.weatherRecord.Hide();
#if TotalVer
            this.weatherState.Hide();
#endif
            
            this.WeatherToolStripBtn.Checked = true;
            this.WeatherCtrToolStripBtn.Checked = false;
            this.WeatherSmsToolStripBtn.Checked = false;
            this.WeatherRecordToolStripBtn.Checked = false;
            this.WeatherStateToolStripBtn.Checked = false;

            this.우량기상태WToolStripMenuItem.Checked = true;
            this.우량기제어RToolStripMenuItem.Checked = false;
            this.sMS그룹관리SToolStripMenuItem.Checked = false;
            this.이력조회DToolStripMenuItem.Checked = false;
            this.정보통계ToolStripMenuItem.Checked = false;
        }

        //측기 제어 버튼 클릭
        private void WeatherCtrToolStripBtn_Click(object sender, EventArgs e)
        {
            this.weatherCtrForm.Show();
            this.weatherForm.Hide();
            this.weatherSmsForm.Hide();
            this.weatherRecord.Hide();
#if TotalVer
            this.weatherState.Hide();
#endif

            this.WeatherCtrToolStripBtn.Checked = true;
            this.WeatherToolStripBtn.Checked = false;
            this.WeatherSmsToolStripBtn.Checked = false;
            this.WeatherRecordToolStripBtn.Checked = false;
            this.WeatherStateToolStripBtn.Checked = false;

            this.우량기제어RToolStripMenuItem.Checked = true;
            this.우량기상태WToolStripMenuItem.Checked = false;
            this.sMS그룹관리SToolStripMenuItem.Checked = false;
            this.이력조회DToolStripMenuItem.Checked = false;
            this.정보통계ToolStripMenuItem.Checked = false;
        }

        //메뉴의 SMS 그룹 관리 클릭
        private void sMS그룹관리SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.weatherSmsForm.Show();
            this.weatherCtrForm.Hide();
            this.weatherForm.Hide();
            this.weatherRecord.Hide();
#if TotalVer
            this.weatherState.Hide();
#endif

            this.WeatherSmsToolStripBtn.Checked = true;
            this.WeatherCtrToolStripBtn.Checked = false;
            this.WeatherToolStripBtn.Checked = false;
            this.WeatherRecordToolStripBtn.Checked = false;
            this.WeatherStateToolStripBtn.Checked = false;

            this.sMS그룹관리SToolStripMenuItem.Checked = true;
            this.우량기제어RToolStripMenuItem.Checked = false;
            this.우량기상태WToolStripMenuItem.Checked = false;
            this.이력조회DToolStripMenuItem.Checked = false;
            this.정보통계ToolStripMenuItem.Checked = false;
        }

        //메뉴의 이력 조회 클릭
        private void WeatherRecordToolStripBtn_Click(object sender, EventArgs e)
        {
            this.weatherRecord.Show();
            this.weatherSmsForm.Hide();
            this.weatherCtrForm.Hide();
            this.weatherForm.Hide();
#if TotalVer
            this.weatherState.Hide();
#endif

            this.WeatherRecordToolStripBtn.Checked = true;
            this.WeatherSmsToolStripBtn.Checked = false;
            this.WeatherCtrToolStripBtn.Checked = false;
            this.WeatherToolStripBtn.Checked = false;
            this.WeatherStateToolStripBtn.Checked = false;

            this.이력조회DToolStripMenuItem.Checked = true;
            this.sMS그룹관리SToolStripMenuItem.Checked = false;
            this.우량기제어RToolStripMenuItem.Checked = false;
            this.우량기상태WToolStripMenuItem.Checked = false;
            this.정보통계ToolStripMenuItem.Checked = false;
        }

        //메뉴의 정보 통계 클릭
        private void WeatherStateToolStripBtn_Click(object sender, EventArgs e)
        {
#if TotalVer
            this.weatherState.Show();
#endif
            this.weatherRecord.Hide();
            this.weatherSmsForm.Hide();
            this.weatherCtrForm.Hide();
            this.weatherForm.Hide();

            this.WeatherStateToolStripBtn.Checked = true;
            this.WeatherRecordToolStripBtn.Checked = false;
            this.WeatherSmsToolStripBtn.Checked = false;
            this.WeatherCtrToolStripBtn.Checked = false;
            this.WeatherToolStripBtn.Checked = false;

            this.정보통계ToolStripMenuItem.Checked = true;
            this.이력조회DToolStripMenuItem.Checked = false;
            this.sMS그룹관리SToolStripMenuItem.Checked = false;
            this.우량기제어RToolStripMenuItem.Checked = false;
            this.우량기상태WToolStripMenuItem.Checked = false;
        }

        //메뉴의 자가 진단 클릭
        private void 자가진단ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.weatherSelfForm == null)
            {
                this.weatherSelfForm = new WeatherSelfForm();
            }
            else
            {
                this.weatherSelfForm = null;
                this.weatherSelfForm = new WeatherSelfForm();
            }

            this.weatherSelfForm.Show();
        }

        //메뉴의 환경 설정 클릭
        private void 환경설정OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (this.weatherOptionForm = new WeatherOptionForm())
            {
                this.weatherOptionForm.ServerConEvt += new EventHandler<ServerStartEventArgs>(weatherOptionForm_ServerConEvt);
                this.weatherOptionForm.ServerStopEvt += new EventHandler<ServerStopEventArgs>(weatherOptionForm_ServerStopEvt);
                this.weatherOptionForm.ClientConEvt += new EventHandler<TcpClientConEventArgs>(weatherOptionForm_ClientConEvt);
                this.weatherOptionForm.ClientConEndEvt += new EventHandler<TcpClientConEndEventArgs>(weatherOptionForm_ClientConEndEvt);
                this.weatherOptionForm.SerialConEvt += new EventHandler<SerialConEventArgs>(weatherOptionForm_SerialConEvt);
                this.weatherOptionForm.SerialConEndEvt += new EventHandler<SerialConEndEventArgs>(weatherOptionForm_SerialConEndEvt);
                this.weatherOptionForm.OptionDataSetEvt += new EventHandler<WeatherDataOptionSetEventArgs>(weatherOptionForm_OptionDataSetEvt);
                this.weatherOptionForm.DBDataSetEvt += new EventHandler<DBDataSetEventArgs>(weatherOptionForm_DBDataSetEvt);
                this.weatherOptionForm.AutoLoginEvt += new EventHandler<AutoLoginEventArgs>(weatherOptionForm_AutoLoginEvt);
                this.weatherOptionForm.CDMAPortEvt += new EventHandler<CDMAPortEventArgs>(weatherOptionForm_CDMAPortEvt);
                this.weatherOptionForm.TcpIsUseEvt += new EventHandler<TcpIsUseEventArgs>(weatherOptionForm_TcpIsUseEvt);
                this.weatherOptionForm.SerialIsUseEvt += new EventHandler<SerialIsUseEventArgs>(weatherOptionForm_SerialIsUseEvt);
                this.weatherOptionForm.WDeviceChkTimeEvt += new EventHandler<WDeviceChkTimeEventArgs>(weatherOptionForm_WDeviceChkTimeEvt);
                this.weatherOptionForm.OnTcpServer_IsChange += new EventHandler<ServerStartEventArgs>(weatherOptionForm_OnTcpServer_IsChange);
                this.weatherOptionForm.OnEthernetServer_IsChange += new EventHandler<ServerStartEventArgs>(weatherOptionForm_OnEthernetServer_IsChange);
                this.weatherOptionForm.OnEthernetServerConEvt += new EventHandler<ServerStartEventArgs>(weatherOptionForm_OnEthernetServerConEvt);
                this.weatherOptionForm.OnEthernetServerStopEvt += new EventHandler<ServerStopEventArgs>(weatherOptionForm_OnEthernetServerStopEvt);
                this.weatherOptionForm.ServerState = this.server.bServerListen;
                this.weatherOptionForm.EthernetServerState = this.ethernetServer.IsRunning;
                this.weatherOptionForm.TcpClient1 = this.client_1.TcpConn;
                this.weatherOptionForm.TcpClient2 = this.client_2.TcpConn;

                if (this.serial_1 == null)
                {
                    this.weatherOptionForm.SerialClient1 = false;
                }
                else
                {
                    this.weatherOptionForm.SerialClient1 = this.serial_1.IsOpenState();
                }

                if (this.serial_2 == null)
                {
                    this.weatherOptionForm.SerialClient2 = false;
                }
                else
                {
                    this.weatherOptionForm.SerialClient2 = this.serial_2.IsOpenState();
                }

                this.weatherOptionForm.SetAutoLogin(Settings.Default.AutoLogin);
                this.weatherOptionForm.ShowDialog();
                this.weatherOptionForm.ServerConEvt -= new EventHandler<ServerStartEventArgs>(weatherOptionForm_ServerConEvt);
                this.weatherOptionForm.ServerStopEvt -= new EventHandler<ServerStopEventArgs>(weatherOptionForm_ServerStopEvt);
                this.weatherOptionForm.ClientConEvt -= new EventHandler<TcpClientConEventArgs>(weatherOptionForm_ClientConEvt);
                this.weatherOptionForm.ClientConEndEvt -= new EventHandler<TcpClientConEndEventArgs>(weatherOptionForm_ClientConEndEvt);
                this.weatherOptionForm.SerialConEvt -= new EventHandler<SerialConEventArgs>(weatherOptionForm_SerialConEvt);
                this.weatherOptionForm.SerialConEndEvt -= new EventHandler<SerialConEndEventArgs>(weatherOptionForm_SerialConEndEvt);
                this.weatherOptionForm.OptionDataSetEvt -= new EventHandler<WeatherDataOptionSetEventArgs>(weatherOptionForm_OptionDataSetEvt);
                this.weatherOptionForm.DBDataSetEvt -= new EventHandler<DBDataSetEventArgs>(weatherOptionForm_DBDataSetEvt);
                this.weatherOptionForm.AutoLoginEvt -= new EventHandler<AutoLoginEventArgs>(weatherOptionForm_AutoLoginEvt);
                this.weatherOptionForm.CDMAPortEvt -= new EventHandler<CDMAPortEventArgs>(weatherOptionForm_CDMAPortEvt);
                this.weatherOptionForm.TcpIsUseEvt -= new EventHandler<TcpIsUseEventArgs>(weatherOptionForm_TcpIsUseEvt);
                this.weatherOptionForm.SerialIsUseEvt -= new EventHandler<SerialIsUseEventArgs>(weatherOptionForm_SerialIsUseEvt);
                this.weatherOptionForm.WDeviceChkTimeEvt -= new EventHandler<WDeviceChkTimeEventArgs>(weatherOptionForm_WDeviceChkTimeEvt);
                this.weatherOptionForm.OnTcpServer_IsChange -= new EventHandler<ServerStartEventArgs>(weatherOptionForm_OnTcpServer_IsChange);
                this.weatherOptionForm.OnEthernetServer_IsChange -= new EventHandler<ServerStartEventArgs>(weatherOptionForm_OnEthernetServer_IsChange);
                this.weatherOptionForm.OnEthernetServerConEvt -= new EventHandler<ServerStartEventArgs>(weatherOptionForm_OnEthernetServerConEvt);
                this.weatherOptionForm.OnEthernetServerStopEvt -= new EventHandler<ServerStopEventArgs>(weatherOptionForm_OnEthernetServerStopEvt);
            }
        }

        /// <summary>
        /// 옵션창에서 주는 TCP 이더넷 서버 셋팅 값 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_OnEthernetServer_IsChange(object sender, ServerStartEventArgs e)
        {
            Settings.Default.EthernetIp = e.Ip;
            Settings.Default.EthernetPort = e.Port.ToString();
            Settings.Default.Save();
        }

        /// <summary>
        /// 옵션창에서 주는 TCP 서버 셋팅 값 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_OnTcpServer_IsChange(object sender, ServerStartEventArgs e)
        {
            Settings.Default.ServerIp = e.Ip;
            Settings.Default.ServerPort = e.Port.ToString();
            Settings.Default.Save();
        }

        //옵션창에서 주는 측기 통신 시간 설정 이벤트
        private void weatherOptionForm_WDeviceChkTimeEvt(object sender, WDeviceChkTimeEventArgs e)
        {
            Settings.Default.WDeviceChkTime = e.ChkTime;
            Settings.Default.Save();
            this.WDeviceChkTimeInterval = int.Parse(Settings.Default.WDeviceChkTime.ToString());

            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("측기의 통신 체크 시간이 {0} 분으로 설정되었습니다.", e.ChkTime) });
            }
            else
            {
                this.SetRWeatherData(string.Format("측기의 통신 체크 시간이 {0} 분으로 설정되었습니다.", e.ChkTime));
            }

            for (int i = 0; i < this.CheckerList.Count; i++)
            {
                this.CheckerList[i].Flag = false;
            }

            this.CheckerList.Clear();
            this.GetWDeviceState();
        }

        //Seial 통신의 정보를 받아 메인 리소스에 저장한다. 초기 로딩 시 사용하기 위함.
        private void weatherOptionForm_SerialIsUseEvt(object sender, SerialIsUseEventArgs e)
        {
            if (e.Num == (byte)SType.client_1)
            {
                Settings.Default.Serial1IsUse = e.IsUse;
                Settings.Default.S1Com = e.ComPort;
                Settings.Default.S1Rate = e.BaudRate;
                Settings.Default.S1DataBit = e.DataBits;
                Settings.Default.S1Parity = e.Parity;
                Settings.Default.S1StopBit = e.StopBit;
                Settings.Default.Save();
            }
            else if (e.Num == (byte)SType.client_2)
            {
                Settings.Default.Serial2IsUse = e.IsUse;
                Settings.Default.S2Com = e.ComPort;
                Settings.Default.S2Rate = e.BaudRate;
                Settings.Default.S2DataBit = e.DataBits;
                Settings.Default.S2Parity = e.Parity;
                Settings.Default.S2StopBit = e.StopBit;
                Settings.Default.Save();
            }
        }

        //TCP 클라이언트의 정보를 받아 메인 리소스에 저장한다. 초기 로딩 시 사용하기 위함.
        private void weatherOptionForm_TcpIsUseEvt(object sender, TcpIsUseEventArgs e)
        {
            Settings.Default.Tcp1IsUse = e.Tcp1IsUse;
            Settings.Default.Tcp2IsUse = e.Tcp2IsUse;
            Settings.Default.Tcp1Ip = e.Tcp1IP;
            Settings.Default.Tcp1Port = e.Tcp1PORT;
            Settings.Default.Tcp2Ip = e.Tcp2IP;
            Settings.Default.Tcp2Port = e.Tcp2PORT;
            Settings.Default.Save();
        }

        //메뉴의 종료 클릭
        private void 종료XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //메뉴의 측기 정보 관리 클릭
        private void 우량기정보관리GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (this.deviceMng = new DeviceMng())
            {
                this.deviceMng.ShowDialog();
            }
        }

        //메뉴의 프로그램정보 클릭
        private void 프로그램정보IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (this.aboutForm = new WeatherAboutForm(wRSName, this.wRSVerList))
            {
                this.aboutForm.ShowDialog();
            }
        }
        #endregion

        #region Event
        //SMS 전송 후 결과 이벤트
        private void cdmaComm_onCDMA_SendResultEvt(object sender, CDMA_SendResultEvtArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} 로 전송한 SMS가 {1} 하였습니다.", e.RcvNum, (e.bResult == true) ? "성공" : "실패") });
            }
            else
            {
                this.SetRWeatherData(string.Format("{0} 로 전송한 SMS가 {1} 하였습니다.", e.RcvNum, (e.bResult == true) ? "성공" : "실패"));
            }
        }

        //CDMA 포트 설정 변경 후 이벤트
        private void weatherOptionForm_CDMAPortEvt(object sender, CDMAPortEventArgs e)
        {
            Settings.Default.CdmaPort = e.ComPort;
            Settings.Default.Save();

            this.cdmaComm.SerialClose();
            this.cdmaComm.onSerialConnectEvt -= new EventHandler<SerialConnect>(cdmaComm_onSerialConnectEvt);
            this.cdmaComm.onSerialDisConnectEvt -= new EventHandler<SerialDisConnect>(cdmaComm_onSerialDisConnectEvt);
            this.cdmaComm.onMyNumberRcvEvt -= new EventHandler<MyNumberRcvEvtArgs>(cdmaComm_onMyNumberRcvEvt);
            this.cdmaComm.onCDMA_SendResultEvt -= new EventHandler<CDMA_SendResultEvtArgs>(cdmaComm_onCDMA_SendResultEvt);
            this.cdmaComm = null;
            Thread.Sleep(20);

            this.cdmaComm = new CDMAComm(e.ComPort, 115200, Parity.None, 8, StopBits.One);
            this.cdmaComm.onSerialConnectEvt += new EventHandler<SerialConnect>(cdmaComm_onSerialConnectEvt);
            this.cdmaComm.onSerialDisConnectEvt += new EventHandler<SerialDisConnect>(cdmaComm_onSerialDisConnectEvt);
            this.cdmaComm.onMyNumberRcvEvt += new EventHandler<MyNumberRcvEvtArgs>(cdmaComm_onMyNumberRcvEvt);
            this.cdmaComm.onCDMA_SendResultEvt += new EventHandler<CDMA_SendResultEvtArgs>(cdmaComm_onCDMA_SendResultEvt);
            this.cdmaComm.SerialCon();

            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 모듈의 포트를 {0} 로 설정하였습니다.", e.ComPort) });
            }
            else
            {
                this.SetRWeatherData(string.Format("CDMA 모듈의 포트를 {0} 로 설정하였습니다.", e.ComPort));
            }
        }

        //CDMA 모듈과 연결 후 CDMA 전화번호를 가져오는 이벤트
        private void cdmaComm_onMyNumberRcvEvt(object sender, MyNumberRcvEvtArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 모듈의 전화번호는 {0} 입니다.", e.MyNumber) });
            }
            else
            {
                this.SetRWeatherData(string.Format("CDMA 모듈의 전화번호는 {0} 입니다.", e.MyNumber));
            }

            this.dataMng.CDMA = true;
        }

        //WOU 측기의 무시시간 제어에 사용하는 이벤트
        private void dataMng_onWOUFTimeControlEvt(object sender, WOUWDeviceControlFTimeEventArgs e)
        {
            WDevice tmpDevice = this.dataMng.GetWDevice(e.FKDevice);
            Proto008 p008 = ProtoMng.GetProtoObj("008") as Proto008;
            p008.Length = "025";
            p008.ID = tmpDevice.ID.PadLeft(15, '0');
            p008.Data = string.Format("{0}{1}", e.SameTime, e.DownTime);
            p008.ChkSum = 31;
            byte[] totProto = p008.MakeProto();
            this.SendFromWRS(totProto);
        }

        //WOU 측기의 임계치 제어에 사용하는 이벤트
        private void dataMng_onWOUAlarmControlEvt(object sender, WOUWDeviceControlAlarmEventArgs e)
        {
            WDevice tmpDevice = this.dataMng.GetWDevice(e.FKDevice);
            Proto007 p007 = ProtoMng.GetProtoObj("007") as Proto007;
            p007.Length = "035";
            p007.ID = tmpDevice.ID.PadLeft(15, '0');
            p007.Data = string.Format("{0}{1}{2}{3}", e.SKind, e.Alarm1, e.Alarm2, e.Alarm3);
            p007.ChkSum = 41;
            byte[] totProto = p007.MakeProto();
            this.SendFromWRS(totProto);
        }

        /// <summary>
        /// WOU 측기에 상태 요청을 하는 경우의 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onWOURequestEvt(object sender, WOUWDeviceRequestEventArgs e)
        {
            WDevice tmpDevice = this.dataMng.GetWDevice(e.PKID);
            Proto004 p004 = ProtoMng.GetProtoObj("004") as Proto004;
            p004.Length = "019";
            p004.ID = tmpDevice.ID.PadLeft(15, '0');
            p004.Data = string.Empty;
            p004.ChkSum = 25;
            byte[] totProto = p004.MakeProto();
            this.SendFromWRS(totProto);
            Thread.Sleep(20);

            Proto005 p005 = ProtoMng.GetProtoObj("005") as Proto005;
            p005.Length = "019";
            p005.ID = tmpDevice.ID.PadLeft(15, '0');
            p005.Data = string.Empty;
            p005.ChkSum = 25;
            totProto = p005.MakeProto();
            this.SendFromWRS(totProto);
            Thread.Sleep(20);

            Proto006 p006 = ProtoMng.GetProtoObj("006") as Proto006;
            p006.Length = "019";
            p006.ID = tmpDevice.ID.PadLeft(15, '0');
            p006.Data = string.Empty;
            p006.ChkSum = 25;
            totProto = p006.MakeProto();
            this.SendFromWRS(totProto);
            Thread.Sleep(20);
        }

        /// <summary>
        /// CDMA 모듈과의 시리얼 통신 끊어짐 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cdmaComm_onSerialDisConnectEvt(object sender, SerialDisConnect e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 모듈과 Serial 통신이 종료되었습니다.") });
            }
            else
            {
                this.SetRWeatherData(string.Format("CDMA 모듈과 Serial 통신이 종료되었습니다."));
            }
        }

        /// <summary>
        /// CDMA 모듈과의 시리얼 통신 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cdmaComm_onSerialConnectEvt(object sender, SerialConnect e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 모듈의 Serial 통신 Open을 성공했습니다.") });
            }
            else
            {
                this.SetRWeatherData(string.Format("CDMA 모듈의 Serial 통신 Open을 성공했습니다."));
            }
        }

        /// <summary>
        /// SMS 전송 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onSendSmsMsgEvt(object sender, SendSmsMsgEventArgs e)
        {
            try
            {
                this.cdmaComm.CDMA_SendData(e.Msg, e.CellNum, DateTime.Now);

                string tmpStr = Encoding.Default.GetString(e.Msg);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} 번호로 ( {1} ) SMS를 전송했습니다.", e.CellNum, tmpStr) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("{0} 번호로 ( {1} ) SMS를 전송했습니다.", e.CellNum, tmpStr));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.dataMng_onSendSmsMsgEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS User에게 SMS 전송 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onSendSmsUserMsgEvt(object sender, SendSmsMsgEventArgs e)
        {
            try
            {
                this.cdmaComm.CDMA_SendData(e.Msg, e.CellNum, DateTime.Now);

                string tmpStr = Encoding.Default.GetString(e.Msg);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} 번호로 ( {1} ) SMS를 전송했습니다.", e.CellNum, tmpStr) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("{0} 번호로 ( {1} ) SMS를 전송했습니다.", e.CellNum, tmpStr));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.dataMng_onSendSmsUserMsgEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// byte[]를 던져 파싱 후 날아오는 이벤트 (RAT 관련)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parsing_onRainMachineRcvEvt(object sender, RainMachineRcv e)
        {
            try
            {
                this.server_onRainMachineRcvEvt(this, e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.parsing_onRainMachineRcvEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// byte[]를 던져 파싱 후 날아오는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parsing_onWOU_RcvDataEvt(object sender, WOU_RcvData e)
        {
            byte[] totProto = new byte[0];
            WDevice wDeviceRoot = new WDevice();

            if (e.DivisionID == "WOU")
            {
                wDeviceRoot = this.dataMng.GetWouDevice(e.DiviceID.Substring(2, 13));
            }
            else if (e.DivisionID == "RAT")
            {
                wDeviceRoot = this.dataMng.GetRatDevice(e.DiviceID.Substring(2, 13));
            }
            else if (e.DivisionID == "HSD")
            {
                wDeviceRoot = this.dataMng.GetHSDDevice(e.DiviceID);
            }
            else if (e.DivisionID == "DSD")
            {
                wDeviceRoot = this.dataMng.GetDSDDevice(e.DiviceID);
            }

            if (e.DivisionID == "WOU" || e.DivisionID == "RAT")
            {
                if (!this.dataMng.GetWDeviceComparer(wDeviceRoot))
                {
                    if (e.CmdCode != "006" && e.CmdCode != "012") //통신상태확인, SMS전송은 제외
                    {
                        return;
                    }
                }
            }

            switch (e.CmdCode)
            {
                case "001": //강수량 데이터
                    //DB저장
                    DateTime datetime001Tmp = new DateTime(int.Parse(e.listDetailData[4].Substring(0, 4)),
                                                        int.Parse(e.listDetailData[4].Substring(4, 2)),
                                                        int.Parse(e.listDetailData[4].Substring(6, 2)),
                                                        int.Parse(e.listDetailData[4].Substring(8, 2)),
                                                        int.Parse(e.listDetailData[4].Substring(10, 2)),
                                                        int.Parse(e.listDetailData[4].Substring(12, 2)));

                    WRainData wrR = null;
                    wrR = new WRainData(0, wDeviceRoot.PKID, datetime001Tmp, e.listDetailData[0], string.Empty,
                        e.listDetailData[1], string.Empty, e.listDetailData[2], e.listDetailData[3]);
                    this.dataMng.AddRainData(wrR);

                    //송신 프로토콜 조합
                    Proto001 p001 = ProtoMng.GetProtoObj("001") as Proto001;
                    p001.Division = e.DivisionID;
                    p001.ID = e.DiviceID.PadLeft(15, '0');
                    p001.Data = string.Format("010{0}00000{1}00000{2}{3}{4}", e.listDetailData[0], e.listDetailData[1]
                        , e.listDetailData[2], e.listDetailData[3], e.listDetailData[4]);
                    totProto = p001.MakeProto();
                    this.SendFromWRS(totProto);
                    break;

                case "002": //시험 요청
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 임계치 시험 요청 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 임계치 시험 요청 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeTestR = WeatherDataMng.WIType.강수센서시험요청;

                    if (e.listDetailData[0] == "1") //강우량계
                    {
                        wiTypeTestR = WeatherDataMng.WIType.강수센서시험요청;
                    }
                    else if (e.listDetailData[0] == "2") //수위계
                    {
                        wiTypeTestR = WeatherDataMng.WIType.수위센서시험요청;
                    }
                    else if (e.listDetailData[0] == "3") //유속계
                    {
                        wiTypeTestR = WeatherDataMng.WIType.유속센서시험요청;
                    }

                    WTypeDeviceItem TestRTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeTestR);
                    WDeviceItem wdiTestR = new WDeviceItem(0, wDeviceRoot.PKID, TestRTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                    this.dataMng.AddDeviceItemData(wdiTestR);

                    //송신 프로토콜 조합
                    CProto06 cProto06 = CProtoMng.GetProtoObj("06") as CProto06;
                    cProto06.Header = "[";
                    cProto06.Length = "023";
                    cProto06.ID = e.DiviceID.Substring(2, 13);
                    cProto06.MainCode = "0";
                    cProto06.SubCode = "H";
                    cProto06.RecvType = "1";
                    cProto06.Data = string.Format("{0}{1}", e.listDetailData[0], e.listDetailData[1]);
                    cProto06.CRC = "00000";
                    cProto06.Tail = "]";

                    byte[] buff = cProto06.MakeProto();
                    this.dataMng.SendSmsMsg(wDeviceRoot.CellNumber, buff);
                    Thread.Sleep(20);
                    break;

                case "003": //임계치 제어 응답
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 임계치 제어 응답 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 임계치 제어 응답 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeAlarmCtr = WeatherDataMng.WIType.임계치제어응답;
                    WTypeDeviceItem AlarmCtrTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarmCtr);
                    WDeviceItem tmpAlarmCtr = new WDeviceItem(0, wDeviceRoot.PKID, AlarmCtrTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(tmpAlarmCtr);
                    break;

                case "004": //무시시간 제어 응답
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 무시시간 제어 응답 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 무시시간 제어 응답 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeFTimeCtr = WeatherDataMng.WIType.무시시간제어응답;
                    WTypeDeviceItem FTimeCtrTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFTimeCtr);
                    WDeviceItem tmpFTimeCtr = new WDeviceItem(0, wDeviceRoot.PKID, FTimeCtrTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(tmpFTimeCtr);
                    break;

                case "006": //통신 상태 요청(폴링)
                    Proto009 p009 = ProtoMng.GetProtoObj("009") as Proto009;
                    p009.Division = e.DivisionID;
                    p009.ID = e.DiviceID.PadLeft(15, '0');
                    totProto = p009.MakeProto();
                    this.SendFromWRS(totProto);
                    break;

                case "007": //센서 상태 정보
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 센서 상태정보 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 센서 상태정보 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiType = WeatherDataMng.WIType.강수센서상태;

                    if (e.listDetailData[0] == "2")
                    {
                        wiType = WeatherDataMng.WIType.수위센서상태;
                    }
                    else if (e.listDetailData[0] == "3")
                    {
                        wiType = WeatherDataMng.WIType.유속센서상태;
                    }

                    WTypeDeviceItem tmpTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiType);
                    WDeviceItem wdi = new WDeviceItem(0, wDeviceRoot.PKID, tmpTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                    this.dataMng.AddDeviceItemData(wdi);

                    //송신 프로토콜 조합
                    Proto010 p010 = ProtoMng.GetProtoObj("010") as Proto010;
                    p010.Division = e.DivisionID;
                    p010.ID = e.DiviceID.PadLeft(15, '0');
                    p010.Data = string.Format("{0}{1}", e.listDetailData[0], e.listDetailData[1]);
                    totProto = p010.MakeProto();
                    this.SendFromWRS(totProto);
                    break;

                case "008": //임계치 정보 요청
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 임계치 정보요청 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 임계치 정보요청 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeAlarmD = WeatherDataMng.WIType.강수임계치요청;

                    if (e.listDetailData[0] == "2")
                    {
                        wiTypeAlarmD = WeatherDataMng.WIType.수위임계치요청;
                    }
                    else if (e.listDetailData[0] == "3")
                    {
                        wiTypeAlarmD = WeatherDataMng.WIType.유속임계치요청;
                    }

                    WTypeDeviceItem AlarmDTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarmD);
                    WDeviceRequest tmp = new WDeviceRequest(0, wDeviceRoot.PKID, AlarmDTypeDeviceItem.PKID, DateTime.Now, 0, e.listDetailData[0]);
                    this.dataMng.AddDeviceRequest(tmp);

                    //송신 프로토콜 조합
                    CProto07 cProto07 = CProtoMng.GetProtoObj("07") as CProto07;
                    cProto07.Header = "[";
                    cProto07.Length = "022";
                    cProto07.ID = wDeviceRoot.ID;
                    cProto07.MainCode = "0";
                    cProto07.SubCode = "L";
                    cProto07.RecvType = "1";
                    cProto07.Data = string.Format("{0}", e.listDetailData[0]);
                    cProto07.CRC = "00000";
                    cProto07.Tail = "]";

                    totProto = cProto07.MakeProto();
                    this.dataMng.SendSmsMsg(wDeviceRoot.CellNumber, totProto);
                    break;

                case "009": //무시시간 정보 요청
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 무시시간 정보요청 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 무시시간 정보요청 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeFTime = WeatherDataMng.WIType.무시시간요청;
                    WTypeDeviceItem FTimeTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFTime);
                    WDeviceRequest tmpFTime = new WDeviceRequest(0, wDeviceRoot.PKID, FTimeTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                    this.dataMng.AddDeviceRequest(tmpFTime);

                    //송신 프로토콜 조합
                    CProto08 cProto08 = CProtoMng.GetProtoObj("08") as CProto08;
                    cProto08.Header = "[";
                    cProto08.Length = "021";
                    cProto08.ID = wDeviceRoot.ID;
                    cProto08.MainCode = "0";
                    cProto08.SubCode = "s";
                    cProto08.RecvType = "1";
                    cProto08.Data = string.Empty;
                    cProto08.CRC = "00000";
                    cProto08.Tail = "]";

                    totProto = cProto08.MakeProto();
                    this.dataMng.SendSmsMsg(wDeviceRoot.CellNumber, totProto);
                    break;

                case "010": //임계치 상태 정보
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 임계치 상태정보 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 임계치 상태정보 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeAlarm = WeatherDataMng.WIType.강수임계치1단계;
                    WTypeDeviceItem AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                    WDeviceItem wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);

                    if (e.listDetailData[0] == "1")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.강수임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.강수임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.강수임계치3단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);
                    }
                    else if (e.listDetailData[0] == "2")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.수위임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.수위임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.수위임계치3단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);
                    }
                    else if (e.listDetailData[0] == "3")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.유속임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.유속임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.유속임계치3단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wDeviceRoot.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);
                    }

                    //송신 프로토콜 조합
                    Proto011 p011 = ProtoMng.GetProtoObj("011") as Proto011;
                    p011.Division = e.DivisionID;
                    p011.ID = e.DiviceID.PadLeft(15, '0');
                    p011.Data = e.listDetailData[0] + e.listDetailData[1] + e.listDetailData[2] + e.listDetailData[3];
                    totProto = p011.MakeProto();
                    this.SendFromWRS(totProto);
                    break;

                case "011": //무시시간 상태 정보
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 무시시간 상태정보 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 무시시간 상태정보 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    //DB 저장
                    WeatherDataMng.WIType wiTypeFTimeS = WeatherDataMng.WIType.동일레벨무시시간;
                    WTypeDeviceItem FTimeSTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFTimeS);
                    WDeviceItem wdiFTimeS = new WDeviceItem(0, wDeviceRoot.PKID, FTimeSTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiFTimeS);
                    Thread.Sleep(20);

                    wiTypeFTimeS = WeatherDataMng.WIType.하향레벨무시시간;
                    FTimeSTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFTimeS);
                    wdiFTimeS = new WDeviceItem(0, wDeviceRoot.PKID, FTimeSTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                    this.dataMng.AddDeviceItemData(wdiFTimeS);
                    Thread.Sleep(20);

                    //송신 프로토콜 조합
                    Proto012 p012 = ProtoMng.GetProtoObj("012") as Proto012;
                    p012.Division = e.DivisionID;
                    p012.ID = e.DiviceID.PadLeft(15, '0');
                    p012.Data = e.listDetailData[0] + e.listDetailData[1];
                    totProto = p012.MakeProto();
                    this.SendFromWRS(totProto);
                    break;

                case "012": //발령 SMS 송신
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial : {0} 의 {1} 에서 발령 SMS 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("Serial : {0} 의 {1} 에서 발령 SMS 데이터가 수신되었습니다.",
                            e.DivisionID, wDeviceRoot.ID));
                    }

                    byte[] tmpMsg = Encoding.Default.GetBytes(e.listDetailData[0]);

                    for (int i = 1; i < e.listDetailData.Count; i++)
                    {
                        if (e.listDetailData[i].Substring(0, 2) == "00")
                        {
                            this.cdmaComm.CDMA_SendData(tmpMsg, e.listDetailData[i].Substring(1, 10), DateTime.Now);
                        }
                        else
                        {
                            this.cdmaComm.CDMA_SendData(tmpMsg, e.listDetailData[i], DateTime.Now);
                        }

                        Thread.Sleep(20);
                    }

                    break;

                case "100": //임계치(알람)
                    //DB저장
                    DateTime wAlarmDatetimeTmp = new DateTime(int.Parse(e.listDetailData[4].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[4].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[4].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[4].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[4].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[4].Substring(12, 2)));
                    WAlarmData wa = new WAlarmData(0, wDeviceRoot.PKID, byte.Parse(e.listDetailData[0]), wAlarmDatetimeTmp,
                        byte.Parse(e.listDetailData[1]), byte.Parse(e.listDetailData[2]), e.listDetailData[3]);
                    this.dataMng.AddAlarmData(wa);

                    //팝업 UI
                    bool real = false;

                    if (e.listDetailData[0] == "0")
                    {
                        real = true;
                    }

                    string deviceId = string.Format("{0}({1})", wDeviceRoot.Name, e.DiviceID);
                    string level = (e.listDetailData[1] == "1") ? "우량계" :
                                        (e.listDetailData[1] == "2") ? "수위계" :
                                        (e.listDetailData[1] == "3") ? "유속계" :
                                        (e.listDetailData[1] == "4") ? "풍향풍속계" : "기타";
                    level += (e.listDetailData[2] == "1") ? ", 주의(1단계)" :
                                (e.listDetailData[2] == "2") ? ", 경계(2단계)" :
                                (e.listDetailData[2] == "3") ? ", 대피(3단계)" : ", 기타";
                    double tmpdata = (e.listDetailData[1] == "2") ? double.Parse(e.listDetailData[3]) * 0.01 : double.Parse(e.listDetailData[3]) * 0.1;
                    string data = (e.listDetailData[1] == "1") ? string.Format("현재 {0} (mm)", tmpdata) :
                                    (e.listDetailData[1] == "2") ? string.Format("현재 {0} (meter)", tmpdata) : string.Format("현재 {0} (m/s)", tmpdata);
                    DateTime dt = new DateTime(int.Parse(e.listDetailData[4].Substring(0, 4)),
                                                int.Parse(e.listDetailData[4].Substring(4, 2)),
                                                int.Parse(e.listDetailData[4].Substring(6, 2)),
                                                int.Parse(e.listDetailData[4].Substring(8, 2)),
                                                int.Parse(e.listDetailData[4].Substring(10, 2)),
                                                int.Parse(e.listDetailData[4].Substring(12, 2)));
                    this.aStruct.Real = real;
                    this.aStruct.ID = deviceId;
                    this.aStruct.Level = level;
                    this.aStruct.Data = data;
                    this.aStruct.DDTime = dt;

                    this.AlarmPopTD = new Thread(new ThreadStart(this.PopUpThread));
                    this.AlarmPopTD.IsBackground = true;
                    this.AlarmPopTD.Start();

                    //송신 프로토콜 조합
                    Proto100 p100 = ProtoMng.GetProtoObj("100") as Proto100;
                    p100.Division = e.DivisionID;
                    p100.ID = e.DiviceID.PadLeft(15, '0');
                    p100.Data = e.listDetailData[0] + e.listDetailData[1] + e.listDetailData[2] + e.listDetailData[3] + e.listDetailData[4];
                    totProto = p100.MakeProto();
                    this.SendFromWRS(totProto);

                    //측기 담당자에게 SMS 송신
                    List<WSmsUser> tmpSmsUserList = this.dataMng.getSmsUserList(wDeviceRoot.PKID);
                    string smsUserMsg = string.Format("[{0} 임계치알람 발생] {5}시{6}분 {1} {2} {3} {4} 발생!",
                        (real == true) ? "실제" : "시험",
                        wDeviceRoot.Name,
                        (e.listDetailData[1] == "1") ? "강우" :
                                (e.listDetailData[1] == "2") ? "수위" :
                                (e.listDetailData[1] == "3") ? "유속" :
                                (e.listDetailData[1] == "4") ? "풍향풍속" : "기타",
                        (e.listDetailData[2] == "0") ? "해제" :
                                (e.listDetailData[2] == "1") ? "주의(1단계)" :
                                (e.listDetailData[2] == "2") ? "경계(2단계)" :
                                (e.listDetailData[2] == "3") ? "대피(3단계)" : ", 기타",
                        (e.listDetailData[1] == "1") ? string.Format("현재 {0}mm", tmpdata) :
                                    (e.listDetailData[1] == "2") ? string.Format("현재 {0}m", tmpdata) :
                                    string.Format("현재 {0}m/s", tmpdata),
                        dt.Hour, dt.Minute);

                    for (int i = 0; i < tmpSmsUserList.Count; i++)
                    {
                        if (e.listDetailData[2] == "1")
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList[i].PKID, WeatherDataMng.SMSType.임계치1단계).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, smsUserMsg);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg));
                            }
                        }
                        else if (e.listDetailData[2] == "2")
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList[i].PKID, WeatherDataMng.SMSType.임계치2단계).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, smsUserMsg);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg));
                            }
                        }
                        else if (e.listDetailData[2] == "3")
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList[i].PKID, WeatherDataMng.SMSType.임계치3단계).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, smsUserMsg);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg));
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 자동로그인 설정 항목 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_AutoLoginEvt(object sender, AutoLoginEventArgs e)
        {
            Settings.Default.AutoLogin = e.AutoFlag;
            Settings.Default.Save();
        }

        /// <summary>
        /// DB 설정 항목 변경 후 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_DBDataSetEvt(object sender, DBDataSetEventArgs e)
        {
            Settings.Default.DbIp = e.IP;
            Settings.Default.DbPort = e.PORT;
            Settings.Default.DbId = e.ID;
            Settings.Default.DbPw = e.PW;
            Settings.Default.DbSid = e.SID;
            Settings.Default.Save();

            this.dataMng.setDBData(e.IP, e.PORT, e.ID, e.PW, e.SID);
        }

        /// <summary>
        /// CDMA 클라이언트 연결 이벤트(서버 모듈에서 주는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_onClientConnectEvt(object sender, ClientConnect e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 클라이언트가 연결되었습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("CDMA 클라이언트가 연결되었습니다."));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.server_onClientConnectEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// CDMA 클라이언트 연결 해제 이벤트(서버 모듈에서 주는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_onClientDisConnectEvt(object sender, ClientDisConnect e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("CDMA 클라이언트가 연결 해제되었습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("CDMA 클라이언트가 연결 해제되었습니다."));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.server_onClientDisConnectEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// byte[] 데이터 receive 이벤트(서버 모듈에서 주는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_onRecieveByteArrayEvt(object sender, RecieveByteArray e)
        {
            #region Low Data
            //string tmpStr = Encoding.Default.GetString(e.rcvBytes);

            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP 클라이언트 데이터 [ {0} ]", tmpStr) });
            //}
            //else
            //{
            //    this.SetRWeatherData(string.Format("TCP 클라이언트 데이터 [ {0} ]", tmpStr));
            //}
            #endregion

            #region Low Data System Log
            string tmpStr = Encoding.Default.GetString(e.rcvBytes);
            EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.Information, "CDMA - " + tmpStr);
            #endregion
        }

        /// <summary>
        /// 클라이언트1 통신 연결 성공/종료 시 이벤트(클라이언트 모듈에서 주는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_1_onConnectResultEvt(object sender, ConnectResult e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetButton(this.SetTcpClientButton), new object[] { (byte)CType.client_1, e.bConnect });
                }
                else
                {
                    this.SetTcpClientButton((byte)CType.client_1, e.bConnect);
                }

                if (e.bConnect) //연결 이벤트
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("IP : {0}, Port : {1} 서버로 통신 연결되었습니다.",
                    this.client_1.IPAddr, this.client_1.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("IP : {0}, Port : {1} 서버로 통신 연결되었습니다.",
                            this.client_1.IPAddr, this.client_1.Port));
                    }
                }
                else //해제 이벤트
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("IP : {0}, Port : {1} TCP 연결이 종료되었습니다.",
                        this.client_1.IPAddr, this.client_1.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("IP : {0}, Port : {1} TCP 연결이 종료되었습니다.", this.client_1.IPAddr, this.client_1.Port));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.client_1_onConnectResultEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 클라이언트2 통신 연결 성공/종료 시 이벤트(클라이언트 모듈에서 주는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_2_onConnectResultEvt(object sender, ConnectResult e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetButton(this.SetTcpClientButton), new object[] { (byte)CType.client_2, e.bConnect });
                }
                else
                {
                    this.SetTcpClientButton((byte)CType.client_2, e.bConnect);
                }

                if (e.bConnect) //연결 이벤트
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("IP : {0}, Port : {1} 서버로 통신 연결되었습니다.",
                    this.client_2.IPAddr, this.client_2.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("IP : {0}, Port : {1} 서버로 통신 연결되었습니다.",
                            this.client_2.IPAddr, this.client_2.Port));
                    }
                }
                else //해제 이벤트
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("IP : {0}, Port : {1} TCP 연결이 종료되었습니다.",
                        this.client_2.IPAddr, this.client_2.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("IP : {0}, Port : {1} TCP 연결이 종료되었습니다.", this.client_2.IPAddr, this.client_2.Port));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.client_2_onConnectResultEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// CDMA 서버시작 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_ServerConEvt(object sender, ServerStartEventArgs e)
        {
            try
            {
                bool tmpBool = this.server.NetworkListen(e.Ip, e.Port);

                Settings.Default.ServerIp = e.Ip.ToString();
                Settings.Default.ServerPort = e.Port.ToString();
                Settings.Default.Save();

                if (tmpBool)
                {
                    this.server.onRainMachineRcvEvt += new EventHandler<RainMachineRcv>(server_onRainMachineRcvEvt);
                    this.server.onClientConnectEvt += new EventHandler<ClientConnect>(server_onClientConnectEvt);
                    this.server.onClientDisConnectEvt += new EventHandler<ClientDisConnect>(server_onClientDisConnectEvt);
                    this.server.onRecieveByteArrayEvt += new EventHandler<RecieveByteArray>(server_onRecieveByteArrayEvt);

                    if (this.weatherOptionForm != null)
                    {
                        this.weatherOptionForm.ServerConButton = false;
                        this.weatherOptionForm.ServerConEndButton = true;
                    }

                    this.serverToolstripLB.Image = Resources.DMB_systemGreen;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} / {1} 로 CDMA 서버가 시작되었습니다.", e.Ip, e.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("{0} / {1} 로 CDMA 서버가 시작되었습니다.", e.Ip, e.Port));
                    }
                }
                else
                {
                    if (this.weatherOptionForm != null)
                    {
                        this.weatherOptionForm.ServerConEndButton = false;
                        this.weatherOptionForm.ServerConButton = true;
                    }

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} / {1} 로 CDMA 서버 시작을 실패하였습니다.", e.Ip, e.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("{0} / {1} 로 CDMA 서버 시작을 실패하였습니다.", e.Ip, e.Port));
                    }
                }
            }
            catch (Exception ex)
            {
                this.weatherOptionForm.ServerConEndButton = false;
                this.weatherOptionForm.ServerConButton = true;

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} / {1} 로 CDMA 서버 시작을 실패하였습니다.", e.Ip, e.Port) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("{0} / {1} 로 CDMA 서버 시작을 실패하였습니다.", e.Ip, e.Port));
                }

                Console.WriteLine(string.Format("MainForm.weatherOptionForm_ServerConEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// CDMA 서버종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_ServerStopEvt(object sender, ServerStopEventArgs e)
        {
            try
            {
                this.server.NetworkListenEnd();
                this.server.onRainMachineRcvEvt -= new EventHandler<RainMachineRcv>(server_onRainMachineRcvEvt);
                this.server.onClientConnectEvt -= new EventHandler<ClientConnect>(server_onClientConnectEvt);
                this.server.onClientDisConnectEvt -= new EventHandler<ClientDisConnect>(server_onClientDisConnectEvt);
                this.server.onRecieveByteArrayEvt -= new EventHandler<RecieveByteArray>(server_onRecieveByteArrayEvt);
                this.weatherOptionForm.ServerConEndButton = false;
                this.weatherOptionForm.ServerConButton = true;
                this.serverToolstripLB.Image = Resources.DMB_systemRed;

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { "CDMA 서버가 종료되었습니다." });
                }
                else
                {
                    this.SetRWeatherData("CDMA 서버가 종료되었습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.weatherOptionForm_ServerStopEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 이더넷 서버시작 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_OnEthernetServerConEvt(object sender, ServerStartEventArgs e)
        {
            try
            {
                this.ethernetServer.UseIpAsKey = false;
                bool tmpBool = this.ethernetServer.Start(e.Ip, e.Port);

                Settings.Default.EthernetIp = e.Ip.ToString();
                Settings.Default.EthernetPort = e.Port.ToString();
                Settings.Default.Save();

                if (this.ethernetServer.IsRunning)
                {
                    this.ethernetServer.acceptEvtHandler += new AcceptEvtHandler(ethernetServer_acceptEvtHandler);

                    if (this.weatherOptionForm != null)
                    {
                        this.weatherOptionForm.EthernetServerConBtn = false;
                        this.weatherOptionForm.EthernetServerConEndBtn = true;
                    }

                    this.serverToolstrip_E_LB.Image = Resources.DMB_systemGreen;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} / {1} 로 이더넷 서버가 시작되었습니다.", e.Ip, e.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("{0} / {1} 로 이더넷 서버가 시작되었습니다.", e.Ip, e.Port));
                    }
                }
                else
                {
                    if (this.weatherOptionForm != null)
                    {
                        this.weatherOptionForm.EthernetServerConEndBtn = false;
                        this.weatherOptionForm.EthernetServerConBtn = true;
                    }

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} / {1} 로 이더넷 서버 시작을 실패하였습니다.", e.Ip, e.Port) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("{0} / {1} 로 이더넷 서버 시작을 실패하였습니다.", e.Ip, e.Port));
                    }
                }
            }
            catch (Exception ex)
            {
                this.weatherOptionForm.EthernetServerConEndBtn = false;
                this.weatherOptionForm.EthernetServerConBtn = true;

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0} / {1} 로 이더넷 서버 시작을 실패하였습니다.", e.Ip, e.Port) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("{0} / {1} 로 이더넷 서버 시작을 실패하였습니다.", e.Ip, e.Port));
                }

                Console.WriteLine(string.Format("MainForm.weatherOptionForm_OnEthernetServerConEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 이더넷 서버종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherOptionForm_OnEthernetServerStopEvt(object sender, ServerStopEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dataMng.E_ClientList.Count; i++)
                {
                    WDevice tmpDevice = this.dataMng.GetRatDevice(this.dataMng.E_ClientList[i].ID);

                    if (tmpDevice.EthernetUse)
                    {
                        this.weatherForm.setEthernetState(tmpDevice.PKID, 0);
                    }
                }

                this.dataMng.E_ClientList.Clear();

                for (int i = 0; i < this.EthernetCheckerList.Count; i++)
                {
                    this.EthernetCheckerList[i].Flag = false;
                }

                this.EthernetCheckerList.Clear();

                this.ethernetServer.CloseAllClient();
                this.ethernetServer.Stop();
                this.ethernetServer.acceptEvtHandler -= new AcceptEvtHandler(ethernetServer_acceptEvtHandler);
                this.serverToolstrip_E_LB.Image = Resources.DMB_systemRed;

                if (this.weatherOptionForm != null)
                {
                    this.weatherOptionForm.EthernetServerConEndBtn = false;
                    this.weatherOptionForm.EthernetServerConBtn = true;
                }

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { "이더넷 서버가 종료되었습니다." });
                }
                else
                {
                    this.SetRWeatherData("이더넷 서버가 종료되었습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.weatherOptionForm_OnEthernetServerStopEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 이더넷 TCP 서버 클라이언트 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ethernetServer_acceptEvtHandler(object sender, AdengAcceptEvtArgs e)
        {
            try
            {
                e.ClientSocket.recvEvtHandler += new RecvEvtHandler(ClientSocket_recvEvtHandler);
                e.ClientSocket.closeEvtHandler += new CloseEvtHandler(ClientSocket_closeEvtHandler);

                this.dataMng.E_ClientList.Add(new EthernetClient(e.ClientSocket, string.Empty, string.Empty, string.Empty));

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("이더넷 클라이언트가 연결되었습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("이더넷 클라이언트가 연결되었습니다."));
                }

                //이더넷 통신 폴링 체크
                EthernetChecker eChecker = new EthernetChecker(this.EthernetPollingInterval, e.ClientSocket);
                eChecker.onEthernetStateEvt += new EventHandler<EthernetStateEventArgs>(eChecker_onEthernetStateEvt);
                eChecker.Start();
                this.EthernetCheckerList.Add(eChecker);

#if debug
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("연결! 이더넷클라이언트 - {0}, 폴링체크 리스트 - {1}, 이더넷서버에 리스트 - {2}",
                        this.dataMng.E_ClientList.Count, this.EthernetCheckerList.Count, this.ethernetServer.dicClients.Count) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("연결! 이더넷클라이언트 - {0}, 폴링체크 리스트 - {1}, 이더넷서버에 리스트 - {2}",
                        this.dataMng.E_ClientList.Count, this.EthernetCheckerList.Count, this.ethernetServer.dicClients.Count));
                }
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.ethernetServer_acceptEvtHandler - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 이더넷 폴링 체크 후 '이상'으로 반환되는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eChecker_onEthernetStateEvt(object sender, EthernetStateEventArgs e)
        {
            try
            {
                EthernetChecker tmpChecker = new EthernetChecker();

                for (int i = 0; i < this.EthernetCheckerList.Count; i++)
                {
                    if (this.EthernetCheckerList[i].Client == e.Client)
                    {
                        this.EthernetCheckerList[i].Flag = false;
                        tmpChecker = this.EthernetCheckerList[i];
                    }
                }

                this.EthernetCheckerList.Remove(tmpChecker);
                e.Client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.eChecker_onEthernetStateEvt - {0}", ex.Message));
            }
        }

        /// <summary>
        /// TCP 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientSocket_recvEvtHandler(object sender, AdengRecvEvtArgs e)
        {
            try
            {
                bool isEmpty = false;

                for (int i = 0; i < this.dataMng.E_ClientList.Count; i++)
                {
                    if (this.dataMng.E_ClientList[i].ID == string.Empty)
                    {
                        isEmpty = true;
                        break;
                    }
                }

                if (isEmpty)
                {
                    string strRcvData = Encoding.Default.GetString(e.Buff);
                    string deviceId = string.Empty;
                    char subCmdCode = char.MinValue;

                    if (e.Buff[0] == 0x02 && strRcvData.Substring(1, 3) == "RAT")
                    {
                        deviceId = strRcvData.Substring(7, 13);
                        subCmdCode = char.Parse(strRcvData.Substring(21, 1));

                        for (int i = 0; i < this.dataMng.E_ClientList.Count; i++)
                        {
                            if (this.dataMng.E_ClientList[i].Client == e.ClientSocket)
                            {
                                this.dataMng.E_ClientList[i].ID = deviceId;
                                WDevice tmpWDevice = this.dataMng.GetRatDevice(deviceId);
                                this.dataMng.E_ClientList[i].CellNum = tmpWDevice.CellNumber;

                                if (tmpWDevice.EthernetUse)
                                {
                                    this.weatherForm.setEthernetState(tmpWDevice.PKID, 1);
                                }
                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < this.EthernetCheckerList.Count; i++)
                {
                    if (this.EthernetCheckerList[i].Client == e.ClientSocket)
                    {
                        this.EthernetCheckerList[i].CheckTime = DateTime.Now;
                        this.EthernetCheckerList[i].IsFirst = true;
                    }
                }

                #region Low Data System Log
                string tmpRcvData = Encoding.Default.GetString(e.Buff);
                string tmpRst = tmpRcvData.Substring(21, 1);

                if (tmpRst != "i")
                {
                    EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.Information, "이더넷 - " + tmpRcvData);
                }
                #endregion

                this.parsing.ParsingRcvProtocol(e.Buff);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.ClientSocket_recvEvtHandler - {0}", ex.Message));
            }
        }

        /// <summary>
        /// TCP 연결 해제 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientSocket_closeEvtHandler(object sender, AdengCloseEvtArgs e)
        {
            try
            {
                EthernetClient tmpClient = new EthernetClient();

                for (int i = 0; this.dataMng.E_ClientList.Count > i; i++)
                {
                    if (this.dataMng.E_ClientList[i].Client == e.ClientSocket)
                    {
                        tmpClient = this.dataMng.E_ClientList[i];
                        break;
                    }
                }

                if (tmpClient.Client.Key != string.Empty)
                {
                    this.dataMng.E_ClientList.Remove(tmpClient);
                }

                this.ethernetServer.dicClients.Remove(e.ClientSocket.Key);
                
                EthernetChecker tmpChecker = new EthernetChecker();

                for (int i = 0; i < this.EthernetCheckerList.Count; i++)
                {
                    if (this.EthernetCheckerList[i].Client == e.ClientSocket)
                    {
                        this.EthernetCheckerList[i].Flag = false;
                        tmpChecker = this.EthernetCheckerList[i];
                    }
                }

                this.EthernetCheckerList.Remove(tmpChecker);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("이더넷 클라이언트가 연결 해제되었습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("이더넷 클라이언트가 연결 해제되었습니다."));
                }

                e.ClientSocket.recvEvtHandler -= new RecvEvtHandler(ClientSocket_recvEvtHandler);
                e.ClientSocket.closeEvtHandler -= new CloseEvtHandler(ClientSocket_closeEvtHandler);

                WDevice tmpWDevice = this.dataMng.GetRatDevice(tmpClient.ID);

                if (tmpWDevice.PKID > 0 && tmpWDevice.EthernetUse)
                {
                    this.weatherForm.setEthernetState(tmpWDevice.PKID, 0);
                }

#if debug
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("연결해제! 이더넷클라이언트 - {0}, 폴링체크 리스트 - {1}, 이더넷서버에 리스트 - {2}",
                        this.dataMng.E_ClientList.Count, this.EthernetCheckerList.Count, this.ethernetServer.dicClients.Count) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("연결해제! 이더넷클라이언트 - {0}, 폴링체크 리스트 - {1}, 이더넷서버에 리스트 - {2}",
                        this.dataMng.E_ClientList.Count, this.EthernetCheckerList.Count, this.ethernetServer.dicClients.Count));
                }
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.ClientSocket_closeEvtHandler - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 측기의 데이터를 받아 서버가 파싱해서 주는 데이터 클래스 이벤트(서버 모듈에서 주는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_onRainMachineRcvEvt(object sender, RainMachineRcv e)
        {
            byte[] totProto = new byte[0];
            WDevice mainWDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
            
            if (!this.dataMng.GetWDeviceComparer(mainWDeviceTmp))
            {
                return;
            }

            switch (e.SubCmdCode)
            {
                case 'I': //기상관측 데이터
                    if (e.listDetailData[0] == "1") //우량계
                    {
                        //DB저장
                        WDevice wDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        DateTime datetimeTmp = new DateTime(int.Parse(e.listDetailData[8].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[8].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(12, 2)));
                        WRainData wr = new WRainData(0, wDeviceTmp.PKID, datetimeTmp, e.listDetailData[2], e.listDetailData[3],
                            e.listDetailData[4], e.listDetailData[5], e.listDetailData[6], e.listDetailData[7]);
                        this.dataMng.AddRainData(wr);

                        //송신 프로토콜 조합
                        Proto001 p001 = ProtoMng.GetProtoObj("001") as Proto001;
                        p001.Division = "RAT";
                        p001.ID = e.DiviceID.PadLeft(15, '0');

                        for (int i = 1; i < 9; i++)
                        {
                            p001.Data += e.listDetailData[i];
                        }

                        totProto = p001.MakeProto();
                    }
                    else if (e.listDetailData[0] == "2") //수위계
                    {
                        //DB저장
                        WDevice wDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        DateTime datetimeTmp = new DateTime(int.Parse(e.listDetailData[10].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[10].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[10].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[10].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[10].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[10].Substring(12, 2)));
                        WWaterLevelData wwl = new WWaterLevelData(0, wDeviceTmp.PKID, datetimeTmp, e.listDetailData[2], e.listDetailData[3],
                            e.listDetailData[4], e.listDetailData[5], e.listDetailData[6], e.listDetailData[7], e.listDetailData[8], e.listDetailData[9]);
                        this.dataMng.AddWaterLevelData(wwl);

                        //송신 프로토콜 조합
                        Proto002 p002 = ProtoMng.GetProtoObj("002") as Proto002;
                        p002.Division = "RAT";
                        p002.ID = e.DiviceID.PadLeft(15, '0');
                        p002.Data = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", e.listDetailData[1], e.listDetailData[2], e.listDetailData[3]
                            , e.listDetailData[4], e.listDetailData[5], e.listDetailData[8], e.listDetailData[9], e.listDetailData[10]);
                        totProto = p002.MakeProto();
                    }
                    else if (e.listDetailData[0] == "3") //유속계
                    {
                        //DB 저장
                        WDevice wDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        DateTime datetimeTmp = new DateTime(int.Parse(e.listDetailData[9].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[9].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(12, 2)));
                        WFlowSpeedData wfs = new WFlowSpeedData(0, wDeviceTmp.PKID, datetimeTmp, e.listDetailData[2], e.listDetailData[3], e.listDetailData[4],
                            e.listDetailData[5], e.listDetailData[6], e.listDetailData[7], e.listDetailData[8]);
                        this.dataMng.AddFlowSpeedData(wfs);

                        //송신 프로토콜 조합
                        Proto003 p003 = ProtoMng.GetProtoObj("003") as Proto003;
                        p003.Division = "RAT";
                        p003.ID = e.DiviceID.PadLeft(15, '0');
                        p003.Data = string.Format("{0}{1}{2}{3}{4}{5}", e.listDetailData[1], e.listDetailData[3], e.listDetailData[4]
                            , e.listDetailData[7], e.listDetailData[8], e.listDetailData[9]);
                        totProto = p003.MakeProto();
                    }
                    else if (e.listDetailData[0] == "4") //풍향풍속계(향후 예정)
                    {
                    }
                    
                    string tmpStr = Encoding.Default.GetString(totProto);
                    EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.SuccessAudit, "시리얼 보내기 전 - " + tmpStr);
                    this.SendFromWRS(totProto);
                    EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.SuccessAudit, "시리얼 보내고 바로 - " + tmpStr);
                    
                    break;

                case 'e': //임계치 데이터
                    //DB저장
                    WDevice wAlarmDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    DateTime wAlarmDatetimeTmp = new DateTime(int.Parse(e.listDetailData[5].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[5].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[5].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[5].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[5].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[5].Substring(12, 2)));
                    WAlarmData wa = new WAlarmData(0, wAlarmDeviceTmp.PKID, byte.Parse(e.listDetailData[0]), wAlarmDatetimeTmp,
                        byte.Parse(e.listDetailData[1]), byte.Parse(e.listDetailData[3]), e.listDetailData[2]);
                    this.dataMng.AddAlarmData(wa);

                    //팝업 UI
                    bool real = false;
                    
                    if (e.listDetailData[0] == "0")
                    {
                        real = true;
                    }

                    string deviceId = string.Format("{0}({1})", wAlarmDeviceTmp.Name, e.DiviceID);
                    string level = (e.listDetailData[1] == "1") ? "우량계" :
                                        (e.listDetailData[1] == "2") ? "수위계" :
                                        (e.listDetailData[1] == "3") ? "유속계" :
                                        (e.listDetailData[1] == "4") ? "풍향풍속계" : "기타";
                    level += (e.listDetailData[3] == "0") ? ", 해제" :
                                (e.listDetailData[3] == "1") ? ", 주의(1단계)" :
                                (e.listDetailData[3] == "2") ? ", 경계(2단계)" :
                                (e.listDetailData[3] == "3") ? ", 대피(3단계)" : ", 기타";
                    double tmpdata = (e.listDetailData[1] == "2") ? double.Parse(e.listDetailData[2]) * 0.01 : double.Parse(e.listDetailData[2]) * 0.1;
                    string data = (e.listDetailData[1] == "1") ? string.Format("현재 {0} (mm)", tmpdata) :
                                    (e.listDetailData[1] == "2") ? string.Format("현재 {0} (meter)", tmpdata) : string.Format("현재 {0} (m/s)", tmpdata);
                    DateTime dt = new DateTime(int.Parse(e.listDetailData[5].Substring(0, 4)),
                                                int.Parse(e.listDetailData[5].Substring(4, 2)),
                                                int.Parse(e.listDetailData[5].Substring(6, 2)),
                                                int.Parse(e.listDetailData[5].Substring(8, 2)),
                                                int.Parse(e.listDetailData[5].Substring(10, 2)),
                                                int.Parse(e.listDetailData[5].Substring(12, 2)));
                    this.aStruct.Real = real;
                    this.aStruct.ID = deviceId;
                    this.aStruct.Level = level;
                    this.aStruct.Data = data;
                    this.aStruct.DDTime = dt;

                    this.AlarmPopTD = new Thread(new ThreadStart(this.PopUpThread));
                    this.AlarmPopTD.IsBackground = true;
                    this.AlarmPopTD.Start();

                    //송신 프로토콜 조합
                    if (Settings.Default.DataAlarmCB) //체크 On
                    {
                        Proto100 p100 = ProtoMng.GetProtoObj("100") as Proto100;
                        p100.Division = "RAT";
                        p100.ID = e.DiviceID.PadLeft(15, '0');
                        p100.Data = e.listDetailData[0] + e.listDetailData[1] + e.listDetailData[3] + e.listDetailData[2] + e.listDetailData[5];
                        totProto = p100.MakeProto();

                        this.SendFromWRS(totProto);
                    }

                    //측기 담당자에게 SMS 송신
                    List<WSmsUser> tmpSmsUserList = this.dataMng.getSmsUserList(wAlarmDeviceTmp.PKID);
                    string smsUserMsg = string.Format("[{0} 임계치알람 발생] {5}시{6}분 {1} {2} {3} {4} 발생!",
                        (real == true) ? "실제" : "시험",
                        wAlarmDeviceTmp.Name,
                        (e.listDetailData[1] == "1") ? "강우" :
                                (e.listDetailData[1] == "2") ? "수위" :
                                (e.listDetailData[1] == "3") ? "유속" :
                                (e.listDetailData[1] == "4") ? "풍향풍속" : "기타",
                        (e.listDetailData[3] == "0") ? "해제" :
                                (e.listDetailData[3] == "1") ? "주의(1단계)" :
                                (e.listDetailData[3] == "2") ? "경계(2단계)" :
                                (e.listDetailData[3] == "3") ? "대피(3단계)" : ", 기타",
                        (e.listDetailData[1] == "1") ? string.Format("현재 {0}mm", tmpdata) :
                                    (e.listDetailData[1] == "2") ? string.Format("현재 {0}m", tmpdata) :
                                    string.Format("현재 {0}m/s", tmpdata),
                        dt.Hour, dt.Minute);

                    for (int i = 0; i < tmpSmsUserList.Count; i++)
                    {
                        if (e.listDetailData[3] == "1")
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList[i].PKID, WeatherDataMng.SMSType.임계치1단계).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, smsUserMsg);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg));
                            }
                        }
                        else if (e.listDetailData[3] == "2")
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList[i].PKID, WeatherDataMng.SMSType.임계치2단계).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, smsUserMsg);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg));
                            }
                        }
                        else if (e.listDetailData[3] == "3")
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList[i].PKID, WeatherDataMng.SMSType.임계치3단계).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, smsUserMsg);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg));
                            }
                        }
                    }
                    break;

                case 'O': //옵션의 자가진단 결과(각 센서 상태)
                    //DB 저장
                    WDevice wSensorStateTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiType = WeatherDataMng.WIType.NotUse;
                    
                    if (e.listDetailData[0] == "1")
                    {
                        wiType = WeatherDataMng.WIType.강수센서상태;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 강수센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 강수센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "2")
                    {
                        wiType = WeatherDataMng.WIType.수위센서상태;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 수위센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 수위센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "3")
                    {
                        wiType = WeatherDataMng.WIType.유속센서상태;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 유속센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 유속센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "4")
                    {
                        wiType = WeatherDataMng.WIType.태양전지;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 태양전지센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 태양전지센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }

                    if (wiType != WeatherDataMng.WIType.NotUse)
                    {
                        WTypeDeviceItem tmpTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiType);
                        WDeviceItem wdi = new WDeviceItem(0, wSensorStateTmp.PKID, tmpTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdi);

                        WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp.PKID, tmpTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceAlarmItems(wdi1);
                    }

                    //송신 프로토콜 조합
                    if (Settings.Default.DataSelfCheckCB) //체크 On
                    {
                        Proto010 p010 = ProtoMng.GetProtoObj("010") as Proto010;
                        p010.Division = "RAT";
                        p010.ID = e.DiviceID.PadLeft(15, '0');
                        p010.Data = e.listDetailData[0] + e.listDetailData[1];
                        totProto = p010.MakeProto();

                        this.SendFromWRS(totProto);
                    }

                    //팝업 UI
                    string wDeviceAlarmId = string.Format("{0}({1})", wSensorStateTmp.Name, e.DiviceID);
                    string wDeviceAlarmData = string.Empty;
                    DateTime wDeviceAlarmDt = DateTime.Now;

                    if (e.listDetailData[0] == "1")
                    {
                        wDeviceAlarmData = string.Format("우량계 {0}",
                            (e.listDetailData[1] == "0") ? string.Format("정상") :
                            (e.listDetailData[1] == "1") ? string.Format("이상") :
                            (e.listDetailData[1] == "2") ? string.Format("이상(합선)") : string.Format("이상(단선)"));
                    }
                    else if (e.listDetailData[0] == "2")
                    {
                        wDeviceAlarmData = string.Format("수위계 {0}", (e.listDetailData[1] == "0") ? string.Format("정상") : string.Format("이상"));
                    }
                    else if (e.listDetailData[0] == "3")
                    {
                        wDeviceAlarmData = string.Format("유속계 {0}", (e.listDetailData[1] == "0") ? string.Format("정상") : string.Format("이상"));
                    }
                    else if (e.listDetailData[0] == "4")
                    {
                        wDeviceAlarmData = string.Format("태양전지 {0}", (e.listDetailData[1] == "0") ? string.Format("정상") : string.Format("저전압"));
                    }

                    this.daStruct.ID = wDeviceAlarmId;
                    this.daStruct.Data = wDeviceAlarmData;
                    this.daStruct.DDTime = wDeviceAlarmDt;

                    this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                    this.deviceAlarmTD.IsBackground = true;
                    this.deviceAlarmTD.Start();

                    //SMS 송신
                    if (e.listDetailData[1] == "1" || e.listDetailData[1] == "2" || e.listDetailData[1] == "3")
                    {
                        List<WSmsUser> tmpSmsUserList1 = this.dataMng.getSmsUserList(wSensorStateTmp.PKID);
                        string smsUserMsg1 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                            wDeviceAlarmDt.Hour,
                            wDeviceAlarmDt.Minute,
                            wSensorStateTmp.Name,
                            (e.listDetailData[0] == "1") ? "우량계" :
                                    (e.listDetailData[0] == "2") ? "수위계" :
                                    (e.listDetailData[0] == "3") ? "유속계" : "태양전지",
                            (e.listDetailData[1] == "0") ? "정상" :
                                    (e.listDetailData[1] == "1") ? "이상" :
                                    (e.listDetailData[1] == "2") ? "이상(합선)" : "이상(단선)");

                        for (int i = 0; i < tmpSmsUserList1.Count; i++)
                        {
                            if (this.dataMng.getMapSmsItem(tmpSmsUserList1[i].PKID, WeatherDataMng.SMSType.센서상태).IsUse)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList1[i].PKID, wDeviceAlarmDt, smsUserMsg1);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                                this.dataMng.SendSmsUserMsg(tmpSmsUserList1[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg1));
                            }
                        }
                    }
                    break;

                case 'V': //옵션의 펌웨어 버전
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 펌웨어 버전 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 펌웨어 버전 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wFWVerTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeFWVer = WeatherDataMng.WIType.펌웨어버전;
                    WTypeDeviceItem FWVerTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFWVer);
                    WDeviceItem wdiFWVer = new WDeviceItem(0, wFWVerTmp.PKID, FWVerTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiFWVer);
                    
                    //송신 프로토콜 조합
                    if (Settings.Default.DataFWVerCB) //체크 On
                    {
                        //현재 펌웨어 버전은 보내지 않음(추후 변경 가능)
                    }
                    break;

                case 'B': //옵션의 배터리 상태
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 배터리 상태 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 배터리 상태 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    if (e.listDetailData[0] == "1")
                    {
                        WDevice wBattTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        WeatherDataMng.WIType wiTypeBatt = WeatherDataMng.WIType.배터리전압;
                        WTypeDeviceItem BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        WDeviceItem wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리전류;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리저항;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리온도;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[4]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리수명;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[6]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리상태;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[7]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);
                    }
                    else if (e.listDetailData[0] == "2")
                    {
                        WDevice wBattTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        WeatherDataMng.WIType wiTypeBatt = WeatherDataMng.WIType.배터리2전압;
                        WTypeDeviceItem BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        WDeviceItem wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리2전류;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리2저항;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리2온도;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[4]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리2수명;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[6]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);

                        wiTypeBatt = WeatherDataMng.WIType.배터리2상태;
                        BattTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeBatt);
                        wdiBatt = new WDeviceItem(0, wBattTmp.PKID, BattTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[7]);
                        this.dataMng.AddDeviceItemData(wdiBatt);
                        Thread.Sleep(20);
                    }

                    //송신 프로토콜 조합
                    if (Settings.Default.DataBattCB) //체크 On
                    {
                        //현재 배터리 상태는 보내지 않음(추후 변경 가능)
                    }
                    break;

                case 'S': //옵션의 태양전지 상태
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 태양전지 상태 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 태양전지 상태 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wSunTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeSun = WeatherDataMng.WIType.태양전지;
                    WTypeDeviceItem SunTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeSun);
                    WDeviceItem wdiSun = new WDeviceItem(0, wSunTmp.PKID, SunTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                    this.dataMng.AddDeviceItemData(wdiSun);

                    //송신 프로토콜 조합
                    if (Settings.Default.DataSolarCB) //체크 On
                    {
                        //현재 태양전지 상태는 보내지 않음(추후 변경 가능)
                    }
                    break;

                case 'd': //옵션의 도어 상태
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 도어 상태 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 도어 상태 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wDoorTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeDoor = WeatherDataMng.WIType.DOOR;
                    WTypeDeviceItem DoorTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeDoor);
                    WDeviceItem wdiDoor = new WDeviceItem(0, wDoorTmp.PKID, DoorTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiDoor);

                    //송신 프로토콜 조합
                    if (Settings.Default.DataDoorCB) //체크 On
                    {
                        //현재 도어 상태는 보내지 않음(추후 변경 가능)
                    }
                    break;

                case 'W': //기상관측센서 상태 정보
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 센서 사용여부 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 센서 사용여부 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wSUseTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeSUse = WeatherDataMng.WIType.강수센서사용여부;
                    WTypeDeviceItem SUseTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeSUse);
                    WDeviceItem wdiSUse = new WDeviceItem(0, wSUseTmp.PKID, SUseTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiSUse);
                    Thread.Sleep(20);

                    wiTypeSUse = WeatherDataMng.WIType.수위센서사용여부;
                    SUseTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeSUse);
                    wdiSUse = new WDeviceItem(0, wSUseTmp.PKID, SUseTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                    this.dataMng.AddDeviceItemData(wdiSUse);
                    Thread.Sleep(20);

                    wiTypeSUse = WeatherDataMng.WIType.유속센서사용여부;
                    SUseTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeSUse);
                    wdiSUse = new WDeviceItem(0, wSUseTmp.PKID, SUseTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                    this.dataMng.AddDeviceItemData(wdiSUse);
                    break;
                
                case 's': //무시시간 정보
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 무시시간 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 무시시간 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wFTimeTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeFTime = WeatherDataMng.WIType.동일레벨무시시간;
                    WTypeDeviceItem FTimeTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFTime);
                    WDeviceItem wdiFTime = new WDeviceItem(0, wFTimeTmp.PKID, FTimeTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[4]);
                    this.dataMng.AddDeviceItemData(wdiFTime);
                    Thread.Sleep(20);

                    wiTypeFTime = WeatherDataMng.WIType.하향레벨무시시간;
                    FTimeTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFTime);
                    wdiFTime = new WDeviceItem(0, wFTimeTmp.PKID, FTimeTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[5]);
                    this.dataMng.AddDeviceItemData(wdiFTime);
                    Thread.Sleep(20);

                    //송신 프로토콜 조합
                    Proto012 p012 = ProtoMng.GetProtoObj("012") as Proto012;
                    p012.Division = "RAT";
                    p012.ID = e.DiviceID.PadLeft(15, '0');
                    p012.Data = e.listDetailData[4] + e.listDetailData[5];
                    totProto = p012.MakeProto();
                    this.SendFromWRS(totProto);
                    break;
                
                case 'F': //FAN
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 FAN 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 FAN 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wFanTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeFan = WeatherDataMng.WIType.FAN;
                    WTypeDeviceItem FanTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeFan);
                    WDeviceItem wdiFan = new WDeviceItem(0, wFanTmp.PKID, FanTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiFan);
                    break;
                
                case 'L': //임계치 정보 전송
                    //DB 저장
                    WDevice wAlarmTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeAlarm = WeatherDataMng.WIType.강수임계치1단계;
                    WTypeDeviceItem AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                    WDeviceItem wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);

                    if (e.listDetailData[0] == "1")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.강수임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.강수임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.강수임계치3단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 강수 임계치 정보 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 강수 임계치 정보 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "2")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.수위임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.수위임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.수위임계치3단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 수위 임계치 정보 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 수위 임계치 정보 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "3")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.유속임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.유속임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.유속임계치3단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[3]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 유속 임계치 정보 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 유속 임계치 정보 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "4")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.배터리임계치1단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.배터리임계치2단계;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 배터리 임계치 정보 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 배터리 임계치 정보 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "5")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.함체FAN동작임계치;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 함체 FAN 임계치 정보 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 함체 FAN 임계치 정보 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }
                    else if (e.listDetailData[0] == "6")
                    {
                        //배터리 함체 없음
                    }
                    else if (e.listDetailData[0] == "7")
                    {
                        wiTypeAlarm = WeatherDataMng.WIType.태양전지1차;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        wiTypeAlarm = WeatherDataMng.WIType.태양전지2차;
                        AlarmTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAlarm);
                        wdiAlarm = new WDeviceItem(0, wAlarmTmp.PKID, AlarmTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[2]);
                        this.dataMng.AddDeviceItemData(wdiAlarm);
                        Thread.Sleep(20);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 태양전지 임계치 정보 데이터가 수신되었습니다.", e.DiviceID) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 태양전지 임계치 정보 데이터가 수신되었습니다.", e.DiviceID));
                        }
                    }

                    //송신 프로토콜 조합
                    Proto011 p011 = ProtoMng.GetProtoObj("011") as Proto011;
                    p011.Division = "RAT";
                    p011.ID = e.DiviceID.PadLeft(15, '0');
                    p011.Data = e.listDetailData[0] + e.listDetailData[1] + e.listDetailData[2] + e.listDetailData[3];
                    totProto = p011.MakeProto();
                    this.SendFromWRS(totProto);
                    break;
                
                case 'A': //안테나 감도
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 안테나 감도 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 안테나 감도 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wRssiTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeRssi = WeatherDataMng.WIType.CDMA감도;
                    WTypeDeviceItem RssiTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeRssi);
                    WDeviceItem wdiRssi = new WDeviceItem(0, wRssiTmp.PKID, RssiTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiRssi);
                    break;
                
                case 'E': //이벤트 로그(추후 적용)
                    break;
                
                case 'C': //통신상태
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 통신 상태 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 통신 상태 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wComTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeCom = WeatherDataMng.WIType.RAT통신상태;
                    WTypeDeviceItem ComTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeCom);
                    WDeviceItem wdiCom = new WDeviceItem(0, wComTmp.PKID, ComTypeDeviceItem.PKID, DateTime.Now, "1");
                    this.dataMng.AddDeviceItemData(wdiCom);
                    break;
                
                case 'r': //AS 보고
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 A/S 보고 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 A/S 보고 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wASTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeAS = WeatherDataMng.WIType.AS보고;
                    WTypeDeviceItem ASTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAS);
                    WDeviceItem wdiAS = new WDeviceItem(0, wASTmp.PKID, ASTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiAS);
                    break;
                
                case 'f': //알람 보고
                    bool tmpBool = true; //같은 변수 사용을 위한 변수

                    switch (e.listDetailData[0])
                    {
                        case "000":
                        case "001":
                        case "002":
                        case "003":
                        case "004":
                        case "005":
                        case "006":
                        case "007":
                        case "008":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.NotUse;

                                if (e.listDetailData[0] == "1")
                                {
                                    wiType1 = WeatherDataMng.WIType.강수센서상태;

                                    if (this.InvokeRequired)
                                    {
                                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 강수센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                                    }
                                    else
                                    {
                                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 강수센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                                    }
                                }
                                else if (e.listDetailData[0] == "2")
                                {
                                    wiType1 = WeatherDataMng.WIType.수위센서상태;

                                    if (this.InvokeRequired)
                                    {
                                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 수위센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                                    }
                                    else
                                    {
                                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 수위센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                                    }
                                }
                                else if (e.listDetailData[0] == "3")
                                {
                                    wiType1 = WeatherDataMng.WIType.유속센서상태;

                                    if (this.InvokeRequired)
                                    {
                                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 유속센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                                    }
                                    else
                                    {
                                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 유속센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                                    }
                                }
                                else if (e.listDetailData[0] == "4")
                                {
                                    wiType1 = WeatherDataMng.WIType.태양전지;

                                    if (this.InvokeRequired)
                                    {
                                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 태양전지센서 상태 데이터가 수신되었습니다.", e.DiviceID) });
                                    }
                                    else
                                    {
                                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 태양전지센서 상태 데이터가 수신되었습니다.", e.DiviceID));
                                    }
                                }

                                if (wiType1 != WeatherDataMng.WIType.NotUse)
                                {
                                    WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                    WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, e.listDetailData[1]);
                                    this.dataMng.AddDeviceAlarmItems(wdi1);
                                }

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = string.Empty;
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                if (e.listDetailData[0] == "1")
                                {
                                    wDeviceAlarmData1 = string.Format("우량계 {0}",
                                        (e.listDetailData[1] == "0") ? string.Format("정상") :
                                        (e.listDetailData[1] == "1") ? string.Format("이상") :
                                        (e.listDetailData[1] == "2") ? string.Format("이상(합선)") : string.Format("이상(단선)"));
                                }
                                else if (e.listDetailData[0] == "2")
                                {
                                    wDeviceAlarmData1 = string.Format("수위계 {0}", (e.listDetailData[1] == "0") ? string.Format("정상") : string.Format("이상"));
                                }
                                else if (e.listDetailData[0] == "3")
                                {
                                    wDeviceAlarmData1 = string.Format("유속계 {0}", (e.listDetailData[1] == "0") ? string.Format("정상") : string.Format("이상"));
                                }
                                else if (e.listDetailData[0] == "4")
                                {
                                    wDeviceAlarmData1 = string.Format("태양전지 {0}", (e.listDetailData[1] == "0") ? string.Format("정상") : string.Format("저전압"));
                                }

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                if (e.listDetailData[1] == "1" || e.listDetailData[1] == "2" || e.listDetailData[1] == "3")
                                {
                                    List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                    string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                        wDeviceAlarmDt1.Hour,
                                        wDeviceAlarmDt1.Minute,
                                        wSensorStateTmp1.Name,
                                        (e.listDetailData[0] == "1") ? "우량계" :
                                                (e.listDetailData[0] == "2") ? "수위계" :
                                                (e.listDetailData[0] == "3") ? "유속계" : "태양전지",
                                        (e.listDetailData[1] == "0") ? "정상" :
                                                (e.listDetailData[1] == "1") ? "이상" :
                                                (e.listDetailData[1] == "2") ? "이상(합선)" : "이상(단선)");

                                    for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                    {
                                        if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.센서상태).IsUse)
                                        {
                                            WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                            this.dataMng.AddSmsSend(tmpSmsSend1);
                                            this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                        }
                                    }
                                }
                            }
                            break;

                        case "009":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1전압상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 전압 정상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "010":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1전압상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 전압 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리1 전압",
                                    " 이상");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리1전압이상).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "011":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1온도상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 온도 정상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "012":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1온도상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 온도 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리1 온도",
                                    " 이상");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리1온도이상).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "013":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1점검시기;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 점검 시기";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리1 점검시기",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리1점검시기).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "014":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1교체시기;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 교체 시기";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리1 교체시기",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리1교체시기).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "015":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리1교체초기화;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리1 교체(초기화)";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리1 교체(초기화)",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리1교체초기화).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "016":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2전압상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 전압 정상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "017":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2전압상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 전압 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리2 전압",
                                    " 이상");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리2전압이상).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "018":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2온도상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 온도 정상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "019":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2온도상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 온도 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리2 온도",
                                    " 이상");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리2온도이상).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "020":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2점검시기;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 점검 시기";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리2 점검시기",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리2점검시기).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "021":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2교체시기;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 교체 시기";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리2 교체시기",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리2교체시기).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "022":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리2교체초기화;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리2 교체(초기화)";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "배터리2 교체(초기화)",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.배터리2교체초기화).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "023":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.AC전압입력;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "AC 전압 입력";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "024":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.AC전압입력;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "AC 전압 미입력";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "025":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.태양전지전압입력;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "태양전지 전압 입력";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "026":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.태양전지전압입력;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "태양전지 전압 미입력";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "029":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리충전상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리 충전 중";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "030":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리충전상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리 만충";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "033":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.FAN;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "2");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "RAT FAN 정상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "034":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.FAN;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "RAT FAN 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "RAT FAN 이상",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.FAN이상).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "037":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.CDMA감도낮음;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, string.Empty);
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "CDMA-RSSI 감도 낮음";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "041":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.CDMA시간설정이상;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "CDMA 시간 설정 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();

                                //SMS 송신
                                List<WSmsUser> tmpSmsUserList2 = this.dataMng.getSmsUserList(wSensorStateTmp1.PKID);
                                string smsUserMsg2 = string.Format("[측기알람 발생] {0}시{1}분 {2} {3} {4}알람 발생!",
                                    wDeviceAlarmDt1.Hour,
                                    wDeviceAlarmDt1.Minute,
                                    wSensorStateTmp1.Name,
                                    "CDMA 시간설정 이상",
                                    " ");

                                for (int i = 0; i < tmpSmsUserList2.Count; i++)
                                {
                                    if (this.dataMng.getMapSmsItem(tmpSmsUserList2[i].PKID, WeatherDataMng.SMSType.CDMA시간설정이상).IsUse)
                                    {
                                        WSmsSend tmpSmsSend1 = new WSmsSend(0, tmpSmsUserList2[i].PKID, wDeviceAlarmDt1, smsUserMsg2);
                                        this.dataMng.AddSmsSend(tmpSmsSend1);
                                        this.dataMng.SendSmsUserMsg(tmpSmsUserList2[i].TelNum.Replace("-", ""), Encoding.Default.GetBytes(smsUserMsg2));
                                    }
                                }
                            }
                            break;

                        case "047":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리감지센서통신상태;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리 감지센서 통신 이상";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "048":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.DOOR;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "도어 열림";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "049":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.DOOR;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "도어 닫힘";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "056":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.우량계데이터감지;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "우량 감지";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "057":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.우량계데이터감지;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "우량 미감지";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "058":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.수위계데이터감지;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "수위 감지";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "059":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.수위계데이터감지;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "수위 미감지";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "060":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.유속계데이터감지;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "유속 감지";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "061":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.유속계데이터감지;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "유속 미감지";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "062":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리사용여부;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "0");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리 사용 중";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;

                        case "063":
                            if (tmpBool)
                            {
                                //DB 저장
                                WDevice wSensorStateTmp1 = this.dataMng.GetRatDevice(e.DiviceID);
                                WeatherDataMng.WIType wiType1 = WeatherDataMng.WIType.배터리사용여부;
                                WTypeDeviceItem tmpTypeDeviceItem1 = this.dataMng.GetTypeDeviceItem(wiType1);
                                WDeviceAlarmItems wdi1 = new WDeviceAlarmItems(0, wSensorStateTmp1.PKID, tmpTypeDeviceItem1.PKID, DateTime.Now, "1");
                                this.dataMng.AddDeviceAlarmItems(wdi1);

                                //팝업 UI
                                string wDeviceAlarmId1 = string.Format("{0}({1})", wSensorStateTmp1.Name, e.DiviceID);
                                string wDeviceAlarmData1 = "배터리 미사용 중";
                                DateTime wDeviceAlarmDt1 = DateTime.Now;

                                this.daStruct.ID = wDeviceAlarmId1;
                                this.daStruct.Data = wDeviceAlarmData1;
                                this.daStruct.DDTime = wDeviceAlarmDt1;

                                this.deviceAlarmTD = new Thread(new ThreadStart(this.DevicePopUpThread));
                                this.deviceAlarmTD.IsBackground = true;
                                this.deviceAlarmTD.Start();
                            }
                            break;
                    }
                    break;
                
                case 'g': //CDMA IP, PORT
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 CDMA IP/PORT 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 CDMA IP/PORT 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wIPTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeIP = WeatherDataMng.WIType.IP;
                    WTypeDeviceItem IPTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeIP);
                    WDeviceItem wdiIP = new WDeviceItem(0, wIPTmp.PKID, IPTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiIP);
                    Thread.Sleep(20);

                    wiTypeIP = WeatherDataMng.WIType.PORT;
                    IPTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeIP);
                    wdiIP = new WDeviceItem(0, wIPTmp.PKID, IPTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                    this.dataMng.AddDeviceItemData(wdiIP);
                    break;

                case 'h': //이더넷 IP, PORT
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP : RAT의 {0} 에서 이더넷 IP/PORT 데이터가 수신되었습니다.", e.DiviceID) });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("TCP : RAT의 {0} 에서 이더넷 IP/PORT 데이터가 수신되었습니다.", e.DiviceID));
                    }

                    //DB 저장
                    WDevice wEIPTmp = this.dataMng.GetRatDevice(e.DiviceID);
                    WeatherDataMng.WIType wiTypeEIP = WeatherDataMng.WIType.이더넷IP;
                    WTypeDeviceItem EIPTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeEIP);
                    WDeviceItem wdiEIP = new WDeviceItem(0, wEIPTmp.PKID, EIPTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[0]);
                    this.dataMng.AddDeviceItemData(wdiEIP);
                    Thread.Sleep(20);

                    wiTypeEIP = WeatherDataMng.WIType.이더넷PORT;
                    EIPTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeEIP);
                    wdiEIP = new WDeviceItem(0, wEIPTmp.PKID, EIPTypeDeviceItem.PKID, DateTime.Now, e.listDetailData[1]);
                    this.dataMng.AddDeviceItemData(wdiEIP);
                    break;

                case 'o': //이벤트 로그 개수(추후 적용)
                    break;

                case 'R': //기상 데이터 요청에 의해 들어오는 데이터
                    if (e.listDetailData[0] == "1") //우량계
                    {
                        //DB저장
                        WDevice wDeviceRTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        DateTime datetimeRTmp = new DateTime(int.Parse(e.listDetailData[6].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[6].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[6].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[6].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[6].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[6].Substring(12, 2)));
                        WRainData wrR = new WRainData(0, wDeviceRTmp.PKID, datetimeRTmp, "00000", e.listDetailData[1],
                            e.listDetailData[2], e.listDetailData[3], e.listDetailData[4], e.listDetailData[5]);
                        this.dataMng.AddRainData(wrR);

                        //송신 프로토콜 조합
                        Proto001 p001 = ProtoMng.GetProtoObj("001") as Proto001;
                        p001.Division = "RAT";
                        p001.ID = e.DiviceID.PadLeft(15, '0');
                        p001.Data = string.Format("01000000{0}{1}{2}{3}{4}{5}", e.listDetailData[1], e.listDetailData[2], e.listDetailData[3],
                                                    e.listDetailData[4], e.listDetailData[5], e.listDetailData[6]);
                        totProto = p001.MakeProto();
                    }
                    else if (e.listDetailData[0] == "2") //수위계
                    {
                        //DB저장
                        WDevice wDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        DateTime datetimeTmp = new DateTime(int.Parse(e.listDetailData[9].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[9].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[9].Substring(12, 2)));
                        WWaterLevelData wwl = new WWaterLevelData(0, wDeviceTmp.PKID, datetimeTmp, e.listDetailData[1], e.listDetailData[2],
                            e.listDetailData[3], e.listDetailData[4], e.listDetailData[5], e.listDetailData[6], e.listDetailData[7], e.listDetailData[8]);
                        this.dataMng.AddWaterLevelData(wwl);

                        //송신 프로토콜 조합
                        Proto002 p002 = ProtoMng.GetProtoObj("002") as Proto002;
                        p002.Division = "RAT";
                        p002.ID = e.DiviceID.PadLeft(15, '0');
                        p002.Data = string.Format("010{0}{1}{2}{3}{4}{5}{6}"
                            , e.listDetailData[1], e.listDetailData[2], e.listDetailData[3], e.listDetailData[4]
                            , e.listDetailData[7], e.listDetailData[8], e.listDetailData[9]);
                        totProto = p002.MakeProto();
                    }
                    else if (e.listDetailData[0] == "3") //유속계
                    {
                        //DB 저장
                        WDevice wDeviceTmp = this.dataMng.GetRatDevice(e.DiviceID);
                        DateTime datetimeTmp = new DateTime(int.Parse(e.listDetailData[8].Substring(0, 4)),
                                                            int.Parse(e.listDetailData[8].Substring(4, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(6, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(8, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(10, 2)),
                                                            int.Parse(e.listDetailData[8].Substring(12, 2)));
                        WFlowSpeedData wfs = new WFlowSpeedData(0, wDeviceTmp.PKID, datetimeTmp, e.listDetailData[1], e.listDetailData[2], e.listDetailData[3],
                            e.listDetailData[4], e.listDetailData[5], e.listDetailData[6], e.listDetailData[7]);
                        this.dataMng.AddFlowSpeedData(wfs);

                        //송신 프로토콜 조합
                        Proto003 p003 = ProtoMng.GetProtoObj("003") as Proto003;
                        p003.Division = "RAT";
                        p003.ID = e.DiviceID.PadLeft(15, '0');
                        p003.Data = string.Format("010{0}{1}{2}{3}{4}", e.listDetailData[2], e.listDetailData[3]
                            , e.listDetailData[6], e.listDetailData[7], e.listDetailData[8]);
                        totProto = p003.MakeProto();
                    }
                    else if (e.listDetailData[0] == "4") //풍향풍속계(향후 예정)
                    {
                    }

#if debug
                    string tmpStr = Encoding.Default.GetString(totProto);
                    EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.SuccessAudit, "시리얼 보내기 전 - " + tmpStr);
                    this.SendFromWRS(totProto);
                    EventLogMng.WriteLog("WeatherRSystem", System.Diagnostics.EventLogEntryType.SuccessAudit, "시리얼 보내고 바로 - " + tmpStr);
#endif

                    break;

                case 'i':
                    if (mainWDeviceTmp.EthernetUse)
                    {
                        CProto08 cProto08 = CProtoMng.GetProtoObj("08") as CProto08;
                        cProto08.Header = "[";
                        cProto08.Length = "021";
                        cProto08.ID = mainWDeviceTmp.ID;
                        cProto08.MainCode = "5";
                        cProto08.SubCode = "i";
                        cProto08.RecvType = "0";
                        cProto08.CRC = "00000";
                        cProto08.Tail = "]";
                        byte[] buff = cProto08.MakeProto();

                        EthernetClient tmp = this.getEthernetClient(mainWDeviceTmp.ID);

                        if (tmp.Client.Key != string.Empty)
                        {
                            tmp.Client.Send(buff, buff.Length);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        //옵션창에서 주는 tcp 연결 이벤트
        private void weatherOptionForm_ClientConEvt(object sender, TcpClientConEventArgs e)
        {
            try
            {
                if (e.Num == (byte)CType.client_1)
                {
                    this.client_1.Connect(e.IP, e.Port);

                    Settings.Default.Tcp1Ip = e.IP;
                    Settings.Default.Tcp1Port = e.Port.ToString();
                    Settings.Default.Tcp1IsUse = true;
                    Settings.Default.Save();
                }
                else if (e.Num == (byte)CType.client_2)
                {
                    this.client_2.Connect(e.IP, e.Port);

                    Settings.Default.Tcp2Ip = e.IP;
                    Settings.Default.Tcp2Port = e.Port.ToString();
                    Settings.Default.Tcp2IsUse = true;
                    Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("TCP 연결을 실패하였습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("TCP 연결을 실패하였습니다."));
                }

                Console.WriteLine(string.Format("MainForm.weatherOptionForm_ClientConEvt() - {0}", ex.Message));
            }
        }

        //옵션창에서 주는 tcp 연결 종료 이벤트
        private void weatherOptionForm_ClientConEndEvt(object sender, TcpClientConEndEventArgs e)
        {
            if (e.Num == (byte)CType.client_1)
            {
                this.client_1.TcpClose();
            }
            else if (e.Num == (byte)CType.client_2)
            {
                this.client_2.TcpClose();
            }
        }

        //옵션창에서 주는 Serial 연결 이벤트
        private void weatherOptionForm_SerialConEvt(object sender, SerialConEventArgs e)
        {
            try
            {
                Parity parity = (e.Parity == "홀수") ? Parity.Odd : (e.Parity == "짝수") ? Parity.Even : Parity.None;
                StopBits stopBit = (e.StopBit == "2") ? StopBits.Two : (e.StopBit == "1.5") ? StopBits.OnePointFive : StopBits.One;

                if (e.Num == (byte)SType.client_1)
                {
                    this.serial_1 = WeatherSerial.getInstance();

                    if (this.serial_1.Open(e.ComPort, int.Parse(e.BaudRate), int.Parse(e.DataBits), parity, stopBit))
                    {
                        this.serial_1.onSerialRecvDataEvt += new EventHandler<SerialReceiveDataEvt>(serial_1_onSerialRecvDataEvt);

                        if (this.weatherOptionForm != null)
                        {
                            this.weatherOptionForm.SerialCon1Button = false;
                            this.weatherOptionForm.SerialConEnd1Button = true;
                        }

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial을 연결하였습니다.",
                            e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial을 연결하였습니다.",
                                e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit));
                        }
                    }
                    else
                    {
                        if (this.weatherOptionForm != null)
                        {
                            this.weatherOptionForm.SerialConEnd1Button = false;
                            this.weatherOptionForm.SerialCon1Button = true;
                        }

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial 연결을 실패하였습니다.",
                            e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial 연결을 실패하였습니다.",
                                e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit));
                        }
                    }

                    Settings.Default.Serial1IsUse = true;
                    Settings.Default.S1Com = e.ComPort;
                    Settings.Default.S1Rate = e.BaudRate;
                    Settings.Default.S1DataBit = e.DataBits;
                    Settings.Default.S1Parity = e.Parity;
                    Settings.Default.S1StopBit = e.StopBit;
                    Settings.Default.Save();
                }
                else if (e.Num == (byte)SType.client_2)
                {
                    this.serial_2 = WeatherSerial.getInstance();

                    if (this.serial_2.Open(e.ComPort, int.Parse(e.BaudRate), int.Parse(e.DataBits), parity, stopBit))
                    {
                        this.serial_2.onSerialRecvDataEvt += new EventHandler<SerialReceiveDataEvt>(serial_2_onSerialRecvDataEvt);

                        if (this.weatherOptionForm != null)
                        {
                            this.weatherOptionForm.SerialCon2Button = false;
                            this.weatherOptionForm.SerialConEnd2Button = true;
                        }

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial을 연결하였습니다.",
                            e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial을 연결하였습니다.",
                                e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit));
                        }
                    }
                    else
                    {
                        if (this.weatherOptionForm != null)
                        {
                            this.weatherOptionForm.SerialConEnd2Button = false;
                            this.weatherOptionForm.SerialCon2Button = true;
                        }

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial 연결을 실패하였습니다.",
                            e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit) });
                        }
                        else
                        {
                            this.SetRWeatherData(string.Format("{0}, {1}, {2}, {3}, {4} 로 Serial 연결을 실패하였습니다.",
                                e.ComPort, e.BaudRate, e.DataBits, e.Parity, e.StopBit));
                        }
                    }

                    Settings.Default.Serial2IsUse = true;
                    Settings.Default.S2Com = e.ComPort;
                    Settings.Default.S2Rate = e.BaudRate;
                    Settings.Default.S2DataBit = e.DataBits;
                    Settings.Default.S2Parity = e.Parity;
                    Settings.Default.S2StopBit = e.StopBit;
                    Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial 연결을 실패하였습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("Serial 연결을 실패하였습니다."));
                }

                Console.WriteLine(string.Format("MainForm.weatherOptionForm_SerialConEvt() - {0}", ex.Message));
            }
        }

        //1번 시리얼에서 주는 Receive Data
        private void serial_1_onSerialRecvDataEvt(object sender, SerialReceiveDataEvt e)
        {
            this.parsing.ParsingRcvStandardProtocol(e.Data);

            #region Low Data
            //string tmpStr = Encoding.Default.GetString(e.Data);

            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("1번 시리얼 데이터 [ {0} ]", tmpStr) });
            //}
            //else
            //{
            //    this.SetRWeatherData(string.Format("1번 시리얼 데이터 [ {0} ]", tmpStr));
            //}
            #endregion
        }

        //2번 시리얼에서 주는 Receive Data
        private void serial_2_onSerialRecvDataEvt(object sender, SerialReceiveDataEvt e)
        {
            this.parsing.ParsingRcvStandardProtocol(e.Data);

            #region Low Data
            //string tmpStr = Encoding.Default.GetString(e.Data);

            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("2번 시리얼 데이터 [ {0} ]", tmpStr) });
            //}
            //else
            //{
            //    this.SetRWeatherData(string.Format("2번 시리얼 데이터 [ {0} ]", tmpStr));
            //}
            #endregion
        }

        //옵션창에서 주는 Serial 연결 종료 이벤트
        private void weatherOptionForm_SerialConEndEvt(object sender, SerialConEndEventArgs e)
        {
            if (e.Num == (byte)SType.client_1)
            {
                if (this.serial_1.Close())
                {
                    this.weatherOptionForm.SerialCon1Button = true;
                    this.weatherOptionForm.SerialConEnd1Button = false;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("1번 Serial 연결을 종료하였습니다.") });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("1번 Serial 연결을 종료하였습니다."));
                    }
                }
                else
                {
                    this.weatherOptionForm.SerialCon1Button = false;
                    this.weatherOptionForm.SerialConEnd1Button = true;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("1번 Serial 연결 종료를 실패하였습니다.") });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("1번 Serial 연결 종료를 실패하였습니다."));
                    }
                }
            }
            else if (e.Num == (byte)SType.client_2)
            {
                if (this.serial_2.Close())
                {
                    this.weatherOptionForm.SerialCon2Button = true;
                    this.weatherOptionForm.SerialConEnd2Button = false;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("2번 Serial 연결을 종료하였습니다.") });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("2번 Serial 연결을 종료하였습니다."));
                    }
                }
                else
                {
                    this.weatherOptionForm.SerialCon2Button = false;
                    this.weatherOptionForm.SerialConEnd2Button = true;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("2번 Serial 연결 종료를 실패하였습니다.") });
                    }
                    else
                    {
                        this.SetRWeatherData(string.Format("2번 Serial 연결 종료를 실패하였습니다."));
                    }
                }
            }
        }

        //옵션창에서 주는 측기 데이터 설정 항목 이벤트
        private void weatherOptionForm_OptionDataSetEvt(object sender, WeatherDataOptionSetEventArgs e)
        {
            Settings.Default.DataAlarmCB = e.DataAlarm;
            Settings.Default.DataSelfCheckCB = e.DataSelfChk;
            Settings.Default.DataFWVerCB = e.DataFWVer;
            Settings.Default.DataBattCB = e.DataBatt;
            Settings.Default.DataSolarCB = e.DataSolar;
            Settings.Default.DataDoorCB = e.DataDoor;
            Settings.Default.Save();

            string tmp1 = (e.DataAlarm) ? "설정됨" : "미설정";
            string tmp2 = (e.DataSelfChk) ? "설정됨" : "미설정";
            string tmp3 = (e.DataFWVer) ? "설정됨" : "미설정";
            string tmp4 = (e.DataBatt) ? "설정됨" : "미설정";
            string tmp5 = (e.DataSolar) ? "설정됨" : "미설정";
            string tmp6 = (e.DataDoor) ? "설정됨" : "미설정";

            if (this.InvokeRequired)
            {
                this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format(
                    "측기 데이터가 설정되었습니다.\n임계치 설정값/{0}, 자가진단 결과/{1}, 펌웨어 버전/{2}, 배터리 상태/{3}, 태양전지 상태/{4}, 도어 상태/{5}",
                    tmp1, tmp2, tmp3, tmp4, tmp5, tmp6)});
            }
            else
            {
                this.SetRWeatherData(string.Format(
                    "측기 데이터가 설정되었습니다.\n임계치 설정값/{0}, 자가진단 결과/{1}, 펌웨어 버전/{2}, 배터리 상태/{3}, 태양전지 상태/{4}, 도어 상태/{5}",
                    tmp1, tmp2, tmp3, tmp4, tmp5, tmp6));
            }
        }
        #endregion

        /// <summary>
        /// 측기의 ID를 받아 이더넷 리스트에서 찾아 반환한다.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public EthernetClient getEthernetClient(string _id)
        {
            EthernetClient rst = new EthernetClient();

            for (int i = 0; i < this.dataMng.E_ClientList.Count; i++)
            {
                if (this.dataMng.E_ClientList[i].ID == _id)
                {
                    rst = this.dataMng.E_ClientList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// 로그인 데이터 확인
        /// </summary>
        /// <returns></returns>
        private bool LoadEvnData()
        {
            if (this.weatherLogin == null)
            {
                this.weatherLogin = new WeatherLogin();
            }

            this.weatherLogin.LoginID = Settings.Default.UserID;
            this.weatherLogin.LoginPW = Settings.Default.UserPW;
            this.weatherLogin.AutoLogin = Settings.Default.AutoLogin;

            if (this.weatherLogin.ShowDialog() == DialogResult.OK)
            {
                this.thredSplash();
                this.saveConfig();
            }
            else
            {
                this.flagLogin = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 측기 상태 체크를 시작한다.
        /// </summary>
        private void GetWDeviceState()
        {
            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                StateChecker wDeviceChecker = new StateChecker(this.WDeviceChkTimeInterval * 3, this.dataMng.DeviceList[i].PKID);
                wDeviceChecker.onWDeviceStateEvt += new EventHandler<WDeviceStateEventArgs>(wDeviceChecker_onWDeviceStateEvt);
                wDeviceChecker.Start();
                this.CheckerList.Add(wDeviceChecker);
            }
        }

        /// <summary>
        /// 측기 이상 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wDeviceChecker_onWDeviceStateEvt(object sender, WDeviceStateEventArgs e)
        {
            WDevice tmpWDevice = this.dataMng.GetWDevice(e.WDevicePKID);

            if (tmpWDevice.PKID != 0)
            {
                Proto013 p013 = ProtoMng.GetProtoObj("013") as Proto013;
                p013.Division = this.dataMng.GetTypeDevice(tmpWDevice.TypeDevice).Name;
                p013.ID = tmpWDevice.ID.PadLeft(15, '0');
                p013.Data = "1";
                byte[] buff = p013.MakeProto();
                this.SendFromWRS(buff);
            }
        }

        /// <summary>
        /// Splash 다이얼로그 관련
        /// </summary>
        private void thredSplash()
        {
            this.splashThread = new Thread(new ThreadStart(this.ShowSplash));
            this.splashThread.IsBackground = true;
            this.splashThread.Start();
        }

        /// <summary>
        /// Splash 다이얼로그 관련
        /// </summary>
        private void ShowSplash()
        {
            try
            {
                if (this.splashDlg != null)
                {
                    this.splashDlg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.ShowSplash() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 로그인 데이터 저장
        /// </summary>
        private void saveConfig()
        {
            try
            {
                Settings.Default.UserID = this.weatherLogin.LoginID;
                Settings.Default.UserPW = this.weatherLogin.LoginPW;
                Settings.Default.AutoLogin = this.weatherLogin.AutoLogin;
                Settings.Default.Save();

                this.userId = this.weatherLogin.LoginID;
                this.userPass = this.weatherLogin.LoginPW;
                this.weatherLogin = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("saveConfig - " + ex.Message);
            }
        }

        /// <summary>
        /// Splash 다이얼로그 관련
        /// </summary>
        private void CloseSplash()
        {
            try
            {
                if (this.splashDlg != null)
                {
                    if (this.splashDlg.InvokeRequired)
                    {
                        this.Invoke(new InvokerSplashClose(this.closeSplashExit));
                    }
                    else
                    {
                        this.closeSplashExit();
                    }
                }

                this.splashThread.Abort();
                this.splashThread = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.CloseSplash() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// Splash 다이얼로그 관련
        /// </summary>
        private void closeSplashExit()
        {
            this.splashDlg.CanClose = false;
            this.splashDlg.Close();
            this.splashDlg.Dispose();
        }

        /// <summary>
        /// 팝업 창을 위한 쓰레드 메소드
        /// </summary>
        private void PopUpThread()
        {
            if (this.alarmWindow == null)
            {
                this.alarmWindow = new AlarmWindow(this.aStruct.Real, this.aStruct.ID, this.aStruct.Level, this.aStruct.Data, this.aStruct.DDTime);
                this.alarmWindow.PopUp = true;
                this.alarmWindow.ShowDialog();
            }
            else
            {
                this.alarmWindow.SetAlarmWindow(this.aStruct.Real, this.aStruct.ID, this.aStruct.Level, this.aStruct.Data, this.aStruct.DDTime);

                if (!this.alarmWindow.PopUp)
                {
                    this.alarmWindow.PopUp = true;
                    this.alarmWindow.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 알람 창을 위한 쓰레드 메소드
        /// </summary>
        private void DevicePopUpThread()
        {
            try
            {
                if (this.deviceAlarm == null)
                {
                    this.deviceAlarm = new WDeviceAlarmWindow(this.daStruct.ID, this.daStruct.Data, this.daStruct.DDTime);
                    this.deviceAlarm.PopUp = true;
                    this.deviceAlarm.ShowDialog();
                }
                else
                {
                    this.deviceAlarm.SetAlarmWindow(this.daStruct.ID, this.daStruct.Data, this.daStruct.DDTime);
                    
                    if (!this.deviceAlarm.PopUp)
                    {
                        this.deviceAlarm.PopUp = true;
                        this.deviceAlarm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("MainForm.DevicePopUpThread - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 사용자 정보 확인
        /// </summary>
        /// <returns></returns>
        private bool Userconfirm()
        {
            bool confirm = false;

            foreach (WUser user in this.dataMng.UserList)
            {
                if (user.UserID == this.userId)
                {
                    if (user.UserPW == this.userPass)
                    {
                        confirm = true;
                        break;
                    }
                }
            }

            return confirm;
        }

        /// <summary>
        /// 연결 상태를 판단해 데이터를 전송한다.
        /// </summary>
        /// <param name="_buff"></param>
        private void SendFromWRS(byte[] _buff)
        {
            if (this.client_1.TcpConn)
            {
                this.client_1.SendData(_buff);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("IP : {0}, Port : {1} 로 데이터를 전송했습니다.",
                            this.client_1.IPAddr, this.client_1.Port) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("IP : {0}, Port : {1} 로 데이터를 전송했습니다.",
                            this.client_1.IPAddr, this.client_1.Port));
                }
            }

            if (this.client_2.TcpConn)
            {
                this.client_2.SendData(_buff);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("IP : {0}, Port : {1} 로 데이터를 전송했습니다.",
                            this.client_2.IPAddr, this.client_2.Port) });
                }
                else
                {
                    this.SetRWeatherData(string.Format("IP : {0}, Port : {1} 로 데이터를 전송했습니다.",
                            this.client_2.IPAddr, this.client_2.Port));
                }
            }

            if (this.serial_1 != null && this.serial_1.IsOpenState())
            {
                this.serial_1.SendDate(_buff);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial_1 로 데이터를 전송했습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("Serial_1 로 데이터를 전송했습니다."));
                }
            }

            if (this.serial_2 != null && this.serial_2.IsOpenState())
            {
                this.serial_2.SendDate(_buff);

                if (this.InvokeRequired)
                {
                    this.Invoke(new invokeSetWord(this.SetRWeatherData), new object[] { string.Format("Serial_2 로 데이터를 전송했습니다.") });
                }
                else
                {
                    this.SetRWeatherData(string.Format("Serial_2 로 데이터를 전송했습니다."));
                }
            }
        }

        /// <summary>
        /// splash invoke
        /// </summary>
        private void closesplash()
        {
            this.splashDlg.Close();
            this.splashDlg = null;
            this.splashDlg = new WeatherSplash(this.Text);
            this.splashDlg.BackgroundImage = Resources.Weather_Loading;
        }

        /// <summary>
        /// 측기 데이터로그 텍스트박스에 로그를 남긴다.
        /// </summary>
        /// <param name="_str"></param>
        private void SetRWeatherData(string _str)
        {
            this.weatherForm.SetWeatherData(_str);
        }

        /// <summary>
        /// 이벤트에 따라 tcp 관련 버튼 상태를 셋팅한다.
        /// </summary>
        /// <param name="_num"></param>
        /// <param name="_cmd"></param>
        private void SetTcpClientButton(byte _num, bool _stat)
        {
            if (this.weatherOptionForm != null)
            {
                if (_num == (byte)CType.client_1)
                {
                    if (_stat)
                    {
                        this.weatherOptionForm.TcpClient1ConButton = false;
                        this.weatherOptionForm.TcpClient1ConEndButton = true;
                        this.weatherOptionForm.TcpClient1 = true;
                    }
                    else
                    {
                        this.weatherOptionForm.TcpClient1ConButton = true;
                        this.weatherOptionForm.TcpClient1ConEndButton = false;
                        this.weatherOptionForm.TcpClient1 = false;
                    }
                }
                else if (_num == (byte)CType.client_2)
                {
                    if (_stat)
                    {
                        this.weatherOptionForm.TcpClient2ConButton = false;
                        this.weatherOptionForm.TcpClient2ConEndButton = true;
                        this.weatherOptionForm.TcpClient2 = true;
                    }
                    else
                    {
                        this.weatherOptionForm.TcpClient2ConButton = true;
                        this.weatherOptionForm.TcpClient2ConEndButton = false;
                        this.weatherOptionForm.TcpClient2 = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 임계치 팝업에 쓰이는 구조체
    /// </summary>
    public struct AlarmStrt
    {
        private bool real;
        private string id;
        private string level;
        private string data;
        private DateTime dt;

        public bool Real
        {
            get { return this.real; }
            set { this.real = value; }
        }

        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        public string Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        public DateTime DDTime
        {
            get { return this.dt; }
            set { this.dt = value; }
        }
    }

    /// <summary>
    /// 알람 팝업에 쓰이는 구조체
    /// </summary>
    public struct DeviceAlarmStrt
    {
        private string id;
        private string data;
        private DateTime dt;

        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        public DateTime DDTime
        {
            get { return this.dt; }
            set { this.dt = value; }
        }
    }
}