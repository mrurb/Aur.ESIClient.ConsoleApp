using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;

using Microsoft.Extensions.Logging;

using System.Net;

namespace Aur.ESIClient.ConsoleApp.Core;
public class EVEDataRepo : IEVEDataRepo
{
    public List<AuthorizedCharacterData> EveChars { get; set; } = new List<AuthorizedCharacterData>();
}
