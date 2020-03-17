using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using System.Data;

using ChartFX.WinForms; //차트를 쓰기 위함
using System.Collections.Generic; //List 자료형을 쓰기 위함

namespace ADEng.Module.WeatherSystem
{
    /// <summary>
    /// Summary description for WeatherStatsReport.
    /// </summary>
    public partial class WeatherStatsReport : DataDynamics.ActiveReports.ActiveReport3
    {
        //차트 컨트롤
        Chart chtWeather = null;

        /// <summary>
        ///기본 생성자
        /// </summary>
        public WeatherStatsReport()
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
        /// 조회일자
        /// </param>
        /// <param name="unit">
        /// 시간 단위(한글)
        /// </param>
        /// <param name="deviceName">
        /// 측기명
        /// </param>
        /// <param name="typeWeather">
        /// 관측 종류
        /// </param>
        ///<param name="dataList">
        /// 차트에 출력할 데이터
        /// </param>
        ///<param name="xAxisList">
        /// x축 라벨 표시 리스트
        /// </param>
        ///<param name="timeNumber">
        /// 시간 단위(숫자)
        /// </param>
        ///<param name="alarmLevelList">
        /// 알람 레벨 값 리스트
        /// </param>
        public WeatherStatsReport(string title, string term, string unit, string deviceName, string typeWeather
                                                  , List<double> dataList, List<XAxis> xAxisList, double timeNumber, List<double> alarmLevelList)
        {
            InitializeComponent();

            //레포트 헤더에 조건 데이터 출력
            printCondition(title, term, unit, deviceName, typeWeather);

            // CustomChart  컨트롤Type을 ChartFX로 바꾸기 
            SetChartType();

            //차트 기본 속성 세팅
            setChartAttribute();

            // 차트 그리기
            drawChart(typeWeather, dataList, xAxisList, timeNumber, alarmLevelList);
        }

        /// <summary>
        /// 레포트 헤더에 조건 데이터 출력
        /// </summary>
        /// <param name="title">
        /// 레포트제목
        /// </param>
        /// <param name="term">
        /// 조회일자
        /// </param>
        /// <param name="unit">
        /// 시간 단위(한글)
        /// </param>
        /// <param name="deviceName">
        /// 측기명
        /// </param>
        /// <param name="typeWeather">
        /// 관측 종류
        /// </param>
        private void printCondition(string title, string term, string unit, string deviceName, string typeWeather)
        {
            this.txtTitle.Text = title; //레포트제목
            this.txtTerm.Text = term; //조회일자
            this.txtUnit.Text = unit;//시간 단위(한글)
            this.txtDeviceName.Text = deviceName;//측기명
            this.txtTypeWeather.Text = typeWeather;//관측 종류
        }

        /// <summary>
        /// CustomChart  컨트롤Type을 ChartFX로 바꾸기
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
        /// 차트 기본 속성 세팅
        /// </summary>
        private void setChartAttribute()
        {
            //그래프 선 개수
            chtWeather.Data.Series = 1;

            // 범례 
            chtWeather.LegendBox.Visible = false;

            //폰트 사이즈 설정
            chtWeather.AxisX.Font = new Font(FontFamily.GenericSerif, 5);
        }


        /// <summary>
        /// 차트 그리기
        /// </summary>
        /// <param name="typeWeather">
        /// 관측 종류
        /// </param>
        ///<param name="dataList">
        /// 차트에 출력할 데이터
        /// </param>
        ///<param name="xAxisList">
        /// x축 라벨 표시 리스트
        /// </param>
        ///<param name="timeNumber">
        /// 시간 단위(숫자)
        /// </param>
        ///<param name="alarmLevelList">
        /// 알람 레벨 값 리스트
        /// </param>
        private void drawChart(string typeWeather, List<double> dataList, List<XAxis> xAxisList
                                           , double timeNumber, List<double> alarmLevelList)
        {
            //Y 축 출력 Format 및 차트 종류 설정
            SetAxixYandGraphType(typeWeather);

            //단위시간 받아오기
            double number = timeNumber; //double 형
            int numberToInt = Convert.ToInt32(number); //int로 변환

            // 모든 데이터 클리어
            chtWeather.Data.Clear();

            //전체 포인트 개수 설정 (number*6  =  시간* 1시간당 개수)
            chtWeather.Data.Points = (numberToInt * 6) + 1;

            //임계치 알람 선 그리기
            drawAlarmLine(typeWeather, alarmLevelList);

            //X축 명칭 출력
            SetAxisXName(xAxisList);

            //X 축 시간에 해당하는 데이터 출력
            printDataBytimeOnGraph(xAxisList, dataList);
        }


