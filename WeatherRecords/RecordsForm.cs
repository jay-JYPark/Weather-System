using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using ADEng.Library; //DAC ����� ����
using ADEng.Library.WeatherSystem; // WeatheObject ����� ����
using DataDynamics.ActiveReports;//Report ����� ����
using ADEng.Control;//clsPView(����Ʈ ��� �� ����) ����� ����
using WeatherRecords.Properties;




/// �ּ� �����ϱ� ( ���� �� ���� )
/// <summary>
/// ���� ����/  ������� / �˶��ܰ�  ����Ʈ�� �����´�.
/// </summary>
/// <param name="_sdt">���� �ð�</param>
/// <param name="_edt">���� �ð�</param>
/// <returns>��� ���� ������ �߻��ϴ� ��� ���� ����Ʈ�� ��ȯ</returns>
///

namespace ADEng.Module.WeatherSystem
{
    public partial class RecordsForm : Form
    {
        //�÷� ������ ���� ���
        private ListViewColumnSorter lvwColumnSorterWeather = new ListViewColumnSorter();
        private ListViewColumnSorter lvwColumnSorterAlarm = new ListViewColumnSorter();
        private ListViewColumnSorter lvwColumnSorterDevice = new ListViewColumnSorter();
        private ListViewColumnSorter lvwColumnSorterDeviceAlarm = new ListViewColumnSorter();

        oracleDAC odec = null;
        StringBuilder sBuilder = null;
        DataTable dTable = null;


        //���� ���� ����Ʈ
        List<string> typeWeatherList = new List<string>();

        //���� ����Ʈ
        List<WDevice> wDeviceList = new List<WDevice>();

        //���� ������ ���� ���� ����Ʈ
        List<string> selectedDeviceList = new List<string>();

        //��ȸ�� �Ͼ �����Ϳ� ���� ����� �����Ѵ�. (���� ���� ����)
        // �������
        ////��ȸ�Ⱓ From  ~ ��ȸ�Ⱓ To
        StringBuilder sBuilderTerm = null;
        ////�����
        string strDeviceName = string.Empty;
        ////��������
        string strTypeWeather = string.Empty;

        //�Ӱ�ġ����
        ////��ȸ�Ⱓ From  ~ ��ȸ�Ⱓ To
        StringBuilder sBuilderTermAlarm = null;
        ////�����
        string strDeviceNameAlarm = string.Empty;
        ////��������
        string strTypeWeatherAlarm = string.Empty;

        //��������
        ////��ȸ�Ⱓ From ~��ȸ�Ⱓ To
        StringBuilder sBuilderTermDevice = null;
        ////�����
        string strDeviceNameDevice = string.Empty;

        //���� �˶� ����
        ////��ȸ�Ⱓ From ~��ȸ�Ⱓ To
        StringBuilder sBuilderTermDeviceAlarm = null;
        ////�����
        string strDeviceNameDeviceAlarm = string.Empty;


        //bool wIsSorted = false; //��� ���� ����

        private WeatherDataMng dataMng = null;

        /// <summary>
        ///�⺻ ������
        /// </summary>
        public RecordsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �̷� ��ȸ ���� �ε� �� ��
        /// </summary>
        private void RecordsForm_Load(object sender, EventArgs e)
        {
            //���ҽ� ����
            string ip = Settings.Default.DbIp;
            string port = Settings.Default.DbPort;
            string id = Settings.Default.DbId;
            string pw = Settings.Default.DbPw;
            string sid = Settings.Default.DbSid;

            //����Ŭ Connection
            this.odec = new ADEng.Library.oracleDAC(id, pw, ip, port, sid);


            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onDBDataSetEvt += new EventHandler<SetDBDataEventArgs>(dataMng_onDBDataSetEvt);

            //�÷� ������ ���� ��� ����
            this.lstWeather.ListViewItemSorter = lvwColumnSorterWeather;
            this.lstAlarm.ListViewItemSorter = lvwColumnSorterAlarm;
            this.lstDevice.ListViewItemSorter = lvwColumnSorterDevice;
            this.lstDeviceAlarm.ListViewItemSorter = lvwColumnSorterDeviceAlarm;

            //���� ���� ����Ʈ ��������
            getTypeWeatherList();

            //���� ����Ʈ ��������
            getDeviceList();

            //��� ����Ʈ ���� ROW  ���� ���� (Tip: Dummy Image �� �־� ���� ����)
            changeRowHeight();

            // ��ȸ ������ Base ���� �ε�
            loadBaseData();

            //��� ����Ʈ���� �Ӽ� Full Row Select
            setListViewFullRowSelect();
        }

        /// <summary>
        /// DB ���� �� ���� �� �߻��ϴ� �̺�Ʈ
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
        ///��� ����Ʈ���� �Ӽ� Full Row Select
        /// </summary>
        private void setListViewFullRowSelect()
        {
            this.lstWeather.FullRowSelect = true;
            this.lstAlarm.FullRowSelect = true;
            this.lstDevice.FullRowSelect = true;
            this.lstDeviceAlarm.FullRowSelect = true;
        }


        /// <summary>
        ///���� ���� ����Ʈ ��������
        /// </summary>
        private void getTypeWeatherList()
        {
            this.typeWeatherList.Add("����");
            this.typeWeatherList.Add("����");
            this.typeWeatherList.Add("����");
        }


