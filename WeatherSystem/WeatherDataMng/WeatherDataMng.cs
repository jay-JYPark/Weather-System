using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using ADEng.Library;
using ADEng.Library.WeatherSystem;

namespace ADEng.Module.WeatherSystem
{
    public class WeatherDataMng :IDisposable
    {
        private delegate void DBConEventArgsHandler(object sender, DBConEventArgs dbcea);
        private delegate void DBConTestEventArgsHandler(object sender, DBConTestEventArgs dbctea);
        private delegate void AddWDeviceEventArgsHandler(object sender, AddWDeviceEventArgs awdea);
        private delegate void UpdateWDeviceEventArgsHandler(object sender, UpdateWDeviceEventArgs udea);
        private delegate void DeleteWDeviceEventArgsHandler(object sender, DeleteWDeviceEventArgs ddea);
        private delegate void AddRainDataEventArgsHandler(object sender, AddRainDataEventArgs ardea);
        private delegate void AddWaterLevelEventArgsHandler(object sender, AddWaterLevelEventArgs awlea);
        private delegate void AddFlowSpeedEventArgsHandler(object sender, AddFlowSpeedEventArgs afsea);
        private delegate void AddAlarmEventArgsHandler(object sender, AddAlarmEventArgs aaea);
        private delegate void AddWDeviceItemDataEventArgsHandler(object sender, AddWDeviceItemDataEventArgs awdidea);
        private delegate void AddWDeviceRequestEventArgsHandler(object sender, AddWDeviceRequestEventArgs awdrea);
        private delegate void SendSmsMsgEventArgsHandler(object sender, SendSmsMsgEventArgs ssmea);
        private delegate void WOUWDeviceRequestEventArgsHandler(object sender, WOUWDeviceRequestEventArgs wwdrea);
        private delegate void WOUWDeviceControlAlarmEventArgsHandler(object sender, WOUWDeviceControlAlarmEventArgs wwdcaea);
        private delegate void WOUWDeviceControlFTimeEventArgsHandler(object sender, WOUWDeviceControlFTimeEventArgs wwdcftea);
        private delegate void LatelyRainDataEventArgsHandler(object sender, LatelyRainDataEventArgs lrdea);
        private delegate void LatelyWaterLevelDataEventArgsHandler(object sender, LatelyWaterLevelDataEventArgs lwldea);
        private delegate void LatelyFlowSpeedDataEventArgsHandler(object sender, LatelyFlowSpeedDataEventArgs lfsdea);
        private delegate void AddSmsUserEventArgsHandler(object sender, AddSmsUserEventArgs asuea);
        private delegate void UpdateSmsUserEventArgsHandler(object sender, UpdateSmsUserEventArgs usuea);
        private delegate void DelSmsUserEventArgsHandler(object sender, DelSmsUserEventArgs dsuea);
        private delegate void SetDBDataEventArgsHandler(object sender, SetDBDataEventArgs sdbdea);
        private delegate void WDeviceAlarmItemsEventArgsHandler(object sender, WDeviceAlarmItemsEventArgs wdaiea);
        private delegate void EthernetClientSendEventArgsHandler(object sender, EthernetClientSendEventArgs ecsea);

        public event EventHandler<DBConEventArgs> onDBConEvt;                               //DB연결 이벤트
        public event EventHandler<DBConTestEventArgs> onDBTestConEvt;                       //DB연결 테스트 이벤트
        public event EventHandler<AddWDeviceEventArgs> onAddWDeviceEvt;                     //측기 등록 이벤트
        public event EventHandler<UpdateWDeviceEventArgs> onUpdateWDeviceEvt;               //측기 수정 이벤트
        public event EventHandler<DeleteWDeviceEventArgs> onDeleteWDeviceEvt;               //측기 삭제 이벤트
        public event EventHandler<AddRainDataEventArgs> onAddRainDataEvt;                   //강수 데이터 수신 이벤트
        public event EventHandler<AddWaterLevelEventArgs> onAddWaterLevelDataEvt;           //수위 데이터 수신 이벤트
        public event EventHandler<AddFlowSpeedEventArgs> onAddFlowSpeedDataEvt;             //유속 데이터 수신 이벤트
        public event EventHandler<AddAlarmEventArgs> onAddAlarmDataEvt;                     //임계치 알람 데이터 수신 이벤트
        public event EventHandler<AddWDeviceItemDataEventArgs> onAddWDeviceItemDataEvt;     //측기 상태 및 제어 데이터 수신 이벤트
        public event EventHandler<AddWDeviceRequestEventArgs> onAddWDeviceRequestEvt;       //측기 요청 및 제어 이벤트 (사용자가 발생시키는 이벤트)
        public event EventHandler<SendSmsMsgEventArgs> onSendSmsMsgEvt;                     //SMS 전송 이벤트
        public event EventHandler<WOUWDeviceRequestEventArgs> onWOURequestEvt;              //WOU 측기에 상태요청하는 이벤트
        public event EventHandler<WOUWDeviceControlAlarmEventArgs> onWOUAlarmControlEvt;    //WOU 측기의 임계치 제어하는 이벤트
        public event EventHandler<WOUWDeviceControlFTimeEventArgs> onWOUFTimeControlEvt;    //WOU 측기의 무시시간을 제어하는 이벤트
        public event EventHandler<LatelyRainDataEventArgs> onLatelyRainDataEvt;             //가장 최근의 강수 데이터를 가져오는 이벤트
        public event EventHandler<LatelyWaterLevelDataEventArgs> onLatelyWaterLevelEvt;     //가장 최근의 수위 데이터를 가져오는 이벤트
        public event EventHandler<LatelyFlowSpeedDataEventArgs> onLatelyFlowSpeedEvt;       //가장 최근의 유속 데이터를 가져오는 이벤트
        public event EventHandler<AddSmsUserEventArgs> onAddSmsUserEvt;                     //SMS 사용자를 등록할 때 사용하는 이벤트
        public event EventHandler<UpdateSmsUserEventArgs> onUpdateSmsUserEvt;               //SMS 사용자를 수정할 때 사용하는 이벤트
        public event EventHandler<DelSmsUserEventArgs> onDelSmsUserEvt;                     //SMS 사용자를 삭제할 때 사용하는 이벤트
        public event EventHandler<SetDBDataEventArgs> onDBDataSetEvt;                       //DB 설정 값 변경 시 사용하는 이벤트
        public event EventHandler<SendSmsMsgEventArgs> onSendSmsUserMsgEvt;                 //SMS User에게 SMS 전송하는 이벤트
        public event EventHandler<WDeviceAlarmItemsEventArgs> onWDeviceAlarmEvt;            //측기 알람 이벤트
        public event EventHandler<EthernetClientSendEventArgs> onEthernetClientSendEvt;     //이더넷 클라이언트 데이터 전송 이벤트

        private oracleDAC ora = null;
        private static WeatherDataMng instance = null;
        private static Mutex mutex = new Mutex();

        private List<WUser> userList = new List<WUser>();
        private List<WTypeDevice> typeDeviceList = new List<WTypeDevice>();
        private List<WDevice> deviceList = new List<WDevice>();
        private List<WTypeAlarm> typeAlarmList = new List<WTypeAlarm>();
        private List<WTypeSensor> typeSensorList = new List<WTypeSensor>();
        private WRainData rainData = new WRainData();
        private WWaterLevelData waterLevelData = new WWaterLevelData();
        private WFlowSpeedData flowSpeedData = new WFlowSpeedData();
        private WAlarmData alarmData = new WAlarmData();
        private List<WTypeDeviceItem> typeDeviceItemList = new List<WTypeDeviceItem>();
        private List<WSmsUser> smsUserList = new List<WSmsUser>();
        private List<MapSmsUser> mapSmsList = new List<MapSmsUser>();
        private List<MapSmsDeviceItem> mapSmsItemList = new List<MapSmsDeviceItem>();
        private List<EthernetClient> e_ClientList = new List<EthernetClient>();

        private DataTable userDT = new DataTable("user");
        private DataTable typeDeviceDT = new DataTable("typeDevice");
        private DataTable deviceDT = new DataTable("device");
        private DataTable typeAlarmDT = new DataTable("typeAlarm");
        private DataTable typeSensorDT = new DataTable("typeSensor");
        private DataTable rainDataDT = new DataTable("rainData");
        private DataTable waterLevelDataDT = new DataTable("waterLevelData");
        private DataTable flowSpeedDataDT = new DataTable("flowSpeedData");
        private DataTable alarmDataDT = new DataTable("alarmData");
        private DataTable typeDeviceItemDT = new DataTable("typeDeviceItem");
        private DataTable smsUserDT = new DataTable("smsUser");
        private DataTable mapSmsDT = new DataTable("mapSmsUser");
        private DataTable mapSmsItemDT = new DataTable("mapSmsItem");
        private bool cdmaState = false;

        /// <summary>
        /// 측기 상태 항목 enum's
        /// </summary>
        public enum WIType
        {
            배터리상태 = 1,
            태양전지 = 2,
            시간 = 3,
            FAN = 4,
            DOOR = 5,
            CDMA감도 = 6,
            강수임계치1단계 = 7,
            강수임계치2단계 = 8,
            강수임계치3단계 = 9,
            수위임계치1단계 = 10,
            수위임계치2단계 = 11,
            수위임계치3단계 = 12,
            유속임계치1단계 = 13,
            유속임계치2단계 = 14,
            유속임계치3단계 = 15,
            배터리임계치1단계 = 16,
            배터리임계치2단계 = 17,
            태양전지1차 = 18,
            태양전지2차 = 19,
            동일레벨무시시간 = 20,
            하향레벨무시시간 = 21,
            관측모드 = 22,
            강수센서상태 = 23,
            수위센서상태 = 24,
            유속센서상태 = 25,
            펌웨어버전 = 26,
            강수센서사용여부 = 27,
            수위센서사용여부 = 28,
            유속센서사용여부 = 29,
            RAT통신상태 = 30,
            AS보고 = 31,
            IP = 32,
            PORT = 33,
            ALL = 34,
            업그레이드IP = 35,
            업그레이드PORT = 36,
            RESET = 37,
            강수센서시험요청 = 38,
            수위센서시험요청 = 39,
            유속센서시험요청 = 40,
            강수임계치요청 = 41,
            수위임계치요청 = 42,
            유속임계치요청 = 43,
            무시시간요청 = 44,
            임계치제어응답 = 45,
            무시시간제어응답 = 46,
            배터리전압 = 47,
            배터리전류 = 48,
            배터리저항 = 49,
            배터리온도 = 50,
            배터리수명 = 51,
            함체FAN동작임계치 = 52,
            배터리2전압 = 53,
            배터리2전류 = 54,
            배터리2저항 = 55,
            배터리2온도 = 56,
            배터리2수명 = 57,
            배터리2상태 = 58,
            배터리1전압상태 = 59,
            배터리1온도상태 = 60,
            배터리1점검시기 = 61,
            배터리1교체시기 = 62,
            배터리1교체초기화 = 63,
            배터리2전압상태 = 64,
            배터리2온도상태 = 65,
            배터리2점검시기 = 66,
            배터리2교체시기 = 67,
            배터리2교체초기화 = 68,
            AC전압입력 = 69,
            태양전지전압입력 = 70,
            배터리충전상태 = 71,
            CDMA감도낮음 = 72,
            CDMA시간설정이상 = 73,
            배터리감지센서통신상태 = 74,
            우량계데이터감지 = 75,
            수위계데이터감지 = 76,
            유속계데이터감지 = 77,
            배터리사용여부 = 78,
            이더넷IP = 79,
            이더넷PORT = 80,
            NotUse = 100
        }