        /// <summary>
        /// X축 명칭 출력
        /// </summary>
        ///<param name="xAxisList">
        /// 출력할 X 축이름
        /// </param>
        private void SetAxisXName(List<XAxis> xAxisList)
        {
            for (int i = 0; i < xAxisList.Count; i++)
            {
                chtWeather.AxisX.Labels[i] = xAxisList[i].DateOutputFormat;
            }
        }


        /// <summary>
        /// Y 축 출력 Format 및 차트 종류 설정
        /// </summary>
        ///<param name="typeWeather">
        /// 관측 종류(기상정보 종류)
        /// </param>
        private void SetAxixYandGraphType(string typeWeather)
        {
            if (typeWeather.Equals("강우"))
            {
                // Y 축 데이터 포맷 설정
                chtWeather.AxisY.LabelsFormat.CustomFormat = "#.0 mm";
                // 차트 종류
                chtWeather.Gallery = ChartFX.WinForms.Gallery.Bar;
            }
            else if (typeWeather.Equals("수위"))
            {
                // Y 축 데이터 포맷 설정
                chtWeather.AxisY.LabelsFormat.CustomFormat = "#.#0 m";
                // 차트 종류
                chtWeather.Gallery = ChartFX.WinForms.Gallery.Area;
            }
            else if (typeWeather.Equals("유속"))
            {
                // Y 축 데이터 포맷 설정
                chtWeather.AxisY.LabelsFormat.CustomFormat = "#.0 m/s";
                // 차트 종류
                chtWeather.Gallery = ChartFX.WinForms.Gallery.Curve;
            }
        }


        /// <summary>
        ///  X 축 시간에 해당하는 데이터 출력
        /// </summary>
        ///<param name="xAxisList">
        /// X 축 라벨 표시 리스트
        /// </param>
        ///<param name="dataList">
        /// 차트에 출력할 데이터
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
        ///  임계치 알람 선 그리기
        /// </summary>
        ///<param name="strTypeWeather">관측 종류(강우, 수위, 유속)</param>
        private void drawAlarmLine(string strTypeWeather, List<double> alarmLevelList)
        {
            if (alarmLevelList != null)
            {
                //그래프의 알람라인
                CustomGridLine alarmLine1 = null;
                CustomGridLine alarmLine2 = null;
                CustomGridLine alarmLine3 = null;

                //그래프 선 초기화
                chtWeather.AxisY.CustomGridLines.Clear();

                //경계
                alarmLine1 = new CustomGridLine();
                alarmLine1.Value = alarmLevelList[0];
                alarmLine1.Color = Color.OrangeRed;
                alarmLine1.Text = "경계";
                alarmLine1.TextColor = Color.Orange;
                chtWeather.AxisY.CustomGridLines.Add(alarmLine1);

                //위험
                alarmLine2 = new CustomGridLine();
                alarmLine2.Value = alarmLevelList[1];
                alarmLine2.Color = Color.OrangeRed;
                alarmLine2.Text = "위험";
                alarmLine2.TextColor = Color.OrangeRed;
                chtWeather.AxisY.CustomGridLines.Add(alarmLine2);

                //대피
                alarmLine3 = new CustomGridLine();
                alarmLine3.Value = alarmLevelList[2];
                alarmLine3.Color = Color.Red;
                alarmLine3.Text = "대피";
                alarmLine3.TextColor = Color.Red;
                chtWeather.AxisY.CustomGridLines.Add(alarmLine3);
            }
        }

    }
}
