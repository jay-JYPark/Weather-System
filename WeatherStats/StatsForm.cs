using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ADEng.Library;  //DAC ����� ����
using ADEng.Library.WeatherSystem; //WDevice ����� ����
using ChartFX.WinForms; //��Ʈ FX CustomGridLine�� �׸��� ����

using ADEng.Control;
using WeatherStats.Properties;//Report �ε��� ���� ���� ���� (clsPView)


namespace ADEng.Module.WeatherSystem
{
    public partial class StatsForm : Form
    {
        private WeatherDataMng dataMng = null;

        //����Ŭ ����
        oracleDAC odec = null;
        StringBuilder sBuilder = null;
        DataTable dTable = null;

        //���� ����Ʈ
        List<WDevice> wDeviceList = new List<WDevice>();
        //�ð� ����Ʈ
        List<Time> timeList = new List<Time>();
        //X�� ����Ʈ
        List<XAxis> xAxisList = new List<XAxis>();
        //�׷����� ����Ʈ ��
        List<int> pointList = new List<int>();
        ////�˶����� ����Ʈ
        List<double> alarmLevelList = null;

        //��Ʈ�� �Ѱ��� DataList
        List<double> dataList = null;

        //��ȸ ��, ���� ����
        //��ȸ�Ⱓ
        string strTerm = string.Empty;

        //�ð�����
        string strUnit = string.Empty;

        //�����
        string strDeviceName = string.Empty;

        //��������
        string strTypeWeather = string.Empty;


        /// <summary>
        /// �⺻ ������
        /// </summary>
        public StatsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        ///  ���� �ε�ɶ�
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

            //���� ����Ʈ ��������
            getDeviceList();

            //�ð� ����Ʈ �־��ֱ�
            getTimeList();

            //��Ʈ �޴� ����
            blockChartMenu();

            //��Ʈ �⺻ �Ӽ� ����
            setChartAttribute();

            // ��ȸ ������ Base ���� �ε�
            loadBaseData();

            // ��� ������ Ŭ����
            chtWeather2.Data.Clear();
        }

        //DB �׸��� �����ϸ� �߻��ϴ� �̺�Ʈ
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
        ///��Ʈ �޴� ����
        /// </summary>
        private void blockChartMenu()
        {
            chtWeather2.ContextMenus = false;
            chtWeather2.ToolBar.Visible = false;
            chtWeather2.MenuBar.Visible = false;
            chtWeather2.ToolTips = false;
        }

