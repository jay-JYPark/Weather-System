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
    public partial class WeatherSelfForm : Form
    {
        private WeatherDataMng dataMng = null;
        private delegate void InvokeSetWDeviceItemData(WDeviceItem _wdi);

        public WeatherSelfForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onAddWDeviceItemDataEvt += new EventHandler<AddWDeviceItemDataEventArgs>(dataMng_onAddWDeviceItemDataEvt);
            this.dataMng.onAddWDeviceEvt += new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onUpdateWDeviceEvt += new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt += new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
            this.init();
            this.DeviceInit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.dataMng.onAddWDeviceItemDataEvt -= new EventHandler<AddWDeviceItemDataEventArgs>(dataMng_onAddWDeviceItemDataEvt);
            this.dataMng.onAddWDeviceEvt -= new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onUpdateWDeviceEvt -= new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt -= new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
        }

        //측기 삭제 시 이벤트
        private void dataMng_onDeleteWDeviceEvt(object sender, DeleteWDeviceEventArgs e)
        {
            for (int i = 0; i < e.WDList.Count; i++)
            {
                if (this.SelfTestDeviceLV.Items.ContainsKey(e.WDList[i].PKID.ToString()))
                {
                    this.SelfTestDeviceLV.Items[e.WDList[i].PKID.ToString()].Remove();
                }
            }

            this.SetListViewIndex(this.SelfTestDeviceLV);
        }

        //측기 수정 시 이벤트
        private void dataMng_onUpdateWDeviceEvt(object sender, UpdateWDeviceEventArgs e)
        {
            if (this.SelfTestDeviceLV.Items.ContainsKey(e.WD.PKID.ToString()))
            {
                this.SelfTestDeviceLV.Items[e.WD.PKID.ToString()].SubItems[2].Text = string.Format("{0}({1})", e.WD.Name, this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name);
            }
        }

        //측기 등록 시 이벤트
        private void dataMng_onAddWDeviceEvt(object sender, AddWDeviceEventArgs e)
        {
            if (this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name == "RAT")
            {
                this.SelfTestDeviceLV.Items.Add(this.GetListViewItem(e.WD));
                this.SetListViewIndex(this.SelfTestDeviceLV);
            }
        }

        /// <summary>
        /// 측기의 센서/태양전지 상태를 받는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWDeviceItemDataEvt(object sender, AddWDeviceItemDataEventArgs e)
        {
            if (this.SelfTestDeviceLV.InvokeRequired)
            {
                this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { e.WDI });
            }
            else
            {
                this.SetWDeviceItemDataM(e.WDI);
            }
        }

        /// <summary>
        /// 측기의 데이터 또는 상태 값 등을 받아 리스트에 셋팅한다.
        /// </summary>
        /// <param name="_wdi"></param>
        public void SetWDeviceItemDataM(WDeviceItem _wdi)
        {
            switch (_wdi.FKDeviceItem)
            {
                case (uint)WeatherDataMng.WIType.강수센서상태:
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[3].Text = string.Format("{0}",
                        (_wdi.Value == "0") ? "정상" :
                        (_wdi.Value == "1") ? "이상" :
                        (_wdi.Value == "2") ? "이상(합선)" : "이상(단선)");

                    if (_wdi.Value == "0")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[3].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1" || _wdi.Value == "2" || _wdi.Value == "3")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[3].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.수위센서상태:
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[4].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" : "이상");

                    if (_wdi.Value == "0")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[4].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[4].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.유속센서상태:
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[5].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" : "이상");

                    if (_wdi.Value == "0")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[5].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[5].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.태양전지:
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[6].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" : "저전압");

                    if (_wdi.Value == "0")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[6].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1")
                    {
                        this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].SubItems[6].ForeColor = Color.OrangeRed;
                    }
                    break;
            }

            if (this.SelfTestDeviceLV.SelectedItems.Count == 1)
            {
                if (this.SelfTestDeviceLV.SelectedItems[0].Name == _wdi.FKDevice.ToString())
                {
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].Selected = false;
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].Focused = false;
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].Selected = true;
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].Focused = true;
                    this.SelfTestDeviceLV.Items[_wdi.FKDevice.ToString()].EnsureVisible();
                }
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void init()
        {
            #region 리스트뷰 초기화
            ColumnHeader dIcon = new ColumnHeader();
            dIcon.Text = string.Empty;
            dIcon.Width = 0;
            this.SelfTestDeviceLV.Columns.Add(dIcon);

            ColumnHeader dPkid = new ColumnHeader();
            dPkid.Text = "번호";
            dPkid.Width = 43;
            this.SelfTestDeviceLV.Columns.Add(dPkid);

            ColumnHeader dName = new ColumnHeader();
            dName.Text = "이름";
            dName.Width = 180;
            dName.TextAlign = HorizontalAlignment.Center;
            this.SelfTestDeviceLV.Columns.Add(dName);

            ColumnHeader dRFs = new ColumnHeader();
            dRFs.Text = "강수센서 상태";
            dRFs.Width = 110;
            dRFs.TextAlign = HorizontalAlignment.Center;
            this.SelfTestDeviceLV.Columns.Add(dRFs);

            ColumnHeader dWLs = new ColumnHeader();
            dWLs.Text = "수위센서 상태";
            dWLs.Width = 110;
            dWLs.TextAlign = HorizontalAlignment.Center;
            this.SelfTestDeviceLV.Columns.Add(dWLs);

            ColumnHeader dWSs = new ColumnHeader();
            dWSs.Text = "유속센서 상태";
            dWSs.Width = 110;
            dWSs.TextAlign = HorizontalAlignment.Center;
            this.SelfTestDeviceLV.Columns.Add(dWSs);

            ColumnHeader dSunBatt = new ColumnHeader();
            dSunBatt.Text = "태양전지 상태";
            dSunBatt.Width = 110;
            dSunBatt.TextAlign = HorizontalAlignment.Center;
            this.SelfTestDeviceLV.Columns.Add(dSunBatt);
            #endregion
        }

        /// <summary>
        /// 측기 리스트 초기화
        /// </summary>
        private void DeviceInit()
        {
            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                if (this.dataMng.GetTypeDevice(this.dataMng.DeviceList[i].TypeDevice).Name == "RAT")
                {
                    this.SelfTestDeviceLV.Items.Add(this.GetListViewItem(this.dataMng.DeviceList[i]));
                }
            }
        }

        /// <summary>
        /// WDevice 클래스를 받아 LisetViewItem으로 반환한다.
        /// </summary>
        /// <param name="_wd"></param>
        /// <returns></returns>
        private ListViewItem GetListViewItem(WDevice _wd)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.UseItemStyleForSubItems = false;

            lvi.Name = string.Format("{0}", _wd.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", this.SelfTestDeviceLV.Items.Count + 1));
            lvi.SubItems.Add(string.Format("{0}({1})", _wd.Name, this.dataMng.GetTypeDevice(_wd.TypeDevice).Name));
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);

            return lvi;
        }

        /// <summary>
        /// 리스트뷰의 인덱스를 처음부터 다시 셋팅한다.
        /// </summary>
        /// <param name="lv"></param>
        private void SetListViewIndex(ListView lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                int c = i + 1;
                lv.Items[i].SubItems[1].Text = c.ToString();
            }
        }

        //요청 버튼 클릭
        private void SelfTestBtn_Click(object sender, EventArgs e)
        {
            if (this.SelfTestDeviceLV.SelectedItems.Count == 0)
            {
                MessageBox.Show("요청할 측기를 선택하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.RainFallCB.Checked == false && this.WaterLevelCB.Checked == false
                && this.WaterFlowCB.Checked == false && this.SunBattCB.Checked == false)
            {
                MessageBox.Show("요청 항목을 선택하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.SunBattCB.Checked)
            {
                MessageBox.Show("태양전지는 센서가 위치한 지역의 일조량이 충분해야 정확한 상태를 진단합니다.",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            WaitBarMng.Start();
            Thread.Sleep(2000);

            for (int i = 0; i < this.SelfTestDeviceLV.SelectedItems.Count; i++)
            {
                WDevice tmpDevice = this.dataMng.GetWDevice(uint.Parse(this.SelfTestDeviceLV.SelectedItems[i].Name));

                if (this.RainFallCB.Checked)
                {
                    WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.강수센서상태;
                    WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                    WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                    this.dataMng.AddDeviceRequest(tmp);

                    CProto06 cProto06 = CProtoMng.GetProtoObj("06") as CProto06;
                    cProto06.Header = "[";
                    cProto06.Length = "022";
                    cProto06.ID = tmpDevice.ID;
                    cProto06.MainCode = "0";
                    cProto06.SubCode = "O";
                    cProto06.RecvType = "1";
                    cProto06.Data = "1";
                    cProto06.CRC = "00000";
                    cProto06.Tail = "]";

                    if (tmpDevice.EthernetUse)
                    {
                        cProto06.RecvType = "3";
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendEthernetMsg(tmpDevice.ID, buff);
                    }
                    else
                    {
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendSmsMsg(tmpDevice.CellNumber, buff);
                    }
                    
                    Thread.Sleep(20);
                }

                if (this.WaterLevelCB.Checked)
                {
                    WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.수위센서상태;
                    WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                    WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                    this.dataMng.AddDeviceRequest(tmp);

                    CProto06 cProto06 = CProtoMng.GetProtoObj("06") as CProto06;
                    cProto06.Header = "[";
                    cProto06.Length = "022";
                    cProto06.ID = tmpDevice.ID;
                    cProto06.MainCode = "0";
                    cProto06.SubCode = "O";
                    cProto06.RecvType = "1";
                    cProto06.Data = "2";
                    cProto06.CRC = "00000";
                    cProto06.Tail = "]";

                    if (tmpDevice.EthernetUse)
                    {
                        cProto06.RecvType = "3";
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendEthernetMsg(tmpDevice.ID, buff);
                    }
                    else
                    {
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendSmsMsg(tmpDevice.CellNumber, buff);
                    }
                    
                    Thread.Sleep(20);
                }

                if (this.WaterFlowCB.Checked)
                {
                    WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.유속센서상태;
                    WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                    WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                    this.dataMng.AddDeviceRequest(tmp);

                    CProto06 cProto06 = CProtoMng.GetProtoObj("06") as CProto06;
                    cProto06.Header = "[";
                    cProto06.Length = "022";
                    cProto06.ID = tmpDevice.ID;
                    cProto06.MainCode = "0";
                    cProto06.SubCode = "O";
                    cProto06.RecvType = "1";
                    cProto06.Data = "3";
                    cProto06.CRC = "00000";
                    cProto06.Tail = "]";

                    if (tmpDevice.EthernetUse)
                    {
                        cProto06.RecvType = "3";
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendEthernetMsg(tmpDevice.ID, buff);
                    }
                    else
                    {
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendSmsMsg(tmpDevice.CellNumber, buff);
                    }
                    
                    Thread.Sleep(20);
                }

                if (this.SunBattCB.Checked)
                {
                    WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.태양전지;
                    WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                    WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                    this.dataMng.AddDeviceRequest(tmp);

                    CProto06 cProto06 = CProtoMng.GetProtoObj("06") as CProto06;
                    cProto06.Header = "[";
                    cProto06.Length = "022";
                    cProto06.ID = tmpDevice.ID;
                    cProto06.MainCode = "0";
                    cProto06.SubCode = "O";
                    cProto06.RecvType = "1";
                    cProto06.Data = "4";
                    cProto06.CRC = "00000";
                    cProto06.Tail = "]";

                    if (tmpDevice.EthernetUse)
                    {
                        cProto06.RecvType = "3";
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendEthernetMsg(tmpDevice.ID, buff);
                    }
                    else
                    {
                        byte[] buff = cProto06.MakeProto();
                        this.dataMng.SendSmsMsg(tmpDevice.CellNumber, buff);
                    }
                    
                    Thread.Sleep(20);
                }
            }

            WaitBarMng.Close();
        }

        //닫기 버튼 클릭
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}