        /// <summary>
        /// SMS 수신 항목 enum's
        /// </summary>
        public enum SMSType
        {
            임계치1단계 = 1,
            임계치2단계 = 2,
            임계치3단계 = 3,
            배터리1전압이상 = 4,
            배터리1온도이상 = 5,
            배터리1점검시기 = 6,
            배터리1교체시기 = 7,
            배터리1교체초기화 = 8,
            배터리2전압이상 = 9,
            배터리2온도이상 = 10,
            배터리2점검시기 = 11,
            배터리2교체시기 = 12,
            배터리2교체초기화 = 13,
            FAN이상 = 14,
            CDMA시간설정이상 = 15,
            센서상태 = 16
        }

        #region 접근
        /// <summary>
        /// 사용자 리스트
        /// </summary>
        public List<WUser> UserList
        {
            get { return this.userList; }
            set { this.userList = value; }
        }

        /// <summary>
        /// 장비 종류 리스트
        /// </summary>
        public List<WTypeDevice> TypeDeviceList
        {
            get { return this.typeDeviceList; }
            set { this.typeDeviceList = value; }
        }

        /// <summary>
        /// 장비 리스트
        /// </summary>
        public List<WDevice> DeviceList
        {
            get { return this.deviceList; }
            set { this.deviceList = value; }
        }

        /// <summary>
        /// 알람 종류 리스트
        /// </summary>
        public List<WTypeAlarm> TypeAlarmList
        {
            get { return this.typeAlarmList; }
            set { this.typeAlarmList = value; }
        }

        /// <summary>
        /// 센서 종류 리스트
        /// </summary>
        public List<WTypeSensor> TypeSensorList
        {
            get { return this.typeSensorList; }
            set { this.typeSensorList = value; }
        }

        /// <summary>
        /// SMS 사용자 리스트
        /// </summary>
        public List<WSmsUser> SmsUserList
        {
            get { return this.smsUserList; }
            set { this.smsUserList = value; }
        }

        /// <summary>
        /// SMS 사용자와 측기를 연결시키는 클래스 리스트
        /// </summary>
        public List<MapSmsUser> MapSmsList
        {
            get { return this.mapSmsList; }
            set { this.mapSmsList = value; }
        }

        /// <summary>
        /// SMS 사용자와 수신받을 항목을 연결하는 클래스 리스트
        /// </summary>
        public List<MapSmsDeviceItem> MapSmsItemList
        {
            get { return this.mapSmsItemList; }
            set { this.mapSmsItemList = value; }
        }

        /// <summary>
        /// CDMA 모뎀 상태
        /// </summary>
        public bool CDMA
        {
            get { return this.cdmaState; }
            set { this.cdmaState = value; }
        }

        /// <summary>
        /// 이더넷 클라이언트 리스트
        /// </summary>
        public List<EthernetClient> E_ClientList
        {
            get { return this.e_ClientList; }
            set { this.e_ClientList = value; }
        }
        #endregion

        public WeatherDataMng()
        {
        }

        /// <summary>
        /// DataManager 생성 메소드
        /// </summary>
        /// <returns></returns>
        public static WeatherDataMng getInstance()
        {
            mutex.WaitOne();

            if (instance == null)
            {
                instance = new WeatherDataMng();
            }

            mutex.ReleaseMutex();
            return instance;
        }

        /// <summary>
        /// DB에서 테이블을 읽어 기초데이터를 생성한다.
        /// </summary>
        /// <returns></returns>
        public bool SetInitData()
        {
            if (this.ora == null || this.ora.ConnectionState == System.Data.ConnectionState.Closed)
            {
                return false;
            }

            this.GetDataBaseAll(); //DB의 데이터를 가져온다.
            this.ForNoHaveWDevice();
            return true;
        }

        /// <summary>
        /// 등록되지 않은 외부 측기 데이터를 DB에 저장하기 위한 메소드
        /// </summary>
        private void ForNoHaveWDevice()
        {
            string userQuery = "SELECT COUNT(*) FROM DEVICE WHERE PKID = 56";
            this.userDT = this.ora.getDataTable(userQuery, "datarcv");
        }

        /// <summary>
        /// 데이터 리스트를 초기화 한다.
        /// </summary>
        /// <returns></returns>
        public void SetRemoveData()
        {
            userList.Clear();
            typeDeviceList.Clear();
            deviceList.Clear();
            typeAlarmList.Clear();
            typeSensorList.Clear();
            typeDeviceItemList.Clear();
            mapSmsItemList.Clear();
        }

        /// <summary>
        /// 데이터베이스에 연결한다.
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_port"></param>
        /// <param name="_id"></param>
        /// <param name="_pw"></param>
        /// <param name="_sid"></param>
        /// <returns></returns>
        public void DataBaseCon(string _ip, string _port, string _id, string _pw, string _sid)
        {
            bool rst = false;

            if (this.ora == null)
            {
                this.ora = new oracleDAC(_id, _pw, _ip, _port, _sid);
                rst = this.ora.openDb();
            }
            else
            {
                this.ora.closeDb();
                this.ora = null;
                this.ora = new oracleDAC(_id, _pw, _ip, _port, _sid);
                Thread.Sleep(20);
                rst = this.ora.openDb();
            }

            if (this.onDBConEvt != null)
            {
                this.onDBConEvt(this, new DBConEventArgs(rst));
            }
        }

        /// <summary>
        /// 데이터베이스에 연결한다.
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_port"></param>
        /// <param name="_id"></param>
        /// <param name="_pw"></param>
        /// <param name="_sid"></param>
        /// <returns></returns>
        public bool DataBaseConnect(string _ip, string _port, string _id, string _pw, string _sid)
        {
            bool rst = false;

            if (this.ora == null)
            {
                this.ora = new oracleDAC(_id, _pw, _ip, _port, _sid);
                rst = this.ora.openDb();
            }
            else
            {
                this.ora.closeDb();
                this.ora = null;
                this.ora = new oracleDAC(_id, _pw, _ip, _port, _sid);
                Thread.Sleep(20);
                rst = this.ora.openDb();
            }

            return rst;
        }

        /// <summary>
        /// 데이터베이스 연결을 테스트한다.
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_port"></param>
        /// <param name="_id"></param>
        /// <param name="_pw"></param>
        /// <param name="_sid"></param>
        public void DataBaseTest(string _ip, string _port, string _id, string _pw, string _sid)
        {
            bool rst = false;

            if (this.ora == null)
            {
                this.ora = new oracleDAC(_id, _pw, _ip, _port, _sid);
                rst = this.ora.openDb();
            }
            else
            {
                this.ora.closeDb();
                this.ora = null;
                this.ora = new oracleDAC(_id, _pw, _ip, _port, _sid);
                Thread.Sleep(20);
                rst = this.ora.openDb();
            }

            this.ora.closeDb();
            this.ora = null;

            if (this.onDBTestConEvt != null)
            {
                this.onDBTestConEvt(this, new DBConTestEventArgs(rst));
            }
        }

