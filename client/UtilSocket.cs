using System;
using System.Net;
using System.Net.Sockets;

using Structures;

namespace SocketSystem {
    //========================================================================
    /// <summary> This class abstracts a socket </summary>
    public class CSocketClient {
    // Delegate Method Types
        /// <summary> DelType: Called when a message is received </summary>
        public delegate void MESSAGE_HANDLER(CSocketClient pSocket, Int32 iNumberOfBytes);

        /// <summary> DelType: Called when a connection is closed </summary>
        public delegate void CLOSE_HANDLER(CSocketClient pSocket);

        /// <summary> DelType: Called when a socket error occurs </summary>
        public delegate void ERROR_HANDLER(CSocketClient pSocket, Exception pException);


    // Private Properties
        private NetworkStream m_pNetworkStream;
        /// <summary> RefType: A network stream object </summary>
        private NetworkStream GetNetworkStream {get{return m_pNetworkStream;} set{m_pNetworkStream = value;}}

        private TcpClient m_pTcpClient;
        /// <summary> RefType: A TcpClient object for socket connection </summary>
        private TcpClient GetTcpClient {get{return m_pTcpClient;} set{m_pTcpClient = value;}}

        private AsyncCallback m_pCallbackReadMethod;
        /// <summary> RetType: A callback object for processing recieved socket data </summary>
        private AsyncCallback GetCallbackReadMethod {get{return m_pCallbackReadMethod;} set{m_pCallbackReadMethod = value;}}

        private AsyncCallback m_pCallbackWriteMethod;
        /// <summary> RetType: A callback object for processing send socket data </summary>
        private AsyncCallback GetCallbackWriteMethod {get{return m_pCallbackWriteMethod;} set{m_pCallbackWriteMethod = value;}}

        private MESSAGE_HANDLER m_pfnMessageHandler;
        /// <summary> DelType: A reference to a user supplied function to be called when a socket message arrives </summary>
        private MESSAGE_HANDLER GetMessageHandler {get{return m_pfnMessageHandler;} set{m_pfnMessageHandler = value;}}

        private CLOSE_HANDLER m_pfnCloseHandler;
        /// <summary> DelType: A reference to a user supplied function to be called when a socket connection is closed </summary>
        private CLOSE_HANDLER GetCloseHandler {get{return m_pfnCloseHandler;} set{m_pfnCloseHandler = value;}}

        private ERROR_HANDLER m_pfnErrorHandler;
        /// <summary> DelType: A reference to a user supplied function to be called when a socket error occurs </summary>
        private ERROR_HANDLER GetErrorHandler {get{return m_pfnErrorHandler;} set{m_pfnErrorHandler = value;}}

        private Boolean m_bDisposeFlag;
        /// <summary> SimType: Flag to indicate if the class has been disposed </summary>
        private Boolean IsDisposed {get{return m_bDisposeFlag;} set{m_bDisposeFlag = value;}}

    // Public Properties
        private String m_strIpAddress;
        /// <summary> RefType: The IpAddress the client is connect to </summary>
        public String GetIpAddress {get{return m_strIpAddress;} set{m_strIpAddress = value;}}

        private Int16 m_iPort;
        /// <summary> SimType: The Port to either connect to or listen on </summary>
        public Int16 GetPort {get{return m_iPort;} set{m_iPort = value;}}

        private Object m_pUserArg;
        /// <summary> RefType: A reference to a user defined object </summary>
        public Object GetUserArg {get{return m_pUserArg;} set{m_pUserArg = value;}}

        private Byte[] m_bytRawBuffer;
        /// <summary> SimType: A raw buffer to capture data comming off the socket </summary>
        public Byte[] GetRawBuffer {get{return m_bytRawBuffer;} set{m_bytRawBuffer = value;}}

        private Int32 m_iSizeOfRawBuffer;
        /// <summary> SimType: Size of the raw buffer for received socket data </summary>
        public Int32 GetSizeOfRawBuffer {get{return m_iSizeOfRawBuffer;} set{m_iSizeOfRawBuffer = value;}}

        private Int32 m_iSocketListIndex;
        /// <summary> SimType: Index into the Socket List Array </summary>
        public Int32 GetSocketListIndex {get{return m_iSocketListIndex;} set{m_iSocketListIndex = value;}}

        private Socket m_pClientSocket;
        /// <summary> RefType: The socket for the client connection </summary>
        public Socket GetClientSocket {get{return m_pClientSocket;} set{m_pClientSocket = value;}}

