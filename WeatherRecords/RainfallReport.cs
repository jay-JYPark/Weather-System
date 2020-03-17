using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

using System.Data; //DataTable Type�� ����ϱ� ����

namespace ADEng.Module.WeatherSystem
{
    /// <summary>
    /// Summary description for WeatherReport.
    /// </summary>
    public partial class RainfallReport : DataDynamics.ActiveReports.ActiveReport3
    {
        /// <summary>
        ///�⺻ ������
        /// </summary>
        public RainfallReport()
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
        /// <param name="typeWeather">
        /// ���� ����
        /// </param>
        public RainfallReport(string title, string term, string deviceName, string typeWeather, DataTable dTable)
        {
            InitializeComponent();

            this.txtTitle.Text = title; //Ÿ��Ʋ
            this.txtTerm.Text = term; //��ȸ�Ⱓ
            this.txtDeviceName.Text = deviceName;//�����
            this.txtTypeWeather.Text = typeWeather;//��������

           
            if (dTable != null && dTable.Rows.Count!=0)
            {
                 this.DataSource = dTable;//����
                 //���� ����
                 //this.Fields["accum10min"].Value = 30 * 0.1;
                this.txtTotal.Text = dTable.Rows.Count.ToString(); //����Ʈ ��� Total 
                this.txtTotalEnd.Text = dTable.Rows.Count.ToString(); //����Ʈ �ϴ� Total
            }

            this.txtPrintDate.Text = DateTime.Now.ToString("yyyy-MM-dd  HH�� mm��"); //��� �Ͻ�
        }

    }
}
