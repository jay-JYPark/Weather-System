using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using WeatherOptionForm.Properties;

namespace ADEng.Module.WeatherSystem
{
    public partial class WeatherOptionForm : Form
    {
        private delegate void ServerStartEventArgsHandler(object sender, ServerStartEventArgs ssea);
        private delegate void ServerStopEventArgsHandler(object sender, ServerStopEventArgs ssea);
        private delegate void TcpClientConEventArgsHandler(object sender, TcpClientConEventArgs tccea);
        private delegate void TcpClientConEndEventArgsHandler(object sender, TcpClientConEndEventArgs tcceea);
        private delegate void SerialConEventArgsHandler(object sender, SerialConEventArgs scea);
        private delegate void SerialConEndEventArgsHandler(object sender, SerialConEndEventArgs sceea);
        private delegate void WeatherDataOptionSetEventArgsHandler(object sender, WeatherDataOptionSetEventArgs wdosea);
        private delegate void DBDataSetEventArgsHandler(object sender, DBDataSetEventArgs dbdea);
        private delegate void AutoLoginEventArgsHandler(object sender, AutoLoginEventArgs alea);
        private delegate void CDMAPortEventArgsHandler(object sender, CDMAPortEventArgs cpea);
        private delegate void TcpIsUseEventArgsHandler(object sender, TcpIsUseEventArgs tiuea);
        private delegate void SerialIsUseEventArgsHandler(object sender, SerialIsUseEventArgs siuea);
        private delegate void WDeviceChkTimeEventArgsHandler(object sender, WDeviceChkTimeEventArgs wdctea);

        public event EventHandler<ServerStartEventArgs> ServerConEvt;
        public event EventHandler<ServerStopEventArgs> ServerStopEvt;
        public event EventHandler<TcpClientConEventArgs> ClientConEvt;
        public event EventHandler<TcpClientConEndEventArgs> ClientConEndEvt;
        public event EventHandler<SerialConEventArgs> SerialConEvt;
        public event EventHandler<SerialConEndEventArgs> SerialConEndEvt;
        public event EventHandler<WeatherDataOptionSetEventArgs> OptionDataSetEvt;
        public event EventHandler<DBDataSetEventArgs> DBDataSetEvt;
        public event EventHandler<AutoLoginEventArgs> AutoLoginEvt;
        public event EventHandler<CDMAPortEventArgs> CDMAPortEvt;
        public event EventHandler<TcpIsUseEventArgs> TcpIsUseEvt;
        public event EventHandler<SerialIsUseEventArgs> SerialIsUseEvt;
        public event EventHandler<WDeviceChkTimeEventArgs> WDeviceChkTimeEvt;
        public event EventHandler<ServerStartEventArgs> OnTcpServer_IsChange;
        public event EventHandler<ServerStartEventArgs> OnEthernetServer_IsChange;
        public event EventHandler<ServerStartEventArgs> OnEthernetServerConEvt;
        public event EventHandler<ServerStopEventArgs> OnEthernetServerStopEvt;

        private WeatherDataMng dataMng = null;

        private bool ServerStat = false;
        private bool EthernetServerStat = false;    //이더넷 서버 상태
        private bool TcpClient_1 = false;
        private bool TcpClient_2 = false;
        private bool SerialClient_1 = false;
        private bool SerialClient_2 = false;
        private bool optionData = false;            //측기 데이터 설정 항목의 체크를 위한 변수
        private bool dbData = false;                //DB 항목의 체크를 위한 변수
        private bool cdmaData = false;              //CDMA 항목의 체크를 위한 변수
        private bool wDeviceChkTime = false;        //측기 통신 상태 항목의 체크를 위한 변수
        private bool TcpServer_1_IsChange = false;  //TCP 서버 항목의 체크를 위한 변수
        private bool TcpServer_2_IsChange = false;  //TCP 이더넷 서버 항목의 체크를 위한 변수

        //tcp 클라이언트 enum's
        private enum CType
        {
            client1 = 1,
            client2 = 2
        }

        //serial 클라이언트 enum's
        private enum SType
        {
            client1 = 1,
            client2 = 2
        }

        #region 접근
        /// <summary>
        /// 측기가 연결하는 서버 상태
        /// </summary>
        public bool ServerState
        {
            get { return this.ServerStat; }
            set { this.ServerStat = value; }
        }

        /// <summary>
        /// 측기가 연결하는 이더넷 서버 상태
        /// </summary>
        public bool EthernetServerState
        {
            get { return this.EthernetServerStat; }
            set { this.EthernetServerStat = value; }
        }

        public bool TcpClient1
        {
            get { return this.TcpClient_1; }
            set { this.TcpClient_1 = value; }
        }

        public bool TcpClient2
        {
            get { return this.TcpClient_2; }
            set { this.TcpClient_2 = value; }
        }

        public bool SerialClient1
        {
            get { return this.SerialClient_1; }
            set { this.SerialClient_1 = value; }
        }

        public bool SerialClient2
        {
            get { return this.SerialClient_2; }
            set { this.SerialClient_2 = value; }
        }

        /// <summary>
        /// 서버연결 버튼의 Enabled 상태
        /// </summary>
        public bool ServerConButton
        {
            set { this.ConBtn.Enabled = value; }
        }

        /// <summary>
        /// 서버종료 버튼의 Enabled 상태
        /// </summary>
        public bool ServerConEndButton
        {
            set { this.ConEndBtn.Enabled = value; }
        }

        /// <summary>
        /// 이더넷 서버연결 버튼의 Enabled 상태
        /// </summary>
        public bool EthernetServerConBtn
        {
            set { this.Con_E_Btn.Enabled = value; }
        }

        /// <summary>
        /// 이더넷 서버종료 버튼의 Enabled 상태
        /// </summary>
        public bool EthernetServerConEndBtn
        {
            set { this.ConEnd_E_Btn.Enabled = value; }
        }

        /// <summary>
        /// tcp1 통신연결 버튼의 Enabled 상태
        /// </summary>
        public bool TcpClient1ConButton
        {
            set { this.Tcp1StartBtn.Enabled = value; }
        }

        /// <summary>
        /// tcp1 연결종료 버튼의 Enabled 상태
        /// </summary>
        public bool TcpClient1ConEndButton
        {
            set { this.Tcp1StopBtn.Enabled = value; }
        }

        /// <summary>
        /// tcp2 통신연결 버튼의 Enabled 상태
        /// </summary>
        public bool TcpClient2ConButton
        {
            set { this.Tcp2StartBtn.Enabled = value; }
        }

