namespace ADEng.Module.WeatherSystem
{
    partial class WeatherControlMng
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeatherControlMng));
            this.ControlSidePN = new System.Windows.Forms.Panel();
            this.WDeviceTVPN = new System.Windows.Forms.Panel();
            this.WDeviceTV = new System.Windows.Forms.TreeView();
            this.WDeviceTVSubPN = new System.Windows.Forms.Panel();
            this.WDeviceTVLB = new System.Windows.Forms.Label();
            this.RequestBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.ControlMainPN = new System.Windows.Forms.Panel();
            this.ControlMainPB = new System.Windows.Forms.PictureBox();
            this.ControlMainLB = new System.Windows.Forms.Label();
            this.RequestItemSubPN = new System.Windows.Forms.Panel();
            this.FTime2TB = new System.Windows.Forms.NumericUpDown();
            this.FTime1TB = new System.Windows.Forms.NumericUpDown();
            this.FTime2LB = new System.Windows.Forms.Label();
            this.FTime1LB = new System.Windows.Forms.Label();
            this.Upgrade2TB = new System.Windows.Forms.TextBox();
            this.Upgrade1TB = new System.Windows.Forms.TextBox();
            this.IpPort2TB = new System.Windows.Forms.TextBox();
            this.IpPort1TB = new System.Windows.Forms.TextBox();
            this.Alarm3TB = new System.Windows.Forms.TextBox();
            this.Alarm2TB = new System.Windows.Forms.TextBox();
            this.Alarm1TB = new System.Windows.Forms.TextBox();
            this.AlarmCB = new System.Windows.Forms.ComboBox();
            this.ResetRB = new System.Windows.Forms.RadioButton();
            this.UpgradeRB = new System.Windows.Forms.RadioButton();
            this.IpPortRB = new System.Windows.Forms.RadioButton();
            this.FTimeRB = new System.Windows.Forms.RadioButton();
            this.AlarmRB = new System.Windows.Forms.RadioButton();
            this.RequestItemPN = new System.Windows.Forms.Panel();
            this.RequestItemLB = new System.Windows.Forms.Label();
            this.ControlSide2PN = new System.Windows.Forms.Panel();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.ControlTipLB = new System.Windows.Forms.Label();
            this.EIpPort2TB = new System.Windows.Forms.TextBox();
            this.EIpPort1TB = new System.Windows.Forms.TextBox();
            this.EIpPortRB = new System.Windows.Forms.RadioButton();
            this.WDeviceTVPN.SuspendLayout();
            this.WDeviceTVSubPN.SuspendLayout();
            this.ControlMainPN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMainPB)).BeginInit();
            this.RequestItemSubPN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FTime2TB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FTime1TB)).BeginInit();
            this.RequestItemPN.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlSidePN
            // 
            this.ControlSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ControlSidePN.BackgroundImage")));
            this.ControlSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ControlSidePN.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlSidePN.Location = new System.Drawing.Point(0, 40);
            this.ControlSidePN.Name = "ControlSidePN";
            this.ControlSidePN.Size = new System.Drawing.Size(532, 5);
            this.ControlSidePN.TabIndex = 2;
            // 
            // WDeviceTVPN
            // 
            this.WDeviceTVPN.Controls.Add(this.WDeviceTV);
            this.WDeviceTVPN.Controls.Add(this.WDeviceTVSubPN);
            this.WDeviceTVPN.Location = new System.Drawing.Point(5, 46);
            this.WDeviceTVPN.Name = "WDeviceTVPN";
            this.WDeviceTVPN.Size = new System.Drawing.Size(210, 339);
            this.WDeviceTVPN.TabIndex = 4;
            // 
            // WDeviceTV
            // 
            this.WDeviceTV.CheckBoxes = true;
            this.WDeviceTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WDeviceTV.FullRowSelect = true;
            this.WDeviceTV.HideSelection = false;
            this.WDeviceTV.Indent = 19;
            this.WDeviceTV.ItemHeight = 19;
            this.WDeviceTV.Location = new System.Drawing.Point(0, 25);
            this.WDeviceTV.Name = "WDeviceTV";
            this.WDeviceTV.PathSeparator = "/";
            this.WDeviceTV.ShowNodeToolTips = true;
            this.WDeviceTV.Size = new System.Drawing.Size(210, 314);
            this.WDeviceTV.TabIndex = 1;
            this.WDeviceTV.DoubleClick += new System.EventHandler(this.WDeviceTV_DoubleClick);
            // 
            // WDeviceTVSubPN
            // 
            this.WDeviceTVSubPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceTVSubPN.BackgroundImage")));
            this.WDeviceTVSubPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceTVSubPN.Controls.Add(this.WDeviceTVLB);
            this.WDeviceTVSubPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WDeviceTVSubPN.Location = new System.Drawing.Point(0, 0);
            this.WDeviceTVSubPN.Name = "WDeviceTVSubPN";
            this.WDeviceTVSubPN.Size = new System.Drawing.Size(210, 25);
            this.WDeviceTVSubPN.TabIndex = 0;
            // 
            // WDeviceTVLB
            // 
            this.WDeviceTVLB.BackColor = System.Drawing.Color.Transparent;
            this.WDeviceTVLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WDeviceTVLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.WDeviceTVLB.Location = new System.Drawing.Point(0, 0);
            this.WDeviceTVLB.Name = "WDeviceTVLB";
            this.WDeviceTVLB.Size = new System.Drawing.Size(86, 25);
            this.WDeviceTVLB.TabIndex = 0;
            this.WDeviceTVLB.Text = "  측기 선택";
            this.WDeviceTVLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RequestBtn
            // 
            this.RequestBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestBtn.Location = new System.Drawing.Point(367, 394);
            this.RequestBtn.Name = "RequestBtn";
            this.RequestBtn.Size = new System.Drawing.Size(75, 23);
            this.RequestBtn.TabIndex = 5;
            this.RequestBtn.Text = "제어";
            this.RequestBtn.UseVisualStyleBackColor = true;
            this.RequestBtn.Click += new System.EventHandler(this.RequestBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.Location = new System.Drawing.Point(448, 394);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 6;
            this.CloseBtn.Text = "닫기";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // ControlMainPN
            // 
            this.ControlMainPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ControlMainPN.BackgroundImage")));
            this.ControlMainPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ControlMainPN.Controls.Add(this.ControlMainPB);
            this.ControlMainPN.Controls.Add(this.ControlMainLB);
            this.ControlMainPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlMainPN.Location = new System.Drawing.Point(0, 0);
            this.ControlMainPN.Name = "ControlMainPN";
            this.ControlMainPN.Size = new System.Drawing.Size(532, 40);
            this.ControlMainPN.TabIndex = 1;
            // 
            // ControlMainPB
            // 
            this.ControlMainPB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlMainPB.BackColor = System.Drawing.Color.Transparent;
            this.ControlMainPB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ControlMainPB.BackgroundImage")));
            this.ControlMainPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ControlMainPB.Location = new System.Drawing.Point(475, 3);
            this.ControlMainPB.Name = "ControlMainPB";
            this.ControlMainPB.Size = new System.Drawing.Size(39, 35);
            this.ControlMainPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ControlMainPB.TabIndex = 1;
            this.ControlMainPB.TabStop = false;
            // 
            // ControlMainLB
            // 
            this.ControlMainLB.AutoSize = true;
            this.ControlMainLB.BackColor = System.Drawing.Color.Transparent;
            this.ControlMainLB.Location = new System.Drawing.Point(12, 9);
            this.ControlMainLB.Name = "ControlMainLB";
            this.ControlMainLB.Size = new System.Drawing.Size(265, 12);
            this.ControlMainLB.TabIndex = 0;
            this.ControlMainLB.Text = "측기와 제어항목을 선택하여 측기를 제어합니다.";
            // 
            // RequestItemSubPN
            // 
            this.RequestItemSubPN.BackColor = System.Drawing.Color.White;
            this.RequestItemSubPN.Controls.Add(this.EIpPort2TB);
            this.RequestItemSubPN.Controls.Add(this.EIpPort1TB);
            this.RequestItemSubPN.Controls.Add(this.EIpPortRB);
            this.RequestItemSubPN.Controls.Add(this.FTime2TB);
            this.RequestItemSubPN.Controls.Add(this.FTime1TB);
            this.RequestItemSubPN.Controls.Add(this.FTime2LB);
            this.RequestItemSubPN.Controls.Add(this.FTime1LB);
            this.RequestItemSubPN.Controls.Add(this.Upgrade2TB);
            this.RequestItemSubPN.Controls.Add(this.Upgrade1TB);
            this.RequestItemSubPN.Controls.Add(this.IpPort2TB);
            this.RequestItemSubPN.Controls.Add(this.IpPort1TB);
            this.RequestItemSubPN.Controls.Add(this.Alarm3TB);
            this.RequestItemSubPN.Controls.Add(this.Alarm2TB);
            this.RequestItemSubPN.Controls.Add(this.Alarm1TB);
            this.RequestItemSubPN.Controls.Add(this.AlarmCB);
            this.RequestItemSubPN.Controls.Add(this.ResetRB);
            this.RequestItemSubPN.Controls.Add(this.UpgradeRB);
            this.RequestItemSubPN.Controls.Add(this.IpPortRB);
            this.RequestItemSubPN.Controls.Add(this.FTimeRB);
            this.RequestItemSubPN.Controls.Add(this.AlarmRB);
            this.RequestItemSubPN.Controls.Add(this.RequestItemPN);
            this.RequestItemSubPN.Location = new System.Drawing.Point(221, 46);
            this.RequestItemSubPN.Name = "RequestItemSubPN";
            this.RequestItemSubPN.Size = new System.Drawing.Size(305, 339);
            this.RequestItemSubPN.TabIndex = 7;
            // 
            // FTime2TB
            // 
            this.FTime2TB.Enabled = false;
            this.FTime2TB.Location = new System.Drawing.Point(207, 121);
            this.FTime2TB.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.FTime2TB.Name = "FTime2TB";
            this.FTime2TB.Size = new System.Drawing.Size(60, 21);
            this.FTime2TB.TabIndex = 20;
            this.FTime2TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FTime2TB.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // FTime1TB
            // 
            this.FTime1TB.Enabled = false;
            this.FTime1TB.Location = new System.Drawing.Point(113, 121);
            this.FTime1TB.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.FTime1TB.Name = "FTime1TB";
            this.FTime1TB.Size = new System.Drawing.Size(60, 21);
            this.FTime1TB.TabIndex = 19;
            this.FTime1TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FTime1TB.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // FTime2LB
            // 
            this.FTime2LB.AutoSize = true;
            this.FTime2LB.Location = new System.Drawing.Point(268, 130);
            this.FTime2LB.Name = "FTime2LB";
            this.FTime2LB.Size = new System.Drawing.Size(27, 12);
            this.FTime2LB.TabIndex = 18;
            this.FTime2LB.Text = "(분)";
            this.FTime2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FTime1LB
            // 
            this.FTime1LB.AutoSize = true;
            this.FTime1LB.Location = new System.Drawing.Point(174, 130);
            this.FTime1LB.Name = "FTime1LB";
            this.FTime1LB.Size = new System.Drawing.Size(27, 12);
            this.FTime1LB.TabIndex = 17;
            this.FTime1LB.Text = "(분)";
            this.FTime1LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Upgrade2TB
            // 
            this.Upgrade2TB.Enabled = false;
            this.Upgrade2TB.Location = new System.Drawing.Point(240, 285);
            this.Upgrade2TB.MaxLength = 4;
            this.Upgrade2TB.Name = "Upgrade2TB";
            this.Upgrade2TB.Size = new System.Drawing.Size(52, 21);
            this.Upgrade2TB.TabIndex = 16;
            this.Upgrade2TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Upgrade2TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpPort2TB_KeyDown);
            // 
            // Upgrade1TB
            // 
            this.Upgrade1TB.Enabled = false;
            this.Upgrade1TB.Location = new System.Drawing.Point(124, 285);
            this.Upgrade1TB.MaxLength = 15;
            this.Upgrade1TB.Name = "Upgrade1TB";
            this.Upgrade1TB.Size = new System.Drawing.Size(110, 21);
            this.Upgrade1TB.TabIndex = 15;
            this.Upgrade1TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Upgrade1TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarm1TB_KeyDown);
            // 
            // IpPort2TB
            // 
            this.IpPort2TB.Enabled = false;
            this.IpPort2TB.Location = new System.Drawing.Point(240, 175);
            this.IpPort2TB.MaxLength = 4;
            this.IpPort2TB.Name = "IpPort2TB";
            this.IpPort2TB.Size = new System.Drawing.Size(52, 21);
            this.IpPort2TB.TabIndex = 14;
            this.IpPort2TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IpPort2TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpPort2TB_KeyDown);
            // 
            // IpPort1TB
            // 
            this.IpPort1TB.Enabled = false;
            this.IpPort1TB.Location = new System.Drawing.Point(124, 175);
            this.IpPort1TB.MaxLength = 15;
            this.IpPort1TB.Name = "IpPort1TB";
            this.IpPort1TB.Size = new System.Drawing.Size(110, 21);
            this.IpPort1TB.TabIndex = 13;
            this.IpPort1TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IpPort1TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarm1TB_KeyDown);
            // 
            // Alarm3TB
            // 
            this.Alarm3TB.Enabled = false;
            this.Alarm3TB.Location = new System.Drawing.Point(240, 65);
            this.Alarm3TB.MaxLength = 5;
            this.Alarm3TB.Name = "Alarm3TB";
            this.Alarm3TB.Size = new System.Drawing.Size(52, 21);
            this.Alarm3TB.TabIndex = 10;
            this.Alarm3TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Alarm3TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarm1TB_KeyDown);
            // 
            // Alarm2TB
            // 
            this.Alarm2TB.Enabled = false;
            this.Alarm2TB.Location = new System.Drawing.Point(182, 65);
            this.Alarm2TB.MaxLength = 5;
            this.Alarm2TB.Name = "Alarm2TB";
            this.Alarm2TB.Size = new System.Drawing.Size(52, 21);
            this.Alarm2TB.TabIndex = 9;
            this.Alarm2TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Alarm2TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarm1TB_KeyDown);
            // 
            // Alarm1TB
            // 
            this.Alarm1TB.Enabled = false;
            this.Alarm1TB.Location = new System.Drawing.Point(124, 65);
            this.Alarm1TB.MaxLength = 5;
            this.Alarm1TB.Name = "Alarm1TB";
            this.Alarm1TB.Size = new System.Drawing.Size(52, 21);
            this.Alarm1TB.TabIndex = 8;
            this.Alarm1TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Alarm1TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarm1TB_KeyDown);
            // 
            // AlarmCB
            // 
            this.AlarmCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AlarmCB.Enabled = false;
            this.AlarmCB.FormattingEnabled = true;
            this.AlarmCB.Location = new System.Drawing.Point(34, 65);
            this.AlarmCB.Name = "AlarmCB";
            this.AlarmCB.Size = new System.Drawing.Size(65, 20);
            this.AlarmCB.TabIndex = 7;
            this.AlarmCB.SelectedIndexChanged += new System.EventHandler(this.AlarmCB_SelectedIndexChanged);
            // 
            // ResetRB
            // 
            this.ResetRB.AutoSize = true;
            this.ResetRB.Location = new System.Drawing.Point(17, 314);
            this.ResetRB.Name = "ResetRB";
            this.ResetRB.Size = new System.Drawing.Size(47, 16);
            this.ResetRB.TabIndex = 6;
            this.ResetRB.Text = "리셋";
            this.ResetRB.UseVisualStyleBackColor = true;
            // 
            // UpgradeRB
            // 
            this.UpgradeRB.AutoSize = true;
            this.UpgradeRB.Location = new System.Drawing.Point(17, 263);
            this.UpgradeRB.Name = "UpgradeRB";
            this.UpgradeRB.Size = new System.Drawing.Size(175, 16);
            this.UpgradeRB.TabIndex = 5;
            this.UpgradeRB.Text = "원격 업그레이드 (IP/PORT)";
            this.UpgradeRB.UseVisualStyleBackColor = true;
            this.UpgradeRB.CheckedChanged += new System.EventHandler(this.UpgradeRB_CheckedChanged);
            // 
            // IpPortRB
            // 
            this.IpPortRB.AutoSize = true;
            this.IpPortRB.Location = new System.Drawing.Point(17, 155);
            this.IpPortRB.Name = "IpPortRB";
            this.IpPortRB.Size = new System.Drawing.Size(141, 16);
            this.IpPortRB.TabIndex = 4;
            this.IpPortRB.Text = "CDMA IP/PORT 설정";
            this.IpPortRB.UseVisualStyleBackColor = true;
            this.IpPortRB.CheckedChanged += new System.EventHandler(this.IpPortRB_CheckedChanged);
            // 
            // FTimeRB
            // 
            this.FTimeRB.AutoSize = true;
            this.FTimeRB.Location = new System.Drawing.Point(17, 99);
            this.FTimeRB.Name = "FTimeRB";
            this.FTimeRB.Size = new System.Drawing.Size(223, 16);
            this.FTimeRB.TabIndex = 3;
            this.FTimeRB.Text = "무시시간 설정 (동일 레벨/하향 레벨)";
            this.FTimeRB.UseVisualStyleBackColor = true;
            this.FTimeRB.CheckedChanged += new System.EventHandler(this.FTimeRB_CheckedChanged);
            // 
            // AlarmRB
            // 
            this.AlarmRB.AutoSize = true;
            this.AlarmRB.Location = new System.Drawing.Point(17, 43);
            this.AlarmRB.Name = "AlarmRB";
            this.AlarmRB.Size = new System.Drawing.Size(159, 16);
            this.AlarmRB.TabIndex = 2;
            this.AlarmRB.Text = "임계치 설정 (1/2/3 단계)";
            this.AlarmRB.UseVisualStyleBackColor = true;
            this.AlarmRB.CheckedChanged += new System.EventHandler(this.AlarmRB_CheckedChanged);
            // 
            // RequestItemPN
            // 
            this.RequestItemPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RequestItemPN.BackgroundImage")));
            this.RequestItemPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RequestItemPN.Controls.Add(this.RequestItemLB);
            this.RequestItemPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.RequestItemPN.Location = new System.Drawing.Point(0, 0);
            this.RequestItemPN.Name = "RequestItemPN";
            this.RequestItemPN.Size = new System.Drawing.Size(305, 25);
            this.RequestItemPN.TabIndex = 1;
            // 
            // RequestItemLB
            // 
            this.RequestItemLB.BackColor = System.Drawing.Color.Transparent;
            this.RequestItemLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.RequestItemLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RequestItemLB.Location = new System.Drawing.Point(0, 0);
            this.RequestItemLB.Name = "RequestItemLB";
            this.RequestItemLB.Size = new System.Drawing.Size(99, 25);
            this.RequestItemLB.TabIndex = 0;
            this.RequestItemLB.Text = "  제어항목 선택";
            this.RequestItemLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ControlSide2PN
            // 
            this.ControlSide2PN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ControlSide2PN.BackgroundImage")));
            this.ControlSide2PN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ControlSide2PN.Location = new System.Drawing.Point(0, 386);
            this.ControlSide2PN.Name = "ControlSide2PN";
            this.ControlSide2PN.Size = new System.Drawing.Size(532, 5);
            this.ControlSide2PN.TabIndex = 8;
            // 
            // MainImageList
            // 
            this.MainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.MainImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ControlTipLB
            // 
            this.ControlTipLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ControlTipLB.Location = new System.Drawing.Point(11, 396);
            this.ControlTipLB.Name = "ControlTipLB";
            this.ControlTipLB.Size = new System.Drawing.Size(331, 20);
            this.ControlTipLB.TabIndex = 9;
            this.ControlTipLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EIpPort2TB
            // 
            this.EIpPort2TB.Enabled = false;
            this.EIpPort2TB.Location = new System.Drawing.Point(240, 230);
            this.EIpPort2TB.MaxLength = 4;
            this.EIpPort2TB.Name = "EIpPort2TB";
            this.EIpPort2TB.Size = new System.Drawing.Size(52, 21);
            this.EIpPort2TB.TabIndex = 23;
            this.EIpPort2TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.EIpPort2TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpPort2TB_KeyDown);
            // 
            // EIpPort1TB
            // 
            this.EIpPort1TB.Enabled = false;
            this.EIpPort1TB.Location = new System.Drawing.Point(124, 230);
            this.EIpPort1TB.MaxLength = 15;
            this.EIpPort1TB.Name = "EIpPort1TB";
            this.EIpPort1TB.Size = new System.Drawing.Size(110, 21);
            this.EIpPort1TB.TabIndex = 22;
            this.EIpPort1TB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.EIpPort1TB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Alarm1TB_KeyDown);
            // 
            // EIpPortRB
            // 
            this.EIpPortRB.AutoSize = true;
            this.EIpPortRB.Location = new System.Drawing.Point(17, 210);
            this.EIpPortRB.Name = "EIpPortRB";
            this.EIpPortRB.Size = new System.Drawing.Size(141, 16);
            this.EIpPortRB.TabIndex = 21;
            this.EIpPortRB.Text = "이더넷 IP/PORT 설정";
            this.EIpPortRB.UseVisualStyleBackColor = true;
            this.EIpPortRB.CheckedChanged += new System.EventHandler(this.EIpPortRB_CheckedChanged);
            // 
            // WeatherControlMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 421);
            this.Controls.Add(this.ControlTipLB);
            this.Controls.Add(this.ControlSide2PN);
            this.Controls.Add(this.RequestItemSubPN);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.RequestBtn);
            this.Controls.Add(this.WDeviceTVPN);
            this.Controls.Add(this.ControlSidePN);
            this.Controls.Add(this.ControlMainPN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WeatherControlMng";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "측기 제어";
            this.TopMost = true;
            this.WDeviceTVPN.ResumeLayout(false);
            this.WDeviceTVSubPN.ResumeLayout(false);
            this.ControlMainPN.ResumeLayout(false);
            this.ControlMainPN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMainPB)).EndInit();
            this.RequestItemSubPN.ResumeLayout(false);
            this.RequestItemSubPN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FTime2TB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FTime1TB)).EndInit();
            this.RequestItemPN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ControlMainLB;
        private System.Windows.Forms.Panel ControlMainPN;
        private System.Windows.Forms.PictureBox ControlMainPB;
        private System.Windows.Forms.Panel ControlSidePN;
        private System.Windows.Forms.Panel WDeviceTVPN;
        private System.Windows.Forms.Button RequestBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Panel WDeviceTVSubPN;
        private System.Windows.Forms.Label WDeviceTVLB;
        private System.Windows.Forms.Panel RequestItemSubPN;
        private System.Windows.Forms.Panel RequestItemPN;
        private System.Windows.Forms.Label RequestItemLB;
        private System.Windows.Forms.Panel ControlSide2PN;
        private System.Windows.Forms.RadioButton ResetRB;
        private System.Windows.Forms.RadioButton UpgradeRB;
        private System.Windows.Forms.RadioButton IpPortRB;
        private System.Windows.Forms.RadioButton FTimeRB;
        private System.Windows.Forms.RadioButton AlarmRB;
        private System.Windows.Forms.ComboBox AlarmCB;
        private System.Windows.Forms.TextBox Alarm3TB;
        private System.Windows.Forms.TextBox Alarm2TB;
        private System.Windows.Forms.TextBox Alarm1TB;
        private System.Windows.Forms.TextBox IpPort2TB;
        private System.Windows.Forms.TextBox IpPort1TB;
        private System.Windows.Forms.TextBox Upgrade2TB;
        private System.Windows.Forms.TextBox Upgrade1TB;
        public System.Windows.Forms.TreeView WDeviceTV;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.Label FTime1LB;
        private System.Windows.Forms.NumericUpDown FTime2TB;
        private System.Windows.Forms.NumericUpDown FTime1TB;
        private System.Windows.Forms.Label FTime2LB;
        private System.Windows.Forms.Label ControlTipLB;
        private System.Windows.Forms.TextBox EIpPort2TB;
        private System.Windows.Forms.TextBox EIpPort1TB;
        private System.Windows.Forms.RadioButton EIpPortRB;
    }
}

