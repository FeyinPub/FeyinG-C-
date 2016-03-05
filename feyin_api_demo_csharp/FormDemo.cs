using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Xml;
using System.Net;
using System.Text;
using System.Web;
using System.IO;

namespace demo
{
	/// <summary>
	/// Form1 ��ժҪ˵����
	/// </summary>
	public class FormDemo : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.TextBox txtServerPort;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cmdSendMessage;
        private Label label3;
        private Label label5;
        private TextBox txtSecurityKey;
        private TextBox txtMemberCode;
        private Label label7;
        private TextBox txtDeviceNo;
        private Label label8;
        private Label label9;
        private TextBox txtMsgNo2;
        private Button cmdQueryState;
        private Label sendResultLabel;
        private Button cmdSendFormatedMessage;
        private Label label4;
        private TextBox txtMsgNo;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private TextBox txtDeviceList;
        private Button cmdGetDeviceList;
        private Button cmdGetException;
        private TextBox txtException;
        private Label label15;
        private Button cmdGetExample;
        private Label label16;
        private Label queryStateLabel;
        private Label label17;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;


        /// <summary>
        /// ȡ��ʱ����������� java�е� System.currentTimeMillis()
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentMilli()
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            return (long)javaSpan.TotalMilliseconds;
        }

        /// <summary>
        /// ȡ��һ���ַ�����md5ֵ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }

        /// <summary>
        /// ���͸�ʽ������Ϣ����
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="securityKey"></param>
        /// <param name="msgNo"></param>
        /// <param name="deviceNo"></param>
        /// <param name="formatedMessage"></param>
        /// <returns></returns>
        public string SendFormatedMessage(string memberCode, string securityKey, string msgNo, string deviceNo, string msgDetail)
        {
            return SendFormatedMessage(memberCode, securityKey, msgNo, deviceNo, msgDetail, "", "", "", "", "", "", "", "", "");
        }

        public string SendFormatedMessage(string memberCode, string securityKey, string msgNo, string deviceNo, string msgDetail, string charge, 
			string customerName, string customerPhone, string customerAddress, string customerMemo, 
			string extra1, string extra2, string extra3, string extra4)
        {
			int mode = 1;
			long reqTime = GetCurrentMilli();
			string qstr = "memberCode=" + HttpUtility.UrlEncode(memberCode, Encoding.UTF8);
			qstr += "&charge=" + HttpUtility.UrlEncode(charge, Encoding.UTF8);
			qstr += "&customerName=" + HttpUtility.UrlEncode(customerName, Encoding.UTF8);
			qstr += "&customerPhone=" + HttpUtility.UrlEncode(customerPhone, Encoding.UTF8);
			qstr += "&customerAddress=" + HttpUtility.UrlEncode(customerAddress, Encoding.UTF8);
			qstr += "&customerMemo=" + HttpUtility.UrlEncode(customerMemo, Encoding.UTF8);
			qstr += "&msgDetail=" + HttpUtility.UrlEncode(msgDetail, Encoding.UTF8);
			qstr += "&deviceNo=" + HttpUtility.UrlEncode(deviceNo, Encoding.UTF8);
			qstr += "&msgNo=" + HttpUtility.UrlEncode(msgNo, Encoding.UTF8);
			qstr += "&extra1=" + HttpUtility.UrlEncode(extra1, Encoding.UTF8);
			qstr += "&extra2=" + HttpUtility.UrlEncode(extra2, Encoding.UTF8);
			qstr += "&extra3=" + HttpUtility.UrlEncode(extra3, Encoding.UTF8);
			qstr += "&extra4=" + HttpUtility.UrlEncode(extra4, Encoding.UTF8);
            qstr += "&mode=" + mode;
            qstr += "&reqTime=" + reqTime;
            qstr += "&securityCode=" + GetMD5Hash(memberCode + customerName + customerPhone + customerAddress
                                                + customerMemo  + msgDetail + deviceNo + msgNo + reqTime + securityKey);

            return SendMessage(qstr); 
        }

        /// <summary>
        /// �������ɸ�ʽ�Ķ���
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="securityKey"></param>
        /// <param name="msgNo"></param>
        /// <param name="deviceNo"></param>
        /// <param name="content"></param>
        /// <returns></returns>
		public string SendFreeMessage(string memberCode, string securityKey, string msgNo,string deviceNo, string msgDetail)
		{
			int mode = 2;
            long reqTime = GetCurrentMilli();
            string qstr = "memberCode=" + HttpUtility.UrlEncode(memberCode, Encoding.UTF8);
            qstr += "&deviceNo=" + HttpUtility.UrlEncode(deviceNo, Encoding.UTF8);
            qstr += "&msgNo=" + HttpUtility.UrlEncode(msgNo, Encoding.UTF8);
            qstr += "&msgDetail=" + HttpUtility.UrlEncode(msgDetail, Encoding.UTF8);
            qstr += "&mode=" + mode;
            qstr += "&reqTime=" + reqTime;
            qstr += "&securityCode=" + GetMD5Hash(memberCode + msgDetail + deviceNo + msgNo + reqTime + securityKey);

            return SendMessage(qstr);
		}

        /// <summary>
        /// ���õķ�����Ϣ����
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="securityKey"></param>
        /// <param name="msgNo"></param>
        /// <param name="deviceNo"></param>
        /// <param name="content"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private string SendMessage(string qstr)
        {

            HttpWebRequest req = (HttpWebRequest)
                HttpWebRequest.Create("http://my.feyin.net/api/sendMsg");

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = qstr.Length;

            StreamWriter writer = new StreamWriter(req.GetRequestStream(), Encoding.ASCII);
            writer.Write(qstr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //Ĭ�ϱ���
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string data = reader.ReadToEnd();

            response.Close();

            return data;
        }

        /// <summary>
        /// ������HTTP��ѯ����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string Query(string url)
        {
            
            HttpWebRequest req = (HttpWebRequest)
                HttpWebRequest.Create(url);

            req.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();

            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //Ĭ�ϱ���
            }

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string data = reader.ReadToEnd();

            response.Close();

            return data;

        }

        /// <summary>
        /// ��ѯ��ӡ״̬
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="msgNo"></param>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public string QueryState(string memberCode,string msgNo,string securityKey)
	   {
            long reqTime = GetCurrentMilli();

            string qstr = "http://my.feyin.net/api/queryState?memberCode=" + HttpUtility.UrlEncode(memberCode, Encoding.UTF8);
            qstr += "&reqTime=" + reqTime;
            qstr += "&msgNo=" + HttpUtility.UrlEncode(msgNo, Encoding.UTF8);
            qstr += "&securityCode=" + GetMD5Hash(memberCode + reqTime + securityKey + msgNo);


            return Query(qstr);
    
		}

        /// <summary>
        /// ��ѯ�豸�б�
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public string ListDevice(string memberCode, string securityKey)
        {
            long reqTime = GetCurrentMilli();

            string qstr = "http://my.feyin.net/api/listDevice?memberCode=" + HttpUtility.UrlEncode(memberCode, Encoding.UTF8);
            qstr += "&reqTime=" + reqTime;
            qstr += "&securityCode=" + GetMD5Hash(memberCode + reqTime + securityKey);


            return Query(qstr);

        }

        /// <summary>
        /// ��ѯ�쳣�б�
        /// </summary>
        /// <param name="memberCode"></param>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public string ListException(string memberCode, string securityKey)
        {
            long reqTime = GetCurrentMilli();

            string qstr = "http://my.feyin.net/api/listException?memberCode=" + HttpUtility.UrlEncode(memberCode, Encoding.UTF8);
            qstr += "&reqTime=" + reqTime;
            qstr += "&securityCode=" + GetMD5Hash(memberCode + reqTime + securityKey);


            return Query(qstr);

        }



		public FormDemo()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.cmdSendMessage = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSecurityKey = new System.Windows.Forms.TextBox();
            this.txtMemberCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDeviceNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMsgNo2 = new System.Windows.Forms.TextBox();
            this.cmdQueryState = new System.Windows.Forms.Button();
            this.sendResultLabel = new System.Windows.Forms.Label();
            this.cmdSendFormatedMessage = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMsgNo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtDeviceList = new System.Windows.Forms.TextBox();
            this.cmdGetDeviceList = new System.Windows.Forms.Button();
            this.cmdGetException = new System.Windows.Forms.Button();
            this.txtException = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmdGetExample = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.queryStateLabel = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(54, 16);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(100, 21);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "my.feyin.net";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(209, 16);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(100, 21);
            this.txtServerPort.TabIndex = 2;
            this.txtServerPort.Text = "80";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "������:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(168, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "�˿�:";
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(54, 161);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(293, 171);
            this.txtContent.TabIndex = 12;
            // 
            // cmdSendMessage
            // 
            this.cmdSendMessage.Location = new System.Drawing.Point(53, 350);
            this.cmdSendMessage.Name = "cmdSendMessage";
            this.cmdSendMessage.Size = new System.Drawing.Size(105, 23);
            this.cmdSendMessage.TabIndex = 14;
            this.cmdSendMessage.Text = "��ӡ���ɸ�ʽ��Ϣ";
            this.cmdSendMessage.Click += new System.EventHandler(this.cmdSendMessage_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 37);
            this.label6.TabIndex = 16;
            this.label6.Text = "��ӡ����:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(493, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "��Կ:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(333, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "�ʺ�:";
            // 
            // txtSecurityKey
            // 
            this.txtSecurityKey.Location = new System.Drawing.Point(534, 16);
            this.txtSecurityKey.Name = "txtSecurityKey";
            this.txtSecurityKey.Size = new System.Drawing.Size(100, 21);
            this.txtSecurityKey.TabIndex = 18;
            this.txtSecurityKey.Text = "b06ae5c9";
            // 
            // txtMemberCode
            // 
            this.txtMemberCode.Location = new System.Drawing.Point(378, 16);
            this.txtMemberCode.Name = "txtMemberCode";
            this.txtMemberCode.Size = new System.Drawing.Size(100, 21);
            this.txtMemberCode.TabIndex = 17;
            this.txtMemberCode.Text = "7";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "�豸:";
            // 
            // txtDeviceNo
            // 
            this.txtDeviceNo.Location = new System.Drawing.Point(53, 130);
            this.txtDeviceNo.Name = "txtDeviceNo";
            this.txtDeviceNo.Size = new System.Drawing.Size(127, 21);
            this.txtDeviceNo.TabIndex = 21;
            this.txtDeviceNo.Text = "460000000646394";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(186, 133);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 16);
            this.label8.TabIndex = 23;
            this.label8.Text = "�豸��ţ���460000000646394";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(2, 449);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "��Ϣ��:";
            // 
            // txtMsgNo2
            // 
            this.txtMsgNo2.Location = new System.Drawing.Point(53, 444);
            this.txtMsgNo2.Name = "txtMsgNo2";
            this.txtMsgNo2.Size = new System.Drawing.Size(128, 21);
            this.txtMsgNo2.TabIndex = 24;
            // 
            // cmdQueryState
            // 
            this.cmdQueryState.Location = new System.Drawing.Point(188, 444);
            this.cmdQueryState.Name = "cmdQueryState";
            this.cmdQueryState.Size = new System.Drawing.Size(75, 23);
            this.cmdQueryState.TabIndex = 26;
            this.cmdQueryState.Text = "��ѯ";
            this.cmdQueryState.Click += new System.EventHandler(this.cmdQueryState_Click_1);
            // 
            // sendResultLabel
            // 
            this.sendResultLabel.Location = new System.Drawing.Point(130, 385);
            this.sendResultLabel.Name = "sendResultLabel";
            this.sendResultLabel.Size = new System.Drawing.Size(217, 16);
            this.sendResultLabel.TabIndex = 27;
            // 
            // cmdSendFormatedMessage
            // 
            this.cmdSendFormatedMessage.Location = new System.Drawing.Point(164, 350);
            this.cmdSendFormatedMessage.Name = "cmdSendFormatedMessage";
            this.cmdSendFormatedMessage.Size = new System.Drawing.Size(110, 23);
            this.cmdSendFormatedMessage.TabIndex = 28;
            this.cmdSendFormatedMessage.Text = "��ӡ��ʽ����Ϣ";
            this.cmdSendFormatedMessage.Click += new System.EventHandler(this.cmdSendFormatedMessage_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "��Ϣ��:";
            // 
            // txtMsgNo
            // 
            this.txtMsgNo.Location = new System.Drawing.Point(54, 104);
            this.txtMsgNo.Name = "txtMsgNo";
            this.txtMsgNo.Size = new System.Drawing.Size(127, 21);
            this.txtMsgNo.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(187, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(179, 16);
            this.label10.TabIndex = 31;
            this.label10.Text = "Ψһ����Ϣ�ţ�������Ϣ����";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(52, 385);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 16);
            this.label11.TabIndex = 32;
            this.label11.Text = "����������:";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("����", 13F);
            this.label12.ForeColor = System.Drawing.Color.Coral;
            this.label12.Location = new System.Drawing.Point(127, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 27);
            this.label12.TabIndex = 33;
            this.label12.Text = "��Ϣ��ӡ";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("����", 13F);
            this.label13.ForeColor = System.Drawing.Color.Coral;
            this.label13.Location = new System.Drawing.Point(93, 413);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(121, 28);
            this.label13.TabIndex = 34;
            this.label13.Text = "��ӡ״̬��ѯ";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("����", 13F);
            this.label14.ForeColor = System.Drawing.Color.Coral;
            this.label14.Location = new System.Drawing.Point(531, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(87, 27);
            this.label14.TabIndex = 35;
            this.label14.Text = "�豸��ѯ";
            // 
            // txtDeviceList
            // 
            this.txtDeviceList.Location = new System.Drawing.Point(378, 104);
            this.txtDeviceList.Multiline = true;
            this.txtDeviceList.Name = "txtDeviceList";
            this.txtDeviceList.ReadOnly = true;
            this.txtDeviceList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeviceList.Size = new System.Drawing.Size(353, 130);
            this.txtDeviceList.TabIndex = 36;
            // 
            // cmdGetDeviceList
            // 
            this.cmdGetDeviceList.Location = new System.Drawing.Point(513, 249);
            this.cmdGetDeviceList.Name = "cmdGetDeviceList";
            this.cmdGetDeviceList.Size = new System.Drawing.Size(105, 23);
            this.cmdGetDeviceList.TabIndex = 37;
            this.cmdGetDeviceList.Text = "��ȡ�豸�б�";
            this.cmdGetDeviceList.Click += new System.EventHandler(this.cmdGetDeviceList_Click);
            // 
            // cmdGetException
            // 
            this.cmdGetException.Location = new System.Drawing.Point(513, 471);
            this.cmdGetException.Name = "cmdGetException";
            this.cmdGetException.Size = new System.Drawing.Size(105, 23);
            this.cmdGetException.TabIndex = 40;
            this.cmdGetException.Text = "��ȡ�쳣�б�";
            this.cmdGetException.Click += new System.EventHandler(this.cmdGetException_Click);
            // 
            // txtException
            // 
            this.txtException.Location = new System.Drawing.Point(378, 326);
            this.txtException.Multiline = true;
            this.txtException.Name = "txtException";
            this.txtException.ReadOnly = true;
            this.txtException.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtException.Size = new System.Drawing.Size(353, 130);
            this.txtException.TabIndex = 39;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("����", 13F);
            this.label15.ForeColor = System.Drawing.Color.Coral;
            this.label15.Location = new System.Drawing.Point(531, 290);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 21);
            this.label15.TabIndex = 38;
            this.label15.Text = "�쳣��ѯ";
            // 
            // cmdGetExample
            // 
            this.cmdGetExample.Location = new System.Drawing.Point(3, 290);
            this.cmdGetExample.Name = "cmdGetExample";
            this.cmdGetExample.Size = new System.Drawing.Size(40, 23);
            this.cmdGetExample.TabIndex = 41;
            this.cmdGetExample.Text = "ʾ��";
            this.cmdGetExample.Click += new System.EventHandler(this.cmdGetExample_Click);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(52, 478);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 16);
            this.label16.TabIndex = 43;
            this.label16.Text = "����������:";
            // 
            // queryStateLabel
            // 
            this.queryStateLabel.Location = new System.Drawing.Point(132, 478);
            this.queryStateLabel.Name = "queryStateLabel";
            this.queryStateLabel.Size = new System.Drawing.Size(204, 16);
            this.queryStateLabel.TabIndex = 42;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(3, 249);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(45, 37);
            this.label17.TabIndex = 44;
            this.label17.Text = "��ʽ������";
            // 
            // FormDemo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(743, 502);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.queryStateLabel);
            this.Controls.Add(this.cmdGetExample);
            this.Controls.Add(this.cmdGetException);
            this.Controls.Add(this.txtException);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cmdGetDeviceList);
            this.Controls.Add(this.txtDeviceList);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMsgNo);
            this.Controls.Add(this.cmdSendFormatedMessage);
            this.Controls.Add(this.sendResultLabel);
            this.Controls.Add(this.cmdQueryState);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtMsgNo2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDeviceNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSecurityKey);
            this.Controls.Add(this.txtMemberCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdSendMessage);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.txtServer);
            this.Name = "FormDemo";
            this.Text = "Feyin API Test";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormDemo());
		}


		private void cmdSendMessage_Click(object sender, System.EventArgs e)
		{
            this.cmdSendMessage.Enabled = false;
            this.sendResultLabel.Text = "������...";
            string ret = SendFreeMessage(txtMemberCode.Text, txtSecurityKey.Text, txtMsgNo.Text, txtDeviceNo.Text, txtContent.Text);
            this.sendResultLabel.Text = ret;
            this.cmdSendMessage.Enabled = true;

			
		}


        private void cmdSendFormatedMessage_Click(object sender, EventArgs e)
        {
            this.cmdSendFormatedMessage.Enabled = false;
            this.sendResultLabel.Text = "������...";
            string ret = SendFormatedMessage(txtMemberCode.Text, txtSecurityKey.Text, txtMsgNo.Text, txtDeviceNo.Text, txtContent.Text);
            this.sendResultLabel.Text = ret;

            this.cmdSendFormatedMessage.Enabled = true;
        }

        
        
        private void cmdQueryState_Click_1(object sender, EventArgs e)
        {
            this.cmdQueryState.Enabled = false;
            string ret = this.QueryState(txtMemberCode.Text, txtMsgNo2.Text, txtSecurityKey.Text);
            this.queryStateLabel.Text = ret;

            this.cmdQueryState.Enabled = true;

        }

        private void cmdGetDeviceList_Click(object sender, EventArgs e)
        {
            this.cmdGetDeviceList.Enabled = false;
            string ret = this.ListDevice(txtMemberCode.Text, txtSecurityKey.Text);
            this.txtDeviceList.Text = ret;

            this.cmdGetDeviceList.Enabled = true;
        }

        private void cmdGetException_Click(object sender, EventArgs e)
        {
            this.cmdGetException.Enabled = false;
            string ret = this.ListException(txtMemberCode.Text, txtSecurityKey.Text);
            this.txtException.Text = ret;
            this.cmdGetException.Enabled = true;
        }

        private void cmdGetExample_Click(object sender, EventArgs e)
        {
            string content = "��Ʒ����A@1000@3||��Ʒ����B@2000@4||��Ʒ����C@3000@9";
            this.txtContent.Text = content;
        }


       




	}
}
