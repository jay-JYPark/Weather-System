using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ADEng.Library;  //DAC 사용을 위함
using ADEng.Library.WeatherSystem; //WDevice 사용을 위함
using ChartFX.WinForms; //차트 FX CustomGridLine을 그리기 위함

using ADEng.Control;
using WeatherStats.Properties;//Report 로드할 폼을 쓰기 위함 (clsPView)


namespace ADEng.Module.WeatherSystem
{
    public partial class StatsForm : Form
    {
        private WeatherDataMng dataMng = null;

        //오라클 접속
        oracleDAC odec = null;
        StringBuilder sBuilder = null;
        DataTable dTable = null;

        //측기 리스트
        List<WDevice> wDeviceList = new List<WDevice>();
        //시간 리스트
        List<Time> timeList = new List<Time>();
        //X축 리스트
        List<XAxis> xAxisList = new List<XAxis>();
        //그래프의 포인트 값
        List<int> pointList = new List<int>();
        ////알람레벨 리스트
        List<double> alarmLevelList = null;

        //차트에 넘겨줄 DataList
        List<double> dataList = null;

        //조회 시, 조건 저장
        //조회기간
        string strTerm = string.Empty;

        //시간단위
        string strUnit = string.Empty;

        //측기명
        string strDeviceName = string.Empty;

        //관측종류
        string strTypeWeather = string.Empty;


        /// <summary>
        /// 기본 생성자
        /// </summary>
        public StatsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        ///  폼이 로드될때
        /// </summary>
        private void StatsForm_Load(object sender, EventArgs e)
        {
            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onDBDataSetEvt += new EventHandler<SetDBDataEventArgs>(dataMng_onDBDataSetEvt);

            string ip = Settings.Default.DbIp;
            string port = Settings.Default.DbPort;
            string id = Settings.Default.DbId;
            string pw = Settings.Default.DbPw;
            string sid = Settings.Default.DbSid;

            this.odec = new ADEng.Library.oracleDAC(id, pw, ip, port, sid);

            //측기 리스트 가져오기
            getDeviceList();

            //시간 리스트 넣어주기
            getTimeList();

            //차트 메뉴 막기
            blockChartMenu();

            //차트 기본 속성 세팅
            setChartAttribute();

            // 조회 조건의 Base 정보 로드
            loadBaseData();

            // 모든 데이터 클리어
            chtWeather2.Data.Clear();
        }

        //DB 항목을 설정하면 발생하는 이벤트
        private void dataMng_onDBDataSetEvt(object sender, SetDBDataEventArgs e)
        {
            Settings.Default.DbIp = e.Ip;
            Settings.Default.DbPort = e.Port;
            Settings.Default.DbId = e.Id;
            Settings.Default.DbPw = e.Pw;
            Settings.Default.DbSid = e.Sid;
            Settings.Default.Save();
        }

        /// <summary>
        ///차트 메뉴 막기
        /// </summary>
        private void blockChartMenu()
        {
            chtWeather2.ContextMenus = false;
            chtWeather2.ToolBar.Visible = false;
            chtWeather2.MenuBar.Visible = false;
            chtWeather2.ToolTips = false;
        }

