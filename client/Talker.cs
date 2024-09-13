using SpeechLib;

namespace Structures
{
    public class Talker
    {
        public string SpeakingText { get; set; }
        private SpVoice speech;

        public Talker(string text)
        {
            SpeakingText = text;
            speech = new SpVoice();
        }

        // Speak text on a background thread
        public void SpeakText()
        {
            speech.Speak(SpeakingText, SpeechVoiceSpeakFlags.SVSFDefault);
        }
    }
}