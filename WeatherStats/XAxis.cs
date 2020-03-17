using System;
using System.Collections.Generic;
using System.Text;

namespace ADEng.Module.WeatherSystem
{
    public class XAxis
    {
        private string dateFullName = string.Empty;
        private string dateOutputFormat = string.Empty;

        /// <summary>
        /// �ð� Full Name
        /// </summary>
        public string DateFullName
        {
            get { return dateFullName; }
            set { dateFullName = value; }
        }

        /// <summary>
        ///  �ð� ��� �ð�
        /// </summary>
        public string DateOutputFormat
        {
            get { return dateOutputFormat; }
            set { dateOutputFormat = value; }
        }

       
        /// <summary>
        /// �⺻������
        /// </summary>
        public XAxis()
        {
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="_number">
        /// �ð� Full Name
        /// </param>
        /// <param name="_text">
        /// �ð� ��� �ð�
        /// </param>
        public XAxis(string _dateFullName, string _dateOutputFormat)
        {
            this.dateFullName = _dateFullName;
            this.dateOutputFormat = _dateOutputFormat;
        }
    }
}
