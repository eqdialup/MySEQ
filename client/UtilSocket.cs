using System;
using System.Net.Sockets;

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
        private NetworkStream _networkStream;

        /// <summary> RefType: A TcpClient object for socket connection </summary>
        private TcpClient _TcpClient;

        /// <summary> RetType: A callback object for processing recieved socket data </summary>
        private readonly AsyncCallback _callbackReadMethod;

        /// <summary> RetType: A callback object for processing send socket data </summary>
        private readonly AsyncCallback _callbackWriteMethod;

        /// <summary> DelType: A reference to a user supplied function to be called when a socket message arrives </summary>
        private readonly MESSAGE_HANDLER _messageHandler;

        /// <summary> DelType: A reference to a user supplied function to be called when a socket connection is closed </summary>
        private readonly CLOSE_HANDLER _closeHandler;

        /// <summary> SimType: A raw buffer to capture data comming off the socket </summary>
        public byte[] RawBuffer { get; set; }

        /// <summary> SimType: Size of the raw buffer for received socket data </summary>
        public int SizeOfRawBuffer { get; set; }

        // Constructor, Finalize, Dispose
        //********************************************************************
        /// <summary> Constructor for client support </summary>
        /// <param name="iSizeOfRawBuffer"> SimType: The size of the raw buffer </param>
        /// <param name="messageHandler"> DelType: Reference to the user defined message handler method </param>
        /// <param name="closeHandler"> DelType: Reference to the user defined close handler method </param>
        public CSocketClient(int iSizeOfRawBuffer,
            MESSAGE_HANDLER messageHandler, CLOSE_HANDLER closeHandler)
        {
            SizeOfRawBuffer = iSizeOfRawBuffer;
            RawBuffer = new byte[iSizeOfRawBuffer];
            _messageHandler = messageHandler;
            _closeHandler = closeHandler;
            _callbackReadMethod = new AsyncCallback(ReceiveComplete);
            _callbackWriteMethod = new AsyncCallback(SendComplete);
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
                if (_networkStream.CanRead)
                {
                    // Read the current bytes from the stream buffer
                    var iBytesRecieved = _networkStream.EndRead(ar);

                    // If there are bytes to process else the connection is lost
                    if (iBytesRecieved > 0)
                    {
                        // A message came in send it to the MessageHandler
                        try { _messageHandler(this, iBytesRecieved); }
                        catch (Exception ex) { LogLib.WriteLine("Error GetMessageHandler - CSocketClient.ReceiveComplete(): ", ex); }

                        // Wait for a new message
                        Receive();
                    }
                    else
                    {
                        _closeHandler(this);
                        LogLib.WriteLine("CSocketClient.ReceiveComplete(): Shuting Down", LogLevel.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error CSocketClient.ReceiveComplete(): ", ex);
                _closeHandler(this);
            }
        }

        //********************************************************************
        /// <summary> Called when a message is sent </summary>
        /// <param name="ar"> RefType: An async result interface </param>
        private void SendComplete(IAsyncResult ar) => _networkStream.EndWrite(ar);

        // Public Methods
        //********************************************************************
        /// <summary> Function used to connect to a server </summary>
        /// <param name="server"> RefType: The address to connect to </param>
        /// <param name="iPort"> SimType: The Port to connect to </param>
        public void Connect(string server, int iPort)
        {
            if (_networkStream != null)
            {
                return;
            }
            // Attempt to establish a connection
            _TcpClient = new TcpClient(server, iPort);
            _networkStream = _TcpClient.GetStream();

            // Set these socket options
            _TcpClient.ReceiveBufferSize = cbufferSize;
            _TcpClient.SendBufferSize = cbufferSize;
            _TcpClient.NoDelay = true;

            Receive();
        }

        //********************************************************************
        /// <summary> Function used to disconnect from the server </summary>
        public void Disconnect()
        {
            // Close down the connection
            _networkStream?.Close();
            _TcpClient?.Close();
        }

        /// <summary> Function to send a raw buffer to the server </summary>
        /// <param name="data"> RefType: A Raw buffer of bytes to send </param>
        public void Send(byte[] data)
        {
            if (_networkStream.CanWrite)
            {
                // Issue an asynchronus write
                _networkStream.BeginWrite(data, 0, data.GetLength(0), _callbackWriteMethod, null);
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
            if (_networkStream.CanRead)
            {
                // Issue an asynchronous read
                _networkStream.BeginRead(RawBuffer, 0, SizeOfRawBuffer, _callbackReadMethod, null);
            }
            else
            {
                LogLib.WriteLine("Error CSocketClient.Receive(): Socket Closed");
            }
        }
    }
}