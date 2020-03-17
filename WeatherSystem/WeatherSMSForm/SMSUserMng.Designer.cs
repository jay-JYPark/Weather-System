namespace ADEng.Module.WeatherSystem
{
    partial class SMSUserMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMSUserMng));
            this.SmsUserMngPN = new System.Windows.Forms.Panel();
            this.SmsUserMngPB = new System.Windows.Forms.PictureBox();
            this.SmsUserMngLB = new System.Windows.Forms.Label();
            this.SmsUserMngSidePN = new System.Windows.Forms.Panel();
            this.SmsMngMainPN = new System.Windows.Forms.Panel();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SmsUserRemarkLB = new System.Windows.Forms.Label();
            this.SmsUserRemarkTB = new System.Windows.Forms.TextBox();
            this.SelectWDeviceLB = new System.Windows.Forms.Label();
            this.SelectWDeviceLV = new System.Windows.Forms.ListView();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.SmsUserTelNumTB = new System.Windows.Forms.TextBox();
            this.SmsUserNameTB = new System.Windows.Forms.TextBox();
            this.SmsUserTelNumLB = new System.Windows.Forms.Label();
            this.SmsUserNameLB = new System.Windows.Forms.Label();
            this.SmsUserDataPN = new System.Windows.Forms.Panel();
            this.SmsUserDataLB = new System.Windows.Forms.Label();
            this.WDeviceTV = new System.Windows.Forms.TreeView();
            this.WDevicePN = new System.Windows.Forms.Panel();
            this.WDeviceLB = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancleBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.SouthSidePN = new System.Windows.Forms.Panel();
            this.RecvKindLB = new System.Windows.Forms.Label();
            this.RecvKindBtn = new System.Windows.Forms.Button();
            this.SmsUserMngPN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmsUserMngPB)).BeginInit();
            this.SmsMngMainPN.SuspendLayout();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.SmsUserDataPN.SuspendLayout();
            this.WDevicePN.SuspendLayout();
            this.SuspendLayout();
            // 
            // SmsUserMngPN
            // 
            this.SmsUserMngPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsUserMngPN.BackgroundImage")));
            this.SmsUserMngPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsUserMngPN.Controls.Add(this.SmsUserMngPB);
            this.SmsUserMngPN.Controls.Add(this.SmsUserMngLB);
            this.SmsUserMngPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsUserMngPN.Location = new System.Drawing.Point(0, 0);
            this.SmsUserMngPN.Name = "SmsUserMngPN";
            this.SmsUserMngPN.Size = new System.Drawing.Size(433, 40);
            this.SmsUserMngPN.TabIndex = 0;
            // 
            // SmsUserMngPB
            // 
            this.SmsUserMngPB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SmsUserMngPB.BackColor = System.Drawing.Color.Transparent;
            this.SmsUserMngPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsUserMngPB.Location = new System.Drawing.Point(381, 3);
            this.SmsUserMngPB.Name = "SmsUserMngPB";
            this.SmsUserMngPB.Size = new System.Drawing.Size(39, 35);
            this.SmsUserMngPB.TabIndex = 2;
            this.SmsUserMngPB.TabStop = false;
            // 
            // SmsUserMngLB
            // 
            this.SmsUserMngLB.AutoSize = true;
            this.SmsUserMngLB.BackColor = System.Drawing.Color.Transparent;
            this.SmsUserMngLB.Location = new System.Drawing.Point(12, 9);
            this.SmsUserMngLB.Name = "SmsUserMngLB";
            this.SmsUserMngLB.Size = new System.Drawing.Size(0, 12);
            this.SmsUserMngLB.TabIndex = 1;
            // 
            // SmsUserMngSidePN
            // 
            this.SmsUserMngSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsUserMngSidePN.BackgroundImage")));
            this.SmsUserMngSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsUserMngSidePN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsUserMngSidePN.Location = new System.Drawing.Point(0, 40);
            this.SmsUserMngSidePN.Name = "SmsUserMngSidePN";
            this.SmsUserMngSidePN.Size = new System.Drawing.Size(433, 5);
            this.SmsUserMngSidePN.TabIndex = 3;
            // 
            // SmsMngMainPN
            // 
            this.SmsMngMainPN.Controls.Add(this.SplitContainer1);
            this.SmsMngMainPN.Location = new System.Drawing.Point(0, 43);
            this.SmsMngMainPN.Name = "SmsMngMainPN";
            this.SmsMngMainPN.Size = new System.Drawing.Size(430, 254);
            this.SmsMngMainPN.TabIndex = 4;
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.RecvKindBtn);
            this.SplitContainer1.Panel1.Controls.Add(this.RecvKindLB);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserRemarkLB);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserRemarkTB);
            this.SplitContainer1.Panel1.Controls.Add(this.SelectWDeviceLB);
            this.SplitContainer1.Panel1.Controls.Add(this.SelectWDeviceLV);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserTelNumTB);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserNameTB);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserTelNumLB);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserNameLB);
            this.SplitContainer1.Panel1.Controls.Add(this.SmsUserDataPN);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.WDeviceTV);
            this.SplitContainer1.Panel2.Controls.Add(this.WDevicePN);
            this.SplitContainer1.Size = new System.Drawing.Size(430, 254);
            this.SplitContainer1.SplitterDistance = 247;
            this.SplitContainer1.TabIndex = 0;
            // 
            // SmsUserRemarkLB
            // 
            this.SmsUserRemarkLB.Location = new System.Drawing.Point(15, 191);
            this.SmsUserRemarkLB.Name = "SmsUserRemarkLB";
            this.SmsUserRemarkLB.Size = new System.Drawing.Size(72, 21);
            this.SmsUserRemarkLB.TabIndex = 28;
            this.SmsUserRemarkLB.Text = "Remark :";
            this.SmsUserRemarkLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmsUserRemarkTB
            // 
            this.SmsUserRemarkTB.Location = new System.Drawing.Point(87, 191);
            this.SmsUserRemarkTB.MaxLength = 20;
            this.SmsUserRemarkTB.Name = "SmsUserRemarkTB";
            this.SmsUserRemarkTB.Size = new System.Drawing.Size(151, 21);
            this.SmsUserRemarkTB.TabIndex = 27;
            // 
            // SelectWDeviceLB
            // 
            this.SelectWDeviceLB.Location = new System.Drawing.Point(15, 95);
            this.SelectWDeviceLB.Name = "SelectWDeviceLB";
            this.SelectWDeviceLB.Size = new System.Drawing.Size(72, 21);
            this.SelectWDeviceLB.TabIndex = 26;
            this.SelectWDeviceLB.Text = "관리측기 :";
            this.SelectWDeviceLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SelectWDeviceLV
            // 
            this.SelectWDeviceLV.Location = new System.Drawing.Point(87, 95);
            this.SelectWDeviceLV.Name = "SelectWDeviceLV";
            this.SelectWDeviceLV.Size = new System.Drawing.Size(151, 88);
            this.SelectWDeviceLV.SmallImageList = this.MainImageList;
            this.SelectWDeviceLV.TabIndex = 25;
            this.SelectWDeviceLV.UseCompatibleStateImageBehavior = false;
            this.SelectWDeviceLV.View = System.Windows.Forms.View.SmallIcon;
            // 
            // MainImageList
            // 
            this.MainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.MainImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SmsUserTelNumTB
            // 
            this.SmsUserTelNumTB.Location = new System.Drawing.Point(87, 66);
            this.SmsUserTelNumTB.MaxLength = 13;
            this.SmsUserTelNumTB.Name = "SmsUserTelNumTB";
            this.SmsUserTelNumTB.Size = new System.Drawing.Size(151, 21);
            this.SmsUserTelNumTB.TabIndex = 22;
            this.SmsUserTelNumTB.TextChanged += new System.EventHandler(this.SmsUserNameTB_TextChanged);
            this.SmsUserTelNumTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SmsUserTelNumTB_KeyDown);
            // 
            // SmsUserNameTB
            // 
            this.SmsUserNameTB.Location = new System.Drawing.Point(87, 37);
            this.SmsUserNameTB.MaxLength = 10;
            this.SmsUserNameTB.Name = "SmsUserNameTB";
            this.SmsUserNameTB.Size = new System.Drawing.Size(151, 21);
            this.SmsUserNameTB.TabIndex = 21;
            this.SmsUserNameTB.TextChanged += new System.EventHandler(this.SmsUserNameTB_TextChanged);
            // 
            // SmsUserTelNumLB
            // 
            this.SmsUserTelNumLB.Location = new System.Drawing.Point(15, 66);
            this.SmsUserTelNumLB.Name = "SmsUserTelNumLB";
            this.SmsUserTelNumLB.Size = new System.Drawing.Size(72, 21);
            this.SmsUserTelNumLB.TabIndex = 24;
            this.SmsUserTelNumLB.Text = "전화번호 :";
            this.SmsUserTelNumLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmsUserNameLB
            // 
            this.SmsUserNameLB.Location = new System.Drawing.Point(15, 37);
            this.SmsUserNameLB.Name = "SmsUserNameLB";
            this.SmsUserNameLB.Size = new System.Drawing.Size(72, 21);
            this.SmsUserNameLB.TabIndex = 23;
            this.SmsUserNameLB.Text = "이름 :";
            this.SmsUserNameLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmsUserDataPN
            // 
            this.SmsUserDataPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsUserDataPN.BackgroundImage")));
            this.SmsUserDataPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsUserDataPN.Controls.Add(this.SmsUserDataLB);
            this.SmsUserDataPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsUserDataPN.Location = new System.Drawing.Point(0, 0);
            this.SmsUserDataPN.Name = "SmsUserDataPN";
            this.SmsUserDataPN.Size = new System.Drawing.Size(247, 25);
            this.SmsUserDataPN.TabIndex = 3;
            // 
            // SmsUserDataLB
            // 
            this.SmsUserDataLB.BackColor = System.Drawing.Color.Transparent;
            this.SmsUserDataLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.SmsUserDataLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SmsUserDataLB.Location = new System.Drawing.Point(0, 0);
            this.SmsUserDataLB.Name = "SmsUserDataLB";
            this.SmsUserDataLB.Size = new System.Drawing.Size(125, 25);
            this.SmsUserDataLB.TabIndex = 0;
            this.SmsUserDataLB.Text = "  SMS 사용자 정보";
            this.SmsUserDataLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.WDeviceTV.Size = new System.Drawing.Size(179, 229);
            this.WDeviceTV.TabIndex = 3;
            this.WDeviceTV.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.WDeviceTV_AfterCheck);
            this.WDeviceTV.DoubleClick += new System.EventHandler(this.WDeviceTV_DoubleClick);
            // 
            // WDevicePN
            // 
            this.WDevicePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDevicePN.BackgroundImage")));
            this.WDevicePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDevicePN.Controls.Add(this.WDeviceLB);
            this.WDevicePN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WDevicePN.Location = new System.Drawing.Point(0, 0);
            this.WDevicePN.Name = "WDevicePN";
            this.WDevicePN.Size = new System.Drawing.Size(179, 25);
            this.WDevicePN.TabIndex = 2;
            // 
            // WDeviceLB
            // 
            this.WDeviceLB.BackColor = System.Drawing.Color.Transparent;
            this.WDeviceLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WDeviceLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.WDeviceLB.Location = new System.Drawing.Point(0, 0);
            this.WDeviceLB.Name = "WDeviceLB";
            this.WDeviceLB.Size = new System.Drawing.Size(80, 25);
            this.WDeviceLB.TabIndex = 0;
            this.WDeviceLB.Text = "  측기 선택";
            this.WDeviceLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(183, 314);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 5;
            this.OkBtn.Text = "확인";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancleBtn
            // 
            this.CancleBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancleBtn.Location = new System.Drawing.Point(264, 314);
            this.CancleBtn.Name = "CancleBtn";
            this.CancleBtn.Size = new System.Drawing.Size(75, 23);
            this.CancleBtn.TabIndex = 6;
            this.CancleBtn.Text = "취소";
            this.CancleBtn.UseVisualStyleBackColor = true;
            this.CancleBtn.Click += new System.EventHandler(this.CancleBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBtn.Enabled = false;
            this.SaveBtn.Location = new System.Drawing.Point(345, 314);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 7;
            this.SaveBtn.Text = "적용";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // SouthSidePN
            // 
            this.SouthSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SouthSidePN.BackgroundImage")));
            this.SouthSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SouthSidePN.Location = new System.Drawing.Point(0, 303);
            this.SouthSidePN.Name = "SouthSidePN";
            this.SouthSidePN.Size = new System.Drawing.Size(433, 5);
            this.SouthSidePN.TabIndex = 8;
            // 
            // RecvKindLB
            // 
            this.RecvKindLB.Location = new System.Drawing.Point(15, 223);
            this.RecvKindLB.Name = "RecvKindLB";
            this.RecvKindLB.Size = new System.Drawing.Size(72, 21);
            this.RecvKindLB.TabIndex = 29;
            this.RecvKindLB.Text = "수신항목 :";
            this.RecvKindLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RecvKindBtn
            // 
            this.RecvKindBtn.Location = new System.Drawing.Point(87, 221);
            this.RecvKindBtn.Name = "RecvKindBtn";
            this.RecvKindBtn.Size = new System.Drawing.Size(100, 23);
            this.RecvKindBtn.TabIndex = 30;
            this.RecvKindBtn.Text = "사용자 지정...";
            this.RecvKindBtn.UseVisualStyleBackColor = true;
            this.RecvKindBtn.Click += new System.EventHandler(this.RecvKindBtn_Click);
            // 
            // SMSUserMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 345);
            this.Controls.Add(this.SouthSidePN);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CancleBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.SmsMngMainPN);
            this.Controls.Add(this.SmsUserMngSidePN);
            this.Controls.Add(this.SmsUserMngPN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SMSUserMng";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SmsUserMngPN.ResumeLayout(false);
            this.SmsUserMngPN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SmsUserMngPB)).EndInit();
            this.SmsMngMainPN.ResumeLayout(false);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.SmsUserDataPN.ResumeLayout(false);
            this.WDevicePN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SmsUserMngPN;
        private System.Windows.Forms.Label SmsUserMngLB;
        private System.Windows.Forms.PictureBox SmsUserMngPB;
        private System.Windows.Forms.Panel SmsUserMngSidePN;
        private System.Windows.Forms.Panel SmsMngMainPN;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancleBtn;
        private System.Windows.Forms.Button SaveBtn;
        public System.Windows.Forms.TreeView WDeviceTV;
        private System.Windows.Forms.Panel WDevicePN;
        private System.Windows.Forms.Label WDeviceLB;
        private System.Windows.Forms.Panel SmsUserDataPN;
        private System.Windows.Forms.Label SmsUserDataLB;
        private System.Windows.Forms.Label SmsUserRemarkLB;
        private System.Windows.Forms.TextBox SmsUserRemarkTB;
        private System.Windows.Forms.Label SelectWDeviceLB;
        private System.Windows.Forms.ListView SelectWDeviceLV;
        private System.Windows.Forms.TextBox SmsUserTelNumTB;
        private System.Windows.Forms.TextBox SmsUserNameTB;
        private System.Windows.Forms.Label SmsUserTelNumLB;
        private System.Windows.Forms.Label SmsUserNameLB;
        private System.Windows.Forms.Panel SouthSidePN;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.Label RecvKindLB;
        private System.Windows.Forms.Button RecvKindBtn;

    }
}