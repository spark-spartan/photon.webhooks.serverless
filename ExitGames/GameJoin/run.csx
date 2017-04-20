#r "Newtonsoft.Json"
#load "../Models/Game.csx"
#load "../Common/Nether.csx"
using System;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string appId, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Get request body
    GameLeaveRequest body = await req.Content.ReadAsAsync<GameLeaveRequest>();

    NetherRequest netherRequest = new NetherRequest
    {
        type = "game-start",
        clientUtcTime = DateTime.UtcNow,
        gameSessionId = appId,
        gamerTag = body.UserId
    };

    var response = await Nether.NetherPost(netherRequest, log);

    if(response.IsSuccessStatusCode)
    { 
        var okMsg = $"{req.RequestUri} - Recieved Game Join Request";
        log.Info(okMsg);
        return req.CreateResponse(HttpStatusCode.OK, okMsg);
    }
    else
    {
        var errMsg = $"{req.RequestUri} - Returned an Error from Nether - {await response.Content.ReadAsStringAsync()}";
        log.Error(errMsg);
        return req.CreateResponse(HttpStatusCode.BadRequest, errMsg);
    }
}