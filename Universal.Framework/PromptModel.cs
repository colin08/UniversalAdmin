using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 提示模型类
    /// </summary>
    public class PromptModel
    {
        private string _linkurl = "";//跳转地址
        private string _status = ""; //状态码
        private string _message = "";//提示信息
        private string _details = "";//消息详情
        private int _countdownmodel = 0;//倒计时模型
        private int _countdowntime = 5;//倒计时时间
        private bool _isshowlink = true;//是否显示跳转地址
        private bool _isautolink = true;//是否自动跳转

        public PromptModel()
        {

        }

        /// <summary>
        /// 显示消息，不做跳转
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public PromptModel(string status,string message, string details)
        {
            _status = status;
            _details = details;
            _message = message;
            _isshowlink = false;
            _isautolink = false;
        }

        /// <summary>
        /// 进行自动跳转，但不显示倒计时模型
        /// </summary>
        /// <param name="linkUrl"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public PromptModel(string linkUrl,string status, string message, string details)
        {
            _linkurl = linkUrl;
            _details = details;
            _status = status;
            _message = message;
        }

        /// <summary>
        /// 跳转前显示倒计时模型
        /// </summary>
        /// <param name="linkUrl"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="time"></param>
        public PromptModel(string linkUrl, string status, string message, string details,int time)
        {
            _linkurl = linkUrl;
            _details = details;
            _status = status;
            _message = message;
            if(time >0)
            {
                _countdownmodel = 1;
                _countdowntime = time;
            }
        }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string LinkUrl
        {
            get { return _linkurl; }
            set { _linkurl = value; }
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 倒计时模型
        /// </summary>
        public int CountdownModel
        {
            get { return _countdownmodel; }
            set { _countdownmodel = value; }
        }

        /// <summary>
        /// 倒计时时间
        /// </summary>
        public int CountdownTime
        {
            get { return _countdowntime; }
            set { _countdowntime = value; }
        }

        /// <summary>
        /// 是否显示返回地址
        /// </summary>
        public bool IsShowLink
        {
            get { return _isshowlink; }
            set { _isshowlink = value; }
        }

        /// <summary>
        /// 是否自动返回
        /// </summary>
        public bool IsAutoLink
        {
            get { return _isautolink; }
            set { _isautolink = value; }
        }
    }
}
