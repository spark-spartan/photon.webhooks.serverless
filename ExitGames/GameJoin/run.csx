#r "Newtonsoft.Json"
#load "../Models/Game.csx"
using System;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string appId, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Get request body
    GameLeaveRequest body = await req.Content.ReadAsAsync<GameLeaveRequest>();

    using (var client = new HttpClient())
    {
        NetherRequest nether = new NetherRequest
        {
            type = "game-start",
            clientUtcTime = DateTime.UtcNow,
            gameSessionId = appId,
            gamerTag = body.UserId
        };
        client.BaseAddress = new Uri("https://exitgamesnether.servicebus.windows.net:443/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("Authorization", "SharedAccessSignature sr=https%3A%2F%2Fexitgamesnether.servicebus.windows.net%3A443%2Fanalyticevents%2Fmessages&sig=6hYf66bSdXh54qZd50LeJpct7O7uTJKN3uZY5p%2Bu%2BEY%3D&se=1494682796&skn=Send");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await client.PostAsJsonAsync("analyticevents/messages", nether);
    }


        var okMsg = $"{req.RequestUri} - Recieved Game Join Request";
    log.Info(okMsg);
    return req.CreateResponse(HttpStatusCode.OK, okMsg);
}