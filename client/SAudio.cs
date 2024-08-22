
using System.Threading.Tasks;

namespace myseq
{
    public static class SAudio
    {
        //private static string sndFile;

        //        private static void PlaySnd() => SafeNativeMethods.PlaySound(sndFile, new System.IntPtr(0), new System.IntPtr(0));

        public static async void Play(string fileName) => await Task.Run(() 
            => SafeNativeMethods.PlaySound(fileName, new System.IntPtr(0), new System.IntPtr(0)));
    }
}