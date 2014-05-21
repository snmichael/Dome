using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Demo.Framework.Core
{
    /// <summary>
    ///SendMail 电子邮件发送类
    /// </summary>
    [Serializable]
    public class SendToMail
    {
        public SendToMail()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        private string mailMessage;

        /// <summary>
        /// 获取邮件发送时产生的提示信息
        /// </summary>
        public string MailMessage
        {
            get { return mailMessage; }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="host">指定 smtp 服务器地址</param>
        /// <param name="port">指定 smtp 服务器的端口，默认是25</param>
        /// <param name="userName">指定 smtp 服务器的端口 的账户 如测试本地，无需提供</param>
        /// <param name="password">指定 smtp 服务器的端口 的密码</param>
        /// <param name="fromMail">发信人地址</param>
        /// <param name="replyToMail">ReplyTo 表示对方回复邮件时默认的接收地址，即：你用一个邮箱发信，但却用另一个来收信</param>
        /// <param name="ccMailAddress">邮件的抄送者，支持群发，多个邮件地址之间用 半角逗号 分开 如："a@163.com,b@163.com,c@163.com"</param>
        /// <param name="bccMailAddress">//邮件的密送者，支持群发，多个邮件地址之间用 半角逗号 分开 "d@163.com,e@163.com"</param>
        /// <param name="toMailAddress">邮件的接收者，支持群发，多个地址之间用 半角逗号 分开 "g@163.com,h@163.com"</param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="IsBodyHtml">邮件正文是否是HTML格式 true:是， false:否</param>
        /// <param name="body">主题</param>
        /// <param name="achList">附件,支持多附件 List<Attachment></param>
        public void SendEmail(string host,
            int port,
            string userName,
            string password,
            string fromMail,
            string replyToMail,
            Hashtable ccMailAddress,
            string bccMailAddress,
            Hashtable toMailAddress,
            string Subject,
            bool IsBodyHtml,
            string body,
            List<Attachment> achList,
            IList<LinkedResource> linkedResourceList,
            MailPriority priority = MailPriority.Normal)
        {
            this.mailMessage = "";
            if (CheckParas(host, fromMail, toMailAddress, Subject, body) == false) return;
            SmtpClient smtp = new SmtpClient(); //实例化一个SmtpClient
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //将smtp的出站方式设为 Network
            smtp.EnableSsl = false;//smtp服务器是否启用SSL加密
            smtp.Host = host; //指定 smtp 服务器地址
            smtp.Port = port; //指定 smtp 服务器的端口，默认是25
            smtp.UseDefaultCredentials = true;
            //SMTP服务器认证
            if (userName != "")
                smtp.Credentials = new NetworkCredential(userName, password);
            MailMessage mm = new MailMessage(); //实例化一个邮件类
            mm.Priority = MailPriority.Normal; //邮件的优先级，分为 Low, Normal, High
            mm.From = new MailAddress(fromMail, "", Encoding.GetEncoding(936));
            //ReplyTo 表示对方回复邮件时默认的接收地址，即：你用一个邮箱发信，但却用另一个来收信
            if (replyToMail != "")
                mm.ReplyTo = new MailAddress(replyToMail, "我的接收邮箱", Encoding.GetEncoding(936));
            //邮件的抄送者，支持群发，多个邮件地址之间用 半角逗号 分开
            if (ccMailAddress != null && ccMailAddress.Count > 0)
            {
                foreach (DictionaryEntry de in ccMailAddress)
                {
                    mm.CC.Add(new MailAddress(de.Key.ToString(), de.Value.ToString(), Encoding.GetEncoding(936)));
                }
            }

            //邮件的密送者，支持群发，多个邮件地址之间用 半角逗号 分开
            //可以任意设置，此信息包含在邮件头中，但并不会验证有效性，也不会显示给收件人
            if (bccMailAddress != "")
                mm.Bcc.Add(bccMailAddress);

            //邮件的接收者，支持群发，多个地址之间用 半角逗号 分开
            if (toMailAddress != null && toMailAddress.Count > 0)
            {
                foreach (DictionaryEntry de in toMailAddress)
                {
                    var tos = de.Value.ToString().Split(';');

                    foreach (var to in tos)
                    {
                        mm.To.Add(new MailAddress(to, to, Encoding.GetEncoding(936)));
                    }
                }
            }
            mm.Priority = priority;
            mm.Subject = Subject; //邮件标题
            mm.SubjectEncoding = Encoding.GetEncoding(936);
            // 这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码。
            // 936是简体中文的pagecode，如果是英文标题，这句可以忽略不用
            mm.IsBodyHtml = true; //邮件正文是否是HTML格式
            //邮件正文的编码， 设置不正确， 接收者会收到乱码
            mm.BodyEncoding = Encoding.GetEncoding(936);


            if (linkedResourceList != null && linkedResourceList.Count > 0)
            {
                AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                foreach (var resource in linkedResourceList)
                {
                    htmlBody.LinkedResources.Add(resource);
                }
                mm.AlternateViews.Add(htmlBody);  
            }
            else
            {
                //邮件正文
                mm.Body = body;    
            }

            
            //添加附件，第二个参数，表示附件的文件类型，可以不用指定
            if (achList != null && achList.Count > 0)
            {
                foreach (Attachment ach in achList)
                {
                    mm.Attachments.Add(ach);
                }
            }
            try
            {
                //发送邮件，如果不返回异常， 则大功告成了。
                smtp.Send(mm);
            }
            catch (Exception ex)
            {
                this.mailMessage = "邮件发送失败!";
                throw;
            }
            finally
            {
                if (smtp!=null)
                {
                    smtp.Dispose();
                }
            }

            this.mailMessage = "邮件发送成功!";
        }

        /// <summary>
        /// 验证邮件参数信息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="fromMail"></param>
        /// <param name="toMailAddress"></param>
        /// <param name="Subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private bool CheckParas(string host,
            string fromMail,
            Hashtable toMailAddress,
            string Subject,
            string body)
        {
            if (host == "")
            {
                this.mailMessage = "请指定 smtp 服务器地址";
                return false;
            }
            if (fromMail == "")
            {
                this.mailMessage = "请指定发信人地址";
                return false;
            }
            if (toMailAddress == null || toMailAddress.Count == 0)
            {
                this.mailMessage = "请至少指定一个收信人地址";
                return false;
            }
            if (Subject == "")
            {
                this.mailMessage = "请指定邮件标题";
                return false;
            }
            if (body == "")
            {
                this.mailMessage = "请输入邮件正文或内容";
                return false;
            }
            return true;
        }
    }
}