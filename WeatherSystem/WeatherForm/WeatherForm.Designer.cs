namespace ADEng.Module.WeatherSystem
{
    partial class WeatherForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeatherForm));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.WeatherListView = new System.Windows.Forms.ListView();
            this.MainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.상태요청ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.제어ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.WeatherListPN = new System.Windows.Forms.Panel();
            this.WeatherListLB = new System.Windows.Forms.Label();
            this.WeatherDetailMainPN = new System.Windows.Forms.Panel();
            this.BattState2LB = new System.Windows.Forms.Label();
            this.BattLife2LB = new System.Windows.Forms.Label();
            this.BattTempo2LB = new System.Windows.Forms.Label();
            this.BattC2LB = new System.Windows.Forms.Label();
            this.BattA2LB = new System.Windows.Forms.Label();
            this.BattVolt2LB = new System.Windows.Forms.Label();
            this.RemarkLB = new System.Windows.Forms.Label();
            this.FWVerStateLB = new System.Windows.Forms.Label();
            this.DoorStateLB = new System.Windows.Forms.Label();
            this.FanStateLB = new System.Windows.Forms.Label();
            this.BattStateLB = new System.Windows.Forms.Label();
            this.BattLifeLB = new System.Windows.Forms.Label();
            this.BattTempoLB = new System.Windows.Forms.Label();
            this.BattCLB = new System.Windows.Forms.Label();
            this.BattALB = new System.Windows.Forms.Label();
            this.BattVoltLB = new System.Windows.Forms.Label();
            this.CdmaRssiLB = new System.Windows.Forms.Label();
            this.CdmaPortLB = new System.Windows.Forms.Label();
            this.CdmaIpLB = new System.Windows.Forms.Label();
            this.DownFTimeLB = new System.Windows.Forms.Label();
            this.SameFTimeLB = new System.Windows.Forms.Label();
            this.WeatherDetailPN = new System.Windows.Forms.Panel();
            this.WeatherDetailLB = new System.Windows.Forms.Label();
            this.WeatherDataTextBox = new System.Windows.Forms.RichTextBox();
            this.WeatherDataPN = new System.Windows.Forms.Panel();
            this.WeatherDataResetLB = new System.Windows.Forms.Label();
            this.WeatherDataLB = new System.Windows.Forms.Label();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RemarkToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.MainContextMenu.SuspendLayout();
            this.WeatherListPN.SuspendLayout();
            this.WeatherDetailMainPN.SuspendLayout();
            this.WeatherDetailPN.SuspendLayout();
            this.WeatherDataPN.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer1.IsSplitterFixed = true;
            this.SplitContainer1.Location = new System.Drawing.Point(2, 2);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.SplitContainer2);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.WeatherDataTextBox);
            this.SplitContainer1.Panel2.Controls.Add(this.WeatherDataPN);
            this.SplitContainer1.Size = new System.Drawing.Size(973, 684);
            this.SplitContainer1.SplitterDistance = 509;
            this.SplitContainer1.TabIndex = 0;
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer2.IsSplitterFixed = true;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Name = "SplitContainer2";
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.WeatherListView);
            this.SplitContainer2.Panel1.Controls.Add(this.WeatherListPN);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.WeatherDetailMainPN);
            this.SplitContainer2.Panel2.Controls.Add(this.WeatherDetailPN);
            this.SplitContainer2.Size = new System.Drawing.Size(973, 509);
            this.SplitContainer2.SplitterDistance = 664;
            this.SplitContainer2.TabIndex = 0;
            // 
            // WeatherListView
            // 
            this.WeatherListView.BackColor = System.Drawing.Color.White;
            this.WeatherListView.ContextMenuStrip = this.MainContextMenu;
            this.WeatherListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WeatherListView.FullRowSelect = true;
            this.WeatherListView.GridLines = true;
            this.WeatherListView.HideSelection = false;
            this.WeatherListView.Location = new System.Drawing.Point(0, 25);
            this.WeatherListView.Name = "WeatherListView";
            this.WeatherListView.Size = new System.Drawing.Size(664, 484);
            this.WeatherListView.StateImageList = this.MainImageList;
            this.WeatherListView.TabIndex = 1;
            this.WeatherListView.UseCompatibleStateImageBehavior = false;
            this.WeatherListView.View = System.Windows.Forms.View.Details;
            this.WeatherListView.SelectedIndexChanged += new System.EventHandler(this.WeatherListView_SelectedIndexChanged);
            // 
            // MainContextMenu
            // 
            this.MainContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.상태요청ToolStripMenuItem,
            this.제어ToolStripMenuItem});
            this.MainContextMenu.Name = "MainContextMenu";
            this.MainContextMenu.Size = new System.Drawing.Size(129, 48);
            // 
            // 상태요청ToolStripMenuItem
            // 
            this.상태요청ToolStripMenuItem.Name = "상태요청ToolStripMenuItem";
            this.상태요청ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.상태요청ToolStripMenuItem.Text = "상태 요청";
            this.상태요청ToolStripMenuItem.Click += new System.EventHandler(this.상태요청ToolStripMenuItem_Click);
            // 
            // 제어ToolStripMenuItem
            // 
            this.제어ToolStripMenuItem.Name = "제어ToolStripMenuItem";
            this.제어ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.제어ToolStripMenuItem.Text = "제어";
            this.제어ToolStripMenuItem.Click += new System.EventHandler(this.제어ToolStripMenuItem_Click);
            // 
            // MainImageList
            // 
            this.MainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MainImageList.ImageStream")));
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MainImageList.Images.SetKeyName(0, "DMB_systemRed.png");
            this.MainImageList.Images.SetKeyName(1, "DMB_systemGreen.png");
            // 
            // WeatherListPN
            // 
            this.WeatherListPN.BackgroundImage = global::WeatherForm.Properties.Resources.DMB_main;
            this.WeatherListPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WeatherListPN.Controls.Add(this.WeatherListLB);
            this.WeatherListPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WeatherListPN.Location = new System.Drawing.Point(0, 0);
            this.WeatherListPN.Name = "WeatherListPN";
            this.WeatherListPN.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.WeatherListPN.Size = new System.Drawing.Size(664, 25);
            this.WeatherListPN.TabIndex = 0;
            // 
            // WeatherListLB
            // 
            this.WeatherListLB.BackColor = System.Drawing.Color.Transparent;
            this.WeatherListLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WeatherListLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.WeatherListLB.Location = new System.Drawing.Point(10, 0);
            this.WeatherListLB.Name = "WeatherListLB";
            this.WeatherListLB.Size = new System.Drawing.Size(102, 25);
            this.WeatherListLB.TabIndex = 0;
            this.WeatherListLB.Text = "측기 리스트";
            this.WeatherListLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WeatherDetailMainPN
            // 
            this.WeatherDetailMainPN.BackColor = System.Drawing.Color.White;
            this.WeatherDetailMainPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WeatherDetailMainPN.BackgroundImage")));
            this.WeatherDetailMainPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WeatherDetailMainPN.Controls.Add(this.BattState2LB);
            this.WeatherDetailMainPN.Controls.Add(this.BattLife2LB);
            this.WeatherDetailMainPN.Controls.Add(this.BattTempo2LB);
            this.WeatherDetailMainPN.Controls.Add(this.BattC2LB);
            this.WeatherDetailMainPN.Controls.Add(this.BattA2LB);
            this.WeatherDetailMainPN.Controls.Add(this.BattVolt2LB);
            this.WeatherDetailMainPN.Controls.Add(this.RemarkLB);
            this.WeatherDetailMainPN.Controls.Add(this.FWVerStateLB);
            this.WeatherDetailMainPN.Controls.Add(this.DoorStateLB);
            this.WeatherDetailMainPN.Controls.Add(this.FanStateLB);
            this.WeatherDetailMainPN.Controls.Add(this.BattStateLB);
            this.WeatherDetailMainPN.Controls.Add(this.BattLifeLB);
            this.WeatherDetailMainPN.Controls.Add(this.BattTempoLB);
            this.WeatherDetailMainPN.Controls.Add(this.BattCLB);
            this.WeatherDetailMainPN.Controls.Add(this.BattALB);
            this.WeatherDetailMainPN.Controls.Add(this.BattVoltLB);
            this.WeatherDetailMainPN.Controls.Add(this.CdmaRssiLB);
            this.WeatherDetailMainPN.Controls.Add(this.CdmaPortLB);
            this.WeatherDetailMainPN.Controls.Add(this.CdmaIpLB);
            this.WeatherDetailMainPN.Controls.Add(this.DownFTimeLB);
            this.WeatherDetailMainPN.Controls.Add(this.SameFTimeLB);
            this.WeatherDetailMainPN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WeatherDetailMainPN.Location = new System.Drawing.Point(0, 25);
            this.WeatherDetailMainPN.Name = "WeatherDetailMainPN";
            this.WeatherDetailMainPN.Size = new System.Drawing.Size(305, 484);
            this.WeatherDetailMainPN.TabIndex = 1;
            // 
            // BattState2LB
            // 
            this.BattState2LB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattState2LB.BackColor = System.Drawing.Color.Transparent;
            this.BattState2LB.Location = new System.Drawing.Point(206, 236);
            this.BattState2LB.Name = "BattState2LB";
            this.BattState2LB.Size = new System.Drawing.Size(85, 19);
            this.BattState2LB.TabIndex = 20;
            this.BattState2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattLife2LB
            // 
            this.BattLife2LB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattLife2LB.BackColor = System.Drawing.Color.Transparent;
            this.BattLife2LB.Location = new System.Drawing.Point(206, 212);
            this.BattLife2LB.Name = "BattLife2LB";
            this.BattLife2LB.Size = new System.Drawing.Size(85, 19);
            this.BattLife2LB.TabIndex = 19;
            this.BattLife2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattTempo2LB
            // 
            this.BattTempo2LB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattTempo2LB.BackColor = System.Drawing.Color.Transparent;
            this.BattTempo2LB.Location = new System.Drawing.Point(206, 188);
            this.BattTempo2LB.Name = "BattTempo2LB";
            this.BattTempo2LB.Size = new System.Drawing.Size(85, 19);
            this.BattTempo2LB.TabIndex = 18;
            this.BattTempo2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattC2LB
            // 
            this.BattC2LB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattC2LB.BackColor = System.Drawing.Color.Transparent;
            this.BattC2LB.Location = new System.Drawing.Point(63, 236);
            this.BattC2LB.Name = "BattC2LB";
            this.BattC2LB.Size = new System.Drawing.Size(96, 19);
            this.BattC2LB.TabIndex = 17;
            this.BattC2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattA2LB
            // 
            this.BattA2LB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattA2LB.BackColor = System.Drawing.Color.Transparent;
            this.BattA2LB.Location = new System.Drawing.Point(63, 212);
            this.BattA2LB.Name = "BattA2LB";
            this.BattA2LB.Size = new System.Drawing.Size(96, 19);
            this.BattA2LB.TabIndex = 16;
            this.BattA2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattVolt2LB
            // 
            this.BattVolt2LB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattVolt2LB.BackColor = System.Drawing.Color.Transparent;
            this.BattVolt2LB.Location = new System.Drawing.Point(63, 188);
            this.BattVolt2LB.Name = "BattVolt2LB";
            this.BattVolt2LB.Size = new System.Drawing.Size(96, 19);
            this.BattVolt2LB.TabIndex = 15;
            this.BattVolt2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemarkLB
            // 
            this.RemarkLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RemarkLB.BackColor = System.Drawing.Color.Transparent;
            this.RemarkLB.Location = new System.Drawing.Point(107, 454);
            this.RemarkLB.Name = "RemarkLB";
            this.RemarkLB.Size = new System.Drawing.Size(176, 19);
            this.RemarkLB.TabIndex = 14;
            this.RemarkLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RemarkLB.MouseHover += new System.EventHandler(this.RemarkLB_MouseHover);
            // 
            // FWVerStateLB
            // 
            this.FWVerStateLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FWVerStateLB.BackColor = System.Drawing.Color.Transparent;
            this.FWVerStateLB.Location = new System.Drawing.Point(107, 433);
            this.FWVerStateLB.Name = "FWVerStateLB";
            this.FWVerStateLB.Size = new System.Drawing.Size(176, 19);
            this.FWVerStateLB.TabIndex = 13;
            this.FWVerStateLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DoorStateLB
            // 
            this.DoorStateLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DoorStateLB.BackColor = System.Drawing.Color.Transparent;
            this.DoorStateLB.Location = new System.Drawing.Point(107, 409);
            this.DoorStateLB.Name = "DoorStateLB";
            this.DoorStateLB.Size = new System.Drawing.Size(176, 19);
            this.DoorStateLB.TabIndex = 12;
            this.DoorStateLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FanStateLB
            // 
            this.FanStateLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FanStateLB.BackColor = System.Drawing.Color.Transparent;
            this.FanStateLB.Location = new System.Drawing.Point(107, 385);
            this.FanStateLB.Name = "FanStateLB";
            this.FanStateLB.Size = new System.Drawing.Size(176, 19);
            this.FanStateLB.TabIndex = 11;
            this.FanStateLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattStateLB
            // 
            this.BattStateLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattStateLB.BackColor = System.Drawing.Color.Transparent;
            this.BattStateLB.Location = new System.Drawing.Point(206, 136);
            this.BattStateLB.Name = "BattStateLB";
            this.BattStateLB.Size = new System.Drawing.Size(85, 19);
            this.BattStateLB.TabIndex = 10;
            this.BattStateLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattLifeLB
            // 
            this.BattLifeLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattLifeLB.BackColor = System.Drawing.Color.Transparent;
            this.BattLifeLB.Location = new System.Drawing.Point(206, 112);
            this.BattLifeLB.Name = "BattLifeLB";
            this.BattLifeLB.Size = new System.Drawing.Size(85, 19);
            this.BattLifeLB.TabIndex = 9;
            this.BattLifeLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattTempoLB
            // 
            this.BattTempoLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattTempoLB.BackColor = System.Drawing.Color.Transparent;
            this.BattTempoLB.Location = new System.Drawing.Point(206, 88);
            this.BattTempoLB.Name = "BattTempoLB";
            this.BattTempoLB.Size = new System.Drawing.Size(85, 19);
            this.BattTempoLB.TabIndex = 8;
            this.BattTempoLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattCLB
            // 
            this.BattCLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattCLB.BackColor = System.Drawing.Color.Transparent;
            this.BattCLB.Location = new System.Drawing.Point(63, 136);
            this.BattCLB.Name = "BattCLB";
            this.BattCLB.Size = new System.Drawing.Size(96, 19);
            this.BattCLB.TabIndex = 7;
            this.BattCLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattALB
            // 
            this.BattALB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattALB.BackColor = System.Drawing.Color.Transparent;
            this.BattALB.Location = new System.Drawing.Point(63, 112);
            this.BattALB.Name = "BattALB";
            this.BattALB.Size = new System.Drawing.Size(96, 19);
            this.BattALB.TabIndex = 6;
            this.BattALB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BattVoltLB
            // 
            this.BattVoltLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BattVoltLB.BackColor = System.Drawing.Color.Transparent;
            this.BattVoltLB.Location = new System.Drawing.Point(63, 88);
            this.BattVoltLB.Name = "BattVoltLB";
            this.BattVoltLB.Size = new System.Drawing.Size(96, 19);
            this.BattVoltLB.TabIndex = 5;
            this.BattVoltLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CdmaRssiLB
            // 
            this.CdmaRssiLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CdmaRssiLB.BackColor = System.Drawing.Color.Transparent;
            this.CdmaRssiLB.Location = new System.Drawing.Point(107, 334);
            this.CdmaRssiLB.Name = "CdmaRssiLB";
            this.CdmaRssiLB.Size = new System.Drawing.Size(139, 19);
            this.CdmaRssiLB.TabIndex = 4;
            this.CdmaRssiLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CdmaPortLB
            // 
            this.CdmaPortLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CdmaPortLB.BackColor = System.Drawing.Color.Transparent;
            this.CdmaPortLB.Location = new System.Drawing.Point(107, 311);
            this.CdmaPortLB.Name = "CdmaPortLB";
            this.CdmaPortLB.Size = new System.Drawing.Size(139, 19);
            this.CdmaPortLB.TabIndex = 3;
            this.CdmaPortLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CdmaIpLB
            // 
            this.CdmaIpLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CdmaIpLB.BackColor = System.Drawing.Color.Transparent;
            this.CdmaIpLB.Location = new System.Drawing.Point(107, 287);
            this.CdmaIpLB.Name = "CdmaIpLB";
            this.CdmaIpLB.Size = new System.Drawing.Size(139, 19);
            this.CdmaIpLB.TabIndex = 2;
            this.CdmaIpLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DownFTimeLB
            // 
            this.DownFTimeLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DownFTimeLB.BackColor = System.Drawing.Color.Transparent;
            this.DownFTimeLB.Location = new System.Drawing.Point(222, 35);
            this.DownFTimeLB.Name = "DownFTimeLB";
            this.DownFTimeLB.Size = new System.Drawing.Size(67, 19);
            this.DownFTimeLB.TabIndex = 1;
            this.DownFTimeLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SameFTimeLB
            // 
            this.SameFTimeLB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SameFTimeLB.BackColor = System.Drawing.Color.Transparent;
            this.SameFTimeLB.Location = new System.Drawing.Point(80, 35);
            this.SameFTimeLB.Name = "SameFTimeLB";
            this.SameFTimeLB.Size = new System.Drawing.Size(67, 19);
            this.SameFTimeLB.TabIndex = 0;
            this.SameFTimeLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WeatherDetailPN
            // 
            this.WeatherDetailPN.BackgroundImage = global::WeatherForm.Properties.Resources.DMB_main;
            this.WeatherDetailPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WeatherDetailPN.Controls.Add(this.WeatherDetailLB);
            this.WeatherDetailPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WeatherDetailPN.Location = new System.Drawing.Point(0, 0);
            this.WeatherDetailPN.Name = "WeatherDetailPN";
            this.WeatherDetailPN.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.WeatherDetailPN.Size = new System.Drawing.Size(305, 25);
            this.WeatherDetailPN.TabIndex = 0;
            // 
            // WeatherDetailLB
            // 
            this.WeatherDetailLB.BackColor = System.Drawing.Color.Transparent;
            this.WeatherDetailLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WeatherDetailLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.WeatherDetailLB.Location = new System.Drawing.Point(10, 0);
            this.WeatherDetailLB.Name = "WeatherDetailLB";
            this.WeatherDetailLB.Size = new System.Drawing.Size(112, 25);
            this.WeatherDetailLB.TabIndex = 2;
            this.WeatherDetailLB.Text = "측기 상세정보";
            this.WeatherDetailLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WeatherDataTextBox
            // 
            this.WeatherDataTextBox.BackColor = System.Drawing.Color.LightGray;
            this.WeatherDataTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WeatherDataTextBox.ForeColor = System.Drawing.Color.Black;
            this.WeatherDataTextBox.Location = new System.Drawing.Point(0, 25);
            this.WeatherDataTextBox.Name = "WeatherDataTextBox";
            this.WeatherDataTextBox.Size = new System.Drawing.Size(973, 146);
            this.WeatherDataTextBox.TabIndex = 1;
            this.WeatherDataTextBox.Text = "";
            // 
            // WeatherDataPN
            // 
            this.WeatherDataPN.BackgroundImage = global::WeatherForm.Properties.Resources.DMB_main;
            this.WeatherDataPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WeatherDataPN.Controls.Add(this.WeatherDataResetLB);
            this.WeatherDataPN.Controls.Add(this.WeatherDataLB);
            this.WeatherDataPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WeatherDataPN.Location = new System.Drawing.Point(0, 0);
            this.WeatherDataPN.Name = "WeatherDataPN";
            this.WeatherDataPN.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.WeatherDataPN.Size = new System.Drawing.Size(973, 25);
            this.WeatherDataPN.TabIndex = 0;
            // 
            // WeatherDataResetLB
            // 
            this.WeatherDataResetLB.BackColor = System.Drawing.Color.Transparent;
            this.WeatherDataResetLB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WeatherDataResetLB.Dock = System.Windows.Forms.DockStyle.Right;
            this.WeatherDataResetLB.Image = global::WeatherForm.Properties.Resources.DMB_reset;
            this.WeatherDataResetLB.Location = new System.Drawing.Point(953, 0);
            this.WeatherDataResetLB.Name = "WeatherDataResetLB";
            this.WeatherDataResetLB.Size = new System.Drawing.Size(20, 25);
            this.WeatherDataResetLB.TabIndex = 2;
            this.MainToolTip.SetToolTip(this.WeatherDataResetLB, "데이터 로그 초기화");
            this.WeatherDataResetLB.Click += new System.EventHandler(this.WeatherDataResetLB_Click);
            // 
            // WeatherDataLB
            // 
            this.WeatherDataLB.BackColor = System.Drawing.Color.Transparent;
            this.WeatherDataLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WeatherDataLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.WeatherDataLB.Location = new System.Drawing.Point(10, 0);
            this.WeatherDataLB.Name = "WeatherDataLB";
            this.WeatherDataLB.Size = new System.Drawing.Size(129, 25);
            this.WeatherDataLB.TabIndex = 1;
            this.WeatherDataLB.Text = "측기 데이터 로그";
            this.WeatherDataLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WeatherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 688);
            this.Controls.Add(this.SplitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WeatherForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "우량기 상태";
            this.Activated += new System.EventHandler(this.WeatherForm_Activated);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.ResumeLayout(false);
            this.MainContextMenu.ResumeLayout(false);
            this.WeatherListPN.ResumeLayout(false);
            this.WeatherDetailMainPN.ResumeLayout(false);
            this.WeatherDetailPN.ResumeLayout(false);
            this.WeatherDataPN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.Panel WeatherListPN;
        private System.Windows.Forms.ListView WeatherListView;
        private System.Windows.Forms.Panel WeatherDetailPN;
        private System.Windows.Forms.RichTextBox WeatherDataTextBox;
        private System.Windows.Forms.Panel WeatherDataPN;
        private System.Windows.Forms.Panel WeatherDetailMainPN;
        private System.Windows.Forms.Label WeatherListLB;
        private System.Windows.Forms.Label WeatherDetailLB;
        private System.Windows.Forms.Label WeatherDataLB;
        private System.Windows.Forms.Label WeatherDataResetLB;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.Label SameFTimeLB;
        private System.Windows.Forms.Label FWVerStateLB;
        private System.Windows.Forms.Label DoorStateLB;
        private System.Windows.Forms.Label FanStateLB;
        private System.Windows.Forms.Label BattStateLB;
        private System.Windows.Forms.Label BattLifeLB;
        private System.Windows.Forms.Label BattTempoLB;
        private System.Windows.Forms.Label BattCLB;
        private System.Windows.Forms.Label BattALB;
        private System.Windows.Forms.Label BattVoltLB;
        private System.Windows.Forms.Label CdmaRssiLB;
        private System.Windows.Forms.Label CdmaPortLB;
        private System.Windows.Forms.Label CdmaIpLB;
        private System.Windows.Forms.Label DownFTimeLB;
        private System.Windows.Forms.Label RemarkLB;
        private System.Windows.Forms.ToolTip RemarkToolTip;
        private System.Windows.Forms.ContextMenuStrip MainContextMenu;
        private System.Windows.Forms.ToolStripMenuItem 상태요청ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 제어ToolStripMenuItem;
        private System.Windows.Forms.Label BattState2LB;
        private System.Windows.Forms.Label BattLife2LB;
        private System.Windows.Forms.Label BattTempo2LB;
        private System.Windows.Forms.Label BattC2LB;
        private System.Windows.Forms.Label BattA2LB;
        private System.Windows.Forms.Label BattVolt2LB;
    }
}

