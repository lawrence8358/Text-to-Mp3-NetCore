using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Cloud.TextToSpeech.V1;
using Newtonsoft.Json;
using TextToMp3.Models;

namespace TextToMp3
{
    class Program
    {
        #region Members

        const string ConfigDir = "Config";
        const string TextDir = "_InputTextFile";
        const string Mp3Dir = "_OutputMp3";

        #endregion

        #region Properties

        private static SettingModel SettingConfig
        {
            get
            {
                string settingFile = CombinToStartupPath(Path.Combine(ConfigDir, "setting.json"));
                return JsonConvert.DeserializeObject<SettingModel>(File.ReadAllText(settingFile));
            }
        }

        private static string InputDirectory => CombinToStartupPath(TextDir);

        private static string OutputDirectory => CombinToStartupPath(Mp3Dir);

        #endregion

        #region Main 

        static void Main(string[] args)
        {
            try
            {
                InitSampleFile();
                TryCreateDirectory(OutputDirectory);

                if (Validation(InputDirectory, out var files))
                {
                    var config = SettingConfig;
                    SetGoogleEnvironment();
                    ConvertTextToMp3(config, files);
                    ShowSuccess();
                }
                else
                {
                    ShowExitError();
                }
            }
            catch (Exception ex)
            {
                ErrorConsole($"發生不可預期的錯誤 :: { ex.Message } { Environment.NewLine }請按任意鍵結束轉換!");
                Console.ReadLine();
            }
        }

        #endregion

        #region Methods

        private static void InitSampleFile()
        {
            string sampleFile = Path.Combine(InputDirectory, "sample.txt");
            if (!Directory.Exists(InputDirectory) && !File.Exists(sampleFile))
            {
                TryCreateDirectory(InputDirectory);

                string createText = "您好，歡迎使用 Google 文字轉語音服務";
                File.WriteAllText(sampleFile, createText);
            }
        }

        private static void TryCreateDirectory(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        private static bool Validation(string path, out List<string> files)
        {
            files = new List<string>();

            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                var result = di.GetFiles("*.txt", SearchOption.AllDirectories)
                    .Where(info => info.Length > 0)
                    .Select(info => info.FullName);

                files.AddRange(result);
            }

            return files.Count() != 0;
        }

        private static string CombinToStartupPath(string fileName)
        {
            string appPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            return Path.Combine(appPath, fileName);
        }

        private static void SetGoogleEnvironment()
        {
            string license = CombinToStartupPath(Path.Combine(ConfigDir, "license.json"));

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", license);
        }

        private static void ConvertTextToMp3(SettingModel config, List<string> files)
        {
            var client = TextToSpeechClient.Create();

            var voice = new VoiceSelectionParams
            {
                LanguageCode = config.Voice.LanguageCode,
                SsmlGender = SsmlVoiceGender.Neutral,
                Name = config.Voice.Name
            };

            var audioConfig = new AudioConfig
            {
                AudioEncoding = config.AudioConfig.AudioEncoding.AsAudioEncoding(),
                EffectsProfileId = { config.AudioConfig.EffectsProfileId },
                Pitch = config.AudioConfig.Pitch,
                SpeakingRate = config.AudioConfig.SpeakingRate
            };

            files.ForEach(file =>
            {
                Console.WriteLine($"檔案 { Path.GetFileName(file) } 處理中");

                var response = client.SynthesizeSpeech(new SynthesizeSpeechRequest
                {
                    Input = new SynthesisInput { Text = File.ReadAllText(file) },
                    Voice = voice,
                    AudioConfig = audioConfig
                }); ;

                var mp3Name = $"{ Path.GetFileNameWithoutExtension(file) }.mp3";
                var outPath = Path.Combine(OutputDirectory, mp3Name);
                if (File.Exists(outPath))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"語音檔 { outPath } 已存在，略過本次轉換. { Environment.NewLine }");
                    Console.ResetColor();
                }
                else
                {
                    using (Stream output = File.Create(outPath))
                    {
                        response.AudioContent.WriteTo(output);
                        Console.WriteLine($"語音檔 { outPath } 轉換完畢. { Environment.NewLine }");
                    }
                }
            });
        }

        private static void ErrorConsole(string message)
        {
            // Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void ShowExitError()
        {
            ErrorConsole($"資料夾 { TextDir } 內無要轉換的檔案，請按任意鍵結束轉換!");
            Console.ReadLine();
        }

        private static void ShowSuccess()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"語音檔轉換完成，請按任意鍵結束轉換!");
            Console.ResetColor();
            Console.ReadLine();
        }

        #endregion 
    }
}
