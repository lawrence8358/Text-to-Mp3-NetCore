using System.ComponentModel;
using Newtonsoft.Json;

namespace TextToMp3.Models
{
    public class SettingModel
    {
        public AudioConfigModel AudioConfig { get; set; }
        public VoiceModel Voice { get; set; }

    }

    public class AudioConfigModel
    {
        [DefaultValue("LINEAR16")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string AudioEncoding { get; set; }

        [DefaultValue("handset-class-device")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string EffectsProfileId { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public double Pitch { get; set; }

        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public double SpeakingRate { get; set; }
    }

    public class VoiceModel
    {
        [DefaultValue("cmn-CN")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string LanguageCode { get; set; }
        [DefaultValue("cmn-CN-Wavenet-A")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Name { get; set; }
    }
}

