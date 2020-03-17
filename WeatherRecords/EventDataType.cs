using System;
using System.Collections.Generic;
using System.Text;

namespace ADEng.Module.WeatherSystem
{
    public class EventDataType
    {
        private string itemId = string.Empty;

        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        private string checkTime = string.Empty;

        public string CheckTime
        {
            get { return checkTime; }
            set { checkTime = value; }
        }

        private string deviceName = string.Empty;

        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }
        private string checkType = string.Empty;

        public string CheckType
        {
            get { return checkType; }
            set { checkType = value; }
        }
        private string itemName = string.Empty;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        private string chkValue = string.Empty;

        public string ChkValue
        {
            get { return chkValue; }
            set { chkValue = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        public EventDataType()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_itemId">
        /// 측기항목아이디
        /// </param>
        /// <param name="_checkTime">
        /// 체크시각
        /// </param>
        /// <param name="_deviceName">
        /// 측기명
        /// </param>
        /// <param name="_checkType">
        /// 구분
        /// </param>
        /// <param name="_itemName">
        /// 상태항목
        /// </param>
        /// <param name="_chkValue">
        /// 상태값
        /// </param>
        public EventDataType(string _itemId, string _checkTime, string _deviceName, string _checkType, string _itemName, string _chkValue)
        {
            this.itemId = _itemId;
            this.checkTime = _checkTime;
            this.deviceName = _deviceName;
            this.checkType = _checkType;
            this.itemName = _itemName;
            this.chkValue = _chkValue;
        }
    }
}
