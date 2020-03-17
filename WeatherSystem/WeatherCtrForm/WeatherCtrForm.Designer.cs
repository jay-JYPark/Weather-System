namespace ADEng.Module.WeatherSystem
{
    partial class WeatherCtrForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeatherCtrForm));
            this.MainPN = new System.Windows.Forms.Panel();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.WData1LV = new System.Windows.Forms.ListView();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.WData1PN = new System.Windows.Forms.Panel();
            this.WData1LB = new System.Windows.Forms.Label();
            this.SplitContainer3 = new System.Windows.Forms.SplitContainer();
            this.WData2LV = new System.Windows.Forms.ListView();
            this.WData2PN = new System.Windows.Forms.Panel();
            this.WData2LB = new System.Windows.Forms.Label();
            this.WData3LV = new System.Windows.Forms.ListView();
            this.WData3PN = new System.Windows.Forms.Panel();
            this.WData3LB = new System.Windows.Forms.Label();
            this.DeviceIndexPN = new System.Windows.Forms.Panel();
            this.DeviceIndexLB = new System.Windows.Forms.Label();
            this.DeviceIndexLV = new System.Windows.Forms.ListView();
            this.SubImageList = new System.Windows.Forms.ImageList(this.components);
            this.ControlBtn = new System.Windows.Forms.Button();
            this.RequestBtn = new System.Windows.Forms.Button();
            this.MainPN.SuspendLayout();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.WData1PN.SuspendLayout();
            this.SplitContainer3.Panel1.SuspendLayout();
            this.SplitContainer3.Panel2.SuspendLayout();
            this.SplitContainer3.SuspendLayout();
            this.WData2PN.SuspendLayout();
            this.WData3PN.SuspendLayout();
            this.DeviceIndexPN.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPN
            // 
            this.MainPN.BackColor = System.Drawing.Color.Transparent;
            this.MainPN.Controls.Add(this.SplitContainer1);
            this.MainPN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPN.Location = new System.Drawing.Point(2, 2);
            this.MainPN.Name = "MainPN";
            this.MainPN.Size = new System.Drawing.Size(973, 684);
            this.MainPN.TabIndex = 0;
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer1.IsSplitterFixed = true;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.SplitContainer2);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.DeviceIndexPN);
            this.SplitContainer1.Panel2.Controls.Add(this.DeviceIndexLV);
            this.SplitContainer1.Panel2.Controls.Add(this.ControlBtn);
            this.SplitContainer1.Panel2.Controls.Add(this.RequestBtn);
            this.SplitContainer1.Size = new System.Drawing.Size(973, 684);
            this.SplitContainer1.SplitterDistance = 600;
            this.SplitContainer1.TabIndex = 0;
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
            this.SplitContainer2.Panel1.Controls.Add(this.WData1LV);
            this.SplitContainer2.Panel1.Controls.Add(this.WData1PN);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.SplitContainer3);
            this.SplitContainer2.Size = new System.Drawing.Size(973, 600);
            this.SplitContainer2.SplitterDistance = 198;
            this.SplitContainer2.TabIndex = 0;
            // 
            // WData1LV
            // 
            this.WData1LV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WData1LV.FullRowSelect = true;
            this.WData1LV.GridLines = true;
            this.WData1LV.HideSelection = false;
            this.WData1LV.Location = new System.Drawing.Point(0, 25);
            this.WData1LV.Name = "WData1LV";
            this.WData1LV.Size = new System.Drawing.Size(973, 173);
            this.WData1LV.SmallImageList = this.MainImageList;
            this.WData1LV.StateImageList = this.MainImageList;
            this.WData1LV.TabIndex = 1;
            this.WData1LV.UseCompatibleStateImageBehavior = false;
            this.WData1LV.View = System.Windows.Forms.View.Details;
            this.WData1LV.DoubleClick += new System.EventHandler(this.WData1LV_DoubleClick);
            // 
            // MainImageList
            // 
            this.MainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MainImageList.ImageStream")));
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MainImageList.Images.SetKeyName(0, "우량기.png");
            this.MainImageList.Images.SetKeyName(1, "수위알람 수신.png");
            // 
            // WData1PN
            // 
            this.WData1PN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WData1PN.BackgroundImage")));
            this.WData1PN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WData1PN.Controls.Add(this.WData1LB);
            this.WData1PN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WData1PN.Location = new System.Drawing.Point(0, 0);
            this.WData1PN.Name = "WData1PN";
            this.WData1PN.Size = new System.Drawing.Size(973, 25);
            this.WData1PN.TabIndex = 0;
            // 
            // WData1LB
            // 
            this.WData1LB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WData1LB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.WData1LB.Location = new System.Drawing.Point(0, 0);
            this.WData1LB.Name = "WData1LB";
            this.WData1LB.Size = new System.Drawing.Size(144, 25);
            this.WData1LB.TabIndex = 0;
            this.WData1LB.Text = "  강수 데이터 (mm)";
            this.WData1LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SplitContainer3
            // 
            this.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer3.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer3.Name = "SplitContainer3";
            this.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer3.Panel1
            // 
            this.SplitContainer3.Panel1.Controls.Add(this.WData2LV);
            this.SplitContainer3.Panel1.Controls.Add(this.WData2PN);
            // 
            // SplitContainer3.Panel2
            // 
            this.SplitContainer3.Panel2.Controls.Add(this.WData3LV);
            this.SplitContainer3.Panel2.Controls.Add(this.WData3PN);
            this.SplitContainer3.Size = new System.Drawing.Size(973, 398);
            this.SplitContainer3.SplitterDistance = 197;
            this.SplitContainer3.TabIndex = 0;
            // 
            // WData2LV
            // 
            this.WData2LV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WData2LV.FullRowSelect = true;
            this.WData2LV.GridLines = true;
            this.WData2LV.HideSelection = false;
            this.WData2LV.Location = new System.Drawing.Point(0, 25);
            this.WData2LV.Name = "WData2LV";
            this.WData2LV.Size = new System.Drawing.Size(973, 172);
            this.WData2LV.SmallImageList = this.MainImageList;
            this.WData2LV.StateImageList = this.MainImageList;
            this.WData2LV.TabIndex = 1;
            this.WData2LV.UseCompatibleStateImageBehavior = false;
            this.WData2LV.View = System.Windows.Forms.View.Details;
            this.WData2LV.DoubleClick += new System.EventHandler(this.WData2LV_DoubleClick);
            // 
            // WData2PN
            // 
            this.WData2PN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WData2PN.BackgroundImage")));
            this.WData2PN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WData2PN.Controls.Add(this.WData2LB);
            this.WData2PN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WData2PN.Location = new System.Drawing.Point(0, 0);
            this.WData2PN.Name = "WData2PN";
            this.WData2PN.Size = new System.Drawing.Size(973, 25);
            this.WData2PN.TabIndex = 0;
            // 
            // WData2LB
            // 
            this.WData2LB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WData2LB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.WData2LB.Location = new System.Drawing.Point(0, 0);
            this.WData2LB.Name = "WData2LB";
            this.WData2LB.Size = new System.Drawing.Size(127, 25);
            this.WData2LB.TabIndex = 0;
            this.WData2LB.Text = "  수위 데이터 (cm)";
            this.WData2LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WData3LV
            // 
            this.WData3LV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WData3LV.FullRowSelect = true;
            this.WData3LV.GridLines = true;
            this.WData3LV.HideSelection = false;
            this.WData3LV.Location = new System.Drawing.Point(0, 25);
            this.WData3LV.Name = "WData3LV";
            this.WData3LV.Size = new System.Drawing.Size(973, 172);
            this.WData3LV.SmallImageList = this.MainImageList;
            this.WData3LV.StateImageList = this.MainImageList;
            this.WData3LV.TabIndex = 1;
            this.WData3LV.UseCompatibleStateImageBehavior = false;
            this.WData3LV.View = System.Windows.Forms.View.Details;
            this.WData3LV.DoubleClick += new System.EventHandler(this.WData3LV_DoubleClick);
            // 
            // WData3PN
            // 
            this.WData3PN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WData3PN.BackgroundImage")));
            this.WData3PN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WData3PN.Controls.Add(this.WData3LB);
            this.WData3PN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WData3PN.Location = new System.Drawing.Point(0, 0);
            this.WData3PN.Name = "WData3PN";
            this.WData3PN.Size = new System.Drawing.Size(973, 25);
            this.WData3PN.TabIndex = 0;
            // 
            // WData3LB
            // 
            this.WData3LB.Dock = System.Windows.Forms.DockStyle.Left;
            this.WData3LB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.WData3LB.Location = new System.Drawing.Point(0, 0);
            this.WData3LB.Name = "WData3LB";
            this.WData3LB.Size = new System.Drawing.Size(144, 25);
            this.WData3LB.TabIndex = 0;
            this.WData3LB.Text = "  유속 데이터 (m/s)";
            this.WData3LB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeviceIndexPN
            // 
            this.DeviceIndexPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DeviceIndexPN.BackgroundImage")));
            this.DeviceIndexPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DeviceIndexPN.Controls.Add(this.DeviceIndexLB);
            this.DeviceIndexPN.Dock = System.Windows.Forms.DockStyle.Top;
            this.DeviceIndexPN.Location = new System.Drawing.Point(0, 0);
            this.DeviceIndexPN.Name = "DeviceIndexPN";
            this.DeviceIndexPN.Size = new System.Drawing.Size(973, 25);
            this.DeviceIndexPN.TabIndex = 3;
            // 
            // DeviceIndexLB
            // 
            this.DeviceIndexLB.Dock = System.Windows.Forms.DockStyle.Left;
            this.DeviceIndexLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DeviceIndexLB.Location = new System.Drawing.Point(0, 0);
            this.DeviceIndexLB.Name = "DeviceIndexLB";
            this.DeviceIndexLB.Size = new System.Drawing.Size(144, 25);
            this.DeviceIndexLB.TabIndex = 0;
            this.DeviceIndexLB.Text = "  선택 측기";
            this.DeviceIndexLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeviceIndexLV
            // 
            this.DeviceIndexLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceIndexLV.LargeImageList = this.SubImageList;
            this.DeviceIndexLV.Location = new System.Drawing.Point(0, 28);
            this.DeviceIndexLV.Name = "DeviceIndexLV";
            this.DeviceIndexLV.Size = new System.Drawing.Size(889, 49);
            this.DeviceIndexLV.SmallImageList = this.SubImageList;
            this.DeviceIndexLV.TabIndex = 2;
            this.DeviceIndexLV.UseCompatibleStateImageBehavior = false;
            this.DeviceIndexLV.View = System.Windows.Forms.View.List;
            this.DeviceIndexLV.DoubleClick += new System.EventHandler(this.DeviceIndexLV_DoubleClick);
            // 
            // SubImageList
            // 
            this.SubImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.SubImageList.ImageSize = new System.Drawing.Size(22, 22);
            this.SubImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ControlBtn
            // 
            this.ControlBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlBtn.Location = new System.Drawing.Point(895, 54);
            this.ControlBtn.Name = "ControlBtn";
            this.ControlBtn.Size = new System.Drawing.Size(75, 23);
            this.ControlBtn.TabIndex = 1;
            this.ControlBtn.Text = "제어";
            this.ControlBtn.UseVisualStyleBackColor = true;
            this.ControlBtn.Click += new System.EventHandler(this.ControlBtn_Click);
            // 
            // RequestBtn
            // 
            this.RequestBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestBtn.Location = new System.Drawing.Point(895, 28);
            this.RequestBtn.Name = "RequestBtn";
            this.RequestBtn.Size = new System.Drawing.Size(75, 23);
            this.RequestBtn.TabIndex = 0;
            this.RequestBtn.Text = "상태 요청";
            this.RequestBtn.UseVisualStyleBackColor = true;
            this.RequestBtn.Click += new System.EventHandler(this.RequestBtn_Click);
            // 
            // WeatherCtrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 688);
            this.Controls.Add(this.MainPN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WeatherCtrForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "우량기 제어";
            this.MainPN.ResumeLayout(false);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.ResumeLayout(false);
            this.WData1PN.ResumeLayout(false);
            this.SplitContainer3.Panel1.ResumeLayout(false);
            this.SplitContainer3.Panel2.ResumeLayout(false);
            this.SplitContainer3.ResumeLayout(false);
            this.WData2PN.ResumeLayout(false);
            this.WData3PN.ResumeLayout(false);
            this.DeviceIndexPN.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPN;
        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.Panel WData1PN;
        private System.Windows.Forms.Panel WData2PN;
        private System.Windows.Forms.Label WData1LB;
        private System.Windows.Forms.Label WData2LB;
        private System.Windows.Forms.ListView WData1LV;
        private System.Windows.Forms.ListView WData2LV;
        private System.Windows.Forms.ImageList MainImageList;
        private System.Windows.Forms.SplitContainer SplitContainer3;
        private System.Windows.Forms.Panel WData3PN;
        private System.Windows.Forms.ListView WData3LV;
        private System.Windows.Forms.Label WData3LB;
        private System.Windows.Forms.Button ControlBtn;
        private System.Windows.Forms.Button RequestBtn;
        private System.Windows.Forms.ListView DeviceIndexLV;
        private System.Windows.Forms.ImageList SubImageList;
        private System.Windows.Forms.Panel DeviceIndexPN;
        private System.Windows.Forms.Label DeviceIndexLB;
    }
}