        /// <summary>
        ///���� ����Ʈ ��������
        /// </summary>
        private void getDeviceList()
        {
            try
            {
                if (odec.openDb())
                {
                    //���� ����Ʈ ����ϱ�
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
        ///�ð� ����Ʈ �־��ֱ�
        /// </summary>
        private void getTimeList()
        {
            this.timeList.Add(new Time(2.0, "���� 2�ð�"));
            this.timeList.Add(new Time(6.0, "���� 6�ð�"));
            this.timeList.Add(new Time(12.0, "���� 12�ð�"));
            this.timeList.Add(new Time(24.0, "���� 1��"));
            //this.timeList.Add(new Time(7.0, 7��")); //�� ���ķ�...�Ϸ簡 �Ѿ...�ð����� ȯ��...?
            //this.timeList.Add(new Time(30.0, "1��"));  //��¥ ������ ����..
            //this.timeList.Add(new Time(365.0, "1��"));
        }


        /// <summary>
        /// ��Ʈ �⺻ �Ӽ� ����
        /// </summary>
        private void setChartAttribute()
        {
            //�׷��� �� ����
            chtWeather2.Data.Series = 1;

            // ���� 
            this.chtWeather2.LegendBox.Visible = false;

            //��Ʈ ������ ����
            chtWeather2.AxisX.Font = new Font(FontFamily.GenericSansSerif, 8);
        }

        /// <summary>
        /// ��ȸ ������ Base ���� �ε�
        /// </summary>
        private void loadBaseData()
        {
            //�ð� ����Ʈ�� �߰�
            addTimeList();

            //���� ����Ʈ ���
            addDeviceList();
        }

        /// <summary>
        /// �ð� ����Ʈ�� �߰�
        /// </summary>
        private void addTimeList()
        {
            foreach (Time time in timeList)
            {
                cbxByTime.Items.Add(time.Text);
            }
            if (cbxByTime.Items.Count > 0)
            {
                cbxByTime.SelectedIndex = 0; //ù��° �׸� ����
            }
        }


        /// <summary>
        /// ���� ����Ʈ ���
        /// </summary>
        private void addDeviceList()
        {
            foreach (WDevice device in wDeviceList)
            {
                //��� ���� Tab Page  �� ���⸮��Ʈ
                this.cbxDeviceName.Items.Add(device.Name.Trim());
            }
            if (cbxDeviceName.Items.Count > 0)
            {
                this.cbxDeviceName.SelectedIndex = 0; //����Ʈ�� ù �׸� �����ϱ�
            }
        }


        /// <summary>
        /// ���� �޺��ڽ� �ε��� ����� �ش� ���� ����Ʈ�� �����ش�.
        /// </summary>
        private void cbxDeviceNameIndexChanged(object sender, EventArgs e)
        {
            cbxTypeWeather.Items.Clear();
            cbxTypeWeather.Enabled = true;


            switch (wDeviceList[cbxDeviceName.SelectedIndex].HaveSensor.ToString())
            {
                case "1":
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "2":
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "3":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "4":
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "5":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "6":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "7":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    break;
                case "8":
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "9":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "10":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "11":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "12":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "13":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "14":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                case "15":
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("����");
                    cbxTypeWeather.Items.Add("ǳ��ǳ��");
                    break;
                default:
                    cbxTypeWeather.Items.Add("���� ����");
                    break;
            }            
            cbxTypeWeather.SelectedIndex = 0; // ù �׸� �����ϱ�
        }


        /// <summary>
        /// ��ȸ ��ư Ŭ�� ��,
        /// </summary>
        private void btnSearchClick(object sender, EventArgs e)
        {
            //����1:  ������� ����(����,...)
            //����2:  �ð�����(2�ð�,...)
            if (cbxTypeWeather.SelectedItem.ToString().Equals("����"))
            {
                // Y �� ������ ���� ����
                chtWeather2.AxisY.LabelsFormat.CustomFormat = "#.0 mm";
                // ��Ʈ ����
                chtWeather2.Gallery = ChartFX.WinForms.Gallery.Bar;
            }
            else if (cbxTypeWeather.SelectedItem.ToString().Equals("����"))
            {
                // Y �� ������ ���� ����
                chtWeather2.AxisY.LabelsFormat.CustomFormat = "#.#0 m";
                // ��Ʈ ����
                chtWeather2.Gallery = ChartFX.WinForms.Gallery.Area;
            }
            else if (cbxTypeWeather.SelectedItem.ToString().Equals("����"))
            {
                // Y �� ������ ���� ����
                chtWeather2.AxisY.LabelsFormat.CustomFormat = "#.0 m/s";
                // ��Ʈ ����
                chtWeather2.Gallery = ChartFX.WinForms.Gallery.Curve;
            }

            //�����ð� �޾ƿ���
            double number = timeList[cbxByTime.SelectedIndex].Number; //double ��
            int numberToInt = Convert.ToInt32(number); //int�� ��ȯ
            //int numberToIntMultiple = numberToInt * 6;

            //X �� �ð� ����Ʈ ����
            makeXAxisList2(numberToInt);

            // ��� ������ Ŭ����
            chtWeather2.Data.Clear();

            //��ü ����Ʈ ���� ����
            //number*6  =  �ð�* 1�ð��� ����
            chtWeather2.Data.Points = (numberToInt * 6) + 1;

            //�Ӱ�ġ �˶� �� �׸���
            drawAlarmLine(cbxTypeWeather.SelectedItem.ToString());

            //X�� ��Ī ���
            for (int i = 0; i < xAxisList.Count; i++)
            //for (int i = 0; i <= 12; i++)
            {
                chtWeather2.AxisX.Labels[i] = xAxisList[i].DateOutputFormat;
            }

            //X �� �ð��� �ش��ϴ� ������ ���
            printDataBytimeOnGraph(cbxTypeWeather.SelectedItem.ToString());

            //�����ð��� ���� X �� ��� ���� ����
            printAxisXTerm();

            //��ȸ���� ����
            saveConstraint();

        }

        /// <summary>
        ///��ȸ���� ����
        /// </summary>
        private void saveConstraint()
        {
            //��ȸ�Ⱓ
           strTerm = this.dtpDate.Value.ToString("yyyy-MM-dd  HH�� mm��");

            //�ð�����
            strUnit = this.cbxByTime.SelectedItem.ToString();

            //�����
           strDeviceName = wDeviceList[cbxDeviceName.SelectedIndex].Name;

            //��������
            strTypeWeather = cbxTypeWeather.SelectedItem.ToString();
        }

        /// <summary>
        ///�����ð��� ���� X �� ��� ���� ����
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
        ///  X �� �ð��� �ش��ϴ� ������ ���
        /// </summary>
        ///<param name="strTypeWeather">���� ����(����, ����, ����)</param>
        private void printDataBytimeOnGraph(string strTypeWeather)
        {
            dataList = new List<double>(xAxisList.Count); //��Ʈ�� �Ѱ��� ������ ����Ʈ
            //�Ҽ��� �����͸� ǥ���ϱ� ����
            try
            {
                if (odec.openDb())
                {
                    switch (strTypeWeather)
                    {
                        case "����":
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
                        case "����":
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
                        case "����":
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
        ///  X �࿡ �ش��ϴ� �ð��� �����͸� �����´�.
        /// </summary>
        /// <param name="strDatetime">�ش� �ð�</param>
        /// <returns>������ ��ȯ</returns>
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
        ///  X �� �ð� ����Ʈ ����
        /// </summary>
        /// <param name="numberToInt">�ð� ����(2,6,12,...)</param>
        private void makeXAxisList2(int numberToInt)
        {
            //���� �ð�: 0,10,20,30,40,50 �� ���� 10�� ������ number (���ýð�) ��ŭ ����Ʈ �����.
            double numberDouble = Convert.ToDouble(numberToInt);
            DateTime tmpDate = new DateTime();
            tmpDate = dtpDate.Value.AddHours(-numberDouble);//�ش� �ð� ������ �ʱⰪ ����
            int minTime = Convert.ToInt32(tmpDate.ToString("HH:mm").Substring(3, 2)); //�и� ������

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

            //����Ʈ �ʱ�ȭ
            xAxisList.Clear();

            //  �ð�* 1�ð��� ����(10�п� 1��, So 6��)
            int numberToIntMultiple = numberToInt * 6;

            //�ð� ���� :  2�ð��϶�
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
            //�ð� ���� :  6�ð��϶�
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
            //�ð� ���� :  12�ð��϶�
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
            //�ð� ���� :  24�ð��϶�
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
        ///  �Ӱ�ġ �˶� �� �׸���
        /// </summary>
        ///<param name="strTypeWeather">���� ����(����, ����, ����)</param>
        private void drawAlarmLine(string strTypeWeather)
        {
            //�׷����� �˶�����
            alarmLevelList = null;
            alarmLevelList = new List<double>(3); //1,2,3 �� �Ӱ�ġ�� �������
            CustomGridLine alarmLine1 = null; // �˶����� ( �׸��� ����)
            CustomGridLine alarmLine2 = null;
            CustomGridLine alarmLine3 = null;

            //�׷��� �� �ʱ�ȭ
            chtWeather2.AxisY.CustomGridLines.Clear();

            switch (strTypeWeather)
            {
                case "����":
                    alarmLevelList.Add(getAlarmValue("7"));//������ 1�� �Ӱ�ġ
                    alarmLevelList.Add(getAlarmValue("8"));//������ 2�� �Ӱ�ġ
                    alarmLevelList.Add(getAlarmValue("9")); //������ 3�� �Ӱ�ġ
                    break;
                case "����":
                    alarmLevelList.Add(getAlarmValue("10"));//���� 1�� �Ӱ�ġ
                    alarmLevelList.Add(getAlarmValue("11"));//���� 2�� �Ӱ�ġ
                    alarmLevelList.Add(getAlarmValue("12")); //���� 3�� �Ӱ�ġ
                    break;
                case "����":
                    alarmLevelList.Add(getAlarmValue("13"));//���� 1�� �Ӱ�ġ
                    alarmLevelList.Add(getAlarmValue("14"));//���� 2�� �Ӱ�ġ
                    alarmLevelList.Add(getAlarmValue("15")); //���� 3�� �Ӱ�ġ

                    break;
            }


            //���
            alarmLine1 = new CustomGridLine();
            alarmLine1.Value = alarmLevelList[0]; //�������(����,...) 1�� �Ӱ�ġ
            alarmLine1.Color = Color.Orange;
            alarmLine1.Text = "����";
            alarmLine1.TextColor = Color.Orange;
            chtWeather2.AxisY.CustomGridLines.Add(alarmLine1);

            //����
            alarmLine2 = new CustomGridLine();
            alarmLine2.Value = alarmLevelList[1];//�������(����,...) 2�� �Ӱ�ġ
            alarmLine2.Color = Color.Violet;
            alarmLine2.Text = "���";
            alarmLine2.TextColor = Color.OrangeRed;
            chtWeather2.AxisY.CustomGridLines.Add(alarmLine2);

            //����
            alarmLine3 = new CustomGridLine();
            alarmLine3.Value = alarmLevelList[2];//�������(����,...) 3�� �Ӱ�ġ
            alarmLine3.Color = Color.Red;
            alarmLine3.Text = "����";
            alarmLine3.TextColor = Color.Red;
            chtWeather2.AxisY.CustomGridLines.Add(alarmLine3);
        }

        /// <summary>
        /// �˶� �� ������
        /// </summary>
        /// <param name="level">���� �׸� ��(���� �� ���������) eg: ���� 1�� �Ӱ�ġ </param>
        /// <returns>�˶� ������ ��ȯ</returns>
        private double getAlarmValue(string level)
        {
            double levelValue = double.MinValue;
            try
            {
                if (odec.openDb())
                {
                    sBuilder = null;
                    dTable = null;
                    // number = -number; //���̳ʽ��� ��ȣ �ٲ��ֱ�

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
        ///  ��� ��ư Ŭ�� ��, 
        /// </summary>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            ////��ȸ�Ⱓ
            //string strTerm = this.dtpDate.Value.ToString("yyyy-MM-dd  HH�� mm��");

            ////�ð�����
            //string strUnit = this.cbxByTime.SelectedItem.ToString();

            ////�����
            //string strDeviceName = wDeviceList[cbxDeviceName.SelectedIndex].Name;

            ////��������
            //string strTypeWeather = cbxTypeWeather.SelectedItem.ToString();

            //����Ʈ ���� �� View
            fPrint viewForm = null;

            WeatherStatsReport weatherStatsReport = new WeatherStatsReport("[ ������� ��� ]"
                                                                                               , strTerm, strUnit, strDeviceName, strTypeWeather
                                                                                               , dataList, this.xAxisList, timeList[cbxByTime.SelectedIndex].Number
                                                                                               , alarmLevelList);
            viewForm = new fPrint(weatherStatsReport, "������� �����ȸ");

            //Form SHOW
            viewForm.Show();
        }

    }
}