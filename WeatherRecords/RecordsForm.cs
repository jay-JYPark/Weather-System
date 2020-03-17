using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using ADEng.Library; //DAC 사용을 위함
using ADEng.Library.WeatherSystem; // WeatheObject 사용을 위함
using DataDynamics.ActiveReports;//Report 사용을 위함
using ADEng.Control;//clsPView(레포트 출력 폼 포함) 사용을 위함
using WeatherRecords.Properties;




/// 주석 참고하기 ( 참고 후 삭제 )
/// <summary>
/// 측기 정보/  기상정보 / 알람단계  리스트를 가져온다.
/// </summary>
/// <param name="_sdt">시작 시간</param>
/// <param name="_edt">종료 시간</param>
/// <returns>어느 정도 범위에 발생하는 장비 제어 리스트를 반환</returns>
///

namespace ADEng.Module.WeatherSystem
{
    public partial class RecordsForm : Form
    {
        //컬럼 소팅을 위한 멤버
        private ListViewColumnSorter lvwColumnSorterWeather = new ListViewColumnSorter();
        private ListViewColumnSorter lvwColumnSorterAlarm = new ListViewColumnSorter();
        private ListViewColumnSorter lvwColumnSorterDevice = new ListViewColumnSorter();
        private ListViewColumnSorter lvwColumnSorterDeviceAlarm = new ListViewColumnSorter();

        oracleDAC odec = null;
        StringBuilder sBuilder = null;
        DataTable dTable = null;


        //관측 종류 리스트
        List<string> typeWeatherList = new List<string>();

        //측기 리스트
        List<WDevice> wDeviceList = new List<WDevice>();

        //관측 종류에 따른 측기 리스트
        List<string> selectedDeviceList = new List<string>();

        //조회가 일어난 데이터에 대해 출력을 수행한다. (이전 조건 저장)
        // 기상정보
        ////조회기간 From  ~ 조회기간 To
        StringBuilder sBuilderTerm = null;
        ////측기명
        string strDeviceName = string.Empty;
        ////관측종류
        string strTypeWeather = string.Empty;

        //임계치정보
        ////조회기간 From  ~ 조회기간 To
        StringBuilder sBuilderTermAlarm = null;
        ////측기명
        string strDeviceNameAlarm = string.Empty;
        ////관측종류
        string strTypeWeatherAlarm = string.Empty;

        //측기정보
        ////조회기간 From ~조회기간 To
        StringBuilder sBuilderTermDevice = null;
        ////측기명
        string strDeviceNameDevice = string.Empty;

        //측기 알람 정보
        ////조회기간 From ~조회기간 To
        StringBuilder sBuilderTermDeviceAlarm = null;
        ////측기명
        string strDeviceNameDeviceAlarm = string.Empty;


        //bool wIsSorted = false; //기상 정보 정렬

        private WeatherDataMng dataMng = null;

        /// <summary>
        ///기본 생성자
        /// </summary>
        public RecordsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 이력 조회 폼을 로드 할 때
        /// </summary>
        private void RecordsForm_Load(object sender, EventArgs e)
        {
            //리소스 참조
            string ip = Settings.Default.DbIp;
            string port = Settings.Default.DbPort;
            string id = Settings.Default.DbId;
            string pw = Settings.Default.DbPw;
            string sid = Settings.Default.DbSid;

            //오라클 Connection
            this.odec = new ADEng.Library.oracleDAC(id, pw, ip, port, sid);


            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onDBDataSetEvt += new EventHandler<SetDBDataEventArgs>(dataMng_onDBDataSetEvt);

            //컬럼 소팅을 위한 멤버 세팅
            this.lstWeather.ListViewItemSorter = lvwColumnSorterWeather;
            this.lstAlarm.ListViewItemSorter = lvwColumnSorterAlarm;
            this.lstDevice.ListViewItemSorter = lvwColumnSorterDevice;
            this.lstDeviceAlarm.ListViewItemSorter = lvwColumnSorterDeviceAlarm;

            //관측 종류 리스트 가져오기
            getTypeWeatherList();

            //측기 리스트 가져오기
            getDeviceList();

            //모든 리스트 뷰의 ROW  높이 변경 (Tip: Dummy Image 를 넣어 높이 조정)
            changeRowHeight();

            // 조회 조건의 Base 정보 로드
            loadBaseData();

            //모든 리스트뷰의 속성 Full Row Select
            setListViewFullRowSelect();
        }

        /// <summary>
        /// DB 설정 값 변경 시 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        ///모든 리스트뷰의 속성 Full Row Select
        /// </summary>
        private void setListViewFullRowSelect()
        {
            this.lstWeather.FullRowSelect = true;
            this.lstAlarm.FullRowSelect = true;
            this.lstDevice.FullRowSelect = true;
            this.lstDeviceAlarm.FullRowSelect = true;
        }


        /// <summary>
        ///관측 종류 리스트 가져오기
        /// </summary>
        private void getTypeWeatherList()
        {
            this.typeWeatherList.Add("강우");
            this.typeWeatherList.Add("수위");
            this.typeWeatherList.Add("유속");
        }


        /// <summary>
        ///측기 리스트 가져오기
        /// </summary>
        private void getDeviceList()
        {
            try
            {
                //전체 추가
                this.wDeviceList.Add(new WDevice(0, "0", "전체", "0", 0, 0, false, "0"));

                if (odec.openDb())
                {
                    //측기 리스트 출력하기
                    //측기명 추가
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
                                                                               , dRow["remark"].ToString()));
                    }
                    dTable = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("RecordsForm.getDeviceList() : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
        }


        /// <summary>
        /// 모든 리스트 뷰의 ROW  높이 변경
        /// </summary>
        private void changeRowHeight()
        {
            ImageList dummyImageList = new ImageList();
            dummyImageList.ImageSize = new System.Drawing.Size(1, 20);

            this.lstWeather.SmallImageList = dummyImageList; //기상정보 리스트에 적용.
            this.lstAlarm.SmallImageList = dummyImageList; // 임계치정보 리스트에 적용.
            this.lstDevice.SmallImageList = dummyImageList; // 측기정보 리스트에 적용.
            this.lstDeviceAlarm.SmallImageList = dummyImageList; // 알람정보 리스트에 적용.
        }


        /// <summary>
        /// 조회 조건의 Base 정보 로드
        /// </summary>
        private void loadBaseData()
        {
            //관측 종류 리스트 ComboBox  출력
            foreach (string item in typeWeatherList)
            {
                //기상 정보 Tab Page  의 관측 종류 리스트 
                this.cbxTypeWeather.Items.Add(item);
                // 알람 정보 Tab Page 의 측기리스트
                this.cbxTypeWeatherAlarm.Items.Add(item);
            }
            this.cbxTypeWeather.SelectedIndex = 0;
            this.cbxTypeWeatherAlarm.SelectedIndex = 0;

            //측기 리스트 Combobox 출력
            foreach (WDevice device in wDeviceList)
            {
                // 측기 상태 Tab Page 의 측기리스트
                this.cbxDeviceNameStatus.Items.Add(device.Name.Trim());
                //측기 알람 Tab Page 의 측기리스트
                this.cbxDeviceNameDeviceAlarm.Items.Add(device.Name.Trim());
            }
            //리스트의 첫 항목 선택하기
            this.cbxDeviceNameStatus.SelectedIndex = 0;
            this.cbxDeviceNameDeviceAlarm.SelectedIndex = 0;


            //조회기간 시작 시각 세팅 00시 00분 00초
            this.dtpFromWeather.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
            this.dtpFromAlarm.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
            this.dtpFromDevice.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
            this.dtpFromDeviceAlarm.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
        }


