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
    public partial class SMSRecvKindDlg : Form
    {
        /// <summary>
        /// 수신할 SMS 항목 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rskea"></param>
        private delegate void RecvSMSKindEventHandler(object sender, RecvSMSKindEventArgs rskea);

        /// <summary>
        /// 수신할 SMS 항목 체크 이벤트
        /// </summary>
        public event EventHandler<RecvSMSKindEventArgs> OnSmsRecvKind;

        /// <summary>
        /// 생성자
        /// </summary>
        public SMSRecvKindDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="smsKindClass"></param>
        public SMSRecvKindDlg(WSmsRecvKind _smsKindClass)
        {
            InitializeComponent();

            this.init(_smsKindClass);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void init(WSmsRecvKind _smsKindClass)
        {
            this.Alarm1CB.Checked = _smsKindClass.Alarm1;
            this.Alarm2CB.Checked = _smsKindClass.Alarm2;
            this.Alarm3CB.Checked = _smsKindClass.Alarm3;
            this.Batt1Volt.Checked = _smsKindClass.Batt1Volt;
            this.Batt1Tempo.Checked = _smsKindClass.Batt1Tempo;
            this.Batt1Test.Checked = _smsKindClass.Batt1Test;
            this.Batt1Repair.Checked = _smsKindClass.Batt1Repair;
            this.Batt1Reset.Checked = _smsKindClass.Batt1Reset;
            this.Batt2Volt.Checked = _smsKindClass.Batt2Volt;
            this.Batt2Tempo.Checked = _smsKindClass.Batt2Tempo;
            this.Batt2Test.Checked = _smsKindClass.Batt2Test;
            this.Batt2Repair.Checked = _smsKindClass.Batt2Repair;
            this.Batt2Reset.Checked = _smsKindClass.Batt2Reset;
            this.SensorCB.Checked = _smsKindClass.SensorState;
            this.FanCB.Checked = _smsKindClass.FanState;
            this.CdmaCB.Checked = _smsKindClass.CDMATime;
        }

        /// <summary>
        /// 확인 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            WSmsRecvKind wSmsRecvKind = new WSmsRecvKind(
                this.Alarm1CB.Checked,
                this.Alarm2CB.Checked,
                this.Alarm3CB.Checked,
                this.Batt1Volt.Checked,
                this.Batt1Tempo.Checked,
                this.Batt1Test.Checked,
                this.Batt1Repair.Checked,
                this.Batt1Reset.Checked,
                this.Batt2Volt.Checked,
                this.Batt2Tempo.Checked,
                this.Batt2Test.Checked,
                this.Batt2Repair.Checked,
                this.Batt2Reset.Checked,
                this.SensorCB.Checked,
                this.FanCB.Checked,
                this.CdmaCB.Checked);

            if (this.OnSmsRecvKind != null)
            {
                this.OnSmsRecvKind(this, new RecvSMSKindEventArgs(wSmsRecvKind));
            }

            this.Close();
        }

        //임계치 알람 항목 체크 변경 이벤트
        private void Alarm1CB_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.Alarm1CB.Checked && this.Alarm2CB.Checked && this.Alarm3CB.Checked)
            //{
            //    this.AlarmAllCB.Checked = true;
            //}
            //else
            //{
            //    this.AlarmAllCB.Checked = false;
            //}
        }

        //임계치 알람 전체 체크 변경 이벤트
        private void AlarmAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AlarmAllCB.Checked)
            {
                this.Alarm1CB.Checked = true;
                this.Alarm2CB.Checked = true;
                this.Alarm3CB.Checked = true;
            }
            else
            {
                this.Alarm1CB.Checked = false;
                this.Alarm2CB.Checked = false;
                this.Alarm3CB.Checked = false;
            }
        }

        //배터리 1 항목 체크 변경 이벤트
        private void Batt1Volt_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.Batt1Volt.Checked && this.Batt1Tempo.Checked && this.Batt1Test.Checked && this.Batt1Repair.Checked && this.Batt1Reset.Checked)
            //{
            //    this.batt1AllCB.Checked = true;
            //}
            //else
            //{
            //    this.batt1AllCB.Checked = false;
            //}
        }

        //배터리 1 전체 체크 변경 이벤트
        private void batt1AllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.batt1AllCB.Checked)
            {
                this.Batt1Volt.Checked = true;
                this.Batt1Tempo.Checked = true;
                this.Batt1Test.Checked = true;
                this.Batt1Repair.Checked = true;
                this.Batt1Reset.Checked = true;
            }
            else
            {
                this.Batt1Volt.Checked = false;
                this.Batt1Tempo.Checked = false;
                this.Batt1Test.Checked = false;
                this.Batt1Repair.Checked = false;
                this.Batt1Reset.Checked = false;
            }
        }

        //배터리 2 항목 체크 변경 이벤트
        private void Batt2Volt_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.Batt2Volt.Checked && this.Batt2Tempo.Checked && this.Batt2Test.Checked && this.Batt2Repair.Checked && this.Batt2Reset.Checked)
            //{
            //    this.batt2AllCB.Checked = true;
            //}
            //else
            //{
            //    this.batt2AllCB.Checked = false;
            //}
        }

        //배터리 2 전체 체크 변경 이벤트
        private void batt2AllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.batt2AllCB.Checked)
            {
                this.Batt2Volt.Checked = true;
                this.Batt2Tempo.Checked = true;
                this.Batt2Test.Checked = true;
                this.Batt2Repair.Checked = true;
                this.Batt2Reset.Checked = true;
            }
            else
            {
                this.Batt2Volt.Checked = false;
                this.Batt2Tempo.Checked = false;
                this.Batt2Test.Checked = false;
                this.Batt2Repair.Checked = false;
                this.Batt2Reset.Checked = false;
            }
        }

        //기타 항목 체크 변경 이벤트
        private void SensorCB_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.SensorCB.Checked && this.FanCB.Checked && this.CdmaCB.Checked)
            //{
            //    this.EtcAllCB.Checked = true;
            //}
            //else
            //{
            //    this.EtcAllCB.Checked = false;
            //}
        }

        //기타 전체 체크 변경 이벤트
        private void EtcAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EtcAllCB.Checked)
            {
                this.SensorCB.Checked = true;
                this.FanCB.Checked = true;
                this.CdmaCB.Checked = true;
            }
            else
            {
                this.SensorCB.Checked = false;
                this.FanCB.Checked = false;
                this.CdmaCB.Checked = false;
            }
        }
    }

    /// <summary>
    /// 수신할 SMS 항목 이벤트에 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class RecvSMSKindEventArgs : EventArgs
    {
        private WSmsRecvKind smsKindClass;

        public WSmsRecvKind SmsKindClass
        {
            get { return this.smsKindClass; }
            set { this.smsKindClass = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_smsKindClass"></param>
        public RecvSMSKindEventArgs(WSmsRecvKind _smsKindClass)
        {
            this.smsKindClass = _smsKindClass;
        }
    }
}