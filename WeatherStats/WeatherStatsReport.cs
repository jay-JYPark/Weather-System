using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using System.Data;

using ChartFX.WinForms; //��Ʈ�� ���� ����
using System.Collections.Generic; //List �ڷ����� ���� ����

namespace ADEng.Module.WeatherSystem
{
    /// <summary>
    /// Summary description for WeatherStatsReport.
    /// </summary>
    public partial class WeatherStatsReport : DataDynamics.ActiveReports.ActiveReport3
    {
        //��Ʈ ��Ʈ��
        Chart chtWeather = null;

        /// <summary>
        ///�⺻ ������
        /// </summary>
        public WeatherStatsReport()
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
        /// ��ȸ����
        /// </param>
        /// <param name="unit">
        /// �ð� ����(�ѱ�)
        /// </param>
        /// <param name="deviceName">
        /// �����
        /// </param>
        /// <param name="typeWeather">
        /// ���� ����
        /// </param>
        ///<param name="dataList">
        /// ��Ʈ�� ����� ������
        /// </param>
        ///<param name="xAxisList">
        /// x�� �� ǥ�� ����Ʈ
        /// </param>
        ///<param name="timeNumber">
        /// �ð� ����(����)
        /// </param>
        ///<param name="alarmLevelList">
        /// �˶� ���� �� ����Ʈ
        /// </param>
        public WeatherStatsReport(string title, string term, string unit, string deviceName, string typeWeather
                                                  , List<double> dataList, List<XAxis> xAxisList, double timeNumber, List<double> alarmLevelList)
        {
            InitializeComponent();

            //����Ʈ ����� ���� ������ ���
            printCondition(title, term, unit, deviceName, typeWeather);

            // CustomChart  ��Ʈ��Type�� ChartFX�� �ٲٱ� 
            SetChartType();

            //��Ʈ �⺻ �Ӽ� ����
            setChartAttribute();

            // ��Ʈ �׸���
            drawChart(typeWeather, dataList, xAxisList, timeNumber, alarmLevelList);
        }

        /// <summary>
        /// ����Ʈ ����� ���� ������ ���
        /// </summary>
        /// <param name="title">
        /// ����Ʈ����
        /// </param>
        /// <param name="term">
        /// ��ȸ����
        /// </param>
        /// <param name="unit">
        /// �ð� ����(�ѱ�)
        /// </param>
        /// <param name="deviceName">
        /// �����
        /// </param>
        /// <param name="typeWeather">
        /// ���� ����
        /// </param>
        private void printCondition(string title, string term, string unit, string deviceName, string typeWeather)
        {
            this.txtTitle.Text = title; //����Ʈ����
            this.txtTerm.Text = term; //��ȸ����
            this.txtUnit.Text = unit;//�ð� ����(�ѱ�)
            this.txtDeviceName.Text = deviceName;//�����
            this.txtTypeWeather.Text = typeWeather;//���� ����
        }

        /// <summary>
        /// CustomChart  ��Ʈ��Type�� ChartFX�� �ٲٱ�
        /// </summary>
        private void SetChartType()
        {
            chtWeather = null;
            chtWeather = new Chart();
            Type tp = chtWeather.GetType();
            this.customChart.Type = tp;
            chtWeather = (Chart)this.customChart.Control;
        }


        /// <summary>
        /// ��Ʈ �⺻ �Ӽ� ����
        /// </summary>
        private void setChartAttribute()
        {
            //�׷��� �� ����
            chtWeather.Data.Series = 1;

            // ���� 
            chtWeather.LegendBox.Visible = false;

            //��Ʈ ������ ����
            chtWeather.AxisX.Font = new Font(FontFamily.GenericSerif, 5);
        }


        /// <summary>
        /// ��Ʈ �׸���
        /// </summary>
        /// <param name="typeWeather">
        /// ���� ����
        /// </param>
        ///<param name="dataList">
        /// ��Ʈ�� ����� ������
        /// </param>
        ///<param name="xAxisList">
        /// x�� �� ǥ�� ����Ʈ
        /// </param>
        ///<param name="timeNumber">
        /// �ð� ����(����)
        /// </param>
        ///<param name="alarmLevelList">
        /// �˶� ���� �� ����Ʈ
        /// </param>
        private void drawChart(string typeWeather, List<double> dataList, List<XAxis> xAxisList
                                           , double timeNumber, List<double> alarmLevelList)
        {
            //Y �� ��� Format �� ��Ʈ ���� ����
            SetAxixYandGraphType(typeWeather);

            //�����ð� �޾ƿ���
            double number = timeNumber; //double ��
            int numberToInt = Convert.ToInt32(number); //int�� ��ȯ

            // ��� ������ Ŭ����
            chtWeather.Data.Clear();

            //��ü ����Ʈ ���� ���� (number*6  =  �ð�* 1�ð��� ����)
            chtWeather.Data.Points = (numberToInt * 6) + 1;

            //�Ӱ�ġ �˶� �� �׸���
            drawAlarmLine(typeWeather, alarmLevelList);

            //X�� ��Ī ���
            SetAxisXName(xAxisList);

            //X �� �ð��� �ش��ϴ� ������ ���
            printDataBytimeOnGraph(xAxisList, dataList);
        }


