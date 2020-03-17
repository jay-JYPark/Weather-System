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
        /// �⺻������
        /// </summary>
        public EventDataType()
        {
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="_itemId">
        /// �����׸���̵�
        /// </param>
        /// <param name="_checkTime">
        /// üũ�ð�
        /// </param>
        /// <param name="_deviceName">
        /// �����
        /// </param>
        /// <param name="_checkType">
        /// ����
        /// </param>
        /// <param name="_itemName">
        /// �����׸�
        /// </param>
        /// <param name="_chkValue">
        /// ���°�
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
