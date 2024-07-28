using ESI.NET.Models.SSO;

namespace Aur.ESIClient.ConsoleApp.Core;
public interface IEVEDataRepo
{
    List<AuthorizedCharacterData> EveChars { get; set; }
}