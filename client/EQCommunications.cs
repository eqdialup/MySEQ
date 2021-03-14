// Class Files

using Structures;
using System;
using System.Threading;
using System.Windows.Forms;

namespace myseq

{
    [Flags]
    public enum RequestType
    {
        //Bit Flags determining what data to send to the client
        ZONE = 0x00000001,
        PLAYER = 0x00000002,
        TARGET = 0x00000004,
        MOBS = 0x00000008,
        GROUND_ITEMS = 0x00000010,
        GET_PROCESSINFO = 0x00000020,
        SET_PROCESSINFO = 0x00000040,
        WORLD = 0x00000080
    }

    public class EQCommunications

    {
        private const string ServConErr = "Server Connection Error";

        // Variables to store any incomplete packets till the next chunk arrives.

        private int incompleteCount = 0;

        private bool RequestPending;

        private CSocketClient pSocketClient;

        private bool update_hidden;

        private bool mbGetProcessInfo;

        public int newProcessID;

        private bool send_process;

        private int numPackets; // Total Packets expected

        private int numProcessed; // No. of Packets already processed        

        private byte[] incompletebuffer = new byte[2048];

        private EQData eq;

        private FrmMain f1; // TODO: get rid of this

        public void UpdateHidden()
        {
            update_hidden = true;
        }

        public EQCommunications(EQData eq,FrmMain f1)

        {
            this.eq = eq;

            this.f1 = f1;
        }

        public void StopListening()

        {
            try

            {
                RequestPending = false;

                numPackets = numProcessed = 0;

                pSocketClient?.Dispose();

                pSocketClient = null;
            }
            catch (Exception pException) {LogLib.WriteLine($"Error: StopListening: {pException.Message}");}
        }

        public bool ConnectToServer(string ServerAddress, int ServerPort, bool errMsg = true)

        {
            try {
                if (pSocketClient != null) {
                    Thread.Sleep(2000);

                    pSocketClient.Dispose();

                    pSocketClient = null;
                }

                // Instantiate a CSocketClient object

                pSocketClient = new CSocketClient(100000,
                    new CSocketClient.MESSAGE_HANDLER(MessageHandlerClient),
                    new CSocketClient.CLOSE_HANDLER(CloseHandler)
                    );

                // Establish a connection to the server
                mbGetProcessInfo = true;
                pSocketClient.Connect(ServerAddress,(short) ServerPort);

                return true;
            }
            catch (Exception pException)

            {
                string msg = $"{ServConErr} {pException.Message}";

                LogLib.WriteLine(msg);

                if (errMsg)
                {
                    MessageBox.Show(
                        msg
                        + "\r\nTry selecting a different server!",
                        caption: ServConErr,
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Error);
                }

                return false;
            }
        }

        //********************************************************************

        /// <summary> Called when a message is extracted from the socket </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        /// <param name="iNumberOfBytes"> The number of bytes in the RawBuffer inside the SocketClient </param>
        private void MessageHandlerClient(CSocketClient pSocket, int iNumberOfBytes)
        {
            // Process the packet

            try {ProcessPacket(pSocket.GetRawBuffer, iNumberOfBytes);}
            catch (Exception pException) {LogLib.WriteLine("Error: ProcessPacket: " + pException.Message);}
        }

        //********************************************************************

        /// <summary> Called when a socket connection is closed </summary>
        /// <param name="pSocket"> The SocketClient object the message came from </param>
        private void CloseHandler(CSocketClient pSocket)
        {
            try {
                if (f1 == null)
                    StopListening();
                else
                    f1.StopListening();
            }
            catch (Exception pException) {LogLib.WriteLine($"Error: CloseHandler: {pException.Message}");}
        }

        //********************************************************************

        private void SendData(byte[] data)
        {
            pSocketClient.Send(data);
        }

        public void Tick()
        {
            int Request;

            try
            {
                if (!RequestPending)
                {
                    if (newProcessID > 0 && !mbGetProcessInfo)
                    {
                        if (!send_process)
                        {
                            // We have a request to change the process

                            Request = (int)RequestType.SET_PROCESSINFO;
                            SendData(BitConverter.GetBytes(Request));
                            send_process = true;
                        }
                        else
                        {
                            SendData(BitConverter.GetBytes(newProcessID));
                            send_process = false;
                            newProcessID = 0;
                            mbGetProcessInfo = true;
                        }
                    }
                    else
                    {
                        RequestPending = true;
                        Request = (int)(RequestType.ZONE
                                        | RequestType.PLAYER
                                        | RequestType.TARGET
                                        | RequestType.MOBS
                                        | RequestType.GROUND_ITEMS
                                        | RequestType.WORLD);

                        if (mbGetProcessInfo && newProcessID == 0)
                        {
                            mbGetProcessInfo = false;
                            Request |= (int)RequestType.GET_PROCESSINFO;
                        }

                        SendData(BitConverter.GetBytes(Request));
                    }
                }
            }
            catch (Exception ex) {LogLib.WriteLine("Error: timPackets_Tick: ", ex);}
        }

