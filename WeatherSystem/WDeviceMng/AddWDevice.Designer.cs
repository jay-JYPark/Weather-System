namespace ADEng.Module.WeatherSystem
{
    partial class AddWDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddWDevice));
            this.WDeviceMainLB = new System.Windows.Forms.Label();
            this.WDeviceMainPN = new System.Windows.Forms.Panel();
            this.WDeviceMainPB = new System.Windows.Forms.PictureBox();
            this.WDeviceDivisionLB = new System.Windows.Forms.Label();
            this.WDeviceIDLB = new System.Windows.Forms.Label();
            this.WDeviceNameLB = new System.Windows.Forms.Label();
            this.WDeviceTelNumLB = new System.Windows.Forms.Label();
            this.WDeviceIDTB = new System.Windows.Forms.TextBox();
            this.WDeviceNameTB = new System.Windows.Forms.TextBox();
            this.WDeviceTelNumTB = new System.Windows.Forms.TextBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CancleBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.WDeviceSidePN = new System.Windows.Forms.Panel();
            this.WDeviceUnderPN = new System.Windows.Forms.Panel();
            this.WDeviceDivisionCB = new System.Windows.Forms.ComboBox();
            this.SensorKindLV = new System.Windows.Forms.ListView();
            this.SensorKindLB = new System.Windows.Forms.Label();
            this.WDeviceRemarkTB = new System.Windows.Forms.TextBox();
            this.WDeviceRemarkLB = new System.Windows.Forms.Label();
            this.EternetUseLB = new System.Windows.Forms.Label();
            this.EternetUseCB = new System.Windows.Forms.CheckBox();
            this.WDeviceMainPN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WDeviceMainPB)).BeginInit();
            this.SuspendLayout();
            // 
            // WDeviceMainLB
            // 
            this.WDeviceMainLB.AutoSize = true;
            this.WDeviceMainLB.BackColor = System.Drawing.Color.Transparent;
            this.WDeviceMainLB.Location = new System.Drawing.Point(12, 9);
            this.WDeviceMainLB.Name = "WDeviceMainLB";
            this.WDeviceMainLB.Size = new System.Drawing.Size(0, 12);
            this.WDeviceMainLB.TabIndex = 0;
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
            this.WDeviceMainPN.Size = new System.Drawing.Size(281, 40);
            this.WDeviceMainPN.TabIndex = 3;
            // 
            // WDeviceMainPB
            // 
            this.WDeviceMainPB.BackColor = System.Drawing.Color.Transparent;
            this.WDeviceMainPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceMainPB.Location = new System.Drawing.Point(231, 3);
            this.WDeviceMainPB.Name = "WDeviceMainPB";
            this.WDeviceMainPB.Size = new System.Drawing.Size(39, 35);
            this.WDeviceMainPB.TabIndex = 1;
            this.WDeviceMainPB.TabStop = false;
            // 
            // WDeviceDivisionLB
            // 
            this.WDeviceDivisionLB.Location = new System.Drawing.Point(19, 60);
            this.WDeviceDivisionLB.Name = "WDeviceDivisionLB";
            this.WDeviceDivisionLB.Size = new System.Drawing.Size(72, 21);
            this.WDeviceDivisionLB.TabIndex = 4;
            this.WDeviceDivisionLB.Text = "식별자 :";
            this.WDeviceDivisionLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WDeviceIDLB
            // 
            this.WDeviceIDLB.Location = new System.Drawing.Point(19, 85);
            this.WDeviceIDLB.Name = "WDeviceIDLB";
            this.WDeviceIDLB.Size = new System.Drawing.Size(72, 21);
            this.WDeviceIDLB.TabIndex = 6;
            this.WDeviceIDLB.Text = "ID :";
            this.WDeviceIDLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WDeviceNameLB
            // 
            this.WDeviceNameLB.Location = new System.Drawing.Point(19, 111);
            this.WDeviceNameLB.Name = "WDeviceNameLB";
            this.WDeviceNameLB.Size = new System.Drawing.Size(72, 21);
            this.WDeviceNameLB.TabIndex = 7;
            this.WDeviceNameLB.Text = "이름 :";
            this.WDeviceNameLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WDeviceTelNumLB
            // 
            this.WDeviceTelNumLB.Location = new System.Drawing.Point(19, 137);
            this.WDeviceTelNumLB.Name = "WDeviceTelNumLB";
            this.WDeviceTelNumLB.Size = new System.Drawing.Size(72, 21);
            this.WDeviceTelNumLB.TabIndex = 8;
            this.WDeviceTelNumLB.Text = "전화번호 :";
            this.WDeviceTelNumLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WDeviceIDTB
            // 
            this.WDeviceIDTB.Location = new System.Drawing.Point(97, 85);
            this.WDeviceIDTB.MaxLength = 15;
            this.WDeviceIDTB.Name = "WDeviceIDTB";
            this.WDeviceIDTB.Size = new System.Drawing.Size(165, 21);
            this.WDeviceIDTB.TabIndex = 2;
            this.WDeviceIDTB.TextChanged += new System.EventHandler(this.WDeviceDivisionCB_SelectedIndexChanged);
            // 
            // WDeviceNameTB
            // 
            this.WDeviceNameTB.Location = new System.Drawing.Point(97, 111);
            this.WDeviceNameTB.MaxLength = 10;
            this.WDeviceNameTB.Name = "WDeviceNameTB";
            this.WDeviceNameTB.Size = new System.Drawing.Size(165, 21);
            this.WDeviceNameTB.TabIndex = 3;
            this.WDeviceNameTB.TextChanged += new System.EventHandler(this.WDeviceDivisionCB_SelectedIndexChanged);
            // 
            // WDeviceTelNumTB
            // 
            this.WDeviceTelNumTB.Location = new System.Drawing.Point(97, 137);
            this.WDeviceTelNumTB.MaxLength = 13;
            this.WDeviceTelNumTB.Name = "WDeviceTelNumTB";
            this.WDeviceTelNumTB.Size = new System.Drawing.Size(165, 21);
            this.WDeviceTelNumTB.TabIndex = 4;
            this.WDeviceTelNumTB.TextChanged += new System.EventHandler(this.WDeviceDivisionCB_SelectedIndexChanged);
            this.WDeviceTelNumTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WDeviceTelNumTB_KeyDown);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Enabled = false;
            this.SaveBtn.Location = new System.Drawing.Point(184, 277);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 7;
            this.SaveBtn.Text = "적용";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancleBtn
            // 
            this.CancleBtn.Location = new System.Drawing.Point(103, 277);
            this.CancleBtn.Name = "CancleBtn";
            this.CancleBtn.Size = new System.Drawing.Size(75, 23);
            this.CancleBtn.TabIndex = 6;
            this.CancleBtn.Text = "취소";
            this.CancleBtn.UseVisualStyleBackColor = true;
            this.CancleBtn.Click += new System.EventHandler(this.CancleBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(22, 277);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 5;
            this.OkBtn.Text = "확인";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // WDeviceSidePN
            // 
            this.WDeviceSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceSidePN.BackgroundImage")));
            this.WDeviceSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceSidePN.Dock = System.Windows.Forms.DockStyle.Top;
            this.WDeviceSidePN.Location = new System.Drawing.Point(0, 40);
            this.WDeviceSidePN.Name = "WDeviceSidePN";
            this.WDeviceSidePN.Size = new System.Drawing.Size(281, 5);
            this.WDeviceSidePN.TabIndex = 15;
            // 
            // WDeviceUnderPN
            // 
            this.WDeviceUnderPN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WDeviceUnderPN.BackgroundImage")));
            this.WDeviceUnderPN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WDeviceUnderPN.Location = new System.Drawing.Point(12, 269);
            this.WDeviceUnderPN.Name = "WDeviceUnderPN";
            this.WDeviceUnderPN.Size = new System.Drawing.Size(256, 2);
            this.WDeviceUnderPN.TabIndex = 16;
            // 
            // WDeviceDivisionCB
            // 
            this.WDeviceDivisionCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WDeviceDivisionCB.FormattingEnabled = true;
            this.WDeviceDivisionCB.Location = new System.Drawing.Point(97, 60);
            this.WDeviceDivisionCB.Name = "WDeviceDivisionCB";
            this.WDeviceDivisionCB.Size = new System.Drawing.Size(165, 20);
            this.WDeviceDivisionCB.TabIndex = 1;
            this.WDeviceDivisionCB.SelectedIndexChanged += new System.EventHandler(this.WDeviceDivisionCB_SelectedIndexChanged);
            // 
            // SensorKindLV
            // 
            this.SensorKindLV.CheckBoxes = true;
            this.SensorKindLV.Location = new System.Drawing.Point(97, 163);
            this.SensorKindLV.Name = "SensorKindLV";
            this.SensorKindLV.Size = new System.Drawing.Size(165, 46);
            this.SensorKindLV.TabIndex = 17;
            this.SensorKindLV.UseCompatibleStateImageBehavior = false;
            this.SensorKindLV.View = System.Windows.Forms.View.SmallIcon;
            this.SensorKindLV.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.SensorKindLV_ItemChecked);
            // 
            // SensorKindLB
            // 
            this.SensorKindLB.Location = new System.Drawing.Point(19, 164);
            this.SensorKindLB.Name = "SensorKindLB";
            this.SensorKindLB.Size = new System.Drawing.Size(72, 21);
            this.SensorKindLB.TabIndex = 18;
            this.SensorKindLB.Text = "센서 :";
            this.SensorKindLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WDeviceRemarkTB
            // 
            this.WDeviceRemarkTB.Location = new System.Drawing.Point(97, 238);
            this.WDeviceRemarkTB.MaxLength = 20;
            this.WDeviceRemarkTB.Name = "WDeviceRemarkTB";
            this.WDeviceRemarkTB.Size = new System.Drawing.Size(165, 21);
            this.WDeviceRemarkTB.TabIndex = 19;
            // 
            // WDeviceRemarkLB
            // 
            this.WDeviceRemarkLB.Location = new System.Drawing.Point(19, 239);
            this.WDeviceRemarkLB.Name = "WDeviceRemarkLB";
            this.WDeviceRemarkLB.Size = new System.Drawing.Size(72, 21);
            this.WDeviceRemarkLB.TabIndex = 20;
            this.WDeviceRemarkLB.Text = "Remark :";
            this.WDeviceRemarkLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EternetUseLB
            // 
            this.EternetUseLB.Location = new System.Drawing.Point(19, 214);
            this.EternetUseLB.Name = "EternetUseLB";
            this.EternetUseLB.Size = new System.Drawing.Size(72, 21);
            this.EternetUseLB.TabIndex = 21;
            this.EternetUseLB.Text = "이더넷 :";
            this.EternetUseLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EternetUseCB
            // 
            this.EternetUseCB.AutoSize = true;
            this.EternetUseCB.Location = new System.Drawing.Point(97, 216);
            this.EternetUseCB.Name = "EternetUseCB";
            this.EternetUseCB.Size = new System.Drawing.Size(48, 16);
            this.EternetUseCB.TabIndex = 22;
            this.EternetUseCB.Text = "사용";
            this.EternetUseCB.UseVisualStyleBackColor = true;
            this.EternetUseCB.CheckedChanged += new System.EventHandler(this.EternetUseCB_CheckedChanged);
            // 
            // AddWDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 304);
            this.Controls.Add(this.EternetUseCB);
            this.Controls.Add(this.EternetUseLB);
            this.Controls.Add(this.WDeviceRemarkLB);
            this.Controls.Add(this.WDeviceRemarkTB);
            this.Controls.Add(this.SensorKindLB);
            this.Controls.Add(this.SensorKindLV);
            this.Controls.Add(this.WDeviceDivisionCB);
            this.Controls.Add(this.WDeviceUnderPN);
            this.Controls.Add(this.WDeviceSidePN);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.CancleBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.WDeviceTelNumTB);
            this.Controls.Add(this.WDeviceNameTB);
            this.Controls.Add(this.WDeviceIDTB);
            this.Controls.Add(this.WDeviceTelNumLB);
            this.Controls.Add(this.WDeviceNameLB);
            this.Controls.Add(this.WDeviceIDLB);
            this.Controls.Add(this.WDeviceDivisionLB);
            this.Controls.Add(this.WDeviceMainPN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddWDevice";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WDeviceMainPN.ResumeLayout(false);
            this.WDeviceMainPN.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WDeviceMainPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WDeviceMainLB;
        private System.Windows.Forms.Panel WDeviceMainPN;
        private System.Windows.Forms.PictureBox WDeviceMainPB;
        private System.Windows.Forms.Label WDeviceDivisionLB;
        private System.Windows.Forms.Label WDeviceIDLB;
        private System.Windows.Forms.Label WDeviceNameLB;
        private System.Windows.Forms.Label WDeviceTelNumLB;
        private System.Windows.Forms.TextBox WDeviceIDTB;
        private System.Windows.Forms.TextBox WDeviceNameTB;
        private System.Windows.Forms.TextBox WDeviceTelNumTB;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CancleBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Panel WDeviceSidePN;
        private System.Windows.Forms.Panel WDeviceUnderPN;
        private System.Windows.Forms.ComboBox WDeviceDivisionCB;
        private System.Windows.Forms.ListView SensorKindLV;
        private System.Windows.Forms.Label SensorKindLB;
        private System.Windows.Forms.TextBox WDeviceRemarkTB;
        private System.Windows.Forms.Label WDeviceRemarkLB;
        private System.Windows.Forms.Label EternetUseLB;
        private System.Windows.Forms.CheckBox EternetUseCB;

    }
}