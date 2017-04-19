#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"
#load "../Models/Game.csx"
#load "../Common/CreateGame.csx"
#load "../Common/Azure.csx"
using System;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string appId, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Get request body
    GameLeaveRequest body = await req.Content.ReadAsAsync<GameLeaveRequest>();
    Azure azure = new Azure(log);

    // Set name to query string or body data
    string message;
    if (!IsValid(body, out message))
    {
        log.Error($"{req.RequestUri} - {message}");
        return req.CreateResponse(HttpStatusCode.BadRequest,
            $"{req.RequestUri} - {message}");
    }

    if (body.IsInactive)
    {
        if (body.ActorNr > 0)
        {
            azure.GameInsert(appId, body.UserId, body.GameId, body.ActorNr);
        }
    }
    else
    {
        azure.GameDelete(appId, body.UserId, body.GameId);
        using (var client = new HttpClient())
        {
            NetherRequest nether = new NetherRequest
            {
                type = "game-stop",
                clientUtcTime = DateTime.UtcNow,
                gameSessionId = appId
            };
            client.BaseAddress = new Uri("https://exitgamesnether.servicebus.windows.net:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "SharedAccessSignature sr=https%3A%2F%2Fexitgamesnether.servicebus.windows.net%3A443%2Fanalyticevents%2Fmessages&sig=6hYf66bSdXh54qZd50LeJpct7O7uTJKN3uZY5p%2Bu%2BEY%3D&se=1494682796&skn=Send");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            await client.PostAsJsonAsync("analyticevents/messages", nether);
        }
    }

    var okMsg = $"{req.RequestUri} - {body.UserId} left {body.GameId}";
    log.Info(okMsg);
    return req.CreateResponse(HttpStatusCode.OK, okMsg);
}

private static bool IsValid(GameLeaveRequest request, out string message)
{
    if (string.IsNullOrEmpty(request.GameId))
    {
        message = "Missing GameId.";
        return false;
    }

    if (string.IsNullOrEmpty(request.UserId))
    {
        message = "Missing UserId.";
        return false;
    }

    message = "";
    return true;
}