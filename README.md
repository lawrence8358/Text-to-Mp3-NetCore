# Google Cloud Text To Speech
本範例使用 NET.Core 3.1 在 `Windows` 環境底下，呼叫 `Google Cloud Text-to-Speech API`，將文字轉換成語音並輸出成 mp3 檔案。

<br><br>

---
### 必要條件
* Windows OS，範例使用 Windows 10。
* Visual Studio 2019。

<br><br>

---
### 第一次使用設定步驟
* 替換 `Config\license.json` 金鑰，請參閱 Google Cloud Platform [事前準備](https://cloud.google.com/text-to-speech/docs/quickstart-protocol?hl=zh-tw#before_you_begin)。
* 若有需要替換 `高傳真語音合成` 設定，請替換 `Config\setting.json` 的內容，可參考此處 [範例](https://cloud.google.com/text-to-speech/?_ga=2.130598607.-112166627.1574068989&_gac=1.175843606.1574069539.EAIaIQobChMIxaDbqLnz5QIVCmoqCh3hwAsqEAAYASABEgJtPfD_BwE)。

 
<br><br>

---
### 使用方式
* 請將要轉換的文字檔案 `(*.txt)` 放置在 `_InputTextFile` 資料夾內。
* 執行 `TextToMp3.exe`，轉換成功後，會將檔案輸出在 `_OutputMp3` 資料夾內，若相同檔名已經存在，則不會執行轉換。

<br><br>
   
---
### License
The MIT license