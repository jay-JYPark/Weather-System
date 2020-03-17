using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

using System.Data;

namespace ADEng.Module.WeatherSystem
{
    /// <summary>
    /// Summary description for AlarmReport.
    /// </summary>
    public partial class AlarmReport : DataDynamics.ActiveReports.ActiveReport3
    {
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public AlarmReport()
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
        /// <param name="typeWeather">
        /// 관측종류
        /// </param>
        /// 
        public AlarmReport(string title, string term, string deviceName, string typeWeather, DataTable dTable)
        {
            InitializeComponent();

            this.txtTitle.Text = title; //타이틀
            this.txtTerm.Text = term; //조회기간
            this.txtDeviceName.Text = deviceName;//측기명
            this.txtTypeWeather.Text = typeWeather; //관측종류

            //typeWeather
            switch (typeWeather)
            {
                case "강우":
                    label8.Text = "관측값 (mm)";
                    break;
                case "수위":
                    label8.Text = "관측값 (m)";
                    break;
                case "유속":
                    label8.Text = "관측값 (m/s)";
                    break;
            }
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
