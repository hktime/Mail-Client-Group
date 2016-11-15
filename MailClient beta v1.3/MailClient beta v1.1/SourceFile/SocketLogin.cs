using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MailClient_beta_v1._1.SourceFile
{
    class SocketLogin
    {
        private sealed class LoginBacks
        {
            public const string ServerReadyOK = "220";
            public const string OperateComplete = "250";
            public const string AuthLoginOK = "334";
            public const string UsernameLoginOK = "334";
            public const string PasswordLoginOK = "235";
            public const string TLSReadyOK = "220";

        }
        private sealed class LoginCommand
        {
            public const string Verify = "ehlo hello\r\n";
            public const string EnableSSL = "starttls\r\n";
            public const string AuthLogin = "auth login\r\n";
        }

        private sealed class Pop3_BACK
        {
            public const string OK = "+OK";
            public const string ERROR = "-ERR";
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
        private SslStream ssl_;
        public SslStream ssl_p
        {
            get
            {
                return ssl_;
            }
            set
            {
                ssl_ = value;
            }
        }
        private bool CheckBoxState_ = false;
        public bool CheckBoxState_p
        {
            get
            {
                return CheckBoxState_;
            }
            set
            {
                CheckBoxState_ = value;
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
            if (string.IsNullOrEmpty(receiveInfo_p))
            {
                return Tip.ReceiveInfoEmpty;
            }
            return receiveInfo_p;
        }
        //socket 发送
        private int socketSend(string sendStr)
        {
            byte[] sendBuffer = encoding_ascii_p.GetBytes(sendStr);
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
        //SSL发送
        private void sslSend(string sendStr)
        {
            byte[] sendBuffer = encoding_ascii_p.GetBytes(sendStr);
            ssl_p.Write(sendBuffer);
            //ssl_p.Flush();
        }
        //在SSL通讯过程中，接受服务器发过来的消息并解密
        private string ReadMessage()
        {
            byte[] receiveData = new byte[1024];
            int bytes = -1;
            try
            {
                if (ssl_p.CanRead)
                {
                    bytes = ssl_p.Read(receiveData, 0, receiveData.Length);
                    receiveInfo_p = encoding_ascii_p.GetString(receiveData, 0, receiveData.Length);
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
            return receiveInfo_p;
        }
        private class Tip//Some tips.  
        {
            public static string AuthLoginFail = "Auth login fail !";
            public static string ConnectFail = "Connect mail host fail !";
            public static string IpIllegal = "Ip is illegal !";
            public static string ReceiveInfoEmpty = "Receive info is empty!";
            public static string PasswordLoginFail = "Password login fail !";
            public static string PortIllegal = "Port is illegal !";
            public static string ServerReadyFail = "Server ready fail !";
            public static string UserNameLoginFail = "User name login fail !";
            public static string VerifyFail = "Verify identity fail !";
            public static string HostIllegal = "Host is illegal !";
            public static string FileWriteFail = "Save username and password fail !";
        }
        private static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            MessageBox.Show("Certificate error: " + sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
        private bool GetSsl(string ServerName)
        {
            try
            {
                ssl_p = new SslStream(new NetworkStream(socket_p), true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                ssl_p.AuthenticateAsClient(ServerName);
                ssl_p.ReadTimeout = 5000;
                ssl_p.WriteTimeout = 5000;
                return true;
            }
            catch (SocketException e)
            {
                MessageBox.Show("建立SSL连接失败，错误原因为：" + e.Message);
                return false;
            }
        }
        public bool Login(string ServerName, int port, string username, string password, bool CheckBoxState)
        {
            try
            {
                CheckBoxState_p = CheckBoxState;
                exception_p = new Exception("Error Message: failed,Check the username !");
                if (GetSocket(ServerName, port) == null)
                {
                    return false;
                }
                if (CheckBoxState_p)
                {
                    if (!GetSsl(ServerName))
                    {
                        return false;
                    }
                }
                if (!isServerReady())
                {
                    exception_p = new Exception(Tip.ServerReadyFail + "\nError Message:" + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                if (!UserNameLogin(username))
                {
                    exception_p = new Exception(Tip.UserNameLoginFail + "\nError Message:" + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                if (!PasswordLogin(password))
                {
                    exception_p = new Exception(Tip.PasswordLoginFail + "\nError Message:" + analyReceiveError());
                    socket_p.Close();
                    return false;
                }
                socket_p.Close();
                return true;

            }
            catch (SocketException e)
            {
                MessageBox.Show("异常消息：{0} ", e.Message);
            }
            return true;
        }
        public bool isServerReady()
        {
            if (CheckBoxState_p == true)
            {
                string back = ReadMessage();
                if (string.IsNullOrEmpty(back) || back.Length < 3)
                {
                    return false;
                }
                return back.Substring(0, 3).Equals(Pop3_BACK.OK);
            }
            else
            {
                string back = receive();
                if (string.IsNullOrEmpty(back) || back.Length < 3)
                {
                    return false;
                }
                return back.Substring(0, 3).Equals(Pop3_BACK.OK);
            }

        }
        private bool UserNameLogin(string UserName)
        {
            if (CheckBoxState_p == true)
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    receiveInfo_p = "Username is null !";
                    return false;
                }
                sslSend("user " + UserName + "\r\n");
                string sslUserNameBack = ReadMessage();
                if (string.IsNullOrEmpty(sslUserNameBack) || sslUserNameBack.Length < 3)
                {
                    return false;
                }
                return sslUserNameBack.Substring(0, 3).Equals(Pop3_BACK.OK);
            }
            else
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    receiveInfo_p = "Username is null !";
                    return false;
                }
                socketSend("user " + UserName + "\r\n");
                string UserNameLoginBack = receive();
                if (string.IsNullOrEmpty(UserNameLoginBack) || UserNameLoginBack.Length < 3)
                {
                    return false;
                }
                return UserNameLoginBack.Substring(0, 3).Equals(Pop3_BACK.OK);
            }
        }
        private bool PasswordLogin(string Password)
        {
            if (CheckBoxState_p == true)
            {
                if (string.IsNullOrEmpty(Password))
                {
                    receiveInfo_p = "Password is null !";
                    return false;
                }
                string PasswordBase64 = encoding_base64(Password);
                sslSend("pass " + Password + "\r\n");
                string sslPasswordBack = ReadMessage();
                if (string.IsNullOrEmpty(sslPasswordBack) || sslPasswordBack.Length <= 3)
                {
                    return false;
                }
                return sslPasswordBack.Substring(0, 3).Equals(Pop3_BACK.OK);
            }
            else
            {
                if (string.IsNullOrEmpty(Password))
                {
                    receiveInfo_p = "Password is null !";
                    return false;
                }
                socketSend("pass " + Password + "\r\n");
                string PasswordLoginBack = receive();
                if (string.IsNullOrEmpty(PasswordLoginBack) || PasswordLoginBack.Length < 3)
                {
                    return false;
                }
                return PasswordLoginBack.Substring(0, 3).Equals(Pop3_BACK.OK);

            }

        }
        private Socket GetSocket(string ServerName, int port)
        {
            //Socket tc = null;
            //IPHostEntry用来存DNS的查询结果，包括一串IP地址，通过IPHostEntry对象，可以获取本地或远程主机的相关IP地址。
            IPHostEntry hostEntry = null;
            hostEntry = GetHostEntry(ServerName);
            if (hostEntry != null)
            {
                foreach (IPAddress address in hostEntry.AddressList)
                {
                    socket_p = TryGetSocket(address, port);
                    if (socket_p != null) { break; }
                }
            }
            else
            {
                return null;
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
                if (tc.Connected == false)
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
                MessageBox.Show("解析主机名参数出错，请检查用户名格式是否正确。");
            }
            return null;
        }
        //通过用户名匹配主机名
        public string HostNameReg(string username)
        {
            //匹配@及之后的内容
            string pattern = @"@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14}";
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            Match MatchText;
            string value;
            string HostName;
            try
            {
                MatchText = reg.Match(username);
                value = MatchText.Value;
                //去除@
                HostName = value.Remove(0, 1);
                HostName = string.Concat("pop.", HostName);
                return HostName;
            }
            catch
            {
                MessageBox.Show(Tip.HostIllegal + "\nError Message:" + "Please check the username!");
                return null;
            }

        }
        public bool FileWrite(string username, string password)
        {
            string MainDirec = @"MailClient\\" + username + "\\users";
            string UnPath = @MainDirec + "\\Username.txt";
            string PsPath = @MainDirec + "\\Password.txt";
            try
            {
                //Try to create the directory
                if (!Directory.Exists(MainDirec))
                {
                    DirectoryInfo di = Directory.CreateDirectory(MainDirec);
                }
                using (StreamWriter sw = File.CreateText(UnPath))
                {
                    sw.WriteLine(username);
                }
                using (StreamWriter sw = File.CreateText(PsPath))
                {
                    sw.WriteLine(password);
                    return true;
                }
            }
            catch (IOException e)
            {
                exception_p = new Exception(Tip.FileWriteFail + "\nError Message:" + e.Message);
                return false;
            }

        }
    }
}

