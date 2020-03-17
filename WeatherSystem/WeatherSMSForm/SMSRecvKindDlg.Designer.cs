namespace ADEng.Module.WeatherSystem
{
    partial class SMSRecvKindDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMSRecvKindDlg));
            this.AlarmGB = new System.Windows.Forms.GroupBox();
            this.AlarmAllCB = new System.Windows.Forms.CheckBox();
            this.Alarm3CB = new System.Windows.Forms.CheckBox();
            this.Alarm2CB = new System.Windows.Forms.CheckBox();
            this.Alarm1CB = new System.Windows.Forms.CheckBox();
            this.Batt1Test = new System.Windows.Forms.CheckBox();
            this.Batt1Repair = new System.Windows.Forms.CheckBox();
            this.Batt1Reset = new System.Windows.Forms.CheckBox();
            this.Batt1Volt = new System.Windows.Forms.CheckBox();
            this.Batt1Tempo = new System.Windows.Forms.CheckBox();
            this.Batt1GB = new System.Windows.Forms.GroupBox();
            this.batt1AllCB = new System.Windows.Forms.CheckBox();
            this.Batt2Test = new System.Windows.Forms.CheckBox();
            this.Batt2Repair = new System.Windows.Forms.CheckBox();
            this.Batt2Reset = new System.Windows.Forms.CheckBox();
            this.Batt2Volt = new System.Windows.Forms.CheckBox();
            this.Batt2Tempo = new System.Windows.Forms.CheckBox();
            this.Batt2GB = new System.Windows.Forms.GroupBox();
            this.batt2AllCB = new System.Windows.Forms.CheckBox();
            this.CdmaCB = new System.Windows.Forms.CheckBox();
            this.SensorCB = new System.Windows.Forms.CheckBox();
            this.FanCB = new System.Windows.Forms.CheckBox();
            this.EtcGB = new System.Windows.Forms.GroupBox();
            this.EtcAllCB = new System.Windows.Forms.CheckBox();
            this.OkBtn = new System.Windows.Forms.Button();
            this.SouthSidePN = new System.Windows.Forms.Panel();
            this.AlarmGB.SuspendLayout();
            this.Batt1GB.SuspendLayout();
            this.Batt2GB.SuspendLayout();
            this.EtcGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // AlarmGB
            // 
            this.AlarmGB.Controls.Add(this.AlarmAllCB);
            this.AlarmGB.Controls.Add(this.Alarm3CB);
            this.AlarmGB.Controls.Add(this.Alarm2CB);
            this.AlarmGB.Controls.Add(this.Alarm1CB);
            this.AlarmGB.Location = new System.Drawing.Point(22, 14);
            this.AlarmGB.Name = "AlarmGB";
            this.AlarmGB.Size = new System.Drawing.Size(270, 48);
            this.AlarmGB.TabIndex = 0;
            this.AlarmGB.TabStop = false;
            this.AlarmGB.Text = "임계치 알람";
            // 
            // AlarmAllCB
            // 
            this.AlarmAllCB.AutoSize = true;
            this.AlarmAllCB.Location = new System.Drawing.Point(189, -1);
            this.AlarmAllCB.Name = "AlarmAllCB";
            this.AlarmAllCB.Size = new System.Drawing.Size(88, 16);
            this.AlarmAllCB.TabIndex = 3;
            this.AlarmAllCB.Text = "임계치 전체";
            this.AlarmAllCB.UseVisualStyleBackColor = true;
            this.AlarmAllCB.CheckedChanged += new System.EventHandler(this.AlarmAllCB_CheckedChanged);
            // 
            // Alarm3CB
            // 
            this.Alarm3CB.AutoSize = true;
            this.Alarm3CB.Checked = true;
            this.Alarm3CB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Alarm3CB.Location = new System.Drawing.Point(186, 22);
            this.Alarm3CB.Name = "Alarm3CB";
            this.Alarm3CB.Size = new System.Drawing.Size(54, 16);
            this.Alarm3CB.TabIndex = 2;
            this.Alarm3CB.Text = "3단계";
            this.Alarm3CB.UseVisualStyleBackColor = true;
            this.Alarm3CB.CheckedChanged += new System.EventHandler(this.Alarm1CB_CheckedChanged);
            // 
            // Alarm2CB
            // 
            this.Alarm2CB.AutoSize = true;
            this.Alarm2CB.Checked = true;
            this.Alarm2CB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Alarm2CB.Location = new System.Drawing.Point(108, 22);
            this.Alarm2CB.Name = "Alarm2CB";
            this.Alarm2CB.Size = new System.Drawing.Size(54, 16);
            this.Alarm2CB.TabIndex = 1;
            this.Alarm2CB.Text = "2단계";
            this.Alarm2CB.UseVisualStyleBackColor = true;
            this.Alarm2CB.CheckedChanged += new System.EventHandler(this.Alarm1CB_CheckedChanged);
            // 
            // Alarm1CB
            // 
            this.Alarm1CB.AutoSize = true;
            this.Alarm1CB.Checked = true;
            this.Alarm1CB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Alarm1CB.Location = new System.Drawing.Point(30, 22);
            this.Alarm1CB.Name = "Alarm1CB";
            this.Alarm1CB.Size = new System.Drawing.Size(54, 16);
            this.Alarm1CB.TabIndex = 0;
            this.Alarm1CB.Text = "1단계";
            this.Alarm1CB.UseVisualStyleBackColor = true;
            this.Alarm1CB.CheckedChanged += new System.EventHandler(this.Alarm1CB_CheckedChanged);
            // 
            // Batt1Test
            // 
            this.Batt1Test.AutoSize = true;
            this.Batt1Test.Checked = true;
            this.Batt1Test.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt1Test.Location = new System.Drawing.Point(21, 50);
            this.Batt1Test.Name = "Batt1Test";
            this.Batt1Test.Size = new System.Drawing.Size(72, 16);
            this.Batt1Test.TabIndex = 0;
            this.Batt1Test.Text = "점검시기";
            this.Batt1Test.UseVisualStyleBackColor = true;
            this.Batt1Test.CheckedChanged += new System.EventHandler(this.Batt1Volt_CheckedChanged);
            // 
            // Batt1Repair
            // 
            this.Batt1Repair.AutoSize = true;
            this.Batt1Repair.Checked = true;
            this.Batt1Repair.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt1Repair.Location = new System.Drawing.Point(105, 50);
            this.Batt1Repair.Name = "Batt1Repair";
            this.Batt1Repair.Size = new System.Drawing.Size(72, 16);
            this.Batt1Repair.TabIndex = 1;
            this.Batt1Repair.Text = "교체시기";
            this.Batt1Repair.UseVisualStyleBackColor = true;
            this.Batt1Repair.CheckedChanged += new System.EventHandler(this.Batt1Volt_CheckedChanged);
            // 
            // Batt1Reset
            // 
            this.Batt1Reset.AutoSize = true;
            this.Batt1Reset.Checked = true;
            this.Batt1Reset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt1Reset.Location = new System.Drawing.Point(189, 25);
            this.Batt1Reset.Name = "Batt1Reset";
            this.Batt1Reset.Size = new System.Drawing.Size(60, 16);
            this.Batt1Reset.TabIndex = 2;
            this.Batt1Reset.Text = "초기화";
            this.Batt1Reset.UseVisualStyleBackColor = true;
            this.Batt1Reset.CheckedChanged += new System.EventHandler(this.Batt1Volt_CheckedChanged);
            // 
            // Batt1Volt
            // 
            this.Batt1Volt.AutoSize = true;
            this.Batt1Volt.Checked = true;
            this.Batt1Volt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt1Volt.Location = new System.Drawing.Point(21, 25);
            this.Batt1Volt.Name = "Batt1Volt";
            this.Batt1Volt.Size = new System.Drawing.Size(76, 16);
            this.Batt1Volt.TabIndex = 3;
            this.Batt1Volt.Text = "전압 이상";
            this.Batt1Volt.UseVisualStyleBackColor = true;
            this.Batt1Volt.CheckedChanged += new System.EventHandler(this.Batt1Volt_CheckedChanged);
            // 
            // Batt1Tempo
            // 
            this.Batt1Tempo.AutoSize = true;
            this.Batt1Tempo.Checked = true;
            this.Batt1Tempo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt1Tempo.Location = new System.Drawing.Point(105, 25);
            this.Batt1Tempo.Name = "Batt1Tempo";
            this.Batt1Tempo.Size = new System.Drawing.Size(76, 16);
            this.Batt1Tempo.TabIndex = 4;
            this.Batt1Tempo.Text = "온도 이상";
            this.Batt1Tempo.UseVisualStyleBackColor = true;
            this.Batt1Tempo.CheckedChanged += new System.EventHandler(this.Batt1Volt_CheckedChanged);
            // 
            // Batt1GB
            // 
            this.Batt1GB.Controls.Add(this.batt1AllCB);
            this.Batt1GB.Controls.Add(this.Batt1Tempo);
            this.Batt1GB.Controls.Add(this.Batt1Volt);
            this.Batt1GB.Controls.Add(this.Batt1Reset);
            this.Batt1GB.Controls.Add(this.Batt1Repair);
            this.Batt1GB.Controls.Add(this.Batt1Test);
            this.Batt1GB.Location = new System.Drawing.Point(22, 68);
            this.Batt1GB.Name = "Batt1GB";
            this.Batt1GB.Size = new System.Drawing.Size(270, 77);
            this.Batt1GB.TabIndex = 1;
            this.Batt1GB.TabStop = false;
            this.Batt1GB.Text = "배터리 1";
            // 
            // batt1AllCB
            // 
            this.batt1AllCB.AutoSize = true;
            this.batt1AllCB.Location = new System.Drawing.Point(179, -1);
            this.batt1AllCB.Name = "batt1AllCB";
            this.batt1AllCB.Size = new System.Drawing.Size(98, 16);
            this.batt1AllCB.TabIndex = 5;
            this.batt1AllCB.Text = "배터리 1 전체";
            this.batt1AllCB.UseVisualStyleBackColor = true;
            this.batt1AllCB.CheckedChanged += new System.EventHandler(this.batt1AllCB_CheckedChanged);
            // 
            // Batt2Test
            // 
            this.Batt2Test.AutoSize = true;
            this.Batt2Test.Checked = true;
            this.Batt2Test.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt2Test.Location = new System.Drawing.Point(21, 50);
            this.Batt2Test.Name = "Batt2Test";
            this.Batt2Test.Size = new System.Drawing.Size(72, 16);
            this.Batt2Test.TabIndex = 0;
            this.Batt2Test.Text = "점검시기";
            this.Batt2Test.UseVisualStyleBackColor = true;
            this.Batt2Test.CheckedChanged += new System.EventHandler(this.Batt2Volt_CheckedChanged);
            // 
            // Batt2Repair
            // 
            this.Batt2Repair.AutoSize = true;
            this.Batt2Repair.Checked = true;
            this.Batt2Repair.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt2Repair.Location = new System.Drawing.Point(105, 50);
            this.Batt2Repair.Name = "Batt2Repair";
            this.Batt2Repair.Size = new System.Drawing.Size(72, 16);
            this.Batt2Repair.TabIndex = 1;
            this.Batt2Repair.Text = "교체시기";
            this.Batt2Repair.UseVisualStyleBackColor = true;
            this.Batt2Repair.CheckedChanged += new System.EventHandler(this.Batt2Volt_CheckedChanged);
            // 
            // Batt2Reset
            // 
            this.Batt2Reset.AutoSize = true;
            this.Batt2Reset.Checked = true;
            this.Batt2Reset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt2Reset.Location = new System.Drawing.Point(189, 25);
            this.Batt2Reset.Name = "Batt2Reset";
            this.Batt2Reset.Size = new System.Drawing.Size(60, 16);
            this.Batt2Reset.TabIndex = 2;
            this.Batt2Reset.Text = "초기화";
            this.Batt2Reset.UseVisualStyleBackColor = true;
            this.Batt2Reset.CheckedChanged += new System.EventHandler(this.Batt2Volt_CheckedChanged);
            // 
            // Batt2Volt
            // 
            this.Batt2Volt.AutoSize = true;
            this.Batt2Volt.Checked = true;
            this.Batt2Volt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt2Volt.Location = new System.Drawing.Point(21, 25);
            this.Batt2Volt.Name = "Batt2Volt";
            this.Batt2Volt.Size = new System.Drawing.Size(76, 16);
            this.Batt2Volt.TabIndex = 3;
            this.Batt2Volt.Text = "전압 이상";
            this.Batt2Volt.UseVisualStyleBackColor = true;
            this.Batt2Volt.CheckedChanged += new System.EventHandler(this.Batt2Volt_CheckedChanged);
            // 
            // Batt2Tempo
            // 
            this.Batt2Tempo.AutoSize = true;
            this.Batt2Tempo.Checked = true;
            this.Batt2Tempo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Batt2Tempo.Location = new System.Drawing.Point(105, 25);
            this.Batt2Tempo.Name = "Batt2Tempo";
            this.Batt2Tempo.Size = new System.Drawing.Size(76, 16);
            this.Batt2Tempo.TabIndex = 4;
            this.Batt2Tempo.Text = "온도 이상";
            this.Batt2Tempo.UseVisualStyleBackColor = true;
            this.Batt2Tempo.CheckedChanged += new System.EventHandler(this.Batt2Volt_CheckedChanged);
            // 
            // Batt2GB
            // 
            this.Batt2GB.Controls.Add(this.batt2AllCB);
            this.Batt2GB.Controls.Add(this.Batt2Tempo);
            this.Batt2GB.Controls.Add(this.Batt2Volt);
            this.Batt2GB.Controls.Add(this.Batt2Reset);
            this.Batt2GB.Controls.Add(this.Batt2Repair);
            this.Batt2GB.Controls.Add(this.Batt2Test);
            this.Batt2GB.Location = new System.Drawing.Point(22, 151);
            this.Batt2GB.Name = "Batt2GB";
            this.Batt2GB.Size = new System.Drawing.Size(270, 77);
            this.Batt2GB.TabIndex = 2;
            this.Batt2GB.TabStop = false;
            this.Batt2GB.Text = "배터리 2";
            // 
            // batt2AllCB
            // 
            this.batt2AllCB.AutoSize = true;
            this.batt2AllCB.Location = new System.Drawing.Point(179, -1);
            this.batt2AllCB.Name = "batt2AllCB";
            this.batt2AllCB.Size = new System.Drawing.Size(98, 16);
            this.batt2AllCB.TabIndex = 5;
            this.batt2AllCB.Text = "배터리 2 전체";
            this.batt2AllCB.UseVisualStyleBackColor = true;
            this.batt2AllCB.CheckedChanged += new System.EventHandler(this.batt2AllCB_CheckedChanged);
            // 
            // CdmaCB
            // 
            this.CdmaCB.AutoSize = true;
            this.CdmaCB.Checked = true;
            this.CdmaCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CdmaCB.Location = new System.Drawing.Point(31, 45);
            this.CdmaCB.Name = "CdmaCB";
            this.CdmaCB.Size = new System.Drawing.Size(140, 16);
            this.CdmaCB.TabIndex = 0;
            this.CdmaCB.Text = "CDMA시간 설정 이상";
            this.CdmaCB.UseVisualStyleBackColor = true;
            this.CdmaCB.CheckedChanged += new System.EventHandler(this.SensorCB_CheckedChanged);
            // 
            // SensorCB
            // 
            this.SensorCB.AutoSize = true;
            this.SensorCB.Checked = true;
            this.SensorCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SensorCB.Location = new System.Drawing.Point(31, 23);
            this.SensorCB.Name = "SensorCB";
            this.SensorCB.Size = new System.Drawing.Size(76, 16);
            this.SensorCB.TabIndex = 3;
            this.SensorCB.Text = "센서 상태";
            this.SensorCB.UseVisualStyleBackColor = true;
            this.SensorCB.CheckedChanged += new System.EventHandler(this.SensorCB_CheckedChanged);
            // 
            // FanCB
            // 
            this.FanCB.AutoSize = true;
            this.FanCB.Checked = true;
            this.FanCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FanCB.Location = new System.Drawing.Point(163, 23);
            this.FanCB.Name = "FanCB";
            this.FanCB.Size = new System.Drawing.Size(76, 16);
            this.FanCB.TabIndex = 4;
            this.FanCB.Text = "FAN 이상";
            this.FanCB.UseVisualStyleBackColor = true;
            this.FanCB.CheckedChanged += new System.EventHandler(this.SensorCB_CheckedChanged);
            // 
            // EtcGB
            // 
            this.EtcGB.Controls.Add(this.EtcAllCB);
            this.EtcGB.Controls.Add(this.FanCB);
            this.EtcGB.Controls.Add(this.SensorCB);
            this.EtcGB.Controls.Add(this.CdmaCB);
            this.EtcGB.Location = new System.Drawing.Point(22, 234);
            this.EtcGB.Name = "EtcGB";
            this.EtcGB.Size = new System.Drawing.Size(270, 71);
            this.EtcGB.TabIndex = 3;
            this.EtcGB.TabStop = false;
            this.EtcGB.Text = "기타";
            // 
            // EtcAllCB
            // 
            this.EtcAllCB.AutoSize = true;
            this.EtcAllCB.Location = new System.Drawing.Point(201, -1);
            this.EtcAllCB.Name = "EtcAllCB";
            this.EtcAllCB.Size = new System.Drawing.Size(76, 16);
            this.EtcAllCB.TabIndex = 5;
            this.EtcAllCB.Text = "기타 전체";
            this.EtcAllCB.UseVisualStyleBackColor = true;
            this.EtcAllCB.CheckedChanged += new System.EventHandler(this.EtcAllCB_CheckedChanged);
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(217, 322);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 4;
            this.OkBtn.Text = "확인";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // SouthSidePN
            // 
            this.SouthSidePN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SouthSidePN.BackgroundImage")));
            this.SouthSidePN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SouthSidePN.Location = new System.Drawing.Point(2, 311);
            this.SouthSidePN.Name = "SouthSidePN";
            this.SouthSidePN.Size = new System.Drawing.Size(310, 5);
            this.SouthSidePN.TabIndex = 9;
            // 
            // SMSRecvKindDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 351);
            this.Controls.Add(this.SouthSidePN);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.EtcGB);
            this.Controls.Add(this.Batt2GB);
            this.Controls.Add(this.Batt1GB);
            this.Controls.Add(this.AlarmGB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SMSRecvKindDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMS 수신 항목 설정";
            this.AlarmGB.ResumeLayout(false);
            this.AlarmGB.PerformLayout();
            this.Batt1GB.ResumeLayout(false);
            this.Batt1GB.PerformLayout();
            this.Batt2GB.ResumeLayout(false);
            this.Batt2GB.PerformLayout();
            this.EtcGB.ResumeLayout(false);
            this.EtcGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox AlarmGB;
        private System.Windows.Forms.CheckBox Batt1Test;
        private System.Windows.Forms.CheckBox Batt1Repair;
        private System.Windows.Forms.CheckBox Batt1Reset;
        private System.Windows.Forms.CheckBox Batt1Volt;
        private System.Windows.Forms.CheckBox Batt1Tempo;
        private System.Windows.Forms.GroupBox Batt1GB;
        private System.Windows.Forms.CheckBox Batt2Test;
        private System.Windows.Forms.CheckBox Batt2Repair;
        private System.Windows.Forms.CheckBox Batt2Reset;
        private System.Windows.Forms.CheckBox Batt2Volt;
        private System.Windows.Forms.CheckBox Batt2Tempo;
        private System.Windows.Forms.GroupBox Batt2GB;
        private System.Windows.Forms.CheckBox CdmaCB;
        private System.Windows.Forms.CheckBox SensorCB;
        private System.Windows.Forms.CheckBox FanCB;
        private System.Windows.Forms.GroupBox EtcGB;
        private System.Windows.Forms.CheckBox Alarm3CB;
        private System.Windows.Forms.CheckBox Alarm2CB;
        private System.Windows.Forms.CheckBox Alarm1CB;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Panel SouthSidePN;
        private System.Windows.Forms.CheckBox AlarmAllCB;
        private System.Windows.Forms.CheckBox batt1AllCB;
        private System.Windows.Forms.CheckBox batt2AllCB;
        private System.Windows.Forms.CheckBox EtcAllCB;


    }
}