        // Constructor, Finalize, Dispose
        //********************************************************************
        /// <summary> Constructor for client support </summary>
        /// <param name="iSizeOfRawBuffer"> SimType: The size of the raw buffer </param>
        /// <param name="pUserArg"> RefType: A Reference to the Users arguments </param>
        /// <param name="pfnMessageHandler"> DelType: Reference to the user defined message handler method </param>
        /// <param name="pfnCloseHandler"> DelType: Reference to the user defined close handler method </param>
        /// <param name="pfnErrorHandler"> DelType: Reference to the user defined error handler method </param>
        public CSocketClient(Int32 iSizeOfRawBuffer, Object pUserArg,
            MESSAGE_HANDLER pfnMessageHandler, CLOSE_HANDLER pfnCloseHandler, ERROR_HANDLER pfnErrorHandler) {
            LogLib.WriteLine("Entering in CSocketClient.CSocketClient()", LogLevel.Trace);
            // Create the raw buffer
            GetSizeOfRawBuffer = iSizeOfRawBuffer;
            GetRawBuffer       = new Byte[GetSizeOfRawBuffer];
      
            // Save the user argument
            GetUserArg = pUserArg;

            // Set the handler methods
            GetMessageHandler = pfnMessageHandler;
            GetCloseHandler   = pfnCloseHandler;
            GetErrorHandler   = pfnErrorHandler;
      
            // Set the async socket method handlers
            GetCallbackReadMethod  = new AsyncCallback(ReceiveComplete);
            GetCallbackWriteMethod = new AsyncCallback(SendComplete);

            // Init the dispose flag
            IsDisposed = false;

            LogLib.WriteLine("Exiting in CSocketClient.CSocketClient()", LogLevel.Trace);
        }

        //*******************************************************************
        /// <summary> Finialize </summary>
        ~CSocketClient() {
            if (!IsDisposed)
                Dispose();
        }
        //********************************************************************
        /// <summary> Dispose </summary>
        public void Dispose() {
            LogLib.WriteLine("Entering in CSocketClient.Dispose()", LogLevel.Trace);
            try {
                // Flag that dispose has been called
                IsDisposed = true;
        
                // Disconnect the client from the server
                Disconnect();
            }
            catch (Exception ex) {LogLib.WriteLine("Error in CSocketClient.Dispose(): ", ex);}
      
            LogLib.WriteLine("Exiting in CSocketClient.Dispose()", LogLevel.Trace);
        }

    // Private Methods
        //********************************************************************
        /// <summary> Called when a message arrives </summary>
        /// <param name="ar"> RefType: An async result interface </param>
        private void ReceiveComplete(IAsyncResult ar) {
            LogLib.WriteLine("Entering in CSocketClient.ReceiveComplete()", LogLevel.Trace);
            try {
                // Is the Network Stream object valid
                if (GetNetworkStream.CanRead) {
                    // Read the current bytes from the stream buffer
                    Int32 iBytesRecieved = GetNetworkStream.EndRead(ar);
                           
                    // If there are bytes to process else the connection is lost
                    if (iBytesRecieved > 0) 
                    {
                        // A message came in send it to the MessageHandler
                        try {GetMessageHandler(this, iBytesRecieved);}
                        catch (Exception ex) {LogLib.WriteLine("Error with GetMessageHandler() in CSocketClient.ReceiveComplete(): ", ex);}
          
                        // Wait for a new message
                        Receive();
                    } else {
                        LogLib.WriteLine("CSocketClient.ReceiveComplete(): Shuting Down", LogLevel.Error);
                        throw new Exception("Shut Down");
                    }
                }
            }
 
            catch (Exception) {
                // The connection must have dropped call the CloseHandler
                try {GetCloseHandler(this);}
                catch (Exception ex) {LogLib.WriteLine("Error in CSocketClient.ReceiveComplete(): ", ex);}
 
                // Dispose of the class
                Dispose();
            }

            LogLib.WriteLine("Exiting in CSocketClient.ReceiveComplete()", LogLevel.Trace);
        }

        //********************************************************************
        /// <summary> Called when a message is sent </summary>
        /// <param name="ar"> RefType: An async result interface </param>
        private void SendComplete(IAsyncResult ar) {
            LogLib.WriteLine("Entering in CSocketClient.SendComplete()", LogLevel.Trace);
            try {
                // Is the Network Stream object valid
                if (GetNetworkStream.CanWrite) {
                    GetNetworkStream.EndWrite(ar);
                    LogLib.WriteLine("CSocketClient.SendComplete(): GetNetworkStream.EndWrite()", LogLevel.Debug);
                }
            }
            catch (Exception ex) {LogLib.WriteLine("Error in CSocketClient.SendComplete(): ", ex);}

            LogLib.WriteLine("Exiting in CSocketClient.SendComplete()", LogLevel.Trace);
        }

