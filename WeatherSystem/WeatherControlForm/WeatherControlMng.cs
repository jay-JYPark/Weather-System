using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ADEng.Library.WeatherSystem;
using ADEng.Control.WeatherSystem;

namespace ADEng.Module.WeatherSystem
{
    public partial class WeatherControlMng : Form
    {
        private WeatherDataMng dataMng = null;
        private List<uint> WDevice = new List<uint>();

        public WeatherControlMng()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_WDevice"></param>
        public WeatherControlMng(List<uint> _WDevice)
        {
            InitializeComponent();
            this.WDevice = _WDevice;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dataMng = WeatherDataMng.getInstance();
            this.AlarmRB.Checked = true;

            for (int i = 0; i < this.dataMng.TypeSensorList.Count; i++)
            {
                this.AlarmCB.Items.Add((string)this.dataMng.TypeSensorList[i].Name);
            }

            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                WTypeDevice typeDevice = this.dataMng.GetTypeDevice(this.dataMng.DeviceList[i].TypeDevice);

                if (typeDevice.Name != "HSD" && typeDevice.Name != "DSD")
                {
                    this.WDeviceTV.Nodes.Add(this.dataMng.DeviceList[i].PKID.ToString(), string.Format("{0}({1})", this.dataMng.DeviceList[i].Name, typeDevice.Name));
                }
            }

            for (int i = 0; i < this.WDevice.Count; i++)
            {
                if (this.WDeviceTV.Nodes.ContainsKey(this.WDevice[i].ToString()))
                {
                    this.WDeviceTV.Nodes[this.WDevice[i].ToString()].Checked = true;
                }
            }
        }

