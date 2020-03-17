using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ADEng.Library.WeatherSystem;

namespace ADEng.Module.WeatherSystem
{
    public partial class SMSUserMng : Form
    {
        private WeatherDataMng dataMng = null;
        private byte code = byte.MinValue;
        private uint pkid = uint.MinValue;
        private WSmsRecvKind recvSmsKind = new WSmsRecvKind();

        /// <summary>
        /// 등록, 수정을 구분하기 위한 enum's
        /// </summary>
        private enum DType
        {
            Add = 1,
            Update = 2
        }

        public SMSUserMng()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_code">
        /// 1 - 등록, 2 - 수정
        /// </param>
        /// <param name="_pkid">
        /// 수정일 경우 선택한 사용자
        /// </param>
        public SMSUserMng(byte _code, uint _pkid)
        {
            InitializeComponent();
            
            this.dataMng = WeatherDataMng.getInstance();
            this.code = _code;
            this.pkid = _pkid;

            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                WTypeDevice typeDevice = this.dataMng.GetTypeDevice(this.dataMng.DeviceList[i].TypeDevice);

                if (typeDevice.Name != "HSD" && typeDevice.Name != "DSD")
                {
                    this.WDeviceTV.Nodes.Add(this.dataMng.DeviceList[i].PKID.ToString(), string.Format("{0}({1})", this.dataMng.DeviceList[i].Name, typeDevice.Name));
                }
            }

            if (_code == 1) //등록
            {
                this.Text = string.Format("SMS 사용자 등록");
                this.SmsUserMngPB.BackgroundImage = Resources.sms사용자등록_41;
                this.SmsUserMngLB.Text = string.Format("SMS 사용자를 등록합니다.");
            }
            else if (_code == 2) //수정
            {
                this.Text = string.Format("SMS 사용자 수정");
                this.SmsUserMngPB.BackgroundImage = Resources.sms사용자수정_22;
                this.SmsUserMngLB.Text = string.Format("SMS 사용자를 수정합니다.");
                WSmsUser wsu = this.dataMng.getSmsUser(_pkid);
                this.SmsUserNameTB.Text = wsu.Name;
                this.SmsUserTelNumTB.Text = wsu.TelNum;
                this.SmsUserRemarkTB.Text = wsu.Remark;

                List<MapSmsUser> tmpMsuList = new List<MapSmsUser>();

                for (int i = 0; i < this.dataMng.MapSmsList.Count; i++)
                {
                    if (this.dataMng.MapSmsList[i].FkSmsUser == _pkid)
                    {
                        tmpMsuList.Add(this.dataMng.MapSmsList[i]);
                    }
                }

                for (int i = 0; i < tmpMsuList.Count; i++)
                {
                    if (this.WDeviceTV.Nodes.ContainsKey(tmpMsuList[i].FkDevice.ToString()))
                    {
                        this.WDeviceTV.Nodes[tmpMsuList[i].FkDevice.ToString()].Checked = true;
                    }
                }

                this.SaveBtn.Enabled = false;
            }
        }

