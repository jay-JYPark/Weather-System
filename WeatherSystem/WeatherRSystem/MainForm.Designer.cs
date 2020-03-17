namespace ADEng.Module.WeatherSystem
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.종료XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.보기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.우량기상태WToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.우량기제어RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMS그룹관리SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이력조회DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.정보통계ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.자가진단ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.우량기정보관리GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.환경설정OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.도움말ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.프로그램정보IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatus = new System.Windows.Forms.StatusStrip();
            this.MainTool = new System.Windows.Forms.ToolStrip();
            this.WeatherToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.WeatherCtrToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.WeatherSmsToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.WeatherRecordToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.WeatherStateToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.ToolStripLB1 = new System.Windows.Forms.ToolStripLabel();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.MenuStrip.SuspendLayout();
            this.MainTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.AutoSize = false;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.보기ToolStripMenuItem,
            this.설정ToolStripMenuItem,
            this.도움말ToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.MenuStrip.Size = new System.Drawing.Size(1016, 20);
            this.MenuStrip.TabIndex = 1;
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.종료XToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.파일ToolStripMenuItem.Text = "파일(&F)";
            // 
            // 종료XToolStripMenuItem
            // 
            this.종료XToolStripMenuItem.Name = "종료XToolStripMenuItem";
            this.종료XToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.종료XToolStripMenuItem.Text = "종료(&X)";
            this.종료XToolStripMenuItem.Click += new System.EventHandler(this.종료XToolStripMenuItem_Click);
            // 
            // 보기ToolStripMenuItem
            // 
            this.보기ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.우량기상태WToolStripMenuItem,
            this.우량기제어RToolStripMenuItem,
            this.sMS그룹관리SToolStripMenuItem,
            this.이력조회DToolStripMenuItem,
            this.정보통계ToolStripMenuItem,
            this.ToolStripSeparator2,
            this.자가진단ToolStripMenuItem});
            this.보기ToolStripMenuItem.Name = "보기ToolStripMenuItem";
            this.보기ToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.보기ToolStripMenuItem.Text = "보기(&V)";
            // 
            // 우량기상태WToolStripMenuItem
            // 
            this.우량기상태WToolStripMenuItem.Image = global::WeatherRSystem.Properties.Resources.우량기상태;
            this.우량기상태WToolStripMenuItem.Name = "우량기상태WToolStripMenuItem";
            this.우량기상태WToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.우량기상태WToolStripMenuItem.Text = "측기 시스템 상태(&W)";
            this.우량기상태WToolStripMenuItem.Click += new System.EventHandler(this.WeatherToolStripBtn_Click);
            // 
            // 우량기제어RToolStripMenuItem
            // 
            this.우량기제어RToolStripMenuItem.Image = global::WeatherRSystem.Properties.Resources.우량기제어;
            this.우량기제어RToolStripMenuItem.Name = "우량기제어RToolStripMenuItem";
            this.우량기제어RToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.우량기제어RToolStripMenuItem.Text = "측기 수집 정보(&R)";
            this.우량기제어RToolStripMenuItem.Click += new System.EventHandler(this.WeatherCtrToolStripBtn_Click);
            // 
            // sMS그룹관리SToolStripMenuItem
            // 
            this.sMS그룹관리SToolStripMenuItem.Image = global::WeatherRSystem.Properties.Resources.sms사용자관리_22;
            this.sMS그룹관리SToolStripMenuItem.Name = "sMS그룹관리SToolStripMenuItem";
            this.sMS그룹관리SToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.sMS그룹관리SToolStripMenuItem.Text = "SMS 그룹 관리(&S)";
            this.sMS그룹관리SToolStripMenuItem.Click += new System.EventHandler(this.sMS그룹관리SToolStripMenuItem_Click);
            // 
            // 이력조회DToolStripMenuItem
            // 
            this.이력조회DToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("이력조회DToolStripMenuItem.Image")));
            this.이력조회DToolStripMenuItem.Name = "이력조회DToolStripMenuItem";
            this.이력조회DToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.이력조회DToolStripMenuItem.Text = "이력 조회(&D)";
            this.이력조회DToolStripMenuItem.Click += new System.EventHandler(this.WeatherRecordToolStripBtn_Click);
            // 
            // 정보통계ToolStripMenuItem
            // 
            this.정보통계ToolStripMenuItem.Enabled = false;
            this.정보통계ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("정보통계ToolStripMenuItem.Image")));
            this.정보통계ToolStripMenuItem.Name = "정보통계ToolStripMenuItem";
            this.정보통계ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.정보통계ToolStripMenuItem.Text = "정보 통계(&T)";
            this.정보통계ToolStripMenuItem.Click += new System.EventHandler(this.WeatherStateToolStripBtn_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // 자가진단ToolStripMenuItem
            // 
            this.자가진단ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("자가진단ToolStripMenuItem.Image")));
            this.자가진단ToolStripMenuItem.Name = "자가진단ToolStripMenuItem";
            this.자가진단ToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.자가진단ToolStripMenuItem.Text = "자가 진단(&E)..";
            this.자가진단ToolStripMenuItem.Click += new System.EventHandler(this.자가진단ToolStripMenuItem_Click);
            // 
            // 설정ToolStripMenuItem
            // 
            this.설정ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.우량기정보관리GToolStripMenuItem,
            this.ToolStripSeparator1,
            this.환경설정OToolStripMenuItem});
            this.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem";
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.설정ToolStripMenuItem.Text = "설정(&C)";
            // 
            // 우량기정보관리GToolStripMenuItem
            // 
            this.우량기정보관리GToolStripMenuItem.Image = global::WeatherRSystem.Properties.Resources.우량기정보관리_24_24;
            this.우량기정보관리GToolStripMenuItem.Name = "우량기정보관리GToolStripMenuItem";
            this.우량기정보관리GToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.우량기정보관리GToolStripMenuItem.Text = "측기 정보 관리(&G)..";
            this.우량기정보관리GToolStripMenuItem.Click += new System.EventHandler(this.우량기정보관리GToolStripMenuItem_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // 환경설정OToolStripMenuItem
            // 
            this.환경설정OToolStripMenuItem.Image = global::WeatherRSystem.Properties.Resources.DMB_dialogue_control_24;
            this.환경설정OToolStripMenuItem.Name = "환경설정OToolStripMenuItem";
            this.환경설정OToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.환경설정OToolStripMenuItem.Text = "환경 설정(&O)..";
            this.환경설정OToolStripMenuItem.Click += new System.EventHandler(this.환경설정OToolStripMenuItem_Click);
            // 
            // 도움말ToolStripMenuItem
            // 
            this.도움말ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.프로그램정보IToolStripMenuItem});
            this.도움말ToolStripMenuItem.Name = "도움말ToolStripMenuItem";
            this.도움말ToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.도움말ToolStripMenuItem.Text = "도움말(&H)";
            // 
            // 프로그램정보IToolStripMenuItem
            // 
            this.프로그램정보IToolStripMenuItem.Image = global::WeatherRSystem.Properties.Resources.MenuIcon_ProgramInfo;
            this.프로그램정보IToolStripMenuItem.Name = "프로그램정보IToolStripMenuItem";
            this.프로그램정보IToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.프로그램정보IToolStripMenuItem.Text = "프로그램 정보(&I)";
            this.프로그램정보IToolStripMenuItem.Click += new System.EventHandler(this.프로그램정보IToolStripMenuItem_Click);
            // 
            // MainStatus
            // 
            this.MainStatus.AutoSize = false;
            this.MainStatus.Location = new System.Drawing.Point(0, 719);
            this.MainStatus.Name = "MainStatus";
            this.MainStatus.Size = new System.Drawing.Size(1016, 22);
            this.MainStatus.TabIndex = 3;
            // 
            // MainTool
            // 
            this.MainTool.AutoSize = false;
            this.MainTool.BackgroundImage = global::WeatherRSystem.Properties.Resources.DMB_title;
            this.MainTool.GripMargin = new System.Windows.Forms.Padding(0);
            this.MainTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WeatherToolStripBtn,
            this.WeatherCtrToolStripBtn,
            this.WeatherSmsToolStripBtn,
            this.WeatherRecordToolStripBtn,
            this.WeatherStateToolStripBtn,
            this.ToolStripLB1});
            this.MainTool.Location = new System.Drawing.Point(0, 20);
            this.MainTool.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.MainTool.Name = "MainTool";
            this.MainTool.Padding = new System.Windows.Forms.Padding(3);
            this.MainTool.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainTool.Size = new System.Drawing.Size(1016, 40);
            this.MainTool.Stretch = true;
            this.MainTool.TabIndex = 2;
            // 
            // WeatherToolStripBtn
            // 
            this.WeatherToolStripBtn.AutoSize = false;
            this.WeatherToolStripBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.WeatherToolStripBtn.Image = global::WeatherRSystem.Properties.Resources.우량기상태;
            this.WeatherToolStripBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.WeatherToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WeatherToolStripBtn.Name = "WeatherToolStripBtn";
            this.WeatherToolStripBtn.Size = new System.Drawing.Size(135, 30);
            this.WeatherToolStripBtn.Text = " 측기 시스템 상태";
            this.WeatherToolStripBtn.ToolTipText = "측기 시스템 상태";
            this.WeatherToolStripBtn.Click += new System.EventHandler(this.WeatherToolStripBtn_Click);
            // 
            // WeatherCtrToolStripBtn
            // 
            this.WeatherCtrToolStripBtn.AutoSize = false;
            this.WeatherCtrToolStripBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.WeatherCtrToolStripBtn.Image = global::WeatherRSystem.Properties.Resources.우량기제어;
            this.WeatherCtrToolStripBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.WeatherCtrToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WeatherCtrToolStripBtn.Name = "WeatherCtrToolStripBtn";
            this.WeatherCtrToolStripBtn.Size = new System.Drawing.Size(128, 30);
            this.WeatherCtrToolStripBtn.Text = " 측기 수집 정보";
            this.WeatherCtrToolStripBtn.ToolTipText = "측기 수집 정보";
            this.WeatherCtrToolStripBtn.Click += new System.EventHandler(this.WeatherCtrToolStripBtn_Click);
            // 
            // WeatherSmsToolStripBtn
            // 
            this.WeatherSmsToolStripBtn.AutoSize = false;
            this.WeatherSmsToolStripBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.WeatherSmsToolStripBtn.Image = global::WeatherRSystem.Properties.Resources.sms사용자관리_22;
            this.WeatherSmsToolStripBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.WeatherSmsToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WeatherSmsToolStripBtn.Name = "WeatherSmsToolStripBtn";
            this.WeatherSmsToolStripBtn.Size = new System.Drawing.Size(128, 30);
            this.WeatherSmsToolStripBtn.Text = " SMS 그룹 관리";
            this.WeatherSmsToolStripBtn.ToolTipText = "SMS 그룹 관리";
            this.WeatherSmsToolStripBtn.Click += new System.EventHandler(this.sMS그룹관리SToolStripMenuItem_Click);
            // 
            // WeatherRecordToolStripBtn
            // 
            this.WeatherRecordToolStripBtn.AutoSize = false;
            this.WeatherRecordToolStripBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.WeatherRecordToolStripBtn.Image = ((System.Drawing.Image)(resources.GetObject("WeatherRecordToolStripBtn.Image")));
            this.WeatherRecordToolStripBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.WeatherRecordToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WeatherRecordToolStripBtn.Name = "WeatherRecordToolStripBtn";
            this.WeatherRecordToolStripBtn.Size = new System.Drawing.Size(102, 30);
            this.WeatherRecordToolStripBtn.Text = " 이력 조회";
            this.WeatherRecordToolStripBtn.ToolTipText = "이력 조회";
            this.WeatherRecordToolStripBtn.Click += new System.EventHandler(this.WeatherRecordToolStripBtn_Click);
            // 
            // WeatherStateToolStripBtn
            // 
            this.WeatherStateToolStripBtn.AutoSize = false;
            this.WeatherStateToolStripBtn.Enabled = false;
            this.WeatherStateToolStripBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.WeatherStateToolStripBtn.Image = ((System.Drawing.Image)(resources.GetObject("WeatherStateToolStripBtn.Image")));
            this.WeatherStateToolStripBtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.WeatherStateToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WeatherStateToolStripBtn.Name = "WeatherStateToolStripBtn";
            this.WeatherStateToolStripBtn.Size = new System.Drawing.Size(102, 30);
            this.WeatherStateToolStripBtn.Text = " 정보 통계";
            this.WeatherStateToolStripBtn.ToolTipText = "정보 통계";
            this.WeatherStateToolStripBtn.Click += new System.EventHandler(this.WeatherStateToolStripBtn_Click);
            // 
            // ToolStripLB1
            // 
            this.ToolStripLB1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripLB1.AutoSize = false;
            this.ToolStripLB1.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripLB1.BackgroundImage = global::WeatherRSystem.Properties.Resources.Weather_title_White_White;
            this.ToolStripLB1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ToolStripLB1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripLB1.ForeColor = System.Drawing.Color.White;
            this.ToolStripLB1.Name = "ToolStripLB1";
            this.ToolStripLB1.Size = new System.Drawing.Size(230, 40);
            // 
            // MainImageList
            // 
            this.MainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.MainImageList.ImageSize = new System.Drawing.Size(20, 20);
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 741);
            this.Controls.Add(this.MainStatus);
            this.Controls.Add(this.MainTool);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(614, 460);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "기상 연계 서버시스템";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.MainTool.ResumeLayout(false);
            this.MainTool.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 보기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 도움말ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip MainTool;
        private System.Windows.Forms.ToolStripMenuItem 종료XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 환경설정OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 프로그램정보IToolStripMenuItem;
        private System.Windows.Forms.StatusStrip MainStatus;
        private System.Windows.Forms.ToolStripButton WeatherToolStripBtn;
        private System.Windows.Forms.ToolStripMenuItem 우량기상태WToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton WeatherCtrToolStripBtn;
        private System.Windows.Forms.ToolStripMenuItem 우량기제어RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 우량기정보관리GToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel ToolStripLB1;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.ToolStripButton WeatherSmsToolStripBtn;
        private System.Windows.Forms.ToolStripMenuItem sMS그룹관리SToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton WeatherRecordToolStripBtn;
        private System.Windows.Forms.ToolStripMenuItem 이력조회DToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton WeatherStateToolStripBtn;
        private System.Windows.Forms.ToolStripMenuItem 정보통계ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 자가진단ToolStripMenuItem;
    }
}

