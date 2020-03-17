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
        /// �ð�(����)
        /// </summary>
        public double Number
        {
            get { return this.number; }
            set { this.number = value; }
        }


        /// <summary>
        ///  �ð�����(�ؽ�Ʈ)
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


         /// <summary>
        /// �⺻������
        /// </summary>
        public Time()
        {
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="_number">
        /// �ð�(����)
        /// </param>
        /// <param name="_text">
        /// �ð�����(�ؽ�Ʈ)
        /// </param>
        public Time(double _number, string _text)
        {
            this.number = _number;
            this.text = _text;            
        }
    }
}
