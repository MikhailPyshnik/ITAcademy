using System;
using System.Collections.Generic;
using FinalProject.Beans;
using FinalProject.Models;
using Leaf.xNet;
using log4net;
using Newtonsoft.Json.Linq;

namespace FinalProject.Services.Impl
{
    public class VKServiseImpl : VkService
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<VKPost> GetVkPostByDomain(string domain, int size, User user)
        {
            HttpRequest request = new HttpRequest();
            string responce = "";
            string URL = $"https://api.vk.com/method/wall.get?+&token={user.Token}&domain={domain}&count={size}&v=5.95";
            log.Info($"Request by Domain");
            responce = request.Get($"https://api.vk.com/method/wall.get?+&access_token={user.Token}&domain={domain}&count={size}&v=5.95").ToString();

            return GetVkListPost(responce, size);
        }

        public List<VKPost> GetVkPostById(int id, int size, User user)
        {
            HttpRequest request = new HttpRequest();
            string responce = "";
            string URL = $"https://api.vk.com/method/wall.get?+&token={user.Token}&domain={id}&count={size}&v=5.95";
            log.Info($"Request by ID");
            responce = request.Get($"https://api.vk.com/method/wall.get?+&access_token={user.Token}&owner_id={id}&count={size}&v=5.95").ToString();

            return GetVkListPost(responce, size);
        }

        public List<VKPost> GetVkListPost(string responce, int size)
        {
            JObject VKJson = JObject.Parse(responce);
            JArray items = (JArray)VKJson["response"]["items"];

            List<VKPost> vkposts = new List<VKPost>();

            foreach (var item in items.Children())
            {
                VKPost parsedPost = new VKPost();
                parsedPost.Parse(item.ToObject<JObject>());
                vkposts.Add(parsedPost);
            }
            return vkposts;
        }

        public string GetVKToken(User user)
        {
            HttpRequest request = new HttpRequest();
            string responce = "";
            try
            {
                responce = request.Post("https://oauth.vk.com/token?grant_type=password&client_id=2274003&client_secret=hHbZxrka2uZ6jB1inYsH&username=" + user.Login + "&password=" + user.Password).ToString();
            }
            catch (HttpException ex)
            {
                log.Error("Can not get Token for user.");
                return null;
            }
            var details = JObject.Parse(responce);
            log.Info("Token is received.");
            return  (String) details["access_token"];
        }
    }
}