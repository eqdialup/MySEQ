using SpeechLib;

namespace Structures
{
    public class Talker
    {
        public string SpeakingText { get; set; }
        public static SpVoice Speech { get; set; } = new SpVoice();

        public void SpeakText() => Speech.Speak(SpeakingText, SpeechVoiceSpeakFlags.SVSFDefault);
    }
}

