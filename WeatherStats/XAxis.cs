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
        /// 시간 Full Name
        /// </summary>
        public string DateFullName
        {
            get { return dateFullName; }
            set { dateFullName = value; }
        }

        /// <summary>
        ///  시간 출력 시간
        /// </summary>
        public string DateOutputFormat
        {
            get { return dateOutputFormat; }
            set { dateOutputFormat = value; }
        }

       
        /// <summary>
        /// 기본생성자
        /// </summary>
        public XAxis()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_number">
        /// 시간 Full Name
        /// </param>
        /// <param name="_text">
        /// 시간 출력 시간
        /// </param>
        public XAxis(string _dateFullName, string _dateOutputFormat)
        {
            this.dateFullName = _dateFullName;
            this.dateOutputFormat = _dateOutputFormat;
        }
    }
}