        /// <summary>
        ///측기 리스트 가져오기
        /// </summary>
        private void getDeviceList()
        {
            try
            {
                if (odec.openDb())
                {
                    //측기 리스트 출력하기
                    sBuilder = null;
                    dTable = null;

                    sBuilder = new StringBuilder(100);
                    sBuilder.Append("SELECT * FROM device WHERE isuse = 1 ORDER BY devicename");

                    dTable = odec.getDataTable(sBuilder.ToString(), "device");

                    foreach (DataRow dRow in dTable.Rows)
                    {
                        this.wDeviceList.Add(new WDevice(Convert.ToUInt32(dRow["pkid"])
                                                                               , dRow["deviceid"].ToString()
                                                                               , dRow["devicename"].ToString()
                                                                               , dRow["telephone"].ToString()
                                                                               , Convert.ToUInt32(dRow["fktypedevice"])
                                                                               , Convert.ToByte(dRow["sensor"])
                                                                               , (Convert.ToByte(dRow["ethernetUse"]) == 1) ? true : false
                                                                               , dRow["remark"].ToString())
                                                        );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("NListForm.printData() : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
        }


        /// <summary>
        ///시간 리스트 넣어주기
        /// </summary>
        private void getTimeList()
        {
            this.timeList.Add(new Time(2.0, "이전 2시간"));
            this.timeList.Add(new Time(6.0, "이전 6시간"));
            this.timeList.Add(new Time(12.0, "이전 12시간"));
            this.timeList.Add(new Time(24.0, "이전 1일"));
            //this.timeList.Add(new Time(7.0, 7일")); //이 이후로...하루가 넘어감...시간으로 환산...?
            //this.timeList.Add(new Time(30.0, "1달"));  //날짜 변동이 있음..
            //this.timeList.Add(new Time(365.0, "1년"));
        }


        /// <summary>
        /// 차트 기본 속성 세팅
        /// </summary>
        private void setChartAttribute()
        {
            //그래프 선 개수
            chtWeather2.Data.Series = 1;

            // 범례 
            this.chtWeather2.LegendBox.Visible = false;

            //폰트 사이즈 설정
            chtWeather2.AxisX.Font = new Font(FontFamily.GenericSansSerif, 8);
        }

        /// <summary>
        /// 조회 조건의 Base 정보 로드
        /// </summary>
        private void loadBaseData()
        {
            //시간 리스트를 추가
            addTimeList();

            //측기 리스트 출력
            addDeviceList();
        }

        /// <summary>
        /// 시간 리스트를 추가
        /// </summary>
        private void addTimeList()
        {
            foreach (Time time in timeList)
            {
                cbxByTime.Items.Add(time.Text);
            }
            if (cbxByTime.Items.Count > 0)
            {
                cbxByTime.SelectedIndex = 0; //첫번째 항목 선택
            }
        }


        /// <summary>
        /// 측기 리스트 출력
        /// </summary>
        private void addDeviceList()
        {
            foreach (WDevice device in wDeviceList)
            {
                //기상 정보 Tab Page  의 측기리스트
                this.cbxDeviceName.Items.Add(device.Name.Trim());
            }
            if (cbxDeviceName.Items.Count > 0)
            {
                this.cbxDeviceName.SelectedIndex = 0; //리스트의 첫 항목 선택하기
            }
        }


        /// <summary>
        /// 측기 콤보박스 인덱스 변경시 해당 센서 리스트를 보여준다.
        /// </summary>
        private void cbxDeviceNameIndexChanged(object sender, EventArgs e)
        {
            cbxTypeWeather.Items.Clear();
            cbxTypeWeather.Enabled = true;


            switch (wDeviceList[cbxDeviceName.SelectedIndex].HaveSensor.ToString())
            {
                case "1":
                    cbxTypeWeather.Items.Add("강우");
                    break;
                case "2":
                    cbxTypeWeather.Items.Add("수위");
                    break;
                case "3":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("수위");
                    break;
                case "4":
                    cbxTypeWeather.Items.Add("유속");
                    break;
                case "5":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("유속");
                    break;
                case "6":
                    cbxTypeWeather.Items.Add("수위");
                    cbxTypeWeather.Items.Add("유속");
                    break;
                case "7":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("수위");
                    cbxTypeWeather.Items.Add("유속");
                    break;
                case "8":
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "9":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "10":
                    cbxTypeWeather.Items.Add("수위");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "11":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("수위");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "12":
                    cbxTypeWeather.Items.Add("유속");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "13":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("유속");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "14":
                    cbxTypeWeather.Items.Add("수위");
                    cbxTypeWeather.Items.Add("유속");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                case "15":
                    cbxTypeWeather.Items.Add("강우");
                    cbxTypeWeather.Items.Add("수위");
                    cbxTypeWeather.Items.Add("유속");
                    cbxTypeWeather.Items.Add("풍향풍속");
                    break;
                default:
                    cbxTypeWeather.Items.Add("센서 없음");
                    break;
            }            
            cbxTypeWeather.SelectedIndex = 0; // 첫 항목 선택하기
        }


        /// <summary>
        /// 조회 버튼 클릭 시,
        /// </summary>
        private void btnSearchClick(object sender, EventArgs e)
        {
            //조건1:  기상정보 종류(강우,...)
            //조건2:  시간단위(2시간,...)
            if (cbxTypeWeather.SelectedItem.ToString().Equals("강우"))
            {
                // Y 축 데이터 포맷 설정
                chtWeather2.AxisY.LabelsFormat.CustomFormat = "#.0 mm";
                // 차트 종류
                chtWeather2.Gallery = ChartFX.WinForms.Gallery.Bar;
            }
            else if (cbxTypeWeather.SelectedItem.ToString().Equals("수위"))
            {
                // Y 축 데이터 포맷 설정
                chtWeather2.AxisY.LabelsFormat.CustomFormat = "#.#0 m";
                // 차트 종류
                chtWeather2.Gallery = ChartFX.WinForms.Gallery.Area;
            }
            else if (cbxTypeWeather.SelectedItem.ToString().Equals("유속"))
            {
                // Y 축 데이터 포맷 설정
                chtWeather2.AxisY.LabelsFormat.CustomFormat = "#.0 m/s";
                // 차트 종류
                chtWeather2.Gallery = ChartFX.WinForms.Gallery.Curve;
            }

            //단위시간 받아오기
            double number = timeList[cbxByTime.SelectedIndex].Number; //double 형
            int numberToInt = Convert.ToInt32(number); //int로 변환
            //int numberToIntMultiple = numberToInt * 6;

            //X 축 시간 리스트 생성
            makeXAxisList2(numberToInt);

            // 모든 데이터 클리어
            chtWeather2.Data.Clear();

            //전체 포인트 개수 설정
            //number*6  =  시간* 1시간당 개수
            chtWeather2.Data.Points = (numberToInt * 6) + 1;

            //임계치 알람 선 그리기
            drawAlarmLine(cbxTypeWeather.SelectedItem.ToString());

            //X축 명칭 출력
            for (int i = 0; i < xAxisList.Count; i++)
            //for (int i = 0; i <= 12; i++)
            {
                chtWeather2.AxisX.Labels[i] = xAxisList[i].DateOutputFormat;
            }

            //X 축 시간에 해당하는 데이터 출력
            printDataBytimeOnGraph(cbxTypeWeather.SelectedItem.ToString());

            //단위시간에 따른 X 축 출력 개수 설정
            printAxisXTerm();

            //조회조건 저장
            saveConstraint();

        }

        /// <summary>
        ///조회조건 저장
        /// </summary>
        private void saveConstraint()
        {
            //조회기간
           strTerm = this.dtpDate.Value.ToString("yyyy-MM-dd  HH시 mm분");

            //시간단위
            strUnit = this.cbxByTime.SelectedItem.ToString();

            //측기명
           strDeviceName = wDeviceList[cbxDeviceName.SelectedIndex].Name;

            //관측종류
            strTypeWeather = cbxTypeWeather.SelectedItem.ToString();
        }

        /// <summary>
        ///단위시간에 따른 X 축 출력 개수 설정
        /// </summary>
        private void printAxisXTerm()
        {
            switch (Convert.ToInt32(timeList[cbxByTime.SelectedIndex].Number))
            {
                case 2:
                    chtWeather2.AxisX.Step = 0;
                    break;
                case 6:
                    chtWeather2.AxisX.Step = 3;
                    break;
                case 12:
                    chtWeather2.AxisX.Step = 6;
                    break;
                case 24:
                    chtWeather2.AxisX.Step = 12;
                    break;
                default:
                    chtWeather2.AxisX.Step = 0;
                    break;
            }
            //chtWeather2.AxisY.Max = chtWeather2.Data
        }


        /// <summary>
        ///  X 축 시간에 해당하는 데이터 출력
        /// </summary>
        ///<param name="strTypeWeather">관측 종류(강우, 수위, 유속)</param>
        private void printDataBytimeOnGraph(string strTypeWeather)
        {
            dataList = new List<double>(xAxisList.Count); //차트에 넘겨줄 데이터 리스트
            //소수점 데이터를 표현하기 위함
            try
            {
                if (odec.openDb())
                {
                    switch (strTypeWeather)
                    {
                        case "강우":
                            for (int i = 0; i < xAxisList.Count; i++)
                            {
                                sBuilder = null;
                                dTable = null;
                                sBuilder = new StringBuilder(300);

                                sBuilder.AppendFormat(" SELECT  accum10min FROM rainfall WHERE fkdevice = {0} "
                                                                   , wDeviceList[cbxDeviceName.SelectedIndex].PKID.ToString());
                                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                                  , xAxisList[i].DateFullName);
                                sBuilder.AppendFormat(" AND chktime <= to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                                    , xAxisList[i].DateFullName);

                                dTable = odec.getDataTable(sBuilder.ToString(), "rainfall");

                                string strResult = string.Empty;
                                if ((dTable.Rows.Count) > 0 &&
                                    (dTable.Rows[0]["accum10min"].ToString() != ""))
                                {
                                    if (dTable.Rows[0]["accum10min"].ToString() != "0")
                                    {
                                        strResult = String.Format("{0:0.0} ", (double.Parse(dTable.Rows[0]["accum10min"].ToString())) * 0.1);
                                    }
                                    else
                                    {
                                        strResult = "0.0";
                                    }
                                    chtWeather2.Data[0, i] = Convert.ToDouble(strResult);
                                    dataList.Add(chtWeather2.Data[0, i]);
                                }
                                else
                                {
                                    dataList.Add(double.MinValue);
                                }
                            }
                            break;
                        case "수위":
                            for (int i = 0; i < xAxisList.Count; i++)
                            {
                                sBuilder = null;
                                dTable = null;

                                sBuilder = new StringBuilder(300);

                                sBuilder.AppendFormat(" SELECT now FROM waterlevel WHERE fkdevice = {0} "
                                                                   , wDeviceList[cbxDeviceName.SelectedIndex].PKID.ToString());
                                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                                  , xAxisList[i].DateFullName);
                                sBuilder.AppendFormat(" AND chktime <= to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                                    , xAxisList[i].DateFullName);

                                dTable = odec.getDataTable(sBuilder.ToString(), "waterlevel");
                                string strResult = string.Empty;
                                if ((dTable.Rows.Count) > 0 &&
                                    (dTable.Rows[0]["now"].ToString() != ""))
                                {
                                    if (dTable.Rows[0]["now"].ToString() != "0")
                                    {
                                        strResult = String.Format("{0:0.00} ", (double.Parse(dTable.Rows[0]["now"].ToString())) * 0.01);
                                    }
                                    else
                                    {
                                        strResult = "0.00";
                                    }
                                    chtWeather2.Data[0, i] = Convert.ToDouble(strResult);
                                    dataList.Add(chtWeather2.Data[0, i]);
                                }
                                else
                                {
                                    dataList.Add(double.MinValue);
                                }
                            }
                            break;
                        case "유속":
                            for (int i = 0; i < xAxisList.Count; i++)
                            {
                                sBuilder = null;
                                dTable = null;

                                sBuilder = new StringBuilder(300);

                                sBuilder.AppendFormat(" SELECT  now FROM flowspeed WHERE fkdevice = {0} "
                                                                   , wDeviceList[cbxDeviceName.SelectedIndex].PKID.ToString());
                                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                                  , xAxisList[i].DateFullName);
                                sBuilder.AppendFormat(" AND chktime <= to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                                    , xAxisList[i].DateFullName);

                                dTable = odec.getDataTable(sBuilder.ToString(), "flowspeed");
                                string strResult = string.Empty;
                                if ((dTable.Rows.Count) > 0 &&
                                    (dTable.Rows[0]["now"].ToString() != ""))
                                {
                                    if (dTable.Rows[0]["now"].ToString() != "0")
                                    {
                                        strResult = String.Format("{0:0.0} ", (double.Parse(dTable.Rows[0]["now"].ToString())) * 0.1);
                                    }
                                    else
                                    {
                                        strResult = "0.0";
                                    }
                                    chtWeather2.Data[0, i] = Convert.ToDouble(strResult);
                                    dataList.Add(chtWeather2.Data[0, i]);
                                }
                                else
                                {
                                    dataList.Add(double.MinValue);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("StatsForm.printDataBytimeOnGraph(string) : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
        }


        /// <summary>
        ///  X 축에 해당하는 시간의 데이터를 가져온다.
        /// </summary>
        /// <param name="strDatetime">해당 시간</param>
        /// <returns>데이터 반환</returns>
        private double getDataByTime(string strDatetime)
        {
            for (int i = 0; i < xAxisList.Count; i++)
            {
                sBuilder = null;
                dTable = null;

                sBuilder = new StringBuilder(100);
                sBuilder.AppendFormat(" SELECT  accum10min, chktime FROM rainfall WHERE fkdevice = {0} "
                                                   , wDeviceList[cbxDeviceName.SelectedIndex].PKID.ToString());
                dTable = odec.getDataTable(sBuilder.ToString(), "rainfall");
            }
            return 0.0;
        }


        /// <summary>
        ///  X 축 시간 리스트 생성
        /// </summary>
        /// <param name="numberToInt">시간 단위(2,6,12,...)</param>
        private void makeXAxisList2(int numberToInt)
        {
            //시작 시간: 0,10,20,30,40,50 분 부터 10분 단위로 number (선택시간) 만큼 리스트 만든다.
            double numberDouble = Convert.ToDouble(numberToInt);
            DateTime tmpDate = new DateTime();
            tmpDate = dtpDate.Value.AddHours(-numberDouble);//해당 시간 전으로 초기값 세팅
            int minTime = Convert.ToInt32(tmpDate.ToString("HH:mm").Substring(3, 2)); //분만 얻어오기

            if (minTime >= 0 && minTime < 10)//0
            {
                tmpDate = Convert.ToDateTime(tmpDate.ToString("yyyy-MM-dd HH:00:00"));
            }
            else if (minTime >= 10 && minTime < 20)
            {
                tmpDate = Convert.ToDateTime(tmpDate.ToString("yyyy-MM-dd HH:10:00"));
            }
            else if (minTime >= 20 && minTime < 30)
            {
                tmpDate = Convert.ToDateTime(tmpDate.ToString("yyyy-MM-dd HH:20:00"));
            }
            else if (minTime >= 30 && minTime < 40)
            {
                tmpDate = Convert.ToDateTime(tmpDate.ToString("yyyy-MM-dd HH:30:00"));
            }
            else if (minTime >= 40 && minTime < 50)
            {
                tmpDate = Convert.ToDateTime(tmpDate.ToString("yyyy-MM-dd HH:40:00"));
            }
            else if (minTime >= 50 && minTime <= 59)
            {
                tmpDate = Convert.ToDateTime(tmpDate.ToString("yyyy-MM-dd HH:50:00"));
            }

            //리스트 초기화
            xAxisList.Clear();

            //  시간* 1시간당 개수(10분에 1개, So 6개)
            int numberToIntMultiple = numberToInt * 6;

            //시간 단위 :  2시간일때
            if (numberToInt.Equals(2))
            {
                for (int n = 0; n <= numberToIntMultiple; n++)
                {
                    if (n == 0 || tmpDate.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5).Equals("00:00"))
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("yy.MM.dd HH:mm")));
                    }
                    else
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    }
                    tmpDate = tmpDate.AddMinutes(10.0);
                }
            }
            //시간 단위 :  6시간일때
            else if (numberToInt.Equals(6))
            {
                //for (int n = 0; n <= 12; n++)
                for (int n = 0; n <= numberToIntMultiple; n++)
                {
                    if (n == 0 || tmpDate.ToString().Substring(11, 5).Equals("00:00"))
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("yyyy.MM.dd HH:mm")));
                    }
                    else
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    }
                    tmpDate = tmpDate.AddMinutes(10.0);

                    //else if (n % 6 == 0)
                    //{
                    //    xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    //}
                    //else
                    //{
                    //    //xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), ""));
                    //    xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    //}
                    // tmpDate = tmpDate.AddMinutes(10.0);
                    //tmpDate = tmpDate.AddMinutes(30.0);
                }

            }
            //시간 단위 :  12시간일때
            else if (numberToInt.Equals(12))
            {
                for (int n = 0; n <= numberToIntMultiple; n++)
                {
                    if (n == 0 || tmpDate.ToString().Substring(11, 5).Equals("00:00"))
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("yyyy.MM.dd HH:mm")));
                    }
                    else
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    }
                    tmpDate = tmpDate.AddMinutes(10.0);

