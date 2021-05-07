namespace myseq
{
    public class ProcessInfo
    {
        public ProcessInfo(int ProcessID, string sCharName)
        {
            this.ProcessID = ProcessID;
            SCharName = sCharName;
        }

        public int ProcessID { get; set; }
        public string SCharName { get; set; }
    }
}