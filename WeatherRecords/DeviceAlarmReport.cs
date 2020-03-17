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
        /// �⺻ ������
        /// </summary>
        public DeviceAlarmReport()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="title">
        /// ����Ʈ����
        /// </param>
        /// <param name="term">
        /// ��ȸ�Ⱓ
        /// </param>
        /// <param name="deviceName">
        /// �����
        /// </param>
        /// 
        public DeviceAlarmReport(string title, string term, string deviceName, DataTable dTable)
        {
            InitializeComponent();

            this.txtTitle.Text = title; //Ÿ��Ʋ
            this.txtTerm.Text = term; //��ȸ�Ⱓ
            this.txtDeviceName.Text = deviceName;//�����

            if (dTable != null)
            {
                this.DataSource = dTable;//����
                this.txtTotal.Text = dTable.Rows.Count.ToString(); //����Ʈ ��� Total 
                this.txtTotalEnd.Text = dTable.Rows.Count.ToString(); //����Ʈ �ϴ� Total
            }

            this.txtPrintDate.Text = DateTime.Now.ToString("yyyy-MM-dd  HH�� mm��"); //��� �Ͻ�     
        }
    }
}
