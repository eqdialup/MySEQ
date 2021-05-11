// Class Files

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using myseq;

namespace Structures

{
    public class EQCommunications
    {
        private const string ServConErr = "Server Connection Error";

        // Variables to store any incomplete packets till the next chunk arrives.
        private int incompleteCount = 0;

        private bool RequestPending;

        private CSocketClient pSocketClient;

        // Processing stuff-
        public ProcessInfo CurrentProcess {get; private set; } = new ProcessInfo(0, "");
        private int processcount;
        public List<ProcessInfo> ColProcesses { get; } = new List<ProcessInfo>();

        private bool update_hidden;

        private bool mbGetProcessInfo;
        private bool send_process;

        private int numPackets; // Total Packets expected

        private int numProcessed; // No. of Packets already processed

        private readonly byte[] incompletebuffer = new byte[2048];

        private readonly EQData eq;
        private readonly MainForm f1;

        public int NewProcessID { get; set; }
        public string curZone { get; set; }
        public string mapnameWithLabels { get; set; }

        public void UpdateHidden()
        {
            update_hidden = true;
        }

        public EQCommunications(EQData eq, MainForm f1)
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
                pSocketClient?.Disconnect();
                pSocketClient = null;
            }
            catch (Exception pException) { LogLib.WriteLine($"Error: StopListening: {pException.Message}"); }
        }

        public bool ConnectToServer(string ServerAddress, int ServerPort, bool errMsg = true)

        {
            try
            {
                pSocketClient?.Disconnect();

                // Instantiate a CSocketClient object
                pSocketClient = new CSocketClient(100000,
                    new CSocketClient.MESSAGE_HANDLER(MessageHandlerClient),
                    new CSocketClient.CLOSE_HANDLER(CloseHandler)
                    );

                // Establish a connection to the server
                mbGetProcessInfo = true;
                pSocketClient.Connect(ServerAddress, ServerPort);

                return true;
            }
            catch (Exception pException)
            {
                var msg = $"{ServConErr} {pException.Message}";
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
        private void MessageHandlerClient(CSocketClient pSocket, int iNumberOfBytes)
        {
            ProcessPacket(pSocket.GetRawBuffer, iNumberOfBytes);
        }
        private void CloseHandler(CSocketClient pSocket)
        {
            if (f1 == null)
            {
                StopListening();
            }
            else
            {
                f1.StopListening();
            }
        }

        //********************************************************************

        public void Tick()
        {
            int Request;

            try
            {
                if (!RequestPending)
                {
                    if (NewProcessID > 0 && !mbGetProcessInfo)
                    {
                        if (!send_process)
                        {
                            // We have a request to change the process

                            Request = (int)RequestTypes.SET_PROCESSINFO;
                            SendData(BitConverter.GetBytes(Request));
                            send_process = true;
                        }
                        else
                        {
                            SendData(BitConverter.GetBytes(NewProcessID));
                            send_process = false;
                            NewProcessID = 0;
                            mbGetProcessInfo = true;
                        }
                    }
                    else
                    {
                        RequestPending = true;
                        Request = (int)(RequestTypes.ZONE
                                        | RequestTypes.PLAYER
                                        | RequestTypes.TARGET
                                        | RequestTypes.MOBS
                                        | RequestTypes.GROUND_ITEMS
                                        | RequestTypes.WORLD);

                        if (mbGetProcessInfo && NewProcessID == 0)
                        {
                            mbGetProcessInfo = false;
                            Request |= (int)RequestTypes.GET_PROCESSINFO;
                        }

                        SendData(BitConverter.GetBytes(Request));
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error: timPackets_Tick: ", ex); }
        }

        private void SendData(byte[] data) => pSocketClient.Send(data);

        public void CharRefresh()
        {
            if (pSocketClient != null)
            {
                mbGetProcessInfo = true;
            }
        }

        public void SwitchCharacter(int CharacterIndex)
        {
            if (ColProcesses.Count >= CharacterIndex)
            {
                ProcessInfo PI = ColProcesses[CharacterIndex - 1];
                if (PI?.ProcessID > 0)
                {
                    NewProcessID = PI.ProcessID;
                }
            }
        }

        public bool CanSwitchChars() => NewProcessID == 0 && !mbGetProcessInfo;

        private void ProcessPacket(byte[] packet, int bytes)
        {
            var offset = 0;

            const int SIZE_OF_PACKET = 100; //104 on new server

            try
            {
                if (bytes > 0)
                {
                    // we have received some bytes, check if this is the beginning of a new packet or a chunk of an existing one.

                    offset = CheckStart(packet);

                    eq.BeginProcessPacket(); //clears spawn&ground arrays

                    for (; offset + SIZE_OF_PACKET <= bytes; offset += SIZE_OF_PACKET)
                    {
                        Spawninfo si = new Spawninfo();

                        if (offset < 0)
                        {
                            // copy the missing chunk of the incomplete packet to the incomplete packet buffer
                            try
                            {
                                PacketCopy(packet, SIZE_OF_PACKET);
                            }
                            catch (Exception ex) { LogLib.WriteLine("Error: ProcessPacket: Copy Incomplete packet buffer: ", ex); }
                            incompleteCount = 0;
                            if (incompletebuffer.Length == 0)
                            {
                                numPackets = 0;
                                break;
                            }
                            si.Frombytes(incompletebuffer, 0);
                        }
                        else
                        {
                            si.Frombytes(packet, offset);
                        }

                        numProcessed++;
                        ProcessPacket(si);
                    }

                    eq.ProcessSpawnList(f1.SpawnList);
                    eq.ProcessGroundItemList(f1.GroundItemList);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error: ProcessPacket: ", ex); }

            ProcessedPackets(packet, bytes, offset);
        }

        private void ProcessPacket(Spawninfo si)
        {
            // SPAWN  // si.flags == 0
            // Target // si.flags == 1
            //  MAP   // si.flags == 4
            // GROUND // si.flags == 5
            //ProcInfo// si.flags == 6
            //World//    si.flags == 8
            // PLAYER // si.flags == 253

            switch (si.flags)
            {
                case PacketType.Zone:

                    f1.ProcessMap(si);

                    break;

                case PacketType.Player:

                    eq.ProcessGamer(si, f1);

                    break;

                case PacketType.GroundItem:

                    eq.ProcessGroundItems(si);

                    break;

                case PacketType.Target:

                    eq.ProcessTarget(si);

                    break;

                case PacketType.World:

                    eq.ProcessWorld(si);

                    break;

                case PacketType.Spawn:

                    eq.ProcessSpawns(si, update_hidden);

                    break;

                case PacketType.GetProcessInfo:

                    ProcessProcessInfo(si);
                    break;

                default:

                    LogLib.WriteLine("Unknown Packet Type: " + si.flags.ToString(), LogLevel.Warning);

                    break;
            }
        }

        private void PacketCopy(byte[] packet, int SIZE_OF_PACKET)
        {
            if (incompleteCount > 0 && packet.Length > 0)
            {
                Array.Copy(packet, 0, incompletebuffer, incompleteCount, SIZE_OF_PACKET - incompleteCount);
            }
        }

        private int CheckStart(byte[] packet)
        {
            int offset;
            if (numPackets == 0)
            {
                // The first word in the data stream is the number of packets

                numPackets = BitConverter.ToInt32(packet, 0);
                offset = 4;
                StartNewPackets();
            }
            else
            {
                // We havent finished processing packets, so check if we have any extra bytes stored in our incomplete buffer.

                offset = -incompleteCount;
            }

            return offset;
        }

        private void ProcessProcessInfo(Spawninfo si)
        {
            ProcessInfo ProcInfo = new ProcessInfo(si.SpawnID, si.Name);

            if (si.SpawnID == 0)
            {
                ProcInfo.SCharName = "";
                CurrentProcess = ProcInfo;
            }
            else
            {
                processcount++;

                while (ColProcesses.Count > 0 && ColProcesses.Count >= processcount)
                {
                    ColProcesses.Remove(ColProcesses[ColProcesses.Count - 1]);
                }

                ColProcesses.Add(ProcInfo);

                f1.ShowCharsInList(si, ProcInfo);
            }
        }

        private void StartNewPackets() => processcount = 0;

        private void ProcessedPackets(byte[] packet, int bytes, int offset)
        {
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

                CheckMobs();
                f1.MapConInvalidate();
            }
        }

        private void FinalizeProcess()
        {
            RequestPending = false;
            update_hidden = false;

            numPackets = numProcessed = 0;

            incompleteCount = 0;
            // Make sure that the incomplete buffer is actually empty
            if (incompletebuffer.Length > 0)
            {
                for (var pp = 0; pp < incompletebuffer.Length; pp++)
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

        private void CheckMobs() => eq.CheckMobs(f1.SpawnList, f1.GroundItemList);
        internal void ProcessClear() => ColProcesses.Clear();
    }
}