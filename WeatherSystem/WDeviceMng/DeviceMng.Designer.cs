namespace ADEng.Module.WeatherSystem
{
    partial class DeviceMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceMng));
            this.WDeviceLV = new System.Windows.Forms.ListView();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.CloseBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.DelBtn = new System.Windows.Forms.Button();
            this.WDeviceListPN = new System.Windows.Forms.Panel();
            this.WDeviceListLB = new System.Windows.Forms.Label();
            this.WDeviceSidePN = new System.Windows.Forms.Panel();
            this.WDeviceMainPN = new System.Windows.Forms.Panel();
            this.WDeviceMainPB = new System.Windows.Forms.PictureBox();
            this.WDeviceMainLB = new System.Windows.Forms.Label();
            this.WDeviceListPN.SuspendLayout();
            this.WDeviceMainPN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WDeviceMainPB)).BeginInit();
            this.SuspendLayout();
            // 
            // WDeviceLV
            // 
            this.WDeviceLV.FullRowSelect = true;
            this.WDeviceLV.GridLines = true;
            this.WDeviceLV.HideSelection = false;
            this.WDeviceLV.Location = new System.Drawing.Point(12, 75);
            this.WDeviceLV.Name = "WDeviceLV";
            this.WDeviceLV.Size = new System.Drawing.Size(436, 236);
            this.WDeviceLV.StateImageList = this.MainImageList;
            this.WDeviceLV.TabIndex = 5;
            this.WDeviceLV.UseCompatibleStateImageBehavior = false;
            this.WDeviceLV.View = System.Windows.Forms.View.Details;
            this.WDeviceLV.DoubleClick += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // MainImageList
            // 
            this.MainImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.MainImageList.ImageSize = new System.Drawing.Size(20, 20);
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(373, 317);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 6;
            this.CloseBtn.Text = "닫기";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(12, 317);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 7;
            this.AddBtn.Text = "등록";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(93, 317);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(75, 23);
            this.UpdateBtn.TabIndex = 8;
            this.UpdateBtn.Text = "수정";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // DelBtn
            // 
            this.DelBtn.Location = new System.Drawing.Point(174, 317);
            this.DelBtn.Name = "DelBtn";
            this.DelBtn.Size = new System.Drawing.Size(75, 23);
            this.DelBtn.TabIndex = 9;
            this.DelBtn.Text = "삭제";
            this.DelBtn.UseVisualStyleBackColor = true;
            this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
            // 
            // WDeviceListPN
            // 
            this.WDeviceListPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceListPN.BackgroundImage")));
            this.WDeviceListPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceListPN.Controls.Add(this.WDeviceListLB);
            this.WDeviceListPN.Location = new System.Drawing.Point(12, 45);
            this.WDeviceListPN.Name = "WDeviceListPN";
            this.WDeviceListPN.Size = new System.Drawing.Size(436, 24);
            this.WDeviceListPN.TabIndex = 4;
            // 
            // WDeviceListLB
            // 
            this.WDeviceListLB.BackColor = System.Drawing.SystemColors.Control;
            this.WDeviceListLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WDeviceListLB.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.WDeviceListLB.Location = new System.Drawing.Point(0, 0);
            this.WDeviceListLB.Name = "WDeviceListLB";
            this.WDeviceListLB.Size = new System.Drawing.Size(60, 24);
            this.WDeviceListLB.TabIndex = 1;
            this.WDeviceListLB.Text = "측기 목록";
            this.WDeviceListLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WDeviceSidePN
            // 
            this.WDeviceSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceSidePN.BackgroundImage")));
            this.WDeviceSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceSidePN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WDeviceSidePN.Location = new System.Drawing.Point(0, 40);
            this.WDeviceSidePN.Name = "WDeviceSidePN";
            this.WDeviceSidePN.Size = new System.Drawing.Size(460, 5);
            this.WDeviceSidePN.TabIndex = 3;
            // 
            // WDeviceMainPN
            // 
            this.WDeviceMainPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceMainPN.BackgroundImage")));
            this.WDeviceMainPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceMainPN.Controls.Add(this.WDeviceMainPB);
            this.WDeviceMainPN.Controls.Add(this.WDeviceMainLB);
            this.WDeviceMainPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WDeviceMainPN.Location = new System.Drawing.Point(0, 0);
            this.WDeviceMainPN.Name = "WDeviceMainPN";
            this.WDeviceMainPN.Size = new System.Drawing.Size(460, 40);
            this.WDeviceMainPN.TabIndex = 2;
            // 
            // WDeviceMainPB
            // 
            this.WDeviceMainPB.BackColor = System.Drawing.Color.Transparent;
            this.WDeviceMainPB.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceMainPB.BackgroundImage")));
            this.WDeviceMainPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceMainPB.Location = new System.Drawing.Point(403, 3);
            this.WDeviceMainPB.Name = "WDeviceMainPB";
            this.WDeviceMainPB.Size = new System.Drawing.Size(39, 35);
            this.WDeviceMainPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.WDeviceMainPB.TabIndex = 1;
            this.WDeviceMainPB.TabStop = false;
            // 
            // WDeviceMainLB
            // 
            this.WDeviceMainLB.AutoSize = true;
            this.WDeviceMainLB.BackColor = System.Drawing.Color.Transparent;
            this.WDeviceMainLB.Location = new System.Drawing.Point(12, 9);
            this.WDeviceMainLB.Name = "WDeviceMainLB";
            this.WDeviceMainLB.Size = new System.Drawing.Size(173, 12);
            this.WDeviceMainLB.TabIndex = 0;
            this.WDeviceMainLB.Text = "측기를 등록/수정/삭제 합니다.";
            // 
            // DeviceMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 352);
            this.Controls.Add(this.DelBtn);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.WDeviceLV);
            this.Controls.Add(this.WDeviceListPN);
            this.Controls.Add(this.WDeviceSidePN);
            this.Controls.Add(this.WDeviceMainPN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceMng";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "측기 정보 관리";
            this.WDeviceListPN.ResumeLayout(false);
            this.WDeviceMainPN.ResumeLayout(false);
            this.WDeviceMainPN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WDeviceMainPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel WDeviceSidePN;
        private System.Windows.Forms.Panel WDeviceMainPN;
        private System.Windows.Forms.PictureBox WDeviceMainPB;
        private System.Windows.Forms.Label WDeviceMainLB;
        private System.Windows.Forms.Panel WDeviceListPN;
        private System.Windows.Forms.Label WDeviceListLB;
        private System.Windows.Forms.ListView WDeviceLV;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.Button DelBtn;
        private System.Windows.Forms.ImageList MainImageList;
    }
}

