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
using Adeng.Framework.Net;

namespace ADEng.Module.WeatherSystem
{
    public partial class WeatherForm : Form
    {
        private WeatherDataMng dataMng = null;
        private WeatherControlMng controlForm = null;
        private delegate void InvokeSetWeatherDataLog(string _str);
        private delegate void InvokeSetWDeviceItemData(WDeviceItem _wdi);
        private const int NotUseIcon = 10;

        public WeatherForm()
        {
            InitializeComponent();

            this.init();
            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onAddWDeviceEvt += new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onUpdateWDeviceEvt += new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt += new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
            this.dataMng.onAddRainDataEvt += new EventHandler<AddRainDataEventArgs>(dataMng_onAddRainDataEvt);
            this.dataMng.onAddWaterLevelDataEvt += new EventHandler<AddWaterLevelEventArgs>(dataMng_onAddWaterLevelDataEvt);
            this.dataMng.onAddFlowSpeedDataEvt += new EventHandler<AddFlowSpeedEventArgs>(dataMng_onAddFlowSpeedDataEvt);
            this.dataMng.onAddAlarmDataEvt += new EventHandler<AddAlarmEventArgs>(dataMng_onAddAlarmDataEvt);
            this.dataMng.onAddWDeviceItemDataEvt += new EventHandler<AddWDeviceItemDataEventArgs>(dataMng_onAddWDeviceItemDataEvt);
            this.dataMng.onAddWDeviceRequestEvt += new EventHandler<AddWDeviceRequestEventArgs>(dataMng_onAddWDeviceRequestEvt);
            this.dataMng.onWDeviceAlarmEvt += new EventHandler<WDeviceAlarmItemsEventArgs>(dataMng_onWDeviceAlarmEvt);
            this.DeviceInit();
        }