        /// <summary>
        /// 제어를 하기 전 유효성 검사를 진행한다.
        /// </summary>
        private bool TreeViewValidation()
        {
            try
            {
                //선택한 측기 검사
                int selectCnt = 0;

                for (int i = 0; i < this.WDeviceTV.Nodes.Count; i++)
                {
                    if (this.WDeviceTV.Nodes[i].Checked)
                    {
                        selectCnt++;
                    }
                }

                if (selectCnt == 0)
                {
                    return false;
                }

                if (this.AlarmRB.Checked) //임계치 선택 검사
                {
                    if (this.AlarmCB.Text == string.Empty || this.Alarm1TB.Text == string.Empty
                        || this.Alarm2TB.Text == string.Empty || this.Alarm3TB.Text == string.Empty)
                    {
                        return false;
                    }

                    double tmpInt = int.MinValue;
                    double tmpAlarm = double.MinValue;

                    if (!double.TryParse(this.Alarm1TB.Text, out tmpInt))
                    {
                        return false;
                    }

                    if (!double.TryParse(this.Alarm2TB.Text, out tmpInt))
                    {
                        return false;
                    }

                    if (!double.TryParse(this.Alarm3TB.Text, out tmpInt))
                    {
                        return false;
                    }

                    if ((double.Parse(this.Alarm1TB.Text) < double.Parse(this.Alarm2TB.Text)) &&
                        double.Parse(this.Alarm2TB.Text) < double.Parse(this.Alarm3TB.Text))
                    {
                    }
                    else
                    {
                        return false;
                    }

                    if ((string)this.AlarmCB.SelectedItem == "우량계")
                    {
                        tmpAlarm = 999.9;
                    }
                    else if ((string)this.AlarmCB.SelectedItem == "수위계")
                    {
                        tmpAlarm = 10.0;
                    }
                    else if ((string)this.AlarmCB.SelectedItem == "유속계")
                    {
                        tmpAlarm = 9.9;
                    }

                    if (double.Parse(this.Alarm1TB.Text) < 0 || double.Parse(this.Alarm1TB.Text) > tmpAlarm)
                    {
                        return false;
                    }

                    if (double.Parse(this.Alarm2TB.Text) < 0 || double.Parse(this.Alarm2TB.Text) > tmpAlarm)
                    {
                        return false;
                    }

                    if (double.Parse(this.Alarm3TB.Text) < 0 || double.Parse(this.Alarm3TB.Text) > tmpAlarm)
                    {
                        return false;
                    }
                }
                else if (this.FTimeRB.Checked) //무시시간 선택 검사
                {
                    if (this.FTime1TB.Text == string.Empty || this.FTime2TB.Text == string.Empty)
                    {
                        return false;
                    }
                }
                else if (this.IpPortRB.Checked) //CDMA IP, PORT 선택 검사
                {
                    if (this.IpPort1TB.Text == string.Empty || this.IpPort2TB.Text == string.Empty)
                    {
                        return false;
                    }

                    if (this.IpPort1TB.Text.Length < 7)
                    {
                        return false;
                    }

                    string[] tmpStr = this.IpPort1TB.Text.Split(new char[] { '.' });
                    int tmpInt = int.MinValue;

                    if (tmpStr.Length != 4)
                    {
                        return false;
                    }

                    for (int i = 0; i < tmpStr.Length; i++)
                    {
                        if (int.Parse(tmpStr[i]) > 255)
                        {
                            return false;
                        }
                    }

                    if (!int.TryParse(this.IpPort2TB.Text, out tmpInt))
                    {
                        return false;
                    }

                    if (int.Parse(this.IpPort2TB.Text) == 0)
                    {
                        return false;
                    }
                }
                else if (this.EIpPortRB.Checked) //이더넷 IP, PORT 선택 검사
                {
                    if (this.EIpPort1TB.Text == string.Empty || this.EIpPort2TB.Text == string.Empty)
                    {
                        return false;
                    }

                    if (this.EIpPort1TB.Text.Length < 7)
                    {
                        return false;
                    }

                    string[] tmpStr = this.EIpPort1TB.Text.Split(new char[] { '.' });
                    int tmpInt = int.MinValue;

                    if (tmpStr.Length != 4)
                    {
                        return false;
                    }

                    for (int i = 0; i < tmpStr.Length; i++)
                    {
                        if (int.Parse(tmpStr[i]) > 255)
                        {
                            return false;
                        }
                    }

                    if (!int.TryParse(this.EIpPort2TB.Text, out tmpInt))
                    {
                        return false;
                    }

                    if (int.Parse(this.EIpPort2TB.Text) == 0)
                    {
                        return false;
                    }
                }
                else if (this.UpgradeRB.Checked) //업그레이드 선택 검사
                {
                    if (this.Upgrade1TB.Text == string.Empty || this.Upgrade2TB.Text == string.Empty)
                    {
                        return false;
                    }

                    if (this.Upgrade1TB.Text.Length < 7)
                    {
                        return false;
                    }

                    string[] tmpStr = this.Upgrade1TB.Text.Split(new char[] { '.' });
                    int tmpInt = int.MinValue;

                    if (tmpStr.Length != 4)
                    {
                        return false;
                    }

                    for (int i = 0; i < tmpStr.Length; i++)
                    {
                        if (int.Parse(tmpStr[i]) > 255)
                        {
                            return false;
                        }
                    }

                    if (!int.TryParse(this.Upgrade2TB.Text, out tmpInt))
                    {
                        return false;
                    }

                    if (int.Parse(this.Upgrade2TB.Text) == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //제어 버튼 클릭
        private void RequestBtn_Click(object sender, EventArgs e)
        {
            //유효성 검사
            if (!this.TreeViewValidation())
            {
                MessageBox.Show("제어할 측기를 선택하거나 항목을 올바르게 넣어주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < this.WDeviceTV.Nodes.Count; i++)
            {
                if (this.WDeviceTV.Nodes[i].Checked)
                {
                    WaitBarMng.Start();
                    Thread.Sleep(1500);

                    WDevice wDevice = this.dataMng.GetWDevice(uint.Parse(this.WDeviceTV.Nodes[i].Name));
                    WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(wDevice.TypeDevice);

                    if (wTypeTmp.Name == "RAT") //RAT 우량기를 선택했을 때..
                    {
                        if (this.AlarmRB.Checked) //임계치 선택
                        {
                            //DB 저장
                            if (this.AlarmCB.Text == "우량계")
                            {
                                WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.강수임계치1단계;
                                WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm1TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.강수임계치2단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm2TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.강수임계치3단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm3TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);
                            }
                            else if (this.AlarmCB.Text == "수위계")
                            {
                                WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.수위임계치1단계;
                                WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm1TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.수위임계치2단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm2TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.수위임계치3단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm3TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);
                            }
                            else if (this.AlarmCB.Text == "유속계")
                            {
                                WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.유속임계치1단계;
                                WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm1TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.유속임계치2단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm2TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.유속임계치3단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm3TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);
                            }

                            CProto01 cProto01 = CProtoMng.GetProtoObj("01") as CProto01;
                            cProto01.Header = "[";
                            cProto01.Length = "037";
                            cProto01.ID = wDevice.ID;
                            cProto01.MainCode = "1";
                            cProto01.SubCode = "L";
                            cProto01.RecvType = "1";

                            if (this.AlarmCB.Text == "우량계")
                            {
                                cProto01.Data = "1";
                                int tmpDouble1 = (int)(double.Parse(this.Alarm1TB.Text) * 10);
                                int tmpDouble2 = (int)(double.Parse(this.Alarm2TB.Text) * 10);
                                int tmpDouble3 = (int)(double.Parse(this.Alarm3TB.Text) * 10);

                                cProto01.Data += string.Format("{0}{1}{2}", tmpDouble1.ToString().PadLeft(5, '0'),
                                    tmpDouble2.ToString().PadLeft(5, '0'), tmpDouble3.ToString().PadLeft(5, '0'));
                            }
                            else if (this.AlarmCB.Text == "수위계")
                            {
                                cProto01.Data = "2";
                                int tmpDouble1 = (int)(double.Parse(this.Alarm1TB.Text) * 10);
                                int tmpDouble2 = (int)(double.Parse(this.Alarm2TB.Text) * 10);
                                int tmpDouble3 = (int)(double.Parse(this.Alarm3TB.Text) * 10);

                                cProto01.Data += string.Format("{0}0{1}0{2}0", tmpDouble1.ToString().PadLeft(4, '0'),
                                    tmpDouble2.ToString().PadLeft(4, '0'), tmpDouble3.ToString().PadLeft(4, '0'));
                            }
                            else if (this.AlarmCB.Text == "유속계")
                            {
                                cProto01.Data = "3";
                                int tmpDouble1 = (int)(double.Parse(this.Alarm1TB.Text) * 10);
                                int tmpDouble2 = (int)(double.Parse(this.Alarm2TB.Text) * 10);
                                int tmpDouble3 = (int)(double.Parse(this.Alarm3TB.Text) * 10);

                                cProto01.Data += string.Format("{0}{1}{2}", tmpDouble1.ToString().PadLeft(5, '0'),
                                    tmpDouble2.ToString().PadLeft(5, '0'), tmpDouble3.ToString().PadLeft(5, '0'));
                            }
                            
                            cProto01.CRC = "00000";
                            cProto01.Tail = "]";

                            byte[] buff = cProto01.MakeProto();

                            if (wDevice.EthernetUse)
                            {
                                cProto01.RecvType = "3";
                                byte[] eBuff = cProto01.MakeProto();
                                this.dataMng.SendEthernetMsg(wDevice.ID, eBuff);
                            }
                            else
                            {
                                this.dataMng.SendSmsMsg(wDevice.CellNumber, buff);
                            }
                        }
                        else if (this.FTimeRB.Checked) //무시시간 선택
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.동일레벨무시시간;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.FTime1TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            wiTypeAll = WeatherDataMng.WIType.하향레벨무시시간;
                            AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.FTime2TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            CProto02 cProto02 = CProtoMng.GetProtoObj("02") as CProto02;
                            cProto02.Header = "[";
                            cProto02.Length = "040";
                            cProto02.ID = wDevice.ID;
                            cProto02.MainCode = "1";
                            cProto02.SubCode = "s";
                            cProto02.RecvType = "1";
                            cProto02.Data = string.Format("010010010010{0}{1}0", this.FTime1TB.Text.PadLeft(3, '0'), this.FTime2TB.Text.PadLeft(3, '0'));
                            cProto02.CRC = "00000";
                            cProto02.Tail = "]";

                            if (wDevice.EthernetUse)
                            {
                                cProto02.RecvType = "3";
                                byte[] buff = cProto02.MakeProto();
                                this.dataMng.SendEthernetMsg(wDevice.ID, buff);
                            }
                            else
                            {
                                byte[] buff = cProto02.MakeProto();
                                this.dataMng.SendSmsMsg(wDevice.CellNumber, buff);
                            }
                        }
                        else if (this.IpPortRB.Checked) //CDMA IP, PORT 선택
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.IP;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.IpPort1TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            wiTypeAll = WeatherDataMng.WIType.PORT;
                            AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.IpPort2TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            CProto03 cProto03 = CProtoMng.GetProtoObj("03") as CProto03;
                            cProto03.Header = "[";
                            cProto03.Length = "040";
                            cProto03.ID = wDevice.ID;
                            cProto03.MainCode = "1";
                            cProto03.SubCode = "g";
                            cProto03.RecvType = "1";
                            string[] tmpIpStr = this.IpPort1TB.Text.Split('.');
                            cProto03.Data = string.Format("{0}.{1}.{2}.{3}{4}",
                                tmpIpStr[0].PadLeft(3, '0'),
                                tmpIpStr[1].PadLeft(3, '0'),
                                tmpIpStr[2].PadLeft(3, '0'),
                                tmpIpStr[3].PadLeft(3, '0'),
                                this.IpPort2TB.Text.PadLeft(4, '0'));
                            cProto03.CRC = "00000";
                            cProto03.Tail = "]";

                            if (wDevice.EthernetUse)
                            {
                                cProto03.RecvType = "3";
                                byte[] buff = cProto03.MakeProto();
                                this.dataMng.SendEthernetMsg(wDevice.ID, buff);
                            }
                            else
                            {
                                byte[] buff = cProto03.MakeProto();
                                this.dataMng.SendSmsMsg(wDevice.CellNumber, buff);
                            }
                        }
                        else if (this.EIpPortRB.Checked) //이더넷 IP, PORT 선택
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.이더넷IP;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.EIpPort1TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            wiTypeAll = WeatherDataMng.WIType.이더넷PORT;
                            AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.EIpPort2TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            CProto09 cProto09 = CProtoMng.GetProtoObj("09") as CProto09;
                            cProto09.Header = "[";
                            cProto09.Length = "040";
                            cProto09.ID = wDevice.ID;
                            cProto09.MainCode = "1";
                            cProto09.SubCode = "h";
                            cProto09.RecvType = "1";
                            string[] tmpIpStr = this.EIpPort1TB.Text.Split('.');
                            cProto09.Data = string.Format("{0}.{1}.{2}.{3}{4}",
                                tmpIpStr[0].PadLeft(3, '0'),
                                tmpIpStr[1].PadLeft(3, '0'),
                                tmpIpStr[2].PadLeft(3, '0'),
                                tmpIpStr[3].PadLeft(3, '0'),
                                this.EIpPort2TB.Text.PadLeft(4, '0'));
                            cProto09.CRC = "00000";
                            cProto09.Tail = "]";

                            if (wDevice.EthernetUse)
                            {
                                cProto09.RecvType = "3";
                                byte[] buff = cProto09.MakeProto();
                                this.dataMng.SendEthernetMsg(wDevice.ID, buff);
                            }
                            else
                            {
                                byte[] buff = cProto09.MakeProto();
                                this.dataMng.SendSmsMsg(wDevice.CellNumber, buff);
                            }
                        }
                        else if (this.UpgradeRB.Checked) //업그레이드 선택
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.업그레이드IP;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Upgrade1TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            wiTypeAll = WeatherDataMng.WIType.업그레이드PORT;
                            AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Upgrade2TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            CProto04 cProto04 = CProtoMng.GetProtoObj("04") as CProto04;
                            cProto04.Header = "[";
                            cProto04.Length = "038";
                            cProto04.ID = wDevice.ID;
                            cProto04.MainCode = "2";
                            cProto04.SubCode = "c";
                            cProto04.RecvType = "1";
                            string[] tmpUpgradeStr = this.Upgrade1TB.Text.Split('.');
                            cProto04.Data = string.Format("1{0}{1}{2}{3}{4}",
                                tmpUpgradeStr[0].PadLeft(3, '0'),
                                tmpUpgradeStr[1].PadLeft(3, '0'),
                                tmpUpgradeStr[2].PadLeft(3, '0'),
                                tmpUpgradeStr[3].PadLeft(3, '0'),
                                this.Upgrade2TB.Text.PadLeft(4, '0'));
                            cProto04.CRC = "00000";
                            cProto04.Tail = "]";

                            if (wDevice.EthernetUse)
                            {
                                cProto04.RecvType = "3";
                                byte[] buff = cProto04.MakeProto();
                                this.dataMng.SendEthernetMsg(wDevice.ID, buff);
                            }
                            else
                            {
                                byte[] buff = cProto04.MakeProto();
                                this.dataMng.SendSmsMsg(wDevice.CellNumber, buff);
                            }
                        }
                        else if (this.ResetRB.Checked) //리셋 선택
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.RESET;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, string.Empty);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            CProto05 cProto05 = CProtoMng.GetProtoObj("05") as CProto05;
                            cProto05.Header = "[";
                            cProto05.Length = "022";
                            cProto05.ID = wDevice.ID;
                            cProto05.MainCode = "2";
                            cProto05.SubCode = "c";
                            cProto05.RecvType = "1";
                            cProto05.Data = string.Format("0");
                            cProto05.CRC = "00000";
                            cProto05.Tail = "]";

                            if (wDevice.EthernetUse)
                            {
                                cProto05.RecvType = "3";
                                byte[] buff = cProto05.MakeProto();
                                this.dataMng.SendEthernetMsg(wDevice.ID, buff);
                            }
                            else
                            {
                                byte[] buff = cProto05.MakeProto();
                                this.dataMng.SendSmsMsg(wDevice.CellNumber, buff);
                            }
                        }
                    }
                    else if (wTypeTmp.Name == "WOU") //WOU 우량기를 선택했을 때..
                    {
                        if (this.AlarmRB.Checked) //임계치 선택
                        {
                            //DB 저장
                            if (this.AlarmCB.Text == "우량계")
                            {
                                WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.강수임계치1단계;
                                WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm1TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.강수임계치2단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm2TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.강수임계치3단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm3TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);
                            }
                            else if (this.AlarmCB.Text == "수위계")
                            {
                                WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.수위임계치1단계;
                                WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm1TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.수위임계치2단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm2TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.수위임계치3단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm3TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);
                            }
                            else if (this.AlarmCB.Text == "유속계")
                            {
                                WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.유속임계치1단계;
                                WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm1TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.유속임계치2단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm2TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);