        public void CharRefresh()
        {
            if (pSocketClient != null)
            {
                    mbGetProcessInfo = true;
            }
        }

        public void SwitchCharacter(ProcessInfo PI)
        {
            if (PI?.ProcessID > 0)
                newProcessID = PI.ProcessID;
        }

        public bool CanSwitchChars()
        {
            return (newProcessID == 0) && (!mbGetProcessInfo);
        }

        private void ProcessPacket(byte[] packet, int bytes)
        {
            int offset = 0;

            const int SIZE_OF_PACKET = 100; //104 on new server

            try {
                if (bytes > 0)

                {
                    // we have received some bytes, check if this is the beginning of a new packet or a chunk of an existing one.

                    if (numPackets == 0)

                    {
                        // The first word in the data stream is the number of packets

                        numPackets = BitConverter.ToInt32(packet, 0);
                        offset = 4;
                        f1.StartNewPackets();
                    }
                    else
                    {
                        // We havent finished processing packets, so check if we have any extra bytes stored in our incomplete buffer.

                        offset = -incompleteCount;
                    }

                    eq.BeginProcessPacket(); //clears spawn&ground arrays

                    for (; (offset + SIZE_OF_PACKET) <= bytes; offset += SIZE_OF_PACKET)

                    {
                        SPAWNINFO si = new SPAWNINFO();

                        if (offset < 0)

                        {
                            // copy the missing chunk of the incomplete packet to the incomplete packet buffer

                            try
                            {
                                PacketCopy(packet, SIZE_OF_PACKET);
                            }
                            catch (Exception ex) {LogLib.WriteLine("Error: ProcessPacket: Copy Incomplete packet buffer: " ,ex);}

                            incompleteCount = 0;

                            if (incompletebuffer.Length == 0) {
                                numPackets = 0;
                                break;
                            }

                            si.frombytes(incompletebuffer, 0);
                        }
                        else
                        {
                            si.frombytes(packet, offset);
                        }

                        numProcessed ++;
                        f1.ProcessPacket(si, update_hidden);
                    }

                    f1.ProcessSpawnList();
                    f1.ProcessGroundItemList();
                }
            }
            catch (Exception ex) {LogLib.WriteLine("Error: ProcessPacket: ", ex);}

            if (numProcessed < numPackets)
            {
                if (offset < bytes)
                {
                    // Copy unprocessed bytes into the incomplete buffer
                    IncompleteCopy(packet, bytes, offset);
                }
            }
            else
            {
                // Finished proceessing the request
                FinalizeProcess();

                f1.CheckMobs();
                f1.mapCon.Invalidate();
            }
        }

        private void FinalizeProcess()
        {
            RequestPending = false;
            if (update_hidden)
                update_hidden = false;
            numPackets = numProcessed = 0;

            incompleteCount = 0;
            // Make sure that the incomplete buffer is actually empty
            if (incompletebuffer.Length > 0)
            {
                for (int pp = 0; pp < incompletebuffer.Length; pp++)
                {
                    incompletebuffer[pp] = 0;
                }
            }
        }

        private void IncompleteCopy(byte[] packet, int bytes, int offset)
        {
            incompleteCount = bytes - offset;

            try
            {
                Array.Copy(packet, offset, incompletebuffer, 0, incompleteCount);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error: ProcessPacket(): Copy to Incomplete Buffer: ", ex);
                LogLib.WriteLine($"Packet Size: {packet.Length} Offset: {offset}");
                LogLib.WriteLine($"Buffer Size: {incompletebuffer.Length} Incomplete Size: {incompleteCount}");
            }
        }

        private void PacketCopy(byte[] packet, int SIZE_OF_PACKET)
        {
            if (incompleteCount > 0 && packet.Length > 0)
                Array.Copy(packet, 0, incompletebuffer, incompleteCount, SIZE_OF_PACKET - incompleteCount);
        }
    }
}