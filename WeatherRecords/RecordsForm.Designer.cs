namespace ADEng.Module.WeatherSystem
{
    partial class RecordsForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordsForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabWeatherInfo = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lstWeather = new System.Windows.Forms.ListView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbxTypeWeather = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearchWeather = new System.Windows.Forms.Button();
            this.btnPrintWeather = new System.Windows.Forms.Button();
            this.cbxDeviceNameWeather = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_fromTo = new System.Windows.Forms.Label();
            this.dtpToWeather = new System.Windows.Forms.DateTimePicker();
            this.dtpFromWeather = new System.Windows.Forms.DateTimePicker();
            this.label = new System.Windows.Forms.Label();
            this.tabAlarmInfo = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lstAlarm = new System.Windows.Forms.ListView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbxTypeWeatherAlarm = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSearchAlarm = new System.Windows.Forms.Button();
            this.btnPrintAlarm = new System.Windows.Forms.Button();
            this.cbxDeviceNameAlarm = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpToAlarm = new System.Windows.Forms.DateTimePicker();
            this.dtpFromAlarm = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.tabDeviceStatus = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lstDevice = new System.Windows.Forms.ListView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dtpFromDevice = new System.Windows.Forms.DateTimePicker();
            this.cbxDeviceNameStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearchDevice = new System.Windows.Forms.Button();
            this.btnPrintDevice = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpToDevice = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lstDeviceAlarm = new System.Windows.Forms.ListView();
            this.panel9 = new System.Windows.Forms.Panel();
            this.dtpFromDeviceAlarm = new System.Windows.Forms.DateTimePicker();
            this.cbxDeviceNameDeviceAlarm = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.bntSearchDeviceAlarm = new System.Windows.Forms.Button();
            this.btnPrintDeviceAlarm = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.dtpToDeviceAlarm = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabWeatherInfo.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabAlarmInfo.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabDeviceStatus.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(977, 663);
            this.panel2.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabWeatherInfo);
            this.tabControl1.Controls.Add(this.tabAlarmInfo);
            this.tabControl1.Controls.Add(this.tabDeviceStatus);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(977, 663);
            this.tabControl1.TabIndex = 0;
            // 
            // tabWeatherInfo
            // 
            this.tabWeatherInfo.BackColor = System.Drawing.Color.Transparent;
            this.tabWeatherInfo.Controls.Add(this.panel4);
            this.tabWeatherInfo.Controls.Add(this.panel3);
            this.tabWeatherInfo.Location = new System.Drawing.Point(4, 21);
            this.tabWeatherInfo.Name = "tabWeatherInfo";
            this.tabWeatherInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabWeatherInfo.Size = new System.Drawing.Size(969, 638);
            this.tabWeatherInfo.TabIndex = 0;
            this.tabWeatherInfo.Text = "기상정보";
            this.tabWeatherInfo.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lstWeather);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 33);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(963, 602);
            this.panel4.TabIndex = 1;
            // 
            // lstWeather
            // 
            this.lstWeather.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstWeather.FullRowSelect = true;
            this.lstWeather.GridLines = true;
            this.lstWeather.Location = new System.Drawing.Point(3, 3);
            this.lstWeather.Name = "lstWeather";
            this.lstWeather.Size = new System.Drawing.Size(953, 595);
            this.lstWeather.TabIndex = 0;
            this.lstWeather.UseCompatibleStateImageBehavior = false;
            this.lstWeather.View = System.Windows.Forms.View.Details;
            this.lstWeather.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.Weather_ListView_ColumnClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbxTypeWeather);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.btnSearchWeather);
            this.panel3.Controls.Add(this.btnPrintWeather);
            this.panel3.Controls.Add(this.cbxDeviceNameWeather);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.lb_fromTo);
            this.panel3.Controls.Add(this.dtpToWeather);
            this.panel3.Controls.Add(this.dtpFromWeather);
            this.panel3.Controls.Add(this.label);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(963, 30);
            this.panel3.TabIndex = 0;
            // 
            // cbxTypeWeather
            // 
            this.cbxTypeWeather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTypeWeather.FormattingEnabled = true;
            this.cbxTypeWeather.Location = new System.Drawing.Point(471, 3);
            this.cbxTypeWeather.Name = "cbxTypeWeather";
            this.cbxTypeWeather.Size = new System.Drawing.Size(79, 20);
            this.cbxTypeWeather.TabIndex = 92;
            this.cbxTypeWeather.SelectedIndexChanged += new System.EventHandler(this.cbxTypeWeatherIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(408, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 91;
            this.label3.Text = "관측종류";
            // 
            // btnSearchWeather
            // 
            this.btnSearchWeather.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchWeather.Location = new System.Drawing.Point(800, 1);
            this.btnSearchWeather.Name = "btnSearchWeather";
            this.btnSearchWeather.Size = new System.Drawing.Size(75, 23);
            this.btnSearchWeather.TabIndex = 90;
            this.btnSearchWeather.Text = "조회";
            this.btnSearchWeather.UseVisualStyleBackColor = true;
            this.btnSearchWeather.Click += new System.EventHandler(this.btnWeatherSearchClick);
            // 
            // btnPrintWeather
            // 
            this.btnPrintWeather.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintWeather.Location = new System.Drawing.Point(881, 1);
            this.btnPrintWeather.Name = "btnPrintWeather";
            this.btnPrintWeather.Size = new System.Drawing.Size(75, 23);
            this.btnPrintWeather.TabIndex = 89;
            this.btnPrintWeather.Text = "출력";
            this.btnPrintWeather.UseVisualStyleBackColor = true;
            this.btnPrintWeather.Click += new System.EventHandler(this.btnPrintWeather_Click);
            // 
            // cbxDeviceNameWeather
            // 
            this.cbxDeviceNameWeather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDeviceNameWeather.FormattingEnabled = true;
            this.cbxDeviceNameWeather.Location = new System.Drawing.Point(620, 3);
            this.cbxDeviceNameWeather.Name = "cbxDeviceNameWeather";
            this.cbxDeviceNameWeather.Size = new System.Drawing.Size(133, 20);
            this.cbxDeviceNameWeather.TabIndex = 88;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(570, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 12);
            this.label2.TabIndex = 87;
            this.label2.Text = "측기명";
            // 
            // lb_fromTo
            // 
            this.lb_fromTo.AutoSize = true;
            this.lb_fromTo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_fromTo.Location = new System.Drawing.Point(224, 6);
            this.lb_fromTo.Name = "lb_fromTo";
            this.lb_fromTo.Size = new System.Drawing.Size(15, 12);
            this.lb_fromTo.TabIndex = 86;
            this.lb_fromTo.Text = "~";
            // 
            // dtpToWeather
            // 
            this.dtpToWeather.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpToWeather.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToWeather.Location = new System.Drawing.Point(245, 2);
            this.dtpToWeather.Name = "dtpToWeather";
            this.dtpToWeather.Size = new System.Drawing.Size(153, 21);
            this.dtpToWeather.TabIndex = 85;
            // 
            // dtpFromWeather
            // 
            this.dtpFromWeather.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpFromWeather.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromWeather.Location = new System.Drawing.Point(66, 2);
            this.dtpFromWeather.Name = "dtpFromWeather";
            this.dtpFromWeather.Size = new System.Drawing.Size(152, 21);
            this.dtpFromWeather.TabIndex = 84;
            this.dtpFromWeather.Value = new System.DateTime(2010, 9, 8, 0, 0, 0, 0);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label.Location = new System.Drawing.Point(5, 6);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(57, 12);
            this.label.TabIndex = 83;
            this.label.Text = "조회기간";
            // 
            // tabAlarmInfo
            // 
            this.tabAlarmInfo.BackColor = System.Drawing.Color.Transparent;
            this.tabAlarmInfo.Controls.Add(this.panel6);
            this.tabAlarmInfo.Controls.Add(this.panel5);
            this.tabAlarmInfo.Location = new System.Drawing.Point(4, 21);
            this.tabAlarmInfo.Name = "tabAlarmInfo";
            this.tabAlarmInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlarmInfo.Size = new System.Drawing.Size(969, 638);
            this.tabAlarmInfo.TabIndex = 1;
            this.tabAlarmInfo.Text = "임계치정보";
            this.tabAlarmInfo.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.lstAlarm);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 33);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(963, 602);
            this.panel6.TabIndex = 2;
            // 
            // lstAlarm
            // 
            this.lstAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAlarm.FullRowSelect = true;
            this.lstAlarm.GridLines = true;
            this.lstAlarm.Location = new System.Drawing.Point(3, 3);
            this.lstAlarm.Name = "lstAlarm";
            this.lstAlarm.Size = new System.Drawing.Size(953, 592);
            this.lstAlarm.TabIndex = 0;
            this.lstAlarm.UseCompatibleStateImageBehavior = false;
            this.lstAlarm.View = System.Windows.Forms.View.Details;
            this.lstAlarm.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.Alarm_ListView_ColumnClick);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cbxTypeWeatherAlarm);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.btnSearchAlarm);
            this.panel5.Controls.Add(this.btnPrintAlarm);
            this.panel5.Controls.Add(this.cbxDeviceNameAlarm);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.dtpToAlarm);
            this.panel5.Controls.Add(this.dtpFromAlarm);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(963, 30);
            this.panel5.TabIndex = 1;
            // 
            // cbxTypeWeatherAlarm
            // 
            this.cbxTypeWeatherAlarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTypeWeatherAlarm.FormattingEnabled = true;
            this.cbxTypeWeatherAlarm.Location = new System.Drawing.Point(471, 3);
            this.cbxTypeWeatherAlarm.Name = "cbxTypeWeatherAlarm";
            this.cbxTypeWeatherAlarm.Size = new System.Drawing.Size(79, 20);
            this.cbxTypeWeatherAlarm.TabIndex = 94;
            this.cbxTypeWeatherAlarm.SelectedIndexChanged += new System.EventHandler(this.cbxTypeWeatherAlarm_IndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(408, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 12);
            this.label10.TabIndex = 93;
            this.label10.Text = "관측종류";
            // 
            // btnSearchAlarm
            // 
            this.btnSearchAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchAlarm.Location = new System.Drawing.Point(800, 1);
            this.btnSearchAlarm.Name = "btnSearchAlarm";
            this.btnSearchAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnSearchAlarm.TabIndex = 90;
            this.btnSearchAlarm.Text = "조회";
            this.btnSearchAlarm.UseVisualStyleBackColor = true;
            this.btnSearchAlarm.Click += new System.EventHandler(this.btnSearchAlarm_Click);
            // 
            // btnPrintAlarm
            // 
            this.btnPrintAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintAlarm.Location = new System.Drawing.Point(881, 1);
            this.btnPrintAlarm.Name = "btnPrintAlarm";
            this.btnPrintAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnPrintAlarm.TabIndex = 89;
            this.btnPrintAlarm.Text = "출력";
            this.btnPrintAlarm.UseVisualStyleBackColor = true;
            this.btnPrintAlarm.Click += new System.EventHandler(this.btnPrintAlarm_Click);
            // 
            // cbxDeviceNameAlarm
            // 
            this.cbxDeviceNameAlarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDeviceNameAlarm.FormattingEnabled = true;
            this.cbxDeviceNameAlarm.Location = new System.Drawing.Point(620, 3);
            this.cbxDeviceNameAlarm.Name = "cbxDeviceNameAlarm";
            this.cbxDeviceNameAlarm.Size = new System.Drawing.Size(133, 20);
            this.cbxDeviceNameAlarm.TabIndex = 88;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(570, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 87;
            this.label5.Text = "측기명";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(224, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 12);
            this.label6.TabIndex = 86;
            this.label6.Text = "~";
            // 
            // dtpToAlarm
            // 
            this.dtpToAlarm.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpToAlarm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToAlarm.Location = new System.Drawing.Point(245, 2);
            this.dtpToAlarm.Name = "dtpToAlarm";
            this.dtpToAlarm.Size = new System.Drawing.Size(153, 21);
            this.dtpToAlarm.TabIndex = 85;
            // 
            // dtpFromAlarm
            // 
            this.dtpFromAlarm.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpFromAlarm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromAlarm.Location = new System.Drawing.Point(66, 2);
            this.dtpFromAlarm.Name = "dtpFromAlarm";
            this.dtpFromAlarm.Size = new System.Drawing.Size(152, 21);
            this.dtpFromAlarm.TabIndex = 84;
            this.dtpFromAlarm.Value = new System.DateTime(2010, 9, 8, 0, 0, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(5, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 12);
            this.label7.TabIndex = 83;
            this.label7.Text = "조회기간";
            // 
            // tabDeviceStatus
            // 
            this.tabDeviceStatus.BackColor = System.Drawing.Color.Transparent;
            this.tabDeviceStatus.Controls.Add(this.panel8);
            this.tabDeviceStatus.Controls.Add(this.panel7);
            this.tabDeviceStatus.Location = new System.Drawing.Point(4, 21);
            this.tabDeviceStatus.Name = "tabDeviceStatus";
            this.tabDeviceStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeviceStatus.Size = new System.Drawing.Size(969, 638);
            this.tabDeviceStatus.TabIndex = 2;
            this.tabDeviceStatus.Text = "이벤트";
            this.tabDeviceStatus.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lstDevice);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(3, 33);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(963, 602);
            this.panel8.TabIndex = 3;
            // 
            // lstDevice
            // 
            this.lstDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDevice.FullRowSelect = true;
            this.lstDevice.GridLines = true;
            this.lstDevice.Location = new System.Drawing.Point(3, 3);
            this.lstDevice.Name = "lstDevice";
            this.lstDevice.Size = new System.Drawing.Size(953, 591);
            this.lstDevice.TabIndex = 0;
            this.lstDevice.UseCompatibleStateImageBehavior = false;
            this.lstDevice.View = System.Windows.Forms.View.Details;
            this.lstDevice.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.Device_ListView_ColumnClick);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dtpFromDevice);
            this.panel7.Controls.Add(this.cbxDeviceNameStatus);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.btnSearchDevice);
            this.panel7.Controls.Add(this.btnPrintDevice);
            this.panel7.Controls.Add(this.label8);
            this.panel7.Controls.Add(this.dtpToDevice);
            this.panel7.Controls.Add(this.label9);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(963, 30);
            this.panel7.TabIndex = 2;
            // 
            // dtpFromDevice
            // 
            this.dtpFromDevice.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpFromDevice.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDevice.Location = new System.Drawing.Point(66, 2);
            this.dtpFromDevice.Name = "dtpFromDevice";
            this.dtpFromDevice.Size = new System.Drawing.Size(152, 21);
            this.dtpFromDevice.TabIndex = 93;
            this.dtpFromDevice.Value = new System.DateTime(2010, 9, 8, 0, 0, 0, 0);
            // 
            // cbxDeviceNameStatus
            // 
            this.cbxDeviceNameStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDeviceNameStatus.FormattingEnabled = true;
            this.cbxDeviceNameStatus.Location = new System.Drawing.Point(468, 3);
            this.cbxDeviceNameStatus.Name = "cbxDeviceNameStatus";
            this.cbxDeviceNameStatus.Size = new System.Drawing.Size(133, 20);
            this.cbxDeviceNameStatus.TabIndex = 92;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(418, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 12);
            this.label4.TabIndex = 91;
            this.label4.Text = "측기명";
            // 
            // btnSearchDevice
            // 
            this.btnSearchDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchDevice.Location = new System.Drawing.Point(800, 1);
            this.btnSearchDevice.Name = "btnSearchDevice";
            this.btnSearchDevice.Size = new System.Drawing.Size(75, 23);
            this.btnSearchDevice.TabIndex = 90;
            this.btnSearchDevice.Text = "조회";
            this.btnSearchDevice.UseVisualStyleBackColor = true;
            this.btnSearchDevice.Click += new System.EventHandler(this.btnSearchDevice_Click);
            // 
            // btnPrintDevice
            // 
            this.btnPrintDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintDevice.Location = new System.Drawing.Point(881, 1);
            this.btnPrintDevice.Name = "btnPrintDevice";
            this.btnPrintDevice.Size = new System.Drawing.Size(75, 23);
            this.btnPrintDevice.TabIndex = 89;
            this.btnPrintDevice.Text = "출력";
            this.btnPrintDevice.UseVisualStyleBackColor = true;
            this.btnPrintDevice.Click += new System.EventHandler(this.btnPrintDevice_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(224, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 12);
            this.label8.TabIndex = 86;
            this.label8.Text = "~";
            // 
            // dtpToDevice
            // 
            this.dtpToDevice.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpToDevice.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDevice.Location = new System.Drawing.Point(245, 2);
            this.dtpToDevice.Name = "dtpToDevice";
            this.dtpToDevice.Size = new System.Drawing.Size(153, 21);
            this.dtpToDevice.TabIndex = 85;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(5, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 12);
            this.label9.TabIndex = 83;
            this.label9.Text = "조회기간";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel10);
            this.tabPage1.Controls.Add(this.panel9);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(969, 638);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "알람정보";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.lstDeviceAlarm);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(3, 33);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(963, 602);
            this.panel10.TabIndex = 4;
            // 
            // lstDeviceAlarm
            // 
            this.lstDeviceAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDeviceAlarm.FullRowSelect = true;
            this.lstDeviceAlarm.GridLines = true;
            this.lstDeviceAlarm.Location = new System.Drawing.Point(3, 3);
            this.lstDeviceAlarm.Name = "lstDeviceAlarm";
            this.lstDeviceAlarm.Size = new System.Drawing.Size(953, 591);
            this.lstDeviceAlarm.TabIndex = 0;
            this.lstDeviceAlarm.UseCompatibleStateImageBehavior = false;
            this.lstDeviceAlarm.View = System.Windows.Forms.View.Details;
            this.lstDeviceAlarm.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstDeviceAlarm_ColumnClick);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.dtpFromDeviceAlarm);
            this.panel9.Controls.Add(this.cbxDeviceNameDeviceAlarm);
            this.panel9.Controls.Add(this.label11);
            this.panel9.Controls.Add(this.bntSearchDeviceAlarm);
            this.panel9.Controls.Add(this.btnPrintDeviceAlarm);
            this.panel9.Controls.Add(this.label12);
            this.panel9.Controls.Add(this.dtpToDeviceAlarm);
            this.panel9.Controls.Add(this.label13);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(963, 30);
            this.panel9.TabIndex = 3;
            // 
            // dtpFromDeviceAlarm
            // 
            this.dtpFromDeviceAlarm.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpFromDeviceAlarm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDeviceAlarm.Location = new System.Drawing.Point(66, 2);
            this.dtpFromDeviceAlarm.Name = "dtpFromDeviceAlarm";
            this.dtpFromDeviceAlarm.Size = new System.Drawing.Size(152, 21);
            this.dtpFromDeviceAlarm.TabIndex = 93;
            this.dtpFromDeviceAlarm.Value = new System.DateTime(2010, 9, 8, 0, 0, 0, 0);
            // 
            // cbxDeviceNameDeviceAlarm
            // 
            this.cbxDeviceNameDeviceAlarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDeviceNameDeviceAlarm.FormattingEnabled = true;
            this.cbxDeviceNameDeviceAlarm.Location = new System.Drawing.Point(468, 3);
            this.cbxDeviceNameDeviceAlarm.Name = "cbxDeviceNameDeviceAlarm";
            this.cbxDeviceNameDeviceAlarm.Size = new System.Drawing.Size(133, 20);
            this.cbxDeviceNameDeviceAlarm.TabIndex = 92;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(418, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 12);
            this.label11.TabIndex = 91;
            this.label11.Text = "측기명";
            // 
            // bntSearchDeviceAlarm
            // 
            this.bntSearchDeviceAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bntSearchDeviceAlarm.Location = new System.Drawing.Point(800, 1);
            this.bntSearchDeviceAlarm.Name = "bntSearchDeviceAlarm";
            this.bntSearchDeviceAlarm.Size = new System.Drawing.Size(75, 23);
            this.bntSearchDeviceAlarm.TabIndex = 90;
            this.bntSearchDeviceAlarm.Text = "조회";
            this.bntSearchDeviceAlarm.UseVisualStyleBackColor = true;
            this.bntSearchDeviceAlarm.Click += new System.EventHandler(this.btnSearchDeviceAlarm_Click);
            // 
            // btnPrintDeviceAlarm
            // 
            this.btnPrintDeviceAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintDeviceAlarm.Location = new System.Drawing.Point(881, 1);
            this.btnPrintDeviceAlarm.Name = "btnPrintDeviceAlarm";
            this.btnPrintDeviceAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnPrintDeviceAlarm.TabIndex = 89;
            this.btnPrintDeviceAlarm.Text = "출력";
            this.btnPrintDeviceAlarm.UseVisualStyleBackColor = true;
            this.btnPrintDeviceAlarm.Click += new System.EventHandler(this.btnPrintDeviceAlarm_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(224, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 12);
            this.label12.TabIndex = 86;
            this.label12.Text = "~";
            // 
            // dtpToDeviceAlarm
            // 
            this.dtpToDeviceAlarm.CustomFormat = "yyyy-MM-dd  HH시 mm분";
            this.dtpToDeviceAlarm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDeviceAlarm.Location = new System.Drawing.Point(245, 2);
            this.dtpToDeviceAlarm.Name = "dtpToDeviceAlarm";
            this.dtpToDeviceAlarm.Size = new System.Drawing.Size(153, 21);
            this.dtpToDeviceAlarm.TabIndex = 85;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(5, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 12);
            this.label13.TabIndex = 83;
            this.label13.Text = "조회기간";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(977, 25);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "이력 조회";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RecordsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 688);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RecordsForm";
            this.Text = "측기 이력 조회";
            this.Load += new System.EventHandler(this.RecordsForm_Load);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabWeatherInfo.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabAlarmInfo.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tabDeviceStatus.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabWeatherInfo;
        private System.Windows.Forms.TabPage tabAlarmInfo;
        private System.Windows.Forms.TabPage tabDeviceStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbxTypeWeather;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearchWeather;
        private System.Windows.Forms.Button btnPrintWeather;
        private System.Windows.Forms.ComboBox cbxDeviceNameWeather;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_fromTo;
        private System.Windows.Forms.DateTimePicker dtpToWeather;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Panel panel4;
       // private Adeng.Framework.Ctrl.ListViewEx lstWeather;
        private System.Windows.Forms.ListView lstWeather;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ListView lstAlarm;
        //private Adeng.Framework.Ctrl.ListViewEx lstAlarm;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnSearchAlarm;
        private System.Windows.Forms.Button btnPrintAlarm;
        private System.Windows.Forms.ComboBox cbxDeviceNameAlarm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpToAlarm;
        private System.Windows.Forms.DateTimePicker dtpFromAlarm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ListView lstDevice;
        //private Adeng.Framework.Ctrl.ListViewEx lstDevice;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnSearchDevice;
        private System.Windows.Forms.Button btnPrintDevice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpToDevice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxDeviceNameStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxTypeWeatherAlarm;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpFromWeather;
        private System.Windows.Forms.DateTimePicker dtpFromDevice;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.DateTimePicker dtpFromDeviceAlarm;
        private System.Windows.Forms.ComboBox cbxDeviceNameDeviceAlarm;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bntSearchDeviceAlarm;
        private System.Windows.Forms.Button btnPrintDeviceAlarm;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtpToDeviceAlarm;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel10;
        //private Adeng.Framework.Ctrl.ListViewEx lstDeviceAlarm;
        private System.Windows.Forms.ListView lstDeviceAlarm;
    }
}