        /// <summary>
        /// X�� ��Ī ���
        /// </summary>
        ///<param name="xAxisList">
        /// ����� X ���̸�
        /// </param>
        private void SetAxisXName(List<XAxis> xAxisList)
        {
            for (int i = 0; i < xAxisList.Count; i++)
            {
                chtWeather.AxisX.Labels[i] = xAxisList[i].DateOutputFormat;
            }
        }


        /// <summary>
        /// Y �� ��� Format �� ��Ʈ ���� ����
        /// </summary>
        ///<param name="typeWeather">
        /// ���� ����(������� ����)
        /// </param>
        private void SetAxixYandGraphType(string typeWeather)
        {
            if (typeWeather.Equals("����"))
            {
                // Y �� ������ ���� ����
                chtWeather.AxisY.LabelsFormat.CustomFormat = "#.0 mm";
                // ��Ʈ ����
                chtWeather.Gallery = ChartFX.WinForms.Gallery.Bar;
            }
            else if (typeWeather.Equals("����"))
            {
                // Y �� ������ ���� ����
                chtWeather.AxisY.LabelsFormat.CustomFormat = "#.#0 m";
                // ��Ʈ ����
                chtWeather.Gallery = ChartFX.WinForms.Gallery.Area;
            }
            else if (typeWeather.Equals("����"))
            {
                // Y �� ������ ���� ����
                chtWeather.AxisY.LabelsFormat.CustomFormat = "#.0 m/s";
                // ��Ʈ ����
                chtWeather.Gallery = ChartFX.WinForms.Gallery.Curve;
            }
        }


        /// <summary>
        ///  X �� �ð��� �ش��ϴ� ������ ���
        /// </summary>
        ///<param name="xAxisList">
        /// X �� �� ǥ�� ����Ʈ
        /// </param>
        ///<param name="dataList">
        /// ��Ʈ�� ����� ������
        /// </param>
        private void printDataBytimeOnGraph(List<XAxis> xAxisList, List<double> dataList)
        {
            for (int i = 0; i < xAxisList.Count; i++)
            {
                if (dataList[i] != double.MinValue)
                {
                    chtWeather.Data[0, i] = dataList[i];
                }
            }
        }


        /// <summary>
        ///  �Ӱ�ġ �˶� �� �׸���
        /// </summary>
        ///<param name="strTypeWeather">���� ����(����, ����, ����)</param>
        private void drawAlarmLine(string strTypeWeather, List<double> alarmLevelList)
        {
            if (alarmLevelList != null)
            {
                //�׷����� �˶�����
                CustomGridLine alarmLine1 = null;
                CustomGridLine alarmLine2 = null;
                CustomGridLine alarmLine3 = null;

                //�׷��� �� �ʱ�ȭ
                chtWeather.AxisY.CustomGridLines.Clear();

                //���
                alarmLine1 = new CustomGridLine();
                alarmLine1.Value = alarmLevelList[0];
                alarmLine1.Color = Color.OrangeRed;
                alarmLine1.Text = "���";
                alarmLine1.TextColor = Color.Orange;
                chtWeather.AxisY.CustomGridLines.Add(alarmLine1);

                //����
                alarmLine2 = new CustomGridLine();
                alarmLine2.Value = alarmLevelList[1];
                alarmLine2.Color = Color.OrangeRed;
                alarmLine2.Text = "����";
                alarmLine2.TextColor = Color.OrangeRed;
                chtWeather.AxisY.CustomGridLines.Add(alarmLine2);

                //����
                alarmLine3 = new CustomGridLine();
                alarmLine3.Value = alarmLevelList[2];
                alarmLine3.Color = Color.Red;
                alarmLine3.Text = "����";
                alarmLine3.TextColor = Color.Red;
                chtWeather.AxisY.CustomGridLines.Add(alarmLine3);
            }
        }

    }
}