        /// <summary>
        /// tcp2 연결종료 버튼의 Enabled 상태
        /// </summary>
        public bool TcpClient2ConEndButton
        {
            set { this.Tcp2StopBtn.Enabled = value; }
        }

        /// <summary>
        /// Serial1 통신연결 버튼의 Enabled 상태
        /// </summary>
        public bool SerialCon1Button
        {
            set { this.SerialConBtn1.Enabled = value; }
        }

        /// <summary>
        /// Serial1 연결종료 버튼의 Enabled 상태
        /// </summary>
        public bool SerialConEnd1Button
        {
            set { this.SerialConEndBtn1.Enabled = value; }
        }

        /// <summary>
        /// Serial2 통신연결 버튼의 Enabled 상태
        /// </summary>
        public bool SerialCon2Button
        {
            set { this.SerialConBtn2.Enabled = value; }
        }

        /// <summary>
        /// Serial2 연결종료 버튼의 Enabled 상태
        /// </summary>
        public bool SerialConEnd2Button
        {
            set { this.SerialConEndBtn2.Enabled = value; }
        }
        #endregion

        public WeatherOptionForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onDBConEvt += new EventHandler<DBConEventArgs>(dataMng_onDBConEvt);
            this.dataMng.onDBTestConEvt += new EventHandler<DBConTestEventArgs>(dataMng_onDBTestConEvt);

            this.IpTB.Text = Settings.Default.myIp;
            this.PortTB.Text = Settings.Default.myPort;
            this.Ip_E_TB.Text = Settings.Default.EthernetIP;
            this.Port_E_TB.Text = Settings.Default.EthernetPort;
            this.TcpIp1CB.Checked = Settings.Default.TcpIp1CB;
            this.TcpIp2CB.Checked = Settings.Default.TcpIp2CB;
            this.TcpIp1TB.Text = Settings.Default.Tcp1_IP;
            this.TcpPort1TB.Text = Settings.Default.Tcp1_Port;
            this.TcpIp2TB.Text = Settings.Default.Tcp2_IP;
            this.TcpPort2TB.Text = Settings.Default.Tcp2_Port;
            this.SerialConCB1.Checked = Settings.Default.SerialConCB1;
            this.SerialConCB2.Checked = Settings.Default.SerialConCB2;
            this.DataAlarmCB.Checked = Settings.Default.DataAlarmCB;
            this.DataSelfCheckCB.Checked = Settings.Default.DataSelfCheckCB;
            this.DataFWVerCB.Checked = Settings.Default.DataFWVerCB;
            this.DataBattCB.Checked = Settings.Default.DataBattCB;
            this.DataSolarCB.Checked = Settings.Default.DataSolarCB;
            this.DataDoorCB.Checked = Settings.Default.DataDoorCB;
            this.dbIPTB.Text = Settings.Default.DbIp;
            this.dbPortTB.Text = Settings.Default.DbPort;
            this.dbIdTB.Text = Settings.Default.DbId;
            this.dbPWTB.Text = Settings.Default.DbPw;
            this.dbSidTB.Text = Settings.Default.DbSid;
            this.autoLoginCB.Checked = Settings.Default.autoLoginCB;
            this.WDeviceStateNum.Value = int.Parse(Settings.Default.WDeviceState.ToString());
            this.optionData = false;
            this.wDeviceChkTime = false;
            this.SerialInit(); //시리얼 설정 관련 초기화
            
            //1번 시리얼 초기화
            this.ComPortCB1.Text = Settings.Default.ComPortCB1;
            this.BaudRateCB1.Text = Settings.Default.BaudRateCB1;
            this.DataBitsCB1.Text = Settings.Default.DataBitsCB1;
            this.ParityCB1.Text = Settings.Default.ParityCB1;
            this.StopBitCB1.Text = Settings.Default.StopBitCB1;

            //2번 시리얼 초기화
            this.ComPortCB2.Text = Settings.Default.ComPortCB2;
            this.BaudRateCB2.Text = Settings.Default.BaudRateCB2;
            this.DataBitsCB2.Text = Settings.Default.DataBitsCB2;
            this.ParityCB2.Text = Settings.Default.ParityCB2;
            this.StopBitCB2.Text = Settings.Default.StopBitCB2;

            this.SaveBtn.Enabled = false;

            if (this.ServerStat) //서버 상태에 따라 "서버시작/서버종료" 버튼 Enabled 처리
            {
                this.ConBtn.Enabled = false;
                this.ConEndBtn.Enabled = true;
            }
            else
            {
                this.ConEndBtn.Enabled = false;
                this.ConBtn.Enabled = true;
            }

            if (this.EthernetServerStat) //이더넷 서버 상태에 따라..
            {
                this.Con_E_Btn.Enabled = false;
                this.ConEnd_E_Btn.Enabled = true;
            }
            else
            {
                this.ConEnd_E_Btn.Enabled = false;
                this.Con_E_Btn.Enabled = true;
            }

            if (this.TcpIp1CB.Checked) //tcp1 사용 체크가 되어있으면..
            {
                if (this.TcpClient_1) //현재 연결 상태이면..
                {
                    this.Tcp1StartBtn.Enabled = false;
                    this.Tcp1StopBtn.Enabled = true;
                }
                else
                {
                    this.Tcp1StopBtn.Enabled = false;
                    this.Tcp1StartBtn.Enabled = true;
                }
            }

            if (this.TcpIp2CB.Checked) //tcp2 사용 체크가 되어있으면..
            {
                if (this.TcpClient_2) //현재 연결 상태이면..
                {
                    this.Tcp2StartBtn.Enabled = false;
                    this.Tcp2StopBtn.Enabled = true;
                }
                else
                {
                    this.Tcp2StopBtn.Enabled = false;
                    this.Tcp2StartBtn.Enabled = true;
                }
            }

            if (this.SerialConCB1.Checked) //serial1 사용 체크가 되어있으면..
            {
                if (this.SerialClient1) //현재 연결 상태이면..
                {
                    this.SerialConBtn1.Enabled = false;
                    this.SerialConEndBtn1.Enabled = true;
                }
                else
                {
                    this.SerialConBtn1.Enabled = true;
                    this.SerialConEndBtn1.Enabled = false;
                }
            }

            if (this.SerialConCB2.Checked) //serial2 사용 체크가 되어있으면..
            {
                if (this.SerialClient2) //현재 연결 상태이면..
                {
                    this.SerialConBtn2.Enabled = false;
                    this.SerialConEndBtn2.Enabled = true;
                }
                else
                {
                    this.SerialConBtn2.Enabled = true;
                    this.SerialConEndBtn2.Enabled = false;
                }
            }

