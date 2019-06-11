using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Beans
{
    public class VKPost
    {
        public string Text { get; set; }
        public string TimeDate { get; set; }
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; } 
        public int ComentsCount { get; set; }
        public List<string> ImageURLs { get; set; }
        
        public void Parse(JObject VKPostJson)
        {
            JArray attachments = new JArray();
            if (VKPostJson["attachments"] != null)
            {
                attachments = (JArray)VKPostJson["attachments"];
            }
            else if (VKPostJson["copy_history"] != null)
            {
                attachments = (JArray)VKPostJson["copy_history"][0]["attachments"];
            }
            ImageURLs = new List<string>();
            if (attachments.Count != 0)
            {
                if ((string)attachments[0]["type"] == "photo")
                {
                    foreach (var item in attachments.Children())
                    {
                        if (item["photo"] != null)
                            ImageURLs.Add((string)item["photo"]["sizes"][0]["url"]);
                        if (item["audio"] != null)
                            ImageURLs.Add((string)item["audio"]["url"]);
                    }
                }
                else if ((string)attachments[0]["type"] == "doc")
                {
                    ImageURLs.Add((string)attachments[0]["doc"]["url"]);
                }
                else if ((string)attachments[0]["type"] == "audio")
                {
                    ImageURLs.Add((string)attachments[0]["audio"]["url"]);
                }
            }

            this.TimeDate = new DateTime(1970, 1, 1).Add(TimeSpan.FromTicks((int)VKPostJson["date"] * TimeSpan.TicksPerSecond)).ToLocalTime().ToString();
            this.Text = VKPostJson["text"] != null ? (string)VKPostJson["text"] : "Пусто";
            this.LikesCount = (int)VKPostJson["likes"]["count"];
            if (VKPostJson["views"]!= null)
              this.ViewsCount = (int)VKPostJson["views"]["count"]; 
            this.ComentsCount = (int)VKPostJson["comments"]["count"]; 
        }
    }
}