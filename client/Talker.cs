using SpeechLib;

namespace myseq
{
    public class Talker
    {
        public string speakText {get; set; }
        public static SpVoice Speech { get; set; } = new SpVoice();

        public void SpeakText() => Speech.Speak(speakText, SpeechVoiceSpeakFlags.SVSFDefault);
    }
}

