namespace ADEng.Module.WeatherSystem
{
    partial class WeatherSelfForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeatherSelfForm));
            this.SelfTestMainPN = new System.Windows.Forms.Panel();
            this.SelfTestMainLB = new System.Windows.Forms.Label();
            this.SelfTestMainPB = new System.Windows.Forms.PictureBox();
            this.SelfTestSidePN = new System.Windows.Forms.Panel();
            this.SelfTestDevicePN = new System.Windows.Forms.Panel();
            this.SelfTestDeviceLV = new System.Windows.Forms.ListView();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.SelfTestDeviceTopPN = new System.Windows.Forms.Panel();
            this.SelfTestDeviceTopLB = new System.Windows.Forms.Label();
            this.SelfTestBtn = new System.Windows.Forms.Button();
            this.SelfTestMainGB = new System.Windows.Forms.GroupBox();
            this.RainFallCB = new System.Windows.Forms.CheckBox();
            this.WaterLevelCB = new System.Windows.Forms.CheckBox();
            this.WaterFlowCB = new System.Windows.Forms.CheckBox();
            this.SunBattCB = new System.Windows.Forms.CheckBox();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.SelfTestMainPN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SelfTestMainPB)).BeginInit();
            this.SelfTestDevicePN.SuspendLayout();
            this.SelfTestDeviceTopPN.SuspendLayout();
            this.SelfTestMainGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelfTestMainPN
            // 
            this.SelfTestMainPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SelfTestMainPN.BackgroundImage")));
            this.SelfTestMainPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SelfTestMainPN.Controls.Add(this.SelfTestMainLB);
            this.SelfTestMainPN.Controls.Add(this.SelfTestMainPB);
            this.SelfTestMainPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SelfTestMainPN.Location = new System.Drawing.Point(0, 0);
            this.SelfTestMainPN.Name = "SelfTestMainPN";
            this.SelfTestMainPN.Size = new System.Drawing.Size(732, 40);
            this.SelfTestMainPN.TabIndex = 0;
            // 
            // SelfTestMainLB
            // 
            this.SelfTestMainLB.AutoSize = true;
            this.SelfTestMainLB.BackColor = System.Drawing.Color.Transparent;
            this.SelfTestMainLB.Location = new System.Drawing.Point(12, 9);
            this.SelfTestMainLB.Name = "SelfTestMainLB";
            this.SelfTestMainLB.Size = new System.Drawing.Size(245, 12);
            this.SelfTestMainLB.TabIndex = 2;
            this.SelfTestMainLB.Text = "측기의 센서 및 태양전지 상태를 확인합니다.";
            // 
            // SelfTestMainPB
            // 
            this.SelfTestMainPB.BackColor = System.Drawing.Color.Transparent;
            this.SelfTestMainPB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SelfTestMainPB.BackgroundImage")));
            this.SelfTestMainPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SelfTestMainPB.Location = new System.Drawing.Point(675, 3);
            this.SelfTestMainPB.Name = "SelfTestMainPB";
            this.SelfTestMainPB.Size = new System.Drawing.Size(39, 35);
            this.SelfTestMainPB.TabIndex = 1;
            this.SelfTestMainPB.TabStop = false;
            // 
            // SelfTestSidePN
            // 
            this.SelfTestSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SelfTestSidePN.BackgroundImage")));
            this.SelfTestSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SelfTestSidePN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SelfTestSidePN.Location = new System.Drawing.Point(0, 40);
            this.SelfTestSidePN.Name = "SelfTestSidePN";
            this.SelfTestSidePN.Size = new System.Drawing.Size(732, 5);
            this.SelfTestSidePN.TabIndex = 3;
            // 
            // SelfTestDevicePN
            // 
            this.SelfTestDevicePN.Controls.Add(this.SelfTestDeviceLV);
            this.SelfTestDevicePN.Controls.Add(this.SelfTestDeviceTopPN);
            this.SelfTestDevicePN.Location = new System.Drawing.Point(4, 45);
            this.SelfTestDevicePN.Name = "SelfTestDevicePN";
            this.SelfTestDevicePN.Size = new System.Drawing.Size(725, 350);
            this.SelfTestDevicePN.TabIndex = 4;
            // 
            // SelfTestDeviceLV
            // 
            this.SelfTestDeviceLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelfTestDeviceLV.FullRowSelect = true;
            this.SelfTestDeviceLV.GridLines = true;
            this.SelfTestDeviceLV.HideSelection = false;
            this.SelfTestDeviceLV.Location = new System.Drawing.Point(0, 25);
            this.SelfTestDeviceLV.Name = "SelfTestDeviceLV";
            this.SelfTestDeviceLV.Size = new System.Drawing.Size(725, 325);
            this.SelfTestDeviceLV.StateImageList = this.MainImageList;
            this.SelfTestDeviceLV.TabIndex = 2;
            this.SelfTestDeviceLV.UseCompatibleStateImageBehavior = false;
            this.SelfTestDeviceLV.View = System.Windows.Forms.View.Details;
            // 
            // MainImageList
            // 
            this.MainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.MainImageList.ImageSize = new System.Drawing.Size(20, 20);
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SelfTestDeviceTopPN
            // 
            this.SelfTestDeviceTopPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SelfTestDeviceTopPN.BackgroundImage")));
            this.SelfTestDeviceTopPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SelfTestDeviceTopPN.Controls.Add(this.SelfTestDeviceTopLB);
            this.SelfTestDeviceTopPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.SelfTestDeviceTopPN.Location = new System.Drawing.Point(0, 0);
            this.SelfTestDeviceTopPN.Name = "SelfTestDeviceTopPN";
            this.SelfTestDeviceTopPN.Size = new System.Drawing.Size(725, 25);
            this.SelfTestDeviceTopPN.TabIndex = 1;
            // 
            // SelfTestDeviceTopLB
            // 
            this.SelfTestDeviceTopLB.BackColor = System.Drawing.Color.Transparent;
            this.SelfTestDeviceTopLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.SelfTestDeviceTopLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SelfTestDeviceTopLB.Location = new System.Drawing.Point(0, 0);
            this.SelfTestDeviceTopLB.Name = "SelfTestDeviceTopLB";
            this.SelfTestDeviceTopLB.Size = new System.Drawing.Size(79, 25);
            this.SelfTestDeviceTopLB.TabIndex = 0;
            this.SelfTestDeviceTopLB.Text = "  측기 정보";
            this.SelfTestDeviceTopLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SelfTestBtn
            // 
            this.SelfTestBtn.Location = new System.Drawing.Point(564, 415);
            this.SelfTestBtn.Name = "SelfTestBtn";
            this.SelfTestBtn.Size = new System.Drawing.Size(75, 23);
            this.SelfTestBtn.TabIndex = 5;
            this.SelfTestBtn.Text = "요청";
            this.SelfTestBtn.UseVisualStyleBackColor = true;
            this.SelfTestBtn.Click += new System.EventHandler(this.SelfTestBtn_Click);
            // 
            // SelfTestMainGB
            // 
            this.SelfTestMainGB.Controls.Add(this.RainFallCB);
            this.SelfTestMainGB.Controls.Add(this.WaterLevelCB);
            this.SelfTestMainGB.Controls.Add(this.WaterFlowCB);
            this.SelfTestMainGB.Controls.Add(this.SunBattCB);
            this.SelfTestMainGB.Location = new System.Drawing.Point(14, 401);
            this.SelfTestMainGB.Name = "SelfTestMainGB";
            this.SelfTestMainGB.Size = new System.Drawing.Size(453, 37);
            this.SelfTestMainGB.TabIndex = 6;
            this.SelfTestMainGB.TabStop = false;
            this.SelfTestMainGB.Text = "요청 항목";
            // 
            // RainFallCB
            // 
            this.RainFallCB.AutoSize = true;
            this.RainFallCB.Checked = true;
            this.RainFallCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RainFallCB.Location = new System.Drawing.Point(62, 15);
            this.RainFallCB.Name = "RainFallCB";
            this.RainFallCB.Size = new System.Drawing.Size(76, 16);
            this.RainFallCB.TabIndex = 3;
            this.RainFallCB.Text = "강수 센서";
            this.RainFallCB.UseVisualStyleBackColor = true;
            // 
            // WaterLevelCB
            // 
            this.WaterLevelCB.AutoSize = true;
            this.WaterLevelCB.Checked = true;
            this.WaterLevelCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WaterLevelCB.Location = new System.Drawing.Point(146, 15);
            this.WaterLevelCB.Name = "WaterLevelCB";
            this.WaterLevelCB.Size = new System.Drawing.Size(76, 16);
            this.WaterLevelCB.TabIndex = 2;
            this.WaterLevelCB.Text = "수위 센서";
            this.WaterLevelCB.UseVisualStyleBackColor = true;
            // 
            // WaterFlowCB
            // 
            this.WaterFlowCB.AutoSize = true;
            this.WaterFlowCB.Checked = true;
            this.WaterFlowCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WaterFlowCB.Location = new System.Drawing.Point(230, 15);
            this.WaterFlowCB.Name = "WaterFlowCB";
            this.WaterFlowCB.Size = new System.Drawing.Size(76, 16);
            this.WaterFlowCB.TabIndex = 1;
            this.WaterFlowCB.Text = "유속 센서";
            this.WaterFlowCB.UseVisualStyleBackColor = true;
            // 
            // SunBattCB
            // 
            this.SunBattCB.AutoSize = true;
            this.SunBattCB.Checked = true;
            this.SunBattCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SunBattCB.Location = new System.Drawing.Point(314, 15);
            this.SunBattCB.Name = "SunBattCB";
            this.SunBattCB.Size = new System.Drawing.Size(76, 16);
            this.SunBattCB.TabIndex = 0;
            this.SunBattCB.Text = "태양 전지";
            this.SunBattCB.UseVisualStyleBackColor = true;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(645, 415);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 7;
            this.CloseBtn.Text = "닫기";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // WeatherSelfForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 444);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.SelfTestMainGB);
            this.Controls.Add(this.SelfTestBtn);
            this.Controls.Add(this.SelfTestDevicePN);
            this.Controls.Add(this.SelfTestSidePN);
            this.Controls.Add(this.SelfTestMainPN);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WeatherSelfForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "자가 진단";
            this.TopMost = true;
            this.SelfTestMainPN.ResumeLayout(false);
            this.SelfTestMainPN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SelfTestMainPB)).EndInit();
            this.SelfTestDevicePN.ResumeLayout(false);
            this.SelfTestDeviceTopPN.ResumeLayout(false);
            this.SelfTestMainGB.ResumeLayout(false);
            this.SelfTestMainGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SelfTestMainPN;
        private System.Windows.Forms.PictureBox SelfTestMainPB;
        private System.Windows.Forms.Label SelfTestMainLB;
        private System.Windows.Forms.Panel SelfTestSidePN;
        private System.Windows.Forms.Panel SelfTestDevicePN;
        private System.Windows.Forms.ListView SelfTestDeviceLV;
        private System.Windows.Forms.Panel SelfTestDeviceTopPN;
        private System.Windows.Forms.Label SelfTestDeviceTopLB;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.Button SelfTestBtn;
        private System.Windows.Forms.GroupBox SelfTestMainGB;
        private System.Windows.Forms.CheckBox SunBattCB;
        private System.Windows.Forms.CheckBox RainFallCB;
        private System.Windows.Forms.CheckBox WaterLevelCB;
        private System.Windows.Forms.CheckBox WaterFlowCB;
        private System.Windows.Forms.Button CloseBtn;
    }
}

