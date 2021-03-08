
using SpeechLib;

namespace myseq
{
    public class Talker

    {
        public string speakText;

        public static SpVoice speech = new SpVoice();

        public void SpeakText()

        {
            speech.Speak(speakText, SpeechVoiceSpeakFlags.SVSFDefault);
        }
    }
}