        /// <summary>
        /// 기상 정보 조회 버튼 클릭 시, 
        /// </summary>
        private void btnWeatherSearchClick(object sender, EventArgs e)
        {
            //0. 리스트뷰 헤더, 내용 클리어
            //lstWeather.Columns.Clear();
            //lstWeather.Items.Clear();
            lstWeather.Clear();

            // 기간 체크
            if (checkDate(dtpFromWeather, this.dtpToWeather) >= 0)
            {
                switch (cbxTypeWeather.SelectedItem.ToString())
                {
                    case "강우":
                        getSelectedDeviceListR();
                        searchRainfallInfo(); //조회
                        break;
                    case "수위":
                        getSelectedDeviceListW();
                        searchWaterLevelInfo();//조회
                        break;
                    case "유속":
                        getSelectedDeviceListF();
                        searchFlowSpeedInfo();//조회
                        break;
                    default:
                        break;
                }

                //조회조건 저장 (기상정보)
                setWeatherConstraint();
            }
            else
            {
                MessageBox.Show("시작시각이 끝시각보다 큽니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void getSelectedDeviceListF()
        {
            selectedDeviceList.Clear();
            this.selectedDeviceList.Add("0");
            foreach (WDevice wdevice in wDeviceList)
            {
                if (Convert.ToBoolean((wdevice.HaveSensor >> 2) & 1))
                {
                    //관측 종류에 따른 측기 리스트를 저장한다.
                    this.selectedDeviceList.Add(wdevice.PKID.ToString());
                }
            }
        }

        private void getSelectedDeviceListW()
        {
            selectedDeviceList.Clear();
            this.selectedDeviceList.Add("0");
            foreach (WDevice wdevice in wDeviceList)
            {
                if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                {
                    //관측 종류에 따른 측기 리스트를 저장한다.
                    this.selectedDeviceList.Add(wdevice.PKID.ToString());
                }
            }
        }

        private void getSelectedDeviceListR()
        {
            selectedDeviceList.Clear();
            this.selectedDeviceList.Add("0");
            foreach (WDevice wdevice in wDeviceList)
            {
                if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                {
                    //관측 종류에 따른 측기 리스트를 저장한다.
                    this.selectedDeviceList.Add(wdevice.PKID.ToString());
                }
            }
        }

        /// <summary>
        ///조회조건 저장 (기상정보)
        /// </summary>
        private void setWeatherConstraint()
        {
            //조회기간
            sBuilderTerm = new StringBuilder(100);
            sBuilderTerm.Append(dtpFromWeather.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            sBuilderTerm.Append(dtpToWeather.Value.ToString("yyyy-MM-dd  HH시 mm분"));

            //측기명
            strDeviceName = wDeviceList[cbxDeviceNameWeather.SelectedIndex].Name;

            //관측종류
            strTypeWeather = cbxTypeWeather.SelectedItem.ToString();
        }

        /// <summary>
        /// 기간 체크
        /// </summary>
        /// <param name="_sdt">시작 시간</param>
        /// <param name="_edt">종료 시간</param>
        /// <returns> 시작시각 크면: -1, 같으면: 0, 작으면: -1</returns>
        ///
        private int checkDate(DateTimePicker dateFrom, DateTimePicker dateTo)
        {

            DateTime dFrom = Convert.ToDateTime(dateFrom.Value.ToString("yyyy-MM-dd HH:mm:ss "));
            DateTime dTo = Convert.ToDateTime(dateTo.Value.ToString("yyyy-MM-dd HH:mm:ss "));
            int ret = 0;

            if (dFrom > dTo)
            {
                ret = -1;
            }
            else if (dFrom < dTo)
            {
                ret = 1;
            }
            else if (dFrom == dTo)
            {
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// 기상정보(우량) 정보 조회
        /// </summary>
        private void searchRainfallInfo()
        {
            //2. 데이터 조회
            try
            {
                if (odec.openDb())
                {
                    //강우데이터 쿼리 만들기
                    makeRainfallQuery();

                    dTable = odec.getDataTable(sBuilder.ToString(), "rainfall");
                    int totalCnt = dTable.Rows.Count;

                    if (totalCnt > 0)
                    {

                        //헤더 만들기
                        lstWeather.Columns.Add("번호", 40);
                        lstWeather.Columns.Add("관측시각", 180);
                        lstWeather.Columns.Add("측기명", 120);
                        lstWeather.Columns.Add("10분 누적", 150, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("이동 20분", 150, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("금일", 150, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("전일", 150, HorizontalAlignment.Right);

                        if (totalCnt > 2000)
                        {
                            MessageBox.Show("데이터의 최대 출력 개수 (2000개)를 초과하였습니다.\n 2000개 데이터까지 출력 됩니다. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            printWeatherLstView(2000);
                        }
                        else
                        {
                            printWeatherLstView(totalCnt);
                        }

                        //번호 재 출력
                        for (int i = 0; i < this.lstWeather.Items.Count; i++)
                        {
                            lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                        }
                    }
                    else
                    {
                        lstWeather.Columns.Add("결과", lstWeather.Size.Width - 10, HorizontalAlignment.Center);
                        this.lstWeather.Items.Add(new ListViewItem("데이터가 없습니다."));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("WeatherRecords.searchRainfallInfo() : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
        }

        private void printWeatherLstView(int totalCnt)
        {
            string chkTime = string.Empty;
            string devicename = string.Empty;
            string accum10min = string.Empty;
            string move20min = string.Empty;
            string today = string.Empty;
            string yesterday = string.Empty;
            // int i = 1;

            for (int i = 1; i <= totalCnt; i++)
            {
                if (dTable.Rows[i - 1]["chktime"] != null && dTable.Rows[i - 1]["chktime"].ToString() != "")
                {
                    chkTime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}", Convert.ToDateTime(dTable.Rows[i - 1]["chktime"].ToString()));
                }
                if (dTable.Rows[i - 1]["devicename"] != null && dTable.Rows[i - 1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i - 1]["devicename"].ToString();
                }
                if (dTable.Rows[i - 1]["accum10min"] != null && dTable.Rows[i - 1]["accum10min"].ToString() != "")
                {
                    accum10min = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["accum10min"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["move20min"] != null && dTable.Rows[i - 1]["move20min"].ToString() != "")
                {
                    move20min = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["move20min"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["today"] != null && dTable.Rows[i - 1]["today"].ToString() != "")
                {
                    today = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["today"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["yesterday"] != null && dTable.Rows[i - 1]["yesterday"].ToString() != "")
                {
                    yesterday = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["yesterday"].ToString())) * 0.1);
                }

                this.lstWeather.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , chkTime
                                                                                       , devicename
                                                                                       , accum10min
                                                                                       , move20min
                                                                                       , today
                                                                                       , yesterday
                                                                                            }
                                                                                    )
                                                        );
                // i++;
            }
        }


        /// <summary>
        ///강우데이터 쿼리 만들기
        /// </summary>
        private void makeRainfallQuery()
        {
            sBuilder = null;
            dTable = null;
            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);
            sBuilder = new StringBuilder(350);
            //우량 정보 쿼리 만들기
            if (cbxDeviceNameWeather.SelectedIndex.Equals(0))
            {

                sBuilder.Append(" SELECT  chktime, accum10min, move20min, today, yesterday, d.devicename ");
                sBuilder.Append(" FROM rainfall r  ");
                sBuilder.Append(" JOIN device d ON r.fkdevice = d.pkid  ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sBuilder.Append(" SELECT  chktime, accum10min, move20min, today, yesterday, d.devicename ");
                sBuilder.Append(" FROM rainfall r  ");
                sBuilder.Append(" JOIN device d ON r.fkdevice = d.pkid  ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND r.fkdevice = {0} ", selectedDeviceList[cbxDeviceNameWeather.SelectedIndex].ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            sBuilder.Append(" ORDER BY  chktime ");
        }


        /// <summary>
        /// 기상정보(수위) 정보 조회
        /// </summary>
        private void searchWaterLevelInfo()
        {
            //2. 데이터 조회
            try
            {
                if (odec.openDb())
                {

                    //수위 데이터 쿼리 만들기
                    makeWaterlevelQuery();

                    dTable = odec.getDataTable(sBuilder.ToString(), "waterlevel");
                    int totalCnt = dTable.Rows.Count;

                    if (totalCnt > 0)
                    {

                        //헤더만들기
                        lstWeather.Columns.Add("번호", 40);
                        lstWeather.Columns.Add("관측시각", 180);
                        lstWeather.Columns.Add("측기명", 120);
                        lstWeather.Columns.Add("고도", 0, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("현재 수위", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("15분 변화", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("60분 변화", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("금일 변화", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("금일 최고", 100, HorizontalAlignment.Right);

                        if (totalCnt > 2000)
                        {
                            MessageBox.Show("데이터의 최대 출력 개수 (2000개)를 초과하였습니다.\n 2000개 데이터까지 출력 됩니다. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            printWaterLevelLstView(2000);
                        }
                        else
                        {
                            printWaterLevelLstView(totalCnt);
                        }

                        //번호 재 출력
                        for (int i = 0; i < this.lstWeather.Items.Count; i++)
                        {
                            lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                        }

                    }
                    else
                    {
                        lstWeather.Columns.Add("결과", lstWeather.Size.Width - 10, HorizontalAlignment.Center);
                        this.lstWeather.Items.Add(new ListViewItem("데이터가 없습니다."));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("WeatherRecords.searchWaterLevelInfo() : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
        }

        private void printWaterLevelLstView(int totalCnt)
        {
            string chktime = string.Empty;
            string height = string.Empty;
            string devicename = string.Empty;
            string now = string.Empty;
            string change15min = string.Empty;
            string change60min = string.Empty;
            string changetoday = string.Empty;
            string maxtoday = string.Empty;
            //int i = 1;
            for (int i = 1; i <= totalCnt; i++)
            {
                if (dTable.Rows[i - 1]["chktime"] != null && dTable.Rows[i - 1]["chktime"].ToString() != "")
                {
                    chktime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}", Convert.ToDateTime(dTable.Rows[i - 1]["chktime"].ToString()));
                }
                if (dTable.Rows[i - 1]["devicename"] != null && dTable.Rows[i - 1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i - 1]["devicename"].ToString();
                }
                if (dTable.Rows[i - 1]["height"] != null && dTable.Rows[i - 1]["height"].ToString() != "")
                {
                    height = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["height"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["now"] != null && dTable.Rows[i - 1]["now"].ToString() != "")
                {
                    now = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["now"].ToString())) * 0.01);
                }
                if (dTable.Rows[i - 1]["change15min"] != null && dTable.Rows[i - 1]["change15min"].ToString() != "")
                {
                    change15min = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["change15min"].ToString())) * 0.01);
                }
                if (dTable.Rows[i - 1]["change60min"] != null && dTable.Rows[i - 1]["change60min"].ToString() != "")
                {
                    change60min = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["change60min"].ToString())) * 0.01);
                }
                if (dTable.Rows[i - 1]["changetoday"] != null && dTable.Rows[i - 1]["changetoday"].ToString() != "")
                {
                    changetoday = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["changetoday"].ToString())) * 0.01);
                }
                if (dTable.Rows[i - 1]["maxtoday"] != null && dTable.Rows[i - 1]["maxtoday"].ToString() != "")
                {
                    maxtoday = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["maxtoday"].ToString())) * 0.01);
                }

                this.lstWeather.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , chktime
                                                                                       , devicename
                                                                                       , height
                                                                                       , now
                                                                                       , change15min
                                                                                       , change60min
                                                                                       , changetoday
                                                                                       , maxtoday}));
                //i++;
            }
        }

        /// <summary>
        ///수위 데이터 쿼리 만들기
        /// </summary>
        private void makeWaterlevelQuery()
        {
            sBuilder = null;
            dTable = null;

            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            sBuilder = new StringBuilder(350);
            //수위 정보 쿼리 만들기
            if (cbxDeviceNameWeather.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT  chktime, height, now, change15min, change60min, changetoday, maxtoday, d.devicename ");
                sBuilder.Append(" FROM waterlevel  ");
                sBuilder.Append(" JOIN device d ON fkdevice = d.pkid  ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sBuilder.Append(" SELECT  chktime, height, now, change15min, change60min, changetoday, maxtoday,  d.devicename ");
                sBuilder.Append(" FROM waterlevel  ");
                sBuilder.Append(" JOIN device d ON fkdevice = d.pkid  ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND fkdevice = {0} ", selectedDeviceList[cbxDeviceNameWeather.SelectedIndex].ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            sBuilder.Append("ORDER BY chktime ");
        }

        /// <summary>
        /// 기상정보(유속) 정보 조회
        /// </summary>
        private void searchFlowSpeedInfo()
        {

            //2. 데이터 조회
            try
            {
                if (odec.openDb())
                {
                    //유속 데이터 쿼리 만들기
                    makeFlowspeedQuery();

                    dTable = odec.getDataTable(sBuilder.ToString(), "flowspeed");
                    int totalCnt = dTable.Rows.Count;

                    if (totalCnt > 0)
                    {
                        //헤더 만들기
                        lstWeather.Columns.Add("번호", 40);
                        lstWeather.Columns.Add("관측시각", 180);
                        lstWeather.Columns.Add("측기명", 120);
                        lstWeather.Columns.Add("현재 유속(m/s)", 120, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("15분 변화(m/s)", 120, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("60분 변화(m/s)", 120, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("금일 변화(m/s)", 120, HorizontalAlignment.Right);

                        if (totalCnt > 2000)
                        {
                            MessageBox.Show("데이터의 최대 출력 개수 (2000개)를 초과하였습니다.\n 2000개 데이터까지 출력 됩니다. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            printFlowSpeedLstView(2000);
                        }
                        else
                        {
                            printFlowSpeedLstView(totalCnt);
                        }
                        //번호 재 출력
                        for (int i = 0; i < this.lstWeather.Items.Count; i++)
                        {
                            lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                        }
                    }
                    else
                    {
                        lstWeather.Columns.Add("결과", lstWeather.Size.Width - 10, HorizontalAlignment.Center);
                        this.lstWeather.Items.Add(new ListViewItem("데이터가 없습니다."));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("WeatherRecords.searchFlowSpeedInfo() : {0}", ex.Message));
            }
            finally
            {
                odec.closeDb();
            }
        }

        private void printFlowSpeedLstView(int totalCnt)
        {
            lstWeather.Columns.Add("금일 최고(m/s)", 120, HorizontalAlignment.Right);

            string chktime = string.Empty;
            string devicename = string.Empty;
            string now = string.Empty;
            string change15min = string.Empty;
            string change60min = string.Empty;
            string changetoday = string.Empty;
            string maxtoday = string.Empty;
            //int i = 1;
            for (int i = 1; i <= totalCnt; i++)
            {
                if (dTable.Rows[i - 1]["chktime"] != null && dTable.Rows[i - 1]["chktime"].ToString() != "")
                {
                    chktime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}", Convert.ToDateTime(dTable.Rows[i - 1]["chktime"].ToString()));
                }
                if (dTable.Rows[i - 1]["devicename"] != null && dTable.Rows[i - 1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i - 1]["devicename"].ToString();
                }
                if (dTable.Rows[i - 1]["now"] != null && dTable.Rows[i - 1]["now"].ToString() != "")
                {
                    now = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["now"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["change15min"] != null && dTable.Rows[i - 1]["change15min"].ToString() != "")
                {
                    change15min = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["change15min"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["change60min"] != null && dTable.Rows[i - 1]["change60min"].ToString() != "")
                {
                    change60min = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["change60min"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["changetoday"] != null && dTable.Rows[i - 1]["changetoday"].ToString() != "")
                {
                    changetoday = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["changetoday"].ToString())) * 0.1);
                }
                if (dTable.Rows[i - 1]["maxtoday"] != null && dTable.Rows[i - 1]["maxtoday"].ToString() != "")
                {
                    maxtoday = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["maxtoday"].ToString())) * 0.1);
                }

                this.lstWeather.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , chktime
                                                                                       , devicename
                                                                                       , now
                                                                                       , change15min
                                                                                       , change60min
                                                                                       , changetoday
                                                                                       , maxtoday}));
                //i++;
            }
        }

        /// <summary>
        ///유속 데이터 쿼리 만들기
        /// </summary>
        private void makeFlowspeedQuery()
        {
            sBuilder = null;
            dTable = null;

            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //유속 정보 쿼리 만들기
            sBuilder = new StringBuilder(350);
            if (cbxDeviceNameWeather.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT  chktime, now, change15min, change60min, changetoday, maxtoday , d.devicename ");
                sBuilder.Append(" FROM flowspeed f  ");
                sBuilder.Append(" JOIN device d ON f.fkdevice = d.pkid  ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sBuilder.Append(" SELECT  chktime, now, change15min, change60min, changetoday, maxtoday , d.devicename ");
                sBuilder.Append(" FROM flowspeed f ");
                sBuilder.Append(" JOIN device d ON f.fkdevice = d.pkid  ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat("AND fkdevice = {0} ", selectedDeviceList[cbxDeviceNameWeather.SelectedIndex].ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            sBuilder.Append("ORDER BY chktime");
        }


        /// <summary>
        /// 알람 정보 조회 버튼 클릭 시, 
        /// </summary>
        private void btnSearchAlarm_Click(object sender, EventArgs e)
        {
            lstAlarm.Items.Clear();
            lstAlarm.Columns.Clear();

            dTable = null;

            if (checkDate(dtpFromAlarm, this.dtpToAlarm) >= 0)
            {
                selectedDeviceList.Clear();
                switch (this.cbxTypeWeatherAlarm.SelectedItem.ToString())
                {
                    case "강우":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                            {
                                this.selectedDeviceList.Add(wdevice.PKID.ToString());
                            }
                        }
                        break;
                    case "수위":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                            {
                                this.selectedDeviceList.Add(wdevice.PKID.ToString());
                            }
                        }
                        break;
                    case "유속":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            if (Convert.ToBoolean((wdevice.HaveSensor >> 2) & 1))
                            {
                                this.selectedDeviceList.Add(wdevice.PKID.ToString());
                            }
                        }
                        break;
                    case "전체":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            this.selectedDeviceList.Add(wdevice.PKID.ToString());
                        }
                        break;
                }


                try
                {
                    if (odec.openDb())
                    {
                        // 알람 쿼리 만들기
                        makeAlarmQuery();

                        dTable = odec.getDataTable(sBuilder.ToString(), "alarmInfo");
                        int totalCnt = dTable.Rows.Count;

                        if (totalCnt > 0)
                        {
                            lstAlarm.Columns.Add("번호", 40);
                            lstAlarm.Columns.Add("임계치발생시각", 180);
                            lstAlarm.Columns.Add("측기명", 120);
                            lstAlarm.Columns.Add("임계치레벨", 150, HorizontalAlignment.Center);
                            lstAlarm.Columns.Add("센서종류", 150, HorizontalAlignment.Center);
                            switch (cbxTypeWeatherAlarm.SelectedItem.ToString())
                            {
                                case "강우":
                                    lstAlarm.Columns.Add("관측값(mm)", 150, HorizontalAlignment.Center);
                                    break;
                                case "수위":
                                    lstAlarm.Columns.Add("관측값(m)", 150, HorizontalAlignment.Center);
                                    break;
                                case "유속":
                                    lstAlarm.Columns.Add("관측값(m/s)", 150, HorizontalAlignment.Center);
                                    break;
                            }
                            lstAlarm.Columns.Add("구분", 150, HorizontalAlignment.Center);

                            if (totalCnt > 2000)
                            {
                                MessageBox.Show("데이터의 최대 출력 개수 (2000개)를 초과하였습니다.\n 2000개 데이터까지 출력 됩니다. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                printAlarmLstView(2000);
                            }
                            else
                            {
                                printAlarmLstView(totalCnt);
                            }

                            //번호 재 출력
                            for (int i = 0; i < this.lstAlarm.Items.Count; i++)
                            {
                                lstAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                            }
                        }
                        else
                        {
                            lstAlarm.Columns.Add("결과", lstAlarm.Size.Width - 10, HorizontalAlignment.Center);
                            this.lstAlarm.Items.Add(new ListViewItem("데이터가 없습니다."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("WeatherRecords.btnSearchAlarm_Click() : {0}", ex.Message));
                }
                finally
                {
                    odec.closeDb();
                }
                //조회조건 저장 (알람정보)
                setAlarmConstraint();

            }
            else
            {
                MessageBox.Show("시작시각이 끝시각보다 큽니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printAlarmLstView(int totalCnt)
        {
            //int i = 1;
            string alarmvalue = string.Empty;
            string alarmTime = string.Empty;
            string devicename = string.Empty;
            for (int i = 1; i <= totalCnt; i++)
            {
                if (dTable.Rows[i - 1]["alarmtime"] != null && dTable.Rows[i - 1]["alarmtime"].ToString() != "")
                {
                    alarmTime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}", Convert.ToDateTime(dTable.Rows[i - 1]["alarmtime"].ToString()));
                }
                if (dTable.Rows[i - 1]["devicename"] != null && dTable.Rows[i - 1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i - 1]["devicename"].ToString();
                }
                if (dTable.Rows[i - 1]["alarmvalue"] != null && dTable.Rows[i - 1]["alarmvalue"].ToString() != "")
                {
                    switch (cbxTypeWeatherAlarm.SelectedItem.ToString())
                    {
                        case "강우":
                            alarmvalue = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["alarmvalue"].ToString())) * 0.1);
                            break;
                        case "수위":
                            alarmvalue = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["alarmvalue"].ToString())) * 0.01);
                            break;
                        case "유속":
                            alarmvalue = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["alarmvalue"].ToString())) * 0.1);
                            break;
                    }
                }

                this.lstAlarm.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , alarmTime
                                                                                       , devicename
                                                                                       , dTable.Rows[i-1]["alarmtype"].ToString()
                                                                                       , dTable.Rows[i-1]["sensortype"].ToString()
                                                                                       , alarmvalue
                                                                                       , dTable.Rows[i-1]["realmode"].ToString()})
                                                                                   );
                switch (dTable.Rows[i - 1]["alarmtype"].ToString())
                {
                    case "주의":
                        lstAlarm.Items[i - 1].SubItems[4].ForeColor = Color.Orange;
                        break;
                    case "경계":
                        lstAlarm.Items[i - 1].SubItems[4].ForeColor = Color.Violet;
                        break;
                    case "대피":
                        lstAlarm.Items[i - 1].SubItems[4].ForeColor = Color.Red;
                        break;
                }

