using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TQ.Core.Helpers
{
    /// <summary>
    /// Manage the B2C users operations
    /// </summary>
    public class B2CGraphClient : IB2CGraphClient
    {
        private string Tenant { get; set; }

        private readonly AuthenticationContext authContext;
        private readonly ClientCredential credential;
        private readonly string bearer = "Bearer";
        private readonly string users = "/users/";
        private readonly string applicationType = "application/json";

        /// <inheritdoc/>
        public B2CGraphClient(string clientId, string clientSecret, string tenant)
        {
            // The tenant are pulled in from the App.config file
            this.Tenant = tenant;

            // The AuthenticationContext is ADAL's primary class, in which you indicate the direcotry to use.
            this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);

            // The ClientCredential is where you pass in your client_id and client_secret, which are
            // provided to Azure AD in order to receive an access_token using the app's identity.
            this.credential = new ClientCredential(clientId, clientSecret);
        }

        /// <inheritdoc/>
        public async Task<string> GetUserByObjectIdAsync(string objectId)
        {
            return await SendGraphGetRequestAsync(users + objectId, null).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> GetAllUsersAsync(bool active)
        {
            string query = "$filter=accountEnabled%20eq%20" + active.ToString().ToLower();
            return await SendGraphGetRequestAsync("/users", query).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> CreateUserAsync(string json)
        {
            return await SendGraphPostRequestAsync("/users", json).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> UpdateUserAsync(string objectId, string json)
        {
            return await SendGraphPatchRequestAsync(users + objectId, json).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> DeleteUserAsync(string objectId)
        {
            return await SendGraphDeleteRequestAsync(users + objectId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> RegisterExtensionAsync(string objectId, string body)
        {
            return await SendGraphPostRequestAsync("/applications/" + objectId + "/extensionProperties", body).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> UnregisterExtensionAsync(string appObjectId, string extensionObjectId)
        {
            return await SendGraphDeleteRequestAsync("/applications/" + appObjectId + "/extensionProperties/" + extensionObjectId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> GetExtensionsAsync(string appObjectId)
        {
            return await SendGraphGetRequestAsync("/applications/" + appObjectId + "/extensionProperties", null).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> GetApplicationsAsync(string query)
        {
            return await SendGraphGetRequestAsync("/applications", query).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetUserGroupByObjectIdAsync(string objectId)
        {
            return await SendGraphGetGroupsAsync(users + objectId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> CreateGroupAsync(string json)
        {
            return await SendGraphPostRequestGroupAsync("/groups", json).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<bool> AssignUserToGroupAsync(string groupObjectid, string userObjectid)
        {
            return await SendGraphRequestAssignUserGroupAsync("/groups/", groupObjectid, userObjectid).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<string> SendGraphGetRequestAsync(string api, string query)
        {
            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            AuthenticationResult result = await authContext.AcquireTokenAsync("https://graph.windows.net", credential).ConfigureAwait(false);

            // For B2C user managment, be sure to use the 1.6 Graph API version.
            using var httpClient = new HttpClient();
            string url = "https://graph.windows.net/" + Tenant + api + "?" + Global.AadGraphVersion;
            if (!string.IsNullOrEmpty(query))
            {
                url += "&" + query;
            }

            ////Console.ForegroundColor = ConsoleColor.Cyan;
            ////Console.WriteLine("GET " + url);
            ////Console.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            ////Console.WriteLine(string.Empty);

            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException($"Error Calling the Graph API: \n {JsonConvert.SerializeObject(formatted, Formatting.Indented)}");
            }

            ////Console.ForegroundColor = ConsoleColor.Green;
            ////Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            ////Console.WriteLine(string.Empty);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> SendGraphGetGroupsAsync(string api)
        {
            AuthenticationResult result = await authContext.AcquireTokenAsync("https://graph.windows.net", credential).ConfigureAwait(false);
            var groups = new List<string>();

            // For B2C user managment, be sure to use the 1.6 Graph API version.
            using (var httpClient = new HttpClient())
            {
                const int accessTokenUpperIndex = 80;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken.Substring(0, accessTokenUpperIndex));
                string url = "https://graph.windows.net/" + Tenant + api + "/$links/memberOf" + "?" + Global.AadGraphVersion;

                // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
                HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

                string resp = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var groupArray = (JArray)JObject.Parse(resp)["value"];
                foreach (var group in groupArray)
                {
                    var groupUrl = group["url"].Value<string>();
                    var reqUrl = groupUrl + "?api-version=1.6&$select=displayName";
                    HttpRequestMessage groupResquest = new HttpRequestMessage(HttpMethod.Get, reqUrl);
                    groupResquest.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
                    HttpResponseMessage response1 = await httpClient.SendAsync(groupResquest).ConfigureAwait(false);
                    string groupResp = await response1.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var name = JObject.Parse(groupResp)["displayName"].Value<string>();
                    groups.Add(name);
                }
            }

            return groups;
        }

        /// <inheritdoc/>
        public async Task<string> GetUserByEmailAsync(string emailid)
        {
            string query = string.Format("$filter=signInNames/any(x:x/value%20eq%20'{0}')", emailid);
            return await SendGraphGetRequestAsync("/users", query).ConfigureAwait(false);
        }

        private async Task<bool> SendGraphPostRequestGroupAsync(string api, string json)
        {
            bool isCreated;

            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Global.AadGraphResourceId, credential).ConfigureAwait(false);
            using var httpClient = new HttpClient();
            string url = Global.AadGraphEndpoint + Tenant + api + "?" + Global.AadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, applicationType);
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                isCreated = true;
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException($"Error Calling the Graph API: \n {JsonConvert.SerializeObject(formatted, Formatting.Indented)}");
            }

            return isCreated;
        }

        private async Task<bool> SendGraphRequestAssignUserGroupAsync(string api, string groupObjectid, string userObjectid)
        {
            bool isAssigned;

            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Global.AadGraphResourceId, credential).ConfigureAwait(false);
            using var httpClient = new HttpClient();
            string url = Global.AadGraphEndpoint + Tenant + api + groupObjectid + "/$links/members?" + Global.AadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);

            string groupUrl = Global.AadGraphEndpoint + Tenant + "/directoryObjects/" + userObjectid;
            string json = "{" + string.Format("\"url\":\"{0}\"", groupUrl) + "}";
            request.Content = new StringContent(json, Encoding.UTF8, applicationType);
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                isAssigned = true;
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
            return isAssigned;
        }

        private async Task<string> SendGraphDeleteRequestAsync(string api)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Global.AadGraphResourceId, credential).ConfigureAwait(false);
            using var httpClient = new HttpClient();
            string url = Global.AadGraphEndpoint + Tenant + api + "?" + Global.AadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException($"Error Calling the Graph API: \n {JsonConvert.SerializeObject(formatted, Formatting.Indented)}");
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<string> SendGraphPatchRequestAsync(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Global.AadGraphResourceId, credential).ConfigureAwait(false);
            using var httpClient = new HttpClient();
            string url = Global.AadGraphEndpoint + Tenant + api + "?" + Global.AadGraphVersion;

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, applicationType);
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException($"Error Calling the Graph API: \n {JsonConvert.SerializeObject(formatted, Formatting.Indented)}");
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<string> SendGraphPostRequestAsync(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Global.AadGraphResourceId, credential).ConfigureAwait(false);
            using var httpClient = new HttpClient();
            string url = Global.AadGraphEndpoint + Tenant + api + "?" + Global.AadGraphVersion;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(bearer, result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, applicationType);
            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException($"Error Calling the Graph API: \n {JsonConvert.SerializeObject(formatted, Formatting.Indented)}");
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}