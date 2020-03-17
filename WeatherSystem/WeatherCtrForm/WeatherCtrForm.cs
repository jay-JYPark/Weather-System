using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ADEng.Library.WeatherSystem;
using ADEng.Control.WeatherSystem;

namespace ADEng.Module.WeatherSystem
{
    public partial class WeatherCtrForm : Form
    {
        private WeatherDataMng dataMng = null;
        private WeatherControlMng controlForm = null;
        private delegate void InvokeSetRainData(WRainData _wr);
        private delegate void InvokeSetWaterLevelData(WWaterLevelData _wwl);
        private delegate void InvokeSetFlowSpeedData(WFlowSpeedData _wfs);
        private delegate void InvokeSetWDeviceItemData(WDeviceItem _wdi, byte _cmd);

        public WeatherCtrForm()
        {
            InitializeComponent();

            this.dataMng = WeatherDataMng.getInstance();
            this.dataMng.onAddWDeviceEvt += new EventHandler<AddWDeviceEventArgs>(dataMng_onAddWDeviceEvt);
            this.dataMng.onUpdateWDeviceEvt += new EventHandler<UpdateWDeviceEventArgs>(dataMng_onUpdateWDeviceEvt);
            this.dataMng.onDeleteWDeviceEvt += new EventHandler<DeleteWDeviceEventArgs>(dataMng_onDeleteWDeviceEvt);
            this.dataMng.onAddRainDataEvt += new EventHandler<AddRainDataEventArgs>(dataMng_onAddRainDataEvt);
            this.dataMng.onAddWaterLevelDataEvt += new EventHandler<AddWaterLevelEventArgs>(dataMng_onAddWaterLevelDataEvt);
            this.dataMng.onAddFlowSpeedDataEvt += new EventHandler<AddFlowSpeedEventArgs>(dataMng_onAddFlowSpeedDataEvt);
            this.dataMng.onAddWDeviceItemDataEvt += new EventHandler<AddWDeviceItemDataEventArgs>(dataMng_onAddWDeviceItemDataEvt);
            this.dataMng.onLatelyRainDataEvt += new EventHandler<LatelyRainDataEventArgs>(dataMng_onLatelyRainDataEvt);
            this.dataMng.onLatelyWaterLevelEvt += new EventHandler<LatelyWaterLevelDataEventArgs>(dataMng_onLatelyWaterLevelEvt);
            this.dataMng.onLatelyFlowSpeedEvt += new EventHandler<LatelyFlowSpeedDataEventArgs>(dataMng_onLatelyFlowSpeedEvt);
            this.init();
            this.DeviceInit();
            this.dataMng.GetAllLatelyData();
        }

        #region event
        /// <summary>
        /// 가장 최근의 유속 데이터를 주는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onLatelyFlowSpeedEvt(object sender, LatelyFlowSpeedDataEventArgs e)
        {
            if (this.WData3LV.InvokeRequired)
            {
                this.Invoke(new InvokeSetFlowSpeedData(this.SetLVFlowSpeed), new object[] { e.FS });
            }
            else
            {
                this.SetLVFlowSpeed(e.FS);
            }
        }

        /// <summary>
        /// 가장 최근의 수위 데이터를 주는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onLatelyWaterLevelEvt(object sender, LatelyWaterLevelDataEventArgs e)
        {
            if (this.WData2LV.InvokeRequired)
            {
                this.Invoke(new InvokeSetWaterLevelData(this.SetLVWaterLevel), new object[] { e.WL });
            }
            else
            {
                this.SetLVWaterLevel(e.WL);
            }
        }

        /// <summary>
        /// 가장 최근의 강수 데이터를 주는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onLatelyRainDataEvt(object sender, LatelyRainDataEventArgs e)
        {
            if (this.WData1LV.InvokeRequired)
            {
                this.Invoke(new InvokeSetRainData(this.SetLVRainData), new object[] { e.WR });
            }
            else
            {
                this.SetLVRainData(e.WR);
            }
        }