        /// <summary>
        /// 측기 알람 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onWDeviceAlarmEvt(object sender, WDeviceAlarmItemsEventArgs e)
        {
            if (e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.FAN || e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.DOOR)
            {
                WDeviceItem wDeviceItem = new WDeviceItem(
                    e.WDeviceAlarm.PKID,
                    e.WDeviceAlarm.FKDevice,
                    e.WDeviceAlarm.FKDeviceItem,
                    e.WDeviceAlarm.DDTime,
                    e.WDeviceAlarm.Value);

                if (this.WeatherListView.InvokeRequired)
                {
                    this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { wDeviceItem });
                }
                else
                {
                    this.SetWDeviceItemDataM(wDeviceItem);
                }
            }
            else if (e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.배터리1전압상태 ||
                e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.배터리1온도상태)
            {
                WDeviceItem wDeviceItem = new WDeviceItem(
                    e.WDeviceAlarm.PKID,
                    e.WDeviceAlarm.FKDevice,
                    (uint)WeatherDataMng.WIType.배터리상태,
                    e.WDeviceAlarm.DDTime,
                    e.WDeviceAlarm.Value);

                if (this.WeatherListView.InvokeRequired)
                {
                    this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { wDeviceItem });
                }
                else
                {
                    this.SetWDeviceItemDataM(wDeviceItem);
                }
            }
            else if (e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.배터리2전압상태 ||
                e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.배터리2온도상태)
            {
                WDeviceItem wDeviceItem = new WDeviceItem(
                    e.WDeviceAlarm.PKID,
                    e.WDeviceAlarm.FKDevice,
                    (uint)WeatherDataMng.WIType.배터리2상태,
                    e.WDeviceAlarm.DDTime,
                    e.WDeviceAlarm.Value);

                if (this.WeatherListView.InvokeRequired)
                {
                    this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { wDeviceItem });
                }
                else
                {
                    this.SetWDeviceItemDataM(wDeviceItem);
                }
            }
            else if (e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.배터리1교체시기)
            {
                WDeviceItem wDeviceItem = new WDeviceItem(
                    e.WDeviceAlarm.PKID,
                    e.WDeviceAlarm.FKDevice,
                    (uint)WeatherDataMng.WIType.배터리상태,
                    e.WDeviceAlarm.DDTime,
                    "1");

                if (this.WeatherListView.InvokeRequired)
                {
                    this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { wDeviceItem });
                }
                else
                {
                    this.SetWDeviceItemDataM(wDeviceItem);
                }
            }
            else if (e.WDeviceAlarm.FKDeviceItem == (uint)WeatherDataMng.WIType.배터리2교체시기)
            {
                WDeviceItem wDeviceItem = new WDeviceItem(
                    e.WDeviceAlarm.PKID,
                    e.WDeviceAlarm.FKDevice,
                    (uint)WeatherDataMng.WIType.배터리2상태,
                    e.WDeviceAlarm.DDTime,
                    "1");

                if (this.WeatherListView.InvokeRequired)
                {
                    this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { wDeviceItem });
                }
                else
                {
                    this.SetWDeviceItemDataM(wDeviceItem);
                }
            }
        }

        /// <summary>
        /// RAT 측기가 주는 기상데이터 & 알람데이터 & 측기데이터 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWDeviceItemDataEvt(object sender, AddWDeviceItemDataEventArgs e)
        {
            if (this.WeatherListView.InvokeRequired)
            {
                this.Invoke(new InvokeSetWDeviceItemData(this.SetWDeviceItemDataM), new object[] { e.WDI });
            }
            else
            {
                this.SetWDeviceItemDataM(e.WDI);
            }
        }

        /// <summary>
        /// 측기 요청/제어를 한 후 이벤트(사용자가 발생시키는 이벤트)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dataMng_onAddWDeviceRequestEvt(object sender, AddWDeviceRequestEventArgs e)
        {
            WDevice wDevice = this.dataMng.GetWDevice(e.WDR.FkDevice);
            WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(wDevice.TypeDevice);
            WTypeDeviceItem typeDeviceItem = this.dataMng.GetTypeDeviceItem(e.WDR.FkDeviceItem);
            string real = (e.WDR.IsControl == 0) ? "요청" : "제어";

            if (this.WeatherDataTextBox.InvokeRequired)
            {
                this.Invoke(new InvokeSetWeatherDataLog(this.SetWeatherData), new object[] { string.Format("{0}의 {1} 측기의 {2} 항목을 {3} 하였습니다.",
                    wTypeTmp.Name, wDevice.ID, typeDeviceItem.Name, real ) });
            }
            else
            {
                this.SetWeatherData(string.Format("{0}의 {1} 측기의 {2} 항목을 {3} 하였습니다.",
                    wTypeTmp.Name, wDevice.ID, typeDeviceItem.Name, real));
            }
        }

        /// <summary>
        /// 임계치 알람 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddAlarmDataEvt(object sender, AddAlarmEventArgs e)
        {
            WDevice wDeviceTmp = this.dataMng.GetWDevice(e.WA.FKDevice);
            WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(wDeviceTmp.TypeDevice);
            string level = (e.WA.AlarmType.ToString() == "1") ? "우량계" :
                                (e.WA.AlarmType.ToString() == "2") ? "수위계" :
                                (e.WA.AlarmType.ToString() == "3") ? "유속계" :
                                (e.WA.AlarmType.ToString() == "4") ? "풍향풍속계" : "기타";
            level += (e.WA.AlarmLevel.ToString() == "0") ? ", 해제" :
                        (e.WA.AlarmLevel.ToString() == "1") ? ", 주의(1단계)" :
                        (e.WA.AlarmLevel.ToString() == "2") ? ", 경계(2단계)" :
                        (e.WA.AlarmLevel.ToString() == "3") ? ", 대피(3단계)" : ", 기타";

            if (this.WeatherDataTextBox.InvokeRequired)
            {
                this.Invoke(new InvokeSetWeatherDataLog(this.SetWeatherData), new object[] { string.Format("{0}의 {1} 측기의 {2} 임계치 알람 이벤트가 발생되었습니다.",
                    wTypeTmp.Name, wDeviceTmp.ID, level) });
            }
            else
            {
                this.SetWeatherData(string.Format("{0}의 {1} 측기의 {2} 임계치 알람 이벤트가 발생되었습니다.",
                    wTypeTmp.Name, wDeviceTmp.ID, level));
            }
        }

        /// <summary>
        /// 유속 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddFlowSpeedDataEvt(object sender, AddFlowSpeedEventArgs e)
        {
            WDevice wDeviceTmp = this.dataMng.GetWDevice(e.WFS.FKDevice);
            WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(wDeviceTmp.TypeDevice);

            if (this.WeatherDataTextBox.InvokeRequired)
            {
                this.Invoke(new InvokeSetWeatherDataLog(this.SetWeatherData), new object[] { string.Format("{0}의 {1} 측기의 유속 데이터가 수신되었습니다.", wTypeTmp.Name, wDeviceTmp.ID) });
            }
            else
            {
                this.SetWeatherData(string.Format("{0}의 {1} 측기의 유속 데이터가 수신되었습니다.", wTypeTmp.Name, wDeviceTmp.ID));
            }
        }

        /// <summary>
        /// 수위 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWaterLevelDataEvt(object sender, AddWaterLevelEventArgs e)
        {
            WDevice wDeviceTmp = this.dataMng.GetWDevice(e.WWL.FKDevice);
            WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(wDeviceTmp.TypeDevice);

            if (this.WeatherDataTextBox.InvokeRequired)
            {
                this.Invoke(new InvokeSetWeatherDataLog(this.SetWeatherData), new object[] { string.Format("{0}의 {1} 측기의 수위 데이터가 수신되었습니다.", wTypeTmp.Name, wDeviceTmp.ID) });
            }
            else
            {
                this.SetWeatherData(string.Format("{0}의 {1} 측기의 수위 데이터가 수신되었습니다.", wTypeTmp.Name, wDeviceTmp.ID));
            }
        }

        /// <summary>
        /// 강수 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddRainDataEvt(object sender, AddRainDataEventArgs e)
        {
            WDevice wDeviceTmp = this.dataMng.GetWDevice(e.WR.FKDevice);
            WTypeDevice wTypeTmp = this.dataMng.GetTypeDevice(wDeviceTmp.TypeDevice);

            if (this.WeatherDataTextBox.InvokeRequired)
            {
                this.Invoke(new InvokeSetWeatherDataLog(this.SetWeatherData), new object[] { string.Format("{0}의 {1} 측기의 강수 데이터가 수신되었습니다.", wTypeTmp.Name, wDeviceTmp.ID) });
            }
            else
            {
                this.SetWeatherData(string.Format("{0}의 {1} 측기의 강수 데이터가 수신되었습니다.", wTypeTmp.Name, wDeviceTmp.ID));
            }
        }

        /// <summary>
        /// 측기 삭제 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onDeleteWDeviceEvt(object sender, DeleteWDeviceEventArgs e)
        {
            for (int i = 0; i < e.WDList.Count; i++)
            {
                this.WeatherListView.Items.RemoveByKey(e.WDList[i].PKID.ToString());
                this.SetWeatherData(string.Format("{0}의 {1} 측기가 삭제되었습니다.", this.dataMng.GetTypeDevice(e.WDList[i].TypeDevice).Name, e.WDList[i].ID));
            }

            this.SetListViewIndex(this.WeatherListView);
        }

        /// <summary>
        /// 측기 수정 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onUpdateWDeviceEvt(object sender, UpdateWDeviceEventArgs e)
        {
            if (e.WD.EthernetUse)
            {
                EthernetClient tmpE = this.dataMng.getEthernetClient(e.WD.ID);

                if (tmpE.Client == null || tmpE.ID == string.Empty || !tmpE.Client.Connected)
                {
                    this.WeatherListView.Items[e.WD.PKID.ToString()].StateImageIndex = 0;
                }
                else
                {
                    this.WeatherListView.Items[e.WD.PKID.ToString()].StateImageIndex = 1;
                }
            }
            else
            {
                this.WeatherListView.Items[e.WD.PKID.ToString()].StateImageIndex = NotUseIcon;
            }

            this.WeatherListView.Items[e.WD.PKID.ToString()].SubItems[2].Text = this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name;
            this.WeatherListView.Items[e.WD.PKID.ToString()].SubItems[3].Text = e.WD.ID;
            this.WeatherListView.Items[e.WD.PKID.ToString()].SubItems[4].Text = e.WD.Name;
            this.WeatherListView.Items[e.WD.PKID.ToString()].SubItems[5].Text = e.WD.CellNumber;

            if (this.WeatherListView.SelectedItems.Count == 1)
            {
                if (this.WeatherListView.SelectedItems[0].Name == e.WD.PKID.ToString())
                {
                    this.WeatherListView.Items[e.WD.PKID.ToString()].Selected = false;
                    this.WeatherListView.Items[e.WD.PKID.ToString()].Focused = false;
                    this.WeatherListView.Items[e.WD.PKID.ToString()].Selected = true;
                    this.WeatherListView.Items[e.WD.PKID.ToString()].Focused = true;
                    this.WeatherListView.Items[e.WD.PKID.ToString()].EnsureVisible();
                }
            }

            this.SetWeatherData(string.Format("{0}의 {1} 측기가 수정되었습니다.", this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name, e.WD.ID));
        }

        /// <summary>
        /// 측기 등록 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWDeviceEvt(object sender, AddWDeviceEventArgs e)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.UseItemStyleForSubItems = false;

            if (e.WD.EthernetUse)
            {
                lvi.StateImageIndex = 0;
            }

            lvi.Name = string.Format("{0}", e.WD.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", this.WeatherListView.Items.Count + 1));
            lvi.SubItems.Add(this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name);
            lvi.SubItems.Add(e.WD.ID);
            lvi.SubItems.Add(e.WD.Name);
            lvi.SubItems.Add(e.WD.CellNumber);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);

            this.WeatherListView.Items.Add(lvi);
            this.SetWeatherData(string.Format("{0}의 {1} 측기가 등록되었습니다.", this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name, e.WD.ID));
        }

        /// <summary>
        /// 측기의 데이터 또는 상태 값 등을 받아 리스트에 셋팅한다.
        /// </summary>
        /// <param name="_wdi"></param>
        public void SetWDeviceItemDataM(WDeviceItem _wdi)
        {
            switch (_wdi.FKDeviceItem)
            {
                case (uint)WeatherDataMng.WIType.동일레벨무시시간:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[6].Text = string.Format("{0} 분", uint.Parse(_wdi.Value));
                    break;

                case (uint)WeatherDataMng.WIType.하향레벨무시시간:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[7].Text = string.Format("{0} 분", uint.Parse(_wdi.Value));
                    break;

                case (uint)WeatherDataMng.WIType.배터리전압:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[8].Text = string.Format("{0} (V)", double.Parse(_wdi.Value) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리전류:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[9].Text = string.Format("{0}/{1} (A)",
                        (_wdi.Value[0] == '+') ? "충전 중" : "방전 중", double.Parse(_wdi.Value.Substring(1, 4)) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리저항:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[10].Text = string.Format("{0} (mΩ)", double.Parse(_wdi.Value) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리온도:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[11].Text = string.Format("{0} {1} (℃)",
                        (_wdi.Value[0] == '+') ? "영상" : "영하", double.Parse(_wdi.Value.Substring(1, 4)) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리수명:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[12].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" :
                                                                                                    (_wdi.Value == "1") ? "점검 요망" : "교환");
                    if (_wdi.Value == "0")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[12].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1" || _wdi.Value == "2")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[12].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.배터리상태:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[13].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" : "이상");

                    if (_wdi.Value == "0")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[13].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[13].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.배터리2전압:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[14].Text = string.Format("{0} (V)", double.Parse(_wdi.Value) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리2전류:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[15].Text = string.Format("{0}/{1} (A)",
                        (_wdi.Value[0] == '+') ? "충전 중" : "방전 중", double.Parse(_wdi.Value.Substring(1, 4)) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리2저항:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[16].Text = string.Format("{0} (mΩ)", double.Parse(_wdi.Value) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리2온도:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[17].Text = string.Format("{0} {1} (℃)",
                        (_wdi.Value[0] == '+') ? "영상" : "영하", double.Parse(_wdi.Value.Substring(1, 4)) * 0.1);
                    break;

                case (uint)WeatherDataMng.WIType.배터리2수명:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[18].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" :
                                                                                                    (_wdi.Value == "1") ? "점검 요망" : "교환");
                    if (_wdi.Value == "0")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[18].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1" || _wdi.Value == "2")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[18].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.배터리2상태:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[19].Text = string.Format("{0}", (_wdi.Value == "0") ? "정상" : "이상");

                    if (_wdi.Value == "0")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[19].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[19].ForeColor = Color.OrangeRed;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.FAN:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[20].Text = string.Format("{0}", (_wdi.Value == "0") ? "이상" :
                                                                                                                    (_wdi.Value == "1") ? "정상(ON)" : "정상(OFF)");

                    if (_wdi.Value == "0")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[20].ForeColor = Color.OrangeRed;
                    }
                    else if (_wdi.Value == "1" || _wdi.Value == "2")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[20].ForeColor = Color.Black;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.DOOR:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[21].Text = string.Format("{0}", (_wdi.Value == "0") ? "닫힘" : "열림");

                    if (_wdi.Value == "0")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[21].ForeColor = Color.Black;
                    }
                    else if (_wdi.Value == "1")
                    {
                        this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[21].ForeColor = Color.Blue;
                    }
                    break;

                case (uint)WeatherDataMng.WIType.펌웨어버전:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[22].Text = string.Format("20{0},{1}", _wdi.Value.Substring(0, 6), _wdi.Value[6]);
                    break;

                case (uint)WeatherDataMng.WIType.CDMA감도:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[23].Text = string.Format("{0} (dbm)", _wdi.Value);
                    break;

                case (uint)WeatherDataMng.WIType.IP:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[24].Text = string.Format("{0}", _wdi.Value);
                    break;

                case (uint)WeatherDataMng.WIType.PORT:
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].SubItems[25].Text = string.Format("{0}", uint.Parse(_wdi.Value));
                    break;
            }

            if (this.WeatherListView.SelectedItems.Count == 1)
            {
                if (this.WeatherListView.SelectedItems[0].Name == _wdi.FKDevice.ToString())
                {
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].Selected = false;
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].Focused = false;
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].Selected = true;
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].Focused = true;
                    this.WeatherListView.Items[_wdi.FKDevice.ToString()].EnsureVisible();
                }
            }
        }

        /// <summary>
        /// 리스트뷰의 인덱스를 처음부터 다시 셋팅한다.
        /// </summary>
        /// <param name="lv"></param>
        public void SetListViewIndex(ListView lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                int c = i + 1;
                lv.Items[i].SubItems[1].Text = c.ToString();
            }
        }

        /// <summary>
        /// 데이터로그 텍스트박스에 로그를 남긴다.
        /// </summary>
        /// <param name="_str"></param>
        public void SetWeatherData(string _str)
        {
            if (this.WeatherDataTextBox.Text.Length > Int32.MaxValue)
            {
                this.WeatherDataTextBox.Text = string.Empty;
            }

            this.WeatherDataTextBox.Text += string.Format("\n{0} - {1}", _str, DateTime.Now);
            this.WeatherDataTextBox.SelectionStart = this.WeatherDataTextBox.Text.Length;
            this.WeatherDataTextBox.SelectionLength = 0;
            this.WeatherDataTextBox.ScrollToCaret();
        }

        //데이터로그 텍스트박스 초기화
        private void WeatherDataResetLB_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("측기 데이터 로그를 초기화 합니다.", "데이터 로그", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                this.WeatherDataTextBox.Text = string.Empty;
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
            dIcon.Width = 25;
            this.WeatherListView.Columns.Add(dIcon);

            ColumnHeader dPkid = new ColumnHeader();
            dPkid.Text = "번호";
            dPkid.Width = 43;
            dPkid.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dPkid);

            ColumnHeader dDivision = new ColumnHeader();
            dDivision.Text = "식별자";
            dDivision.Width = 60;
            dDivision.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dDivision);

            ColumnHeader dId = new ColumnHeader();
            dId.Text = "ID";
            dId.Width = 120;
            dId.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dId);

            ColumnHeader dName = new ColumnHeader();
            dName.Text = "이름";
            dName.Width = 140;
            dName.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dName);

            ColumnHeader dTelNum = new ColumnHeader();
            dTelNum.Text = "전화번호";
            dTelNum.Width = 90;
            dTelNum.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dTelNum);

            ColumnHeader dTooIgnore = new ColumnHeader();
            dTooIgnore.Text = "동일레벨 무시시간";
            dTooIgnore.Width = 115;
            dTooIgnore.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dTooIgnore);

            ColumnHeader dDownIgnore = new ColumnHeader();
            dDownIgnore.Text = "하향레벨 무시시간";
            dDownIgnore.Width = 115;
            dDownIgnore.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dDownIgnore);

            ColumnHeader dBattVolt = new ColumnHeader();
            dBattVolt.Text = "배터리1 전압";
            dBattVolt.Width = 85;
            dBattVolt.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBattVolt);

            ColumnHeader dBattA = new ColumnHeader();
            dBattA.Text = "배터리1 전류";
            dBattA.Width = 100;
            dBattA.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBattA);

            ColumnHeader dBattC = new ColumnHeader();
            dBattC.Text = "배터리1 저항";
            dBattC.Width = 85;
            dBattC.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBattC);

            ColumnHeader dBattT = new ColumnHeader();
            dBattT.Text = "배터리1 온도";
            dBattT.Width = 85;
            dBattT.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBattT);

            ColumnHeader dBattL = new ColumnHeader();
            dBattL.Text = "배터리1 수명";
            dBattL.Width = 85;
            dBattL.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBattL);

            ColumnHeader dBatt = new ColumnHeader();
            dBatt.Text = "배터리1 상태";
            dBatt.Width = 85;
            dBatt.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt);

            ColumnHeader dBatt2Volt = new ColumnHeader();
            dBatt2Volt.Text = "배터리2 전압";
            dBatt2Volt.Width = 85;
            dBatt2Volt.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt2Volt);

            ColumnHeader dBatt2A = new ColumnHeader();
            dBatt2A.Text = "배터리2 전류";
            dBatt2A.Width = 100;
            dBatt2A.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt2A);

            ColumnHeader dBatt2C = new ColumnHeader();
            dBatt2C.Text = "배터리2 저항";
            dBatt2C.Width = 85;
            dBatt2C.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt2C);

            ColumnHeader dBatt2T = new ColumnHeader();
            dBatt2T.Text = "배터리2 온도";
            dBatt2T.Width = 85;
            dBatt2T.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt2T);

            ColumnHeader dBatt2L = new ColumnHeader();
            dBatt2L.Text = "배터리2 수명";
            dBatt2L.Width = 85;
            dBatt2L.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt2L);

            ColumnHeader dBatt2 = new ColumnHeader();
            dBatt2.Text = "배터리2 상태";
            dBatt2.Width = 85;
            dBatt2.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dBatt2);

            ColumnHeader dFan = new ColumnHeader();
            dFan.Text = "FAN";
            dFan.Width = 70;
            dFan.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dFan);

            ColumnHeader dDoor = new ColumnHeader();
            dDoor.Text = "도어";
            dDoor.Width = 70;
            dDoor.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dDoor);

            ColumnHeader dFWVer = new ColumnHeader();
            dFWVer.Text = "F/W 버전";
            dFWVer.Width = 70;
            dFWVer.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dFWVer);

            ColumnHeader dCDMArssi = new ColumnHeader();
            dCDMArssi.Text = "CDMA 감도";
            dCDMArssi.Width = 80;
            dCDMArssi.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dCDMArssi);

            ColumnHeader dIPPort = new ColumnHeader();
            dIPPort.Text = "IP";
            dIPPort.Width = 100;
            dIPPort.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dIPPort);

            ColumnHeader dIPPort2 = new ColumnHeader();
            dIPPort2.Text = "PORT";
            dIPPort2.Width = 50;
            dIPPort2.TextAlign = HorizontalAlignment.Center;
            this.WeatherListView.Columns.Add(dIPPort2);
            #endregion
        }

        /// <summary>
        /// 측기 리스트 초기화
        /// </summary>
        private void DeviceInit()
        {
            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                this.WeatherListView.Items.Add(this.GetListViewItem(this.dataMng.DeviceList[i]));
            }
        }

        /// <summary>
        /// ListView의 StateImage를 셋팅한다.
        /// </summary>
        /// <param name="_pkid">
        /// 측기 PKID
        /// </param>
        /// <param name="_imageIndex">
        /// 0 - 이상
        /// 1 - 정상
        /// </param>
        public void setEthernetState(uint _pkid, int _imageIndex)
        {
            MethodInvoker setInvoker = delegate()
            {
                this.WeatherListView.Items[_pkid.ToString()].StateImageIndex = _imageIndex;
            };

            if (this.WeatherListView.InvokeRequired)
            {
                this.Invoke(setInvoker);
            }
            else
            {
                setInvoker();
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

            if (_wd.EthernetUse)
            {
                lvi.StateImageIndex = 0;
            }

            lvi.Name = string.Format("{0}", _wd.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", this.WeatherListView.Items.Count + 1));
            lvi.SubItems.Add(this.dataMng.GetTypeDevice(_wd.TypeDevice).Name);
            lvi.SubItems.Add(_wd.ID);
            lvi.SubItems.Add(_wd.Name);
            lvi.SubItems.Add(_wd.CellNumber);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);

            return lvi;
        }

        //리스트뷰 클릭 이벤트
        private void WeatherListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.WeatherListView.SelectedItems.Count == 1)
            {
                WDevice tmpWDevice = this.dataMng.GetWDevice(uint.Parse(this.WeatherListView.SelectedItems[0].Name));

                this.SameFTimeLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[6].Text;
                this.DownFTimeLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[7].Text;
                this.BattVoltLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[8].Text;
                this.BattALB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[9].Text;
                this.BattCLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[10].Text;
                this.BattTempoLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[11].Text;
                this.BattLifeLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[12].Text;
                this.BattStateLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[13].Text;
                this.BattVolt2LB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[14].Text;
                this.BattA2LB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[15].Text;
                this.BattC2LB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[16].Text;
                this.BattTempo2LB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[17].Text;
                this.BattLife2LB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[18].Text;
                this.BattState2LB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[19].Text;
                this.FanStateLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[20].Text;
                this.DoorStateLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[21].Text;
                this.FWVerStateLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[22].Text;
                this.CdmaRssiLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[23].Text;
                this.CdmaIpLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[24].Text;
                this.CdmaPortLB.Text = this.WeatherListView.Items[this.WeatherListView.SelectedItems[0].Name].SubItems[25].Text;
                this.RemarkLB.Text = tmpWDevice.Remark;

                if (this.BattLifeLB.Text == "정상")
                {
                    this.BattLifeLB.ForeColor = Color.Black;
                }
                else
                {
                    this.BattLifeLB.ForeColor = Color.OrangeRed;
                }

                if (this.BattStateLB.Text == "정상")
                {
                    this.BattStateLB.ForeColor = Color.Black;
                }
                else
                {
                    this.BattStateLB.ForeColor = Color.OrangeRed;
                }

                if (this.BattLife2LB.Text == "정상")
                {
                    this.BattLife2LB.ForeColor = Color.Black;
                }
                else
                {
                    this.BattLife2LB.ForeColor = Color.OrangeRed;
                }

                if (this.BattState2LB.Text == "정상")
                {
                    this.BattState2LB.ForeColor = Color.Black;
                }
                else
                {
                    this.BattState2LB.ForeColor = Color.OrangeRed;
                }

                if (this.FanStateLB.Text == "이상")
                {
                    this.FanStateLB.ForeColor = Color.OrangeRed;
                }
                else
                {
                    this.FanStateLB.ForeColor = Color.Black;
                }

                if (this.DoorStateLB.Text == "닫힘")
                {
                    this.DoorStateLB.ForeColor = Color.Black;
                }
                else
                {
                    this.DoorStateLB.ForeColor = Color.Blue;
                }
            }
            else
            {
                this.SameFTimeLB.Text = string.Empty;
                this.DownFTimeLB.Text = string.Empty;
                this.BattVoltLB.Text = string.Empty;
                this.BattALB.Text = string.Empty;
                this.BattCLB.Text = string.Empty;
                this.BattTempoLB.Text = string.Empty;
                this.BattLifeLB.Text = string.Empty;
                this.BattStateLB.Text = string.Empty;
                this.BattVolt2LB.Text = string.Empty;
                this.BattA2LB.Text = string.Empty;
                this.BattC2LB.Text = string.Empty;
                this.BattTempo2LB.Text = string.Empty;
                this.BattLife2LB.Text = string.Empty;
                this.BattState2LB.Text = string.Empty;
                this.FanStateLB.Text = string.Empty;
                this.DoorStateLB.Text = string.Empty;
                this.FWVerStateLB.Text = string.Empty;
                this.CdmaRssiLB.Text = string.Empty;
                this.CdmaIpLB.Text = string.Empty;
                this.CdmaPortLB.Text = string.Empty;
                this.RemarkLB.Text = string.Empty;
            }
        }

        //Remark 텍스트박스의 내용이 크기를 넘어섰을 때..
        private void RemarkLB_MouseHover(object sender, EventArgs e)
        {
            if (this.WeatherListView.SelectedItems.Count == 1)
            {
                WDevice tmpWDevice = this.dataMng.GetWDevice(uint.Parse(this.WeatherListView.SelectedItems[0].Name));

                if (tmpWDevice.Remark.Length > 14)
                {
                    this.RemarkToolTip.SetToolTip(this.RemarkLB, tmpWDevice.Remark);
                }
                else
                {
                    this.RemarkToolTip.RemoveAll();
                }
            }
        }

        //마우스 오른쪽 버튼 상태요청 클릭
        private void 상태요청ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.WeatherListView.SelectedItems.Count > 0)
                {
                    WaitBarMng.Start();
                    Thread.Sleep(2000);

                    for (int i = 0; i < this.WeatherListView.SelectedItems.Count; i++)
                    {
                        WDevice tmpDevice = this.dataMng.GetWDevice(uint.Parse(this.WeatherListView.SelectedItems[i].Name));

                        for (int j = 0; j < this.dataMng.TypeDeviceList.Count; j++)
                        {
                            if (tmpDevice.TypeDevice == this.dataMng.TypeDeviceList[j].PKID)
                            {
                                if (this.dataMng.TypeDeviceList[j].Name == "RAT") //RAT에 전체 상태 요청하는 경우
                                {
                                    //DB 저장
                                    WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.ALL;
                                    WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                    WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                                    this.dataMng.AddDeviceRequest(tmp);

                                    CProto00 cProto00 = CProtoMng.GetProtoObj("00") as CProto00;
                                    cProto00.Header = "[";
                                    cProto00.Length = "021";
                                    cProto00.ID = tmpDevice.ID;
                                    cProto00.MainCode = "0";
                                    cProto00.SubCode = "a";
                                    cProto00.RecvType = "1";
                                    cProto00.CRC = "00000";
                                    cProto00.Tail = "]";

                                    if (tmpDevice.EthernetUse)
                                    {
                                        cProto00.RecvType = "3";
                                        byte[] buff = cProto00.MakeProto();
                                        this.dataMng.SendEthernetMsg(tmpDevice.ID, buff);
                                    }
                                    else
                                    {
                                        byte[] buff = cProto00.MakeProto();
                                        this.dataMng.SendSmsMsg(tmpDevice.CellNumber, buff);
                                    }
                                }
                                else if (this.dataMng.TypeDeviceList[j].Name == "WOU") //WOU에 상태 요청하는 경우
                                {
                                    //DB 저장
                                    WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.ALL;
                                    WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                    WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                                    this.dataMng.AddDeviceRequest(tmp);
                                    this.dataMng.WOUWDeviceRequest(tmpDevice.PKID); //serial로 요청한다.
                                }
                            }
                        }
                    }

                    WaitBarMng.Close();
                }
                else
                {
                    MessageBox.Show("선택한 측기가 없습니다.", "상태 요청", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherForm.상태요청ToolStripMenuItem_Click() - {0}", ex.Message));
                WaitBarMng.Close();
            }
        }

        //마우스 오른쪽 버튼 제어 클릭
        private void 제어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<uint> WDeviceSelected = new List<uint>();

                for (int i = 0; i < this.WeatherListView.SelectedItems.Count; i++)
                {
                    WDeviceSelected.Add(uint.Parse(this.WeatherListView.SelectedItems[i].Name));
                }

                this.controlForm = new WeatherControlMng(WDeviceSelected);
                this.controlForm.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherForm.제어ToolStripMenuItem_Click() - {0}", ex.Message));
                WaitBarMng.Close();
            }
        }

        //폼을 Show했을 때 발생하는 이벤트
        private void WeatherForm_Activated(object sender, EventArgs e)
        {
            this.WeatherDataTextBox.SelectionStart = this.WeatherDataTextBox.Text.Length;
            this.WeatherDataTextBox.SelectionLength = 0;
            this.WeatherDataTextBox.ScrollToCaret();
        }
    }
}