                // i++;
            }
        }

        /// <summary>
        ///조회조건 저장 (알람정보)
        /// </summary>
        private void setAlarmConstraint()
        {
            //조회기간
            sBuilderTermAlarm = new StringBuilder(100);
            sBuilderTermAlarm.Append(dtpFromAlarm.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            sBuilderTermAlarm.Append(dtpToAlarm.Value.ToString("yyyy-MM-dd  HH시 mm분"));

            //측기명
            strDeviceNameAlarm = wDeviceList[cbxDeviceNameAlarm.SelectedIndex].Name;

            //관측종류
            strTypeWeatherAlarm = cbxTypeWeatherAlarm.SelectedItem.ToString();
        }

        /// <summary>
        /// 알람 쿼리 만들기
        /// </summary>
        private void makeAlarmQuery()
        {
            sBuilder = null;
            dTable = null;

            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);


            //측기 리스트 출력하기
            sBuilder = new StringBuilder(1000);

            if (!cbxDeviceNameAlarm.SelectedItem.ToString().Equals("전체"))
            {
                sBuilder.Append(" SELECT  alarmtime, t.typename alarmtype, s.typename sensortype, alarmvalue, fktypesensor ");
                sBuilder.Append(" , DECODE (realmode, 0,'실제', 1,'시험') realmode , d.devicename ");
                sBuilder.Append("  FROM alarm al ");
                sBuilder.Append(" JOIN typealarmlevel t ON t.pkid = al.fktypealarmlevel ");
                sBuilder.Append(" JOIN typesensor s ON s.pkid = al.fktypesensor ");
                sBuilder.Append(" JOIN device d ON d.pkid = al.fkdevice ");
                sBuilder.AppendFormat(" WHERE d.isuse = 1 ");
                sBuilder.AppendFormat(" AND al.fkdevice = {0} "
                                                        , selectedDeviceList[cbxDeviceNameAlarm.SelectedIndex].ToString());
            }

            else if (cbxDeviceNameAlarm.SelectedItem.ToString().Equals("전체"))
            {
                sBuilder.Append(" SELECT  alarmtime, t.typename alarmtype, s.typename sensortype, alarmvalue, fktypesensor ");
                sBuilder.Append(" , DECODE (realmode, 0,'실제', 1,'시험') realmode ,  d.devicename ");
                sBuilder.Append("  FROM alarm al ");
                sBuilder.Append(" JOIN typealarmlevel t ON t.pkid = al.fktypealarmlevel ");
                sBuilder.Append(" JOIN typesensor s ON s.pkid = al.fktypesensor ");
                sBuilder.Append(" JOIN device d ON d.pkid = al.fkdevice ");
                sBuilder.AppendFormat(" WHERE d.isuse = 1 ");
            }

            switch (cbxTypeWeatherAlarm.SelectedItem.ToString())
            {
                case "강우":
                    sBuilder.AppendFormat(" AND fktypesensor = {0} ", "1");
                    break;
                case "수위":
                    sBuilder.AppendFormat(" AND fktypesensor = {0} ", "2");
                    break;
                case "유속":
                    sBuilder.AppendFormat(" AND fktypesensor = {0} ", "3");
                    break;
            }

            sBuilder.AppendFormat(" AND al.alarmtime >=to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , dtpFromAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
            sBuilder.AppendFormat(" AND al.alarmtime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                  , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            sBuilder.Append(" ORDER BY al.alarmtime ");
        }


        /// <summary>
        /// 측기상태 조회버튼 클릭 시,
        /// </summary>
        private void btnSearchDevice_Click(object sender, EventArgs e)
        {
            //0. 리스트뷰 헤더, 내용 클리어
            lstDevice.Columns.Clear();
            lstDevice.Items.Clear();

            // 기간 체크
            if (checkDate(dtpFromDevice, dtpToDevice) >= 0)
            {
                try
                {
                    if (odec.openDb())
                    {
                        // 이벤트 조회 쿼리 만들기 (요청 DataTable)
                        //makeEventQuery();
                        makeEventQuery2();

                        dTable = odec.getDataTable(sBuilder.ToString(), "devicerequestANDrequest");
                        int totalCnt = dTable.Rows.Count;

                        #region 통합 리스트 만드는 쿼리
                        // 이벤트 조회 쿼리 만들기 (응답 DataTable)
                        //DataTable dTable2 = new DataTable();

                        //makeEventResponseQuery();

                        // dTable2 = odec.getDataTable(sBuilder.ToString(), "deviceResponse");

                        //리스트로 통합
                        ///List<EventDataType> eventList = dTablesToList(dTable, dTable2);
                        //eventList.Sort(delegate(EventDataType e1, EventDataType e2)
                        // {
                        //   if (DateTime.Parse(e1.CheckTime) < DateTime.Parse(e2.CheckTime))
                        //   {
                        //       return -1;
                        //   }
                        //  else if (DateTime.Parse(e1.CheckTime) > DateTime.Parse(e2.CheckTime))
                        //   {
                        //        return 1;
                        //    }
                        //   return 0;//e1.CheckTime.CompareTo(e2.CheckTime);
                        // }); 
                        #endregion
                        #region 통합리스트를 사용하였을때의 소스
                        //if (eventList.Count > 0)
                        //{
                        //    lstDevice.Columns.Add("번호", 40);
                        //    lstDevice.Columns.Add("체크시각", 180);
                        //    lstDevice.Columns.Add("측기명", 120);
                        //    lstDevice.Columns.Add("구분", 150, HorizontalAlignment.Center);
                        //    lstDevice.Columns.Add("상태항목", 150);
                        //    lstDevice.Columns.Add("상태값", 100);

                        //    int i = 1;
                        //    string value = string.Empty; //상태값
                        //    string chktime = string.Empty; //시각
                        //    string devicename = string.Empty;//측기명

                        //    foreach (EventDataType item in eventList)
                        //    {
                        //        value = string.Empty;
                        //        if (item.CheckTime != null && item.CheckTime != "")
                        //        {
                        //            chktime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}"
                        //                                                    , Convert.ToDateTime(item.CheckTime));
                        //        }
                        //        if (item.DeviceName != null && item.DeviceName != "")
                        //        {
                        //            devicename = item.DeviceName;
                        //        }
                        //        if (item.ChkValue != null && item.ChkValue != "")
                        //        {
                        //            value = item.ChkValue;
                        //            #region SWITCH
                        //            switch (item.ItemId.Trim())
                        //            {
                        //                // "배터리 상태":
                        //                //0:정상, 1:이상
                        //                case "1":
                        //                    value = item.ChkValue.Equals("0") ? "정상" : item.ChkValue.Equals("1") ? "이상" : "";
                        //                    break;
                        //                //"태양전지 상태":
                        //                //0: 정상, 1: 이상
                        //                case "2":
                        //                    value = item.ChkValue.Equals("0") ? "정상" : item.ChkValue.Equals("1") ? "이상" : "";
                        //                    break;
                        //                //"시간"
                        //                //빼기(주석으로)
                        //                //case "3":
                        //                //    value = String.Format("{0}시{1}분", dRow["chkvalue"].ToString().Substring(0, 2), dRow["chkvalue"].ToString().Substring(2, 2));
                        //                //    break;
                        //                //"FAN 상태":
                        //                //0: 이상 발생
                        //                //1: 정상(ON)
                        //                //2: 정상(OFF)
                        //                case "4":
                        //                    value = item.ChkValue.Equals("0") ? "이상 발생"
                        //                                : item.ChkValue.Equals("1") ? "정상(ON)"
                        //                                : item.ChkValue.Equals("2") ? "정상(OFF)"
                        //                                : "";
                        //                    break;
                        //                //"도어 상태":
                        //                //0: 문 닫힘
                        //                //1: 문 열림
                        //                case "5":
                        //                    value = item.ChkValue.Equals("0") ? "문 닫힘"
                        //                                : item.ChkValue.Equals("1") ? "문 열림"
                        //                                : "";
                        //                    break;
                        //                //"안테나 감도":
                        //                //데이터임 dbm
                        //                case "6":
                        //                    value = String.Format("{0} dbm", item.ChkValue);
                        //                    break;
                        //                //강수량 임계치 1차
                        //                case "7":
                        //                    value = String.Format("{0:0.0} mm", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //강수량 임계치 2차
                        //                case "8":
                        //                    value = String.Format("{0} mm", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //강수량 임계치 3차
                        //                case "9":
                        //                    value = String.Format("{0} mm", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //수위계 임계치 1차
                        //                case "10":
                        //                    value = String.Format("{0:0.00} m", (double.Parse(item.ChkValue)) * 0.01);
                        //                    break;
                        //                //수위계 임계치 2차
                        //                case "11":
                        //                    value = String.Format("{0:0.00} m", (double.Parse(item.ChkValue)) * 0.01);
                        //                    break;
                        //                //수위계 임계치 3차
                        //                case "12":
                        //                    value = String.Format("{0:0.00} m", (double.Parse(item.ChkValue)) * 0.01);
                        //                    break;
                        //                //유속계 임계치 1차
                        //                case "13":
                        //                    value = String.Format("{0:0.0} m/s", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //유속계 임계치 2차
                        //                case "14":
                        //                    value = String.Format("{0:0.0} m/s", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //유속계 임계치 3차
                        //                case "15":
                        //                    value = String.Format("{0:0.0} m/s", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //배터리 임계치 1차(상한)
                        //                //빼기(주석으로)
                        //                //case "16":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //배터리 임계치 2차(하한)
                        //                //빼기(주석으로)
                        //                //case "17":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //태양전지 1차(상한)
                        //                //빼기(주석으로)
                        //                //case "18":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //태양전지 2차(하한)
                        //                //빼기(주석으로)
                        //                //case "19":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //동일레벨 무시시간
                        //                case "20":
                        //                    value = String.Format("{0} 분", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //하향레벨 무시시간
                        //                case "21":
                        //                    value = String.Format("{0} 분", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //관측모드
                        //                //빼기(주석으로)
                        //                //case "22":
                        //                //    value =  item.ChkValue.Equals("0") ? "일반모드" :  item.ChkValue.Equals("1") ? "인공지능모드" : "";
                        //                //    break;
                        //                //강수 센서 상태
                        //                //0: 정상
                        //                //1: 이상
                        //                //2: 합선
                        //                //3: 단선
                        //                case "23":
                        //                    value = item.ChkValue.Equals("0") ? "정상"
                        //                                : item.ChkValue.Equals("1") ? "이상"
                        //                                : item.ChkValue.Equals("2") ? "합선"
                        //                                : item.ChkValue.Equals("3") ? "단선"
                        //                                : "";
                        //                    break;
                        //                //수위 센서 상태
                        //                //0: 정상
                        //                //1: 이상
                        //                case "24":
                        //                    value = item.ChkValue.Equals("0") ? "정상"
                        //                                : item.ChkValue.Equals("1") ? "이상"
                        //                                : "";
                        //                    break;
                        //                //유속 센서 상태
                        //                //0: 정상
                        //                //1: 이상
                        //                case "25":
                        //                    value = item.ChkValue.Equals("0") ? "정상"
                        //                                : item.ChkValue.Equals("1") ? "이상"
                        //                                : "";
                        //                    break;
                        //                //F/W 버전
                        //                //그대로 넣기
                        //                case "26":
                        //                    value = item.ChkValue;
                        //                    break;
                        //                //강수 센서 사용여부
                        //                //빼기 (주석으로)
                        //                //case "27":
                        //                //    value =  item.ChkValue.Equals("0") ? "사용" :  item.ChkValue.Equals("1") ? "미사용" : "";
                        //                //    break;
                        //                //수위 센서 사용여부
                        //                //빼기 (주석으로)
                        //                //case "28":
                        //                //    value =  item.ChkValue.Equals("0") ? "사용" :  item.ChkValue.Equals("1") ? "미사용" : "";
                        //                //    break;
                        //                //유속 센서 사용여부
                        //                //빼기 (주석으로)
                        //                //case "29":
                        //                //    value =  item.ChkValue.Equals("0") ? "사용" :  item.ChkValue.Equals("1") ? "미사용" : "";
                        //                //    break;
                        //                //통신상태
                        //                //빼기 (주석으로)
                        //                //case "30":
                        //                //    value =  item.ChkValue.Equals("0") ? "프로토콜 응답 없음" :  item.ChkValue.Equals("1") ? "TCP/IP 응답 " : "SMS 응답";
                        //                //    break;
                        //                //A/S 작업 보고
                        //                //맞음
                        //                case "31":
                        //                    value = item.ChkValue.Equals("0") ? "A/S 작업 종료"
                        //                                : item.ChkValue.Equals("1") ? "A/S 작업 시작"
                        //                                : "";
                        //                    break;
                        //                //RAT 전체 상태
                        //                //값없음
                        //                case "34":
                        //                    value = string.Empty;
                        //                    break;
                        //                //강수 센서 시험 요청
                        //                //값+ "단계 임계치" 
                        //                case "38":
                        //                    value = String.Format("{0} 단계 임계치", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //수위 센서 시험 요청
                        //                //값+ "단계 임계치" 
                        //                case "39":
                        //                    value = String.Format("{0} 단계 임계치", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //유속 센서 시험 요청
                        //                //값 + "단계 임계치" 
                        //                case "40":
                        //                    value = String.Format("{0} 단계 임계치", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //강수 임계치 요청
                        //                //값 + "단계 임계치"
                        //                case "41":
                        //                    value = String.Format("{0} 단계 임계치", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //수위 임계치 요청
                        //                //값 + "단계 임계치"
                        //                case "42":
                        //                    value = String.Format("{0} 단계 임계치", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //유속 임계치 요청
                        //                //값 + "단계 임계치"
                        //                case "43":
                        //                    value = String.Format("{0} 단계 임계치", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //무시시간요청
                        //                //값없음
                        //                case "44":
                        //                    value = string.Empty;
                        //                    break;
                        //                //임계치 제어 응답
                        //                //빼기 (주석처리)
                        //                //case "45":
                        //                //    value = String.Format("{0}분 ", (int.Parse( item.ChkValue)));
                        //                //    break;
                        //                //무시시간 제어 응답
                        //                //빼기 (주석처리)
                        //                //case "46":
                        //                //    value = String.Format("{0}분 ", (int.Parse( item.ChkValue)));
                        //                //    break;
                        //                //배터리 전압
                        //                case "47":
                        //                    value = String.Format("{0:0.0}V ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //배터리 전류
                        //                case "48":
                        //                    value = String.Format("{0:0.0}A ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //배터리 저항
                        //                case "49":
                        //                    value = String.Format("{0:0.0}mΩ ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //배터리 온도
                        //                case "50":
                        //                    value = String.Format("{0:0.0}℃ ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //배터리 수명
                        //                //2: 교환
                        //                case "51":
                        //                    value = item.ChkValue.Equals("0") ? "정상"
                        //                                 : item.ChkValue.Equals("1") ? "점검요망"
                        //                                 : item.ChkValue.Equals("2") ? "교환"
                        //                                 : "";
                        //                    break;

                        //            }
                        //            #endregion
                        //        }
                        //        //에 따라 chkValue 의 내용이 달라진다.

                        //        this.lstDevice.Items.Add(new ListViewItem(new string[]{i.ToString()
                        //                                                               , chktime
                        //                                                               , devicename
                        //                                                               , item.CheckType
                        //                                                               , item.ItemName
                        //                                                               , value}));

                        //        i++;
                        //    }
                        //} 
                        #endregion
                        #region 요청만 하였을 때의 Request
                        //if (dTable.Rows.Count > 0)
                        //{
                        //    if (dTable.Rows.Count < 1000)
                        //    {
                        //        //헤더 만들기
                        //        ColumnHeaderEx colEx = new ColumnHeaderEx();
                        //        colEx.Text = "";
                        //        colEx.Width = 0;
                        //        colEx.SortType = SortType.NONE;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "번호";
                        //        colEx.Width = 40;
                        //        colEx.SortType = SortType.NONE;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "체크시각";
                        //        colEx.Width = 180;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "측기명";
                        //        colEx.Width = 120;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "구분";
                        //        colEx.Width = 150;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "상태항목";
                        //        colEx.Width = 150;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "상태값";
                        //        colEx.Width = 100;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        //lstDevice.Columns.Add("번호", 40);
                        //        //lstDevice.Columns.Add("체크시각", 150);
                        //        //lstDevice.Columns.Add("측기명", 120);
                        //        //lstDevice.Columns.Add("구분", 150, HorizontalAlignment.Center);
                        //        //lstDevice.Columns.Add("상태항목", 150, HorizontalAlignment.Left);
                        //        //lstDevice.Columns.Add("상태값", 100, HorizontalAlignment.Right);


                        //        int i = 1;
                        //        string value = string.Empty; //상태값
                        //        string chktime = string.Empty; //시각
                        //        string devicename = string.Empty;//측기명
                        //        foreach (DataRow dRow in dTable.Rows)
                        //        {
                        //            value = string.Empty;
                        //            if (dRow["chktime"] != null && dRow["chktime"].ToString() != "")
                        //            {
                        //                chktime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}", Convert.ToDateTime(dRow["chktime"].ToString()));
                        //            }
                        //            if (dRow["devicename"] != null && dRow["devicename"].ToString() != "")
                        //            {
                        //                devicename = dRow["devicename"].ToString();
                        //            }
                        //            if (dRow["chkvalue"] != null && dRow["chkvalue"].ToString() != "")
                        //            {
                        //                value = dRow["chkvalue"].ToString();
                        //                switch (dRow["pkid"].ToString())
                        //                {
                        //                    // "배터리 상태":
                        //                    //0:정상, 1:이상
                        //                    case "1":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "정상" : dRow["chkvalue"].ToString().Equals("1") ? "이상" : "";
                        //                        break;
                        //                    //"태양전지 상태":
                        //                    //0: 정상, 1: 이상
                        //                    case "2":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "정상" : dRow["chkvalue"].ToString().Equals("1") ? "이상 " : "";
                        //                        break;
                        //                    //"시간"
                        //                    //빼기(주석으로)
                        //                    //case "3":
                        //                    //    value = String.Format("{0}시{1}분", dRow["chkvalue"].ToString().Substring(0, 2), dRow["chkvalue"].ToString().Substring(2, 2));
                        //                    //    break;
                        //                    //"FAN 상태":
                        //                    //0: 이상 발생
                        //                    //1: 정상(ON)
                        //                    //2: 정상(OFF)
                        //                    case "4":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "이상 발생" : dRow["chkvalue"].ToString().Equals("1") ? "정상(ON)" : dRow["chkvalue"].ToString().Equals("2") ? "정상(OFF)" : "";
                        //                        break;
                        //                    //"도어 상태":
                        //                    //0: 문 닫힘
                        //                    //1: 문 열림
                        //                    case "5":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "문 닫힘" : dRow["chkvalue"].ToString().Equals("1") ? "문 열림" : "";
                        //                        break;
                        //                    //"안테나 감도":
                        //                    //데이터임 dbm
                        //                    case "6":
                        //                        value = String.Format("{0} dbm", dRow["chkvalue"].ToString());
                        //                        break;
                        //                    //강수량 임계치 1차
                        //                    case "7":
                        //                        value = String.Format("{0:0.0} mm", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //강수량 임계치 2차
                        //                    case "8":
                        //                        value = String.Format("{0} mm", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //강수량 임계치 3차
                        //                    case "9":
                        //                        value = String.Format("{0} mm", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //수위계 임계치 1차
                        //                    case "10":
                        //                        value = String.Format("{0:0.00} m", (double.Parse(dRow["chkvalue"].ToString())) * 0.01);
                        //                        break;
                        //                    //수위계 임계치 2차
                        //                    case "11":
                        //                        value = String.Format("{0:0.00} m", (double.Parse(dRow["chkvalue"].ToString())) * 0.01);
                        //                        break;
                        //                    //수위계 임계치 3차
                        //                    case "12":
                        //                        value = String.Format("{0:0.00} m", (double.Parse(dRow["chkvalue"].ToString())) * 0.01);
                        //                        break;
                        //                    //유속계 임계치 1차
                        //                    case "13":
                        //                        value = String.Format("{0:0.0} m/s", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //유속계 임계치 2차
                        //                    case "14":
                        //                        value = String.Format("{0:0.0} m/s", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //유속계 임계치 3차
                        //                    case "15":
                        //                        value = String.Format("{0:0.0} m/s", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //배터리 임계치 1차(상한)
                        //                    //빼기(주석으로)
                        //                    //case "16":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //배터리 임계치 2차(하한)
                        //                    //빼기(주석으로)
                        //                    //case "17":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //태양전지 1차(상한)
                        //                    //빼기(주석으로)
                        //                    //case "18":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //태양전지 2차(하한)
                        //                    //빼기(주석으로)
                        //                    //case "19":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //동일레벨 무시시간
                        //                    case "20":
                        //                        value = String.Format("{0} 분", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //하향레벨 무시시간
                        //                    case "21":
                        //                        value = String.Format("{0} 분", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //관측모드
                        //                    //빼기(주석으로)
                        //                    //case "22":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "일반모드" : dRow["chkvalue"].ToString().Equals("1") ? "인공지능모드" : "";
                        //                    //    break;
                        //                    //강수 센서 상태
                        //                    //0: 정상
                        //                    //1: 이상
                        //                    //2: 합선
                        //                    //3: 단선
                        //                    case "23":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "정상" : dRow["chkvalue"].ToString().Equals("1") ? "이상" : dRow["chkvalue"].ToString().Equals("2") ? "합선" : dRow["chkvalue"].ToString().Equals("3") ? "단선" : "";
                        //                        break;
                        //                    //수위 센서 상태
                        //                    //0: 정상
                        //                    //1: 이상
                        //                    case "24":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "정상" : dRow["chkvalue"].ToString().Equals("1") ? "이상" : "";
                        //                        break;
                        //                    //유속 센서 상태
                        //                    //0: 정상
                        //                    //1: 이상
                        //                    case "25":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "정상" : dRow["chkvalue"].ToString().Equals("1") ? "이상" : "";
                        //                        break;
                        //                    //F/W 버전
                        //                    //그대로 넣기
                        //                    case "26":
                        //                        value = dRow["chkvalue"].ToString();
                        //                        break;
                        //                    //강수 센서 사용여부
                        //                    //빼기 (주석으로)
                        //                    //case "27":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "사용" : dRow["chkvalue"].ToString().Equals("1") ? "미사용" : "";
                        //                    //    break;
                        //                    //수위 센서 사용여부
                        //                    //빼기 (주석으로)
                        //                    //case "28":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "사용" : dRow["chkvalue"].ToString().Equals("1") ? "미사용" : "";
                        //                    //    break;
                        //                    //유속 센서 사용여부
                        //                    //빼기 (주석으로)
                        //                    //case "29":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "사용" : dRow["chkvalue"].ToString().Equals("1") ? "미사용" : "";
                        //                    //    break;
                        //                    //통신상태
                        //                    //빼기 (주석으로)
                        //                    //case "30":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "프로토콜 응답 없음" : dRow["chkvalue"].ToString().Equals("1") ? "TCP/IP 응답 " : "SMS 응답";
                        //                    //    break;
                        //                    //A/S 작업 보고
                        //                    //맞음
                        //                    case "31":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "A/S 작업 종료" : dRow["chkvalue"].ToString().Equals("1") ? "A/S 작업 시작" : "";
                        //                        break;
                        //                    //RAT 전체 상태
                        //                    //값없음
                        //                    case "34":
                        //                        value = string.Empty;
                        //                        break;
                        //                    //강수 센서 시험 요청
                        //                    //값+ "단계 임계치" 
                        //                    case "38":
                        //                        value = String.Format("{0} 단계 임계치", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //수위 센서 시험 요청
                        //                    //값+ "단계 임계치" 
                        //                    case "39":
                        //                        value = String.Format("{0} 단계 임계치", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //유속 센서 시험 요청
                        //                    //값 + "단계 임계치" 
                        //                    case "40":
                        //                        value = String.Format("{0} 단계 임계치", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //강수 임계치 요청
                        //                    //값 + "단계 임계치"
                        //                    case "41":
                        //                        value = String.Format("{0} 단계 임계치", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //수위 임계치 요청
                        //                    //값 + "단계 임계치"
                        //                    case "42":
                        //                        value = String.Format("{0} 단계 임계치", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //유속 임계치 요청
                        //                    //값 + "단계 임계치"
                        //                    case "43":
                        //                        value = String.Format("{0} 단계 임계치", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //무시시간요청
                        //                    //값없음
                        //                    case "44":
                        //                        value = string.Empty;
                        //                        break;
                        //                    //임계치 제어 응답
                        //                    //빼기 (주석처리)
                        //                    //case "45":
                        //                    //    value = String.Format("{0}분 ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                    //    break;
                        //                    //무시시간 제어 응답
                        //                    //빼기 (주석처리)
                        //                    //case "46":
                        //                    //    value = String.Format("{0}분 ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                    //    break;
                        //                    //배터리 전압
                        //                    case "47":
                        //                        value = String.Format("{0:0.0}V ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //배터리 전류
                        //                    case "48":
                        //                        value = String.Format("{0:0.0}A ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //배터리 저항
                        //                    case "49":
                        //                        value = String.Format("{0:0.0}mΩ ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //배터리 온도
                        //                    case "50":
                        //                        value = String.Format("{0:0.0}℃ ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //배터리 수명
                        //                    //2: 교환
                        //                    case "51":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "정상" : dRow["chkvalue"].ToString().Equals("1") ? "점검요망" : dRow["chkvalue"].ToString().Equals("2") ? "교환" : "";
                        //                        break;

                        //                }
                        //            }
                        //            //에 따라 chkValue 의 내용이 달라진다.

                        //            ListViewItemEx lvItemEx = new ListViewItemEx();
                        //            lvItemEx.SubItems.Add(i.ToString());
                        //            lvItemEx.SubItems.Add(chktime);
                        //            lvItemEx.SubItems.Add(devicename);
                        //            lvItemEx.SubItems.Add(dRow["iscontrol"].ToString());
                        //            lvItemEx.SubItems.Add(dRow["itemname"].ToString());
                        //            lvItemEx.SubItems.Add(value);
                        //            this.lstDevice.Items.Add(lvItemEx);


                        //            //this.lstDevice.Items.Add(new ListViewItem(new string[]{i.ToString()
                        //            //                                                       , chktime
                        //            //                                                       , devicename
                        //            //                                                       , dRow["iscontrol"].ToString()
                        //            //                                                       , dRow["itemname"].ToString()
                        //            //                                                       , value})
                        //            //                                                                   );
                        //            i++;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("출력 데이터의 최대 개수는 1000개 입니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    }
                        //} 
                        #endregion
                       
                        if (totalCnt > 0)
                        {
                            //
                            lstDevice.Columns.Add("번호", 40);
                            lstDevice.Columns.Add("체크시각", 180);
                            lstDevice.Columns.Add("측기명", 120);
                            lstDevice.Columns.Add("구분", 150, HorizontalAlignment.Center);
                            lstDevice.Columns.Add("상태항목", 180);
                            lstDevice.Columns.Add("상태값", 100);

                            if (totalCnt > 2000)
                            {
                                MessageBox.Show("데이터의 최대 출력 개수 (2000개)를 초과하였습니다.\n 2000개 데이터까지 출력 됩니다. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                printEventLstView(2000);
                            }
                            else
                            {
                                printEventLstView(totalCnt);
                            }
                            //번호 재 출력
                            for (int i = 0; i < this.lstDevice.Items.Count; i++)
                            {
                                lstDevice.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                            }
                            
                        }
                        else
                        {
                            this.lstDevice.Columns.Add("결과", lstDevice.Size.Width - 10, HorizontalAlignment.Center);
                            this.lstDevice.Items.Add(new ListViewItem("데이터가 없습니다."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("WeatherRecords.btnSearchDevice_Click() : {0}", ex.Message));
                }
                finally
                {
                    odec.closeDb();
                }
                //조회조건 저장
                setDeviceConstraint();
            }
            else
            {
                MessageBox.Show("시작시각이 끝시각보다 큽니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printEventLstView(int totalCnt)
        {
            string value = string.Empty; //상태값
            string chktime = string.Empty; //시각
            string devicename = string.Empty;//측기명

            for (int i = 1; i <= totalCnt; i++)
            {
                value = string.Empty;
                if (dTable.Rows[i - 1]["chkTime"] != null && dTable.Rows[i - 1]["chkTime"].ToString() != "")
                {
                    chktime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}"
                                                            , Convert.ToDateTime(dTable.Rows[i - 1]["chkTime"]));
                }
                if (dTable.Rows[i - 1]["devicename"] != null && dTable.Rows[i - 1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i - 1]["devicename"].ToString();
                }
                if (dTable.Rows[i - 1]["chkValue"] != null && dTable.Rows[i - 1]["chkValue"].ToString() != "")
                {
                    value = dTable.Rows[i - 1]["chkValue"].ToString();
                    #region SWITCH

                    //di.pkid
                    switch (dTable.Rows[i - 1]["pkid"].ToString())
                    {
                        // "배터리 상태":
                        //0:정상, 1:이상
                        case "1":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //"태양전지 상태":
                        //0: 정상, 1: 이상
                        case "2":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //"시간"
                        //빼기(주석으로)
                        //case "3":
                        //    value = String.Format("{0}시{1}분", dRow["chkvalue"].ToString().Substring(0, 2), dRow["chkvalue"].ToString().Substring(2, 2));
                        //    break;
                        //"FAN 상태":
                        //0: 이상 발생
                        //1: 정상(ON)
                        //2: 정상(OFF)
                        case "4":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "이상 발생"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "정상(ON)"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "정상(OFF)"
                                        : "";
                            break;
                        //"도어 상태":
                        //0: 문 닫힘
                        //1: 문 열림
                        case "5":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "문 닫힘"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "문 열림"
                                        : "";
                            break;
                        //"안테나 감도":
                        //데이터임 dbm
                        case "6":
                            value = String.Format("{0} dbm", dTable.Rows[i - 1]["chkValue"].ToString());
                            break;
                        //강수량 임계치 1차
                        case "7":
                            value = String.Format("{0:0.0} mm", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //강수량 임계치 2차
                        case "8":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //강수량 임계치 3차
                        case "9":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //수위계 임계치 1차
                        case "10":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.01);
                            break;
                        //수위계 임계치 2차
                        case "11":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.01);
                            break;
                        //수위계 임계치 3차
                        case "12":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.01);
                            break;
                        //유속계 임계치 1차
                        case "13":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //유속계 임계치 2차
                        case "14":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //유속계 임계치 3차
                        case "15":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리 임계치 1차(상한)
                        //빼기(주석으로)
                        //case "16":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //배터리 임계치 2차(하한)
                        //빼기(주석으로)
                        //case "17":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //태양전지 1차(상한)
                        //빼기(주석으로)
                        //case "18":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //태양전지 2차(하한)
                        //빼기(주석으로)
                        //case "19":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //동일레벨 무시시간
                        case "20":
                            value = String.Format("{0} 분", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //하향레벨 무시시간
                        case "21":
                            value = String.Format("{0} 분", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //관측모드
                        //빼기(주석으로)
                        //case "22":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "일반모드" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "인공지능모드" : "";
                        //    break;
                        //강수 센서 상태
                        //0: 정상
                        //1: 이상
                        //2: 합선
                        //3: 단선
                        case "23":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "합선"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("3") ? "단선"
                                        : "";
                            break;
                        //수위 센서 상태
                        //0: 정상
                        //1: 이상
                        case "24":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상"
                                        : "";
                            break;
                        //유속 센서 상태
                        //0: 정상
                        //1: 이상
                        case "25":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상"
                                        : "";
                            break;
                        //F/W 버전
                        //그대로 넣기
                        case "26":
                            value = dTable.Rows[i - 1]["chkValue"].ToString();
                            break;
                        //강수 센서 사용여부
                        //빼기 (주석으로)
                        //case "27":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "사용" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미사용" : "";
                        //    break;
                        //수위 센서 사용여부
                        //빼기 (주석으로)
                        //case "28":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "사용" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미사용" : "";
                        //    break;
                        //유속 센서 사용여부
                        //빼기 (주석으로)
                        //case "29":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "사용" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미사용" : "";
                        //    break;
                        //통신상태
                        //빼기 (주석으로)
                        //case "30":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "프로토콜 응답 없음" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "TCP/IP 응답 " : "SMS 응답";
                        //    break;
                        //A/S 작업 보고
                        //맞음
                        case "31":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "A/S 작업 종료"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "A/S 작업 시작"
                                        : "";
                            break;
                        //RAT 전체 상태
                        //값없음
                        case "34":
                            value = string.Empty;
                            break;
                        //강수 센서 시험 요청
                        //값+ "단계 임계치" 
                        case "38":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //수위 센서 시험 요청
                        //값+ "단계 임계치" 
                        case "39":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //유속 센서 시험 요청
                        //값 + "단계 임계치" 
                        case "40":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //강수 임계치 요청
                        //값 + "단계 임계치"
                        case "41":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //수위 임계치 요청
                        //값 + "단계 임계치"
                        case "42":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //유속 임계치 요청
                        //값 + "단계 임계치"
                        case "43":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //무시시간요청
                        //값없음
                        case "44":
                            value = string.Empty;
                            break;
                        //임계치 제어 응답
                        //빼기 (주석처리)
                        //case "45":
                        //    value = String.Format("{0}분 ", (int.Parse(  dTable.Rows[i - 1]["chkValue"].ToString())));
                        //    break;
                        //무시시간 제어 응답
                        //빼기 (주석처리)
                        //case "46":
                        //    value = String.Format("{0}분 ", (int.Parse(  dTable.Rows[i - 1]["chkValue"].ToString())));
                        //    break;
                        //배터리 전압
                        case "47":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리 전류
                        case "48":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리 저항
                        case "49":
                            value = String.Format("{0:0.0}mΩ ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리 온도
                        case "50":
                            value = String.Format("{0:0.0}℃ ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리 수명
                        //2: 교환
                        case "51":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                                         : "";
                            break;
                            //
                        //case "52":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;

                        // 배터리2 전압
                        case "53":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리2 전류
                        case "54":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리2 저항
                        case "55":
                            value = String.Format("{0:0.0}mΩ ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                       //배터리2 온도
                        case "56":
                            value = String.Format("{0:0.0}℃ ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //배터리2 수명
                        case "57":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                                         : "";
                            break;
                        //배터리2 상태
                        case "58":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리1 전압상태
                        case "59":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리1 온도상태
                        case "60":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리1 점검시기
                        //case "61":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리1 교체시기
                        //case "62":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리1 교체(초기화)
                        //case "63":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리2 전압상태
                        case "64":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리2 온도상태
                        case "65":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리2 점검시기
                        //case "66":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리2 교체시기
                        //case "67":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리2 교체(초기화)
                        //case "68":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //AC 전압 입력
                        case "69":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "입력" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미입력" : "";
                            break;
                        //태양전지 전압 입력
                        case "70":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "입력" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미입력" : "";
                            break;
                        //배터리 충전 상태
                        case "71":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "만충" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "충전 중" : "";
                            break;
                        //CDMA RSSI 감도 낮음
                        //case "72":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //CDMA 시간 설정 이상
                        //case "73":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리 감지센서 통신상태
                        case "74":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //우량계 데이터 감지 상태
                        case "75":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "감지" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미감지" : "";
                            break;
                        //수위계 데이터 감지 상태
                        case "76":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "감지" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미감지" : "";
                            break;
                        //유속계 데이터 감지 상태
                        case "77":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "감지" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미감지" : "";
                            break;
                        //배터리 사용 여부
                        case "78":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "사용중" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "미사용 중" : "";
                            break;
                    }
                    #endregion
                }
                //에 따라 chkValue 의 내용이 달라진다.

                this.lstDevice.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , chktime
                                                                                       , devicename
                                                                                       ,dTable.Rows[i - 1]["iscontrol"].ToString()
                                                                                       , dTable.Rows[i - 1]["itemname"].ToString()
                                                                                       , value}));

               // i++;
            }
        }

        private void makeEventQuery2()
        {
            sBuilder = null;
            dTable = null;

            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //측기 리스트 출력하기
            sBuilder = new StringBuilder(500);
            if (this.cbxDeviceNameStatus.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'제어', 0,'요청') iscontrol  ");
                sBuilder.Append("  ,di.itemname, chkvalue FROM devicerequest dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sBuilder.AppendFormat(" UNION ALL ");
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '응답'  iscontrol, di.itemname , dr.resvalue   ");
                sBuilder.Append(" FROM deviceresponse dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sBuilder.Append(" ORDER BY chktime ");
            }
            else
            {
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'제어', 0,'요청') iscontrol  ");
                sBuilder.Append("  ,di.itemname, chkvalue FROM devicerequest dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND dr.fkdevice = {0} "
                                                       , wDeviceList[cbxDeviceNameStatus.SelectedIndex].PKID.ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sBuilder.AppendFormat(" UNION ALL ");
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '응답'  iscontrol, di.itemname , dr.resvalue   ");
                sBuilder.Append(" FROM deviceresponse dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND dr.fkdevice = {0} "
                                                       , wDeviceList[cbxDeviceNameStatus.SelectedIndex].PKID.ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sBuilder.Append(" ORDER BY chktime ");
            }
        }

        //리스트로 통합
        //private List<EventDataType> dTablesToList(DataTable dTable1, DataTable dTable2)
        //{
        //    List<EventDataType> EventList = new List<EventDataType>();
        //    foreach (DataRow dRow in dTable1.Rows)
        //    {
        //        EventList.Add(new EventDataType(dRow["pkid"].ToString()
        //                                                            , dRow["chktime"].ToString()
        //                                                            , dRow["devicename"].ToString()
        //                                                            , dRow["iscontrol"].ToString()
        //                                                            , dRow["itemname"].ToString()
        //                                                            , dRow["chkvalue"].ToString()));
        //    }
        //    foreach (DataRow dRow in dTable2.Rows)
        //    {
        //        EventList.Add(new EventDataType(dRow["pkid"].ToString()
        //                                                            , dRow["chktime"].ToString()
        //                                                            , dRow["devicename"].ToString()
        //                                                            , "응답"
        //                                                            , dRow["itemname"].ToString()
        //                                                            , dTable.Rows[i-1]["resvalue"].ToString()));
        //    }
        //    return EventList;
        //}


        /// <summary>
        /// 이벤트 조회 쿼리 만들기 (응답)
        /// </summary>
        private void makeEventResponseQuery()
        {
            sBuilder = null;

            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //측기 리스트 출력하기
            sBuilder = new StringBuilder(350);
            if (this.cbxDeviceNameStatus.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '응답' , di.itemname , dr.resvalue   ");
                sBuilder.Append(" FROM deviceresponse dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '응답' , di.itemname , dr.resvalue   ");
                sBuilder.Append(" FROM deviceresponse dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND dr.fkdevice = {0} "
                                                       , wDeviceList[cbxDeviceNameStatus.SelectedIndex].PKID.ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //sBuilder.Append(" ORDER BY  dr.chktime  ");
        }

        /// <summary>
        /// 이벤트 조회 쿼리 만들기 (요청)
        /// </summary>
        private void makeEventQuery()
        {
            sBuilder = null;
            dTable = null;

            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(dtpToDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //측기 리스트 출력하기
            sBuilder = new StringBuilder(350);
            if (this.cbxDeviceNameStatus.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'제어', 0,'요청') iscontrol  ");
                sBuilder.Append("  ,di.itemname, chkvalue FROM devicerequest dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'제어', 0,'요청') iscontrol  ");
                sBuilder.Append("  ,di.itemname, chkvalue FROM devicerequest dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND dr.fkdevice = {0} "
                                                       , wDeviceList[cbxDeviceNameStatus.SelectedIndex].PKID.ToString());
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //sBuilder.Append(" ORDER BY chktime ");
        }

        /// <summary>
        ///조회조건 저장 (측기상태)
        /// </summary>
        private void setDeviceConstraint()
        {
            //조회기간
            sBuilderTermDevice = new StringBuilder(100);
            sBuilderTermDevice.Append(dtpFromDevice.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            sBuilderTermDevice.Append(dtpToDevice.Value.ToString("yyyy-MM-dd  HH시 mm분"));

            //측기명
            strDeviceNameDevice = wDeviceList[cbxDeviceNameStatus.SelectedIndex].Name;
        }

        /// <summary>
        /// 기상정보 출력버튼 클릭 시,
        /// </summary>
        private void btnPrintWeather_Click(object sender, EventArgs e)
        {
            ////조회기간
            //StringBuilder sBuilderTerm = new StringBuilder(100);
            //sBuilderTerm.Append(dtpFromWeather.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            //sBuilderTerm.Append(dtpToWeather.Value.ToString("yyyy-MM-dd  HH시 mm분"));

            ////측기명
            //string strDeviceName = wDeviceList[cbxDeviceNameWeather.SelectedIndex].Name;

            ////관측종류
            //string strTypeWeather = cbxTypeWeather.SelectedItem.ToString();


            if (!lstWeather.Columns.Count.Equals(1)
                && (!lstWeather.Items.Count.Equals(0)))
            {
                //레포트 생성 및 View
                fPrint viewForm = null;
                DataTable lstDataTable = null;
                switch (strTypeWeather)
                {
                    case "강우":
                        lstDataTable = lstViewToRainfallDataTable();

                        RainfallReport rainfallReport = new RainfallReport("[ 기상정보 이력 ]"
                                                                                                   , sBuilderTerm.ToString(), strDeviceName, strTypeWeather, lstDataTable);
                        viewForm = new fPrint(rainfallReport, "기상정보 이력");
                        break;

                    case "수위":
                        lstDataTable = lstViewToWaterLevelDataTable();
                        WaterlevelReport waterlevelReport = new WaterlevelReport("[ 기상정보 이력 ]"
                                                                                                     , sBuilderTerm.ToString(), strDeviceName, strTypeWeather, lstDataTable);
                        viewForm = new fPrint(waterlevelReport, "기상정보 이력");
                        break;

                    case "유속":
                        lstDataTable = lstViewToFlowSpeedDataTable();
                        FlowspeedReport flowspeedReport = new FlowspeedReport("[ 기상정보 이력 ]"
                                                                                                          , sBuilderTerm.ToString(), strDeviceName, strTypeWeather, lstDataTable);
                        viewForm = new fPrint(flowspeedReport, "기상정보 이력");
                        break;
                }

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("출력할 데이터가 없습니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///리스트뷰 -> DataTable로 변환 (강우)
        /// </summary>
        private DataTable lstViewToRainfallDataTable()
        {

            DataTable lstDataTable = new DataTable();
            DataRow lstDataRow = null;
            lstDataTable.Columns.Add("chktime");
            lstDataTable.Columns.Add("devicename");
            lstDataTable.Columns.Add("accum10min");
            lstDataTable.Columns.Add("move20min");
            lstDataTable.Columns.Add("today");
            lstDataTable.Columns.Add("yesterday");

            for (int i = 0; i < this.lstWeather.Items.Count; i++)
            {
                lstDataRow = null;
                lstDataRow = lstDataTable.NewRow();
                lstDataRow["chktime"] = lstWeather.Items[i].SubItems[1].Text;
                lstDataRow["devicename"] = lstWeather.Items[i].SubItems[2].Text;
                lstDataRow["accum10min"] = lstWeather.Items[i].SubItems[3].Text;
                lstDataRow["move20min"] = lstWeather.Items[i].SubItems[4].Text;
                lstDataRow["today"] = lstWeather.Items[i].SubItems[5].Text;
                lstDataRow["yesterday"] = lstWeather.Items[i].SubItems[6].Text;
                lstDataTable.Rows.Add(lstDataRow);
            }
            return lstDataTable;
        }

        /// <summary>
        ///리스트뷰 -> DataTable로 변환 (수위)
        /// </summary>
        private DataTable lstViewToWaterLevelDataTable()
        {

            DataTable lstDataTable = new DataTable();
            DataRow lstDataRow = null;

            lstDataTable.Columns.Add("chktime");
            lstDataTable.Columns.Add("devicename");
            lstDataTable.Columns.Add("height");
            lstDataTable.Columns.Add("now");
            lstDataTable.Columns.Add("move15min");
            lstDataTable.Columns.Add("move60min");
            lstDataTable.Columns.Add("changetoday");
            lstDataTable.Columns.Add("maxtoday");

            for (int i = 0; i < this.lstWeather.Items.Count; i++)
            {
                lstDataRow = null;
                lstDataRow = lstDataTable.NewRow();
                lstDataRow["chktime"] = lstWeather.Items[i].SubItems[1].Text;
                lstDataRow["devicename"] = lstWeather.Items[i].SubItems[2].Text;
                lstDataRow["height"] = lstWeather.Items[i].SubItems[3].Text;
                lstDataRow["now"] = lstWeather.Items[i].SubItems[4].Text;
                lstDataRow["move15min"] = lstWeather.Items[i].SubItems[5].Text;
                lstDataRow["move60min"] = lstWeather.Items[i].SubItems[6].Text;
                lstDataRow["changetoday"] = lstWeather.Items[i].SubItems[7].Text;
                lstDataRow["maxtoday"] = lstWeather.Items[i].SubItems[8].Text;
                lstDataTable.Rows.Add(lstDataRow);
            }
            return lstDataTable;
        }

        /// <summary>
        ///리스트뷰 -> DataTable로 변환 (유속)
        /// </summary>
        private DataTable lstViewToFlowSpeedDataTable()
        {
            DataTable lstDataTable = new DataTable();
            DataRow lstDataRow = null;

            lstDataTable.Columns.Add("chktime");
            lstDataTable.Columns.Add("devicename");
            lstDataTable.Columns.Add("now");
            lstDataTable.Columns.Add("change15min");
            lstDataTable.Columns.Add("change60min");
            lstDataTable.Columns.Add("changetoday");
            lstDataTable.Columns.Add("maxtoday");

            for (int i = 0; i < this.lstWeather.Items.Count; i++)
            {
                lstDataRow = null;
                lstDataRow = lstDataTable.NewRow();
                lstDataRow["chktime"] = lstWeather.Items[i].SubItems[1].Text;
                lstDataRow["devicename"] = lstWeather.Items[i].SubItems[2].Text;
                lstDataRow["now"] = lstWeather.Items[i].SubItems[3].Text;
                lstDataRow["change15min"] = lstWeather.Items[i].SubItems[4].Text;
                lstDataRow["change60min"] = lstWeather.Items[i].SubItems[5].Text;
                lstDataRow["changetoday"] = lstWeather.Items[i].SubItems[6].Text;
                lstDataRow["maxtoday"] = lstWeather.Items[i].SubItems[7].Text;
                lstDataTable.Rows.Add(lstDataRow);
            }
            return lstDataTable;
        }

        /// <summary>
        /// 알람정보 출력버튼 클릭 시,
        /// </summary>
        private void btnPrintAlarm_Click(object sender, EventArgs e)
        {
            ////조회기간
            //StringBuilder sBuilderTerm = new StringBuilder(100);
            //sBuilderTerm.Append(dtpFromAlarm.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            //sBuilderTerm.Append(dtpToAlarm.Value.ToString("yyyy-MM-dd HH시 mm분"));

            ////측기명
            //string strDeviceName = wDeviceList[cbxDeviceNameAlarm.SelectedIndex].Name;

            if (!lstAlarm.Columns.Count.Equals(1)
                            && (!lstAlarm.Items.Count.Equals(0)))
            {
                //레포트 생성 및 View
                fPrint viewForm = null;


                //리스트뷰 -> DataTable로 변환 (임계치정보)
                DataTable lstDataTable = lstViewToAlarmDataTable();


                AlarmReport alarmReport = new AlarmReport("[ 임계치정보 이력 ]"
                                                                          , sBuilderTermAlarm.ToString(), strDeviceNameAlarm, this.strTypeWeatherAlarm, lstDataTable);
                //strTypeWeatherAlarm
                viewForm = new fPrint(alarmReport, "임계치정보 이력");

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("출력할 데이터가 없습니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///리스트뷰 -> DataTable로 변환 (알람정보)
        /// </summary>
        private DataTable lstViewToAlarmDataTable()
        {
            DataTable lstDataTable = new DataTable();
            DataRow lstDataRow = null;
            lstDataTable.Columns.Add("alarmtime");
            lstDataTable.Columns.Add("devicename");
            lstDataTable.Columns.Add("alarmtype");
            lstDataTable.Columns.Add("sensortype");
            lstDataTable.Columns.Add("alarmvalue");
            lstDataTable.Columns.Add("realmode");

            for (int i = 0; i < this.lstAlarm.Items.Count; i++)
            {
                lstDataRow = null;
                lstDataRow = lstDataTable.NewRow();
                lstDataRow["alarmtime"] = lstAlarm.Items[i].SubItems[1].Text;
                lstDataRow["devicename"] = lstAlarm.Items[i].SubItems[2].Text;
                lstDataRow["alarmtype"] = lstAlarm.Items[i].SubItems[3].Text;
                lstDataRow["sensortype"] = lstAlarm.Items[i].SubItems[4].Text;
                lstDataRow["alarmvalue"] = lstAlarm.Items[i].SubItems[5].Text;
                lstDataRow["realmode"] = lstAlarm.Items[i].SubItems[6].Text;
                lstDataTable.Rows.Add(lstDataRow);
            }
            return lstDataTable;
        }


        /// <summary>
        /// 측기상태 출력버튼 클릭 시,
        /// </summary>
        private void btnPrintDevice_Click(object sender, EventArgs e)
        {
            ////조회기간
            //StringBuilder sBuilderTerm = new StringBuilder(100);
            //sBuilderTerm.Append(dtpFromDevice.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            //sBuilderTerm.Append(dtpToDevice.Value.ToString("yyyy-MM-dd HH시 mm분"));

            ////측기명
            //string strDeviceName = wDeviceList[cbxDeviceNameStatus.SelectedIndex].Name;

            if (!lstDevice.Columns.Count.Equals(1)
                && (!lstDevice.Items.Count.Equals(0)))
            {
                //레포트 생성 및 View
                fPrint viewForm = null;
                DataTable lstDataTable = null;
                lstDataTable = lstViewToDeviceDataTable();

                DeviceReport deviceReport = new DeviceReport("[ 이벤트 이력]"
                                                                                           , sBuilderTermDevice.ToString(), strDeviceNameDevice, lstDataTable);
                viewForm = new fPrint(deviceReport, "이벤트 이력");

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("출력할 데이터가 없습니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///리스트뷰 -> DataTable로 변환 ( 이벤트 )
        /// </summary>
        private DataTable lstViewToDeviceDataTable()
        {
            DataTable lstDataTable = new DataTable();
            DataRow lstDataRow = null;
            lstDataTable.Columns.Add("chktime");
            lstDataTable.Columns.Add("devicename");
            lstDataTable.Columns.Add("iscontrol");
            lstDataTable.Columns.Add("itemname");
            lstDataTable.Columns.Add("chkvalue");

            for (int i = 0; i < this.lstDevice.Items.Count; i++)
            {
                lstDataRow = null;
                lstDataRow = lstDataTable.NewRow();
                lstDataRow["chktime"] = lstDevice.Items[i].SubItems[1].Text;
                lstDataRow["devicename"] = lstDevice.Items[i].SubItems[2].Text;
                lstDataRow["iscontrol"] = lstDevice.Items[i].SubItems[3].Text;
                lstDataRow["itemname"] = lstDevice.Items[i].SubItems[4].Text;
                lstDataRow["chkvalue"] = lstDevice.Items[i].SubItems[5].Text;
                lstDataTable.Rows.Add(lstDataRow);
            }
            return lstDataTable;
        }

        /// <summary>
        /// 기상정보 리스트 뷰 컬럼 클릭
        /// </summary>
        private void Weather_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            //Determine if clicked column is already the column that is being sorted.
            if (!e.Column.Equals(0))//번호 컬럼이 아니면
            {
                //정렬하기
                if (e.Column == lvwColumnSorterWeather.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorterWeather.Order == SortOrder.Ascending)
                    {
                        lvwColumnSorterWeather.Order = SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorterWeather.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorterWeather.SortColumn = e.Column;
                    lvwColumnSorterWeather.Order = SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                this.lstWeather.Sort();


                //번호 재출력
                for (int i = 0; i < this.lstWeather.Items.Count; i++)
                {
                    lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }

           
        }

        /// <summary>
        /// (알람정보)측기 콤보박스 인덱스 변경시 해당 센서 리스트를 보여준다.
        /// </summary>
        private void cbxDeviceNameAlarmIndexChanged(object sender, EventArgs e)
        {
            cbxTypeWeatherAlarm.Items.Clear();
            cbxTypeWeatherAlarm.Enabled = true;


            switch (wDeviceList[cbxDeviceNameAlarm.SelectedIndex].HaveSensor.ToString())
            {
                case "1":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    break;
                case "2":
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    break;
                case "3":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    break;
                case "4":
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    break;
                case "5":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    break;
                case "6":
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    break;
                case "7":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    break;
                case "8":
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "9":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "10":
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "11":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "12":
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "13":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "14":
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                case "15":
                    cbxTypeWeatherAlarm.Items.Add("강우");
                    cbxTypeWeatherAlarm.Items.Add("수위");
                    cbxTypeWeatherAlarm.Items.Add("유속");
                    cbxTypeWeatherAlarm.Items.Add("풍향풍속");
                    break;
                default:
                    cbxTypeWeatherAlarm.Items.Add("센서 없음");
                    break;
            }
            cbxTypeWeatherAlarm.SelectedIndex = 0; // 첫 항목 선택하기
        }

        /// <summary>
        /// 알람정보 리스트 뷰 컬럼 클릭
        /// </summary>
        private void Alarm_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Determine if clicked column is already the column that is being sorted.
            if (!e.Column.Equals(0))//번호 컬럼이 아니면
            {
                //정렬하기
                if (e.Column == lvwColumnSorterAlarm.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorterAlarm.Order == SortOrder.Ascending)
                    {
                        lvwColumnSorterAlarm.Order = SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorterAlarm.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorterAlarm.SortColumn = e.Column;
                    lvwColumnSorterAlarm.Order = SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                this.lstAlarm.Sort();

                //번호 재 출력
                for (int i = 0; i < this.lstAlarm.Items.Count; i++)
                {
                    lstAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }
          
        }

        /// <summary>
        /// 측기정보 리스트 뷰 컬럼 클릭
        /// </summary>
        private void Device_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Determine if clicked column is already the column that is being sorted.
            if ((!e.Column.Equals(0))
                && (!e.Column.Equals(5)))//정렬할 컬럼이 아니면(번호, 상태값)
            {
                //정렬하기
                if (e.Column == lvwColumnSorterDevice.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorterDevice.Order == SortOrder.Ascending)
                    {
                        lvwColumnSorterDevice.Order = SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorterDevice.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorterDevice.SortColumn = e.Column;
                    lvwColumnSorterDevice.Order = SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                this.lstDevice.Sort();
                
                //번호 재 출력
                for (int i = 0; i < this.lstDevice.Items.Count; i++)
                {
                    lstDevice.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }
          
        }


        /// <summary>
        /// 기상 정보 인덱스 변경시 해당 센서 리스트를 보여준다.
        /// </summary>
        private void cbxTypeWeatherIndexChanged(object sender, EventArgs e)
        {
            //관측 종류에 따른 측기명을 보여준다.
            cbxDeviceNameWeather.Items.Clear();

            switch (this.cbxTypeWeather.SelectedItem.ToString())
            {
                case "강우":
                    this.cbxDeviceNameWeather.Items.Add("전체");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                        {
                            this.cbxDeviceNameWeather.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameWeather.Items.Count > 0) this.cbxDeviceNameWeather.SelectedIndex = 0;
                    break;
                case "수위":
                    this.cbxDeviceNameWeather.Items.Add("전체");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                        {
                            this.cbxDeviceNameWeather.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameWeather.Items.Count > 0) this.cbxDeviceNameWeather.SelectedIndex = 0;
                    break;
                case "유속":
                    this.cbxDeviceNameWeather.Items.Add("전체");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 2) & 1))
                        {
                            this.cbxDeviceNameWeather.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameWeather.Items.Count > 0) this.cbxDeviceNameWeather.SelectedIndex = 0;
                    break;
            }
        }


        //관측 종류 추가 컬럼 선택이 변경되었을 때
        private void cbxTypeWeatherAlarm_IndexChanged(object sender, EventArgs e)
        {
            //관측 종류에 따른 측기명을 보여준다.
            cbxDeviceNameAlarm.Items.Clear();
            selectedDeviceList.Clear();

            switch (this.cbxTypeWeatherAlarm.SelectedItem.ToString())
            {
                case "강우":
                    this.cbxDeviceNameAlarm.Items.Add("전체");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                        {
                            this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
                case "수위":
                    this.cbxDeviceNameAlarm.Items.Add("전체");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                        {
                            this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
                case "유속":
                    this.cbxDeviceNameAlarm.Items.Add("전체");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 2) & 1))
                        {
                            this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
                case "전체":
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
            }
        }


        /// <summary>
        /// 측기 알람 조회 버튼 클릭 시
        /// </summary>
        private void btnSearchDeviceAlarm_Click(object sender, EventArgs e)
        {
            //0. 리스트뷰 헤더, 내용 클리어
            lstDeviceAlarm.Columns.Clear();
            lstDeviceAlarm.Items.Clear();

            // 기간 체크
            if (checkDate(this.dtpFromDeviceAlarm, this.dtpToDeviceAlarm) >= 0)
            {
                try
                {
                    if (odec.openDb())
                    {
                        // 측기 알람 조회 쿼리 만들기 (요청 DataTable)
                        makeDeviceAlarmQuery();

                        dTable = odec.getDataTable(sBuilder.ToString(), "deviceAlarm");
                        int totalCnt = dTable.Rows.Count;

                        if (dTable.Rows.Count > 0)
                        {

                            //헤더 만들기
                            this.lstDeviceAlarm.Columns.Add("번호", 40);
                            this.lstDeviceAlarm.Columns.Add("알람발생시각", 180);
                            this.lstDeviceAlarm.Columns.Add("측기명", 120);
                            this.lstDeviceAlarm.Columns.Add("상태항목", 180);
                            this.lstDeviceAlarm.Columns.Add("상태값", 100);
                            
                            if (totalCnt > 2000)
                            {
                                MessageBox.Show("데이터의 최대 출력 개수 (2000개)를 초과하였습니다.\n 2000개 데이터까지 출력 됩니다. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                printDeviceAlarmLstView(2000);
                            }
                            else
                            {
                                printDeviceAlarmLstView(totalCnt);
                            }

                            //번호 재 출력
                            for (int i = 0; i < this.lstDeviceAlarm.Items.Count; i++)
                            {
                                lstDeviceAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                            }
                            
                        }
                        else
                        {
                            this.lstDeviceAlarm.Columns.Add("결과", this.lstDeviceAlarm.Size.Width - 10, HorizontalAlignment.Center);
                            this.lstDeviceAlarm.Items.Add(new ListViewItem("데이터가 없습니다."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("WeatherRecords.btnSearchDeviceAlarm_Click() : {0}", ex.Message));
                }
                finally
                {
                    odec.closeDb();
                }
                //조회조건 저장
                setDeviceAlarmConstraint();
            }
            else
            {
                MessageBox.Show("시작시각이 끝시각보다 큽니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printDeviceAlarmLstView(int totalCnt)
        {
            //int i = 1;
            string value = string.Empty; //상태값
            string chktime = string.Empty; //시각
            string devicename = string.Empty;//측기명

            for(int i = 1; i <=totalCnt; i++)
            {
                value = string.Empty;

                if (dTable.Rows[i-1]["chktime"] != null && dTable.Rows[i-1]["chktime"].ToString() != "")
                {
                    chktime = String.Format("{0:yyyy-MM-dd  HH 시 mm분 ss초}"
                                                            , Convert.ToDateTime(dTable.Rows[i-1]["chktime"].ToString()));
                }
                if (dTable.Rows[i-1]["devicename"] != null && dTable.Rows[i-1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i-1]["devicename"].ToString();
                }
                if (dTable.Rows[i-1]["resvalue"] != null && dTable.Rows[i-1]["resvalue"].ToString() != "")
                {
                    value = dTable.Rows[i-1]["resvalue"].ToString();

                    #region SWITCH
                    switch (dTable.Rows[i-1]["fkdeviceitem"].ToString().Trim())
                    {
                        // "배터리 상태":
                        //0:정상, 1:이상
                        case "1":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //"태양전지 상태":
                        //0: 정상, 1: 이상
                        case "2":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //"시간"
                        //빼기(주석으로)
                        //case "3":
                        //    value = String.Format("{0}시{1}분", dTable.Rows[i-1]["chkvalue"].ToString().Substring(0, 2), dTable.Rows[i-1]["chkvalue"].ToString().Substring(2, 2));
                        //    break;
                        //"FAN 상태":
                        //0: 이상 발생
                        //1: 정상(ON)
                        //2: 정상(OFF)
                        case "4":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "이상 발생"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "정상(ON)"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("2") ? "정상(OFF)"
                                        : "";
                            break;
                        //"도어 상태":
                        //0: 문 닫힘
                        //1: 문 열림
                        case "5":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "문 닫힘"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "문 열림"
                                        : "";
                            break;
                        //"안테나 감도":
                        //데이터임 dbm
                        case "6":
                            value = String.Format("{0} dbm", dTable.Rows[i-1]["resvalue"].ToString());
                            break;
                        //강수량 임계치 1차
                        case "7":
                            value = String.Format("{0:0.0} mm", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //강수량 임계치 2차
                        case "8":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //강수량 임계치 3차
                        case "9":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //수위계 임계치 1차
                        case "10":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.01);
                            break;
                        //수위계 임계치 2차
                        case "11":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.01);
                            break;
                        //수위계 임계치 3차
                        case "12":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.01);
                            break;
                        //유속계 임계치 1차
                        case "13":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //유속계 임계치 2차
                        case "14":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //유속계 임계치 3차
                        case "15":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리 임계치 1차(상한)
                        //빼기(주석으로)
                        //case "16":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //배터리 임계치 2차(하한)
                        //빼기(주석으로)
                        //case "17":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //태양전지 1차(상한)
                        //빼기(주석으로)
                        //case "18":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //태양전지 2차(하한)
                        //빼기(주석으로)
                        //case "19":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //동일레벨 무시시간
                        case "20":
                            value = String.Format("{0} 분", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //하향레벨 무시시간
                        case "21":
                            value = String.Format("{0} 분", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //관측모드
                        //빼기(주석으로)
                        //case "22":
                        //    value =  dRow["resvalue"].ToString().Equals("0") ? "일반모드" :  dRow["resvalue"].ToString().Equals("1") ? "인공지능모드" : "";
                        //    break;
                        //강수 센서 상태
                        //0: 정상
                        //1: 이상
                        //2: 합선
                        //3: 단선
                        case "23":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "정상"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "이상"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("2") ? "합선"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("3") ? "단선"
                                        : "";
                            break;
                        //수위 센서 상태
                        //0: 정상
                        //1: 이상
                        case "24":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "정상"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "이상"
                                        : "";
                            break;
                        //유속 센서 상태
                        //0: 정상
                        //1: 이상
                        case "25":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "정상"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "이상"
                                        : "";
                            break;
                        //F/W 버전
                        //그대로 넣기
                        case "26":
                            value = dTable.Rows[i-1]["resvalue"].ToString();
                            break;
                        //강수 센서 사용여부
                        //빼기 (주석으로)
                        //case "27":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "사용" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "미사용" : "";
                        //    break;
                        //수위 센서 사용여부
                        //빼기 (주석으로)
                        //case "28":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "사용" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "미사용" : "";
                        //    break;
                        //유속 센서 사용여부
                        //빼기 (주석으로)
                        //case "29":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "사용" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "미사용" : "";
                        //    break;
                        //통신상태
                        //빼기 (주석으로)
                        //case "30":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "프로토콜 응답 없음" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "TCP/IP 응답 " : "SMS 응답";
                        //    break;
                        //A/S 작업 보고
                        //맞음
                        case "31":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "A/S 작업 종료"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "A/S 작업 시작"
                                        : "";
                            break;
                        //RAT 전체 상태
                        //값없음
                        case "34":
                            value = string.Empty;
                            break;
                        //강수 센서 시험 요청
                        //값+ "단계 임계치" 
                        case "38":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //수위 센서 시험 요청
                        //값+ "단계 임계치" 
                        case "39":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //유속 센서 시험 요청
                        //값 + "단계 임계치" 
                        case "40":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //강수 임계치 요청
                        //값 + "단계 임계치"
                        case "41":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //수위 임계치 요청
                        //값 + "단계 임계치"
                        case "42":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //유속 임계치 요청
                        //값 + "단계 임계치"
                        case "43":
                            value = String.Format("{0} 단계 임계치", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //무시시간요청
                        //값없음
                        case "44":
                            value = string.Empty;
                            break;
                        //임계치 제어 응답
                        //빼기 (주석처리)
                        //case "45":
                        //    value = String.Format("{0}분 ", (int.Parse( dTable.Rows[i-1]["resvalue"].ToString())));
                        //    break;
                        //무시시간 제어 응답
                        //빼기 (주석처리)
                        //case "46":
                        //    value = String.Format("{0}분 ", (int.Parse( dTable.Rows[i-1]["resvalue"].ToString())));
                        //    break;
                        //배터리 전압
                        case "47":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리 전류
                        case "48":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리 저항
                        case "49":
                            value = String.Format("{0:0.0}mΩ ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리 온도
                        case "50":
                            value = String.Format("{0:0.0}℃ ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리 수명
                        //2: 교환
                        case "51":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "정상"
                                         : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "점검요망"
                                         : dTable.Rows[i-1]["resvalue"].ToString().Equals("2") ? "교환"
                                         : "";
                            break;
                        //
                        //case "52":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;

                        // 배터리2 전압
                        case "53":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리2 전류
                        case "54":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리2 저항
                        case "55":
                            value = String.Format("{0:0.0}mΩ ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리2 온도
                        case "56":
                            value = String.Format("{0:0.0}℃ ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //배터리2 수명
                        case "57":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                                         : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                                         : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                                         : "";
                            break;
                        //배터리2 상태
                        case "58":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리1 전압상태
                        case "59":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리1 온도상태
                        case "60":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리1 점검시기
                        //case "61":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리1 교체시기
                        //case "62":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리1 교체(초기화)
                        //case "63":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리2 전압상태
                        case "64":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리2 온도상태
                        case "65":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //배터리2 점검시기
                        //case "66":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리2 교체시기
                        //case "67":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리2 교체(초기화)
                        //case "68":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //AC 전압 입력
                        case "69":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "입력" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "미입력" : "";
                            break;
                        //태양전지 전압 입력
                        case "70":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "입력" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "미입력" : "";
                            break;
                        //배터리 충전 상태
                        case "71":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "만충" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "충전 중" : "";
                            break;
                        //CDMA RSSI 감도 낮음
                        //case "72":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //CDMA 시간 설정 이상
                        //case "73":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "점검요망"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "교환"
                        //                 : "";
                        //    break;
                        //배터리 감지센서 통신상태
                        case "74":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "정상" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "이상" : "";
                            break;
                        //우량계 데이터 감지 상태
                        case "75":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "감지" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "미감지" : "";
                            break;
                        //수위계 데이터 감지 상태
                        case "76":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "감지" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "미감지" : "";
                            break;
                        //유속계 데이터 감지 상태
                        case "77":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "감지" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "미감지" : "";
                            break;
                        //배터리 사용 여부
                        case "78":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "사용중" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "미사용 중" : "";
                            break;
                    } 
                    #endregion
                }
                //에 따라 resValue 의 내용이 달라진다.

                this.lstDeviceAlarm.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , chktime
                                                                                       , devicename
                                                                                       , dTable.Rows[i-1]["itemname"].ToString()
                                                                                       , value}));
               // i++;
            }
        }

        //조회조건 저장
        private void setDeviceAlarmConstraint()
        {
            //조회기간
            sBuilderTermDeviceAlarm = new StringBuilder(100);
            sBuilderTermDeviceAlarm.Append(this.dtpFromDeviceAlarm.Value.ToString("yyyy-MM-dd  HH시 mm분 ~ "));
            sBuilderTermDeviceAlarm.Append(this.dtpToDeviceAlarm.Value.ToString("yyyy-MM-dd  HH시 mm분"));

            //측기명
            this.strDeviceNameDeviceAlarm = wDeviceList[this.cbxDeviceNameDeviceAlarm.SelectedIndex].Name;
        }

        // 측기 알람 조회 쿼리 만들기 (요청 DataTable)
        private void makeDeviceAlarmQuery()
        {
            sBuilder = null;
            dTable = null;
            //기간의 끝 시간에서 초 부분을 0으로 만들고 1분을 더하여 쿼리문을 만든다.
            DateTime addMinToDate = Convert.ToDateTime(this.dtpToDeviceAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);
            sBuilder = new StringBuilder(350);
            //우량 정보 쿼리 만들기
            if (this.cbxDeviceNameDeviceAlarm.SelectedIndex.Equals(0))
            {
                sBuilder.Append("  SELECT chktime, d.devicename, di.itemname, resvalue, da.fkdeviceitem ");
                sBuilder.Append("  FROM devicealarm da ");
                sBuilder.Append("  JOIN device d ON d.pkid = da.fkdevice ");
                sBuilder.Append("  JOIN deviceitem di ON di.pkid = da.fkdeviceitem ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , this.dtpFromDeviceAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sBuilder.Append("  SELECT chktime, d.devicename, di.itemname, resvalue, da.fkdeviceitem ");
                sBuilder.Append("  FROM devicealarm da ");
                sBuilder.Append("  JOIN device d ON d.pkid = da.fkdevice ");
                sBuilder.Append("  JOIN deviceitem di ON di.pkid = da.fkdeviceitem ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND da.fkdevice = {0} "
                                                        , wDeviceList[cbxDeviceNameDeviceAlarm.SelectedIndex].PKID.ToString());

                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                    , this.dtpFromDeviceAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                    , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            sBuilder.Append(" ORDER BY chktime ");
        }


        /// <summary>
        /// 측기 알람 출력 버튼 클릭 시 
        /// </summary>
        private void btnPrintDeviceAlarm_Click(object sender, EventArgs e)
        {
            if (!this.lstDeviceAlarm.Columns.Count.Equals(1)
                            && (!this.lstDeviceAlarm.Items.Count.Equals(0)))
            {
                //레포트 생성 및 View
                fPrint viewForm = null;

                //리스트뷰 -> DataTable로 변환 (측기 알람 정보)
                DataTable lstDataTable = lstViewToDeviceAlarmDataTable();

                DeviceAlarmReport deviceAlarmReport = new DeviceAlarmReport("[ 알람정보 이력 ]"
                                                                                                                    , this.sBuilderTermDeviceAlarm.ToString()
                                                                                                                    , this.strDeviceNameDeviceAlarm
                                                                                                                    , lstDataTable);

                viewForm = new fPrint(deviceAlarmReport, "알람정보 이력");

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("출력할 데이터가 없습니다.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///리스트뷰 -> DataTable로 변환 (측기 알람 정보)
        /// </summary>
        private DataTable lstViewToDeviceAlarmDataTable()
        {
            DataTable lstDataTable = new DataTable();
            DataRow lstDataRow = null;
            lstDataTable.Columns.Add("chktime");
            lstDataTable.Columns.Add("devicename");
            lstDataTable.Columns.Add("itemname");
            lstDataTable.Columns.Add("chkvalue");

            for (int i = 0; i < this.lstDeviceAlarm.Items.Count; i++)
            {
                lstDataRow = null;
                lstDataRow = lstDataTable.NewRow();
                lstDataRow["chktime"] = this.lstDeviceAlarm.Items[i].SubItems[1].Text;
                lstDataRow["devicename"] = this.lstDeviceAlarm.Items[i].SubItems[2].Text;
                lstDataRow["itemname"] = this.lstDeviceAlarm.Items[i].SubItems[3].Text;
                lstDataRow["chkvalue"] = this.lstDeviceAlarm.Items[i].SubItems[4].Text;
                lstDataTable.Rows.Add(lstDataRow);
            }
            return lstDataTable;
        }

        private void lstDeviceAlarm_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Determine if clicked column is already the column that is being sorted.
            if ((!e.Column.Equals(0))
                && (!e.Column.Equals(4)))//정렬할 컬럼이 아니면(번호, 상태값)
            {
                //정렬하기
                if (e.Column == lvwColumnSorterDeviceAlarm.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorterDeviceAlarm.Order == SortOrder.Ascending)
                    {
                        lvwColumnSorterDeviceAlarm.Order = SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorterDeviceAlarm.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorterDeviceAlarm.SortColumn = e.Column;
                    lvwColumnSorterDeviceAlarm.Order = SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                this.lstDeviceAlarm.Sort();

                //번호 재 출력
                for (int i = 0; i < this.lstDeviceAlarm.Items.Count; i++)
                {
                    lstDeviceAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }

           
        }

    }
}