        /// <summary>
        /// RAT 측기가 주는 기상데이터 & 알람데이터 & 측기데이터 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWDeviceItemDataEvt(object sender, AddWDeviceItemDataEventArgs e)
        {
            switch (e.WDI.FKDeviceItem)
            {
                case (uint)WeatherDataMng.WIType.강수임계치1단계:
                case (uint)WeatherDataMng.WIType.강수임계치2단계:
                case (uint)WeatherDataMng.WIType.강수임계치3단계:
                    if (this.WData1LV.InvokeRequired)
                    {
                        this.Invoke(new InvokeSetWDeviceItemData(this.SetLVWDeviceItemData), new object[] { e.WDI, (byte)1 });
                    }
                    else
                    {
                        this.SetLVWDeviceItemData(e.WDI, (byte)1);
                    }
                    break;

                case (uint)WeatherDataMng.WIType.수위임계치1단계:
                case (uint)WeatherDataMng.WIType.수위임계치2단계:
                case (uint)WeatherDataMng.WIType.수위임계치3단계:
                    if (this.WData2LV.InvokeRequired)
                    {
                        this.Invoke(new InvokeSetWDeviceItemData(this.SetLVWDeviceItemData), new object[] { e.WDI, (byte)2 });
                    }
                    else
                    {
                        this.SetLVWDeviceItemData(e.WDI, (byte)2);
                    }
                    break;

                case (uint)WeatherDataMng.WIType.유속임계치1단계:
                case (uint)WeatherDataMng.WIType.유속임계치2단계:
                case (uint)WeatherDataMng.WIType.유속임계치3단계:
                    if (this.WData3LV.InvokeRequired)
                    {
                        this.Invoke(new InvokeSetWDeviceItemData(this.SetLVWDeviceItemData), new object[] { e.WDI, (byte)3 });
                    }
                    else
                    {
                        this.SetLVWDeviceItemData(e.WDI, (byte)3);
                    }
                    break;
            }
        }

        /// <summary>
        /// 유속 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddFlowSpeedDataEvt(object sender, AddFlowSpeedEventArgs e)
        {
            if (this.WData3LV.InvokeRequired)
            {
                this.Invoke(new InvokeSetFlowSpeedData(this.SetLVFlowSpeed), new object[] { e.WFS });
            }
            else
            {
                this.SetLVFlowSpeed(e.WFS);
            }
        }

        /// <summary>
        /// 수위 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWaterLevelDataEvt(object sender, AddWaterLevelEventArgs e)
        {
            if (this.WData2LV.InvokeRequired)
            {
                this.Invoke(new InvokeSetWaterLevelData(this.SetLVWaterLevel), new object[] { e.WWL });
            }
            else
            {
                this.SetLVWaterLevel(e.WWL);
            }
        }

        /// <summary>
        /// 강수 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddRainDataEvt(object sender, AddRainDataEventArgs e)
        {
            if (this.WData1LV.InvokeRequired)
            {
                this.Invoke(new InvokeSetRainData(this.SetLVRainData), new object[] { e.WR });
            }
            else
            {
                this.SetLVRainData(e.WR);
            }
        }

