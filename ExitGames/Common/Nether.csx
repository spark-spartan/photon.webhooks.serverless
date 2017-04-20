#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"
#load "../Models/Game.csx"
#load "../Common/CreateGame.csx"
#load "../Common/Azure.csx"
using System;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public static class Nether
{
   public static async Task<HttpResponseMessage> NetherPost(NetherRequest netherRequest, TraceWriter log)
   {
        HttpResponseMessage response = null;

        NetherAuth auth = await GetToken();
        log.Info("Received Bearer Token From Nether");

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(auth.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", auth.authorization);

            response = await client.PostAsJsonAsync("", netherRequest);
            log.Info("Sent Game Event to Nether");
        }

        return response;
    }

    private static async Task<NetherAuth> GetToken()
    {
        NetherAuth auth = null;
        using (var client = new HttpClient())
        {
            var conn = System.Configuration.ConfigurationManager.ConnectionStrings["AzureNetherUrl"].ConnectionString;
            client.BaseAddress = new Uri(conn);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/endpoint");
            if (response.IsSuccessStatusCode)
            {
                auth = await response.Content.ReadAsAsync<NetherAuth>();
            }
        }
        
        return auth;
    }
}

public class NetherAuth
{
    public string url { get; set; }
    public string authorization { get; set; }
}