        /// <summary>
        /// DB의 모든 정보를 가져온다.
        /// </summary>
        private void GetDataBaseAll()
        {
            //user 테이블을 가져온다.
            string userQuery = "SELECT * FROM SYSTEMUSER";
            this.userDT = this.ora.getDataTable(userQuery, "datarcv");

            for (int i = 0; i < this.userDT.Rows.Count; i++)
            {
                object[] userObj = this.userDT.Rows[i].ItemArray;
                WUser tmpUser = new WUser(uint.Parse(userObj[0].ToString()), userObj[3].ToString(), userObj[1].ToString(), userObj[2].ToString());
                this.userList.Add(tmpUser);
            }

            //typeDevice 테이블을 가져온다.
            string typeDeviceQuery = "SELECT * FROM TYPEDEVICE";
            this.typeDeviceDT = this.ora.getDataTable(typeDeviceQuery, "datarcv");

            for (int i = 0; i < this.typeDeviceDT.Rows.Count; i++)
            {
                object[] typeDeviceObj = this.typeDeviceDT.Rows[i].ItemArray;
                WTypeDevice tmpTypeDevice = new WTypeDevice(uint.Parse(typeDeviceObj[0].ToString()), typeDeviceObj[1].ToString(), typeDeviceObj[2].ToString());
                this.typeDeviceList.Add(tmpTypeDevice);
            }

            //Device 테이블을 가져온다.
            string deviceQuery = "SELECT * FROM DEVICE WHERE ISUSE = 1";
            this.deviceDT = this.ora.getDataTable(deviceQuery, "datarcv");

            for (int i = 0; i < this.deviceDT.Rows.Count; i++)
            {
                object[] deviceObj = this.deviceDT.Rows[i].ItemArray;
                WDevice tmpDevice = new WDevice(uint.Parse(deviceObj[0].ToString()), deviceObj[2].ToString(), deviceObj[3].ToString(),
                                                deviceObj[4].ToString(), uint.Parse(deviceObj[1].ToString()), byte.Parse(deviceObj[5].ToString()),
                                                (byte.Parse(deviceObj[8].ToString()) == 1) ? true : false, deviceObj[7].ToString());
                this.deviceList.Add(tmpDevice);
            }

            //typeAlarm 테이블을 가져온다.
            string typeAlarmQuery = string.Format("SELECT * FROM TYPEALARMLEVEL");
            this.typeAlarmDT = this.ora.getDataTable(typeAlarmQuery, "datarcv");

            for (int i = 0; i < this.typeAlarmDT.Rows.Count; i++)
            {
                object[] typeAlarmObj = this.typeAlarmDT.Rows[i].ItemArray;
                WTypeAlarm tmpTypeAlarm = new WTypeAlarm(uint.Parse(typeAlarmObj[0].ToString()), typeAlarmObj[1].ToString());
                this.typeAlarmList.Add(tmpTypeAlarm);
            }

            //typeSensor 테이블을 가져온다.
            string typeSensorQuery = string.Format("SELECT * FROM TYPESENSOR");
            this.typeSensorDT = this.ora.getDataTable(typeSensorQuery, "datarcv");

            for (int i = 0; i < this.typeSensorDT.Rows.Count; i++)
            {
                object[] typeSensorObj = this.typeSensorDT.Rows[i].ItemArray;
                WTypeSensor tmpTypeSensor = new WTypeSensor(uint.Parse(typeSensorObj[0].ToString()), typeSensorObj[1].ToString());
                this.typeSensorList.Add(tmpTypeSensor);
            }

            //typeDeviceItem 테이블을 가져온다.
            string typeDeviceItemQuery = string.Format("SELECT * FROM DEVICEITEM");
            this.typeDeviceItemDT = this.ora.getDataTable(typeDeviceItemQuery, "datarcv");

            for (int i = 0; i < this.typeDeviceItemDT.Rows.Count; i++)
            {
                object[] typeDeviceItemObj = this.typeDeviceItemDT.Rows[i].ItemArray;
                WTypeDeviceItem tmpTypeDeviceItem = new WTypeDeviceItem(uint.Parse(typeDeviceItemObj[0].ToString()), typeDeviceItemObj[1].ToString(),
                                                                        typeDeviceItemObj[2].ToString());
                this.typeDeviceItemList.Add(tmpTypeDeviceItem);
            }

            //smsUser 테이블을 가져온다.
            string smsUserItemQuery = string.Format("SELECT * FROM SMSUSER WHERE ISUSE = 1");
            this.smsUserDT = this.ora.getDataTable(smsUserItemQuery, "datarcv");

            for (int i = 0; i < this.smsUserDT.Rows.Count; i++)
            {
                object[] smsUserItemObj = this.smsUserDT.Rows[i].ItemArray;
                WSmsUser tmpSmsUser = new WSmsUser(uint.Parse(smsUserItemObj[0].ToString()), smsUserItemObj[1].ToString(),
                    smsUserItemObj[2].ToString(), smsUserItemObj[4].ToString());
                this.smsUserList.Add(tmpSmsUser);
            }

            //mapSms 테이블을 가져온다.
            string mapSmsItemQuery = string.Format("SELECT * FROM MAPSMSUSER WHERE ISUSE = 1");
            this.mapSmsDT = this.ora.getDataTable(mapSmsItemQuery, "datarcv");

            for (int i = 0; i < this.mapSmsDT.Rows.Count; i++)
            {
                object[] mapSmsItemObj = this.mapSmsDT.Rows[i].ItemArray;
                MapSmsUser tmpSmsUser = new MapSmsUser(uint.Parse(mapSmsItemObj[0].ToString()), uint.Parse(mapSmsItemObj[1].ToString()),
                    uint.Parse(mapSmsItemObj[2].ToString()));
                this.mapSmsList.Add(tmpSmsUser);
            }

            //mapSmsDeviceItem 테이블을 가져온다.
            string mapSmsDeviceItemQuery = string.Format("SELECT s.pkid, s.fksmsdeviceitem, s.fksmsuser, s.isuse FROM mapsmsdeviceitem s JOIN smsuser u ON u.pkid = s.fksmsuser WHERE u.isuse = '1'");
            this.mapSmsItemDT = this.ora.getDataTable(mapSmsDeviceItemQuery, "datarcv");

            for (int i = 0; i < this.mapSmsItemDT.Rows.Count; i++)
            {
                object[] mapSmsDeviceItemObj = this.mapSmsItemDT.Rows[i].ItemArray;
                MapSmsDeviceItem tmpSmsItem = new MapSmsDeviceItem(uint.Parse(mapSmsDeviceItemObj[0].ToString()), uint.Parse(mapSmsDeviceItemObj[1].ToString()), uint.Parse(mapSmsDeviceItemObj[2].ToString()), (mapSmsDeviceItemObj[3].ToString() == "1") ? true : false);
                this.mapSmsItemList.Add(tmpSmsItem);
            }
        }

        /// <summary>
        /// 가장 최근의 모든 데이터를 가져온다.
        /// </summary>
        public void GetAllLatelyData()
        {
            this.GetRainFlowLately();       //DB의 강수 데이터 중 가장 최근 정보를 가져온다.(UI 로드 용도)
            this.GetWaterLevelLately();     //DB의 수위 데이터 중 가장 최근 정보를 가져온다.(UI 로드 용도)
            this.GetFlowSpeedLately();      //DB의 유속 데이터 중 가장 최근 정보를 가져온다.(UI 로드 용도)
        }

        /// <summary>
        /// DB의 강수 데이터 중 가장 최근 1개의 정보를 가져온다.(UI 로드 용도)
        /// </summary>
        private void GetRainFlowLately()
        {
            DataTable tmpDT = new DataTable();

            for (int i = 0; i < this.deviceList.Count; i++)
            {
                string rainFlowQuery = string.Format("{0}{1}{2}{3}{4}",
                    "select chktime, accum10min, move15min, move20min, move60min, today, yesterday",
                    " from rainfall r",
                    " join (select fkdevice, max(chktime) maxTime from rainfall where fkdevice = ",
                    this.deviceList[i].PKID,
                    " group by fkdevice) r2 on r.chktime = r2.maxTime");
                tmpDT = this.ora.getDataTable(rainFlowQuery, "datarcv");

                for (int j = 0; j < tmpDT.Rows.Count; j++)
                {
                    object[] tmpObj = tmpDT.Rows[j].ItemArray;
                    WRainData wr = new WRainData(0, this.deviceList[i].PKID, tmpObj[0].ToString(), tmpObj[1].ToString(),
                        tmpObj[2].ToString(), tmpObj[3].ToString(), tmpObj[4].ToString(), tmpObj[5].ToString(), tmpObj[6].ToString());
                    wr.DDTime = new DateTime(1, 1, 1);

                    if (this.onLatelyRainDataEvt != null)
                    {
                        this.onLatelyRainDataEvt(this, new LatelyRainDataEventArgs(wr));
                    }
                }
            }
        }

        /// <summary>
        /// DB의 수위 데이터 중 가장 최근 1개의 정보를 가져온다.(UI 로드 용도)
        /// </summary>
        private void GetWaterLevelLately()
        {
            DataTable tmpDT = new DataTable();

            for (int i = 0; i < this.deviceList.Count; i++)
            {
                string waterLevelQuery = string.Format("{0}{1}{2}{3}{4}",
                    "select chktime, height, now, change15min, change60min, changetoday, changeyesterday, maxtoday, maxyesterday",
                    " from waterlevel w",
                    " join (select fkdevice, max(chktime) maxTime from waterlevel where fkdevice = ",
                    this.deviceList[i].PKID,
                    " group by fkdevice) w2 on w.chktime = w2.maxTime");
                tmpDT = this.ora.getDataTable(waterLevelQuery, "datarcv");

                for (int j = 0; j < tmpDT.Rows.Count; j++)
                {
                    object[] tmpObj = tmpDT.Rows[j].ItemArray;
                    WWaterLevelData wl = new WWaterLevelData(0, this.deviceList[i].PKID, tmpObj[0].ToString(),
                        tmpObj[1].ToString(), tmpObj[2].ToString(), tmpObj[3].ToString(), tmpObj[4].ToString(),
                        tmpObj[5].ToString(), tmpObj[6].ToString(), tmpObj[7].ToString(), tmpObj[8].ToString());
                    wl.DDTime = new DateTime(1, 1, 1);

                    if (this.onLatelyWaterLevelEvt != null)
                    {
                        this.onLatelyWaterLevelEvt(this, new LatelyWaterLevelDataEventArgs(wl));
                    }
                }
            }
        }

        /// <summary>
        /// DB의 유속 데이터 중 가장 최근 1개의 정보를 가져온다.(UI 로드 용도)
        /// </summary>
        private void GetFlowSpeedLately()
        {
            DataTable tmpDT = new DataTable();

            for (int i = 0; i < this.deviceList.Count; i++)
            {
                string flowSpeedQuery = string.Format("{0}{1}{2}{3}{4}",
                    "select chktime, now, change15min, change60min, changetoday, changeyesterday, maxtoday, maxyesterday",
                    " from flowspeed f",
                    " join (select fkdevice, max(chktime) maxTime from flowspeed where fkdevice = ",
                    this.deviceList[i].PKID,
                    " group by fkdevice) f2 on f.chktime = f2.maxTime");
                tmpDT = this.ora.getDataTable(flowSpeedQuery, "datarcv");

                for (int j = 0; j < tmpDT.Rows.Count; j++)
                {
                    object[] tmpObj = tmpDT.Rows[j].ItemArray;
                    WFlowSpeedData fs = new WFlowSpeedData(0, this.deviceList[i].PKID, tmpObj[0].ToString(),
                        tmpObj[1].ToString(), tmpObj[2].ToString(), tmpObj[3].ToString(), tmpObj[4].ToString(),
                        tmpObj[5].ToString(), tmpObj[6].ToString(), tmpObj[7].ToString());
                    fs.DDTime = new DateTime(1, 1, 1);

                    if (this.onLatelyFlowSpeedEvt != null)
                    {
                        this.onLatelyFlowSpeedEvt(this, new LatelyFlowSpeedDataEventArgs(fs));
                    }
                }
            }
        }