        /// <summary>
        /// 측기의 데이터를 리스트뷰에 셋팅한다.
        /// </summary>
        /// <param name="_wdi"></param>
        private void SetLVWDeviceItemData(WDeviceItem _wdi, byte _cmd)
        {
            switch (_cmd)
            {
                case 1:
                    switch (_wdi.FKDeviceItem)
                    {
                        case (uint)WeatherDataMng.WIType.강수임계치1단계:
                            this.WData1LV.Items[_wdi.FKDevice.ToString()].SubItems[10].Text = string.Format("{0}", double.Parse(_wdi.Value) * 0.1);
                            break;

                        case (uint)WeatherDataMng.WIType.강수임계치2단계:
                            this.WData1LV.Items[_wdi.FKDevice.ToString()].SubItems[11].Text = string.Format("{0}", double.Parse(_wdi.Value) * 0.1);
                            break;

                        case (uint)WeatherDataMng.WIType.강수임계치3단계:
                            this.WData1LV.Items[_wdi.FKDevice.ToString()].SubItems[12].Text = string.Format("{0}", double.Parse(_wdi.Value) * 0.1);
                            break;
                    }
                    break;

                case 2:
                    switch (_wdi.FKDeviceItem)
                    {
                        case (uint)WeatherDataMng.WIType.수위임계치1단계:
                            this.WData2LV.Items[_wdi.FKDevice.ToString()].SubItems[10].Text = string.Format("{0}", double.Parse(_wdi.Value));
                            break;

                        case (uint)WeatherDataMng.WIType.수위임계치2단계:
                            this.WData2LV.Items[_wdi.FKDevice.ToString()].SubItems[11].Text = string.Format("{0}", double.Parse(_wdi.Value));
                            break;

                        case (uint)WeatherDataMng.WIType.수위임계치3단계:
                            this.WData2LV.Items[_wdi.FKDevice.ToString()].SubItems[12].Text = string.Format("{0}", double.Parse(_wdi.Value));
                            break;
                    }
                    break;

                case 3:
                    switch (_wdi.FKDeviceItem)
                    {
                        case (uint)WeatherDataMng.WIType.유속임계치1단계:
                            this.WData3LV.Items[_wdi.FKDevice.ToString()].SubItems[9].Text = string.Format("{0}", double.Parse(_wdi.Value) * 0.1);
                            break;

                        case (uint)WeatherDataMng.WIType.유속임계치2단계:
                            this.WData3LV.Items[_wdi.FKDevice.ToString()].SubItems[10].Text = string.Format("{0}", double.Parse(_wdi.Value) * 0.1);
                            break;

                        case (uint)WeatherDataMng.WIType.유속임계치3단계:
                            this.WData3LV.Items[_wdi.FKDevice.ToString()].SubItems[11].Text = string.Format("{0}", double.Parse(_wdi.Value) * 0.1);
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// 유속 리스트뷰에 데이터를 셋팅한다.
        /// </summary>
        /// <param name="_wfs"></param>
        private void SetLVFlowSpeed(WFlowSpeedData _wfs)
        {
            if (this.WData3LV.Items.ContainsKey(_wfs.FKDevice.ToString()))
            {
                if (_wfs.DDTime.Year == 1 && _wfs.DDTime.Month == 1 && _wfs.DDTime.Day == 1)
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[3].Text = _wfs.DDTimeStr;
                }
                else
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[3].Text = _wfs.DDTime.ToString();
                }

                if (_wfs.RNow != string.Empty)
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[4].Text = string.Format("{0}", double.Parse(_wfs.RNow) * 0.1);
                }

                if (_wfs.R15min != string.Empty)
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[5].Text = string.Format("{0}", double.Parse(_wfs.R15min) * 0.1);
                }

                if (_wfs.R60min != string.Empty)
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[6].Text = string.Format("{0}", double.Parse(_wfs.R60min) * 0.1);
                }

                if (_wfs.TodayMax != string.Empty)
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[7].Text = string.Format("{0}", double.Parse(_wfs.TodayMax) * 0.1);
                }

