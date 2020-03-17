using System;
using System.Collections.Generic;
using System.Text;

namespace ADEng.Module.WeatherSystem
{
    public class Time
    {
        private double number = double.MinValue;
        private string text = string.Empty;

        /// <summary>
        /// 시간(숫자)
        /// </summary>
        public double Number
        {
            get { return this.number; }
            set { this.number = value; }
        }


        /// <summary>
        ///  시간설명(텍스트)
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


         /// <summary>
        /// 기본생성자
        /// </summary>
        public Time()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_number">
        /// 시간(숫자)
        /// </param>
        /// <param name="_text">
        /// 시간설명(텍스트)
        /// </param>
        public Time(double _number, string _text)
        {
            this.number = _number;
            this.text = _text;            
        }
    }
}