        /// <summary>
        /// 측기를 DB에 추가하고 측기등록 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wd"></param>
        /// <returns></returns>
        public void AddWDevice(WDevice _wd)
        {
            try
            {
                //등록 시, delete_flag 에 0 을 그대로 두고, pkid_in 에 0 을 넣습니다.
                //oracle_parameter(플래그, 데이터타입, 값, 모드 : INPUT/OUTPUT)

                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEDEVICE(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();
                
                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fktypedevice_in", oracle_parameter.OracleDataType.Int32, _wd.TypeDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("deviceid_in", oracle_parameter.OracleDataType.Char, _wd.ID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("devicename_in", oracle_parameter.OracleDataType.Char, _wd.Name, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("telephone_in", oracle_parameter.OracleDataType.Char, _wd.CellNumber, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("sensor_in", oracle_parameter.OracleDataType.Int32, _wd.HaveSensor, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("ethernetUse_in", oracle_parameter.OracleDataType.Int32, (_wd.EthernetUse == true) ? 1 : 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("remark_in", oracle_parameter.OracleDataType.Char, _wd.Remark, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "DEVICE");

                string deviceQuery = "SELECT * FROM DEVICE WHERE ISUSE = 1 AND DEVICEID = '" + _wd.ID + "' AND FKTYPEDEVICE = " + _wd.TypeDevice;
                this.deviceDT = this.ora.getDataTable(deviceQuery, "datarcv");
                WDevice tmpDevice = null;

                for (int i = 0; i < this.deviceDT.Rows.Count; i++)
                {
                    object[] deviceObj = this.deviceDT.Rows[i].ItemArray;
                    tmpDevice = new WDevice(uint.Parse(deviceObj[0].ToString()), deviceObj[2].ToString(), deviceObj[3].ToString(),
                                            deviceObj[4].ToString(), uint.Parse(deviceObj[1].ToString()), byte.Parse(deviceObj[5].ToString()),
                                            (byte.Parse(deviceObj[8].ToString()) == 1) ? true : false, deviceObj[7].ToString());
                }

                this.deviceList.Add(tmpDevice);
                Thread.Sleep(20);

                if (this.onAddWDeviceEvt != null)
                {
                    this.onAddWDeviceEvt(this, new AddWDeviceEventArgs(tmpDevice));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddWDevice() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 수정된 측기를 DB에 저장하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wd"></param>
        public void UpdateWDevice(WDevice _wd)
        {
            try
            {
                //수정 시, delete_flag 에 0 을 그대로 두고, pkid_in 에 수정할 PKID 를 넣습니다.
                //oracle_parameter(플래그, 데이터타입, 값, 모드 : INPUT/OUTPUT)

                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEDEVICE(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, _wd.PKID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fktypedevice_in", oracle_parameter.OracleDataType.Int32, _wd.TypeDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("deviceid_in", oracle_parameter.OracleDataType.Char, _wd.ID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("devicename_in", oracle_parameter.OracleDataType.Char, _wd.Name, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("telephone_in", oracle_parameter.OracleDataType.Char, _wd.CellNumber, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("sensor_in", oracle_parameter.OracleDataType.Int32, _wd.HaveSensor, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("ethernetUse_in", oracle_parameter.OracleDataType.Int32, (_wd.EthernetUse == true) ? 1 : 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("remark_in", oracle_parameter.OracleDataType.Char, _wd.Remark, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int rst = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "DEVICE");

                for (int i = 0; i < this.deviceList.Count; i++)
                {
                    if (this.deviceList[i].PKID == _wd.PKID)
                    {
                        this.deviceList.Remove(this.deviceList[i]);
                        break;
                    }
                }

                this.deviceList.Add(_wd);
                Thread.Sleep(20);

                if (this.onUpdateWDeviceEvt != null)
                {
                    this.onUpdateWDeviceEvt(this, new UpdateWDeviceEventArgs(_wd));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.UpdateWDevice() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 측기를 DB에서 삭제하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wd"></param>
        public void DeleteWDevice(List<WDevice> _wdList)
        {
            try
            {
                //삭제 시, delete_flag 에 1 을 넣고, pkid_in 에 삭제할 PKID 를 넣으면 ISUSE 가 0으로 UPDATE 됩니다.
                //oracle_parameter(플래그, 데이터타입, 값, 모드 : INPUT/OUTPUT)

                for (int i = 0; i < _wdList.Count; i++)
                {
                    StringBuilder sBuilder = new StringBuilder();
                    sBuilder.Append("begin SP_MANAGEDEVICE(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10); end;");
                    List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                    inner_parameters.Add(
                        new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 1, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, _wdList[i].PKID, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("fktypedevice_in", oracle_parameter.OracleDataType.Int32, _wdList[i].TypeDevice, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("deviceid_in", oracle_parameter.OracleDataType.Char, _wdList[i].ID, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("devicename_in", oracle_parameter.OracleDataType.Char, _wdList[i].Name, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("telephone_in", oracle_parameter.OracleDataType.Char, _wdList[i].CellNumber, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("sensor_in", oracle_parameter.OracleDataType.Int32, _wdList[i].HaveSensor, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("ethernetUse_in", oracle_parameter.OracleDataType.Int32, (_wdList[i].EthernetUse == true) ? 1 : 0, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("remark_in", oracle_parameter.OracleDataType.Char, string.Empty, ParameterDirection.Input));
                    inner_parameters.Add(
                        new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                    int rst = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "DEVICE");
                    this.deviceList.Remove(this.GetWDevice(_wdList[i].PKID));
                    Thread.Sleep(20);

                    string smsQuery = string.Format("SELECT * FROM MAPSMSUSER WHERE ISUSE = 1 AND FKDEVICE = '{0}'", _wdList[i].PKID);
                    this.smsUserDT = this.ora.getDataTable(smsQuery, "datarcv");
                    MapSmsUser tmpSmsUser = null;

                    for (int j = 0; j < this.smsUserDT.Rows.Count; j++)
                    {
                        object[] smsUserObj = this.smsUserDT.Rows[j].ItemArray;
                        tmpSmsUser = new MapSmsUser(uint.Parse(smsUserObj[0].ToString()), uint.Parse(smsUserObj[1].ToString()), uint.Parse(smsUserObj[2].ToString()));
                        this.DelMapSmsUser(tmpSmsUser);
                    }
                }

                Thread.Sleep(20);

                if (this.onDeleteWDeviceEvt != null)
                {
                    this.onDeleteWDeviceEvt(this, new DeleteWDeviceEventArgs(_wdList));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.DeleteWDevice() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 강수 데이터를 DB에 저장하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wr"></param>
        public void AddRainData(WRainData _wr)
        {
            try
            {
                //oracle_parameter(프로시져 인자명, 데이터타입, 값, INPUT/OUTPUT)

                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGERAINFALL(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wr.FKDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chktime_in", oracle_parameter.OracleDataType.Date, _wr.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("accum10min_in", oracle_parameter.OracleDataType.Char, _wr.R10min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("move15min_in", oracle_parameter.OracleDataType.Char, _wr.R15min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("move20min_in", oracle_parameter.OracleDataType.Char, _wr.R20min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("move60min_in", oracle_parameter.OracleDataType.Char, _wr.R60min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("today_in", oracle_parameter.OracleDataType.Char, _wr.Today, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("yesterday_in", oracle_parameter.OracleDataType.Char, _wr.Ystday, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "RAINFALL");
                Thread.Sleep(20);

                string resultStr = inner_parameters[9].Parameter.Value.ToString();

                if (resultStr == "")
                {
                    //System.Windows.Forms.MessageBox.Show("DB 용량을 체크하세요.", "DB 저장", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                if (this.onAddRainDataEvt != null)
                {
                    this.onAddRainDataEvt(this, new AddRainDataEventArgs(_wr));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddRainData() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 수위 데이터를 DB에 저장하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wwl"></param>
        public void AddWaterLevelData(WWaterLevelData _wwl)
        {
            try
            {
                //oracle_parameter(프로시져 인자명, 데이터타입, 값, INPUT/OUTPUT)

                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEWATERLEVEL(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11,:12); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wwl.FKDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chktime_in", oracle_parameter.OracleDataType.Date, _wwl.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("height", oracle_parameter.OracleDataType.Char, _wwl.WHigh, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("now", oracle_parameter.OracleDataType.Char, _wwl.WNow, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("change15min_in", oracle_parameter.OracleDataType.Char, _wwl.R15min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("change60min_in", oracle_parameter.OracleDataType.Char, _wwl.R60min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("changeToday_in", oracle_parameter.OracleDataType.Char, _wwl.Today, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("changeYesterday_in", oracle_parameter.OracleDataType.Char, _wwl.Ystday, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("MaxToday_in", oracle_parameter.OracleDataType.Char, _wwl.TodayMax, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("MaxYesterday_in", oracle_parameter.OracleDataType.Char, _wwl.YstdayMax, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "WATERLEVEL");
                Thread.Sleep(20);

                string resultStr = inner_parameters[11].Parameter.Value.ToString();

                if (resultStr == "")
                {
                    System.Windows.Forms.MessageBox.Show("DB 용량을 체크하세요.", "DB 저장", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                if (this.onAddWaterLevelDataEvt != null)
                {
                    this.onAddWaterLevelDataEvt(this, new AddWaterLevelEventArgs(_wwl));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddWaterLevelData() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 유속 데이터를 DB에 저장하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wfs"></param>
        public void AddFlowSpeedData(WFlowSpeedData _wfs)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEFLOWSPEED(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();
                
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wfs.FKDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chktime_in", oracle_parameter.OracleDataType.Date, _wfs.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("now", oracle_parameter.OracleDataType.Char, _wfs.RNow, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("change15min_in", oracle_parameter.OracleDataType.Char, _wfs.R15min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("change60min_in", oracle_parameter.OracleDataType.Char, _wfs.R60min, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("changeToday_in", oracle_parameter.OracleDataType.Char, _wfs.Today, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("changeYesterday_in", oracle_parameter.OracleDataType.Char, _wfs.Ystday, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("MaxToday_in", oracle_parameter.OracleDataType.Char, _wfs.TodayMax, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("MaxYesterday_in", oracle_parameter.OracleDataType.Char, _wfs.YstdayMax, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "FLOWSPEED");
                Thread.Sleep(20);

                string resultStr = inner_parameters[10].Parameter.Value.ToString();

                if (resultStr == "")
                {
                    System.Windows.Forms.MessageBox.Show("DB 용량을 체크하세요.", "DB 저장", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

                if (this.onAddFlowSpeedDataEvt != null)
                {
                    this.onAddFlowSpeedDataEvt(this, new AddFlowSpeedEventArgs(_wfs));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddFlowSpeedData - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 임계치 알람 정보를 DB에 저장하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wa"></param>
        public void AddAlarmData(WAlarmData _wa)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEALARM(:1, :2, :3, :4, :5, :6, :7, :8); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wa.FKDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fktypeSensor_in", oracle_parameter.OracleDataType.Int32, _wa.AlarmType, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkTypeAlarmLevel_in", oracle_parameter.OracleDataType.Int32, _wa.AlarmLevel, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("realMode_in", oracle_parameter.OracleDataType.Int32, _wa.Real, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("alarmTime_in", oracle_parameter.OracleDataType.Date, _wa.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("alarmValue_in", oracle_parameter.OracleDataType.Int32, _wa.Value, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "ALARM");
                Thread.Sleep(20);

                if (this.onAddAlarmDataEvt != null)
                {
                    this.onAddAlarmDataEvt(this, new AddAlarmEventArgs(_wa));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddAlarmData() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 측기의 상태 및 제어 데이터를 받아 DB에 저장하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wdi"></param>
        public void AddDeviceItemData(WDeviceItem _wdi)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEDEVICERESPONSE(:1, :2, :3, :4, :5, :6); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();
                
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wdi.FKDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdeviceitem_in", oracle_parameter.OracleDataType.Int32, _wdi.FKDeviceItem, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chktime_in", oracle_parameter.OracleDataType.Date, _wdi.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chkValue_in", oracle_parameter.OracleDataType.Char, _wdi.Value, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "DEVICERESPONSE");
                Thread.Sleep(20);

                if (this.onAddWDeviceItemDataEvt != null)
                {
                    this.onAddWDeviceItemDataEvt(this, new AddWDeviceItemDataEventArgs(_wdi));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddDeviceItemData() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 측기에 요청 및 제어를 하고 DB에 저장한 후 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wdr"></param>
        public void AddDeviceRequest(WDeviceRequest _wdr)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEDEVICEREQUEST(:1, :2, :3, :4, :5, :6, :7); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wdr.FkDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdeviceitem_in", oracle_parameter.OracleDataType.Int32, _wdr.FkDeviceItem, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chktime_in", oracle_parameter.OracleDataType.Date, _wdr.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("iscontrol_in", oracle_parameter.OracleDataType.Int32, _wdr.IsControl, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("chkValue_in", oracle_parameter.OracleDataType.Char, _wdr.Value, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "DEVICEREQUEST");
                Thread.Sleep(20);

                if (this.onAddWDeviceRequestEvt != null)
                {
                    this.onAddWDeviceRequestEvt(this, new AddWDeviceRequestEventArgs(_wdr));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddDeviceRequest() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS 사용자를 등록하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wsu"></param>
        public void AddSmsUser(WSmsUser _wsu, List<string> _devicePkid)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGESMSUSER(:1, :2, :3, :4, :5, :6, :7); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("userName_in", oracle_parameter.OracleDataType.Char, _wsu.Name, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("cellNumber_in", oracle_parameter.OracleDataType.Char, _wsu.TelNum, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isuse_in", oracle_parameter.OracleDataType.Char, "1", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("remark_in", oracle_parameter.OracleDataType.Char, _wsu.Remark, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "smsuser");

                string smsQuery = string.Format("SELECT * FROM SMSUSER WHERE ISUSE = 1 AND USERNAME = '{0}' AND CELLNUMBER = '{1}'",
                    _wsu.Name, _wsu.TelNum);
                this.smsUserDT = this.ora.getDataTable(smsQuery, "datarcv");
                WSmsUser tmpSmsUser = null;

                for (int i = 0; i < this.smsUserDT.Rows.Count; i++)
                {
                    object[] smsUserObj = this.smsUserDT.Rows[i].ItemArray;
                    tmpSmsUser = new WSmsUser(uint.Parse(smsUserObj[0].ToString()), smsUserObj[1].ToString(), smsUserObj[2].ToString(), smsUserObj[4].ToString());
                    this.smsUserList.Add(tmpSmsUser);
                }

                Thread.Sleep(20);

                for (int i = 0; i < _devicePkid.Count; i++)
                {
                    MapSmsUser msu = new MapSmsUser(0, uint.Parse(_devicePkid[i]), tmpSmsUser.PKID);
                    this.AddMapSmsUser(msu);
                }

                if (this.onAddSmsUserEvt != null)
                {
                    this.onAddSmsUserEvt(this, new AddSmsUserEventArgs(tmpSmsUser));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddSmsUser() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS 사용자를 수정하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wsu"></param>
        public void UpdateSmsUser(WSmsUser _wsu, List<string> _devicePkid)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGESMSUSER(:1, :2, :3, :4, :5, :6, :7); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, _wsu.PKID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("userName_in", oracle_parameter.OracleDataType.Char, _wsu.Name, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("cellNumber_in", oracle_parameter.OracleDataType.Char, _wsu.TelNum, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isuse_in", oracle_parameter.OracleDataType.Char, "1", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("remark_in", oracle_parameter.OracleDataType.Char, _wsu.Remark, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "smsuser");

                for (int i = 0; i < this.smsUserList.Count; i++)
                {
                    if (_wsu.PKID == this.smsUserList[i].PKID)
                    {
                        this.smsUserList.Remove(this.smsUserList[i]);
                        break;
                    }
                }

                this.smsUserList.Add(_wsu);
                Thread.Sleep(20);

                string smsQuery = string.Format("SELECT * FROM MAPSMSUSER WHERE ISUSE = 1 AND FKSMSUSER = {0}", _wsu.PKID);
                this.smsUserDT = this.ora.getDataTable(smsQuery, "datarcv");
                MapSmsUser tmpSmsUser = null;

                for (int i = 0; i < this.smsUserDT.Rows.Count; i++)
                {
                    object[] smsUserObj = this.smsUserDT.Rows[i].ItemArray;
                    tmpSmsUser = new MapSmsUser(uint.Parse(smsUserObj[0].ToString()), uint.Parse(smsUserObj[1].ToString()), uint.Parse(smsUserObj[2].ToString()));
                    this.DelMapSmsUser(tmpSmsUser);
                }

                for (int i = 0; i < _devicePkid.Count; i++)
                {
                    MapSmsUser msu = new MapSmsUser(0, uint.Parse(_devicePkid[i]), _wsu.PKID);
                    this.AddMapSmsUser(msu);
                }

                if (this.onUpdateSmsUserEvt != null)
                {
                    this.onUpdateSmsUserEvt(this, new UpdateSmsUserEventArgs(_wsu));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.UpdateSmsUser() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS 사용자를 삭제하고 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_wsu"></param>
        public void DelSmsUser(WSmsUser _wsu)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGESMSUSER(:1, :2, :3, :4, :5, :6, :7); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 1, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, _wsu.PKID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("userName_in", oracle_parameter.OracleDataType.Char, _wsu.Name, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("cellNumber_in", oracle_parameter.OracleDataType.Char, _wsu.TelNum, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isuse_in", oracle_parameter.OracleDataType.Char, "0", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("remark_in", oracle_parameter.OracleDataType.Char, _wsu.Remark, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "smsuser");
                this.smsUserList.Remove(this.getSmsUser(_wsu.PKID));
                Thread.Sleep(20);

                string smsQuery = string.Format("SELECT * FROM MAPSMSUSER WHERE ISUSE = 1 AND FKSMSUSER = '{0}'", _wsu.PKID);
                this.smsUserDT = this.ora.getDataTable(smsQuery, "datarcv");
                MapSmsUser tmpSmsUser = null;

                for (int i = 0; i < this.smsUserDT.Rows.Count; i++)
                {
                    object[] smsUserObj = this.smsUserDT.Rows[i].ItemArray;
                    tmpSmsUser = new MapSmsUser(uint.Parse(smsUserObj[0].ToString()), uint.Parse(smsUserObj[1].ToString()), uint.Parse(smsUserObj[2].ToString()));
                    this.DelMapSmsUser(tmpSmsUser);
                }

                MapSmsDeviceItem[] tmpMapSmsList = new MapSmsDeviceItem[this.mapSmsItemList.Count];
                this.mapSmsItemList.CopyTo(tmpMapSmsList);
                this.mapSmsItemList.Clear();

                for (int i = 0; i < tmpMapSmsList.Length; i++)
                {
                    if (tmpMapSmsList[i].FkSmsUser != _wsu.PKID)
                    {
                        this.mapSmsItemList.Add(tmpMapSmsList[i]);
                    }
                }

                if (this.onDelSmsUserEvt != null)
                {
                    this.onDelSmsUserEvt(this, new DelSmsUserEventArgs(_wsu));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.DelSmsUser() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS 사용자와 수신항목을 연결한 데이터를 저장한다.
        /// </summary>
        public void AddMapSMSDeviceItem(MapSmsDeviceItem _msu)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEMAPSMSDEVICEITEM(:1, :2, :3, :4, :5); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fksmsuser_in", oracle_parameter.OracleDataType.Int32, _msu.FkSmsUser, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fksmsdeviceitem_in", oracle_parameter.OracleDataType.Int32, _msu.FkDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isUse_in", oracle_parameter.OracleDataType.Varchar2, (_msu.IsUse == true) ? "1" : "0", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = ora.WorkSql(sBuilder.ToString(), inner_parameters, "MAPSMSDEVICEITEM");

                string mapSmsDeviceItemQuery = string.Format("SELECT pkid, fksmsdeviceitem, fksmsuser, isuse FROM mapsmsdeviceitem where fksmsuser = {0} and fksmsdeviceitem = {1}", _msu.FkSmsUser, _msu.FkDevice);
                this.mapSmsItemDT = this.ora.getDataTable(mapSmsDeviceItemQuery, "datarcv");

                for (int i = 0; i < this.mapSmsItemDT.Rows.Count; i++)
                {
                    object[] mapSmsDeviceItemObj = this.mapSmsItemDT.Rows[i].ItemArray;
                    MapSmsDeviceItem tmpSmsItem = new MapSmsDeviceItem(uint.Parse(mapSmsDeviceItemObj[0].ToString()), uint.Parse(mapSmsDeviceItemObj[1].ToString()), uint.Parse(mapSmsDeviceItemObj[2].ToString()), (mapSmsDeviceItemObj[3].ToString() == "1") ? true : false);
                    this.mapSmsItemList.Add(tmpSmsItem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddMapSMSDeviceItem() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS 사용자와 수신항목을 연결한 데이터를 수정한다.
        /// </summary>
        /// <param name="_msu"></param>
        public void UpdateMapSMSDeviceItem(MapSmsDeviceItem _msu)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEMAPSMSDEVICEITEM(:1, :2, :3, :4, :5); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, _msu.PKID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fksmsuser_in", oracle_parameter.OracleDataType.Int32, _msu.FkSmsUser, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fksmsdeviceitem_in", oracle_parameter.OracleDataType.Int32, _msu.FkDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isUse_in", oracle_parameter.OracleDataType.Varchar2, (_msu.IsUse == true) ? "1" : "0", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = ora.WorkSql(sBuilder.ToString(), inner_parameters, "MAPSMSDEVICEITEM");

                for (int i = 0; i < this.mapSmsItemList.Count; i++)
                {
                    if (this.mapSmsItemList[i].PKID == _msu.PKID)
                    {
                        this.mapSmsItemList[i].IsUse = _msu.IsUse;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.UpdateMapSMSDeviceItem() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 사용자와 측기를 연결시키는 데이터를 DB에 저장한다.
        /// </summary>
        /// <param name="_msu"></param>
        private void AddMapSmsUser(MapSmsUser _msu)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEMAPSMSUSER(:1, :2, :3, :4, :5,:6); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _msu.FkDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkSMSUser_in", oracle_parameter.OracleDataType.Int32, _msu.FkSmsUser, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isUse_in", oracle_parameter.OracleDataType.Char, "1", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "mapsmsuser");

                string smsQuery = string.Format("SELECT * FROM MAPSMSUSER WHERE ISUSE = 1 AND FKDEVICE = '{0}' AND FKSMSUSER = '{1}'",
                    _msu.FkDevice, _msu.FkSmsUser);
                this.mapSmsDT = this.ora.getDataTable(smsQuery, "datarcv");
                MapSmsUser tmpSmsUser = null;

                for (int i = 0; i < this.mapSmsDT.Rows.Count; i++)
                {
                    object[] smsUserObj = this.mapSmsDT.Rows[i].ItemArray;
                    tmpSmsUser = new MapSmsUser(uint.Parse(smsUserObj[0].ToString()), uint.Parse(smsUserObj[1].ToString()), uint.Parse(smsUserObj[2].ToString()));
                    this.mapSmsList.Add(tmpSmsUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddMapSmsUser() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 사용자와 측기를 연결시키는 데이터를 DB에서 삭제한다.
        /// </summary>
        /// <param name="_msu"></param>
        private void DelMapSmsUser(MapSmsUser _msu)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGEMAPSMSUSER(:1, :2, :3, :4, :5,:6); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("delete_flag", oracle_parameter.OracleDataType.Int32, 1, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, _msu.PKID, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _msu.FkDevice, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkSMSUser_in", oracle_parameter.OracleDataType.Int32, _msu.FkSmsUser, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("isUse_in", oracle_parameter.OracleDataType.Char, "0", ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "mapsmsuser");
                this.mapSmsList.Remove(this.getMapSmsUser(_msu.PKID));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.DelMapSmsUser() - {0}", ex.Message));
            }
        }

        /// <summary>
        /// SMS 전송 내역을 DB에 저장한다.
        /// </summary>
        /// <param name="_wss"></param>
        public void AddSmsSend(WSmsSend _wss)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("begin SP_MANAGESMSSEND(:1, :2, :3, :4, :5); end;");
                List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

                inner_parameters.Add(
                    new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("fkSMSUser_in", oracle_parameter.OracleDataType.Int32, _wss.FkSmsUser, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("sendtime_in", oracle_parameter.OracleDataType.Date, _wss.DDTime, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("message_in", oracle_parameter.OracleDataType.Char, _wss.Msg, ParameterDirection.Input));
                inner_parameters.Add(
                    new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

                int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "smssend");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.AddSmsSend(WSmsSend _wss) - {0}", ex.Message));
            }
        }

        /// <summary>
        /// 측기의 알람 이벤트를 DB에 저장한다.
        /// </summary>
        /// <param name="_wdai"></param>
        public void AddDeviceAlarmItems(WDeviceAlarmItems _wdai)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("begin SP_MANAGEDEVICEALARM(:1, :2, :3, :4, :5, :6); end;");
            List<oracle_parameter> inner_parameters = new List<oracle_parameter>();

            inner_parameters.Add(
                new oracle_parameter("pkid_in", oracle_parameter.OracleDataType.Int32, 0, ParameterDirection.Input));
            inner_parameters.Add(
                new oracle_parameter("fkdevice_in", oracle_parameter.OracleDataType.Int32, _wdai.FKDevice, ParameterDirection.Input));
            inner_parameters.Add(
                new oracle_parameter("fkdeviceitem_in", oracle_parameter.OracleDataType.Int32, _wdai.FKDeviceItem, ParameterDirection.Input));
            inner_parameters.Add(
                new oracle_parameter("chktime_in", oracle_parameter.OracleDataType.Date, _wdai.DDTime, ParameterDirection.Input));
            inner_parameters.Add(
                new oracle_parameter("resValue_in", oracle_parameter.OracleDataType.Char, _wdai.Value, ParameterDirection.Input));
            inner_parameters.Add(
                new oracle_parameter("retid_out", oracle_parameter.OracleDataType.Int32, DBNull.Value, ParameterDirection.Output));

            int inner_iCheck = this.ora.WorkSql(sBuilder.ToString(), inner_parameters, "devicealarm");

            if (this.onWDeviceAlarmEvt != null)
            {
                this.onWDeviceAlarmEvt(this, new WDeviceAlarmItemsEventArgs(_wdai));
            }
        }

        /// <summary>
        /// DB 설정 값 변경 시 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_port"></param>
        /// <param name="_id"></param>
        /// <param name="_pw"></param>
        /// <param name="_sid"></param>
        public void setDBData(string _ip, string _port, string _id, string _pw, string _sid)
        {
            if (this.onDBDataSetEvt != null)
            {
                this.onDBDataSetEvt(this, new SetDBDataEventArgs(_ip, _port, _id, _pw, _sid));
            }
        }

        /// <summary>
        /// PKID를 받아 SMS사용자와 측기가 연결된 상태 데이터를 검색하여 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <returns></returns>
        public MapSmsUser getMapSmsUser(uint _pkid)
        {
            MapSmsUser rst = new MapSmsUser();

            for (int i = 0; i < this.mapSmsList.Count; i++)
            {
                if (this.mapSmsList[i].PKID == _pkid)
                {
                    rst = this.mapSmsList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// 측기의 PKID를 받아 SMS사용자와 측기가 연결된 상태 데이터를 검색하여 반환한다.
        /// </summary>
        /// <param name="_dPkid"></param>
        /// <returns></returns>
        public List<MapSmsUser> getMapSmsUserList(uint _dPkid)
        {
            List<MapSmsUser> rst = new List<MapSmsUser>();

            for (int i = 0; i < this.mapSmsList.Count; i++)
            {
                if (this.mapSmsList[i].FkDevice == _dPkid)
                {
                    rst.Add(this.mapSmsList[i]);
                }
            }

            return rst;
        }

        /// <summary>
        /// PKID를 받아 SMS 사용자 리스트에서 검색하여 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <returns></returns>
        public WSmsUser getSmsUser(uint _pkid)
        {
            WSmsUser rst = new WSmsUser();

            for (int i = 0; i < this.smsUserList.Count; i++)
            {
                if (this.smsUserList[i].PKID == _pkid)
                {
                    rst = this.smsUserList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// SMS 사용자 PKID와 SMS 수신 항목 PKID를 인자로 받아 검색하여 반환한다.
        /// </summary>
        /// <param name="_pkid">
        /// SMS USER 테이블 PKID
        /// </param>
        /// <param name="_type">
        /// SMS ITEM 테이블 PKID
        /// </param>
        /// <returns></returns>
        public MapSmsDeviceItem getMapSmsItem(uint _pkid, SMSType _type)
        {
            MapSmsDeviceItem rst = new MapSmsDeviceItem();

            for (int i = 0; i < this.mapSmsItemList.Count; i++)
            {
                if (this.mapSmsItemList[i].FkSmsUser == _pkid)
                {
                    if (this.mapSmsItemList[i].FkDevice == (uint)_type)
                    {
                        rst = this.mapSmsItemList[i];
                        break;
                    }
                }
            }

            return rst;
        }

        /// <summary>
        /// 측기 리스트에서 중복 여부를 판단해 리턴한다.
        /// </summary>
        /// <param name="_wd"></param>
        /// <returns>
        /// true - 중복, false - 없음
        /// </returns>
        public bool GetWDeviceComparer(WDevice _wd)
        {
            bool rst = false;

            for (int i = 0; i < this.deviceList.Count; i++)
            {
                if (this.deviceList[i].ID == _wd.ID)
                {
                    if (this.deviceList[i].TypeDevice == _wd.TypeDevice)
                    {
                        rst = true;
                    }
                }
            }

            return rst;
        }

        /// <summary>
        /// 측기종류의 이름을 받아 WTypeDevice 클래스를 반환한다.
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        public WTypeDevice GetTypeDevice(string _name)
        {
            WTypeDevice rst = new WTypeDevice();

            for (int i = 0; i < this.typeDeviceList.Count; i++)
            {
                if (this.typeDeviceList[i].Name == _name)
                {
                    rst = this.typeDeviceList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// 측기종류의 pkid를 받아 WTypeDevice 클래스를 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <returns></returns>
        public WTypeDevice GetTypeDevice(uint _pkid)
        {
            WTypeDevice rst = new WTypeDevice();

            for (int i = 0; i < this.typeDeviceList.Count; i++)
            {
                if (this.typeDeviceList[i].PKID == _pkid)
                {
                    rst = this.typeDeviceList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// 측기의 pkid를 받아 WDevice 클래스를 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <returns></returns>
        public WDevice GetWDevice(uint _pkid)
        {
            WDevice rst = new WDevice();

            for (int i = 0; i < this.deviceList.Count; i++)
            {
                if (this.deviceList[i].PKID == _pkid)
                {
                    rst = this.deviceList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// ID를 인자로 받아 RAT 측기 중에 해당 측기를 WDevice 클래스로 반환한다.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WDevice GetRatDevice(string _id)
        {
            try
            {
                WDevice rst = new WDevice();
                WTypeDevice tmp = new WTypeDevice();

                for (int i = 0; i < this.typeDeviceList.Count; i++)
                {
                    if (this.typeDeviceList[i].Name == "RAT")
                    {
                        tmp = this.typeDeviceList[i];
                        break;
                    }
                }

                for (int i = 0; i < this.deviceList.Count; i++)
                {
                    if (this.deviceList[i].TypeDevice == tmp.PKID)
                    {
                        if (this.deviceList[i].ID == _id)
                        {
                            rst = this.deviceList[i];
                            break;
                        }
                    }
                }

                return rst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.GetRatDevice() - {0}", ex.Message));
                return new WDevice();
            }
        }

        /// <summary>
        /// SMS 사용자의 PKID를 받아 등록된 측기 리스트를 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <returns></returns>
        public List<WDevice> GetWDeviceForSmsUser(uint _pkid)
        {
            List<WDevice> wDeviceList = new List<WDevice>();

            for (int i = 0; i < this.mapSmsList.Count; i++)
            {
                if (this.mapSmsList[i].FkSmsUser == _pkid)
                {
                    wDeviceList.Add(this.GetWDevice(this.mapSmsList[i].FkDevice));
                }
            }

            return wDeviceList;
        }

        /// <summary>
        /// ID를 인자로 받아 HSD 측기 중에 해당 측기를 WDevice 클래스로 반환한다.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WDevice GetHSDDevice(string _id)
        {
            try
            {
                WDevice rst = new WDevice();
                WTypeDevice tmp = new WTypeDevice();

                for (int i = 0; i < this.typeDeviceList.Count; i++)
                {
                    if (this.typeDeviceList[i].Name == "HSD")
                    {
                        tmp = this.typeDeviceList[i];
                        break;
                    }
                }

                for (int i = 0; i < this.deviceList.Count; i++)
                {
                    if (this.deviceList[i].TypeDevice == tmp.PKID)
                    {
                        if (this.deviceList[i].ID == _id)
                        {
                            rst = this.deviceList[i];
                            break;
                        }
                    }
                }

                return rst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.GetHSDDevice() - {0}", ex.Message));
                return new WDevice();
            }
        }

        /// <summary>
        /// ID를 인자로 받아 DSD 측기 중에 해당 측기를 WDevice 클래스로 반환한다.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WDevice GetDSDDevice(string _id)
        {
            try
            {
                WDevice rst = new WDevice();
                WTypeDevice tmp = new WTypeDevice();

                for (int i = 0; i < this.typeDeviceList.Count; i++)
                {
                    if (this.typeDeviceList[i].Name == "DSD")
                    {
                        tmp = this.typeDeviceList[i];
                        break;
                    }
                }

                for (int i = 0; i < this.deviceList.Count; i++)
                {
                    if (this.deviceList[i].TypeDevice == tmp.PKID)
                    {
                        if (this.deviceList[i].ID == _id)
                        {
                            rst = this.deviceList[i];
                            break;
                        }
                    }
                }

                return rst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.GetDSDDevice() - {0}", ex.Message));
                return new WDevice();
            }
        }

        /// <summary>
        /// ID를 인자로 받아 WOU 측기 중에 해당 측기를 WDevice 클래스로 반환한다.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WDevice GetWouDevice(string _id)
        {
            try
            {
                WDevice rst = new WDevice();
                WTypeDevice tmp = new WTypeDevice();

                for (int i = 0; i < this.typeDeviceList.Count; i++)
                {
                    if (this.typeDeviceList[i].Name == "WOU")
                    {
                        tmp = this.typeDeviceList[i];
                        break;
                    }
                }

                for (int i = 0; i < this.deviceList.Count; i++)
                {
                    if (this.deviceList[i].TypeDevice == tmp.PKID)
                    {
                        if (this.deviceList[i].ID == _id)
                        {
                            rst = this.deviceList[i];
                            break;
                        }
                    }
                }

                return rst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.GetWouDevice() - {0}", ex.Message));
                return new WDevice();
            }
        }

        /// <summary>
        /// 이더넷 클라이언트 리스트에서 해당 ID를 가진 클라이언트를 찾아 반환한다.
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public EthernetClient getEthernetClient(string _id)
        {
            EthernetClient rst = new EthernetClient();

            for (int i = 0; i < this.e_ClientList.Count; i++)
            {
                if (this.e_ClientList[i].ID == _id)
                {
                    rst = this.e_ClientList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// deviceItem의 enum 값을 받아 WTypeDeviceItem 클래스를 반환한다.
        /// </summary>
        /// <param name="_wiType"></param>
        /// <returns></returns>
        public WTypeDeviceItem GetTypeDeviceItem(WIType _wiType)
        {
            WTypeDeviceItem rst = new WTypeDeviceItem();

            for (int i = 0; i < this.typeDeviceItemList.Count; i++)
            {
                if (this.typeDeviceItemList[i].PKID == (uint)_wiType)
                {
                    rst = this.typeDeviceItemList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// deviceItem의 pkid 값을 받아 WTypeDeviceItem 클래스를 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        /// <returns></returns>
        public WTypeDeviceItem GetTypeDeviceItem(uint _pkid)
        {
            WTypeDeviceItem rst = new WTypeDeviceItem();

            for (int i = 0; i < this.typeDeviceItemList.Count; i++)
            {
                if (this.typeDeviceItemList[i].PKID == _pkid)
                {
                    rst = this.typeDeviceItemList[i];
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// SMS 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_cellNum"></param>
        /// <param name="_msg"></param>
        public void SendSmsMsg(string _cellNum, byte[] _msg)
        {
            if (this.onSendSmsMsgEvt != null)
            {
                this.onSendSmsMsgEvt(this, new SendSmsMsgEventArgs(_cellNum, _msg));
            }
        }

        /// <summary>
        /// 이더넷 데이터 전송 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_msg"></param>
        public void SendEthernetMsg(string _id, byte[] _msg)
        {
            if (this.onEthernetClientSendEvt != null)
            {
                this.onEthernetClientSendEvt(this, new EthernetClientSendEventArgs(_id, _msg));
            }
        }

        /// <summary>
        /// SMS User에게 전송하는 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="_cellNum"></param>
        /// <param name="_msg"></param>
        public void SendSmsUserMsg(string _cellNum, byte[] _msg)
        {
            if (this.onSendSmsUserMsgEvt != null)
            {
                this.onSendSmsUserMsgEvt(this, new SendSmsMsgEventArgs(_cellNum, _msg));
            }
        }

        /// <summary>
        /// WOU 측기에 상태요청을 한다.(기상데이터, 임계치, 무시시간 요청)
        /// </summary>
        /// <param name="_pkid"></param>
        public void WOUWDeviceRequest(uint _pkid)
        {
            if (this.onWOURequestEvt != null)
            {
                this.onWOURequestEvt(this, new WOUWDeviceRequestEventArgs(_pkid));
            }
        }

        /// <summary>
        /// WOU 측기의 임계치 제어 요청을 한다.
        /// </summary>
        /// <param name="_sKind"></param>
        /// <param name="_alarm1"></param>
        /// <param name="_alarm2"></param>
        /// <param name="_alarm3"></param>
        public void WOUWDeviceAlarmCtr(uint _fkDevice, byte _sKind, string _alarm1, string _alarm2, string _alarm3)
        {
            if (this.onWOUAlarmControlEvt != null)
            {
                this.onWOUAlarmControlEvt(this, new WOUWDeviceControlAlarmEventArgs(_fkDevice, _sKind, _alarm1, _alarm2, _alarm3));
            }
        }

        /// <summary>
        /// WOU 측기의 무시시간 제어 요청을 한다.
        /// </summary>
        /// <param name="_sameTime"></param>
        /// <param name="_downTime"></param>
        public void WOUWDeviceFTimeCtr(uint _fkDevice, string _sameTime, string _downTime)
        {
            if (this.onWOUFTimeControlEvt != null)
            {
                this.onWOUFTimeControlEvt(this, new WOUWDeviceControlFTimeEventArgs(_fkDevice, _sameTime, _downTime));
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.ora != null)
            {
                this.ora.closeDb();
            }
        }

        /// <summary>
        /// 전화번호를 받아 유무를 반환한다.
        /// </summary>
        /// <param name="_cell"></param>
        /// <returns>
        /// 유 - true, 무 - false
        /// </returns>
        public bool SmsUserComparer(string _cell)
        {
            bool rst = false;

            for (int i = 0; i < this.smsUserList.Count; i++)
            {
                if (this.smsUserList[i].TelNum.Replace("-", "") == _cell.Replace("-", ""))
                {
                    rst = true;
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// 전화번호를 받아 WSmsUser 클래스를 반환한다.
        /// </summary>
        /// <param name="_cell"></param>
        /// <returns></returns>
        public List<WSmsUser> getSmsUser(string _cell)
        {
            try
            {
                List<WSmsUser> rst = new List<WSmsUser>();

                for (int i = 0; i < this.smsUserList.Count; i++)
                {
                    if (this.smsUserList[i].TelNum.Replace("-", "") == _cell.Replace("-", ""))
                    {
                        rst.Add(this.smsUserList[i]);
                    }
                }

                return rst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.getSmsUser(string _cell) - {0}", ex.Message));
                return new List<WSmsUser>();
            }
        }

        /// <summary>
        /// 측기의 PKID를 받아 SMS 사용자를 반환한다.
        /// </summary>
        /// <param name="_pkid"></param>
        public List<WSmsUser> getSmsUserList(uint _pkid)
        {
            try
            {
                List<MapSmsUser> tmpMapSmsUserList = new List<MapSmsUser>();
                List<WSmsUser> tmpSmsuserList = new List<WSmsUser>();

                for (int i = 0; i < this.mapSmsList.Count; i++)
                {
                    if (this.mapSmsList[i].FkDevice == _pkid)
                    {
                        tmpMapSmsUserList.Add(this.mapSmsList[i]);
                    }
                }

                for (int i = 0; i < tmpMapSmsUserList.Count; i++)
                {
                    for (int k = 0; k < this.smsUserList.Count; k++)
                    {
                        if (tmpMapSmsUserList[i].FkSmsUser == this.smsUserList[k].PKID)
                        {
                            tmpSmsuserList.Add(this.smsUserList[k]);
                        }
                    }
                }

                return tmpSmsuserList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("WeatherDataMng.getSmsUser(uint _pkid) - {0}", ex.Message));
                return new List<WSmsUser>();
            }
        }
    }

    /// <summary>
    /// DB 연결 후 성공/실패 이벤트에 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class DBConEventArgs : EventArgs
    {
        private bool rst = false;

        public bool Rst
        {
            get { return this.rst; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_rst"></param>
        public DBConEventArgs(bool _rst)
        {
            this.rst = _rst;
        }
    }

    /// <summary>
    /// DB 연결을 테스트 한 후 결과에 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class DBConTestEventArgs : EventArgs
    {
        private bool rst = false;

        public bool Rst
        {
            get { return this.rst; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_rst"></param>
        public DBConTestEventArgs(bool _rst)
        {
            this.rst = _rst;
        }
    }

    /// <summary>
    /// 측기를 추가할 때 쓰이는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddWDeviceEventArgs : EventArgs
    {
        private WDevice wd;

        public WDevice WD
        {
            get { return this.wd; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wd"></param>
        public AddWDeviceEventArgs(WDevice _wd)
        {
            this.wd = _wd;
        }
    }

    /// <summary>
    /// 측기를 수정하고 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class UpdateWDeviceEventArgs : EventArgs
    {
        private WDevice wd;

        public WDevice WD
        {
            get { return this.wd; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wd"></param>
        public UpdateWDeviceEventArgs(WDevice _wd)
        {
            this.wd = _wd;
        }
    }

    /// <summary>
    /// 측기를 삭제하고 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class DeleteWDeviceEventArgs : EventArgs
    {
        private List<WDevice> wdList;

        public List<WDevice> WDList
        {
            get { return this.wdList; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wdList"></param>
        public DeleteWDeviceEventArgs(List<WDevice> _wdList)
        {
            this.wdList = _wdList;
        }
    }

    /// <summary>
    /// 강수 데이터를 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddRainDataEventArgs : EventArgs
    {
        private WRainData wr;

        public WRainData WR
        {
            get { return this.wr; }
            set { this.wr = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wr"></param>
        public AddRainDataEventArgs(WRainData _wr)
        {
            this.wr = _wr;
        }
    }

    /// <summary>
    /// 수위 데이터를 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddWaterLevelEventArgs : EventArgs
    {
        private WWaterLevelData wwl;

        public WWaterLevelData WWL
        {
            get { return this.wwl; }
            set { this.wwl = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wwl"></param>
        public AddWaterLevelEventArgs(WWaterLevelData _wwl)
        {
            this.wwl = _wwl;
        }
    }

    /// <summary>
    /// 유속 데이터를 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddFlowSpeedEventArgs : EventArgs
    {
        private WFlowSpeedData wfs;

        public WFlowSpeedData WFS
        {
            get { return this.wfs; }
            set { this.wfs = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wfs"></param>
        public AddFlowSpeedEventArgs(WFlowSpeedData _wfs)
        {
            this.wfs = _wfs;
        }
    }

    /// <summary>
    /// 임계치 알람 데이터를 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddAlarmEventArgs : EventArgs
    {
        private WAlarmData wa;

        public WAlarmData WA
        {
            get { return this.wa; }
            set { this.wa = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wa"></param>
        public AddAlarmEventArgs(WAlarmData _wa)
        {
            this.wa = _wa;
        }
    }

    /// <summary>
    /// 측기 상태응답 및 제어응답에 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddWDeviceItemDataEventArgs : EventArgs
    {
        private WDeviceItem wdi;

        public WDeviceItem WDI
        {
            get { return this.wdi; }
            set { this.wdi = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_cmd"></param>
        /// <param name="_typeClass"></param>
        public AddWDeviceItemDataEventArgs(WDeviceItem _wdi)
        {
            this.wdi = _wdi;
        }
    }

    /// <summary>
    /// 측기에 요청 및 제어할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddWDeviceRequestEventArgs : EventArgs
    {
        private WDeviceRequest wdr;

        public WDeviceRequest WDR
        {
            get { return this.wdr; }
            set { this.wdr = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wdr"></param>
        public AddWDeviceRequestEventArgs(WDeviceRequest _wdr)
        {
            this.wdr = _wdr;
        }
    }

    /// <summary>
    /// SMS를 전송할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class SendSmsMsgEventArgs : EventArgs
    {
        private string cellNum = string.Empty;
        byte[] msg = new byte[0];

        public string CellNum
        {
            get { return this.cellNum; }
            set { this.cellNum = value; }
        }

        public byte[] Msg
        {
            get { return this.msg; }
            set { this.msg = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_cellNum"></param>
        /// <param name="_msg"></param>
        public SendSmsMsgEventArgs(string _cellNum, byte[] _msg)
        {
            this.cellNum = _cellNum;
            this.msg = _msg;
        }
    }

    /// <summary>
    /// WOU 측기에 상태 요청을 할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class WOUWDeviceRequestEventArgs : EventArgs
    {
        private uint pkid = uint.MinValue;

        public uint PKID
        {
            get { return this.pkid; }
            set { this.pkid = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_pkid"></param>
        public WOUWDeviceRequestEventArgs(uint _pkid)
        {
            this.pkid = _pkid;
        }
    }

    /// <summary>
    /// WOU 측기에 임계치 제어를 할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class WOUWDeviceControlAlarmEventArgs : EventArgs
    {
        private uint fkDevice = uint.MinValue;
        private byte sKind = byte.MinValue;
        private string alarm1 = string.Empty;
        private string alarm2 = string.Empty;
        private string alarm3 = string.Empty;

        public uint FKDevice
        {
            get { return this.fkDevice; }
            set { this.fkDevice = value; }
        }

        public byte SKind
        {
            get { return this.sKind; }
            set { this.sKind = value; }
        }

        public string Alarm1
        {
            get { return this.alarm1; }
            set { this.alarm1 = value; }
        }

        public string Alarm2
        {
            get { return this.alarm2; }
            set { this.alarm2 = value; }
        }

        public string Alarm3
        {
            get { return this.alarm3; }
            set { this.alarm3 = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_sKind"></param>
        /// <param name="_alarm1"></param>
        /// <param name="_alarm2"></param>
        /// <param name="_alarm3"></param>
        public WOUWDeviceControlAlarmEventArgs(uint _fkDevice, byte _sKind, string _alarm1, string _alarm2, string _alarm3)
        {
            this.fkDevice = _fkDevice;
            this.sKind = _sKind;
            this.alarm1 = _alarm1;
            this.alarm2 = _alarm2;
            this.alarm3 = _alarm3;
        }
    }

    /// <summary>
    /// WOU 측기에 무시시간 제어를 할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class WOUWDeviceControlFTimeEventArgs : EventArgs
    {
        private uint fkDevice = uint.MinValue;
        private string sameTime = string.Empty;
        private string downTime = string.Empty;

        public uint FKDevice
        {
            get { return this.fkDevice; }
            set { this.fkDevice = value; }
        }

        public string SameTime
        {
            get { return this.sameTime; }
            set { this.sameTime = value; }
        }

        public string DownTime
        {
            get { return this.downTime; }
            set { this.downTime = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_sameTime"></param>
        /// <param name="_downTime"></param>
        public WOUWDeviceControlFTimeEventArgs(uint _fkDevice, string _sameTime, string _downTime)
        {
            this.fkDevice = _fkDevice;
            this.sameTime = _sameTime;
            this.downTime = _downTime;
        }
    }

    /// <summary>
    /// 가장 최근의 강수 데이터를 가져오는 이벤트 아규먼트 클래스
    /// </summary>
    public class LatelyRainDataEventArgs : EventArgs
    {
        private WRainData wr = new WRainData();

        public WRainData WR
        {
            get { return this.wr; }
            set { this.wr = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wr"></param>
        public LatelyRainDataEventArgs(WRainData _wr)
        {
            this.wr = _wr;
        }
    }

    /// <summary>
    /// 가장 최근의 수위 데이터를 가져오는 이벤트 아규먼트 클래스
    /// </summary>
    public class LatelyWaterLevelDataEventArgs : EventArgs
    {
        private WWaterLevelData wl = new WWaterLevelData();

        public WWaterLevelData WL
        {
            get { return this.wl; }
            set { this.wl = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wl"></param>
        public LatelyWaterLevelDataEventArgs(WWaterLevelData _wl)
        {
            this.wl = _wl;
        }
    }

    /// <summary>
    /// 가장 최근의 유속 데이터를 가져오는 이벤트 아규먼트 클래스
    /// </summary>
    public class LatelyFlowSpeedDataEventArgs : EventArgs
    {
        private WFlowSpeedData fs = new WFlowSpeedData();

        public WFlowSpeedData FS
        {
            get { return this.fs; }
            set { this.fs = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_fs"></param>
        public LatelyFlowSpeedDataEventArgs(WFlowSpeedData _fs)
        {
            this.fs = _fs;
        }
    }

    /// <summary>
    /// SMS 사용자를 등록할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class AddSmsUserEventArgs : EventArgs
    {
        private WSmsUser wsu = new WSmsUser();

        public WSmsUser WSU
        {
            get { return this.wsu; }
            set { this.wsu = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wsu"></param>
        public AddSmsUserEventArgs(WSmsUser _wsu)
        {
            this.wsu = _wsu;
        }
    }

    /// <summary>
    /// SMS 사용자를 수정할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class UpdateSmsUserEventArgs : EventArgs
    {
        private WSmsUser wsu = new WSmsUser();

        public WSmsUser WSU
        {
            get { return this.wsu; }
            set { this.wsu = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wsu"></param>
        public UpdateSmsUserEventArgs(WSmsUser _wsu)
        {
            this.wsu = _wsu;
        }
    }

    /// <summary>
    /// SMS 사용자를 삭제할 때 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class DelSmsUserEventArgs : EventArgs
    {
        private WSmsUser wsu = new WSmsUser();

        public WSmsUser WSU
        {
            get { return this.wsu; }
            set { this.wsu = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wsu"></param>
        public DelSmsUserEventArgs(WSmsUser _wsu)
        {
            this.wsu = _wsu;
        }
    }

    /// <summary>
    /// DB 설정 값 변경 시 발생하는 이벤트 아규먼트 클래스
    /// </summary>
    public class SetDBDataEventArgs : EventArgs
    {
        private string ip = string.Empty;
        private string port = string.Empty;
        private string id = string.Empty;
        private string pw = string.Empty;
        private string sid = string.Empty;

        public string Ip
        {
            get { return this.ip; }
            set { this.ip = value; }
        }

        public string Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Pw
        {
            get { return this.pw; }
            set { this.pw = value; }
        }

        public string Sid
        {
            get { return this.sid; }
            set { this.sid = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_port"></param>
        /// <param name="_id"></param>
        /// <param name="_pw"></param>
        /// <param name="_sid"></param>
        public SetDBDataEventArgs(string _ip, string _port, string _id, string _pw, string _sid)
        {
            this.ip = _ip;
            this.port = _port;
            this.id = _id;
            this.pw = _pw;
            this.sid = _sid;
        }
    }

    /// <summary>
    /// 측기의 알람 시 발생하는 이벤트 아규먼트 클래스
    /// </summary>
    public class WDeviceAlarmItemsEventArgs : EventArgs
    {
        private WDeviceAlarmItems wDeviceAlarmItems = new WDeviceAlarmItems();

        public WDeviceAlarmItems WDeviceAlarm
        {
            get { return this.wDeviceAlarmItems; }
            set { this.wDeviceAlarmItems = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_wda"></param>
        public WDeviceAlarmItemsEventArgs(WDeviceAlarmItems _wda)
        {
            this.wDeviceAlarmItems = _wda;
        }
    }

    /// <summary>
    /// 이더넷 클라이언트로 전송하는 이벤트 아규먼트 클래스
    /// </summary>
    public class EthernetClientSendEventArgs : EventArgs
    {
        private string id = string.Empty;
        private byte[] buff;

        public string ID
        {
            get { return this.id; }
        }

        public byte[] Buff
        {
            get { return this.buff; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_buff"></param>
        public EthernetClientSendEventArgs(string _id, byte[] _buff)
        {
            this.id = _id;
            this.buff = _buff;
        }
    }
}