        /// <summary>
        ///���� ����Ʈ ��������
        /// </summary>
        private void getDeviceList()
        {
            try
            {
                //��ü �߰�
                this.wDeviceList.Add(new WDevice(0, "0", "��ü", "0", 0, 0, false, "0"));

                if (odec.openDb())
                {
                    //���� ����Ʈ ����ϱ�
                    //����� �߰�
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
        /// ��� ����Ʈ ���� ROW  ���� ����
        /// </summary>
        private void changeRowHeight()
        {
            ImageList dummyImageList = new ImageList();
            dummyImageList.ImageSize = new System.Drawing.Size(1, 20);

            this.lstWeather.SmallImageList = dummyImageList; //������� ����Ʈ�� ����.
            this.lstAlarm.SmallImageList = dummyImageList; // �Ӱ�ġ���� ����Ʈ�� ����.
            this.lstDevice.SmallImageList = dummyImageList; // �������� ����Ʈ�� ����.
            this.lstDeviceAlarm.SmallImageList = dummyImageList; // �˶����� ����Ʈ�� ����.
        }


        /// <summary>
        /// ��ȸ ������ Base ���� �ε�
        /// </summary>
        private void loadBaseData()
        {
            //���� ���� ����Ʈ ComboBox  ���
            foreach (string item in typeWeatherList)
            {
                //��� ���� Tab Page  �� ���� ���� ����Ʈ 
                this.cbxTypeWeather.Items.Add(item);
                // �˶� ���� Tab Page �� ���⸮��Ʈ
                this.cbxTypeWeatherAlarm.Items.Add(item);
            }
            this.cbxTypeWeather.SelectedIndex = 0;
            this.cbxTypeWeatherAlarm.SelectedIndex = 0;

            //���� ����Ʈ Combobox ���
            foreach (WDevice device in wDeviceList)
            {
                // ���� ���� Tab Page �� ���⸮��Ʈ
                this.cbxDeviceNameStatus.Items.Add(device.Name.Trim());
                //���� �˶� Tab Page �� ���⸮��Ʈ
                this.cbxDeviceNameDeviceAlarm.Items.Add(device.Name.Trim());
            }
            //����Ʈ�� ù �׸� �����ϱ�
            this.cbxDeviceNameStatus.SelectedIndex = 0;
            this.cbxDeviceNameDeviceAlarm.SelectedIndex = 0;


            //��ȸ�Ⱓ ���� �ð� ���� 00�� 00�� 00��
            this.dtpFromWeather.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
            this.dtpFromAlarm.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
            this.dtpFromDevice.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
            this.dtpFromDeviceAlarm.Value = Convert.ToDateTime(DateTime.Now.ToString().Substring(0, 10) + " 00:00:00");
        }


        /// <summary>
        /// ��� ���� ��ȸ ��ư Ŭ�� ��, 
        /// </summary>
        private void btnWeatherSearchClick(object sender, EventArgs e)
        {
            //0. ����Ʈ�� ���, ���� Ŭ����
            //lstWeather.Columns.Clear();
            //lstWeather.Items.Clear();
            lstWeather.Clear();

            // �Ⱓ üũ
            if (checkDate(dtpFromWeather, this.dtpToWeather) >= 0)
            {
                switch (cbxTypeWeather.SelectedItem.ToString())
                {
                    case "����":
                        getSelectedDeviceListR();
                        searchRainfallInfo(); //��ȸ
                        break;
                    case "����":
                        getSelectedDeviceListW();
                        searchWaterLevelInfo();//��ȸ
                        break;
                    case "����":
                        getSelectedDeviceListF();
                        searchFlowSpeedInfo();//��ȸ
                        break;
                    default:
                        break;
                }

                //��ȸ���� ���� (�������)
                setWeatherConstraint();
            }
            else
            {
                MessageBox.Show("���۽ð��� ���ð����� Ů�ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    //���� ������ ���� ���� ����Ʈ�� �����Ѵ�.
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
                    //���� ������ ���� ���� ����Ʈ�� �����Ѵ�.
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
                    //���� ������ ���� ���� ����Ʈ�� �����Ѵ�.
                    this.selectedDeviceList.Add(wdevice.PKID.ToString());
                }
            }
        }

        /// <summary>
        ///��ȸ���� ���� (�������)
        /// </summary>
        private void setWeatherConstraint()
        {
            //��ȸ�Ⱓ
            sBuilderTerm = new StringBuilder(100);
            sBuilderTerm.Append(dtpFromWeather.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            sBuilderTerm.Append(dtpToWeather.Value.ToString("yyyy-MM-dd  HH�� mm��"));

            //�����
            strDeviceName = wDeviceList[cbxDeviceNameWeather.SelectedIndex].Name;

            //��������
            strTypeWeather = cbxTypeWeather.SelectedItem.ToString();
        }

        /// <summary>
        /// �Ⱓ üũ
        /// </summary>
        /// <param name="_sdt">���� �ð�</param>
        /// <param name="_edt">���� �ð�</param>
        /// <returns> ���۽ð� ũ��: -1, ������: 0, ������: -1</returns>
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
        /// �������(�췮) ���� ��ȸ
        /// </summary>
        private void searchRainfallInfo()
        {
            //2. ������ ��ȸ
            try
            {
                if (odec.openDb())
                {
                    //���쵥���� ���� �����
                    makeRainfallQuery();

                    dTable = odec.getDataTable(sBuilder.ToString(), "rainfall");
                    int totalCnt = dTable.Rows.Count;

                    if (totalCnt > 0)
                    {

                        //��� �����
                        lstWeather.Columns.Add("��ȣ", 40);
                        lstWeather.Columns.Add("�����ð�", 180);
                        lstWeather.Columns.Add("�����", 120);
                        lstWeather.Columns.Add("10�� ����", 150, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("�̵� 20��", 150, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("����", 150, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("����", 150, HorizontalAlignment.Right);

                        if (totalCnt > 2000)
                        {
                            MessageBox.Show("�������� �ִ� ��� ���� (2000��)�� �ʰ��Ͽ����ϴ�.\n 2000�� �����ͱ��� ��� �˴ϴ�. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            printWeatherLstView(2000);
                        }
                        else
                        {
                            printWeatherLstView(totalCnt);
                        }

                        //��ȣ �� ���
                        for (int i = 0; i < this.lstWeather.Items.Count; i++)
                        {
                            lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                        }
                    }
                    else
                    {
                        lstWeather.Columns.Add("���", lstWeather.Size.Width - 10, HorizontalAlignment.Center);
                        this.lstWeather.Items.Add(new ListViewItem("�����Ͱ� �����ϴ�."));
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
                    chkTime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}", Convert.ToDateTime(dTable.Rows[i - 1]["chktime"].ToString()));
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
        ///���쵥���� ���� �����
        /// </summary>
        private void makeRainfallQuery()
        {
            sBuilder = null;
            dTable = null;
            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);
            sBuilder = new StringBuilder(350);
            //�췮 ���� ���� �����
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
        /// �������(����) ���� ��ȸ
        /// </summary>
        private void searchWaterLevelInfo()
        {
            //2. ������ ��ȸ
            try
            {
                if (odec.openDb())
                {

                    //���� ������ ���� �����
                    makeWaterlevelQuery();

                    dTable = odec.getDataTable(sBuilder.ToString(), "waterlevel");
                    int totalCnt = dTable.Rows.Count;

                    if (totalCnt > 0)
                    {

                        //��������
                        lstWeather.Columns.Add("��ȣ", 40);
                        lstWeather.Columns.Add("�����ð�", 180);
                        lstWeather.Columns.Add("�����", 120);
                        lstWeather.Columns.Add("��", 0, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("���� ����", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("15�� ��ȭ", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("60�� ��ȭ", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("���� ��ȭ", 100, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("���� �ְ�", 100, HorizontalAlignment.Right);

                        if (totalCnt > 2000)
                        {
                            MessageBox.Show("�������� �ִ� ��� ���� (2000��)�� �ʰ��Ͽ����ϴ�.\n 2000�� �����ͱ��� ��� �˴ϴ�. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            printWaterLevelLstView(2000);
                        }
                        else
                        {
                            printWaterLevelLstView(totalCnt);
                        }

                        //��ȣ �� ���
                        for (int i = 0; i < this.lstWeather.Items.Count; i++)
                        {
                            lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                        }

                    }
                    else
                    {
                        lstWeather.Columns.Add("���", lstWeather.Size.Width - 10, HorizontalAlignment.Center);
                        this.lstWeather.Items.Add(new ListViewItem("�����Ͱ� �����ϴ�."));
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
                    chktime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}", Convert.ToDateTime(dTable.Rows[i - 1]["chktime"].ToString()));
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
        ///���� ������ ���� �����
        /// </summary>
        private void makeWaterlevelQuery()
        {
            sBuilder = null;
            dTable = null;

            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            sBuilder = new StringBuilder(350);
            //���� ���� ���� �����
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
        /// �������(����) ���� ��ȸ
        /// </summary>
        private void searchFlowSpeedInfo()
        {

            //2. ������ ��ȸ
            try
            {
                if (odec.openDb())
                {
                    //���� ������ ���� �����
                    makeFlowspeedQuery();

                    dTable = odec.getDataTable(sBuilder.ToString(), "flowspeed");
                    int totalCnt = dTable.Rows.Count;

                    if (totalCnt > 0)
                    {
                        //��� �����
                        lstWeather.Columns.Add("��ȣ", 40);
                        lstWeather.Columns.Add("�����ð�", 180);
                        lstWeather.Columns.Add("�����", 120);
                        lstWeather.Columns.Add("���� ����(m/s)", 120, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("15�� ��ȭ(m/s)", 120, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("60�� ��ȭ(m/s)", 120, HorizontalAlignment.Right);
                        lstWeather.Columns.Add("���� ��ȭ(m/s)", 120, HorizontalAlignment.Right);

                        if (totalCnt > 2000)
                        {
                            MessageBox.Show("�������� �ִ� ��� ���� (2000��)�� �ʰ��Ͽ����ϴ�.\n 2000�� �����ͱ��� ��� �˴ϴ�. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            printFlowSpeedLstView(2000);
                        }
                        else
                        {
                            printFlowSpeedLstView(totalCnt);
                        }
                        //��ȣ �� ���
                        for (int i = 0; i < this.lstWeather.Items.Count; i++)
                        {
                            lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                        }
                    }
                    else
                    {
                        lstWeather.Columns.Add("���", lstWeather.Size.Width - 10, HorizontalAlignment.Center);
                        this.lstWeather.Items.Add(new ListViewItem("�����Ͱ� �����ϴ�."));
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
            lstWeather.Columns.Add("���� �ְ�(m/s)", 120, HorizontalAlignment.Right);

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
                    chktime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}", Convert.ToDateTime(dTable.Rows[i - 1]["chktime"].ToString()));
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
        ///���� ������ ���� �����
        /// </summary>
        private void makeFlowspeedQuery()
        {
            sBuilder = null;
            dTable = null;

            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToWeather.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //���� ���� ���� �����
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
        /// �˶� ���� ��ȸ ��ư Ŭ�� ��, 
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
                    case "����":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                            {
                                this.selectedDeviceList.Add(wdevice.PKID.ToString());
                            }
                        }
                        break;
                    case "����":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                            {
                                this.selectedDeviceList.Add(wdevice.PKID.ToString());
                            }
                        }
                        break;
                    case "����":
                        this.selectedDeviceList.Add("0");
                        foreach (WDevice wdevice in wDeviceList)
                        {
                            if (Convert.ToBoolean((wdevice.HaveSensor >> 2) & 1))
                            {
                                this.selectedDeviceList.Add(wdevice.PKID.ToString());
                            }
                        }
                        break;
                    case "��ü":
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
                        // �˶� ���� �����
                        makeAlarmQuery();

                        dTable = odec.getDataTable(sBuilder.ToString(), "alarmInfo");
                        int totalCnt = dTable.Rows.Count;

                        if (totalCnt > 0)
                        {
                            lstAlarm.Columns.Add("��ȣ", 40);
                            lstAlarm.Columns.Add("�Ӱ�ġ�߻��ð�", 180);
                            lstAlarm.Columns.Add("�����", 120);
                            lstAlarm.Columns.Add("�Ӱ�ġ����", 150, HorizontalAlignment.Center);
                            lstAlarm.Columns.Add("��������", 150, HorizontalAlignment.Center);
                            switch (cbxTypeWeatherAlarm.SelectedItem.ToString())
                            {
                                case "����":
                                    lstAlarm.Columns.Add("������(mm)", 150, HorizontalAlignment.Center);
                                    break;
                                case "����":
                                    lstAlarm.Columns.Add("������(m)", 150, HorizontalAlignment.Center);
                                    break;
                                case "����":
                                    lstAlarm.Columns.Add("������(m/s)", 150, HorizontalAlignment.Center);
                                    break;
                            }
                            lstAlarm.Columns.Add("����", 150, HorizontalAlignment.Center);

                            if (totalCnt > 2000)
                            {
                                MessageBox.Show("�������� �ִ� ��� ���� (2000��)�� �ʰ��Ͽ����ϴ�.\n 2000�� �����ͱ��� ��� �˴ϴ�. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                printAlarmLstView(2000);
                            }
                            else
                            {
                                printAlarmLstView(totalCnt);
                            }

                            //��ȣ �� ���
                            for (int i = 0; i < this.lstAlarm.Items.Count; i++)
                            {
                                lstAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                            }
                        }
                        else
                        {
                            lstAlarm.Columns.Add("���", lstAlarm.Size.Width - 10, HorizontalAlignment.Center);
                            this.lstAlarm.Items.Add(new ListViewItem("�����Ͱ� �����ϴ�."));
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
                //��ȸ���� ���� (�˶�����)
                setAlarmConstraint();

            }
            else
            {
                MessageBox.Show("���۽ð��� ���ð����� Ů�ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    alarmTime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}", Convert.ToDateTime(dTable.Rows[i - 1]["alarmtime"].ToString()));
                }
                if (dTable.Rows[i - 1]["devicename"] != null && dTable.Rows[i - 1]["devicename"].ToString() != "")
                {
                    devicename = dTable.Rows[i - 1]["devicename"].ToString();
                }
                if (dTable.Rows[i - 1]["alarmvalue"] != null && dTable.Rows[i - 1]["alarmvalue"].ToString() != "")
                {
                    switch (cbxTypeWeatherAlarm.SelectedItem.ToString())
                    {
                        case "����":
                            alarmvalue = String.Format("{0:0.0}", (double.Parse(dTable.Rows[i - 1]["alarmvalue"].ToString())) * 0.1);
                            break;
                        case "����":
                            alarmvalue = String.Format("{0:0.00}", (double.Parse(dTable.Rows[i - 1]["alarmvalue"].ToString())) * 0.01);
                            break;
                        case "����":
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
                    case "����":
                        lstAlarm.Items[i - 1].SubItems[4].ForeColor = Color.Orange;
                        break;
                    case "���":
                        lstAlarm.Items[i - 1].SubItems[4].ForeColor = Color.Violet;
                        break;
                    case "����":
                        lstAlarm.Items[i - 1].SubItems[4].ForeColor = Color.Red;
                        break;
                }

                // i++;
            }
        }

        /// <summary>
        ///��ȸ���� ���� (�˶�����)
        /// </summary>
        private void setAlarmConstraint()
        {
            //��ȸ�Ⱓ
            sBuilderTermAlarm = new StringBuilder(100);
            sBuilderTermAlarm.Append(dtpFromAlarm.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            sBuilderTermAlarm.Append(dtpToAlarm.Value.ToString("yyyy-MM-dd  HH�� mm��"));

            //�����
            strDeviceNameAlarm = wDeviceList[cbxDeviceNameAlarm.SelectedIndex].Name;

            //��������
            strTypeWeatherAlarm = cbxTypeWeatherAlarm.SelectedItem.ToString();
        }

        /// <summary>
        /// �˶� ���� �����
        /// </summary>
        private void makeAlarmQuery()
        {
            sBuilder = null;
            dTable = null;

            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);


            //���� ����Ʈ ����ϱ�
            sBuilder = new StringBuilder(1000);

            if (!cbxDeviceNameAlarm.SelectedItem.ToString().Equals("��ü"))
            {
                sBuilder.Append(" SELECT  alarmtime, t.typename alarmtype, s.typename sensortype, alarmvalue, fktypesensor ");
                sBuilder.Append(" , DECODE (realmode, 0,'����', 1,'����') realmode , d.devicename ");
                sBuilder.Append("  FROM alarm al ");
                sBuilder.Append(" JOIN typealarmlevel t ON t.pkid = al.fktypealarmlevel ");
                sBuilder.Append(" JOIN typesensor s ON s.pkid = al.fktypesensor ");
                sBuilder.Append(" JOIN device d ON d.pkid = al.fkdevice ");
                sBuilder.AppendFormat(" WHERE d.isuse = 1 ");
                sBuilder.AppendFormat(" AND al.fkdevice = {0} "
                                                        , selectedDeviceList[cbxDeviceNameAlarm.SelectedIndex].ToString());
            }

            else if (cbxDeviceNameAlarm.SelectedItem.ToString().Equals("��ü"))
            {
                sBuilder.Append(" SELECT  alarmtime, t.typename alarmtype, s.typename sensortype, alarmvalue, fktypesensor ");
                sBuilder.Append(" , DECODE (realmode, 0,'����', 1,'����') realmode ,  d.devicename ");
                sBuilder.Append("  FROM alarm al ");
                sBuilder.Append(" JOIN typealarmlevel t ON t.pkid = al.fktypealarmlevel ");
                sBuilder.Append(" JOIN typesensor s ON s.pkid = al.fktypesensor ");
                sBuilder.Append(" JOIN device d ON d.pkid = al.fkdevice ");
                sBuilder.AppendFormat(" WHERE d.isuse = 1 ");
            }

            switch (cbxTypeWeatherAlarm.SelectedItem.ToString())
            {
                case "����":
                    sBuilder.AppendFormat(" AND fktypesensor = {0} ", "1");
                    break;
                case "����":
                    sBuilder.AppendFormat(" AND fktypesensor = {0} ", "2");
                    break;
                case "����":
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
        /// ������� ��ȸ��ư Ŭ�� ��,
        /// </summary>
        private void btnSearchDevice_Click(object sender, EventArgs e)
        {
            //0. ����Ʈ�� ���, ���� Ŭ����
            lstDevice.Columns.Clear();
            lstDevice.Items.Clear();

            // �Ⱓ üũ
            if (checkDate(dtpFromDevice, dtpToDevice) >= 0)
            {
                try
                {
                    if (odec.openDb())
                    {
                        // �̺�Ʈ ��ȸ ���� ����� (��û DataTable)
                        //makeEventQuery();
                        makeEventQuery2();

                        dTable = odec.getDataTable(sBuilder.ToString(), "devicerequestANDrequest");
                        int totalCnt = dTable.Rows.Count;

                        #region ���� ����Ʈ ����� ����
                        // �̺�Ʈ ��ȸ ���� ����� (���� DataTable)
                        //DataTable dTable2 = new DataTable();

                        //makeEventResponseQuery();

                        // dTable2 = odec.getDataTable(sBuilder.ToString(), "deviceResponse");

                        //����Ʈ�� ����
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
                        #region ���ո���Ʈ�� ����Ͽ������� �ҽ�
                        //if (eventList.Count > 0)
                        //{
                        //    lstDevice.Columns.Add("��ȣ", 40);
                        //    lstDevice.Columns.Add("üũ�ð�", 180);
                        //    lstDevice.Columns.Add("�����", 120);
                        //    lstDevice.Columns.Add("����", 150, HorizontalAlignment.Center);
                        //    lstDevice.Columns.Add("�����׸�", 150);
                        //    lstDevice.Columns.Add("���°�", 100);

                        //    int i = 1;
                        //    string value = string.Empty; //���°�
                        //    string chktime = string.Empty; //�ð�
                        //    string devicename = string.Empty;//�����

                        //    foreach (EventDataType item in eventList)
                        //    {
                        //        value = string.Empty;
                        //        if (item.CheckTime != null && item.CheckTime != "")
                        //        {
                        //            chktime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}"
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
                        //                // "���͸� ����":
                        //                //0:����, 1:�̻�
                        //                case "1":
                        //                    value = item.ChkValue.Equals("0") ? "����" : item.ChkValue.Equals("1") ? "�̻�" : "";
                        //                    break;
                        //                //"�¾����� ����":
                        //                //0: ����, 1: �̻�
                        //                case "2":
                        //                    value = item.ChkValue.Equals("0") ? "����" : item.ChkValue.Equals("1") ? "�̻�" : "";
                        //                    break;
                        //                //"�ð�"
                        //                //����(�ּ�����)
                        //                //case "3":
                        //                //    value = String.Format("{0}��{1}��", dRow["chkvalue"].ToString().Substring(0, 2), dRow["chkvalue"].ToString().Substring(2, 2));
                        //                //    break;
                        //                //"FAN ����":
                        //                //0: �̻� �߻�
                        //                //1: ����(ON)
                        //                //2: ����(OFF)
                        //                case "4":
                        //                    value = item.ChkValue.Equals("0") ? "�̻� �߻�"
                        //                                : item.ChkValue.Equals("1") ? "����(ON)"
                        //                                : item.ChkValue.Equals("2") ? "����(OFF)"
                        //                                : "";
                        //                    break;
                        //                //"���� ����":
                        //                //0: �� ����
                        //                //1: �� ����
                        //                case "5":
                        //                    value = item.ChkValue.Equals("0") ? "�� ����"
                        //                                : item.ChkValue.Equals("1") ? "�� ����"
                        //                                : "";
                        //                    break;
                        //                //"���׳� ����":
                        //                //�������� dbm
                        //                case "6":
                        //                    value = String.Format("{0} dbm", item.ChkValue);
                        //                    break;
                        //                //������ �Ӱ�ġ 1��
                        //                case "7":
                        //                    value = String.Format("{0:0.0} mm", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //������ �Ӱ�ġ 2��
                        //                case "8":
                        //                    value = String.Format("{0} mm", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //������ �Ӱ�ġ 3��
                        //                case "9":
                        //                    value = String.Format("{0} mm", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //������ �Ӱ�ġ 1��
                        //                case "10":
                        //                    value = String.Format("{0:0.00} m", (double.Parse(item.ChkValue)) * 0.01);
                        //                    break;
                        //                //������ �Ӱ�ġ 2��
                        //                case "11":
                        //                    value = String.Format("{0:0.00} m", (double.Parse(item.ChkValue)) * 0.01);
                        //                    break;
                        //                //������ �Ӱ�ġ 3��
                        //                case "12":
                        //                    value = String.Format("{0:0.00} m", (double.Parse(item.ChkValue)) * 0.01);
                        //                    break;
                        //                //���Ӱ� �Ӱ�ġ 1��
                        //                case "13":
                        //                    value = String.Format("{0:0.0} m/s", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���Ӱ� �Ӱ�ġ 2��
                        //                case "14":
                        //                    value = String.Format("{0:0.0} m/s", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���Ӱ� �Ӱ�ġ 3��
                        //                case "15":
                        //                    value = String.Format("{0:0.0} m/s", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���͸� �Ӱ�ġ 1��(����)
                        //                //����(�ּ�����)
                        //                //case "16":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //���͸� �Ӱ�ġ 2��(����)
                        //                //����(�ּ�����)
                        //                //case "17":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //�¾����� 1��(����)
                        //                //����(�ּ�����)
                        //                //case "18":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //�¾����� 2��(����)
                        //                //����(�ּ�����)
                        //                //case "19":
                        //                //    value = String.Format("{0} V",  item.ChkValue);
                        //                //    break;
                        //                //���Ϸ��� ���ýð�
                        //                case "20":
                        //                    value = String.Format("{0} ��", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���ⷹ�� ���ýð�
                        //                case "21":
                        //                    value = String.Format("{0} ��", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //�������
                        //                //����(�ּ�����)
                        //                //case "22":
                        //                //    value =  item.ChkValue.Equals("0") ? "�Ϲݸ��" :  item.ChkValue.Equals("1") ? "�ΰ����ɸ��" : "";
                        //                //    break;
                        //                //���� ���� ����
                        //                //0: ����
                        //                //1: �̻�
                        //                //2: �ռ�
                        //                //3: �ܼ�
                        //                case "23":
                        //                    value = item.ChkValue.Equals("0") ? "����"
                        //                                : item.ChkValue.Equals("1") ? "�̻�"
                        //                                : item.ChkValue.Equals("2") ? "�ռ�"
                        //                                : item.ChkValue.Equals("3") ? "�ܼ�"
                        //                                : "";
                        //                    break;
                        //                //���� ���� ����
                        //                //0: ����
                        //                //1: �̻�
                        //                case "24":
                        //                    value = item.ChkValue.Equals("0") ? "����"
                        //                                : item.ChkValue.Equals("1") ? "�̻�"
                        //                                : "";
                        //                    break;
                        //                //���� ���� ����
                        //                //0: ����
                        //                //1: �̻�
                        //                case "25":
                        //                    value = item.ChkValue.Equals("0") ? "����"
                        //                                : item.ChkValue.Equals("1") ? "�̻�"
                        //                                : "";
                        //                    break;
                        //                //F/W ����
                        //                //�״�� �ֱ�
                        //                case "26":
                        //                    value = item.ChkValue;
                        //                    break;
                        //                //���� ���� ��뿩��
                        //                //���� (�ּ�����)
                        //                //case "27":
                        //                //    value =  item.ChkValue.Equals("0") ? "���" :  item.ChkValue.Equals("1") ? "�̻��" : "";
                        //                //    break;
                        //                //���� ���� ��뿩��
                        //                //���� (�ּ�����)
                        //                //case "28":
                        //                //    value =  item.ChkValue.Equals("0") ? "���" :  item.ChkValue.Equals("1") ? "�̻��" : "";
                        //                //    break;
                        //                //���� ���� ��뿩��
                        //                //���� (�ּ�����)
                        //                //case "29":
                        //                //    value =  item.ChkValue.Equals("0") ? "���" :  item.ChkValue.Equals("1") ? "�̻��" : "";
                        //                //    break;
                        //                //��Ż���
                        //                //���� (�ּ�����)
                        //                //case "30":
                        //                //    value =  item.ChkValue.Equals("0") ? "�������� ���� ����" :  item.ChkValue.Equals("1") ? "TCP/IP ���� " : "SMS ����";
                        //                //    break;
                        //                //A/S �۾� ����
                        //                //����
                        //                case "31":
                        //                    value = item.ChkValue.Equals("0") ? "A/S �۾� ����"
                        //                                : item.ChkValue.Equals("1") ? "A/S �۾� ����"
                        //                                : "";
                        //                    break;
                        //                //RAT ��ü ����
                        //                //������
                        //                case "34":
                        //                    value = string.Empty;
                        //                    break;
                        //                //���� ���� ���� ��û
                        //                //��+ "�ܰ� �Ӱ�ġ" 
                        //                case "38":
                        //                    value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���� ���� ���� ��û
                        //                //��+ "�ܰ� �Ӱ�ġ" 
                        //                case "39":
                        //                    value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���� ���� ���� ��û
                        //                //�� + "�ܰ� �Ӱ�ġ" 
                        //                case "40":
                        //                    value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���� �Ӱ�ġ ��û
                        //                //�� + "�ܰ� �Ӱ�ġ"
                        //                case "41":
                        //                    value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���� �Ӱ�ġ ��û
                        //                //�� + "�ܰ� �Ӱ�ġ"
                        //                case "42":
                        //                    value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���� �Ӱ�ġ ��û
                        //                //�� + "�ܰ� �Ӱ�ġ"
                        //                case "43":
                        //                    value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(item.ChkValue)));
                        //                    break;
                        //                //���ýð���û
                        //                //������
                        //                case "44":
                        //                    value = string.Empty;
                        //                    break;
                        //                //�Ӱ�ġ ���� ����
                        //                //���� (�ּ�ó��)
                        //                //case "45":
                        //                //    value = String.Format("{0}�� ", (int.Parse( item.ChkValue)));
                        //                //    break;
                        //                //���ýð� ���� ����
                        //                //���� (�ּ�ó��)
                        //                //case "46":
                        //                //    value = String.Format("{0}�� ", (int.Parse( item.ChkValue)));
                        //                //    break;
                        //                //���͸� ����
                        //                case "47":
                        //                    value = String.Format("{0:0.0}V ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���͸� ����
                        //                case "48":
                        //                    value = String.Format("{0:0.0}A ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���͸� ����
                        //                case "49":
                        //                    value = String.Format("{0:0.0}m�� ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���͸� �µ�
                        //                case "50":
                        //                    value = String.Format("{0:0.0}�� ", (double.Parse(item.ChkValue)) * 0.1);
                        //                    break;
                        //                //���͸� ����
                        //                //2: ��ȯ
                        //                case "51":
                        //                    value = item.ChkValue.Equals("0") ? "����"
                        //                                 : item.ChkValue.Equals("1") ? "���˿��"
                        //                                 : item.ChkValue.Equals("2") ? "��ȯ"
                        //                                 : "";
                        //                    break;

                        //            }
                        //            #endregion
                        //        }
                        //        //�� ���� chkValue �� ������ �޶�����.

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
                        #region ��û�� �Ͽ��� ���� Request
                        //if (dTable.Rows.Count > 0)
                        //{
                        //    if (dTable.Rows.Count < 1000)
                        //    {
                        //        //��� �����
                        //        ColumnHeaderEx colEx = new ColumnHeaderEx();
                        //        colEx.Text = "";
                        //        colEx.Width = 0;
                        //        colEx.SortType = SortType.NONE;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "��ȣ";
                        //        colEx.Width = 40;
                        //        colEx.SortType = SortType.NONE;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "üũ�ð�";
                        //        colEx.Width = 180;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "�����";
                        //        colEx.Width = 120;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "����";
                        //        colEx.Width = 150;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "�����׸�";
                        //        colEx.Width = 150;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        colEx = new ColumnHeaderEx();
                        //        colEx.Text = "���°�";
                        //        colEx.Width = 100;
                        //        colEx.SortType = SortType.TEXT;
                        //        lstDevice.Columns.Add(colEx);

                        //        //lstDevice.Columns.Add("��ȣ", 40);
                        //        //lstDevice.Columns.Add("üũ�ð�", 150);
                        //        //lstDevice.Columns.Add("�����", 120);
                        //        //lstDevice.Columns.Add("����", 150, HorizontalAlignment.Center);
                        //        //lstDevice.Columns.Add("�����׸�", 150, HorizontalAlignment.Left);
                        //        //lstDevice.Columns.Add("���°�", 100, HorizontalAlignment.Right);


                        //        int i = 1;
                        //        string value = string.Empty; //���°�
                        //        string chktime = string.Empty; //�ð�
                        //        string devicename = string.Empty;//�����
                        //        foreach (DataRow dRow in dTable.Rows)
                        //        {
                        //            value = string.Empty;
                        //            if (dRow["chktime"] != null && dRow["chktime"].ToString() != "")
                        //            {
                        //                chktime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}", Convert.ToDateTime(dRow["chktime"].ToString()));
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
                        //                    // "���͸� ����":
                        //                    //0:����, 1:�̻�
                        //                    case "1":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "����" : dRow["chkvalue"].ToString().Equals("1") ? "�̻�" : "";
                        //                        break;
                        //                    //"�¾����� ����":
                        //                    //0: ����, 1: �̻�
                        //                    case "2":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "����" : dRow["chkvalue"].ToString().Equals("1") ? "�̻� " : "";
                        //                        break;
                        //                    //"�ð�"
                        //                    //����(�ּ�����)
                        //                    //case "3":
                        //                    //    value = String.Format("{0}��{1}��", dRow["chkvalue"].ToString().Substring(0, 2), dRow["chkvalue"].ToString().Substring(2, 2));
                        //                    //    break;
                        //                    //"FAN ����":
                        //                    //0: �̻� �߻�
                        //                    //1: ����(ON)
                        //                    //2: ����(OFF)
                        //                    case "4":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "�̻� �߻�" : dRow["chkvalue"].ToString().Equals("1") ? "����(ON)" : dRow["chkvalue"].ToString().Equals("2") ? "����(OFF)" : "";
                        //                        break;
                        //                    //"���� ����":
                        //                    //0: �� ����
                        //                    //1: �� ����
                        //                    case "5":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "�� ����" : dRow["chkvalue"].ToString().Equals("1") ? "�� ����" : "";
                        //                        break;
                        //                    //"���׳� ����":
                        //                    //�������� dbm
                        //                    case "6":
                        //                        value = String.Format("{0} dbm", dRow["chkvalue"].ToString());
                        //                        break;
                        //                    //������ �Ӱ�ġ 1��
                        //                    case "7":
                        //                        value = String.Format("{0:0.0} mm", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //������ �Ӱ�ġ 2��
                        //                    case "8":
                        //                        value = String.Format("{0} mm", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //������ �Ӱ�ġ 3��
                        //                    case "9":
                        //                        value = String.Format("{0} mm", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //������ �Ӱ�ġ 1��
                        //                    case "10":
                        //                        value = String.Format("{0:0.00} m", (double.Parse(dRow["chkvalue"].ToString())) * 0.01);
                        //                        break;
                        //                    //������ �Ӱ�ġ 2��
                        //                    case "11":
                        //                        value = String.Format("{0:0.00} m", (double.Parse(dRow["chkvalue"].ToString())) * 0.01);
                        //                        break;
                        //                    //������ �Ӱ�ġ 3��
                        //                    case "12":
                        //                        value = String.Format("{0:0.00} m", (double.Parse(dRow["chkvalue"].ToString())) * 0.01);
                        //                        break;
                        //                    //���Ӱ� �Ӱ�ġ 1��
                        //                    case "13":
                        //                        value = String.Format("{0:0.0} m/s", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���Ӱ� �Ӱ�ġ 2��
                        //                    case "14":
                        //                        value = String.Format("{0:0.0} m/s", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���Ӱ� �Ӱ�ġ 3��
                        //                    case "15":
                        //                        value = String.Format("{0:0.0} m/s", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���͸� �Ӱ�ġ 1��(����)
                        //                    //����(�ּ�����)
                        //                    //case "16":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //���͸� �Ӱ�ġ 2��(����)
                        //                    //����(�ּ�����)
                        //                    //case "17":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //�¾����� 1��(����)
                        //                    //����(�ּ�����)
                        //                    //case "18":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //�¾����� 2��(����)
                        //                    //����(�ּ�����)
                        //                    //case "19":
                        //                    //    value = String.Format("{0} V", dRow["chkvalue"].ToString());
                        //                    //    break;
                        //                    //���Ϸ��� ���ýð�
                        //                    case "20":
                        //                        value = String.Format("{0} ��", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���ⷹ�� ���ýð�
                        //                    case "21":
                        //                        value = String.Format("{0} ��", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //�������
                        //                    //����(�ּ�����)
                        //                    //case "22":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "�Ϲݸ��" : dRow["chkvalue"].ToString().Equals("1") ? "�ΰ����ɸ��" : "";
                        //                    //    break;
                        //                    //���� ���� ����
                        //                    //0: ����
                        //                    //1: �̻�
                        //                    //2: �ռ�
                        //                    //3: �ܼ�
                        //                    case "23":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "����" : dRow["chkvalue"].ToString().Equals("1") ? "�̻�" : dRow["chkvalue"].ToString().Equals("2") ? "�ռ�" : dRow["chkvalue"].ToString().Equals("3") ? "�ܼ�" : "";
                        //                        break;
                        //                    //���� ���� ����
                        //                    //0: ����
                        //                    //1: �̻�
                        //                    case "24":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "����" : dRow["chkvalue"].ToString().Equals("1") ? "�̻�" : "";
                        //                        break;
                        //                    //���� ���� ����
                        //                    //0: ����
                        //                    //1: �̻�
                        //                    case "25":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "����" : dRow["chkvalue"].ToString().Equals("1") ? "�̻�" : "";
                        //                        break;
                        //                    //F/W ����
                        //                    //�״�� �ֱ�
                        //                    case "26":
                        //                        value = dRow["chkvalue"].ToString();
                        //                        break;
                        //                    //���� ���� ��뿩��
                        //                    //���� (�ּ�����)
                        //                    //case "27":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "���" : dRow["chkvalue"].ToString().Equals("1") ? "�̻��" : "";
                        //                    //    break;
                        //                    //���� ���� ��뿩��
                        //                    //���� (�ּ�����)
                        //                    //case "28":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "���" : dRow["chkvalue"].ToString().Equals("1") ? "�̻��" : "";
                        //                    //    break;
                        //                    //���� ���� ��뿩��
                        //                    //���� (�ּ�����)
                        //                    //case "29":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "���" : dRow["chkvalue"].ToString().Equals("1") ? "�̻��" : "";
                        //                    //    break;
                        //                    //��Ż���
                        //                    //���� (�ּ�����)
                        //                    //case "30":
                        //                    //    value = dRow["chkvalue"].ToString().Equals("0") ? "�������� ���� ����" : dRow["chkvalue"].ToString().Equals("1") ? "TCP/IP ���� " : "SMS ����";
                        //                    //    break;
                        //                    //A/S �۾� ����
                        //                    //����
                        //                    case "31":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "A/S �۾� ����" : dRow["chkvalue"].ToString().Equals("1") ? "A/S �۾� ����" : "";
                        //                        break;
                        //                    //RAT ��ü ����
                        //                    //������
                        //                    case "34":
                        //                        value = string.Empty;
                        //                        break;
                        //                    //���� ���� ���� ��û
                        //                    //��+ "�ܰ� �Ӱ�ġ" 
                        //                    case "38":
                        //                        value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���� ���� ���� ��û
                        //                    //��+ "�ܰ� �Ӱ�ġ" 
                        //                    case "39":
                        //                        value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���� ���� ���� ��û
                        //                    //�� + "�ܰ� �Ӱ�ġ" 
                        //                    case "40":
                        //                        value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���� �Ӱ�ġ ��û
                        //                    //�� + "�ܰ� �Ӱ�ġ"
                        //                    case "41":
                        //                        value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���� �Ӱ�ġ ��û
                        //                    //�� + "�ܰ� �Ӱ�ġ"
                        //                    case "42":
                        //                        value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���� �Ӱ�ġ ��û
                        //                    //�� + "�ܰ� �Ӱ�ġ"
                        //                    case "43":
                        //                        value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                        break;
                        //                    //���ýð���û
                        //                    //������
                        //                    case "44":
                        //                        value = string.Empty;
                        //                        break;
                        //                    //�Ӱ�ġ ���� ����
                        //                    //���� (�ּ�ó��)
                        //                    //case "45":
                        //                    //    value = String.Format("{0}�� ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                    //    break;
                        //                    //���ýð� ���� ����
                        //                    //���� (�ּ�ó��)
                        //                    //case "46":
                        //                    //    value = String.Format("{0}�� ", (int.Parse(dRow["chkvalue"].ToString())));
                        //                    //    break;
                        //                    //���͸� ����
                        //                    case "47":
                        //                        value = String.Format("{0:0.0}V ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���͸� ����
                        //                    case "48":
                        //                        value = String.Format("{0:0.0}A ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���͸� ����
                        //                    case "49":
                        //                        value = String.Format("{0:0.0}m�� ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���͸� �µ�
                        //                    case "50":
                        //                        value = String.Format("{0:0.0}�� ", (double.Parse(dRow["chkvalue"].ToString())) * 0.1);
                        //                        break;
                        //                    //���͸� ����
                        //                    //2: ��ȯ
                        //                    case "51":
                        //                        value = dRow["chkvalue"].ToString().Equals("0") ? "����" : dRow["chkvalue"].ToString().Equals("1") ? "���˿��" : dRow["chkvalue"].ToString().Equals("2") ? "��ȯ" : "";
                        //                        break;

                        //                }
                        //            }
                        //            //�� ���� chkValue �� ������ �޶�����.

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
                        //        MessageBox.Show("��� �������� �ִ� ������ 1000�� �Դϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    }
                        //} 
                        #endregion
                       
                        if (totalCnt > 0)
                        {
                            //
                            lstDevice.Columns.Add("��ȣ", 40);
                            lstDevice.Columns.Add("üũ�ð�", 180);
                            lstDevice.Columns.Add("�����", 120);
                            lstDevice.Columns.Add("����", 150, HorizontalAlignment.Center);
                            lstDevice.Columns.Add("�����׸�", 180);
                            lstDevice.Columns.Add("���°�", 100);

                            if (totalCnt > 2000)
                            {
                                MessageBox.Show("�������� �ִ� ��� ���� (2000��)�� �ʰ��Ͽ����ϴ�.\n 2000�� �����ͱ��� ��� �˴ϴ�. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                printEventLstView(2000);
                            }
                            else
                            {
                                printEventLstView(totalCnt);
                            }
                            //��ȣ �� ���
                            for (int i = 0; i < this.lstDevice.Items.Count; i++)
                            {
                                lstDevice.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                            }
                            
                        }
                        else
                        {
                            this.lstDevice.Columns.Add("���", lstDevice.Size.Width - 10, HorizontalAlignment.Center);
                            this.lstDevice.Items.Add(new ListViewItem("�����Ͱ� �����ϴ�."));
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
                //��ȸ���� ����
                setDeviceConstraint();
            }
            else
            {
                MessageBox.Show("���۽ð��� ���ð����� Ů�ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printEventLstView(int totalCnt)
        {
            string value = string.Empty; //���°�
            string chktime = string.Empty; //�ð�
            string devicename = string.Empty;//�����

            for (int i = 1; i <= totalCnt; i++)
            {
                value = string.Empty;
                if (dTable.Rows[i - 1]["chkTime"] != null && dTable.Rows[i - 1]["chkTime"].ToString() != "")
                {
                    chktime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}"
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
                        // "���͸� ����":
                        //0:����, 1:�̻�
                        case "1":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //"�¾����� ����":
                        //0: ����, 1: �̻�
                        case "2":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //"�ð�"
                        //����(�ּ�����)
                        //case "3":
                        //    value = String.Format("{0}��{1}��", dRow["chkvalue"].ToString().Substring(0, 2), dRow["chkvalue"].ToString().Substring(2, 2));
                        //    break;
                        //"FAN ����":
                        //0: �̻� �߻�
                        //1: ����(ON)
                        //2: ����(OFF)
                        case "4":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�̻� �߻�"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "����(ON)"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "����(OFF)"
                                        : "";
                            break;
                        //"���� ����":
                        //0: �� ����
                        //1: �� ����
                        case "5":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�� ����"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�� ����"
                                        : "";
                            break;
                        //"���׳� ����":
                        //�������� dbm
                        case "6":
                            value = String.Format("{0} dbm", dTable.Rows[i - 1]["chkValue"].ToString());
                            break;
                        //������ �Ӱ�ġ 1��
                        case "7":
                            value = String.Format("{0:0.0} mm", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //������ �Ӱ�ġ 2��
                        case "8":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //������ �Ӱ�ġ 3��
                        case "9":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //������ �Ӱ�ġ 1��
                        case "10":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.01);
                            break;
                        //������ �Ӱ�ġ 2��
                        case "11":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.01);
                            break;
                        //������ �Ӱ�ġ 3��
                        case "12":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.01);
                            break;
                        //���Ӱ� �Ӱ�ġ 1��
                        case "13":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���Ӱ� �Ӱ�ġ 2��
                        case "14":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���Ӱ� �Ӱ�ġ 3��
                        case "15":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸� �Ӱ�ġ 1��(����)
                        //����(�ּ�����)
                        //case "16":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //���͸� �Ӱ�ġ 2��(����)
                        //����(�ּ�����)
                        //case "17":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //�¾����� 1��(����)
                        //����(�ּ�����)
                        //case "18":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //�¾����� 2��(����)
                        //����(�ּ�����)
                        //case "19":
                        //    value = String.Format("{0} V",   dTable.Rows[i - 1]["chkValue"].ToString());
                        //    break;
                        //���Ϸ��� ���ýð�
                        case "20":
                            value = String.Format("{0} ��", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���ⷹ�� ���ýð�
                        case "21":
                            value = String.Format("{0} ��", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //�������
                        //����(�ּ�����)
                        //case "22":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�Ϲݸ��" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�ΰ����ɸ��" : "";
                        //    break;
                        //���� ���� ����
                        //0: ����
                        //1: �̻�
                        //2: �ռ�
                        //3: �ܼ�
                        case "23":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "�ռ�"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("3") ? "�ܼ�"
                                        : "";
                            break;
                        //���� ���� ����
                        //0: ����
                        //1: �̻�
                        case "24":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�"
                                        : "";
                            break;
                        //���� ���� ����
                        //0: ����
                        //1: �̻�
                        case "25":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�"
                                        : "";
                            break;
                        //F/W ����
                        //�״�� �ֱ�
                        case "26":
                            value = dTable.Rows[i - 1]["chkValue"].ToString();
                            break;
                        //���� ���� ��뿩��
                        //���� (�ּ�����)
                        //case "27":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "���" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻��" : "";
                        //    break;
                        //���� ���� ��뿩��
                        //���� (�ּ�����)
                        //case "28":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "���" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻��" : "";
                        //    break;
                        //���� ���� ��뿩��
                        //���� (�ּ�����)
                        //case "29":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "���" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻��" : "";
                        //    break;
                        //��Ż���
                        //���� (�ּ�����)
                        //case "30":
                        //    value =   dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�������� ���� ����" :   dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "TCP/IP ���� " : "SMS ����";
                        //    break;
                        //A/S �۾� ����
                        //����
                        case "31":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "A/S �۾� ����"
                                        : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "A/S �۾� ����"
                                        : "";
                            break;
                        //RAT ��ü ����
                        //������
                        case "34":
                            value = string.Empty;
                            break;
                        //���� ���� ���� ��û
                        //��+ "�ܰ� �Ӱ�ġ" 
                        case "38":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���� ���� ���� ��û
                        //��+ "�ܰ� �Ӱ�ġ" 
                        case "39":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���� ���� ���� ��û
                        //�� + "�ܰ� �Ӱ�ġ" 
                        case "40":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���� �Ӱ�ġ ��û
                        //�� + "�ܰ� �Ӱ�ġ"
                        case "41":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���� �Ӱ�ġ ��û
                        //�� + "�ܰ� �Ӱ�ġ"
                        case "42":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���� �Ӱ�ġ ��û
                        //�� + "�ܰ� �Ӱ�ġ"
                        case "43":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i - 1]["chkValue"].ToString())));
                            break;
                        //���ýð���û
                        //������
                        case "44":
                            value = string.Empty;
                            break;
                        //�Ӱ�ġ ���� ����
                        //���� (�ּ�ó��)
                        //case "45":
                        //    value = String.Format("{0}�� ", (int.Parse(  dTable.Rows[i - 1]["chkValue"].ToString())));
                        //    break;
                        //���ýð� ���� ����
                        //���� (�ּ�ó��)
                        //case "46":
                        //    value = String.Format("{0}�� ", (int.Parse(  dTable.Rows[i - 1]["chkValue"].ToString())));
                        //    break;
                        //���͸� ����
                        case "47":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸� ����
                        case "48":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸� ����
                        case "49":
                            value = String.Format("{0:0.0}m�� ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸� �µ�
                        case "50":
                            value = String.Format("{0:0.0}�� ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸� ����
                        //2: ��ȯ
                        case "51":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                                         : "";
                            break;
                            //
                        //case "52":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;

                        // ���͸�2 ����
                        case "53":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸�2 ����
                        case "54":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸�2 ����
                        case "55":
                            value = String.Format("{0:0.0}m�� ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                       //���͸�2 �µ�
                        case "56":
                            value = String.Format("{0:0.0}�� ", (double.Parse(dTable.Rows[i - 1]["chkValue"].ToString())) * 0.1);
                            break;
                        //���͸�2 ����
                        case "57":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                                         : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                                         : "";
                            break;
                        //���͸�2 ����
                        case "58":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�1 ���л���
                        case "59":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�1 �µ�����
                        case "60":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�1 ���˽ñ�
                        //case "61":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�1 ��ü�ñ�
                        //case "62":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�1 ��ü(�ʱ�ȭ)
                        //case "63":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�2 ���л���
                        case "64":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�2 �µ�����
                        case "65":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�2 ���˽ñ�
                        //case "66":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�2 ��ü�ñ�
                        //case "67":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�2 ��ü(�ʱ�ȭ)
                        //case "68":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //AC ���� �Է�
                        case "69":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�Է�" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���Է�" : "";
                            break;
                        //�¾����� ���� �Է�
                        case "70":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�Է�" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���Է�" : "";
                            break;
                        //���͸� ���� ����
                        case "71":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���� ��" : "";
                            break;
                        //CDMA RSSI ���� ����
                        //case "72":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //CDMA �ð� ���� �̻�
                        //case "73":
                        //    value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["chkValue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸� �������� ��Ż���
                        case "74":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //�췮�� ������ ���� ����
                        case "75":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̰���" : "";
                            break;
                        //������ ������ ���� ����
                        case "76":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̰���" : "";
                            break;
                        //���Ӱ� ������ ���� ����
                        case "77":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̰���" : "";
                            break;
                        //���͸� ��� ����
                        case "78":
                            value = dTable.Rows[i - 1]["chkValue"].ToString().Equals("0") ? "�����" : dTable.Rows[i - 1]["chkValue"].ToString().Equals("1") ? "�̻�� ��" : "";
                            break;
                    }
                    #endregion
                }
                //�� ���� chkValue �� ������ �޶�����.

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

            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //���� ����Ʈ ����ϱ�
            sBuilder = new StringBuilder(500);
            if (this.cbxDeviceNameStatus.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'����', 0,'��û') iscontrol  ");
                sBuilder.Append("  ,di.itemname, chkvalue FROM devicerequest dr ");
                sBuilder.Append(" JOIN deviceitem di ON di.pkid = dr.fkdeviceitem ");
                sBuilder.Append(" JOIN device d ON d.pkid = dr.fkDevice ");
                sBuilder.Append(" WHERE d.isuse = 1  ");
                sBuilder.AppendFormat(" AND chktime >= to_timestamp('{0}', 'YYYY-MM-DD HH24:MI:SS') "
                                                        , dtpFromDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
                sBuilder.AppendFormat(" AND chktime < to_timestamp('{0}','YYYY-MM-DD HH24:MI:SS') "
                                                        , addMinToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sBuilder.AppendFormat(" UNION ALL ");
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '����'  iscontrol, di.itemname , dr.resvalue   ");
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
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'����', 0,'��û') iscontrol  ");
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
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '����'  iscontrol, di.itemname , dr.resvalue   ");
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

        //����Ʈ�� ����
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
        //                                                            , "����"
        //                                                            , dRow["itemname"].ToString()
        //                                                            , dTable.Rows[i-1]["resvalue"].ToString()));
        //    }
        //    return EventList;
        //}


        /// <summary>
        /// �̺�Ʈ ��ȸ ���� ����� (����)
        /// </summary>
        private void makeEventResponseQuery()
        {
            sBuilder = null;

            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //���� ����Ʈ ����ϱ�
            sBuilder = new StringBuilder(350);
            if (this.cbxDeviceNameStatus.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '����' , di.itemname , dr.resvalue   ");
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
                sBuilder.Append(" SELECT  di.pkid, d.devicename, dr.chktime , '����' , di.itemname , dr.resvalue   ");
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
        /// �̺�Ʈ ��ȸ ���� ����� (��û)
        /// </summary>
        private void makeEventQuery()
        {
            sBuilder = null;
            dTable = null;

            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(dtpToDevice.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);

            //���� ����Ʈ ����ϱ�
            sBuilder = new StringBuilder(350);
            if (this.cbxDeviceNameStatus.SelectedIndex.Equals(0))
            {
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'����', 0,'��û') iscontrol  ");
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
                sBuilder.Append(" SELECT di.pkid, d.devicename, chktime, DECODE (dr.iscontrol, 1,'����', 0,'��û') iscontrol  ");
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
        ///��ȸ���� ���� (�������)
        /// </summary>
        private void setDeviceConstraint()
        {
            //��ȸ�Ⱓ
            sBuilderTermDevice = new StringBuilder(100);
            sBuilderTermDevice.Append(dtpFromDevice.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            sBuilderTermDevice.Append(dtpToDevice.Value.ToString("yyyy-MM-dd  HH�� mm��"));

            //�����
            strDeviceNameDevice = wDeviceList[cbxDeviceNameStatus.SelectedIndex].Name;
        }

        /// <summary>
        /// ������� ��¹�ư Ŭ�� ��,
        /// </summary>
        private void btnPrintWeather_Click(object sender, EventArgs e)
        {
            ////��ȸ�Ⱓ
            //StringBuilder sBuilderTerm = new StringBuilder(100);
            //sBuilderTerm.Append(dtpFromWeather.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            //sBuilderTerm.Append(dtpToWeather.Value.ToString("yyyy-MM-dd  HH�� mm��"));

            ////�����
            //string strDeviceName = wDeviceList[cbxDeviceNameWeather.SelectedIndex].Name;

            ////��������
            //string strTypeWeather = cbxTypeWeather.SelectedItem.ToString();


            if (!lstWeather.Columns.Count.Equals(1)
                && (!lstWeather.Items.Count.Equals(0)))
            {
                //����Ʈ ���� �� View
                fPrint viewForm = null;
                DataTable lstDataTable = null;
                switch (strTypeWeather)
                {
                    case "����":
                        lstDataTable = lstViewToRainfallDataTable();

                        RainfallReport rainfallReport = new RainfallReport("[ ������� �̷� ]"
                                                                                                   , sBuilderTerm.ToString(), strDeviceName, strTypeWeather, lstDataTable);
                        viewForm = new fPrint(rainfallReport, "������� �̷�");
                        break;

                    case "����":
                        lstDataTable = lstViewToWaterLevelDataTable();
                        WaterlevelReport waterlevelReport = new WaterlevelReport("[ ������� �̷� ]"
                                                                                                     , sBuilderTerm.ToString(), strDeviceName, strTypeWeather, lstDataTable);
                        viewForm = new fPrint(waterlevelReport, "������� �̷�");
                        break;

                    case "����":
                        lstDataTable = lstViewToFlowSpeedDataTable();
                        FlowspeedReport flowspeedReport = new FlowspeedReport("[ ������� �̷� ]"
                                                                                                          , sBuilderTerm.ToString(), strDeviceName, strTypeWeather, lstDataTable);
                        viewForm = new fPrint(flowspeedReport, "������� �̷�");
                        break;
                }

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("����� �����Ͱ� �����ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///����Ʈ�� -> DataTable�� ��ȯ (����)
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
        ///����Ʈ�� -> DataTable�� ��ȯ (����)
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
        ///����Ʈ�� -> DataTable�� ��ȯ (����)
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
        /// �˶����� ��¹�ư Ŭ�� ��,
        /// </summary>
        private void btnPrintAlarm_Click(object sender, EventArgs e)
        {
            ////��ȸ�Ⱓ
            //StringBuilder sBuilderTerm = new StringBuilder(100);
            //sBuilderTerm.Append(dtpFromAlarm.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            //sBuilderTerm.Append(dtpToAlarm.Value.ToString("yyyy-MM-dd HH�� mm��"));

            ////�����
            //string strDeviceName = wDeviceList[cbxDeviceNameAlarm.SelectedIndex].Name;

            if (!lstAlarm.Columns.Count.Equals(1)
                            && (!lstAlarm.Items.Count.Equals(0)))
            {
                //����Ʈ ���� �� View
                fPrint viewForm = null;


                //����Ʈ�� -> DataTable�� ��ȯ (�Ӱ�ġ����)
                DataTable lstDataTable = lstViewToAlarmDataTable();


                AlarmReport alarmReport = new AlarmReport("[ �Ӱ�ġ���� �̷� ]"
                                                                          , sBuilderTermAlarm.ToString(), strDeviceNameAlarm, this.strTypeWeatherAlarm, lstDataTable);
                //strTypeWeatherAlarm
                viewForm = new fPrint(alarmReport, "�Ӱ�ġ���� �̷�");

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("����� �����Ͱ� �����ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        ///����Ʈ�� -> DataTable�� ��ȯ (�˶�����)
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
        /// ������� ��¹�ư Ŭ�� ��,
        /// </summary>
        private void btnPrintDevice_Click(object sender, EventArgs e)
        {
            ////��ȸ�Ⱓ
            //StringBuilder sBuilderTerm = new StringBuilder(100);
            //sBuilderTerm.Append(dtpFromDevice.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            //sBuilderTerm.Append(dtpToDevice.Value.ToString("yyyy-MM-dd HH�� mm��"));

            ////�����
            //string strDeviceName = wDeviceList[cbxDeviceNameStatus.SelectedIndex].Name;

            if (!lstDevice.Columns.Count.Equals(1)
                && (!lstDevice.Items.Count.Equals(0)))
            {
                //����Ʈ ���� �� View
                fPrint viewForm = null;
                DataTable lstDataTable = null;
                lstDataTable = lstViewToDeviceDataTable();

                DeviceReport deviceReport = new DeviceReport("[ �̺�Ʈ �̷�]"
                                                                                           , sBuilderTermDevice.ToString(), strDeviceNameDevice, lstDataTable);
                viewForm = new fPrint(deviceReport, "�̺�Ʈ �̷�");

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("����� �����Ͱ� �����ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///����Ʈ�� -> DataTable�� ��ȯ ( �̺�Ʈ )
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
        /// ������� ����Ʈ �� �÷� Ŭ��
        /// </summary>
        private void Weather_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            //Determine if clicked column is already the column that is being sorted.
            if (!e.Column.Equals(0))//��ȣ �÷��� �ƴϸ�
            {
                //�����ϱ�
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


                //��ȣ �����
                for (int i = 0; i < this.lstWeather.Items.Count; i++)
                {
                    lstWeather.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }

           
        }

        /// <summary>
        /// (�˶�����)���� �޺��ڽ� �ε��� ����� �ش� ���� ����Ʈ�� �����ش�.
        /// </summary>
        private void cbxDeviceNameAlarmIndexChanged(object sender, EventArgs e)
        {
            cbxTypeWeatherAlarm.Items.Clear();
            cbxTypeWeatherAlarm.Enabled = true;


            switch (wDeviceList[cbxDeviceNameAlarm.SelectedIndex].HaveSensor.ToString())
            {
                case "1":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "2":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "3":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "4":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "5":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "6":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "7":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    break;
                case "8":
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "9":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "10":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "11":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "12":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "13":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "14":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                case "15":
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("����");
                    cbxTypeWeatherAlarm.Items.Add("ǳ��ǳ��");
                    break;
                default:
                    cbxTypeWeatherAlarm.Items.Add("���� ����");
                    break;
            }
            cbxTypeWeatherAlarm.SelectedIndex = 0; // ù �׸� �����ϱ�
        }

        /// <summary>
        /// �˶����� ����Ʈ �� �÷� Ŭ��
        /// </summary>
        private void Alarm_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Determine if clicked column is already the column that is being sorted.
            if (!e.Column.Equals(0))//��ȣ �÷��� �ƴϸ�
            {
                //�����ϱ�
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

                //��ȣ �� ���
                for (int i = 0; i < this.lstAlarm.Items.Count; i++)
                {
                    lstAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }
          
        }

        /// <summary>
        /// �������� ����Ʈ �� �÷� Ŭ��
        /// </summary>
        private void Device_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Determine if clicked column is already the column that is being sorted.
            if ((!e.Column.Equals(0))
                && (!e.Column.Equals(5)))//������ �÷��� �ƴϸ�(��ȣ, ���°�)
            {
                //�����ϱ�
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
                
                //��ȣ �� ���
                for (int i = 0; i < this.lstDevice.Items.Count; i++)
                {
                    lstDevice.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }
          
        }


        /// <summary>
        /// ��� ���� �ε��� ����� �ش� ���� ����Ʈ�� �����ش�.
        /// </summary>
        private void cbxTypeWeatherIndexChanged(object sender, EventArgs e)
        {
            //���� ������ ���� ������� �����ش�.
            cbxDeviceNameWeather.Items.Clear();

            switch (this.cbxTypeWeather.SelectedItem.ToString())
            {
                case "����":
                    this.cbxDeviceNameWeather.Items.Add("��ü");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                        {
                            this.cbxDeviceNameWeather.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameWeather.Items.Count > 0) this.cbxDeviceNameWeather.SelectedIndex = 0;
                    break;
                case "����":
                    this.cbxDeviceNameWeather.Items.Add("��ü");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                        {
                            this.cbxDeviceNameWeather.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameWeather.Items.Count > 0) this.cbxDeviceNameWeather.SelectedIndex = 0;
                    break;
                case "����":
                    this.cbxDeviceNameWeather.Items.Add("��ü");
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


        //���� ���� �߰� �÷� ������ ����Ǿ��� ��
        private void cbxTypeWeatherAlarm_IndexChanged(object sender, EventArgs e)
        {
            //���� ������ ���� ������� �����ش�.
            cbxDeviceNameAlarm.Items.Clear();
            selectedDeviceList.Clear();

            switch (this.cbxTypeWeatherAlarm.SelectedItem.ToString())
            {
                case "����":
                    this.cbxDeviceNameAlarm.Items.Add("��ü");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 0) & 1))
                        {
                            this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
                case "����":
                    this.cbxDeviceNameAlarm.Items.Add("��ü");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 1) & 1))
                        {
                            this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
                case "����":
                    this.cbxDeviceNameAlarm.Items.Add("��ü");
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        if (Convert.ToBoolean((wdevice.HaveSensor >> 2) & 1))
                        {
                            this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                        }
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
                case "��ü":
                    foreach (WDevice wdevice in wDeviceList)
                    {
                        this.cbxDeviceNameAlarm.Items.Add(wdevice.Name);
                    }
                    if (this.cbxDeviceNameAlarm.Items.Count > 0) this.cbxDeviceNameAlarm.SelectedIndex = 0;
                    break;
            }
        }


        /// <summary>
        /// ���� �˶� ��ȸ ��ư Ŭ�� ��
        /// </summary>
        private void btnSearchDeviceAlarm_Click(object sender, EventArgs e)
        {
            //0. ����Ʈ�� ���, ���� Ŭ����
            lstDeviceAlarm.Columns.Clear();
            lstDeviceAlarm.Items.Clear();

            // �Ⱓ üũ
            if (checkDate(this.dtpFromDeviceAlarm, this.dtpToDeviceAlarm) >= 0)
            {
                try
                {
                    if (odec.openDb())
                    {
                        // ���� �˶� ��ȸ ���� ����� (��û DataTable)
                        makeDeviceAlarmQuery();

                        dTable = odec.getDataTable(sBuilder.ToString(), "deviceAlarm");
                        int totalCnt = dTable.Rows.Count;

                        if (dTable.Rows.Count > 0)
                        {

                            //��� �����
                            this.lstDeviceAlarm.Columns.Add("��ȣ", 40);
                            this.lstDeviceAlarm.Columns.Add("�˶��߻��ð�", 180);
                            this.lstDeviceAlarm.Columns.Add("�����", 120);
                            this.lstDeviceAlarm.Columns.Add("�����׸�", 180);
                            this.lstDeviceAlarm.Columns.Add("���°�", 100);
                            
                            if (totalCnt > 2000)
                            {
                                MessageBox.Show("�������� �ִ� ��� ���� (2000��)�� �ʰ��Ͽ����ϴ�.\n 2000�� �����ͱ��� ��� �˴ϴ�. \t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                printDeviceAlarmLstView(2000);
                            }
                            else
                            {
                                printDeviceAlarmLstView(totalCnt);
                            }

                            //��ȣ �� ���
                            for (int i = 0; i < this.lstDeviceAlarm.Items.Count; i++)
                            {
                                lstDeviceAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                            }
                            
                        }
                        else
                        {
                            this.lstDeviceAlarm.Columns.Add("���", this.lstDeviceAlarm.Size.Width - 10, HorizontalAlignment.Center);
                            this.lstDeviceAlarm.Items.Add(new ListViewItem("�����Ͱ� �����ϴ�."));
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
                //��ȸ���� ����
                setDeviceAlarmConstraint();
            }
            else
            {
                MessageBox.Show("���۽ð��� ���ð����� Ů�ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printDeviceAlarmLstView(int totalCnt)
        {
            //int i = 1;
            string value = string.Empty; //���°�
            string chktime = string.Empty; //�ð�
            string devicename = string.Empty;//�����

            for(int i = 1; i <=totalCnt; i++)
            {
                value = string.Empty;

                if (dTable.Rows[i-1]["chktime"] != null && dTable.Rows[i-1]["chktime"].ToString() != "")
                {
                    chktime = String.Format("{0:yyyy-MM-dd  HH �� mm�� ss��}"
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
                        // "���͸� ����":
                        //0:����, 1:�̻�
                        case "1":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //"�¾����� ����":
                        //0: ����, 1: �̻�
                        case "2":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //"�ð�"
                        //����(�ּ�����)
                        //case "3":
                        //    value = String.Format("{0}��{1}��", dTable.Rows[i-1]["chkvalue"].ToString().Substring(0, 2), dTable.Rows[i-1]["chkvalue"].ToString().Substring(2, 2));
                        //    break;
                        //"FAN ����":
                        //0: �̻� �߻�
                        //1: ����(ON)
                        //2: ����(OFF)
                        case "4":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "�̻� �߻�"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "����(ON)"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("2") ? "����(OFF)"
                                        : "";
                            break;
                        //"���� ����":
                        //0: �� ����
                        //1: �� ����
                        case "5":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "�� ����"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�� ����"
                                        : "";
                            break;
                        //"���׳� ����":
                        //�������� dbm
                        case "6":
                            value = String.Format("{0} dbm", dTable.Rows[i-1]["resvalue"].ToString());
                            break;
                        //������ �Ӱ�ġ 1��
                        case "7":
                            value = String.Format("{0:0.0} mm", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //������ �Ӱ�ġ 2��
                        case "8":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //������ �Ӱ�ġ 3��
                        case "9":
                            value = String.Format("{0} mm", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //������ �Ӱ�ġ 1��
                        case "10":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.01);
                            break;
                        //������ �Ӱ�ġ 2��
                        case "11":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.01);
                            break;
                        //������ �Ӱ�ġ 3��
                        case "12":
                            value = String.Format("{0:0.00} m", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.01);
                            break;
                        //���Ӱ� �Ӱ�ġ 1��
                        case "13":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���Ӱ� �Ӱ�ġ 2��
                        case "14":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���Ӱ� �Ӱ�ġ 3��
                        case "15":
                            value = String.Format("{0:0.0} m/s", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸� �Ӱ�ġ 1��(����)
                        //����(�ּ�����)
                        //case "16":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //���͸� �Ӱ�ġ 2��(����)
                        //����(�ּ�����)
                        //case "17":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //�¾����� 1��(����)
                        //����(�ּ�����)
                        //case "18":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //�¾����� 2��(����)
                        //����(�ּ�����)
                        //case "19":
                        //    value = String.Format("{0} V",  dTable.Rows[i-1]["resvalue"].ToString());
                        //    break;
                        //���Ϸ��� ���ýð�
                        case "20":
                            value = String.Format("{0} ��", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���ⷹ�� ���ýð�
                        case "21":
                            value = String.Format("{0} ��", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //�������
                        //����(�ּ�����)
                        //case "22":
                        //    value =  dRow["resvalue"].ToString().Equals("0") ? "�Ϲݸ��" :  dRow["resvalue"].ToString().Equals("1") ? "�ΰ����ɸ��" : "";
                        //    break;
                        //���� ���� ����
                        //0: ����
                        //1: �̻�
                        //2: �ռ�
                        //3: �ܼ�
                        case "23":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "����"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻�"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("2") ? "�ռ�"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("3") ? "�ܼ�"
                                        : "";
                            break;
                        //���� ���� ����
                        //0: ����
                        //1: �̻�
                        case "24":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "����"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻�"
                                        : "";
                            break;
                        //���� ���� ����
                        //0: ����
                        //1: �̻�
                        case "25":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "����"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻�"
                                        : "";
                            break;
                        //F/W ����
                        //�״�� �ֱ�
                        case "26":
                            value = dTable.Rows[i-1]["resvalue"].ToString();
                            break;
                        //���� ���� ��뿩��
                        //���� (�ּ�����)
                        //case "27":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "���" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻��" : "";
                        //    break;
                        //���� ���� ��뿩��
                        //���� (�ּ�����)
                        //case "28":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "���" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻��" : "";
                        //    break;
                        //���� ���� ��뿩��
                        //���� (�ּ�����)
                        //case "29":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "���" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "�̻��" : "";
                        //    break;
                        //��Ż���
                        //���� (�ּ�����)
                        //case "30":
                        //    value =  dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "�������� ���� ����" :  dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "TCP/IP ���� " : "SMS ����";
                        //    break;
                        //A/S �۾� ����
                        //����
                        case "31":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "A/S �۾� ����"
                                        : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "A/S �۾� ����"
                                        : "";
                            break;
                        //RAT ��ü ����
                        //������
                        case "34":
                            value = string.Empty;
                            break;
                        //���� ���� ���� ��û
                        //��+ "�ܰ� �Ӱ�ġ" 
                        case "38":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���� ���� ���� ��û
                        //��+ "�ܰ� �Ӱ�ġ" 
                        case "39":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���� ���� ���� ��û
                        //�� + "�ܰ� �Ӱ�ġ" 
                        case "40":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���� �Ӱ�ġ ��û
                        //�� + "�ܰ� �Ӱ�ġ"
                        case "41":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���� �Ӱ�ġ ��û
                        //�� + "�ܰ� �Ӱ�ġ"
                        case "42":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���� �Ӱ�ġ ��û
                        //�� + "�ܰ� �Ӱ�ġ"
                        case "43":
                            value = String.Format("{0} �ܰ� �Ӱ�ġ", (int.Parse(dTable.Rows[i-1]["resvalue"].ToString())));
                            break;
                        //���ýð���û
                        //������
                        case "44":
                            value = string.Empty;
                            break;
                        //�Ӱ�ġ ���� ����
                        //���� (�ּ�ó��)
                        //case "45":
                        //    value = String.Format("{0}�� ", (int.Parse( dTable.Rows[i-1]["resvalue"].ToString())));
                        //    break;
                        //���ýð� ���� ����
                        //���� (�ּ�ó��)
                        //case "46":
                        //    value = String.Format("{0}�� ", (int.Parse( dTable.Rows[i-1]["resvalue"].ToString())));
                        //    break;
                        //���͸� ����
                        case "47":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸� ����
                        case "48":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸� ����
                        case "49":
                            value = String.Format("{0:0.0}m�� ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸� �µ�
                        case "50":
                            value = String.Format("{0:0.0}�� ", (double.Parse(dTable.Rows[i-1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸� ����
                        //2: ��ȯ
                        case "51":
                            value = dTable.Rows[i-1]["resvalue"].ToString().Equals("0") ? "����"
                                         : dTable.Rows[i-1]["resvalue"].ToString().Equals("1") ? "���˿��"
                                         : dTable.Rows[i-1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                                         : "";
                            break;
                        //
                        //case "52":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;

                        // ���͸�2 ����
                        case "53":
                            value = String.Format("{0:0.0}V ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸�2 ����
                        case "54":
                            value = String.Format("{0:0.0}A ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸�2 ����
                        case "55":
                            value = String.Format("{0:0.0}m�� ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸�2 �µ�
                        case "56":
                            value = String.Format("{0:0.0}�� ", (double.Parse(dTable.Rows[i - 1]["resvalue"].ToString())) * 0.1);
                            break;
                        //���͸�2 ����
                        case "57":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                                         : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                                         : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                                         : "";
                            break;
                        //���͸�2 ����
                        case "58":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�1 ���л���
                        case "59":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�1 �µ�����
                        case "60":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�1 ���˽ñ�
                        //case "61":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�1 ��ü�ñ�
                        //case "62":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�1 ��ü(�ʱ�ȭ)
                        //case "63":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�2 ���л���
                        case "64":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�2 �µ�����
                        case "65":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //���͸�2 ���˽ñ�
                        //case "66":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�2 ��ü�ñ�
                        //case "67":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸�2 ��ü(�ʱ�ȭ)
                        //case "68":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //AC ���� �Է�
                        case "69":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "�Է�" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���Է�" : "";
                            break;
                        //�¾����� ���� �Է�
                        case "70":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "�Է�" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���Է�" : "";
                            break;
                        //���͸� ���� ����
                        case "71":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���� ��" : "";
                            break;
                        //CDMA RSSI ���� ����
                        //case "72":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //CDMA �ð� ���� �̻�
                        //case "73":
                        //    value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "���˿��"
                        //                 : dTable.Rows[i - 1]["resvalue"].ToString().Equals("2") ? "��ȯ"
                        //                 : "";
                        //    break;
                        //���͸� �������� ��Ż���
                        case "74":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�" : "";
                            break;
                        //�췮�� ������ ���� ����
                        case "75":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̰���" : "";
                            break;
                        //������ ������ ���� ����
                        case "76":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̰���" : "";
                            break;
                        //���Ӱ� ������ ���� ����
                        case "77":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̰���" : "";
                            break;
                        //���͸� ��� ����
                        case "78":
                            value = dTable.Rows[i - 1]["resvalue"].ToString().Equals("0") ? "�����" : dTable.Rows[i - 1]["resvalue"].ToString().Equals("1") ? "�̻�� ��" : "";
                            break;
                    } 
                    #endregion
                }
                //�� ���� resValue �� ������ �޶�����.

                this.lstDeviceAlarm.Items.Add(new ListViewItem(new string[]{i.ToString()
                                                                                       , chktime
                                                                                       , devicename
                                                                                       , dTable.Rows[i-1]["itemname"].ToString()
                                                                                       , value}));
               // i++;
            }
        }

        //��ȸ���� ����
        private void setDeviceAlarmConstraint()
        {
            //��ȸ�Ⱓ
            sBuilderTermDeviceAlarm = new StringBuilder(100);
            sBuilderTermDeviceAlarm.Append(this.dtpFromDeviceAlarm.Value.ToString("yyyy-MM-dd  HH�� mm�� ~ "));
            sBuilderTermDeviceAlarm.Append(this.dtpToDeviceAlarm.Value.ToString("yyyy-MM-dd  HH�� mm��"));

            //�����
            this.strDeviceNameDeviceAlarm = wDeviceList[this.cbxDeviceNameDeviceAlarm.SelectedIndex].Name;
        }

        // ���� �˶� ��ȸ ���� ����� (��û DataTable)
        private void makeDeviceAlarmQuery()
        {
            sBuilder = null;
            dTable = null;
            //�Ⱓ�� �� �ð����� �� �κ��� 0���� ����� 1���� ���Ͽ� �������� �����.
            DateTime addMinToDate = Convert.ToDateTime(this.dtpToDeviceAlarm.Value.ToString("yyyy-MM-dd HH:mm:00"));
            addMinToDate = addMinToDate.AddMinutes(1.0);
            sBuilder = new StringBuilder(350);
            //�췮 ���� ���� �����
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
        /// ���� �˶� ��� ��ư Ŭ�� �� 
        /// </summary>
        private void btnPrintDeviceAlarm_Click(object sender, EventArgs e)
        {
            if (!this.lstDeviceAlarm.Columns.Count.Equals(1)
                            && (!this.lstDeviceAlarm.Items.Count.Equals(0)))
            {
                //����Ʈ ���� �� View
                fPrint viewForm = null;

                //����Ʈ�� -> DataTable�� ��ȯ (���� �˶� ����)
                DataTable lstDataTable = lstViewToDeviceAlarmDataTable();

                DeviceAlarmReport deviceAlarmReport = new DeviceAlarmReport("[ �˶����� �̷� ]"
                                                                                                                    , this.sBuilderTermDeviceAlarm.ToString()
                                                                                                                    , this.strDeviceNameDeviceAlarm
                                                                                                                    , lstDataTable);

                viewForm = new fPrint(deviceAlarmReport, "�˶����� �̷�");

                //Form SHOW
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("����� �����Ͱ� �����ϴ�.\t ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        ///����Ʈ�� -> DataTable�� ��ȯ (���� �˶� ����)
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
                && (!e.Column.Equals(4)))//������ �÷��� �ƴϸ�(��ȣ, ���°�)
            {
                //�����ϱ�
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

                //��ȣ �� ���
                for (int i = 0; i < this.lstDeviceAlarm.Items.Count; i++)
                {
                    lstDeviceAlarm.Items[i].SubItems[0].Text = string.Format("{0}", i + 1);
                }
            }

           
        }

    }
}