                if (_wfs.YstdayMax != string.Empty)
                {
                    this.WData3LV.Items[_wfs.FKDevice.ToString()].SubItems[8].Text = string.Format("{0}", double.Parse(_wfs.YstdayMax) * 0.1);
                }
            }
        }

        /// <summary>
        /// 수위 리스트뷰에 데이터를 셋팅한다.
        /// </summary>
        /// <param name="_wwl"></param>
        private void SetLVWaterLevel(WWaterLevelData _wwl)
        {
            if (this.WData2LV.Items.ContainsKey(_wwl.FKDevice.ToString()))
            {
                if (_wwl.DDTime.Year == 1 && _wwl.DDTime.Month == 1 && _wwl.DDTime.Day == 1)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[3].Text = _wwl.DDTimeStr;
                }
                else
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[3].Text = _wwl.DDTime.ToString();
                }

                if (_wwl.WHigh != string.Empty)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[4].Text = string.Format("{0}", double.Parse(_wwl.WHigh) * 10);
                }

                if (_wwl.WNow != string.Empty)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[5].Text = string.Format("{0}", double.Parse(_wwl.WNow));
                }

                if (_wwl.R15min != string.Empty)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[6].Text = string.Format("{0}", double.Parse(_wwl.R15min));
                }

                if (_wwl.R60min != string.Empty)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[7].Text = string.Format("{0}", double.Parse(_wwl.R60min));
                }

                if (_wwl.TodayMax != null)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[8].Text = string.Format("{0}", double.Parse(_wwl.TodayMax));
                }

                if (_wwl.YstdayMax != string.Empty)
                {
                    this.WData2LV.Items[_wwl.FKDevice.ToString()].SubItems[9].Text = string.Format("{0}", double.Parse(_wwl.YstdayMax));
                }
            }
        }

        /// <summary>
        /// 강수 리스트뷰에 데이터를 셋팅한다.
        /// </summary>
        /// <param name="_wr"></param>
        private void SetLVRainData(WRainData _wr)
        {
            if (this.WData1LV.Items.ContainsKey(_wr.FKDevice.ToString()))
            {
                if (_wr.DDTime.Year == 1 && _wr.DDTime.Month == 1 && _wr.DDTime.Day == 1)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[3].Text = _wr.DDTimeStr;
                }
                else
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[3].Text = _wr.DDTime.ToString();
                }

                if (_wr.R10min != string.Empty)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[4].Text = string.Format("{0}", double.Parse(_wr.R10min) * 0.1);
                }

                if (_wr.R15min != string.Empty)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[5].Text = string.Format("{0}", double.Parse(_wr.R15min) * 0.1);
                }

                if (_wr.R20min != string.Empty)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[6].Text = string.Format("{0}", double.Parse(_wr.R20min) * 0.1);
                }

                if (_wr.R60min != string.Empty)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[7].Text = string.Format("{0}", double.Parse(_wr.R60min) * 0.1);
                }

                if (_wr.Today != string.Empty)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[8].Text = string.Format("{0}", double.Parse(_wr.Today) * 0.1);
                }

                if (_wr.Ystday != string.Empty)
                {
                    this.WData1LV.Items[_wr.FKDevice.ToString()].SubItems[9].Text = string.Format("{0}", double.Parse(_wr.Ystday) * 0.1);
                }
            }
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
                if ((e.WDList[i].HaveSensor & 0x01) == 0x01) //강수
                {
                    this.WData1LV.Items.RemoveByKey(e.WDList[i].PKID.ToString());
                }

                if ((e.WDList[i].HaveSensor & 0x02) == 0x02) //수위
                {
                    this.WData2LV.Items.RemoveByKey(e.WDList[i].PKID.ToString());
                }

                if ((e.WDList[i].HaveSensor & 0x04) == 0x04) //유속
                {
                    this.WData3LV.Items.RemoveByKey(e.WDList[i].PKID.ToString());
                }
            }

            this.SetListViewIndex(this.WData1LV);
            this.SetListViewIndex(this.WData2LV);
            this.SetListViewIndex(this.WData3LV);
        }

        /// <summary>
        /// 측기 수정 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onUpdateWDeviceEvt(object sender, UpdateWDeviceEventArgs e)
        {
            if ((e.WD.HaveSensor & 0x01) == 0x01) //강수
            {
                this.WData1LV.Items[e.WD.PKID.ToString()].SubItems[2].Text = e.WD.Name;
            }

            if ((e.WD.HaveSensor & 0x02) == 0x02) //수위
            {
                this.WData2LV.Items[e.WD.PKID.ToString()].SubItems[2].Text = e.WD.Name;
            }

            if ((e.WD.HaveSensor & 0x04) == 0x04) //유속
            {
                this.WData3LV.Items[e.WD.PKID.ToString()].SubItems[2].Text = e.WD.Name;
            }
        }

        /// <summary>
        /// 측기 등록 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataMng_onAddWDeviceEvt(object sender, AddWDeviceEventArgs e)
        {
            if ((e.WD.HaveSensor & 0x01) == 0x01) //강수
            {
                this.WData1LV.Items.Add(this.GetListViewItem(e.WD, this.WData1LV));
            }

            if ((e.WD.HaveSensor & 0x02) == 0x02) //수위
            {
                this.WData2LV.Items.Add(this.GetListViewItem(e.WD, this.WData2LV));
            }

            if ((e.WD.HaveSensor & 0x04) == 0x04) //유속
            {
                this.WData3LV.Items.Add(this.GetListViewItemFS(e.WD, this.WData3LV));
            }
        }
        #endregion

        /// <summary>
        /// 리스트뷰 초기화
        /// </summary>
        private void init()
        {
            #region 리스트뷰 셋팅
            //강수 리스트뷰
            ColumnHeader d1Icon = new ColumnHeader();
            d1Icon.Text = string.Empty;
            d1Icon.Width = 0;
            this.WData1LV.Columns.Add(d1Icon);

            ColumnHeader d1Pkid = new ColumnHeader();
            d1Pkid.Text = "번호";
            d1Pkid.Width = 43;
            this.WData1LV.Columns.Add(d1Pkid);

            ColumnHeader d1Name = new ColumnHeader();
            d1Name.Text = "이름";
            d1Name.Width = 140;
            d1Name.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1Name);

            ColumnHeader d1DTime = new ColumnHeader();
            d1DTime.Text = "수집 시간";
            d1DTime.Width = 150;
            d1DTime.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1DTime);

            ColumnHeader d110Data = new ColumnHeader();
            d110Data.Text = "10분 강수";
            d110Data.Width = 70;
            d110Data.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d110Data);

            ColumnHeader d115Data = new ColumnHeader();
            d115Data.Text = "이동 15분 강수";
            d115Data.Width = 95;
            d115Data.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d115Data);
            
            ColumnHeader d120Data = new ColumnHeader();
            d120Data.Text = "이동 20분 강수";
            d120Data.Width = 95;
            d120Data.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d120Data);

            ColumnHeader d160Data = new ColumnHeader();
            d160Data.Text = "이동 60분 강수";
            d160Data.Width = 95;
            d160Data.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d160Data);

            ColumnHeader d1TodayData = new ColumnHeader();
            d1TodayData.Text = "금일 강수";
            d1TodayData.Width = 70;
            d1TodayData.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1TodayData);

            ColumnHeader d1YstData = new ColumnHeader();
            d1YstData.Text = "전일 강수";
            d1YstData.Width = 70;
            d1YstData.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1YstData);

            ColumnHeader d1Alarm1 = new ColumnHeader();
            d1Alarm1.Text = "1차 임계치";
            d1Alarm1.Width = 75;
            d1Alarm1.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1Alarm1);

            ColumnHeader d1Alarm2 = new ColumnHeader();
            d1Alarm2.Text = "2차 임계치";
            d1Alarm2.Width = 75;
            d1Alarm2.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1Alarm2);

            ColumnHeader d1Alarm3 = new ColumnHeader();
            d1Alarm3.Text = "3차 임계치";
            d1Alarm3.Width = 75;
            d1Alarm3.TextAlign = HorizontalAlignment.Center;
            this.WData1LV.Columns.Add(d1Alarm3);

            //수위 리스트뷰
            ColumnHeader d2Icon = new ColumnHeader();
            d2Icon.Text = string.Empty;
            d2Icon.Width = 0;
            this.WData2LV.Columns.Add(d2Icon);

            ColumnHeader d2Pkid = new ColumnHeader();
            d2Pkid.Text = "번호";
            d2Pkid.Width = 43;
            this.WData2LV.Columns.Add(d2Pkid);

            ColumnHeader d2Name = new ColumnHeader();
            d2Name.Text = "이름";
            d2Name.Width = 140;
            d2Name.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2Name);

            ColumnHeader d2DTime = new ColumnHeader();
            d2DTime.Text = "수집 시간";
            d2DTime.Width = 150;
            d2DTime.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2DTime);

            ColumnHeader d2HData = new ColumnHeader();
            d2HData.Text = "고도";
            d2HData.Width = 60;
            d2HData.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2HData);

            ColumnHeader d2NowData = new ColumnHeader();
            d2NowData.Text = "현재 수위";
            d2NowData.Width = 70;
            d2NowData.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2NowData);

            ColumnHeader d215Data = new ColumnHeader();
            d215Data.Text = "15분 수위 변화량";
            d215Data.Width = 110;
            d215Data.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d215Data);

            ColumnHeader d260Data = new ColumnHeader();
            d260Data.Text = "60분 수위 변화량";
            d260Data.Width = 110;
            d260Data.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d260Data);

            ColumnHeader d2TodayData = new ColumnHeader();
            d2TodayData.Text = "금일 최고수위";
            d2TodayData.Width = 95;
            d2TodayData.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2TodayData);

            ColumnHeader d2YstData = new ColumnHeader();
            d2YstData.Text = "전일 최고수위";
            d2YstData.Width = 95;
            d2YstData.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2YstData);

            ColumnHeader d2Alarm1 = new ColumnHeader();
            d2Alarm1.Text = "1차 임계치";
            d2Alarm1.Width = 75;
            d2Alarm1.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2Alarm1);

            ColumnHeader d2Alarm2 = new ColumnHeader();
            d2Alarm2.Text = "2차 임계치";
            d2Alarm2.Width = 75;
            d2Alarm2.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2Alarm2);

            ColumnHeader d2Alarm3 = new ColumnHeader();
            d2Alarm3.Text = "3차 임계치";
            d2Alarm3.Width = 75;
            d2Alarm3.TextAlign = HorizontalAlignment.Center;
            this.WData2LV.Columns.Add(d2Alarm3);

            //유속 리스트뷰
            ColumnHeader d3Icon = new ColumnHeader();
            d3Icon.Text = string.Empty;
            d3Icon.Width = 0;
            this.WData3LV.Columns.Add(d3Icon);

            ColumnHeader d3Pkid = new ColumnHeader();
            d3Pkid.Text = "번호";
            d3Pkid.Width = 43;
            this.WData3LV.Columns.Add(d3Pkid);

            ColumnHeader d3Name = new ColumnHeader();
            d3Name.Text = "이름";
            d3Name.Width = 140;
            d3Name.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3Name);

            ColumnHeader d3DTime = new ColumnHeader();
            d3DTime.Text = "수집 시간";
            d3DTime.Width = 150;
            d3DTime.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3DTime);

            ColumnHeader d3NowData = new ColumnHeader();
            d3NowData.Text = "현재 유속";
            d3NowData.Width = 70;
            d3NowData.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3NowData);

            ColumnHeader d315Data = new ColumnHeader();
            d315Data.Text = "15분 유속";
            d315Data.Width = 70;
            d315Data.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d315Data);

            ColumnHeader d360Data = new ColumnHeader();
            d360Data.Text = "60분 유속";
            d360Data.Width = 70;
            d360Data.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d360Data);

            ColumnHeader d3TodayData = new ColumnHeader();
            d3TodayData.Text = "금일 최고유속";
            d3TodayData.Width = 95;
            d3TodayData.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3TodayData);

            ColumnHeader d3YstData = new ColumnHeader();
            d3YstData.Text = "전일 최고유속";
            d3YstData.Width = 95;
            d3YstData.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3YstData);

            ColumnHeader d3Alarm1 = new ColumnHeader();
            d3Alarm1.Text = "1차 임계치";
            d3Alarm1.Width = 75;
            d3Alarm1.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3Alarm1);

            ColumnHeader d3Alarm2 = new ColumnHeader();
            d3Alarm2.Text = "2차 임계치";
            d3Alarm2.Width = 75;
            d3Alarm2.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3Alarm2);

            ColumnHeader d3Alarm3 = new ColumnHeader();
            d3Alarm3.Text = "3차 임계치";
            d3Alarm3.Width = 75;
            d3Alarm3.TextAlign = HorizontalAlignment.Center;
            this.WData3LV.Columns.Add(d3Alarm3);
            #endregion
        }

        /// <summary>
        /// 측기 리스트 초기화
        /// </summary>
        private void DeviceInit()
        {
            for (int i = 0; i < this.dataMng.DeviceList.Count; i++)
            {
                if ((this.dataMng.DeviceList[i].HaveSensor & 0x01) == 0x01) //강수
                {
                    this.WData1LV.Items.Add(this.GetListViewItem(this.dataMng.DeviceList[i], this.WData1LV));
                }

                if ((this.dataMng.DeviceList[i].HaveSensor & 0x02) == 0x02) //수위
                {
                    this.WData2LV.Items.Add(this.GetListViewItem(this.dataMng.DeviceList[i], this.WData2LV));
                }

                if ((this.dataMng.DeviceList[i].HaveSensor & 0x04) == 0x04) //유속
                {
                    this.WData3LV.Items.Add(this.GetListViewItemFS(this.dataMng.DeviceList[i], this.WData3LV));
                }
            }
        }

        /// <summary>
        /// WDevice 클래스를 받아 LisetViewItem으로 반환한다. (강수, 수위)
        /// </summary>
        /// <param name="_wd"></param>
        /// <returns></returns>
        private ListViewItem GetListViewItem(WDevice _wd, ListView _lv)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Name = string.Format("{0}", _wd.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", _lv.Items.Count + 1));
            lvi.SubItems.Add(_wd.Name);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);

            return lvi;
        }

        /// <summary>
        /// WDevice 클래스를 받아 LisetViewItem으로 반환한다. (유속)
        /// </summary>
        /// <param name="_wd"></param>
        /// <returns></returns>
        private ListViewItem GetListViewItemFS(WDevice _wd, ListView _lv)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Name = string.Format("{0}", _wd.PKID);
            lvi.Text = string.Empty;
            lvi.SubItems.Add(string.Format("{0}", _lv.Items.Count + 1));
            lvi.SubItems.Add(_wd.Name);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);
            lvi.SubItems.Add(string.Empty);

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

        //상태요청 버튼 클릭
        private void RequestBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.DeviceIndexLV.Items.Count > 0)
                {
                    WaitBarMng.Start();
                    Thread.Sleep(2000);

                    for (int i = 0; i < this.DeviceIndexLV.Items.Count; i++)
                    {
                        WDevice tmpDevice = this.dataMng.GetWDevice(uint.Parse(this.DeviceIndexLV.Items[i].Name));

                        if (tmpDevice.EthernetUse)
                        {
                            //DB 저장
                            WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.ALL;
                            WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                            WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                            this.dataMng.AddDeviceRequest(tmp);

                            CProto00 cProto00 = CProtoMng.GetProtoObj("00") as CProto00;
                            cProto00.Header = "[";
                            cProto00.Length = "021";
                            cProto00.ID = tmpDevice.ID;
                            cProto00.MainCode = "0";
                            cProto00.SubCode = "a";
                            cProto00.RecvType = "3";
                            cProto00.CRC = "00000";
                            cProto00.Tail = "]";

                            byte[] buff = cProto00.MakeProto();
                            this.dataMng.SendEthernetMsg(tmpDevice.ID, buff);
                        }
                        else
                        {
                            for (int j = 0; j < this.dataMng.TypeDeviceList.Count; j++)
                            {
                                if (tmpDevice.TypeDevice == this.dataMng.TypeDeviceList[j].PKID)
                                {
                                    if (this.dataMng.TypeDeviceList[j].Name == "RAT") //RAT에 전체 상태 요청하는 경우
                                    {
                                        //DB 저장
                                        WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.ALL;
                                        WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                        WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                                        this.dataMng.AddDeviceRequest(tmp);

                                        CProto00 cProto00 = CProtoMng.GetProtoObj("00") as CProto00;
                                        cProto00.Header = "[";
                                        cProto00.Length = "021";
                                        cProto00.ID = tmpDevice.ID;
                                        cProto00.MainCode = "0";
                                        cProto00.SubCode = "a";
                                        cProto00.RecvType = "1";
                                        cProto00.CRC = "00000";
                                        cProto00.Tail = "]";

                                        byte[] buff = cProto00.MakeProto();
                                        this.dataMng.SendSmsMsg(tmpDevice.CellNumber, buff);
                                        Thread.Sleep(20);
                                    }
                                    else if (this.dataMng.TypeDeviceList[j].Name == "WOU") //WOU에 상태 요청하는 경우
                                    {
                                        //DB 저장
                                        WeatherDataMng.WIType wiTypeAll = WeatherDataMng.WIType.ALL;
                                        WTypeDeviceItem AllTypeDeviceItem = this.dataMng.GetTypeDeviceItem(wiTypeAll);
                                        WDeviceRequest tmp = new WDeviceRequest(0, tmpDevice.PKID, AllTypeDeviceItem.PKID, DateTime.Now, 0, string.Empty);
                                        this.dataMng.AddDeviceRequest(tmp);
                                        this.dataMng.WOUWDeviceRequest(tmpDevice.PKID); //serial로 요청한다.
                                    }
                                }
                            }
                        }
                    }

                    WaitBarMng.Close();
                }
                else
                {
                    MessageBox.Show("선택한 측기가 없습니다.", "상태 요청", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherCtrForm.RequestBtn_Click() - {0}", ex.Message));
                WaitBarMng.Close();
            }
        }

        //제어 버튼 클릭
        private void ControlBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<uint> WDeviceSelected = new List<uint>();

                for (int i = 0; i < this.DeviceIndexLV.Items.Count; i++)
                {
                    WDeviceSelected.Add(uint.Parse(this.DeviceIndexLV.Items[i].Name));
                }

                this.controlForm = new WeatherControlMng(WDeviceSelected);
                this.controlForm.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherCtrForm.ControlBtn_Click() - {0}", ex.Message));
                WaitBarMng.Close();
            }
        }

        //강수 리스트뷰 아이템 더블 클릭
        private void WData1LV_DoubleClick(object sender, EventArgs e)
        {
            WDevice tmpDevice = this.dataMng.GetWDevice(uint.Parse(this.WData1LV.SelectedItems[0].Name));

            if (this.dataMng.GetTypeDevice(tmpDevice.TypeDevice).Name == "HSD" ||
                this.dataMng.GetTypeDevice(tmpDevice.TypeDevice).Name == "DSD")
            {
                MessageBox.Show("타 측기 시스템은 선택할 수 없습니다.", "측기 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!(this.DeviceIndexLV.Items.ContainsKey(tmpDevice.PKID.ToString())))
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Name = tmpDevice.PKID.ToString();
                    lvi.Text = string.Format("{0}(ID:{1})", tmpDevice.Name, tmpDevice.ID);
                    this.DeviceIndexLV.Items.Add(lvi);
                }
            }
        }

        //수위 리스트뷰 아이템 더블 클릭
        private void WData2LV_DoubleClick(object sender, EventArgs e)
        {
            WDevice tmpDevice = this.dataMng.GetWDevice(uint.Parse(this.WData2LV.SelectedItems[0].Name));

            if (this.dataMng.GetTypeDevice(tmpDevice.TypeDevice).Name == "HSD" ||
                this.dataMng.GetTypeDevice(tmpDevice.TypeDevice).Name == "DSD")
            {
                MessageBox.Show("타 측기 시스템은 선택할 수 없습니다.", "측기 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!(this.DeviceIndexLV.Items.ContainsKey(tmpDevice.PKID.ToString())))
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Name = tmpDevice.PKID.ToString();
                    lvi.Text = string.Format("{0}(ID:{1})", tmpDevice.Name, tmpDevice.ID);
                    this.DeviceIndexLV.Items.Add(lvi);
                }
            }
        }

        //유속 리스트뷰 아이템 더블 클릭
        private void WData3LV_DoubleClick(object sender, EventArgs e)
        {
            WDevice tmpDevice = this.dataMng.GetWDevice(uint.Parse(this.WData3LV.SelectedItems[0].Name));

            if (this.dataMng.GetTypeDevice(tmpDevice.TypeDevice).Name == "HSD" ||
                this.dataMng.GetTypeDevice(tmpDevice.TypeDevice).Name == "DSD")
            {
                MessageBox.Show("타 측기 시스템은 선택할 수 없습니다.", "측기 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!(this.DeviceIndexLV.Items.ContainsKey(tmpDevice.PKID.ToString())))
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Name = tmpDevice.PKID.ToString();
                    lvi.Text = string.Format("{0}(ID:{1})", tmpDevice.Name, tmpDevice.ID);
                    this.DeviceIndexLV.Items.Add(lvi);
                }
            }
        }

        //측기 선택 리스트뷰 아이템 더블 클릭
        private void DeviceIndexLV_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.DeviceIndexLV.Items.RemoveByKey(this.DeviceIndexLV.SelectedItems[0].Name.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherCtrForm.DeviceIndexLV_DoubleClick - {0}", ex.Message));
            }
        }
    }
}