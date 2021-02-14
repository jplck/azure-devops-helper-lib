using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsHelperLib
{
    public class AzureDevOpsHelper
    {
        private string _PAT;
        private string _OrgName;
        private string _ProjectName;

        public string BaseUrl => $"https://dev.azure.com/{_OrgName}/{_ProjectName}/";

        public AzureDevOpsHelper(string PAT, string orgName, string projectName)
        {
            _PAT = PAT;
            _OrgName = orgName;
            _ProjectName = projectName;
        }

        protected async Task<List<T>> GenericFetchAsync<T>(string url)
        {
            var iterationsContent = await CallDevOpsApiWithUrlAsync(url);

            if (!string.IsNullOrEmpty(iterationsContent))
            {
                dynamic iterationContent = JsonConvert.DeserializeObject(iterationsContent);
                return iterationContent.value.ToObject<List<T>>();
            }
            return new List<T>();
        }

        protected async Task<string> CallDevOpsApiWithUrlAsync(string url, string content = "")
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", _PAT))));

                var method = new HttpMethod("GET");
                var request = new HttpRequestMessage(method, url);

                if (content != string.Empty)
                {
                    method = new HttpMethod("POST");

                    request = new HttpRequestMessage(method, url)
                    {
                        Content = new StringContent(content, Encoding.UTF8, "application/json")
                    };
                }

                using (HttpResponseMessage response = client.SendAsync(request).Result)
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
