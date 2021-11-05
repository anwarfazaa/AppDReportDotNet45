using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AppdReporter
{
    class ControllerConnection
    {

        String urlRequest;
        RestClient client;
        RestRequest RestRequestBody;
        String authToken;
        String ControllerUrl;

       public ControllerConnection(String controllerUrl, String clientId, String clientAccount, String clientSecret)
        {
            ControllerUrl = controllerUrl;
            authToken = (String) getAuthToken(clientId, clientAccount,clientSecret).GetValue("access_token");
            RestRequestBody = new RestRequest(Method.GET);
            RestRequestBody.AddHeader("Authorization", "Bearer " + this.authToken);
        }

        public JArray getHealthRuleViolations(String severity , String timeSet)
        {
            urlRequest = @"" + ControllerUrl + "/controller/rest/applications/Server%20&%20Infrastructure%20Monitoring/problems/healthrule-violations?time-range-type=BETWEEN_TIMES&" + timeSet + "&output=json";
            client = new RestClient(urlRequest);
            var action = client.Execute(RestRequestBody);
            return JArray.Parse(action.Content);
        }

        
        // Issues with RestSharp , could not correctly parse response , thus used HttpClient.
        private JObject getAuthToken(String clientId, String clientAccount,String clientSecret)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("POST"), ControllerUrl + "/controller/api/oauth/access_token");
            var stringContent = "grant_type=client_credentials&client_id=" + clientId + "@" + clientAccount + "&client_secret=" + clientSecret;
            request.Content = new StringContent(stringContent);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.appd.cntrl+protobuf;v=1");
            var response = httpClient.SendAsync(request).Result;
            var responseContent = response.Content;
            string responseString = responseContent.ReadAsStringAsync().Result;
            return JObject.Parse(responseString);
        }

       
        
    }
}
