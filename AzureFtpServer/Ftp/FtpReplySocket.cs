using System.Net.Sockets;
using System.Text;
using AzureFtpServer.General;

namespace AzureFtpServer.Ftp
{
   
    internal class FtpReplySocket
    {
        private TcpClient m_theSocket;

        public FtpReplySocket(FtpConnectionObject connectionObject)
        {
            m_theSocket = OpenSocket(connectionObject);
        }

        public bool Loaded
        {
            get { return m_theSocket != null; }
        }

        public void Close()
        {
            SocketHelpers.Close(m_theSocket);
            m_theSocket = null;
        }

        public bool Send(byte[] abData, int nSize)
        {
            return SocketHelpers.Send(m_theSocket, abData, 0, nSize);
        }

        public bool Send(char[] abChars, int nSize)
        {
            return SocketHelpers.Send(m_theSocket, Encoding.ASCII.GetBytes(abChars), 0, nSize);
        }

        public bool Send(string sMessage)
        {
            //byte[] abData = Encoding.ASCII.GetBytes(sMessage);
            byte[] abData = Encoding.GetEncoding("gbk").GetBytes(sMessage);
            return Send(abData, abData.Length);
        }

        public int Receive(byte[] abData)
        {
            return m_theSocket.GetStream().Read(abData, 0, abData.Length);
        }

        private static TcpClient OpenSocket(FtpConnectionObject connectionObject)
        {
            TcpClient socketPasv = connectionObject.PasvSocket;

            if (socketPasv != null)
            {
                connectionObject.PasvSocket = null;
                return socketPasv;
            }

            return SocketHelpers.CreateTcpClient(connectionObject.PortCommandSocketAddress,
                                                 connectionObject.PortCommandSocketPort);
        }
    }
}