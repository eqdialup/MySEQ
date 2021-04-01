using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Structures
{
    //========================================================================
    /// <summary> This class abstracts a socket </summary>
    public class CSocketClient
    {
        private const int cbufferSize = 1048576;

        // Delegate Method Types
        /// <summary> DelType: Called when a message is received </summary>
        public delegate void MESSAGE_HANDLER(CSocketClient pSocket, int iNumberOfBytes);

        /// <summary> DelType: Called when a connection is closed </summary>
        public delegate void CLOSE_HANDLER(CSocketClient pSocket);

        /// <summary> RefType: A network stream object </summary>
        private NetworkStream GetNetworkStream { get; set; }

        /// <summary> RefType: A TcpClient object for socket connection </summary>
        private TcpClient GetTcpClient { get; set; }

        /// <summary> RetType: A callback object for processing recieved socket data </summary>
        private AsyncCallback GetCallbackReadMethod { get; }

        /// <summary> RetType: A callback object for processing send socket data </summary>
        private AsyncCallback GetCallbackWriteMethod { get; }

        /// <summary> DelType: A reference to a user supplied function to be called when a socket message arrives </summary>
        private MESSAGE_HANDLER GetMessageHandler { get; }

        /// <summary> DelType: A reference to a user supplied function to be called when a socket connection is closed </summary>
        private CLOSE_HANDLER GetCloseHandler { get; }

        /// <summary> SimType: A raw buffer to capture data comming off the socket </summary>
        public byte[] GetRawBuffer { get; set; }

        /// <summary> SimType: Size of the raw buffer for received socket data </summary>
        public int GetSizeOfRawBuffer { get; set; }

        // Constructor, Finalize, Dispose
        //********************************************************************
        /// <summary> Constructor for client support </summary>
        /// <param name="iSizeOfRawBuffer"> SimType: The size of the raw buffer </param>
        /// <param name="pfnMessageHandler"> DelType: Reference to the user defined message handler method </param>
        /// <param name="pfnCloseHandler"> DelType: Reference to the user defined close handler method </param>
        public CSocketClient(int iSizeOfRawBuffer,
            MESSAGE_HANDLER pfnMessageHandler, CLOSE_HANDLER pfnCloseHandler)
        {
            // Create the raw buffer
            GetSizeOfRawBuffer = iSizeOfRawBuffer;
            GetRawBuffer = new byte[iSizeOfRawBuffer];

            // Set the handler methods
            GetMessageHandler = pfnMessageHandler;
            GetCloseHandler = pfnCloseHandler;

            // Set the async socket method handlers

            GetCallbackReadMethod = new AsyncCallback(ReceiveComplete);
            GetCallbackWriteMethod = new AsyncCallback(SendComplete);
        }
        // Private Methods
        //********************************************************************
        /// <summary> Called when a message arrives </summary>
        /// <param name="ar"> RefType: An async result interface </param>
        private void ReceiveComplete(IAsyncResult ar)
        {
            try
            {
                // Is the Network Stream object valid
                if (GetNetworkStream.CanRead)
                {
                    // Read the current bytes from the stream buffer
                    var iBytesRecieved = GetNetworkStream.EndRead(ar);

                    // If there are bytes to process else the connection is lost
                    if (iBytesRecieved > 0)
                    {
                        // A message came in send it to the MessageHandler
                        try { GetMessageHandler(this, iBytesRecieved); }
                        catch (Exception ex) { LogLib.WriteLine("Error GetMessageHandler - CSocketClient.ReceiveComplete(): ", ex); }

                        // Wait for a new message
                        Receive();
                    }
                    else
                    {
                        LogLib.WriteLine("CSocketClient.ReceiveComplete(): Shuting Down", LogLevel.Error);
                        throw new SocketException();
                    }
                }
            }
            catch (Exception)
            {
                // The connection must have dropped call the CloseHandler
                try { GetCloseHandler(this); }
                catch (Exception ex) { LogLib.WriteLine("Error CSocketClient.ReceiveComplete(): ", ex); }
                Disconnect();
                // Dispose of the class
                //                Dispose();
            }
        }

        //********************************************************************
        /// <summary> Called when a message is sent </summary>
        /// <param name="ar"> RefType: An async result interface </param>
        private void SendComplete(IAsyncResult ar)
        {
            try
            {
                // Is the Network Stream object valid
                if (GetNetworkStream.CanWrite)
                {
                    GetNetworkStream.EndWrite(ar);
                    LogLib.WriteLine("CSocketClient.SendComplete(): GetNetworkStream.EndWrite()", LogLevel.Debug);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error CSocketClient.SendComplete(): ", ex); }
        }

        // Public Methods
        //********************************************************************
        /// <summary> Function used to connect to a server </summary>
        /// <param name="strIpAddress"> RefType: The address to connect to </param>
        /// <param name="iPort"> SimType: The Port to connect to </param>
        public void Connect(string strIpAddress, int iPort)
        {
            if (GetNetworkStream != null)
            {
                return;
            }
            // Attempt to establish a connection
            GetTcpClient = new TcpClient(strIpAddress, iPort);
            GetNetworkStream = GetTcpClient.GetStream();

            // Set these socket options
            GetTcpClient.ReceiveBufferSize = cbufferSize;
            GetTcpClient.SendBufferSize = cbufferSize;
            GetTcpClient.NoDelay = true;

            Receive();
        }

        //********************************************************************
        /// <summary> Function used to disconnect from the server </summary>
        public void Disconnect()
        {
            // Close down the connection
            GetNetworkStream?.Close();
            GetTcpClient?.Close();
        }

        /// <summary> Function to send a raw buffer to the server </summary>
        /// <param name="pRawBuffer"> RefType: A Raw buffer of bytes to send </param>
        public void Send(byte[] pRawBuffer)
        {
            if (GetNetworkStream?.CanWrite == true)
            {
                // Issue an asynchronus write
                GetNetworkStream.BeginWrite(pRawBuffer, 0, pRawBuffer.GetLength(0), GetCallbackWriteMethod, null);
            }
            else
            {
                LogLib.WriteLine("Error CSocketClient.Send(Byte[]): Socket Closed");
            }
        }

        //********************************************************************
        /// <summary> Wait for a message to arrive </summary>
        public void Receive()
        {
            if (GetNetworkStream?.CanRead == true)
            {
                // Issue an asynchronous read
                GetNetworkStream.BeginRead(GetRawBuffer, 0, GetSizeOfRawBuffer, GetCallbackReadMethod, null);
            }
            else
            {
                LogLib.WriteLine("Error CSocketClient.Receive(): Socket Closed");
            }
        }
    }
}