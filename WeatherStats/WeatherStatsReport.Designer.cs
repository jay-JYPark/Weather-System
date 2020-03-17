namespace ADEng.Module.WeatherSystem
{
    /// <summary>
    /// Summary description for WeatherStatsReport.
    /// </summary>
    partial class WeatherStatsReport
    {
        private DataDynamics.ActiveReports.PageHeader pageHeader;
        private DataDynamics.ActiveReports.Detail detail;
        private DataDynamics.ActiveReports.PageFooter pageFooter;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        #region ActiveReport Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WeatherStatsReport));
            this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
            this.detail = new DataDynamics.ActiveReports.Detail();
            this.customChart = new DataDynamics.ActiveReports.CustomControl();
            this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
            this.reportHeader1 = new DataDynamics.ActiveReports.ReportHeader();
            this.txtTerm = new DataDynamics.ActiveReports.TextBox();
            this.txtTypeWeather = new DataDynamics.ActiveReports.TextBox();
            this.txtDeviceName = new DataDynamics.ActiveReports.TextBox();
            this.txtTitle = new DataDynamics.ActiveReports.TextBox();
            this.txtUnit = new DataDynamics.ActiveReports.TextBox();
            this.label1 = new DataDynamics.ActiveReports.Label();
            this.label3 = new DataDynamics.ActiveReports.Label();
            this.label4 = new DataDynamics.ActiveReports.Label();
            this.label5 = new DataDynamics.ActiveReports.Label();
            this.reportFooter1 = new DataDynamics.ActiveReports.ReportFooter();
            ((System.ComponentModel.ISupportInitialize)(this.customChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTypeWeather)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviceName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Height = 0.2083333F;
            this.pageHeader.Name = "pageHeader";
            // 
            // detail
            // 
            this.detail.ColumnSpacing = 0F;
            this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.customChart});
            this.detail.Height = 3.604167F;
            this.detail.Name = "detail";
            // 
            // customChart
            // 
            this.customChart.Border.BottomColor = System.Drawing.Color.Black;
            this.customChart.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.customChart.Border.LeftColor = System.Drawing.Color.Black;
            this.customChart.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.customChart.Border.RightColor = System.Drawing.Color.Black;
            this.customChart.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.customChart.Border.TopColor = System.Drawing.Color.Black;
            this.customChart.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.customChart.Height = 3.267717F;
            this.customChart.Left = 0F;
            this.customChart.Name = "customChart";
            this.customChart.Top = 0.07874016F;
            this.customChart.Type = typeof(ChartFX.WinForms.Chart);
            this.customChart.Width = 10.07874F;
            // 
            // pageFooter
            // 
            this.pageFooter.Height = 0.25F;
            this.pageFooter.Name = "pageFooter";
            // 
            // reportHeader1
            // 
            this.reportHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
            this.txtTerm,
            this.txtTypeWeather,
            this.txtDeviceName,
            this.txtTitle,
            this.txtUnit,
            this.label1,
            this.label3,
            this.label4,
            this.label5});
            this.reportHeader1.Height = 1.556693F;
            this.reportHeader1.Name = "reportHeader1";
            // 
            // txtTerm
            // 
            this.txtTerm.Border.BottomColor = System.Drawing.Color.Black;
            this.txtTerm.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTerm.Border.LeftColor = System.Drawing.Color.Black;
            this.txtTerm.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTerm.Border.RightColor = System.Drawing.Color.Black;
            this.txtTerm.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTerm.Border.TopColor = System.Drawing.Color.Black;
            this.txtTerm.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTerm.Height = 0.1968504F;
            this.txtTerm.Left = 0.8267717F;
            this.txtTerm.Name = "txtTerm";
            this.txtTerm.Style = "vertical-align: middle; ";
            this.txtTerm.Text = "date";
            this.txtTerm.Top = 0.6692913F;
            this.txtTerm.Width = 3.267717F;
            // 
            // txtTypeWeather
            // 
            this.txtTypeWeather.Border.BottomColor = System.Drawing.Color.Black;
            this.txtTypeWeather.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTypeWeather.Border.LeftColor = System.Drawing.Color.Black;
            this.txtTypeWeather.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTypeWeather.Border.RightColor = System.Drawing.Color.Black;
            this.txtTypeWeather.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTypeWeather.Border.TopColor = System.Drawing.Color.Black;
            this.txtTypeWeather.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTypeWeather.Height = 0.1968504F;
            this.txtTypeWeather.Left = 0.8267717F;
            this.txtTypeWeather.Name = "txtTypeWeather";
            this.txtTypeWeather.Style = "vertical-align: middle; ";
            this.txtTypeWeather.Text = "typeWeather";
            this.txtTypeWeather.Top = 1.259843F;
            this.txtTypeWeather.Width = 1.102362F;
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Border.BottomColor = System.Drawing.Color.Black;
            this.txtDeviceName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtDeviceName.Border.LeftColor = System.Drawing.Color.Black;
            this.txtDeviceName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtDeviceName.Border.RightColor = System.Drawing.Color.Black;
            this.txtDeviceName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtDeviceName.Border.TopColor = System.Drawing.Color.Black;
            this.txtDeviceName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtDeviceName.Height = 0.1968504F;
            this.txtDeviceName.Left = 0.8267717F;
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Style = "vertical-align: middle; ";
            this.txtDeviceName.Text = "deviceName";
            this.txtDeviceName.Top = 1.062992F;
            this.txtDeviceName.Width = 1.338583F;
            // 
            // txtTitle
            // 
            this.txtTitle.Border.BottomColor = System.Drawing.Color.Black;
            this.txtTitle.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTitle.Border.LeftColor = System.Drawing.Color.Black;
            this.txtTitle.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTitle.Border.RightColor = System.Drawing.Color.Black;
            this.txtTitle.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTitle.Border.TopColor = System.Drawing.Color.Black;
            this.txtTitle.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtTitle.Height = 0.3149606F;
            this.txtTitle.Left = 0F;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Style = "text-align: center; font-weight: bold; font-size: 20pt; vertical-align: middle; ";
            this.txtTitle.Text = "title";
            this.txtTitle.Top = 0F;
            this.txtTitle.Width = 10.23622F;
            // 
            // txtUnit
            // 
            this.txtUnit.Border.BottomColor = System.Drawing.Color.Black;
            this.txtUnit.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtUnit.Border.LeftColor = System.Drawing.Color.Black;
            this.txtUnit.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtUnit.Border.RightColor = System.Drawing.Color.Black;
            this.txtUnit.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtUnit.Border.TopColor = System.Drawing.Color.Black;
            this.txtUnit.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.txtUnit.Height = 0.1968504F;
            this.txtUnit.Left = 0.8267717F;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Style = "vertical-align: middle; ";
            this.txtUnit.Text = "unit";
            this.txtUnit.Top = 0.8661418F;
            this.txtUnit.Width = 1.102362F;
            // 
            // label1
            // 
            this.label1.Border.BottomColor = System.Drawing.Color.Black;
            this.label1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label1.Border.LeftColor = System.Drawing.Color.Black;
            this.label1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label1.Border.RightColor = System.Drawing.Color.Black;
            this.label1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label1.Border.TopColor = System.Drawing.Color.Black;
            this.label1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label1.Height = 0.1968504F;
            this.label1.HyperLink = null;
            this.label1.Left = 0F;
            this.label1.Name = "label1";
            this.label1.Style = "ddo-char-set: 129; font-weight: bold; font-size: 9.75pt; font-family: ±¼¸²; ";
            this.label1.Text = "Á¶È¸ÀÏÀÚ: ";
            this.label1.Top = 0.6692914F;
            this.label1.Width = 0.6692913F;
            // 
            // label3
            // 
            this.label3.Border.BottomColor = System.Drawing.Color.Black;
            this.label3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label3.Border.LeftColor = System.Drawing.Color.Black;
            this.label3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label3.Border.RightColor = System.Drawing.Color.Black;
            this.label3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label3.Border.TopColor = System.Drawing.Color.Black;
            this.label3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label3.Height = 0.1968504F;
            this.label3.HyperLink = null;
            this.label3.Left = 0F;
            this.label3.Name = "label3";
            this.label3.Style = "ddo-char-set: 129; font-weight: bold; font-size: 9.75pt; font-family: ±¼¸²; ";
            this.label3.Text = "Ãø ±â ¸í : ";
            this.label3.Top = 1.062992F;
            this.label3.Width = 0.6692913F;
            // 
            // label4
            // 
            this.label4.Border.BottomColor = System.Drawing.Color.Black;
            this.label4.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label4.Border.LeftColor = System.Drawing.Color.Black;
            this.label4.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label4.Border.RightColor = System.Drawing.Color.Black;
            this.label4.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label4.Border.TopColor = System.Drawing.Color.Black;
            this.label4.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label4.Height = 0.1968504F;
            this.label4.HyperLink = null;
            this.label4.Left = 0F;
            this.label4.Name = "label4";
            this.label4.Style = "ddo-char-set: 129; font-weight: bold; font-size: 9.75pt; font-family: ±¼¸²; ";
            this.label4.Text = "°üÃøÁ¾·ù: ";
            this.label4.Top = 1.259843F;
            this.label4.Width = 0.6692914F;
            // 
            // label5
            // 
            this.label5.Border.BottomColor = System.Drawing.Color.Black;
            this.label5.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label5.Border.LeftColor = System.Drawing.Color.Black;
            this.label5.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label5.Border.RightColor = System.Drawing.Color.Black;
            this.label5.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label5.Border.TopColor = System.Drawing.Color.Black;
            this.label5.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
            this.label5.Height = 0.1968504F;
            this.label5.HyperLink = null;
            this.label5.Left = 0F;
            this.label5.Name = "label5";
            this.label5.Style = "ddo-char-set: 129; font-weight: bold; font-size: 9.75pt; font-family: ±¼¸²; ";
            this.label5.Text = "´Ü     À§ : ";
            this.label5.Top = 0.8661418F;
            this.label5.Width = 0.6692913F;
            // 
            // reportFooter1
            // 
            this.reportFooter1.Height = 0.25F;
            this.reportFooter1.Name = "reportFooter1";
            // 
            // WeatherStatsReport
            // 
            this.MasterReport = false;
            this.PageSettings.Margins.Bottom = 0.3937008F;
            this.PageSettings.Margins.Left = 0.7086614F;
            this.PageSettings.Margins.Right = 0.7086614F;
            this.PageSettings.Margins.Top = 0.5905512F;
            this.PageSettings.Orientation = DataDynamics.ActiveReports.Document.PageOrientation.Landscape;
            this.PageSettings.PaperHeight = 11.69F;
            this.PageSettings.PaperWidth = 8.27F;
            this.PrintWidth = 10.22917F;
            this.Sections.Add(this.reportHeader1);
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.pageFooter);
            this.Sections.Add(this.reportFooter1);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
                        "l; font-size: 10pt; color: Black; ", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
                        "lic; ", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"));
            ((System.ComponentModel.ISupportInitialize)(this.customChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTypeWeather)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviceName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private DataDynamics.ActiveReports.ReportHeader reportHeader1;
        private DataDynamics.ActiveReports.ReportFooter reportFooter1;
        private DataDynamics.ActiveReports.TextBox txtTerm;
        private DataDynamics.ActiveReports.TextBox txtTypeWeather;
        private DataDynamics.ActiveReports.TextBox txtDeviceName;
        private DataDynamics.ActiveReports.TextBox txtTitle;
        private DataDynamics.ActiveReports.CustomControl customChart;
        private DataDynamics.ActiveReports.TextBox txtUnit;
        private DataDynamics.ActiveReports.Label label1;
        private DataDynamics.ActiveReports.Label label3;
        private DataDynamics.ActiveReports.Label label4;
        private DataDynamics.ActiveReports.Label label5;
    }
}
