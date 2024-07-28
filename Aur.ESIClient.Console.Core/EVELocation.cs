using ESI.NET;

using Microsoft.Extensions.Logging;

using System;

namespace Aur.ESIClient.ConsoleApp.Core;
public class EVELocation(ILogger<EVELocation> logger, IEsiClient esiClient, IEVEDataRepo eVEData) : IEVELocation
{
    public async Task GetLocationAsync(int index)
    {
        ESI.NET.Models.SSO.AuthorizedCharacterData data = eVEData.EveChars[index];
        esiClient.SetCharacterData(data);

        EsiResponse<ESI.NET.Models.Location.Location> esiResponse = await esiClient.Location.Location();
        ESI.NET.Models.Location.Location location = esiResponse.Data ?? throw new InvalidDataException();
        List<ESI.NET.Models.Universe.ResolvedInfo> names = (await esiClient.Universe.Names([location.SolarSystemId])).Data;

        logger.LogInformation("{Character} is currently in {System}", data.CharacterName, names.First(c => c.Id == location.SolarSystemId).Name);
    }

    public async Task GetLocationsAsync()
    {
        foreach (var item in eVEData.EveChars)
        {
            ESI.NET.Models.SSO.AuthorizedCharacterData data = item;
            esiClient.SetCharacterData(data);

            EsiResponse<ESI.NET.Models.Location.Location> esiResponse = await esiClient.Location.Location();
            ESI.NET.Models.Location.Location location = esiResponse.Data ?? throw new InvalidDataException();
            List<ESI.NET.Models.Universe.ResolvedInfo> names = (await esiClient.Universe.Names([location.SolarSystemId])).Data;

            logger.LogInformation("{Character} is currently in {System}", data.CharacterName, names.First(c => c.Id == location.SolarSystemId).Name);
        }
    }
}
