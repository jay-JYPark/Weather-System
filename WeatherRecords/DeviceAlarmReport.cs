using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using System.Data;

namespace ADEng.Library.WeatherSystem
{
    /// <summary>
    /// Summary description for DeviceAlarmReport.
    /// </summary>
    public partial class DeviceAlarmReport : DataDynamics.ActiveReports.ActiveReport3
    {

        /// <summary>
        /// 기본 생성자
        /// </summary>
        public DeviceAlarmReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="title">
        /// 레포트제목
        /// </param>
        /// <param name="term">
        /// 조회기간
        /// </param>
        /// <param name="deviceName">
        /// 측기명
        /// </param>
        /// 
        public DeviceAlarmReport(string title, string term, string deviceName, DataTable dTable)
        {
            InitializeComponent();

            this.txtTitle.Text = title; //타이틀
            this.txtTerm.Text = term; //조회기간
            this.txtDeviceName.Text = deviceName;//측기명

            if (dTable != null)
            {
                this.DataSource = dTable;//정보
                this.txtTotal.Text = dTable.Rows.Count.ToString(); //레포트 상단 Total 
                this.txtTotalEnd.Text = dTable.Rows.Count.ToString(); //레포트 하단 Total
            }

            this.txtPrintDate.Text = DateTime.Now.ToString("yyyy-MM-dd  HH시 mm분"); //출력 일시     
        }
    }
}