                    //else if (n % 12 == 0)
                    //{
                    //    xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    //}
                    //else
                    //{
                    //    xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), ""));
                    //}
                    //tmpDate = tmpDate.AddMinutes(10.0);
                }
            }
            //시간 단위 :  24시간일때
            else if (numberToInt.Equals(24))
            {
                for (int n = 0; n <= numberToIntMultiple; n++)
                {
                    if (n == 0 || tmpDate.ToString().Substring(11, 5).Equals("00:00"))
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("yyyy.MM.dd HH:mm")));
                    }
                    else
                    {
                        xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    }
                    tmpDate = tmpDate.AddMinutes(10.0);
                    //else if (n % 24 == 0)
                    //{
                    //    xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), tmpDate.ToString("HH:mm")));
                    //}
                    //else
                    //{
                    //    xAxisList.Add(new XAxis(tmpDate.ToString("yyyy-MM-dd HH:mm:ss"), ""));
                    //}
                    //tmpDate = tmpDate.AddMinutes(10.0);
                }
            }
        }


        /// <summary>
        ///  임계치 알람 선 그리기
        /// </summary>
        ///<param name="strTypeWeather">관측 종류(강우, 수위, 유속)</param>
        private void drawAlarmLine(string strTypeWeather)
        {
            //그래프의 알람라인
            alarmLevelList = null;
            alarmLevelList = new List<double>(3); //1,2,3 차 임계치를 담기위함
            CustomGridLine alarmLine1 = null; // 알람라인 ( 그리드 라인)
            CustomGridLine alarmLine2 = null;
            CustomGridLine alarmLine3 = null;

            //그래프 선 초기화
            chtWeather2.AxisY.CustomGridLines.Clear();

            switch (strTypeWeather)
            {
                case "강우":
                    alarmLevelList.Add(getAlarmValue("7"));//강수량 1차 임계치
                    alarmLevelList.Add(getAlarmValue("8"));//강수량 2차 임계치
                    alarmLevelList.Add(getAlarmValue("9")); //강수량 3차 임계치
                    break;
                case "수위":
                    alarmLevelList.Add(getAlarmValue("10"));//수위 1차 임계치
                    alarmLevelList.Add(getAlarmValue("11"));//수위 2차 임계치
                    alarmLevelList.Add(getAlarmValue("12")); //수위 3차 임계치
                    break;
                case "유속":
                    alarmLevelList.Add(getAlarmValue("13"));//유속 1차 임계치
                    alarmLevelList.Add(getAlarmValue("14"));//유속 2차 임계치
                    alarmLevelList.Add(getAlarmValue("15")); //유속 3차 임계치

                    break;
            }


            //경계
            alarmLine1 = new CustomGridLine();
            alarmLine1.Value = alarmLevelList[0]; //기상정보(강우,...) 1차 임계치
            alarmLine1.Color = Color.Orange;
            alarmLine1.Text = "주의";
            alarmLine1.TextColor = Color.Orange;
            chtWeather2.AxisY.CustomGridLines.Add(alarmLine1);

            //위험
            alarmLine2 = new CustomGridLine();
            alarmLine2.Value = alarmLevelList[1];//기상정보(강우,...) 2차 임계치
            alarmLine2.Color = Color.Violet;
            alarmLine2.Text = "경계";
            alarmLine2.TextColor = Color.OrangeRed;
            chtWeather2.AxisY.CustomGridLines.Add(alarmLine2);

            //대피
            alarmLine3 = new CustomGridLine();
            alarmLine3.Value = alarmLevelList[2];//기상정보(강우,...) 3차 임계치
            alarmLine3.Color = Color.Red;
            alarmLine3.Text = "대피";
            alarmLine3.TextColor = Color.Red;
            chtWeather2.AxisY.CustomGridLines.Add(alarmLine3);
        }

        /// <summary>
        /// 알람 값 얻어오기
        /// </summary>
        /// <param name="level">얻어올 항목 값(레벨 및 기상정보별) eg: 강우 1차 임계치 </param>
        /// <returns>알람 레벨값 반환</returns>
        private double getAlarmValue(string level)
        {
            double levelValue = double.MinValue;
            try
            {
                if (odec.openDb())
                {
                    sBuilder = null;
                    dTable = null;
                    // number = -number; //마이너스로 부호 바꿔주기

                    //DateTime pastDate = Convert.ToDateTime(dtpDate.Value.ToString("yyyy-MM-dd HH:mm:00"));
                    //pastDate = pastDate.AddHours(number);

                    sBuilder = new StringBuilder(300);
                    sBuilder.Append(" SELECT dr.resvalue FROM deviceresponse dr ");
                    sBuilder.AppendFormat(" JOIN device d ON d.pkid = dr.fkdevice WHERE d.pkid = {0} AND dr.fkdeviceitem = {1}"
                                                            , wDeviceList[cbxDeviceName.SelectedIndex].PKID, level);
                    sBuilder.Append(" AND dr.chktime = ( SELECT max(chktime)  FROM deviceresponse ");
                    sBuilder.AppendFormat(" WHERE fkdevice = {0} AND fkdeviceitem = {1} "
                                                        , wDeviceList[cbxDeviceName.SelectedIndex].PKID, level);
                    sBuilder.Append("  GROUP BY fkdevice ) ");

                    dTable = odec.getDataTable(sBuilder.ToString(), "deviceresponse");
                    if (dTable.Rows.Count != 0)
                    {
                        levelValue = Convert.ToDouble(dTable.Rows[0]["resvalue"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("StatsForm.getAlarmValue() : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
            return levelValue;
        }


        /// <summary>
        ///  출력 버튼 클릭 시, 
        /// </summary>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            ////조회기간
            //string strTerm = this.dtpDate.Value.ToString("yyyy-MM-dd  HH시 mm분");

            ////시간단위
            //string strUnit = this.cbxByTime.SelectedItem.ToString();

            ////측기명
            //string strDeviceName = wDeviceList[cbxDeviceName.SelectedIndex].Name;

            ////관측종류
            //string strTypeWeather = cbxTypeWeather.SelectedItem.ToString();

            //레포트 생성 및 View
            fPrint viewForm = null;

            WeatherStatsReport weatherStatsReport = new WeatherStatsReport("[ 기상정보 통계 ]"
                                                                                               , strTerm, strUnit, strDeviceName, strTypeWeather
                                                                                               , dataList, this.xAxisList, timeList[cbxByTime.SelectedIndex].Number
                                                                                               , alarmLevelList);
            viewForm = new fPrint(weatherStatsReport, "기상정보 통계조회");

            //Form SHOW
            viewForm.Show();
        }

    }
}