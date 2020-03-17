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
    public partial class SMSMainForm : Form
    {
        private WeatherDataMng dataMng = null;
        private SMSUserMng smsUserMng = null;
        private bool smsSending = false;

        private delegate void InvokeSetSmsData(uint _pkid, string _name, string _num, string _remark);
        private delegate void InvokeSetDelUser(uint _pkid);
        private delegate void InvokeSendSmsData(string _cellNum, string _msg);

        /// <summary>
        /// 등록, 수정을 구분하는 enum's
        /// </summary>
        private enum MType
        {
            Add = 1,
            Update = 2
        }

        public SMSMainForm()
        {
            InitializeComponent();

            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onAddSmsUserEvt += new EventHandler<AddSmsUserEventArgs>(dataMng_onAddSmsUserEvt);
            this.dataMng.onUpdateSmsUserEvt += new EventHandler<UpdateSmsUserEventArgs>(dataMng_onUpdateSmsUserEvt);
            this.dataMng.onDelSmsUserEvt += new EventHandler<DelSmsUserEventArgs>(dataMng_onDelSmsUserEvt);
            this.dataMng.onUpdateWDeviceEvt += new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt += new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
            this.dataMng.onSendSmsUserMsgEvt += new EventHandler<SendSmsMsgEventArgs>(dataMng_onSendSmsUserMsgEvt);
            this.init();

            //SMS 사용자 리스트뷰에 셋팅
            for (int i = 0; i < this.dataMng.SmsUserList.Count; i++)
            {
                this.UserDetailLV.Items.Add(this.GetListviewItems(this.dataMng.SmsUserList[i].PKID,
                    this.dataMng.SmsUserList[i].Name, this.dataMng.SmsUserList[i].TelNum, this.dataMng.SmsUserList[i].Remark));
            }

            //SMS 사용자 트리뷰에 셋팅
            for (int i = 0; i < this.dataMng.SmsUserList.Count; i++)
            {
                this.SmsUserTV.Nodes.Add(this.dataMng.SmsUserList[i].PKID.ToString(), string.Format("{0}({1})", this.dataMng.SmsUserList[i].Name,
                    this.dataMng.SmsUserList[i].TelNum));
                this.SmsUserTV.Nodes[this.dataMng.SmsUserList[i].PKID.ToString()].ImageIndex = 0;
                this.SmsUserTV.Nodes[this.dataMng.SmsUserList[i].PKID.ToString()].SelectedImageIndex = 0;
                List<WDevice> tmpWDeviceList = this.dataMng.GetWDeviceForSmsUser(this.dataMng.SmsUserList[i].PKID);

                for (int j = 0; j < tmpWDeviceList.Count; j++)
                {
                    this.SmsUserTV.Nodes[this.dataMng.SmsUserList[i].PKID.ToString()].Nodes.Add(tmpWDeviceList[j].PKID.ToString(), string.Format("{0}({1})", tmpWDeviceList[j].Name,
                        this.dataMng.GetTypeDevice(tmpWDeviceList[j].TypeDevice).Name));
                    this.SmsUserTV.Nodes[this.dataMng.SmsUserList[i].PKID.ToString()].Nodes[tmpWDeviceList[j].PKID.ToString()].ImageIndex = 1;
                    this.SmsUserTV.Nodes[this.dataMng.SmsUserList[i].PKID.ToString()].Nodes[tmpWDeviceList[j].PKID.ToString()].SelectedImageIndex = 1;
                }
            }
        }

        //SMS 전송 이벤트
        private void dataMng_onSendSmsUserMsgEvt(object sender, SendSmsMsgEventArgs e)
        {
            if (this.SmsSendLV.InvokeRequired)
            {
                this.Invoke(new InvokeSendSmsData(this.SetSmsSendLV), new object[] { e.CellNum, Encoding.Default.GetString(e.Msg) });
            }
            else
            {
                this.SetSmsSendLV(e.CellNum, Encoding.Default.GetString(e.Msg));
            }
        }

        //측기 삭제 시 발생하는 이벤트
        private void dataMng_onDeleteWDeviceEvt(object sender, DeleteWDeviceEventArgs e)
        {
            try
            {
                for (int i = 0; i < e.WDList.Count; i++)
                {
                    for (int j = 0; j < this.SmsUserTV.Nodes.Count; j++)
                    {
                        for (int m = 0; m < this.SmsUserTV.Nodes[j].Nodes.Count; m++)
                        {
                            if (this.SmsUserTV.Nodes[j].Nodes[m].Name == e.WDList[i].PKID.ToString())
                            {
                                this.SmsUserTV.Nodes[j].Nodes[e.WDList[i].PKID.ToString()].Remove();
                            }
                        }
                    }

                    List<MapSmsUser> mapSmsList = this.dataMng.getMapSmsUserList(e.WDList[i].PKID);

                    for (int k = 0; k < mapSmsList.Count; k++)
                    {
                        this.dataMng.MapSmsList.Remove(mapSmsList[k]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("SMSMainForm.dataMng_onDeleteWDeviceEvt() - ", ex.Message));
            }
        }

        //측기 정보 수정 시 발생하는 이벤트
        private void dataMng_onUpdateWDeviceEvt(object sender, UpdateWDeviceEventArgs e)
        {
            for (int i = 0; i < this.SmsUserTV.Nodes.Count; i++)
            {
                for (int j = 0; j < this.SmsUserTV.Nodes[i].Nodes.Count; j++)
                {
                    if (this.SmsUserTV.Nodes[i].Nodes[j].Name == e.WD.PKID.ToString())
                    {
                        this.SmsUserTV.Nodes[i].Nodes[j].Text = string.Format("{0}({1})", e.WD.Name,
                            this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name);
                    }
                }
            }
        }

        //SMS 전송 현황 리스트뷰에 데이터를 셋팅한다.
        private void SetSmsSendLV(string _cellNum, string _msg)
        {
            this.SmsSendLV.Items.Add(this.GetSmsSendLVItem(_cellNum, _msg));
        }

        //ListViewItem을 만든 후 반환한다.
        private ListViewItem GetSmsSendLVItem(string _cell, string _msg)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Text = string.Format("");
            lvi.SubItems.Add(string.Format("{0}", this.SmsSendLV.Items.Count + 1));
            lvi.SubItems.Add(string.Format("{0}", DateTime.Now.ToString()));
            lvi.SubItems.Add(string.Format("{0}", _cell));

            if (_msg.Length > 4)
            {
                if (_msg.Substring(0, 4) == "[RAT")
                {
                    lvi.SubItems.Add(string.Format("측기 상태 요청/제어"));
                }
                else
                {
                    lvi.SubItems.Add(string.Format("{0}", _msg));
                }
            }
            else
            {
                lvi.SubItems.Add(string.Format("{0}", _msg));
            }

            return lvi;
        }

        //SMS 사용자를 삭제하면 발생하는 이벤트
        private void dataMng_onDelSmsUserEvt(object sender, DelSmsUserEventArgs e)
        {
            //리스트뷰에서 삭제
            if (this.UserDetailLV.InvokeRequired)
            {
                Invoke(new InvokeSetDelUser(this.SetDelUserDetailLV), new object[] { e.WSU.PKID });
            }
            else
            {
                this.SetDelUserDetailLV(e.WSU.PKID);
            }

            //트리뷰에서 삭제
            if (this.SmsUserTV.Nodes.ContainsKey(e.WSU.PKID.ToString()))
            {
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Remove();
            }
        }

        //SMS 사용자를 수정하면 발생하는 이벤트
        private void dataMng_onUpdateSmsUserEvt(object sender, UpdateSmsUserEventArgs e)
        {
            //리스트뷰에 셋팅
            if (this.UserDetailLV.InvokeRequired)
            {
                Invoke(new InvokeSetSmsData(this.SetUpdateUserDetailLV), new object[] { e.WSU.PKID, e.WSU.Name, e.WSU.TelNum, e.WSU.Remark });
            }
            else
            {
                this.SetUpdateUserDetailLV(e.WSU.PKID, e.WSU.Name, e.WSU.TelNum, e.WSU.Remark);
            }

            //트리뷰에 셋팅
            this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Text = string.Format("{0}({1})", e.WSU.Name, e.WSU.TelNum);
            List<TreeNode> treeNodeList = new List<TreeNode>();

            for (int i = 0; i < this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes.Count; i++)
            {
                treeNodeList.Add(this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes[i]);
            }

            for (int i = 0; i < treeNodeList.Count; i++)
            {
                this.SmsUserTV.Nodes.Remove(treeNodeList[i]);
            }

            List<WDevice> tmpWDeviceList = this.dataMng.GetWDeviceForSmsUser(e.WSU.PKID);

            for (int i = 0; i < tmpWDeviceList.Count; i++)
            {
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes.Add(tmpWDeviceList[i].PKID.ToString(), string.Format("{0}({1})", tmpWDeviceList[i].Name,
                    this.dataMng.GetTypeDevice(tmpWDeviceList[i].TypeDevice).Name));
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes[tmpWDeviceList[i].PKID.ToString()].ImageIndex = 1;
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes[tmpWDeviceList[i].PKID.ToString()].SelectedImageIndex = 1;
            }
        }

        //SMS 사용자를 등록하면 발생하는 이벤트
        private void dataMng_onAddSmsUserEvt(object sender, AddSmsUserEventArgs e)
        {
            //리스트뷰에 셋팅
            if (this.UserDetailLV.InvokeRequired)
            {
                Invoke(new InvokeSetSmsData(this.SetUserDetailLV), new object[] { e.WSU.PKID, e.WSU.Name, e.WSU.TelNum, e.WSU.Remark });
            }
            else
            {
                this.SetUserDetailLV(e.WSU.PKID, e.WSU.Name, e.WSU.TelNum, e.WSU.Remark);
            }

            //트리뷰에 셋팅
            this.SmsUserTV.Nodes.Add(e.WSU.PKID.ToString(), string.Format("{0}({1})", e.WSU.Name, e.WSU.TelNum));
            this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].ImageIndex = 0;
            this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].SelectedImageIndex = 0;
            List<WDevice> tmpWDeviceList = this.dataMng.GetWDeviceForSmsUser(e.WSU.PKID);

            for (int i = 0; i < tmpWDeviceList.Count; i++)
            {
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes.Add(tmpWDeviceList[i].PKID.ToString(), string.Format("{0}({1})", tmpWDeviceList[i].Name,
                    this.dataMng.GetTypeDevice(tmpWDeviceList[i].TypeDevice).Name));
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes[tmpWDeviceList[i].PKID.ToString()].ImageIndex = 1;
                this.SmsUserTV.Nodes[e.WSU.PKID.ToString()].Nodes[tmpWDeviceList[i].PKID.ToString()].SelectedImageIndex = 1;
            }
        }

        /// <summary>
        /// 리스트뷰 초기화
        /// </summary>
        private void init()
        {
            #region SMS 사용자 상세정보 리스트뷰 초기화
            ColumnHeader smsDIcon = new ColumnHeader();
            smsDIcon.Text = string.Empty;
            smsDIcon.Width = 0;
            this.UserDetailLV.Columns.Add(smsDIcon);

            ColumnHeader smsDPkid = new ColumnHeader();
            smsDPkid.Text = "번호";
            smsDPkid.Width = 43;
            this.UserDetailLV.Columns.Add(smsDPkid);

            ColumnHeader smsDName = new ColumnHeader();
            smsDName.Text = "이름";
            smsDName.Width = 160;
            smsDName.TextAlign = HorizontalAlignment.Left;
            this.UserDetailLV.Columns.Add(smsDName);

            ColumnHeader smsDTelNum = new ColumnHeader();
            smsDTelNum.Text = "전화번호";
            smsDTelNum.Width = 110;
            smsDTelNum.TextAlign = HorizontalAlignment.Left;
            this.UserDetailLV.Columns.Add(smsDTelNum);

            ColumnHeader smsDRemark = new ColumnHeader();
            smsDRemark.Text = "Remark";
            smsDRemark.Width = 370;
            smsDRemark.TextAlign = HorizontalAlignment.Left;
            this.UserDetailLV.Columns.Add(smsDRemark);
            #endregion

            #region SMS 전송 현황 리스트뷰 초기화
            ColumnHeader smsIcon = new ColumnHeader();
            smsIcon.Text = string.Empty;
            smsIcon.Width = 0;
            this.SmsSendLV.Columns.Add(smsIcon);

            ColumnHeader smsPkid = new ColumnHeader();
            smsPkid.Text = "번호";
            smsPkid.Width = 43;
            this.SmsSendLV.Columns.Add(smsPkid);

            ColumnHeader smsDate = new ColumnHeader();
            smsDate.Text = "전송 시간";
            smsDate.Width = 160;
            smsDate.TextAlign = HorizontalAlignment.Left;
            this.SmsSendLV.Columns.Add(smsDate);

            ColumnHeader smsNum = new ColumnHeader();
            smsNum.Text = "전화 번호";
            smsNum.Width = 110;
            smsNum.TextAlign = HorizontalAlignment.Left;
            this.SmsSendLV.Columns.Add(smsNum);

            ColumnHeader smsMsg = new ColumnHeader();
            smsMsg.Text = "내용";
            smsMsg.Width = 370;
            smsMsg.TextAlign = HorizontalAlignment.Left;
            this.SmsSendLV.Columns.Add(smsMsg);
            #endregion
        }

        /// <summary>
        /// SMS 사용자 상세정보 리스트뷰에서 사용자를 삭제한다.
        /// </summary>
        /// <param name="_pkid"></param>
        private void SetDelUserDetailLV(uint _pkid)
        {
            this.UserDetailLV.Items.RemoveByKey(_pkid.ToString());
            this.SetListViewIndex(this.UserDetailLV);
        }

        /// <summary>
        /// SMS 사용자 상세정보 리스트뷰에 업데이트를 한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <param name="_name"></param>
        /// <param name="_num"></param>
        /// <param name="_remark"></param>
        private void SetUpdateUserDetailLV(uint _pkid, string _name, string _num, string _remark)
        {
            this.UserDetailLV.Items[_pkid.ToString()].SubItems[2].Text = _name;
            this.UserDetailLV.Items[_pkid.ToString()].SubItems[3].Text = _num;
            this.UserDetailLV.Items[_pkid.ToString()].SubItems[4].Text = _remark;
        }

        /// <summary>
        /// SMS 사용자 상세정보 리스트뷰에 데이터를 넣는다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <param name="_name"></param>
        /// <param name="_num"></param>
        /// <param name="_remark"></param>
        private void SetUserDetailLV(uint _pkid, string _name, string _num, string _remark)
        {
            this.UserDetailLV.Items.Add(this.GetListviewItems(_pkid, _name, _num, _remark));
            this.SetListViewIndex(this.UserDetailLV);
        }

        /// <summary>
        /// ListviewItem을 만들어 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <param name="_name"></param>
        /// <param name="_num"></param>
        /// <param name="_remark"></param>
        /// <returns></returns>
        private ListViewItem GetListviewItems(uint _pkid, string _name, string _num, string _remark)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Name = _pkid.ToString();
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", this.UserDetailLV.Items.Count + 1));
            lvi.SubItems.Add(_name);
            lvi.SubItems.Add(_num);
            lvi.SubItems.Add(_remark);

            return lvi;
        }

        //등록 버튼 클릭
        private void UserAddBtn_Click(object sender, EventArgs e)
        {
            using (this.smsUserMng = new SMSUserMng((byte)MType.Add, 0))
            {
                this.smsUserMng.ShowDialog();
            }
        }

        //수정 버튼 클릭
        private void UserUpdateBtn_Click(object sender, EventArgs e)
        {
            if (this.UserDetailLV.SelectedItems.Count == 1)
            {
                using (this.smsUserMng = new SMSUserMng((byte)MType.Update, uint.Parse(this.UserDetailLV.SelectedItems[0].Name)))
                {
                    this.smsUserMng.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("수정할 한 명의 사용자를 선택하세요.", "SMS 사용자 관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //삭제 버튼 클릭
        private void UserDelBtn_Click(object sender, EventArgs e)
        {
            if (this.UserDetailLV.SelectedItems.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show("사용자를 삭제하겠습니까?", "SMS 사용자 관리", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    List<WSmsUser> wsuList = new List<WSmsUser>();

                    for (int i = 0; i < this.UserDetailLV.SelectedItems.Count; i++)
                    {
                        WSmsUser wsu = new WSmsUser(uint.Parse(this.UserDetailLV.SelectedItems[i].Name), string.Empty, string.Empty, string.Empty);
                        wsuList.Add(wsu);
                    }

                    for (int i = 0; i < wsuList.Count; i++)
                    {
                        this.dataMng.DelSmsUser(wsuList[i]);
                    }
                }
            }
            else
            {
                MessageBox.Show("삭제할 사용자를 선택하세요.", "SMS 사용자 관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //전송 버튼 클릭
        private void SmsTestSendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataMng.CDMA)
                {
                    if (!this.SmsSendTestValidation())
                    {
                        MessageBox.Show("전화번호를 정확히 입력해 주세요.", "SMS 전송 시험", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (!this.smsSending)
                    {
                        this.smsSending = true;
                        WaitBarMng.Start();
                        Thread.Sleep(1500);

                        if (this.dataMng.SmsUserComparer(this.SmsSendNumTB.Text.Replace("-", ""))) //등록된 사람이 있으면..(DB 저장함)
                        {
                            List<WSmsUser> tmpSmsUserList = this.dataMng.getSmsUser(this.SmsSendNumTB.Text.Replace("-", ""));

                            for (int i = 0; i < tmpSmsUserList.Count; i++)
                            {
                                WSmsSend tmpSmsSend = new WSmsSend(0, tmpSmsUserList[i].PKID, DateTime.Now, this.SmsTestTB.Text);
                                this.dataMng.AddSmsSend(tmpSmsSend);
                            }
                        }
                        else //중복되는 사람이 없으면..(DB 저장 안함)
                        {
                        }

                        this.dataMng.SendSmsUserMsg(this.SmsSendNumTB.Text.Replace("-", ""), Encoding.Default.GetBytes(this.SmsTestTB.Text));
                    }
                }
                else
                {
                    MessageBox.Show("CDMA 장치가 준비되지 않았습니다.", "SMS 전송 시험", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                WaitBarMng.Close();
                this.smsSending = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("SMSMainForm.SmsTestSendBtn_Click() - {0}", ex.Message));
            }
        }

        //SMS 전송 시험의 유효성 검사
        private bool SmsSendTestValidation()
        {
            if (this.SmsSendNumTB.Text == string.Empty)
            {
                return false;
            }

            if (this.SmsSendNumTB.Text.Length < 10)
            {
                return false;
            }

            return true;
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

        //문자 80 byte 체크
        private void SmsTestTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Encoding.Default.GetBytes(this.SmsTestTB.Text).Length > 79 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        //문자 80 byte 체크
        private void SmsTestTB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Encoding.Default.GetBytes(this.SmsTestTB.Text).Length > 80)
                {
                    string tmpStr = this.SmsTestTB.Text;
                    this.SmsTestTB.Clear();

                    byte[] tmpByte = Encoding.Default.GetBytes(tmpStr);
                    this.SmsTestTB.AppendText(Encoding.Default.GetString(tmpByte, 0, 80));
                }

                this.SmsSendLengLB.Text = string.Format("{0} / 80", (Encoding.Default.GetByteCount(this.SmsTestTB.Text)).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("SMSMainForm.SmsTestTB_TextChanged() - {0}", ex.Message));
            }
        }

        //전화번호 텍스트박스에 숫자만 입력 가능하게 하는 이벤트
        private void SmsSendNumTB_KeyDown(object sender, KeyEventArgs e)
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
    }
}