    // Public Methods
        //********************************************************************
        /// <summary> Function used to connect to a server </summary>
        /// <param name="strIpAddress"> RefType: The address to connect to </param>
        /// <param name="iPort"> SimType: The Port to connect to </param>
        public void Connect(String strIpAddress, Int16 iPort) {
            LogLib.WriteLine("Entering in CSocketClient.Connect()", LogLevel.Trace);
            try {
                if (GetNetworkStream == null) {
                    // Set the Ipaddress and Port
                    GetIpAddress = strIpAddress;
                    GetPort      = iPort;
 
                    // Attempt to establish a connection
                    GetTcpClient     = new TcpClient(GetIpAddress, GetPort);
                    GetNetworkStream = GetTcpClient.GetStream();
          
                    // Set these socket options
                    GetTcpClient.ReceiveBufferSize = 1048576;
                    GetTcpClient.SendBufferSize    = 1048576;
                    GetTcpClient.NoDelay           = true;
                    GetTcpClient.LingerState       = new System.Net.Sockets.LingerOption(false,0);
          
                    // Start to receive messages
                    Receive();
                }
            }
      
            catch (System.Net.Sockets.SocketException e) {throw new Exception(e.Message, e.InnerException);}

            LogLib.WriteLine("Entering in CSocketClient.Connect()", LogLevel.Trace);
        }
        //********************************************************************
        /// <summary> Function used to disconnect from the server </summary>
        public void Disconnect() {
            LogLib.WriteLine("Entering in CSocketClient.Disconnect()", LogLevel.Trace);

            // Close down the connection
            if (GetNetworkStream != null)
                GetNetworkStream.Close();
      
            if (GetTcpClient != null)
                GetTcpClient.Close();
 
            if (GetClientSocket != null)
                GetClientSocket.Close();
      
            // Clean up the connection state
            GetClientSocket  = null;
            GetNetworkStream = null;
            GetTcpClient     = null;

            LogLib.WriteLine("Exiting in CSocketClient.Disconnect()", LogLevel.Trace);
        }

        //********************************************************************
        /// <summary> Function to send a string to the server </summary>
        /// <param name="strMessage"> RefType: A string to send </param>
        public void Send(String strMessage) {
            LogLib.WriteLine("Entering in CSocketClient.Send(String): " + strMessage, LogLevel.Trace);

            if ((GetNetworkStream != null) && (GetNetworkStream.CanWrite)) {
                // Convert the string into a Raw Buffer
                Byte[] pRawBuffer = System.Text.Encoding.ASCII.GetBytes(strMessage);
 
                // Issue an asynchronus write
                GetNetworkStream.BeginWrite(pRawBuffer, 0, pRawBuffer.GetLength(0), GetCallbackWriteMethod, null);
            }
            else
                LogLib.WriteLine("Error in CSocketClient.Send(string): Socket Closed");

            LogLib.WriteLine("Exiting in CSocketClient.Send(String): " + strMessage, LogLevel.Trace);
        }
        //********************************************************************
        /// <summary> Function to send a raw buffer to the server </summary>
        /// <param name="pRawBuffer"> RefType: A Raw buffer of bytes to send </param>
        public void Send(Byte[] pRawBuffer) {
            LogLib.WriteLine("Entering in CSocketClient.Send(Byte[])", LogLevel.Trace);
            if ((GetNetworkStream != null) && (GetNetworkStream.CanWrite)) {
                // Issue an asynchronus write
                GetNetworkStream.BeginWrite(pRawBuffer, 0, pRawBuffer.GetLength(0), GetCallbackWriteMethod, null);
            }
            else
                LogLib.WriteLine("Error in CSocketClient.Send(Byte[]): Socket Closed");

            LogLib.WriteLine("Exiting in CSocketClient.Send(Byte[])", LogLevel.Trace);
        }
        //********************************************************************
        /// <summary> Wait for a message to arrive </summary>
        public void Receive() {
            LogLib.WriteLine("Entering in CSocketClient.Receive()", LogLevel.Trace);
            if ((GetNetworkStream != null) && (GetNetworkStream.CanRead)) {
                // Issue an asynchronous read
                GetNetworkStream.BeginRead(GetRawBuffer, 0, GetSizeOfRawBuffer, GetCallbackReadMethod, null);
            }
            else
                LogLib.WriteLine("Error in CSocketClient.Receive(): Socket Closed");

            LogLib.WriteLine("Exiting in CSocketClient.Receive()", LogLevel.Trace);
        }

    } // End of CSocketClient

} // End of namespace SocketSystem

