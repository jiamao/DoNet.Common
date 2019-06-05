//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 邮件操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Mail;
using System.Configuration;

namespace DoNet.Common.Net
{
    public class MailHelper : IDisposable
    {
        const int MaxMailQueueLength = 1000;

        object _lockObject = new object();
        bool _working = false;

        string _mailServerName = string.Empty;
        string _user = string.Empty;
        string _userPassword = string.Empty;
        
        Queue<MailUnit> _mailQueue = new Queue<MailUnit>();


        public MailHelper()
        {
        }

        public void Startup()
        {
            lock (_lockObject)
            {
                if (!_working)
                {

                    Thread workingThread = new Thread(new ThreadStart(Process));
                    _working = true;
                    workingThread.Start();
                }
            }
        }

        public void Shutdown()
        {
            System.Threading.Thread.Sleep(5000);
            lock (_lockObject)
            {
                _working = false;
                _mailQueue.Clear();
            }
        }

        public void Dispose()
        {          
            Shutdown();
        }

        public bool QueueSend(MailUnit unit)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(unit);
                    return true;
                }
            }
        }

        public bool QueueSend(string receiver, string subject, string content, params Attachment[] attachmentList)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(new MailUnit(receiver, subject, content, attachmentList));
                    return true;
                }
            }
        }

        public bool QueueSend(string receiver, string subject, string content, bool ishtml, params Attachment[] attachmentList)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(new MailUnit(receiver, subject, content, ishtml, attachmentList));
                    return true;
                }
            }
        }

        public bool QueueSend(string receiver, string subject, string content, bool ishtml, params LinkedResource[] attachmentList)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(new MailUnit(receiver, subject, content,ishtml, attachmentList));
                    return true;
                }
            }
        }

        public bool QueueSend(string server,string sender,string user,string pwd,string receiver, string subject, string content, params Attachment[] attachmentList)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(new MailUnit(server,user,pwd,receiver,sender, subject, content, attachmentList));
                    return true;
                }
            }
        }

        public bool QueueSend(string server, string sender, string user, string pwd, string receiver, string subject, string content,bool ishtml, params Attachment[] attachmentList)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(new MailUnit(server, user, pwd, receiver, sender, subject, content,ishtml, attachmentList));
                    return true;
                }
            }
        }

        public bool QueueSend(string server, string sender, string user, string pwd, string receiver, string subject, string content, bool ishtml,Attachment[] attachmentList,params LinkedResource[] linkedReses)
        {
            lock (_lockObject)
            {
                if (_mailQueue.Count >= MaxMailQueueLength)
                {
                    return false;
                }
                else
                {
                    _mailQueue.Enqueue(new MailUnit(server, user, pwd, receiver, sender, subject, content, ishtml, attachmentList,linkedReses));
                    return true;
                }
            }
        }

        private void Process()
        {
            while (_working)
            {
                try
                {
                    lock (_lockObject)
                    {
                        if (_mailQueue.Count > 0)
                        {
                            MailUnit mailUnit = _mailQueue.Peek();
                            switch (mailUnit.SendState)
                            {
                                case MailUnit.State.Init:
                                    mailUnit.AsyncSendMail();
                                    break;
                                case MailUnit.State.Doing:
                                    mailUnit.AsyncSendMailCheck();
                                    break;
                                case MailUnit.State.Done:
                                    _mailQueue.Dequeue();
                                    mailUnit.Dispose(); //释放非托管资源，否则会导致附件文件被占用
                                    break;
                                default:
                                    _mailQueue.Dequeue();
                                    IO.Logger.Write("MailUnit State Error : State - " + mailUnit.SendState.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            _working = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    IO.Logger.Write(ex.ToString());
                }

                System.Threading.Thread.Sleep(1000); //5秒
            }
        }

        ~MailHelper()
        {
            Shutdown();
        }
    }

    public class MailUnit : IDisposable
    {
        const int DefaultSendTimeOut = 60000;           //30秒超时
        /// <summary>
        /// 发送完成回调
        /// </summary>
        public event EventHandler SendCompleteHandler;

        public enum State
        {
            Init = 0,
            Doing = 1,
            Done
        }
        string _mailserver;

        public string Mailserver
        {
            get 
            {
                if (string.IsNullOrEmpty(_mailserver))
                {
                    _mailserver = ConfigurationManager.AppSettings["MailServerName"];
                }
                return _mailserver; 
            }
            set { _mailserver = value; }
        }
        string _mailUser;

        public string MailUser
        {
            get 
            {
                if (string.IsNullOrEmpty(_mailUser))
                {
                    _mailUser = ConfigurationManager.AppSettings["MailUserName"];
                }
                return _mailUser; 
            }
            set { _mailUser = value; }
        }
        string _mailPwd;

        public string MailPwd
        {
            get 
            {
                if (string.IsNullOrEmpty(_mailPwd))
                {
                    _mailPwd = ConfigurationManager.AppSettings["MailUserPassword"];
                }
                return _mailPwd; 
            }
            set { _mailPwd = value; }
        }

        bool _isHtml = false;

        public bool IsHtml
        {
            get { return _isHtml; }
            set { _isHtml = value; }
        }

        string _receiver;
        string _subject;
        string _content;
        State _state;
        SmtpClient _mailClient = null;

        public SmtpClient MailClient
        {
            get { return _mailClient; }
            set { _mailClient = value; }
        }
        MailMessage _mailMessage = null;

        public string ConvertHeaderToBase64(string s, string encoding)
        {
            int lineLength = 40;           // 每行处理 40 个字节

            byte[] buffer = Encoding.GetEncoding(encoding).GetBytes(s);       // 转换为字节码
            StringBuilder sb = new StringBuilder();       // 保存最终结果
            string linebase64;
            int block = buffer.Length % lineLength == 0 ? buffer.Length / lineLength : buffer.Length / lineLength + 1;
            for (int i = 0; i < block; i++)
            {
                if (buffer.Length - i * lineLength >= lineLength)
                    linebase64 = Convert.ToBase64String(buffer, i * lineLength, lineLength);
                else
                    linebase64 = Convert.ToBase64String(buffer, i * lineLength, buffer.Length - i * lineLength);
                sb.Append("=?");
                sb.Append(encoding);
                sb.Append("?B?");
                sb.Append(linebase64);
                sb.Append("?=\r\n\t");
            }
            sb.Remove(sb.Length - 3, 3);          // 删除最后的换行符号
            return sb.ToString();
        }

        public MailMessage MailMessage
        {
            get
            {
                _mailMessage = new MailMessage();
                _mailMessage.SubjectEncoding = Encoding.UTF8;
                _mailMessage.BodyEncoding = Encoding.UTF8;
                string isturnsubject=ConfigurationManager.AppSettings["TurnSubject"];
                _mailMessage.Subject = string.IsNullOrEmpty(isturnsubject) || isturnsubject.ToLower().Trim() != "false" ? ConvertHeaderToBase64(_subject, "utf-8") : _subject;

                //没有内嵌资源
                if (linkedres == null || linkedres.Length == 0)
                {
                    _mailMessage.Body = _content;
                }
                else
                {
                    string plainTextBody = "如果你邮件客户端不支持HTML格式，或者你切换到“普通文本”视图，将看到此内容";
                    _mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainTextBody, null, "text/plain"));
                    //_mailMessage.Body = _content;

                    AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(_content, null, "text/html");
                    foreach (LinkedResource lr in linkedres)
                    {
                        htmlBody.LinkedResources.Add(lr);
                    }

                    _mailMessage.AlternateViews.Add(htmlBody);
                }

                _mailMessage.IsBodyHtml = _isHtml;
                //添加附件
                if (_attachmentList != null && _attachmentList.Length > 0)
                {
                    foreach (Attachment attachment in _attachmentList)
                    {
                        if (attachment == null) continue;
                        _mailMessage.Attachments.Add(attachment);
                    }
                }

                _mailMessage.From = new MailAddress(Sender);
                ////如果发送失败，SMTP 服务器将发送 失败邮件告诉我
                _mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (!string.IsNullOrEmpty(MailCC))
                {
                    string[] ccs = MailCC.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in ccs)
                    {
                        if (string.IsNullOrEmpty(str.Trim())) continue;
                        _mailMessage.CC.Add(str.Trim());
                    }
                }

                string[] receivers = Receiver.Split(new char[] { ';',',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in receivers)
                {
                    if (string.IsNullOrEmpty(str.Trim())) continue;
                    _mailMessage.To.Add(str.Trim());
                }

                return _mailMessage;
            }
            set { _mailMessage = value; }
        }

        DateTime _sendTimeStamp = DateTime.MinValue;
        Attachment[] _attachmentList = null;

        string _Sender;
        public string Sender
        {
            get
            {
                if (string.IsNullOrEmpty(_Sender))
                {
                    _Sender = ConfigurationManager.AppSettings["MailSenderMail"];
                }
                return _Sender;
            }
            set
            {
                _Sender = value;
            }
        }

        public string Receiver
        {
            get 
            {
                if (string.IsNullOrEmpty(_receiver))
                {
                    _receiver = ConfigurationManager.AppSettings["MailReceiverMail"];
                }
                return _receiver;
            }
            set { _receiver = value; }
        }

        /// <summary>
        /// 邮件抄送人
        /// </summary>
        public string MailCC
        {
            get;
            set;
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public State SendState
        {
            get { return _state; }
        }

        public Attachment[] Attachments
        {
            get { return _attachmentList; }
            set { _attachmentList = value; }
        }

        LinkedResource[] linkedres = null;
        public LinkedResource[] LinkedReses
        {
            get { return linkedres; }
            set { linkedres = value; }
        }

        public MailUnit(string receiver, string subject, string content)
            : this(receiver, subject, content, null)
        {
        }

        public MailUnit(string receiver, string subject, string content, Attachment[] attachmentList)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            _receiver = receiver;
            _subject = subject;
            _content = content;
            _attachmentList = attachmentList;
        }

        public MailUnit(string receiver, string subject, string content,bool ishtml, Attachment[] attachmentList)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver"); 
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            _isHtml = ishtml;
            _receiver = receiver;
            _subject = subject;
            _content = content;
            _attachmentList = attachmentList;
        }

        public MailUnit(string receiver, string subject, string content, bool ishtml, LinkedResource[] attachmentList)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            _isHtml = ishtml;
            _receiver = receiver;
            _subject = subject;
            _content = content;
            linkedres = attachmentList;
        }

        public MailUnit(string mailserver,string user,string pwd,string receiver,string sender, string subject, string content, Attachment[] attachmentList)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            Mailserver = mailserver;
            MailUser = user;
            MailPwd = pwd;
            Sender = sender;

            _receiver = receiver;
            _subject = subject;
            _content = content;
            _attachmentList = attachmentList;
        }

        public MailUnit(string mailserver, string user, string pwd, string receiver, string sender, string subject, string content,bool ishtml, Attachment[] attachmentList)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            _isHtml = ishtml;
            Mailserver = mailserver;
            MailUser = user;
            MailPwd = pwd;
            Sender = sender;

            _receiver = receiver;
            _subject = subject;
            _content = content;
            _attachmentList = attachmentList;
        }

        public MailUnit(string mailserver, string user, string pwd, string receiver, string sender, string subject, string content, bool ishtml, Attachment[] attachmentList,params LinkedResource[] linkeRes)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            _isHtml = ishtml;
            Mailserver = mailserver;
            MailUser = user;
            MailPwd = pwd;
            Sender = sender;

            _receiver = receiver;
            _subject = subject;
            _content = content;
            _attachmentList = attachmentList;
            linkedres = linkeRes;
        }

        public void AsyncSendMail()
        {
            try
            {
                _mailClient = new SmtpClient(Mailserver);
                _mailClient.Credentials = new System.Net.NetworkCredential(MailUser,MailPwd);

                _mailClient.SendCompleted -= new SendCompletedEventHandler(AsyncSendCompleted);
                _mailClient.SendCompleted += new SendCompletedEventHandler(AsyncSendCompleted);

                _mailClient.SendAsync(MailMessage,null);

                _sendTimeStamp = DateTime.Now;
                _state = State.Doing;
            }
            catch (Exception ex)
            {
                IO.Logger.Write(ex.ToString());
                _state = State.Done;
            }
        }

        public void AsyncSendMailCheck()
        {
            try
            {
                if (((TimeSpan)DateTime.Now.Subtract(_sendTimeStamp)).TotalMilliseconds >= DefaultSendTimeOut)
                {
                    AsyncSendMailCancel();
                    IO.Logger.Write("SendMail Timeout");
                }
            }
            catch (Exception ex)
            {
                IO.Logger.Write(ex.ToString());
            }
        }

        /// <summary>
        /// 实现IDispose接口，释放可能的Attachment非托管资源（File Handle）
        /// </summary>                
        public void Dispose()
        {
            if (_mailMessage != null)
            {
                _mailMessage.Dispose();
            }
        }

        private void AsyncSendMailCancel()
        {
            try
            {
                if (_state != State.Done)
                {
                    if (_mailClient != null)
                    {
                        _mailClient.SendAsyncCancel();
                    }
                }
            }
            catch (Exception ex)
            {
                IO.Logger.Write(ex.ToString());
            }
            finally
            {
                _state = State.Done;
            }
        }

        private void AsyncSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            _state = State.Done;
            if (SendCompleteHandler != null)
            {
                SendCompleteHandler(this, e);
            }
        }
    }
}
