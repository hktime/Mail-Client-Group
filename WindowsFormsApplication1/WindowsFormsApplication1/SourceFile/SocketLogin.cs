using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1.Login
{
    public class SocketLogin
    {
		private sealed class LoginBacks
		{
            public const string ServerReadyOK = "220";
            public const string OperateComplete = "250";
            public const string AuthLoginOK = "334";
			public const string UsernameLoginOK = "334";
			public const string PasswordLoginOK = "235";

		}
		private sealed class LoginCommand
		{
            public const string Verify = "ehlo hello\r\n";
            public const string AuthLogin = "auth login\r\n";
		}

        private Exception exception_ = null;
        public Exception exception_p
        {
            get
            {
                return exception_;
            }
            set
            {
                exception_ = value;
            }
        }
        private Socket socket_;
        public Socket socket_p
        {
            get
            {
                return socket_;
            }
            set
            {
                socket_ = value;
            }
        }
        private IPHostEntry hostEntry_;
        public IPHostEntry hostEntry_p
        {
            get
            {
                return hostEntry_;
            }
            set
            {
                hostEntry_ = value;
            }
        }
        private string receiveInfo_ = "";
        public string receiveInfo_p
        {
            get
            {
                return receiveInfo_;
            }
            set
            {
                receiveInfo_ = value; 
            }
        }
        //ascii encoding
        private Encoding encoding_ascii = Encoding.ASCII;
        private Encoding encoding_ascii_p
        {
            get
            {
                return encoding_ascii;
            }
        }
        //base64编码source
        public static string encoding_base64(string source)
        {
            string encode;
            byte[] bytes = Encoding.Default.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);

            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        private string analyReceiveError()
        {
            if(string.IsNullOrEmpty(receiveInfo_p))
            {
                return Tip.ReceiveInfoEmpty;
            }
            return receiveInfo_p;
        }
        //socket 发送
        private int socketSend(string sendStr)
        {
            byte[ ] sendBuffer = encoding_ascii_p.GetBytes(sendStr);
            return socket_p.Send(sendBuffer);
        }
        //socket 接收
        private string receive()
        {
            byte[] receiveData = new byte[1024];
            int receiveLen = socket_p.Receive(receiveData);
            receiveInfo_p = encoding_ascii_p.GetString(receiveData, 0, receiveLen);
            return receiveInfo_p;
        }
        private class Tip//Easy to change other language tip.  
        {
            public static string AuthLoginFail = "Auth login fail!";
            public static string ConnectFail = "Connect mail host fail!";
            public static string IpIllegal = "Ip is illegal!";
            public static string MailFromFail = "Input sender address fail!";
            public static string PasswordLoginFail = "Password login fail!";
            public static string PortIllegal = "Port is illegal!";
            public static string ReceiveInfoEmpty = "Receive info is empty!";
            public static string RcptToFail = "Input receiver address fail!";
            public static string SenderIllegal = "Sender mail address is illegal!";
            public static string SendMailFail = "Send mail fail!";
            public static string ServerReadyFail = "Server ready fail!";
            public static string UserNameLoginFail = "User name login fail!";
            public static string VerifyFail = "Verify identity fail!";
        }
        public  bool  Login(string ServerName, int port, string username, string password)
        {
            try
            {
                exception_ = null;
                GetSocket(ServerName, port);
                if(!isServerReady())
                {
                    exception_p = new Exception(Tip.ServerReadyFail + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                if(!verify())
                {
                    exception_p = new Exception(Tip.VerifyFail +"\nError Message:"+ analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                if(!authLogin())
                {
                    exception_p = new Exception(Tip.AuthLoginFail + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                if(!UserNameLogin(username))
                {
                    exception_p = new Exception(Tip.UserNameLoginFail + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                if(!PasswordLogin(password))
                {
                    exception_p = new Exception(Tip.PasswordLoginFail + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                socket_p.Close();
                return true;

            }
            catch(SocketException e)
            {
                MessageBox.Show("异常消息：{0} ",e.Message);
            }
            return true;
        }
        public bool isServerReady()
        {
            string back = receive();

            return back.Substring(0, 3).Equals(LoginBacks.ServerReadyOK);
        }
        public bool verify()
        {
            socketSend(LoginCommand.Verify);
            string[] VerifyBacks = receive().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 0; index < VerifyBacks.Length; index++)
            {
                string VerifyBack = VerifyBacks[index];
                if (VerifyBack.Length <= 3)
                {
                    return false;
                }
                if (!VerifyBack.Substring(0,3).Equals(LoginBacks.OperateComplete))
                {
                    return false;
                }
            }
            return true;
        }
        private bool authLogin()
        {
            socketSend(LoginCommand.AuthLogin);
            string authLoginBack = receive();
            if (string.IsNullOrEmpty(authLoginBack) || authLoginBack.Length <= 3)
            {
                return false;
            }
            return authLoginBack.Substring(0, 3).Equals(LoginBacks.AuthLoginOK);
        }
        private bool UserNameLogin(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return false;
            }
            string UserNameBase64 = encoding_base64(UserName);
            socketSend(UserNameBase64 + "\r\n");
            string UserNameLoginBack = receive();
            if (string.IsNullOrEmpty(UserNameLoginBack) ||UserNameLoginBack.Length < 3)
            {
                return false;
            }
            return UserNameLoginBack.Substring(0, 3).Equals(LoginBacks.UsernameLoginOK);
        }
        private bool PasswordLogin(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }
            string PasswordBase64 = encoding_base64(Password);
            socketSend(PasswordBase64 + "\r\n");
            string PasswordLoginBack = receive();
            if (string.IsNullOrEmpty(PasswordLoginBack) || PasswordLoginBack.Length < 3)
            {
                return false;
            }
            return PasswordLoginBack.Substring(0, 3).Equals(LoginBacks.PasswordLoginOK);
        }
        public  Socket GetSocket(string ServerName,int port)
        {
            //Socket tc = null;
            //IPHostEntry用来存DNS的查询结果，包括一串IP地址，通过IPHostEntry对象，可以获取本地或远程主机的相关IP地址。
            IPHostEntry hostEntry = null;
            hostEntry = GetHostEntry(ServerName);
            if(hostEntry != null)
            {
                foreach(IPAddress address in hostEntry.AddressList)
                {
                    socket_p = TryGetSocket(address, port);
                    if(socket_p != null) { break; }
                }
            }
            return socket_p;
        }
        private Socket TryGetSocket(IPAddress address, int port)
        {
            //IPEndPoint对象用来表示一个特定的IP地址和端口的组合
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tc = null;
            try
            {
                //建立TCP连接
                tc = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tc.Connect(ipe);
                if(tc.Connected == false)
                {
                    MessageBox.Show(Tip.ConnectFail);
                    /*tc.ReceiveTimeout = 1000;
                    tc.SendBufferSize = 8192;
                    tc.ReceiveBufferSize = 8192;*/
                }
            }
            catch
            {
                tc = null;
                MessageBox.Show("连接失败！");
            }
            return tc;
        }

        private IPHostEntry GetHostEntry(String ServerName)
        {
			try
			{
				return Dns.GetHostEntry(ServerName);
			}
			catch 
			{
				MessageBox.Show("解析主机名参数出错，请检查主机名格式是否正确。");
			}
            return null;
        }

       
    }

}