            if (Settings.Default.CdmaPort == string.Empty)
            {
                this.CdmaStateLB.Text = string.Format("포트가 설정되어 있지 않습니다.");
            }
            else
            {
                this.CdmaStateLB.Text = string.Format("{0} 포트로 설정되어 있습니다.", Settings.Default.CdmaPort);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.dataMng.onDBConEvt -= new EventHandler<DBConEventArgs>(dataMng_onDBConEvt);
            this.dataMng.onDBTestConEvt -= new EventHandler<DBConTestEventArgs>(dataMng_onDBTestConEvt);
        }

        //DB 연결 시험 후 결과 이벤트
        private void dataMng_onDBTestConEvt(object sender, DBConTestEventArgs e)
        {
            if (e.Rst)
            {
                this.dbTestLB.Text = "DB 테스트를 성공했습니다!";
            }
            else
            {
                this.dbTestLB.Text = "DB 테스트를 실패했습니다.";
            }

            this.Cursor = Cursors.Default;
        }

        //DB 연결 후 결과 이벤트
        private void dataMng_onDBConEvt(object sender, DBConEventArgs e)
        {
            if (e.Rst)
            {
                this.dbTestLB.Text = "DB 연결을 성공했습니다!";
            }
            else
            {
                this.dbTestLB.Text = "DB 연결을 실패했습니다.";
            }

            this.Cursor = Cursors.Default;
        }
        
        //확인 버튼 클릭
        private void OkBtn_Click(object sender, EventArgs e)
        {
            this.Save();
            this.Close();
        }

        //취소 버튼 클릭
        private void CancleBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //적용 버튼 클릭
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        //CDMA 서버시작 버튼 클릭
        private void ConBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int tmpInt = int.MinValue;

                string[] tmpStr = this.IpTB.Text.Split(new char[] { '.' });

