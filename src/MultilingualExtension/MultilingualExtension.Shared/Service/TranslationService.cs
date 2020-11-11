//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Xml;

//using MultilingualExtension.Shared;

//using MultilingualExtension.Shared.Helpers;
//using Newtonsoft.Json;

//namespace MultilingualExtension.Shared.Service
//{
//    public class TranslationService
//    {
//        public TranslationService()
//        {
//        }

//        // This sample uses the Cognitive Services subscription key for all services. To learn more about
//        // authentication options, see: https://docs.microsoft.com/azure/cognitive-services/authentication.
//        // Endpoints for Translator and Bing Spell Check

//        // Dictionary to map language codes from friendly name (sorted case-insensitively on language name)
//        private SortedDictionary<string, string> languageCodesAndTitles =
//            new SortedDictionary<string, string>(Comparer<string>.Create((a, b) => string.Compare(a, b, true)));

//        // ***** GET TRANSLATABLE LANGUAGE CODES
//        private void GetLanguagesForTranslate(string endpoint)
//        {
//            // Send request to get supported language codes
//            string uri = string.Format(endpoint + "{0}?api-version=3.0&scope={1}", "languages", "translation");

//            WebRequest WebRequest = WebRequest.Create(uri);
//            WebRequest.Headers.Add("Accept-Language", "en");
//            WebResponse response = null;
//            // Read and parse the JSON response
//            response = WebRequest.GetResponse();
//            using (var reader = new StreamReader(response.GetResponseStream(), UnicodeEncoding.UTF8))
//            {
//                //var result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(reader.ReadToEnd());
//                //var languages = result["translation"];

//                //languageCodes = languages.Keys.ToArray();
//                //foreach (var kv in languages)
//                //{
//                //    languageCodesAndTitles.Add(kv.Value["name"], kv.Key);
//                //}
//            }
//        }
//        private async Task<Result<Translations>> GoogleTranslateText(string textToTranslate, string fromLanguageCode, string toLanguageCode)
//        {
//            // Set the language from/to in the url (or pass it into this function)
//            string url = String.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", fromLanguageCode, toLanguageCode, Uri.EscapeUriString(textToTranslate));


//            using (var client = new HttpClient())
//            using (var request = new HttpRequestMessage())
//            {
//                request.Method = HttpMethod.Get;
//                request.RequestUri = new Uri(url);


//                var response = await client.SendAsync(request);
//                var responseBody = await response.Content.ReadAsStringAsync();
//                if (response.StatusCode != HttpStatusCode.OK)
//                {
//                    //You will only be allowed to translate about 100 words per hour using the free API.If you abuse this, Google API will return a 429(Too many requests) error.
//                    //TODO: user messsage if more then 100 /h
//                    return new Shared.Helpers.Result<Translations>(responseBody);
//                }
//                else
//                {
//                    int firstDoubleQuotesChar = responseBody.IndexOf("\"");
//                    int seconDoubleQuotesChar = responseBody.IndexOf("\",");
//                    var result = responseBody.Substring(firstDoubleQuotesChar + 1, seconDoubleQuotesChar - firstDoubleQuotesChar - 1);
//                    Translations responsetext = new Translations { text = result, to = toLanguageCode };
//                    return new Shared.Helpers.Result<Translations>(responsetext);
//                }

//            }


//        }

//        private async Task<Result<Translations>> MicrosoftTranslateText(string textToTranslate, string fromLanguageCode, string toLanguageCode, string endpoint, string location, string key)
//        {


//            string uri = string.Format(endpoint + "{0}?api-version=3.0&from={1}&to={2}", "translate", fromLanguageCode, toLanguageCode);

//            System.Object[] body = new System.Object[] { new { Text = textToTranslate } };
//            var requestBody = JsonConvert.SerializeObject(body);

//            var client = new HttpClient();
//            var request = new HttpRequestMessage();
//            request.Method = HttpMethod.Post;
//            request.RequestUri = new Uri(uri);
//            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
//            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
//            request.Headers.Add("Ocp-Apim-Subscription-Region", location);
//            request.Headers.Add("X-ClientTraceId", Guid.NewGuid().ToString());

//            var response = await client.SendAsync(request);
//            var responseBody = await response.Content.ReadAsStringAsync();
//            if (response.StatusCode != HttpStatusCode.OK)
//            {
//                return new Shared.Helpers.Result<Translations>(responseBody);
//            }
//            else
//            {
//                var result = JsonConvert.DeserializeObject<IEnumerable<MicrosoftTranslationResponse>>(responseBody).ToList();

