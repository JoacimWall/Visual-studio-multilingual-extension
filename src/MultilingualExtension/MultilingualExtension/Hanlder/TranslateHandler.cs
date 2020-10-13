using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Gtk;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using Newtonsoft.Json;

namespace MultilingualExtension
{
    class TranslateHandler : CommandHandler
    {
        // This sample uses the Cognitive Services subscription key for all services. To learn more about
        // authentication options, see: https://docs.microsoft.com/azure/cognitive-services/authentication.
        // Endpoints for Translator and Bing Spell Check
        
        // Dictionary to map language codes from friendly name (sorted case-insensitively on language name)
        private SortedDictionary<string, string> languageCodesAndTitles =
            new SortedDictionary<string, string>(Comparer<string>.Create((a, b) => string.Compare(a, b, true)));

        // ***** GET TRANSLATABLE LANGUAGE CODES
        private void GetLanguagesForTranslate(string endpoint)
        {
            // Send request to get supported language codes
            string uri = string.Format(endpoint + "{0}?api-version=3.0&scope={1}", "languages", "translation");

            WebRequest WebRequest = WebRequest.Create(uri);
            WebRequest.Headers.Add("Accept-Language", "en");
            WebResponse response = null;
            // Read and parse the JSON response
            response = WebRequest.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream(), UnicodeEncoding.UTF8))
            {
                var result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(reader.ReadToEnd());
                var languages = result["translation"];

                //languageCodes = languages.Keys.ToArray();
                //foreach (var kv in languages)
                //{
                //    languageCodesAndTitles.Add(kv.Value["name"], kv.Key);
                //}
            }
        }
        private async Task<Helper.Result<Translations>> GoogleTranslateText(string textToTranslate,string fromLanguageCode, string toLanguageCode)
        {
            // Set the language from/to in the url (or pass it into this function)
            string url = String.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", fromLanguageCode, toLanguageCode, Uri.EscapeUriString(textToTranslate));
            

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(url);
               

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //You will only be allowed to translate about 100 words per hour using the free API.If you abuse this, Google API will return a 429(Too many requests) error.
                    //TODO: user messsage if more then 100 /h
                    return new Helper.Result<Translations>(responseBody);
                }
                else
                {
                    int firstDoubleQuotesChar = responseBody.IndexOf("\"");
                    int seconDoubleQuotesChar = responseBody.IndexOf("\",");
                    var result = responseBody.Substring(firstDoubleQuotesChar + 1, seconDoubleQuotesChar - firstDoubleQuotesChar - 1);
                    Translations responsetext = new Translations { text = result, to = toLanguageCode };
                    return new Helper.Result<Translations>(responsetext);
                }

            }


        }

        private async Task<Helper.Result<Translations>> MicrosoftTranslateText(string textToTranslate,string fromLanguageCode, string toLanguageCode, string endpoint, string location, string key)
        {
            
           
            string uri = string.Format(endpoint + "{0}?api-version=3.0&from={1}&to={2}", "translate", fromLanguageCode, toLanguageCode);

            System.Object[] body = new System.Object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);
                request.Headers.Add("X-ClientTraceId", Guid.NewGuid().ToString());

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new Helper.Result<Translations>(responseBody);
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<IEnumerable<MicrosoftTranslationResponse>>(responseBody).ToList();

                    return new Helper.Result<Translations>(result[0].translations[0]);
                }
                // Update the translation field
                //TranslatedTextLabel.Content = translation;
            }
            //return translation;
        }


        protected async override void Run()
        {
            Helper.ProgressBarHelper progress = new Helper.ProgressBarHelper("Translate rows where comment has value 'New'");
            try
            {
                bool useGoogle = true;
                if (Service.SettingsService.TranslationService == "2")
                {
                    useGoogle = false;
                }
                string endpoint = Service.SettingsService.MsoftEndpoint;
                string location = Service.SettingsService.MsoftLocation;
                string key = Service.SettingsService.MsoftKey;
                string masterLanguageCode = Service.SettingsService.MasterLanguageCode;

                ProjectFile selectedItem = (ProjectFile)IdeApp.Workspace.CurrentSelectedItem;
                string filename = selectedItem.Name;

                //validate file
                Regex regex = new Regex(".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx");
                var checkfile = regex.Match(filename);
                if (!checkfile.Success)
                {
                    MessageService.GenericAlert(new GenericMessage { Text = "The Resx file needs to be named xxxx.xx-xx.resx. exsample for swedish 'AppResources.sv-SE.resx' don't select the master file AppResources.resx" });
                    return;

                }

                //get language code to translate to
                string toLanguageCode = checkfile.Value.Substring(1, 2);
                //TODO:Validate if language exist in GetLanguagesForTranslate

                XmlDocument updatedoc = new XmlDocument();
                updatedoc.Load(filename);
                XmlNode rootUpdate = updatedoc.DocumentElement;


                // Select all nodes data
                bool updatefilechanged = false;
                XmlNodeList nodeListUpdate = rootUpdate.SelectNodes("//data");
                foreach (XmlNode dataUpdate in nodeListUpdate)
                {
                    //check if comment exist
                    var commentNode = dataUpdate.SelectSingleNode("comment");
                    if (commentNode == null)
                    {
                        //this should never happend if we add the data nodes throw function updatefiles
                        commentNode = updatedoc.CreateElement("comment"); //item1 ,item2..
                        commentNode.InnerText = Globals.STATUS_COMMENT_NEW;
                        dataUpdate.AppendChild(commentNode);
                    }


                    if (commentNode.InnerText == Globals.STATUS_COMMENT_NEW)
                    {
                        // <data name="Select_All" xml:space="preserve">
                        //< value > Select All </ value >
                        //<comment>New,Translated,Finish</comment>
                        //  </ data >
                        //TODO: We should get te value for translate from the master resx file.

                        Helper.Result<Translations> result;
                        var valueNode = dataUpdate.SelectSingleNode("value");
                        if (useGoogle)
                        {

                            result = await GoogleTranslateText(valueNode.InnerText, masterLanguageCode, toLanguageCode);
                        }
                        else
                        {
                            result = await MicrosoftTranslateText(valueNode.InnerText, masterLanguageCode, toLanguageCode, endpoint, location, key);
                        }

                        Console.Write(result);
                        if (result.WasSuccessful)
                        {
                            updatefilechanged = true;
                            //Update text
                            valueNode.InnerText = result.Model.text;
                            commentNode.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;

                        }
                        else
                        {
                            MessageService.GenericAlert(new GenericMessage { Text = result.ErrorMessage });
                            break;
                        }
                    }

                    progress.pdata.pbar.Pulse();

                    //TODO:Remove before publish 
                    //Silmulate time 
                    await Task.Delay(1000);




                }
                if (updatefilechanged)
                    updatedoc.Save(filename);

                progress.pdata.window.HideAll();
                progress = null;


            }

            catch (Exception ex)
            {
                MessageService.GenericAlert(new GenericMessage { Text = ex.Message });

            }
            finally
            {
                progress.pdata.window.HideAll();
                progress = null;
            }

        }

        protected override void Update(CommandInfo info)
        {

            //TODO: Check if resx files exist.
        }
        public struct ProgressData
        {
            public Gtk.Window window;
            public Gtk.ProgressBar pbar;
            public uint timer;
            public bool activity_mode;
        }
    }

}
