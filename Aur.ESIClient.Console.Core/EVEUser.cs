using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;

using Microsoft.Extensions.Logging;

using System.Net;

namespace Aur.ESIClient.ConsoleApp.Core;
public class EVEUser(ILogger<EVEUser> logger, IEVEDataRepo eVEData, IEsiClient esiClient) : IEVEUser
{
    public async Task AddCharAsync()
    {
        Guid state = Guid.NewGuid();

        string url = esiClient.SSO.CreateAuthenticationUrl(state: state.ToString(), scope: ["esi-location.read_location.v1"]);
        logger.LogInformation(url);

        HttpListener listener = new() { };

        listener.Prefixes.Add("http://localhost:8080/callback/");

        listener.Start();
        logger.LogInformation("Listening...");
        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;

        HttpListenerResponse response = context.Response;
        string responseString = "<HTML><BODY>Character added, you can close this tab.</BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        listener.Stop();

        string? code = request.QueryString["code"];
        SsoToken token = await esiClient.SSO.GetToken(GrantType.AuthorizationCode, code);
        AuthorizedCharacterData auth_char = await esiClient.SSO.Verify(token);

        eVEData.EveChars.Add(auth_char);
        logger.LogInformation("Character added");
    }
}
