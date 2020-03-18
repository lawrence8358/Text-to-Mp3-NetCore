using System;
using Google.Cloud.TextToSpeech.V1;

namespace TextToMp3
{
    public static class Extensions
    {
        public static AudioEncoding AsAudioEncoding(this string type)
        {
            if (type.Equals("Unspecified", StringComparison.InvariantCultureIgnoreCase)) return AudioEncoding.Unspecified;
            if (type.Equals("Linear16", StringComparison.InvariantCultureIgnoreCase)) return AudioEncoding.Linear16;
            if (type.Equals("OggOpus", StringComparison.InvariantCultureIgnoreCase)) return AudioEncoding.OggOpus;

            return AudioEncoding.Mp3;
        }
    }
}
