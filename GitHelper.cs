using AzureDevOpsHelperLib.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsHelperLib
{
    public class GitHelper : AzureDevOpsHelper
    {
        public GitHelper(string PAT, string orgName, string projectName) : base(PAT, orgName, projectName){}

        public async Task<List<Commit>> FetchCommitsFromPullRequestAsync(string repositoryName, int prId)
        {
            string url = string.Format(
                @"{0}/_apis/git/repositories/{1}/pullRequests/{2}/commits?api-version=6.0",
                BaseUrl,
                repositoryName,
                prId);

            return await GenericFetchAsync<Commit>(url);
        }

        public async Task<List<Commit>> GetCommitsWithWorkItemReferencesByIdsAsync(string reposName, string sourceRefName, string lastMergeSourceCommit, int commitsInPr)
        {
            if (commitsInPr == 0) { return new List<Commit>(); }

            string Url = string.Format(
                @"{0}/_apis/git/repositories/{1}/commitsbatch?api-version=6.0&$top={2}",
                BaseUrl,
                reposName,
                commitsInPr);

            var batchRequestPayload = JsonConvert.SerializeObject(
            new
            {
                ItemVersion = new
                {
                    VersionType = "branch",
                    Version = sourceRefName.Replace("refs/heads/", string.Empty)
                },
                IncludeWorkItems = true
            });

            var commitContentPayloadString = await CallDevOpsApiWithUrlAsync(Url, batchRequestPayload);

            if (!string.IsNullOrEmpty(commitContentPayloadString))
            {
                dynamic commitContent = JsonConvert.DeserializeObject(commitContentPayloadString);
                return commitContent.value.ToObject<List<Commit>>();
            }
            return new List<Commit>();
        }

        public async Task<List<Iteration>> FetchPullRequestIterationsAsync(string repositoryName, int prId)
        {
            string url = string.Format(
                @"{0}/_apis/git/repositories/{1}/pullRequests/{2}/iterations?api-version=6.0",
                BaseUrl,
                repositoryName,
                prId);

            return await GenericFetchAsync<Iteration>(url);
        }

        public async Task PostStatusOnPullRequestAsync(bool supportsIteration, string reposioryName, int prId, string status)
        {
            var path = "statuses";
            if (supportsIteration)
            {
                var iterations = await FetchPullRequestIterationsAsync(reposioryName, prId);
                path = $"iterations/{iterations.Last().Id}/statuses";
            }

            string Url = string.Format(
                @"{0}/_apis/git/repositories/{1}/pullrequests/{2}/{3}?api-version=6.0-preview.1",
                BaseUrl,
                reposioryName,
                prId,
                path);

            await CallDevOpsApiWithUrlAsync(Url, status);
        }
    }
}