                if (tmpStr.Length != 4)
                {
                    MessageBox.Show("IP를 형식에 맞게 입력하세요.", "CDMA 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                for (int i = 0; i < tmpStr.Length; i++)
                {
                    if (int.Parse(tmpStr[i]) > 255)
                    {
                        MessageBox.Show("IP를 형식에 맞게 입력하세요.", "CDMA 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (int.TryParse(this.PortTB.Text, out tmpInt))
                {
                    if (tmpInt > 65535)
                    {
                        MessageBox.Show("포트를 형식에 맞게 입력하세요.", "CDMA 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (this.ServerConEvt != null)
                    {
                        this.ServerConEvt(this, new ServerStartEventArgs(this.IpTB.Text, int.Parse(this.PortTB.Text)));
                    }
                }
                else
                {
                    MessageBox.Show("포트를 형식에 맞게 입력하세요.", "CDMA 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("IP/PORT 를 형식에 맞게 입력하세요.", "CDMA 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //CDMA 서버종료 버튼 클릭
        private void ConEndBtn_Click(object sender, EventArgs e)
        {
            if (this.ServerStopEvt != null)
            {
                this.ServerStopEvt(this, new ServerStopEventArgs());
            }
        }

        /// <summary>
        /// 이더넷 서버시작 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Con_E_Btn_Click(object sender, EventArgs e)
        {
            try
            {
                int tmpInt = int.MinValue;

                string[] tmpStr = this.Ip_E_TB.Text.Split(new char[] { '.' });

                if (tmpStr.Length != 4)
                {
                    MessageBox.Show("IP를 형식에 맞게 입력하세요.", "이더넷 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                for (int i = 0; i < tmpStr.Length; i++)
                {
                    if (int.Parse(tmpStr[i]) > 255)
                    {
                        MessageBox.Show("IP를 형식에 맞게 입력하세요.", "이더넷 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (int.TryParse(this.Port_E_TB.Text, out tmpInt))
                {
                    if (tmpInt > 65535)
                    {
                        MessageBox.Show("포트를 형식에 맞게 입력하세요.", "이더넷 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (this.OnEthernetServerConEvt != null)
                    {
                        this.OnEthernetServerConEvt(this, new ServerStartEventArgs(this.Ip_E_TB.Text, int.Parse(this.Port_E_TB.Text)));
                    }
                }
                else
                {
                    MessageBox.Show("포트를 형식에 맞게 입력하세요.", "이더넷 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("IP/PORT 를 형식에 맞게 입력하세요.", "이더넷 서버 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 이더넷 서버종료 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConEnd_E_Btn_Click(object sender, EventArgs e)
        {
            if (this.OnEthernetServerStopEvt != null)
            {
                this.OnEthernetServerStopEvt(this, new ServerStopEventArgs());
            }
        }

        /// <summary>
        /// 설정한 항목 저장
        /// </summary>
        private void Save()
        {
            Settings.Default.myIp = this.IpTB.Text;
            Settings.Default.myPort = this.PortTB.Text;
            Settings.Default.EthernetIP = this.Ip_E_TB.Text;
            Settings.Default.EthernetPort = this.Port_E_TB.Text;
            Settings.Default.TcpIp1CB = this.TcpIp1CB.Checked;
            Settings.Default.TcpIp2CB = this.TcpIp2CB.Checked;
            Settings.Default.Tcp1_IP = this.TcpIp1TB.Text;
            Settings.Default.Tcp1_Port = this.TcpPort1TB.Text;
            Settings.Default.Tcp2_IP = this.TcpIp2TB.Text;
            Settings.Default.Tcp2_Port = this.TcpPort2TB.Text;
            Settings.Default.SerialConCB1 = this.SerialConCB1.Checked;
            Settings.Default.ComPortCB1 = this.ComPortCB1.Text;
            Settings.Default.BaudRateCB1 = this.BaudRateCB1.Text;
            Settings.Default.DataBitsCB1 = this.DataBitsCB1.Text;
            Settings.Default.ParityCB1 = this.ParityCB1.Text;
            Settings.Default.StopBitCB1 = this.StopBitCB1.Text;
            Settings.Default.SerialConCB2 = this.SerialConCB2.Checked;
            Settings.Default.ComPortCB2 = this.ComPortCB2.Text;
            Settings.Default.BaudRateCB2 = this.BaudRateCB2.Text;
            Settings.Default.DataBitsCB2 = this.DataBitsCB2.Text;
            Settings.Default.ParityCB2 = this.ParityCB2.Text;
            Settings.Default.StopBitCB2 = this.StopBitCB2.Text;
            Settings.Default.DataAlarmCB = this.DataAlarmCB.Checked;
            Settings.Default.DataSelfCheckCB = this.DataSelfCheckCB.Checked;
            Settings.Default.DataFWVerCB = this.DataFWVerCB.Checked;
            Settings.Default.DataBattCB = this.DataBattCB.Checked;
            Settings.Default.DataSolarCB = this.DataSolarCB.Checked;
            Settings.Default.DataDoorCB = this.DataDoorCB.Checked;
            Settings.Default.DbIp = this.dbIPTB.Text;
            Settings.Default.DbPort = this.dbPortTB.Text;
            Settings.Default.DbId = this.dbIdTB.Text;
            Settings.Default.DbPw = this.dbPWTB.Text;
            Settings.Default.DbSid = this.dbSidTB.Text;
            Settings.Default.autoLoginCB = this.autoLoginCB.Checked;
            Settings.Default.WDeviceState = this.WDeviceStateNum.Value.ToString();

            if (this.cdmaData)
            {
                Settings.Default.CdmaPort = this.CdmaSubCB.Text;
            }

            Settings.Default.Save();

            this.SaveBtn.Enabled = false;

            if (this.TcpServer_1_IsChange)
            {
                if (this.OnTcpServer_IsChange != null)
                {
                    this.OnTcpServer_IsChange(this, new ServerStartEventArgs(
                        this.IpTB.Text, int.Parse(this.PortTB.Text)));
                }

                this.TcpServer_1_IsChange = false;
            }

            if (this.TcpServer_2_IsChange)
            {
                if (this.OnEthernetServer_IsChange != null)
                {
                    this.OnEthernetServer_IsChange(this, new ServerStartEventArgs(
                        this.Ip_E_TB.Text, int.Parse(this.Port_E_TB.Text)));
                }

                this.TcpServer_2_IsChange = false;
            }

            if (this.optionData)
            {
                if (this.OptionDataSetEvt != null)
                {
                    this.OptionDataSetEvt(this, new WeatherDataOptionSetEventArgs(
                        this.DataAlarmCB.Checked,
                        this.DataSelfCheckCB.Checked,
                        this.DataFWVerCB.Checked,
                        this.DataBattCB.Checked,
                        this.DataSolarCB.Checked,
                        this.DataDoorCB.Checked));
                }

                this.optionData = false;
            }

            if (this.dbData)
            {
                if (this.DBDataSetEvt != null)
                {
                    this.DBDataSetEvt(this, new DBDataSetEventArgs(
                        this.dbIPTB.Text,
                        this.dbPortTB.Text,
                        this.dbIdTB.Text,
                        this.dbPWTB.Text,
                        this.dbSidTB.Text));
                }

                this.dbData = false;
            }

            if (this.AutoLoginEvt != null)
            {
                this.AutoLoginEvt(this, new AutoLoginEventArgs(this.autoLoginCB.Checked));
            }

            if (this.cdmaData)
            {
                if (this.CDMAPortEvt != null)
                {
                    this.CDMAPortEvt(this, new CDMAPortEventArgs(Settings.Default.CdmaPort));
                }

                this.cdmaData = false;
                this.CdmaStateLB.Text = string.Format("{0} 포트로 설정되어 있습니다.", Settings.Default.CdmaPort);
            }

            if (this.TcpIsUseEvt != null)
            {
                this.TcpIsUseEvt(this, new TcpIsUseEventArgs(
                    this.TcpIp1CB.Checked,
                    this.TcpIp2CB.Checked,
                    this.TcpIp1TB.Text,
                    this.TcpPort1TB.Text,
                    this.TcpIp2TB.Text,
                    this.TcpPort2TB.Text));
            }

            if (this.SerialIsUseEvt != null)
            {
                this.SerialIsUseEvt(this, new SerialIsUseEventArgs(
                    (byte)SType.client1, this.SerialConCB1.Checked, this.ComPortCB1.Text, this.BaudRateCB1.Text
                    , this.DataBitsCB1.Text, this.ParityCB1.Text, this.StopBitCB1.Text));
                
                Thread.Sleep(20);
                
                this.SerialIsUseEvt(this, new SerialIsUseEventArgs(
                    (byte)SType.client2, this.SerialConCB2.Checked, this.ComPortCB2.Text, this.BaudRateCB2.Text
                    , this.DataBitsCB2.Text, this.ParityCB2.Text, this.StopBitCB2.Text));
            }

            if (this.wDeviceChkTime)
            {
                if (this.WDeviceChkTimeEvt != null)
                {
                    this.WDeviceChkTimeEvt(this, new WDeviceChkTimeEventArgs(this.WDeviceStateNum.Value.ToString()));
                }

                this.wDeviceChkTime = false;
            }
        }

        //내 포트 텍스트박스 변경 시
        private void PortTB_TextChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;
        }

        //tcp1 사용 체크박스
        private void TcpIp1CB_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;

            if (this.TcpIp1CB.Checked)
            {
                this.TcpIp1TB.Enabled = true;
                this.TcpPort1TB.Enabled = true;

                if (this.TcpClient_1)
                {
                    this.Tcp1StartBtn.Enabled = false;
                    this.Tcp1StopBtn.Enabled = true;
                }
                else
                {
                    this.Tcp1StopBtn.Enabled = false;
                    this.Tcp1StartBtn.Enabled = true;
                }
            }
            else
            {
                this.TcpIp1TB.Enabled = false;
                this.TcpPort1TB.Enabled = false;
                this.Tcp1StartBtn.Enabled = false;
                this.Tcp1StopBtn.Enabled = false;
            }
        }

        //tcp2 사용 체크박스
        private void TcpIp2CB_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;

            if (this.TcpIp2CB.Checked)
            {
                this.TcpIp2TB.Enabled = true;
                this.TcpPort2TB.Enabled = true;

                if (this.TcpClient_2)
                {
                    this.Tcp2StartBtn.Enabled = false;
                    this.Tcp2StopBtn.Enabled = true;
                }
                else
                {
                    this.Tcp2StopBtn.Enabled = false;
                    this.Tcp2StartBtn.Enabled = true;
                }
            }
            else
            {
                this.TcpIp2TB.Enabled = false;
                this.TcpPort2TB.Enabled = false;
                this.Tcp2StartBtn.Enabled = false;
                this.Tcp2StopBtn.Enabled = false;
            }
        }

        //Tcp1 통신연결 버튼 클릭
        private void Tcp1StartBtn_Click(object sender, EventArgs e)
        {
            int tmpInt = int.MinValue;
            string[] tmpStr = this.TcpIp1TB.Text.Split(new char[] { '.' });

            if (tmpStr.Length != 4)
            {
                MessageBox.Show("IP를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < tmpStr.Length; i++)
            {
                if (int.Parse(tmpStr[i]) > 255)
                {
                    MessageBox.Show("IP를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (int.TryParse(this.TcpPort1TB.Text, out tmpInt))
            {
                if (tmpInt > 65535)
                {
                    MessageBox.Show("포트를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.ClientConEvt != null)
                {
                    this.ClientConEvt(this, new TcpClientConEventArgs((byte)CType.client1, this.TcpIp1TB.Text, int.Parse(this.TcpPort1TB.Text)));
                }
            }
            else
            {
                MessageBox.Show("포트를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Tcp1 연결종료 버튼 클릭
        private void Tcp1StopBtn_Click(object sender, EventArgs e)
        {
            if (this.ClientConEndEvt != null)
            {
                this.ClientConEndEvt(this, new TcpClientConEndEventArgs((byte)CType.client1));
            }
        }

        //Tcp2 통신연결 버튼 클릭
        private void Tcp2StartBtn_Click(object sender, EventArgs e)
        {
            int tmpInt = int.MinValue;
            string[] tmpStr = this.TcpIp2TB.Text.Split(new char[] { '.' });

            if (tmpStr.Length != 4)
            {
                MessageBox.Show("IP를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < tmpStr.Length; i++)
            {
                if (int.Parse(tmpStr[i]) > 255)
                {
                    MessageBox.Show("IP를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (int.TryParse(this.TcpPort2TB.Text, out tmpInt))
            {
                if (tmpInt > 65535)
                {
                    MessageBox.Show("포트를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.ClientConEvt != null)
                {
                    this.ClientConEvt(this, new TcpClientConEventArgs((byte)CType.client2, this.TcpIp2TB.Text, int.Parse(this.TcpPort2TB.Text)));
                }
            }
            else
            {
                MessageBox.Show("포트를 형식에 맞게 입력하세요.", "TCP 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Tcp2 연결종료 버튼 클릭
        private void Tcp2StopBtn_Click(object sender, EventArgs e)
        {
            if (this.ClientConEndEvt != null)
            {
                this.ClientConEndEvt(this, new TcpClientConEndEventArgs((byte)CType.client2));
            }
        }

        //시리얼 설정 관련 초기화
        private void SerialInit()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                this.CdmaSubCB.Items.Add(portName);
                this.CdmaSubCB.Sorted = true;

                if (portName != Settings.Default.CdmaPort)
                {
                    this.ComPortCB1.Items.Add(portName);
                    this.ComPortCB1.Sorted = true;
                    this.ComPortCB2.Items.Add(portName);
                    this.ComPortCB2.Sorted = true;
                }
            }

            this.BaudRateCB1.Items.Add("2400");
            this.BaudRateCB1.Items.Add("4800");
            this.BaudRateCB1.Items.Add("9600");
            this.BaudRateCB1.Items.Add("19200");
            this.BaudRateCB1.Items.Add("38400");
            this.BaudRateCB1.Items.Add("57600");
            this.BaudRateCB1.Items.Add("115200");

            this.BaudRateCB2.Items.Add("2400");
            this.BaudRateCB2.Items.Add("4800");
            this.BaudRateCB2.Items.Add("9600");
            this.BaudRateCB2.Items.Add("19200");
            this.BaudRateCB2.Items.Add("38400");
            this.BaudRateCB2.Items.Add("57600");
            this.BaudRateCB2.Items.Add("115200");

            this.DataBitsCB1.Items.Add("5");
            this.DataBitsCB1.Items.Add("6");
            this.DataBitsCB1.Items.Add("7");
            this.DataBitsCB1.Items.Add("8");

            this.DataBitsCB2.Items.Add("5");
            this.DataBitsCB2.Items.Add("6");
            this.DataBitsCB2.Items.Add("7");
            this.DataBitsCB2.Items.Add("8");

            this.ParityCB1.Items.Add("홀수");
            this.ParityCB1.Items.Add("짝수");
            this.ParityCB1.Items.Add("없음");

            this.ParityCB2.Items.Add("홀수");
            this.ParityCB2.Items.Add("짝수");
            this.ParityCB2.Items.Add("없음");

            this.StopBitCB1.Items.Add("1");
            this.StopBitCB1.Items.Add("1.5");
            this.StopBitCB1.Items.Add("2");

            this.StopBitCB2.Items.Add("1");
            this.StopBitCB2.Items.Add("1.5");
            this.StopBitCB2.Items.Add("2");
        }

        //시리얼1 사용 체크박스 클릭 시
        private void SerialConCB1_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;

            if (this.SerialConCB1.Checked)
            {
                this.ComPortCB1.Enabled = true;
                this.BaudRateCB1.Enabled = true;
                this.DataBitsCB1.Enabled = true;
                this.ParityCB1.Enabled = true;
                this.StopBitCB1.Enabled = true;

                if (this.SerialClient_1)
                {
                    this.SerialConBtn1.Enabled = false;
                    this.SerialConEndBtn1.Enabled = true;
                }
                else
                {
                    this.SerialConBtn1.Enabled = true;
                    this.SerialConEndBtn1.Enabled = false;
                }
            }
            else
            {
                this.ComPortCB1.Enabled = false;
                this.BaudRateCB1.Enabled = false;
                this.DataBitsCB1.Enabled = false;
                this.ParityCB1.Enabled = false;
                this.StopBitCB1.Enabled = false;
                this.SerialConBtn1.Enabled = false;
                this.SerialConEndBtn1.Enabled = false;
            }
        }

        //시리얼2 사용 체크박스 클릭 시
        private void SerialConCB2_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;

            if (this.SerialConCB2.Checked)
            {
                this.ComPortCB2.Enabled = true;
                this.BaudRateCB2.Enabled = true;
                this.DataBitsCB2.Enabled = true;
                this.ParityCB2.Enabled = true;
                this.StopBitCB2.Enabled = true;

                if (this.SerialClient_2)
                {
                    this.SerialConBtn2.Enabled = false;
                    this.SerialConEndBtn2.Enabled = true;
                }
                else
                {
                    this.SerialConBtn2.Enabled = true;
                    this.SerialConEndBtn2.Enabled = false;
                }
            }
            else
            {
                this.ComPortCB2.Enabled = false;
                this.BaudRateCB2.Enabled = false;
                this.DataBitsCB2.Enabled = false;
                this.ParityCB2.Enabled = false;
                this.StopBitCB2.Enabled = false;
                this.SerialConBtn2.Enabled = false;
                this.SerialConEndBtn2.Enabled = false;
            }
        }

        //시리얼1 통신연결 버튼 클릭
        private void SerialConBtn1_Click(object sender, EventArgs e)
        {
            if (this.SerialValidate((byte)SType.client1))
            {
                if (this.SerialConEvt != null)
                {
                    this.SerialConEvt(this, new SerialConEventArgs(
                        (byte)SType.client1,
                        this.ComPortCB1.Text,
                        this.BaudRateCB1.Text,
                        this.DataBitsCB1.Text,
                        this.ParityCB1.Text,
                        this.StopBitCB1.Text));
                }
            }
            else
            {
                MessageBox.Show("항목을 모두 입력하세요.", "Serial 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //시리얼1 통신종료 버튼 클릭
        private void SerialConEndBtn1_Click(object sender, EventArgs e)
        {
            if (this.SerialConEndEvt != null)
            {
                this.SerialConEndEvt(this, new SerialConEndEventArgs((byte)SType.client1));
            }
        }

        //시리얼2 통신연결 버튼 클릭
        private void SerialConBtn2_Click(object sender, EventArgs e)
        {
            if (this.SerialValidate((byte)SType.client2))
            {
                if (this.SerialConEvt != null)
                {
                    this.SerialConEvt(this, new SerialConEventArgs(
                        (byte)SType.client2,
                        this.ComPortCB2.Text,
                        this.BaudRateCB2.Text,
                        this.DataBitsCB2.Text,
                        this.ParityCB2.Text,
                        this.StopBitCB2.Text));
                }
            }
            else
            {
                MessageBox.Show("항목을 모두 입력하세요.", "Serial 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //시리얼2 통신종료 버튼 클릭
        private void SerialConEndBtn2_Click(object sender, EventArgs e)
        {
            if (this.SerialConEndEvt != null)
            {
                this.SerialConEndEvt(this, new SerialConEndEventArgs((byte)SType.client2));
            }
        }

        /// <summary>
        /// serial 통신 연결 시 텍스트박스를 검사한다.
        /// </summary>
        /// <param name="_num"></param>
        /// <returns></returns>
        private bool SerialValidate(byte _num)
        {
            if (_num == (byte)SType.client1)
            {
                if (this.ComPortCB1.Text == string.Empty)
                {
                    return false;
                }
                else if (this.BaudRateCB1.Text == string.Empty)
                {
                    return false;
                }
                else if (this.DataBitsCB1.Text == string.Empty)
                {
                    return false;
                }
                else if (this.ParityCB1.Text == string.Empty)
                {
                    return false;
                }
                else if (this.StopBitCB1.Text == string.Empty)
                {
                    return false;
                }
            }
            else if (_num == (byte)SType.client2)
            {
                if (this.ComPortCB2.Text == string.Empty)
                {
                    return false;
                }
                else if (this.BaudRateCB2.Text == string.Empty)
                {
                    return false;
                }
                else if (this.DataBitsCB2.Text == string.Empty)
                {
                    return false;
                }
                else if (this.ParityCB2.Text == string.Empty)
                {
                    return false;
                }
                else if (this.StopBitCB2.Text == string.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        //측기 데이터 설정 항목 체크박스 변경 시
        private void DataAlarmCB_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;
            this.optionData = true;
        }

        //DB 테스트 버튼 클릭
        private void dbTestBtn_Click(object sender, EventArgs e)
        {
            string[] tmpStr = this.dbIPTB.Text.Split(new char[] { '.' });
            int tmpInt = int.MinValue;

            if (tmpStr.Length != 4)
            {
                MessageBox.Show("IP를 형식에 맞게 입력하세요.", "DB 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < tmpStr.Length; i++)
            {
                if (int.Parse(tmpStr[i]) > 255)
                {
                    MessageBox.Show("IP를 형식에 맞게 입력하세요.", "DB 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (int.TryParse(this.dbPortTB.Text, out tmpInt))
            {
                if (tmpInt > 65535)
                {
                    MessageBox.Show("포트를 형식에 맞게 입력하세요.", "DB 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            this.dbTestLB.Text = "잠시만 기다리세요..";
            this.Cursor = Cursors.WaitCursor;
            Thread.Sleep(20);
            this.dataMng.DataBaseTest(this.dbIPTB.Text, this.dbPortTB.Text, this.dbIdTB.Text, this.dbPWTB.Text, this.dbSidTB.Text);
        }

        //DB 연결 버튼 클릭
        private void dbConBtn_Click(object sender, EventArgs e)
        {
            string[] tmpStr = this.dbIPTB.Text.Split(new char[] { '.' });
            int tmpInt = int.MinValue;

            if (tmpStr.Length != 4)
            {
                MessageBox.Show("IP를 형식에 맞게 입력하세요.", "DB 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < tmpStr.Length; i++)
            {
                if (int.Parse(tmpStr[i]) > 255)
                {
                    MessageBox.Show("IP를 형식에 맞게 입력하세요.", "DB 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (int.TryParse(this.dbPortTB.Text, out tmpInt))
            {
                if (tmpInt > 65535)
                {
                    MessageBox.Show("포트를 형식에 맞게 입력하세요.", "DB 연결 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            this.dbTestLB.Text = "잠시만 기다리세요..";
            this.Cursor = Cursors.WaitCursor;
            Thread.Sleep(20);
            this.dataMng.DataBaseCon(this.dbIPTB.Text, this.dbPortTB.Text, this.dbIdTB.Text, this.dbPWTB.Text, this.dbSidTB.Text);
        }

        //DB 연결 항목 텍스트박스 변경 시
        private void dbIPTB_TextChanged(object sender, EventArgs e)
        {
            if (this.dbBtnCheck())
            {
                this.dbTestBtn.Enabled = true;
                this.dbConBtn.Enabled = true;
            }
            else
            {
                this.dbTestBtn.Enabled = false;
                this.dbConBtn.Enabled = false;
            }

            this.SaveBtn.Enabled = true;
            this.dbData = true;
        }

        /// <summary>
        /// DB 연결 항목 텍스트박스를 검사하기 위한 메소드
        /// </summary>
        /// <returns></returns>
        private bool dbBtnCheck()
        {
            if (this.dbIPTB.Text == string.Empty || this.dbPortTB.Text == string.Empty || this.dbIdTB.Text == string.Empty
                || this.dbPWTB.Text == string.Empty || this.dbSidTB.Text == string.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 자동로그인 항목을 저장한다.
        /// </summary>
        /// <param name="_autoFlag"></param>
        public void SetAutoLogin(bool _autoFlag)
        {
            Settings.Default.autoLoginCB = _autoFlag;
            Settings.Default.Save();
        }

        /// <summary>
        /// CDMA 포트 설정의 콤보박스 변경 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdmaSubCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;
            this.cdmaData = true;
        }

        /// <summary>
        /// 측기 통신 상태 항목 설정 변경 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WDeviceStateNum_ValueChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;
            this.wDeviceChkTime = true;
        }

        //측기 서버 포트 텍스트박스에 숫자만 입력 가능하게 하는 이벤트
        private void PortTB_KeyDown(object sender, KeyEventArgs e)
        {
            // 8 - 백스페이스
            // 46 - DEL
            // 37 - 좌 화살표
            // 38 - 위 화살표
            // 39 - 우 화살표
            // 40 - 아래 화살표
            // 16 - 쉬프트
            // (48 ~ 57) - 숫자 0 ~ 9 
            // (96 ~ 105) - 숫자 0 ~ 9
            // 35 - End
            // 36 - Home
            if (e.KeyValue == 8 || e.KeyValue == 46 || e.KeyValue == 16
                || e.KeyValue == 37 || e.KeyValue == 38 || e.KeyValue == 39 || e.KeyValue == 40
                || (e.KeyValue > 47 && e.KeyValue < 58)
                || (e.KeyValue > 95 && e.KeyValue < 106)
                || e.KeyValue == 35 || e.KeyValue == 36)
            {
                e.SuppressKeyPress = false;
            }
            else
            {
                e.SuppressKeyPress = true;
            }
        }

        //IP 텍스트박스에 숫자만(소숫점) 입력 가능하게 하는 이벤트
        private void TcpIp1TB_KeyDown(object sender, KeyEventArgs e)
        {
            // 8 - 백스페이스
            // 46 - DEL
            // 37 - 좌 화살표
            // 38 - 위 화살표
            // 39 - 우 화살표
            // 40 - 아래 화살표
            // 16 - 쉬프트
            // 190 - '.'
            // 110 - '.'
            // (48 ~ 57) - 숫자 0 ~ 9 
            // (96 ~ 105) - 숫자 0 ~ 9
            // 35 - End
            // 36 - Home
            if (e.KeyValue == 8 || e.KeyValue == 46 || e.KeyValue == 37 || e.KeyValue == 38
                || e.KeyValue == 39 || e.KeyValue == 40 || e.KeyValue == 16 || e.KeyValue == 190 || e.KeyValue == 110
                || (e.KeyValue > 47 && e.KeyValue < 58)
                || (e.KeyValue > 95 && e.KeyValue < 106)
                || e.KeyValue == 35 || e.KeyValue == 36)
            {
                e.SuppressKeyPress = false;
            }
            else
            {
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// TCP 서버의 변화를 체크하기 위한 메소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpTB_TextChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;
            this.TcpServer_1_IsChange = true;
        }

        /// <summary>
        /// TCP 이더넷 서버의 변화를 체크하기 위한 메소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ip_E_TB_TextChanged(object sender, EventArgs e)
        {
            this.SaveBtn.Enabled = true;
            this.TcpServer_2_IsChange = true;
        }
    }

    /// <summary>
    /// 서버연결 버튼 클릭에 대한 이벤트 아규먼트 클래스
    /// </summary>
    public class ServerStartEventArgs : EventArgs
    {
        private string ip = string.Empty;
        private int port = int.MinValue;

        public string Ip
        {
            get { return this.ip; }
        }

        public int Port
        {
            get { return this.port; }
        }

        public ServerStartEventArgs()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_port"></param>
        public ServerStartEventArgs(string _ip, int _port)
        {
            this.ip = _ip;
            this.port = _port;
        }
    }

    /// <summary>
    /// 서버종료 버튼 클릭에 대한 이벤트 아규먼트 클래스
    /// </summary>
    public class ServerStopEventArgs : EventArgs
    {
        public ServerStopEventArgs()
        {
        }
    }

    /// <summary>
    /// Tcp 연결 시 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class TcpClientConEventArgs : EventArgs
    {
        private byte num = byte.MinValue;
        private string ip = string.Empty;
        private int port = int.MinValue;

        public byte Num
        {
            get { return this.num; }
        }

        public string IP
        {
            get { return this.ip; }
        }

        public int Port
        {
            get { return this.port; }
        }

        public TcpClientConEventArgs()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_num"></param>
        public TcpClientConEventArgs(byte _num, string _ip, int _port)
        {
            this.num = _num;
            this.ip = _ip;
            this.port = _port;
        }
    }

    /// <summary>
    /// Tcp 연결 종료 시 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class TcpClientConEndEventArgs : EventArgs
    {
        private byte num = byte.MinValue;

        public byte Num
        {
            get { return this.num; }
        }

        public TcpClientConEndEventArgs()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_num"></param>
        public TcpClientConEndEventArgs(byte _num)
        {
            this.num = _num;
        }
    }

    /// <summary>
    /// Serial 연결 시 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class SerialConEventArgs : EventArgs
    {
        private byte num = byte.MinValue;
        private string comPort = string.Empty;
        private string baudRate = string.Empty;
        private string dataBits = string.Empty;
        private string parity = string.Empty;
        private string stopBit = string.Empty;

        public byte Num
        {
            get { return this.num; }
        }

        public string ComPort
        {
            get { return this.comPort; }
        }

        public string BaudRate
        {
            get { return this.baudRate; }
        }

        public string DataBits
        {
            get { return this.dataBits; }
        }

        public string Parity
        {
            get { return this.parity; }
        }

        public string StopBit
        {
            get { return this.stopBit; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_num"></param>
        /// <param name="_comPort"></param>
        /// <param name="_baudRate"></param>
        /// <param name="_dataBits"></param>
        /// <param name="_parity"></param>
        /// <param name="_stopBits"></param>
        public SerialConEventArgs(byte _num, string _comPort, string _baudRate, string _dataBits, string _parity, string _stopBits)
        {
            this.num = _num;
            this.comPort = _comPort;
            this.baudRate = _baudRate;
            this.dataBits = _dataBits;
            this.parity = _parity;
            this.stopBit = _stopBits;
        }
    }

    /// <summary>
    /// Serial 연결종료 시 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class SerialConEndEventArgs : EventArgs
    {
        private byte num = byte.MinValue;

        public byte Num
        {
            get { return this.num; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_num"></param>
        public SerialConEndEventArgs(byte _num)
        {
            this.num = _num;
        }
    }

    /// <summary>
    /// 측기 데이터 설정 후 Main으로 알려주기 위해 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class WeatherDataOptionSetEventArgs : EventArgs
    {
        private bool dataAlarm = false;
        private bool dataSelfChk = false;
        private bool dataFWVer = false;
        private bool dataBatt = false;
        private bool dataSolar = false;
        private bool dataDoor = false;

        public bool DataAlarm
        {
            get { return this.dataAlarm; }
        }

        public bool DataSelfChk
        {
            get { return this.dataSelfChk; }
        }

        public bool DataFWVer
        {
            get { return this.dataFWVer; }
        }

        public bool DataBatt
        {
            get { return this.dataBatt; }
        }

        public bool DataSolar
        {
            get { return this.dataSolar; }
        }

        public bool DataDoor
        {
            get { return this.dataDoor; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_alarm"></param>
        /// <param name="_selfChk"></param>
        /// <param name="_FWVer"></param>
        /// <param name="_batt"></param>
        /// <param name="_solar"></param>
        /// <param name="_door"></param>
        public WeatherDataOptionSetEventArgs(bool _alarm, bool _selfChk, bool _FWVer, bool _batt, bool _solar, bool _door)
        {
            this.dataAlarm = _alarm;
            this.dataSelfChk = _selfChk;
            this.dataFWVer = _FWVer;
            this.dataBatt = _batt;
            this.dataSolar = _solar;
            this.dataDoor = _door;
        }
    }

    /// <summary>
    /// DB 데이터 설정 후 Main으로 알려주기 위해 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class DBDataSetEventArgs : EventArgs
    {
        private string ip = string.Empty;
        private string port = string.Empty;
        private string id = string.Empty;
        private string pw = string.Empty;
        private string sid = string.Empty;

        public string IP
        {
            get { return this.ip; }
        }

        public string PORT
        {
            get { return this.port; }
        }

        public string ID
        {
            get { return this.id; }
        }

        public string PW
        {
            get { return this.pw; }
        }

        public string SID
        {
            get { return this.sid; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_port"></param>
        /// <param name="_id"></param>
        /// <param name="_pw"></param>
        /// <param name="_sid"></param>
        public DBDataSetEventArgs(string _ip, string _port, string _id, string _pw, string _sid)
        {
            this.ip = _ip;
            this.port = _port;
            this.id = _id;
            this.pw = _pw;
            this.sid = _sid;
        }
    }

    /// <summary>
    /// 자동로그인 체크박스 체크 변경 후 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AutoLoginEventArgs : EventArgs
    {
        private bool autoFlag = false;

        public bool AutoFlag
        {
            get { return this.autoFlag; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_autoFlag"></param>
        public AutoLoginEventArgs(bool _autoFlag)
        {
            this.autoFlag = _autoFlag;
        }
    }

    /// <summary>
    /// CDMA 포트 설정 변경 후 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class CDMAPortEventArgs : EventArgs
    {
        private string comPort = string.Empty;

        public string ComPort
        {
            get { return this.comPort; }
            set { this.comPort = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_comPort"></param>
        public CDMAPortEventArgs(string _comPort)
        {
            this.comPort = _comPort;
        }
    }

    /// <summary>
    /// TCP 사용여부를 저장할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class TcpIsUseEventArgs : EventArgs
    {
        private bool tcp1IsUse = false;
        private bool tcp2IsUse = false;
        private string tcp1Ip = string.Empty;
        private string tcp1Port = string.Empty;
        private string tcp2Ip = string.Empty;
        private string tcp2Port = string.Empty;

        public bool Tcp1IsUse
        {
            get { return this.tcp1IsUse; }
        }

        public bool Tcp2IsUse
        {
            get { return this.tcp2IsUse; }
        }

        public string Tcp1IP
        {
            get { return this.tcp1Ip; }
        }

        public string Tcp1PORT
        {
            get { return this.tcp1Port; }
        }

        public string Tcp2IP
        {
            get { return this.tcp2Ip; }
        }

        public string Tcp2PORT
        {
            get { return this.tcp2Port; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_tcp1IsUse"></param>
        /// <param name="_tcp2IsUse"></param>
        public TcpIsUseEventArgs(
            bool _tcp1IsUse,
            bool _tcp2IsUse,
            string _tcp1Ip,
            string _tcp1Port,
            string _tcp2Ip,
            string _tcp2Port)
        {
            this.tcp1IsUse = _tcp1IsUse;
            this.tcp2IsUse = _tcp2IsUse;
            this.tcp1Ip = _tcp1Ip;
            this.tcp1Port = _tcp1Port;
            this.tcp2Ip = _tcp2Ip;
            this.tcp2Port = _tcp2Port;
        }
    }

    /// <summary>
    /// Serial 사용여부를 저장할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class SerialIsUseEventArgs : EventArgs
    {
        private byte num = byte.MinValue;
        private bool isUse = false;
        private string comPort = string.Empty;
        private string baudRate = string.Empty;
        private string dataBits = string.Empty;
        private string parity = string.Empty;
        private string stopBit = string.Empty;

        public byte Num
        {
            get { return this.num; }
        }

        public bool IsUse
        {
            get { return this.isUse; }
        }

        public string ComPort
        {
            get { return this.comPort; }
        }

        public string BaudRate
        {
            get { return this.baudRate; }
        }

        public string DataBits
        {
            get { return this.dataBits; }
        }

        public string Parity
        {
            get { return this.parity; }
        }

        public string StopBit
        {
            get { return this.stopBit; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_num"></param>
        /// <param name="_isUse"></param>
        /// <param name="_comPort"></param>
        /// <param name="_baudRate"></param>
        /// <param name="_dataBits"></param>
        /// <param name="_parity"></param>
        /// <param name="_stopBits"></param>
        public SerialIsUseEventArgs(byte _num, bool _isUse, string _comPort, string _baudRate, string _dataBits, string _parity, string _stopBits)
        {
            this.num = _num;
            this.isUse = _isUse;
            this.comPort = _comPort;
            this.baudRate = _baudRate;
            this.dataBits = _dataBits;
            this.parity = _parity;
            this.stopBit = _stopBits;
        }
    }

    /// <summary>
    /// 측기 체크 시간 변경 시 발생하는 이벤트 아규먼트 클래스
    /// </summary>
    public class WDeviceChkTimeEventArgs : EventArgs
    {
        private string chkTime = string.Empty;

        public string ChkTime
        {
            get { return this.chkTime; }
            set { this.chkTime = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_chkTime"></param>
        public WDeviceChkTimeEventArgs(string _chkTime)
        {
            this.chkTime = _chkTime;
        }
    }
}