        //측기 트리에서 측기를 선택/해제할 때 발생하는 이벤트
        private void WDeviceTV_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                if (!this.SelectWDeviceLV.Items.ContainsKey(e.Node.Name))
                {
                    this.SelectWDeviceLV.Items.Add(this.getListViewItem(e.Node.Name, e.Node.Text));
                }
            }
            else
            {
                if (this.SelectWDeviceLV.Items.ContainsKey(e.Node.Name))
                {
                    this.SelectWDeviceLV.Items.RemoveByKey(e.Node.Name);
                }
            }

            if (this.DataValidate())
            {
                this.SaveBtn.Enabled = true;
            }
            else
            {
                this.SaveBtn.Enabled = false;
            }
        }

        /// <summary>
        /// ListviewItem을 만들어 반환한다.
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_text"></param>
        /// <returns></returns>
        private ListViewItem getListViewItem(string _key, string _text)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Name = _key;
            lvi.Text = _text;

            return lvi;
        }

        //확인 버튼 클릭
        private void OkBtn_Click(object sender, EventArgs e)
        {
            if(!this.DataValidate())
            {
                MessageBox.Show("사용자 정보를 확인하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.SmsUserTelNumTB.Text.Length < 10)
            {
                MessageBox.Show("전화번호를 정확히 입력해 주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.code == (byte)DType.Add)
            {
                if (this.dataMng.SmsUserComparer(SmsUserTelNumTB.Text))
                {
                    MessageBox.Show("이미 등록된 전화번호입니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.save();
                this.Close();
            }
            else if (this.code == (byte)DType.Update)
            {
                this.Update();
                this.Close();
            }
        }

        //취소 버튼 클릭
        private void CancleBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //적용 버튼 클릭
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (!this.DataValidate())
            {
                MessageBox.Show("사용자 정보를 확인하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.SmsUserTelNumTB.Text.Length < 10)
            {
                MessageBox.Show("전화번호를 정확히 입력해 주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.code == (byte)DType.Add)
            {
                if (this.dataMng.SmsUserComparer(SmsUserTelNumTB.Text))
                {
                    MessageBox.Show("이미 등록된 전화번호입니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.save();
            }
            else if (this.code == (byte)DType.Update)
            {
                this.Update();
            }

            this.SaveBtn.Enabled = false;
        }

        //저장 메소드
        private void save()
        {
            WSmsUser wsu = new WSmsUser(0, SmsUserNameTB.Text, SmsUserTelNumTB.Text.Replace("-", ""), SmsUserRemarkTB.Text);
            List<string> strList = new List<string>();

            for (int i = 0; i < this.SelectWDeviceLV.Items.Count; i++)
            {
                strList.Add(this.SelectWDeviceLV.Items[i].Name);
            }

            if (strList.Count > 0)
            {
                //SMS 사용자 등록
                this.dataMng.AddSmsUser(wsu, strList);
                
                //사용자 별 수신받을 항목 저장
                List<WSmsUser> tmpSmsUserList = this.dataMng.getSmsUser(SmsUserTelNumTB.Text);
                WSmsUser tmpSmsUser = new WSmsUser();

                for (int i = 0; i < tmpSmsUserList.Count; i++)
                {
                    if (tmpSmsUserList[i].Name == SmsUserNameTB.Text)
                    {
                        tmpSmsUser = tmpSmsUserList[i];
                        break;
                    }
                }

                #region SMS 수신 항목 DB 저장
                MapSmsDeviceItem mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.임계치1단계, tmpSmsUser.PKID, this.recvSmsKind.Alarm1);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.임계치2단계, tmpSmsUser.PKID, this.recvSmsKind.Alarm2);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.임계치3단계, tmpSmsUser.PKID, this.recvSmsKind.Alarm3);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리1전압이상, tmpSmsUser.PKID, this.recvSmsKind.Batt1Volt);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리1온도이상, tmpSmsUser.PKID, this.recvSmsKind.Batt1Tempo);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리1점검시기, tmpSmsUser.PKID, this.recvSmsKind.Batt1Test);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리1교체시기, tmpSmsUser.PKID, this.recvSmsKind.Batt1Repair);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리1교체초기화, tmpSmsUser.PKID, this.recvSmsKind.Batt1Reset);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리2전압이상, tmpSmsUser.PKID, this.recvSmsKind.Batt2Volt);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리2온도이상, tmpSmsUser.PKID, this.recvSmsKind.Batt2Tempo);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리2점검시기, tmpSmsUser.PKID, this.recvSmsKind.Batt2Test);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리2교체시기, tmpSmsUser.PKID, this.recvSmsKind.Batt2Repair);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.배터리2교체초기화, tmpSmsUser.PKID, this.recvSmsKind.Batt2Reset);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.FAN이상, tmpSmsUser.PKID, this.recvSmsKind.FanState);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.CDMA시간설정이상, tmpSmsUser.PKID, this.recvSmsKind.CDMATime);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);

                mapSmsDeviceItems = new MapSmsDeviceItem(0, (uint)WeatherDataMng.SMSType.센서상태, tmpSmsUser.PKID, this.recvSmsKind.SensorState);
                this.dataMng.AddMapSMSDeviceItem(mapSmsDeviceItems);
                #endregion
            }
        }

        //수정 메소드
        private void Update()
        {
            WSmsUser wsu = new WSmsUser(this.dataMng.getSmsUser(this.pkid).PKID, SmsUserNameTB.Text, SmsUserTelNumTB.Text.Replace("-", ""), SmsUserRemarkTB.Text);
            List<string> strList = new List<string>();

            for (int i = 0; i < this.SelectWDeviceLV.Items.Count; i++)
            {
                strList.Add(this.SelectWDeviceLV.Items[i].Name);
            }

            if (strList.Count > 0)
            {
                //SMS 사용자 정보 업데이트
                this.dataMng.UpdateSmsUser(wsu, strList);

                //SMS 수신 항목 DB 업데이트
                #region SMS 수신 항목 DB 업데이트
                MapSmsDeviceItem tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.임계치1단계);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Alarm1;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.임계치2단계);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Alarm2;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.임계치3단계);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Alarm3;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리1전압이상);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt1Volt;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리1온도이상);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt1Tempo;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리1점검시기);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt1Test;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리1교체시기);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt1Repair;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리1교체초기화);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt1Reset;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리2전압이상);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt2Volt;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리2온도이상);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt2Tempo;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리2점검시기);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt2Test;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리2교체시기);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt2Repair;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.배터리2교체초기화);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.Batt2Reset;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.FAN이상);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.FanState;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.CDMA시간설정이상);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.CDMATime;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);

                tmpMapSmsDeviceItem = this.dataMng.getMapSmsItem(wsu.PKID, WeatherDataMng.SMSType.센서상태);
                tmpMapSmsDeviceItem.IsUse = this.recvSmsKind.SensorState;
                this.dataMng.UpdateMapSMSDeviceItem(tmpMapSmsDeviceItem);
                #endregion
            }
        }

        //트리뷰 아이템 더블클릭
        private void WDeviceTV_DoubleClick(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("SMSUserMng.WDeviceTV_DoubleClick() - {0}", ex.Message));
            }
        }

        //전화번호 텍스트박스에 숫자만 입력 가능하게 하는 이벤트
        private void SmsUserTelNumTB_KeyDown(object sender, KeyEventArgs e)
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
            // 109 - '-'
            // 189 - '-'
            // 35 - End
            // 36 - Home
            if (e.KeyValue == 8 || e.KeyValue == 46 || e.KeyValue == 37 || e.KeyValue == 38
                || e.KeyValue == 39 || e.KeyValue == 40 || e.KeyValue == 16
                || (e.KeyValue > 47 && e.KeyValue < 58)
                || (e.KeyValue > 95 && e.KeyValue < 106)
                || e.KeyValue == 109 || e.KeyValue == 189
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
        /// 데이터 유효성을 검사한다.
        /// </summary>
        /// <returns></returns>
        private bool DataValidate()
        {
            if (this.SmsUserNameTB.Text == string.Empty)
            {
                return false;
            }

            if (this.SmsUserTelNumTB.Text == string.Empty)
            {
                return false;
            }

            if (this.SelectWDeviceLV.Items.Count < 1)
            {
                return false;
            }

            return true;
        }

        //유효성을 검사해 적용 버튼을 활성/비활성화 한다.
        private void SmsUserNameTB_TextChanged(object sender, EventArgs e)
        {
            if (this.DataValidate())
            {
                this.SaveBtn.Enabled = true;
            }
            else
            {
                this.SaveBtn.Enabled = false;
            }
        }

        //수신항목 사용자지정 버튼 클릭
        private void RecvKindBtn_Click(object sender, EventArgs e)
        {
            if (this.code == 1) //등록
            {
                using (SMSRecvKindDlg recvDlg = new SMSRecvKindDlg())
                {
                    recvDlg.OnSmsRecvKind += new EventHandler<RecvSMSKindEventArgs>(recvDlg_OnSmsRecvKind);
                    recvDlg.ShowDialog();
                    recvDlg.OnSmsRecvKind -= new EventHandler<RecvSMSKindEventArgs>(recvDlg_OnSmsRecvKind);
                }
            }
            else if (this.code == 2) //수정
            {
                WSmsRecvKind wSmsRecvKindClass = new WSmsRecvKind();

                for (int i = 0; i < this.dataMng.MapSmsItemList.Count; i++)
                {
                    if (this.dataMng.MapSmsItemList[i].FkSmsUser == this.pkid)
                    {
                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.임계치1단계)
                        {
                            wSmsRecvKindClass.Alarm1 = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.임계치2단계)
                        {
                            wSmsRecvKindClass.Alarm2 = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.임계치3단계)
                        {
                            wSmsRecvKindClass.Alarm3 = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리1전압이상)
                        {
                            wSmsRecvKindClass.Batt1Volt = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리1온도이상)
                        {
                            wSmsRecvKindClass.Batt1Tempo = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리1점검시기)
                        {
                            wSmsRecvKindClass.Batt1Test = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리1교체시기)
                        {
                            wSmsRecvKindClass.Batt1Repair = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리1교체초기화)
                        {
                            wSmsRecvKindClass.Batt1Reset = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리2전압이상)
                        {
                            wSmsRecvKindClass.Batt2Volt = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리2온도이상)
                        {
                            wSmsRecvKindClass.Batt2Tempo = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리2점검시기)
                        {
                            wSmsRecvKindClass.Batt2Test = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리2교체시기)
                        {
                            wSmsRecvKindClass.Batt2Repair = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.배터리2교체초기화)
                        {
                            wSmsRecvKindClass.Batt2Reset = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.센서상태)
                        {
                            wSmsRecvKindClass.SensorState = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.FAN이상)
                        {
                            wSmsRecvKindClass.FanState = this.dataMng.MapSmsItemList[i].IsUse;
                        }

                        if (this.dataMng.MapSmsItemList[i].FkDevice == (uint)WeatherDataMng.SMSType.CDMA시간설정이상)
                        {
                            wSmsRecvKindClass.CDMATime = this.dataMng.MapSmsItemList[i].IsUse;
                        }
                    }
                }

                using (SMSRecvKindDlg recvDlg = new SMSRecvKindDlg(wSmsRecvKindClass))
                {
                    recvDlg.OnSmsRecvKind += new EventHandler<RecvSMSKindEventArgs>(recvDlg_OnSmsRecvKind);
                    recvDlg.ShowDialog();
                    recvDlg.OnSmsRecvKind -= new EventHandler<RecvSMSKindEventArgs>(recvDlg_OnSmsRecvKind);
                }
            }
        }

        /// <summary>
        /// SMS 수신 항목 설정 데이터를 받는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recvDlg_OnSmsRecvKind(object sender, RecvSMSKindEventArgs e)
        {
            this.recvSmsKind.Alarm1 = e.SmsKindClass.Alarm1;
            this.recvSmsKind.Alarm2 = e.SmsKindClass.Alarm2;
            this.recvSmsKind.Alarm3 = e.SmsKindClass.Alarm3;
            this.recvSmsKind.Batt1Repair = e.SmsKindClass.Batt1Repair;
            this.recvSmsKind.Batt1Reset = e.SmsKindClass.Batt1Reset;
            this.recvSmsKind.Batt1Tempo = e.SmsKindClass.Batt1Tempo;
            this.recvSmsKind.Batt1Test = e.SmsKindClass.Batt1Test;
            this.recvSmsKind.Batt1Volt = e.SmsKindClass.Batt1Volt;
            this.recvSmsKind.Batt2Repair = e.SmsKindClass.Batt2Repair;
            this.recvSmsKind.Batt2Reset = e.SmsKindClass.Batt2Reset;
            this.recvSmsKind.Batt2Tempo = e.SmsKindClass.Batt2Tempo;
            this.recvSmsKind.Batt2Test = e.SmsKindClass.Batt2Test;
            this.recvSmsKind.Batt2Volt = e.SmsKindClass.Batt2Volt;
            this.recvSmsKind.CDMATime = e.SmsKindClass.CDMATime;
            this.recvSmsKind.FanState = e.SmsKindClass.FanState;
            this.recvSmsKind.SensorState = e.SmsKindClass.SensorState;

            this.SaveBtn.Enabled = true;
        }
    }
}