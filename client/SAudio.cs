using System.Threading;

namespace myseq
{
    public static class SAudio
    {
        private static string sndFile;

        private static void PlaySnd() => SafeNativeMethods.PlaySound(sndFile, new System.IntPtr(0), new System.IntPtr(0));

        public static void Play(string fileName)
        {
            sndFile = fileName;

            ThreadStart entry = new ThreadStart(PlaySnd);

            Thread thread = new Thread(entry);

            thread.Start();
        }
    }
}