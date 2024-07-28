
namespace Aur.ESIClient.ConsoleApp.Core;

public interface IEVELocation
{
    Task GetLocationAsync(int index);
    Task GetLocationsAsync();
}