//                return new Shared.Helpers.Result<Translations>(result[0].translations[0]);
//            }
//            // Update the translation field
//            //TranslatedTextLabel.Content = translation;
//            //return translation;
//        }
//        private async Task<Result<Boolean>> TranslateFileInternal(bool useGoogle, string masterLanguageCode, string languageToCode, string updatefilePath, string endpoint, string location, string key, Shared.Interface.IProgressBar progress)
//        {


//            //get language code to translate to

//            //TODO:Validate if language exist in GetLanguagesForTranslate

//            XmlDocument updatedoc = new XmlDocument();
//            updatedoc.Load(updatefilePath);
//            XmlNode rootUpdate = updatedoc.DocumentElement;


//            // Select all nodes data
//            bool updatefilechanged = false;
//            XmlNodeList nodeListUpdate = rootUpdate.SelectNodes("//data");
//            foreach (XmlNode dataUpdate in nodeListUpdate)
//            {
//                //check if comment exist
//                var commentNode = dataUpdate.SelectSingleNode("comment");
//                if (commentNode == null)
//                {
//                    //this should never happend if we add the data nodes throw function updatefiles
//                    commentNode = updatedoc.CreateElement("comment"); //item1 ,item2..
//                    commentNode.InnerText = Globals.STATUS_COMMENT_NEW;
//                    dataUpdate.AppendChild(commentNode);
//                }


//                if (commentNode.InnerText == Globals.STATUS_COMMENT_NEW)
//                {
//                    updatefilechanged = true;

//                    Shared.Helpers.Result<Translations> result;
//                    var valueNode = dataUpdate.SelectSingleNode("value");
//                    if (useGoogle)
//                    {

//                        result = await GoogleTranslateText(valueNode.InnerText, masterLanguageCode, languageToCode);
//                    }
//                    else
//                    {
//                        result = await MicrosoftTranslateText(valueNode.InnerText, masterLanguageCode, languageToCode, endpoint, location, key);
//                    }

//                    Console.Write(result);
//                    if (result.WasSuccessful)
//                    {
//                        updatefilechanged = true;
//                        //Update text
//                        valueNode.InnerText = result.Model.text;
//                        commentNode.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;

//                    }
//                    else
//                    {
//                        //TODO: Fix below
//                        //MessageService.GenericAlert(new GenericMessage { Text = result.ErrorMessage });
//                        break;
//                    }
//                }

//                progress.Pulse();
//            }


//            if (updatefilechanged)
//                updatedoc.Save(updatefilePath);

//            return new Shared.Helpers.Result<bool>(true);
            
//        }
//        public async Task<Result<Boolean>> TranslateFile(string selectedFilename, Interface.IProgressBar progress,Interface.ISettingsService settingsService)
//        {
//            try
//            {
                
//                bool useGoogle = true;
//                if (settingsService.TranslationService == 2)
//                    useGoogle = false;

//                string endpoint = settingsService.MsoftEndpoint;
//                string location = settingsService.MsoftLocation;
//                string key = settingsService.MsoftKey;
//                string masterLanguageCode = settingsService.MasterLanguageCode;

//                //validate file
//                var checkfile = RexExHelper.ValidateFilenameIsTargetType(selectedFilename);
//                if (!checkfile.Success)
//                {
//                    int folderindex;

//                    if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
//                       folderindex = selectedFilename.LastIndexOf("\\");
//                    else
//                        folderindex = selectedFilename.LastIndexOf("/");

//                    string masterFolderPath = selectedFilename.Substring(0, folderindex);

//                    string[] fileEntries = Directory.GetFiles(masterFolderPath);
//                    foreach (string fileName in fileEntries)
//                    {
//                        var checkfileInFolder = RexExHelper.ValidateFilenameIsTargetType(fileName);
//                        if (checkfileInFolder.Success)
//                            await TranslateFileInternal(useGoogle, masterLanguageCode, checkfileInFolder.Value.Substring(1, 2), fileName, endpoint, location, key, progress);

//                    }

//                }
//                else
//                {
//                    string masterPath = selectedFilename.Substring(0, checkfile.Index) + ".resx";
//                    await TranslateFileInternal(useGoogle, masterLanguageCode, checkfile.Value.Substring(1, 2), selectedFilename, endpoint, location, key, progress);
//                }
//                return new Result<bool>(true);

//            }

//            catch (Exception ex)
//            {
//                throw ex;

//            }
//            finally
//            {
//                progress.HideAll();
//                progress = null;
//                Console.WriteLine("Translate file completed");
//            }


//        }

//    }
//}
