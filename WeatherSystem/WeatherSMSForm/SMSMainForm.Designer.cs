namespace ADEng.Module.WeatherSystem
{
    partial class SMSMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMSMainForm));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer3 = new System.Windows.Forms.SplitContainer();
            this.SmsUserTV = new System.Windows.Forms.TreeView();
            this.TVImageList = new System.Windows.Forms.ImageList(this.components);
            this.SmsUserPN = new System.Windows.Forms.Panel();
            this.SmsUserLB = new System.Windows.Forms.Label();
            this.SmsSendLengLB = new System.Windows.Forms.Label();
            this.SmsTestSendBtn = new System.Windows.Forms.Button();
            this.SmsTestImgPN = new System.Windows.Forms.Panel();
            this.SmsTestTB = new System.Windows.Forms.TextBox();
            this.SmsSendNumLB = new System.Windows.Forms.Label();
            this.SmsSendNumTB = new System.Windows.Forms.TextBox();
            this.SmsTestPN = new System.Windows.Forms.Panel();
            this.SmsTestLB = new System.Windows.Forms.Label();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.UserDelBtn = new System.Windows.Forms.Button();
            this.UserDetailLV = new System.Windows.Forms.ListView();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.UserUpdateBtn = new System.Windows.Forms.Button();
            this.UserAddBtn = new System.Windows.Forms.Button();
            this.UserDetailPN = new System.Windows.Forms.Panel();
            this.UserDetailLB = new System.Windows.Forms.Label();
            this.SmsSendLV = new System.Windows.Forms.ListView();
            this.SmsSendListPN = new System.Windows.Forms.Panel();
            this.SmsSendListLB = new System.Windows.Forms.Label();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.SplitContainer3.Panel1.SuspendLayout();
            this.SplitContainer3.Panel2.SuspendLayout();
            this.SplitContainer3.SuspendLayout();
            this.SmsUserPN.SuspendLayout();
            this.SmsTestImgPN.SuspendLayout();
            this.SmsTestPN.SuspendLayout();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.UserDetailPN.SuspendLayout();
            this.SmsSendListPN.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer1.IsSplitterFixed = true;
            this.SplitContainer1.Location = new System.Drawing.Point(2, 2);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.SplitContainer3);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Size = new System.Drawing.Size(973, 684);
            this.SplitContainer1.SplitterDistance = 292;
            this.SplitContainer1.TabIndex = 0;
            // 
            // SplitContainer3
            // 
            this.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer3.IsSplitterFixed = true;
            this.SplitContainer3.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer3.Name = "SplitContainer3";
            this.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer3.Panel1
            // 
            this.SplitContainer3.Panel1.Controls.Add(this.SmsUserTV);
            this.SplitContainer3.Panel1.Controls.Add(this.SmsUserPN);
            // 
            // SplitContainer3.Panel2
            // 
            this.SplitContainer3.Panel2.Controls.Add(this.SmsSendLengLB);
            this.SplitContainer3.Panel2.Controls.Add(this.SmsTestSendBtn);
            this.SplitContainer3.Panel2.Controls.Add(this.SmsTestImgPN);
            this.SplitContainer3.Panel2.Controls.Add(this.SmsSendNumLB);
            this.SplitContainer3.Panel2.Controls.Add(this.SmsSendNumTB);
            this.SplitContainer3.Panel2.Controls.Add(this.SmsTestPN);
            this.SplitContainer3.Size = new System.Drawing.Size(292, 684);
            this.SplitContainer3.SplitterDistance = 339;
            this.SplitContainer3.TabIndex = 0;
            // 
            // SmsUserTV
            // 
            this.SmsUserTV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SmsUserTV.FullRowSelect = true;
            this.SmsUserTV.HideSelection = false;
            this.SmsUserTV.ImageIndex = 0;
            this.SmsUserTV.ImageList = this.TVImageList;
            this.SmsUserTV.ItemHeight = 18;
            this.SmsUserTV.Location = new System.Drawing.Point(0, 25);
            this.SmsUserTV.Name = "SmsUserTV";
            this.SmsUserTV.SelectedImageIndex = 0;
            this.SmsUserTV.Size = new System.Drawing.Size(292, 314);
            this.SmsUserTV.TabIndex = 2;
            // 
            // TVImageList
            // 
            this.TVImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TVImageList.ImageStream")));
            this.TVImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.TVImageList.Images.SetKeyName(0, "sms담당자관리_22.png");
            this.TVImageList.Images.SetKeyName(1, "우량기.png");
            // 
            // SmsUserPN
            // 
            this.SmsUserPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsUserPN.BackgroundImage")));
            this.SmsUserPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsUserPN.Controls.Add(this.SmsUserLB);
            this.SmsUserPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsUserPN.Location = new System.Drawing.Point(0, 0);
            this.SmsUserPN.Name = "SmsUserPN";
            this.SmsUserPN.Size = new System.Drawing.Size(292, 25);
            this.SmsUserPN.TabIndex = 1;
            // 
            // SmsUserLB
            // 
            this.SmsUserLB.BackColor = System.Drawing.Color.Transparent;
            this.SmsUserLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.SmsUserLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SmsUserLB.Location = new System.Drawing.Point(0, 0);
            this.SmsUserLB.Name = "SmsUserLB";
            this.SmsUserLB.Size = new System.Drawing.Size(124, 25);
            this.SmsUserLB.TabIndex = 2;
            this.SmsUserLB.Text = "   SMS 사용자";
            this.SmsUserLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmsSendLengLB
            // 
            this.SmsSendLengLB.Location = new System.Drawing.Point(237, 284);
            this.SmsSendLengLB.Name = "SmsSendLengLB";
            this.SmsSendLengLB.Size = new System.Drawing.Size(44, 18);
            this.SmsSendLengLB.TabIndex = 5;
            this.SmsSendLengLB.Text = "0 / 80";
            this.SmsSendLengLB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SmsTestSendBtn
            // 
            this.SmsTestSendBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SmsTestSendBtn.Location = new System.Drawing.Point(0, 306);
            this.SmsTestSendBtn.Name = "SmsTestSendBtn";
            this.SmsTestSendBtn.Size = new System.Drawing.Size(292, 35);
            this.SmsTestSendBtn.TabIndex = 1;
            this.SmsTestSendBtn.Text = "전 송";
            this.SmsTestSendBtn.UseVisualStyleBackColor = true;
            this.SmsTestSendBtn.Click += new System.EventHandler(this.SmsTestSendBtn_Click);
            // 
            // SmsTestImgPN
            // 
            this.SmsTestImgPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsTestImgPN.BackgroundImage")));
            this.SmsTestImgPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsTestImgPN.Controls.Add(this.SmsTestTB);
            this.SmsTestImgPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsTestImgPN.Location = new System.Drawing.Point(0, 25);
            this.SmsTestImgPN.Name = "SmsTestImgPN";
            this.SmsTestImgPN.Size = new System.Drawing.Size(292, 248);
            this.SmsTestImgPN.TabIndex = 4;
            // 
            // SmsTestTB
            // 
            this.SmsTestTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SmsTestTB.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SmsTestTB.Location = new System.Drawing.Point(61, 62);
            this.SmsTestTB.MaxLength = 80;
            this.SmsTestTB.Multiline = true;
            this.SmsTestTB.Name = "SmsTestTB";
            this.SmsTestTB.Size = new System.Drawing.Size(170, 163);
            this.SmsTestTB.TabIndex = 0;
            this.SmsTestTB.TextChanged += new System.EventHandler(this.SmsTestTB_TextChanged);
            this.SmsTestTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SmsTestTB_KeyPress);
            // 
            // SmsSendNumLB
            // 
            this.SmsSendNumLB.AutoSize = true;
            this.SmsSendNumLB.Location = new System.Drawing.Point(37, 284);
            this.SmsSendNumLB.Name = "SmsSendNumLB";
            this.SmsSendNumLB.Size = new System.Drawing.Size(53, 12);
            this.SmsSendNumLB.TabIndex = 3;
            this.SmsSendNumLB.Text = "전화번호";
            // 
            // SmsSendNumTB
            // 
            this.SmsSendNumTB.Location = new System.Drawing.Point(96, 279);
            this.SmsSendNumTB.MaxLength = 13;
            this.SmsSendNumTB.Name = "SmsSendNumTB";
            this.SmsSendNumTB.Size = new System.Drawing.Size(100, 21);
            this.SmsSendNumTB.TabIndex = 2;
            this.SmsSendNumTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SmsSendNumTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SmsSendNumTB_KeyDown);
            // 
            // SmsTestPN
            // 
            this.SmsTestPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsTestPN.BackgroundImage")));
            this.SmsTestPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsTestPN.Controls.Add(this.SmsTestLB);
            this.SmsTestPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsTestPN.Location = new System.Drawing.Point(0, 0);
            this.SmsTestPN.Name = "SmsTestPN";
            this.SmsTestPN.Size = new System.Drawing.Size(292, 25);
            this.SmsTestPN.TabIndex = 0;
            // 
            // SmsTestLB
            // 
            this.SmsTestLB.BackColor = System.Drawing.Color.Transparent;
            this.SmsTestLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.SmsTestLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SmsTestLB.Location = new System.Drawing.Point(0, 0);
            this.SmsTestLB.Name = "SmsTestLB";
            this.SmsTestLB.Size = new System.Drawing.Size(124, 25);
            this.SmsTestLB.TabIndex = 4;
            this.SmsTestLB.Text = "   SMS 전송 시험";
            this.SmsTestLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.UserDelBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.UserDetailLV);
            this.SplitContainer2.Panel1.Controls.Add(this.UserUpdateBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.UserAddBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.UserDetailPN);
            this.SplitContainer2.Panel1MinSize = 125;
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.SmsSendLV);
            this.SplitContainer2.Panel2.Controls.Add(this.SmsSendListPN);
            this.SplitContainer2.Size = new System.Drawing.Size(677, 684);
            this.SplitContainer2.SplitterDistance = 221;
            this.SplitContainer2.TabIndex = 0;
            // 
            // UserDelBtn
            // 
            this.UserDelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UserDelBtn.Location = new System.Drawing.Point(584, 191);
            this.UserDelBtn.Name = "UserDelBtn";
            this.UserDelBtn.Size = new System.Drawing.Size(75, 23);
            this.UserDelBtn.TabIndex = 2;
            this.UserDelBtn.Text = "삭제";
            this.UserDelBtn.UseVisualStyleBackColor = true;
            this.UserDelBtn.Click += new System.EventHandler(this.UserDelBtn_Click);
            // 
            // UserDetailLV
            // 
            this.UserDetailLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.UserDetailLV.FullRowSelect = true;
            this.UserDetailLV.GridLines = true;
            this.UserDetailLV.HideSelection = false;
            this.UserDetailLV.Location = new System.Drawing.Point(0, 25);
            this.UserDetailLV.Name = "UserDetailLV";
            this.UserDetailLV.Size = new System.Drawing.Size(677, 160);
            this.UserDetailLV.StateImageList = this.MainImageList;
            this.UserDetailLV.TabIndex = 1;
            this.UserDetailLV.UseCompatibleStateImageBehavior = false;
            this.UserDetailLV.View = System.Windows.Forms.View.Details;
            this.UserDetailLV.DoubleClick += new System.EventHandler(this.UserUpdateBtn_Click);
            // 
            // MainImageList
            // 
            this.MainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.MainImageList.ImageSize = new System.Drawing.Size(20, 20);
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // UserUpdateBtn
            // 
            this.UserUpdateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UserUpdateBtn.Location = new System.Drawing.Point(503, 191);
            this.UserUpdateBtn.Name = "UserUpdateBtn";
            this.UserUpdateBtn.Size = new System.Drawing.Size(75, 23);
            this.UserUpdateBtn.TabIndex = 1;
            this.UserUpdateBtn.Text = "수정";
            this.UserUpdateBtn.UseVisualStyleBackColor = true;
            this.UserUpdateBtn.Click += new System.EventHandler(this.UserUpdateBtn_Click);
            // 
            // UserAddBtn
            // 
            this.UserAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UserAddBtn.Location = new System.Drawing.Point(422, 191);
            this.UserAddBtn.Name = "UserAddBtn";
            this.UserAddBtn.Size = new System.Drawing.Size(75, 23);
            this.UserAddBtn.TabIndex = 0;
            this.UserAddBtn.Text = "등록";
            this.UserAddBtn.UseVisualStyleBackColor = true;
            this.UserAddBtn.Click += new System.EventHandler(this.UserAddBtn_Click);
            // 
            // UserDetailPN
            // 
            this.UserDetailPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("UserDetailPN.BackgroundImage")));
            this.UserDetailPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UserDetailPN.Controls.Add(this.UserDetailLB);
            this.UserDetailPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.UserDetailPN.Location = new System.Drawing.Point(0, 0);
            this.UserDetailPN.Name = "UserDetailPN";
            this.UserDetailPN.Size = new System.Drawing.Size(677, 25);
            this.UserDetailPN.TabIndex = 0;
            // 
            // UserDetailLB
            // 
            this.UserDetailLB.BackColor = System.Drawing.Color.Transparent;
            this.UserDetailLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.UserDetailLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.UserDetailLB.Location = new System.Drawing.Point(0, 0);
            this.UserDetailLB.Name = "UserDetailLB";
            this.UserDetailLB.Size = new System.Drawing.Size(157, 25);
            this.UserDetailLB.TabIndex = 5;
            this.UserDetailLB.Text = "   SMS 사용자 상세정보";
            this.UserDetailLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmsSendLV
            // 
            this.SmsSendLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SmsSendLV.FullRowSelect = true;
            this.SmsSendLV.GridLines = true;
            this.SmsSendLV.HideSelection = false;
            this.SmsSendLV.Location = new System.Drawing.Point(0, 25);
            this.SmsSendLV.MultiSelect = false;
            this.SmsSendLV.Name = "SmsSendLV";
            this.SmsSendLV.Size = new System.Drawing.Size(677, 434);
            this.SmsSendLV.StateImageList = this.MainImageList;
            this.SmsSendLV.TabIndex = 1;
            this.SmsSendLV.UseCompatibleStateImageBehavior = false;
            this.SmsSendLV.View = System.Windows.Forms.View.Details;
            // 
            // SmsSendListPN
            // 
            this.SmsSendListPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SmsSendListPN.BackgroundImage")));
            this.SmsSendListPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SmsSendListPN.Controls.Add(this.SmsSendListLB);
            this.SmsSendListPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SmsSendListPN.Location = new System.Drawing.Point(0, 0);
            this.SmsSendListPN.Name = "SmsSendListPN";
            this.SmsSendListPN.Size = new System.Drawing.Size(677, 25);
            this.SmsSendListPN.TabIndex = 0;
            // 
            // SmsSendListLB
            // 
            this.SmsSendListLB.BackColor = System.Drawing.Color.Transparent;
            this.SmsSendListLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.SmsSendListLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SmsSendListLB.Location = new System.Drawing.Point(0, 0);
            this.SmsSendListLB.Name = "SmsSendListLB";
            this.SmsSendListLB.Size = new System.Drawing.Size(157, 25);
            this.SmsSendListLB.TabIndex = 3;
            this.SmsSendListLB.Text = "   SMS 전송 현황";
            this.SmsSendListLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SMSMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 688);
            this.Controls.Add(this.SplitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SMSMainForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SMS 사용자 관리";
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer3.Panel1.ResumeLayout(false);
            this.SplitContainer3.Panel2.ResumeLayout(false);
            this.SplitContainer3.Panel2.PerformLayout();
            this.SplitContainer3.ResumeLayout(false);
            this.SmsUserPN.ResumeLayout(false);
            this.SmsTestImgPN.ResumeLayout(false);
            this.SmsTestImgPN.PerformLayout();
            this.SmsTestPN.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.ResumeLayout(false);
            this.UserDetailPN.ResumeLayout(false);
            this.SmsSendListPN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.Button UserDelBtn;
        private System.Windows.Forms.Button UserAddBtn;
        private System.Windows.Forms.Button UserUpdateBtn;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.Panel SmsUserPN;
        private System.Windows.Forms.Label SmsUserLB;
        private System.Windows.Forms.Panel SmsSendListPN;
        private System.Windows.Forms.Label SmsSendListLB;
        private System.Windows.Forms.Panel SmsTestPN;
        private System.Windows.Forms.Label SmsTestLB;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.Label SmsSendNumLB;
        private System.Windows.Forms.TextBox SmsSendNumTB;
        private System.Windows.Forms.Button SmsTestSendBtn;
        private System.Windows.Forms.Panel SmsTestImgPN;
        private System.Windows.Forms.Label SmsSendLengLB;
        private System.Windows.Forms.Panel UserDetailPN;
        private System.Windows.Forms.Label UserDetailLB;
        private System.Windows.Forms.SplitContainer SplitContainer3;
        private System.Windows.Forms.ListView UserDetailLV;
        private System.Windows.Forms.ListView SmsSendLV;
        private System.Windows.Forms.ImageList TVImageList;
        public System.Windows.Forms.TreeView SmsUserTV;
        private System.Windows.Forms.TextBox SmsTestTB;
    }
}

