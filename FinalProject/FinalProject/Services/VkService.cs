using FinalProject.Beans;
using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Services
{
    interface VkService
    {
        string GetVKToken(User user);
        List<VKPost> GetVkPostById(int id, int size, User user);
        List<VKPost> GetVkPostByDomain(string domain, int size, User user);
    }
}
