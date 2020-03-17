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
    public partial class AddWDevice : Form
    {
        private WeatherDataMng dataMng = null;
        private byte mode = byte.MinValue;
        private string wDivision = string.Empty;
        private string wDevicePkid = string.Empty;
        private byte wDSensor = byte.MinValue;
        private bool wDEthernetUse = false;
        private string WDRemark = string.Empty;

        /// <summary>
        /// 추가/수정의 enum's
        /// </summary>
        private enum FType
        {
            add = 0,
            update = 1
        }

        #region 접근
        /// <summary>
        /// 추가/수정을 구분하기 위한 멤버
        /// 0 : 추가, 1 : 수정
        /// </summary>
        public byte Mode
        {
            get { return this.mode; }
            set { this.mode = value; }
        }

        /// <summary>
        /// 측기의 식별자 멤버
        /// </summary>
        public string WDivision
        {
            get { return this.wDivision; }
            set { this.wDivision = value; }
        }

        /// <summary>
        /// 측기의 PKID
        /// </summary>
        public string WDPKID
        {
            get { return this.wDevicePkid; }
            set { this.wDevicePkid = value; }
        }

        /// <summary>
        /// 측기 ID
        /// </summary>
        public string WDIDTB
        {
            get { return this.WDeviceIDTB.Text; }
            set { this.WDeviceIDTB.Text = value; }
        }

        /// <summary>
        /// 측기 이름
        /// </summary>
        public string WDNameTB
        {
            get { return this.WDeviceNameTB.Text; }
            set { this.WDeviceNameTB.Text = value; }
        }

        /// <summary>
        /// 측기 전화번호
        /// </summary>
        public string WDTelTB
        {
            get { return this.WDeviceTelNumTB.Text; }
            set { this.WDeviceTelNumTB.Text = value; }
        }

        /// <summary>
        /// 가지고 있는 센서에 대한 값
        /// </summary>
        public byte WDSensor
        {
            get { return this.wDSensor; }
            set { this.wDSensor = value; }
        }

        /// <summary>
        /// 이더넷 사용여부
        /// </summary>
        public bool WDEthernetUse
        {
            get { return this.wDEthernetUse; }
            set { this.wDEthernetUse = value; }
        }

        /// <summary>
        /// 측기의 부가 설명(위치 등..)
        /// </summary>
        public string WDeviceRemark
        {
            get { return this.WDRemark; }
            set { this.WDRemark = value; }
        }
        #endregion

        public AddWDevice()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dataMng = WeatherDataMng.getInstance();

            for (int i = 0; i < this.dataMng.TypeDeviceList.Count; i++)
            {
                this.WDeviceDivisionCB.Items.Add(string.Format("{0}({1})",
                    this.dataMng.TypeDeviceList[i].Name, this.dataMng.TypeDeviceList[i].Remark));
            }

            for (int i = 0; i < this.dataMng.TypeSensorList.Count; i++)
            {
                this.SensorKindLV.Items.Add(this.dataMng.TypeSensorList[i].Name);
            }

            if (this.mode == (byte)FType.add)
            {
                this.Text = "측기 등록";
                this.WDeviceMainLB.Text = "측기를 등록합니다.";
                this.WDeviceMainPB.BackgroundImage = Resources.우량기정보관리_등록_41_40;
            }
            else if (this.mode == (byte)FType.update)
            {
                this.Text = "측기 수정";
                this.WDeviceMainLB.Text = "측기를 수정합니다.";
                this.WDeviceMainPB.BackgroundImage = Resources.우량기정보관리_수정_41_40;
                this.WDeviceDivisionCB.Enabled = false;
                this.WDeviceIDTB.Enabled = false;

                if ((this.wDSensor & 0x01) == 0x01) //강수
                {
                    for (int i = 0; i < this.SensorKindLV.Items.Count; i++)
                    {
                        if (this.SensorKindLV.Items[i].Text == "우량계")
                        {
                            this.SensorKindLV.Items[i].Checked = true;
                        }
                    }
                }

                if ((this.wDSensor & 0x02) == 0x02) //수위
                {
                    for (int i = 0; i < this.SensorKindLV.Items.Count; i++)
                    {
                        if (this.SensorKindLV.Items[i].Text == "수위계")
                        {
                            this.SensorKindLV.Items[i].Checked = true;
                        }
                    }
                }

                if ((this.wDSensor & 0x04) == 0x04) //유속
                {
                    for (int i = 0; i < this.SensorKindLV.Items.Count; i++)
                    {
                        if (this.SensorKindLV.Items[i].Text == "유속계")
                        {
                            this.SensorKindLV.Items[i].Checked = true;
                        }
                    }
                }
            }

            this.WDeviceDivisionCB.SelectedItem = this.wDivision;
            this.EternetUseCB.Checked = this.wDEthernetUse;
            this.WDeviceRemarkTB.Text = this.WDRemark;
            this.SaveBtn.Enabled = false;
        }

        //확인 버튼 클릭
        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (!this.DataValidate())
            {
                MessageBox.Show("측기 정보를 확인하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.WDeviceTelNumTB.Text.Length < 10)
            {
                MessageBox.Show("전화번호를 정확히 입력해 주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.mode == (byte)FType.add)
            {
                if (!this.save())
                {
                    return;
                }
            }
            else if (this.mode == (byte)FType.update)
            {
                this.WDeviceUpdate();
            }

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
            if (!this.DataValidate())
            {
                MessageBox.Show("측기 정보를 확인하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.WDeviceTelNumTB.Text.Length < 10)
            {
                MessageBox.Show("전화번호를 정확히 입력해 주세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.mode == (byte)FType.add)
            {
                if (!this.save())
                {
                    return;
                }
            }
            else if (this.mode == (byte)FType.update)
            {
                this.WDeviceUpdate();
            }
        }

        /// <summary>
        /// 저장 메소드
        /// </summary>
        private bool save()
        {
            WTypeDevice wtd = this.dataMng.GetTypeDevice(this.WDeviceDivisionCB.Text.Substring(0, 3));
            byte tmpSensor = this.GetSensorValue();
            WDevice wd = new WDevice(0, this.WDeviceIDTB.Text, this.WDeviceNameTB.Text, this.WDeviceTelNumTB.Text.Replace("-", ""),
                wtd.PKID, tmpSensor, this.EternetUseCB.Checked, this.WDeviceRemarkTB.Text);

            if (this.dataMng.GetWDeviceComparer(wd))
            {
                MessageBox.Show("이미 등록된 ID 입니다.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                this.dataMng.AddWDevice(wd);
                this.SaveBtn.Enabled = false;
                return true;
            }
        }

        /// <summary>
        /// 수정 메소드
        /// </summary>
        private void WDeviceUpdate()
        {
            WTypeDevice wtd = this.dataMng.GetTypeDevice(this.WDeviceDivisionCB.Text.Substring(0, 3));
            byte tmpSensor = this.GetSensorValue();
            WDevice wd = new WDevice(uint.Parse(this.wDevicePkid), this.WDeviceIDTB.Text, this.WDeviceNameTB.Text,
                this.WDeviceTelNumTB.Text.Replace("-", ""), wtd.PKID, tmpSensor, this.EternetUseCB.Checked, this.WDeviceRemarkTB.Text);
            this.dataMng.UpdateWDevice(wd);
            this.SaveBtn.Enabled = false;
        }

        /// <summary>
        /// 데이터 유효성을 검사한다.
        /// </summary>
        /// <returns></returns>
        private bool DataValidate()
        {
            if (this.WDeviceDivisionCB.Text == string.Empty)
            {
                return false;
            }

            if (this.WDeviceIDTB.Text == string.Empty)
            {
                return false;
            }

            if (this.WDeviceNameTB.Text == string.Empty)
            {
                return false;
            }

            if (this.WDeviceTelNumTB.Text == string.Empty)
            {
                return false;
            }

            if (this.SensorKindLV.CheckedItems.Count < 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 콤보박스/텍스트박스 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WDeviceDivisionCB_SelectedIndexChanged(object sender, EventArgs e)
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

        /// <summary>
        /// 리스트뷰 아이템 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SensorKindLV_ItemChecked(object sender, ItemCheckedEventArgs e)
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

        /// <summary>
        /// 측기가 가지고 있는 센서에 대한 DB 저장값을 가져온다.
        /// </summary>
        /// <returns></returns>
        private byte GetSensorValue()
        {
            byte tmpSensor = byte.MinValue;

            for (int i = 0; i < this.SensorKindLV.CheckedItems.Count; i++)
            {
                for (int j = 0; j < this.dataMng.TypeSensorList.Count; j++)
                {
                    if (this.SensorKindLV.CheckedItems[i].Text == this.dataMng.TypeSensorList[j].Name)
                    {
                        if (this.dataMng.TypeSensorList[j].PKID == (uint)1)
                        {
                            tmpSensor += (byte)WDevice.SType.RAINS;
                        }

                        if (this.dataMng.TypeSensorList[j].PKID == (uint)2)
                        {
                            tmpSensor += (byte)WDevice.SType.WATERS;
                        }

                        if (this.dataMng.TypeSensorList[j].PKID == (uint)3)
                        {
                            tmpSensor += (byte)WDevice.SType.FLOWS;
                        }
                    }
                }
            }

            return tmpSensor;
        }

        //전화번호 텍스트박스에 숫자만 입력 가능하게 하는 이벤트
        private void WDeviceTelNumTB_KeyDown(object sender, KeyEventArgs e)
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
        /// 이더넷 사용여부 체크박스 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EternetUseCB_CheckedChanged(object sender, EventArgs e)
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
    }
}