                                wiTypeAll = WeatherDataMng.WIType.유속임계치3단계;
                                AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.Alarm3TB.Text);
                                this.dataMng.AddDeviceRequest(tmp);
                                Thread.Sleep(20);
                            }

                            byte tmpByte = byte.MinValue;
                            string tmpValue1 = string.Empty;
                            string tmpValue2 = string.Empty;
                            string tmpValue3 = string.Empty;

                            if (this.AlarmCB.Text == "우량계")
                            {
                                tmpByte = 1;
                                int tmpDouble1 = (int)(double.Parse(this.Alarm1TB.Text) * 10);
                                int tmpDouble2 = (int)(double.Parse(this.Alarm2TB.Text) * 10);
                                int tmpDouble3 = (int)(double.Parse(this.Alarm3TB.Text) * 10);

                                tmpValue1 = string.Format("{0}{1}{2}", tmpDouble1.ToString().PadLeft(5, '0'),
                                    tmpDouble2.ToString().PadLeft(5, '0'), tmpDouble3.ToString().PadLeft(5, '0'));
                            }
                            else if (this.AlarmCB.Text == "수위계")
                            {
                                tmpByte = 2;
                                int tmpDouble1 = (int)(double.Parse(this.Alarm1TB.Text) * 10);
                                int tmpDouble2 = (int)(double.Parse(this.Alarm2TB.Text) * 10);
                                int tmpDouble3 = (int)(double.Parse(this.Alarm3TB.Text) * 10);

                                tmpValue2 = string.Format("{0}0{1}0{2}0", tmpDouble1.ToString().PadLeft(4, '0'),
                                    tmpDouble2.ToString().PadLeft(4, '0'), tmpDouble3.ToString().PadLeft(4, '0'));
                            }
                            else if (this.AlarmCB.Text == "유속계")
                            {
                                tmpByte = 3;
                                int tmpDouble1 = (int)(double.Parse(this.Alarm1TB.Text) * 10);
                                int tmpDouble2 = (int)(double.Parse(this.Alarm2TB.Text) * 10);
                                int tmpDouble3 = (int)(double.Parse(this.Alarm3TB.Text) * 10);

                                tmpValue3 = string.Format("{0}{1}{2}", tmpDouble1.ToString().PadLeft(5, '0'),
                                    tmpDouble2.ToString().PadLeft(5, '0'), tmpDouble3.ToString().PadLeft(5, '0'));
                            }

                            this.dataMng.WOUWDeviceAlarmCtr(wDevice.PKID, tmpByte, tmpValue1, tmpValue2, tmpValue3);
                        }
                        else if (this.FTimeRB.Checked) //무시시간 선택
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.동일레벨무시시간;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.FTime1TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            wiTypeAll = WeatherDataMng.WIType.하향레벨무시시간;
                            AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            tmp = new WDeviceRequest(0, wDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 1, this.FTime2TB.Text);
                            this.dataMng.AddDeviceRequest(tmp);
                            Thread.Sleep(20);

                            this.dataMng.WOUWDeviceFTimeCtr(wDevice.PKID, this.FTime1TB.Text.PadLeft(3, '0'), this.FTime2TB.Text.PadLeft(3, '0'));
                        }
                    }
                }
            }

            WaitBarMng.Close();
        }

        //닫기 버튼 클릭
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //임계치 설정 라디오버튼 체크 체인지 이벤트
        private void AlarmRB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AlarmRB.Checked)
            {
                AlarmCB.Enabled = true;
                Alarm1TB.Enabled = true;
                Alarm2TB.Enabled = true;
                Alarm3TB.Enabled = true;

                if ((string)this.AlarmCB.SelectedItem == "우량계")
                {
                    this.ControlTipLB.Text = string.Format("※우량계 범위(mm) : 0 ~ 999.9");
                }
                else if ((string)this.AlarmCB.SelectedItem == "수위계")
                {
                    this.ControlTipLB.Text = string.Format("※수위계 범위(meter) : 0 ~ 10.0");
                }
                else if ((string)this.AlarmCB.SelectedItem == "유속계")
                {
                    this.ControlTipLB.Text = string.Format("※유속계 범위(m/s) : 0 ~ 9.9");
                }
            }
            else
            {
                AlarmCB.Enabled = false;
                Alarm1TB.Enabled = false;
                Alarm2TB.Enabled = false;
                Alarm3TB.Enabled = false;
                this.ControlTipLB.Text = string.Format("");
            }
        }

        //무시시간 설정 라디오버튼 체크 체인지 이벤트
        private void FTimeRB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.FTimeRB.Checked)
            {
                this.FTime1TB.Enabled = true;
                this.FTime2TB.Enabled = true;
                this.ControlTipLB.Text = string.Format("※무시시간 범위(분) : 0 ~ 999");
            }
            else
            {
                this.FTime1TB.Enabled = false;
                this.FTime2TB.Enabled = false;
                this.ControlTipLB.Text = string.Format("");
            }
        }

        //CDMA IP, PORT 설정 라디오버튼 체크 체인지 이벤트
        private void IpPortRB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IpPortRB.Checked)
            {
                this.IpPort1TB.Enabled = true;
                this.IpPort2TB.Enabled = true;
            }
            else
            {
                this.IpPort1TB.Enabled = false;
                this.IpPort2TB.Enabled = false;
            }
        }

        /// <summary>
        /// 이더넷 IP, PORT 설정 라디오버튼 체크 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EIpPortRB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EIpPortRB.Checked)
            {
                this.EIpPort1TB.Enabled = true;
                this.EIpPort2TB.Enabled = true;
            }
            else
            {
                this.EIpPort1TB.Enabled = false;
                this.EIpPort2TB.Enabled = false;
            }
        }

        //원격 업그레이드 라디오버튼 체크 체인지 이벤트
        private void UpgradeRB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.UpgradeRB.Checked)
            {
                this.Upgrade1TB.Enabled = true;
                this.Upgrade2TB.Enabled = true;
            }
            else
            {
                this.Upgrade1TB.Enabled = false;
                this.Upgrade2TB.Enabled = false;
            }
        }

        //우량기 선택 트리뷰 아이템 더블 클릭
        private void WDeviceTV_DoubleClick(object sender, EventArgs e)
        {
            if (this.WDeviceTV.SelectedNode.Checked)
            {
                this.WDeviceTV.SelectedNode.Checked = false;
            }
            else
            {
                this.WDeviceTV.SelectedNode.Checked = true;
            }
        }

        //포트 텍스트박스에 숫자만 입력 가능하게 하는 이벤트
        private void IpPort2TB_KeyDown(object sender, KeyEventArgs e)
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

        //임계치 설정에 의해 센서를 선택하는 이벤트
        private void AlarmCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)this.AlarmCB.SelectedItem == "우량계")
            {
                this.ControlTipLB.Text = string.Format("※우량계 범위(mm) : 0 ~ 999.9");
            }
            else if ((string)this.AlarmCB.SelectedItem == "수위계")
            {
                this.ControlTipLB.Text = string.Format("※수위계 범위(meter) : 0 ~ 10.0");
            }
            else if ((string)this.AlarmCB.SelectedItem == "유속계")
            {
                this.ControlTipLB.Text = string.Format("※유속계 범위(m/s) : 0 ~ 9.9");
            }
        }

        //임계치 텍스트박스에 숫자만(소숫점) 입력 가능하게 하는 이벤트
        private void Alarm1TB_KeyDown(object sender, KeyEventArgs e)
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
    }
}