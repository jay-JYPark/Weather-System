using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ADEng.Library.WeatherSystem;

namespace ADEng.Module.WeatherSystem
{
    public partial class DeviceMng : Form
    {
        private AddWDevice addWDevice = null;
        private WeatherDataMng dataMng = null;

        public DeviceMng()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.init();
            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onAddWDeviceEvt += new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onUpdateWDeviceEvt += new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt += new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
            this.DeviceInit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.dataMng.onAddWDeviceEvt -= new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onUpdateWDeviceEvt -= new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt -= new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
        }

        /// <summary>
        /// 측기 삭제 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onDeleteWDeviceEvt(object sender, DeleteWDeviceEventArgs e)
        {
            for (int i = 0; i < e.WDList.Count; i++)
            {
                this.WDeviceLV.Items.RemoveByKey(e.WDList[i].PKID.ToString());
            }

            this.SetListViewIndex(this.WDeviceLV);
        }

        /// <summary>
        /// 측기 수정 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onUpdateWDeviceEvt(object sender, UpdateWDeviceEventArgs e)
        {
            try
            {
                this.WDeviceLV.Items[e.WD.PKID.ToString()].SubItems[2].Text = this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name;
                this.WDeviceLV.Items[e.WD.PKID.ToString()].SubItems[3].Text = e.WD.ID;
                this.WDeviceLV.Items[e.WD.PKID.ToString()].SubItems[4].Text = e.WD.Name;
                this.WDeviceLV.Items[e.WD.PKID.ToString()].SubItems[5].Text = e.WD.CellNumber;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("DeviceMng.dataMng_onUpdateWDeviceEvt() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 측기 등록 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWDeviceEvt(object sender, AddWDeviceEventArgs e)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Name = string.Format("{0}", e.WD.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", this.WDeviceLV.Items.Count + 1));
            lvi.SubItems.Add(this.dataMng.GetTypeDevice(e.WD.TypeDevice).Name);
            lvi.SubItems.Add(e.WD.ID);
            lvi.SubItems.Add(e.WD.Name);
            lvi.SubItems.Add(e.WD.CellNumber);

            this.WDeviceLV.Items.Add(lvi);
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void init()
        {
            #region 리스트뷰 초기화
            ColumnHeader dIcon = new ColumnHeader();
            dIcon.Text = string.Empty;
            dIcon.Width = 0;
            this.WDeviceLV.Columns.Add(dIcon);

            ColumnHeader dPkid = new ColumnHeader();
            dPkid.Text = "번호";
            dPkid.Width = 43;
            this.WDeviceLV.Columns.Add(dPkid);

            ColumnHeader dDivision = new ColumnHeader();
            dDivision.Text = "식별자";
            dDivision.Width = 55;
            dDivision.TextAlign = HorizontalAlignment.Center;
            this.WDeviceLV.Columns.Add(dDivision);

            ColumnHeader dId = new ColumnHeader();
            dId.Text = "ID";
            dId.Width = 105;
            dId.TextAlign = HorizontalAlignment.Center;
            this.WDeviceLV.Columns.Add(dId);

            ColumnHeader dName = new ColumnHeader();
            dName.Text = "이름";
            dName.Width = 110;
            dName.TextAlign = HorizontalAlignment.Center;
            this.WDeviceLV.Columns.Add(dName);

            ColumnHeader dTelNum = new ColumnHeader();
            dTelNum.Text = "전화번호";
            dTelNum.Width = 100;
            dTelNum.TextAlign = HorizontalAlignment.Center;
            this.WDeviceLV.Columns.Add(dTelNum);
            #endregion
        }

        //등록 버튼 클릭
        private void AddBtn_Click(object sender, EventArgs e)
        {
            using (this.addWDevice = new AddWDevice())
            {
                this.addWDevice.Mode = 0;
                this.addWDevice.ShowDialog();
            }
        }

        //수정 버튼 클릭
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (this.WDeviceLV.SelectedItems.Count != 1)
            {
                MessageBox.Show("수정할 한 개의 측기를 선택하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (this.addWDevice = new AddWDevice())
            {
                this.addWDevice.Mode = 1;
                WDevice wd = this.dataMng.GetWDevice(uint.Parse(this.WDeviceLV.SelectedItems[0].Name));
                this.addWDevice.WDivision = string.Format("{0}({1})",
                    this.dataMng.GetTypeDevice(wd.TypeDevice).Name,
                    this.dataMng.GetTypeDevice(wd.TypeDevice).Remark);
                this.addWDevice.WDIDTB = wd.ID;
                this.addWDevice.WDNameTB = wd.Name;
                this.addWDevice.WDTelTB = wd.CellNumber;
                this.addWDevice.WDPKID = this.WDeviceLV.SelectedItems[0].Name;
                this.addWDevice.WDSensor = wd.HaveSensor;
                this.addWDevice.WDEthernetUse = wd.EthernetUse;
                this.addWDevice.WDeviceRemark = wd.Remark;
                this.addWDevice.ShowDialog();
            }
        }

        //삭제 버튼 클릭
        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (this.WDeviceLV.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 측기를 선택하세요.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show("측기를 삭제하겠습니까?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                List<WDevice> wdList = new List<WDevice>();
                WDevice wd;

                for (int i = 0; i < this.WDeviceLV.SelectedItems.Count; i++)
                {
                    wd = new WDevice();
                    wd = dataMng.GetWDevice(uint.Parse(this.WDeviceLV.SelectedItems[i].Name));
                    wdList.Add(wd);
                }

                this.dataMng.DeleteWDevice(wdList);
            }
        }

        //닫기 버튼 클릭
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 측기 리스트 초기화
        /// </summary>
        private void DeviceInit()
        {
            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                this.WDeviceLV.Items.Add(this.GetListViewItem(this.dataMng.DeviceList[i]));
            }
        }

        /// <summary>
        /// WDevice 클래스를 받아 LisetViewItem으로 반환한다.
        /// </summary>
        /// <param name="_wd"></param>
        /// <returns></returns>
        private ListViewItem GetListViewItem(WDevice _wd)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Name = string.Format("{0}", _wd.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", this.WDeviceLV.Items.Count + 1));
            lvi.SubItems.Add(this.dataMng.GetTypeDevice(_wd.TypeDevice).Name);
            lvi.SubItems.Add(_wd.ID);
            lvi.SubItems.Add(_wd.Name);
            lvi.SubItems.Add(_wd.CellNumber);

            return lvi;
        }

        /// <summary>
        /// 리스트뷰의 인덱스를 처음부터 다시 셋팅한다.
        /// </summary>
        /// <param name="lv"></param>
        public void SetListViewIndex(ListView lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                int c = i + 1;
                lv.Items[i].SubItems[1].Text = c.ToString();
            }
        }
    }
}