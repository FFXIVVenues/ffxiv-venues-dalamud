using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Interface.Internal;
using Dalamud.Plugin;

namespace FFXIVVenues.Dalamud;

public class VenueService : IVenueService
{
    private readonly DalamudPluginInterface _pluginInterface;
    private readonly HttpClient _httpClient;
    private Dictionary<string, IDalamudTextureWrap?> _banners;
    private Dictionary<string, Task> _bannerTasks;
    private readonly IDalamudTextureWrap _loadingTexture;

    public VenueService(DalamudPluginInterface pluginInterface, HttpClient httpClient)
    {
        this._pluginInterface = pluginInterface;
        this._httpClient = httpClient;
        this._banners = new();
        var loadingImage = Path.Combine(this._pluginInterface.AssemblyLocation.Directory?.FullName!, "Assets/loading.png");
        this._loadingTexture = this._pluginInterface.UiBuilder.LoadImage(loadingImage);
    }

    public IDalamudTextureWrap? GetVenueBanner(string venueId)
    {
        var bannerExists = this._banners.TryGetValue(venueId, out var banner);
        if (bannerExists)
            return banner;
        
        if ( ! _bannerTasks.ContainsKey(venueId))
            _bannerTasks[venueId] = this._httpClient.GetAsync($"venue/banner/{venueId}")
                .ContinueWith(t => this.OnBannerResponseAsync(venueId, t))
                .ContinueWith(t => _bannerTasks.Remove(venueId));
        
        return this._loadingTexture;
    }

    private async Task OnBannerResponseAsync(string venueId, Task<HttpResponseMessage> task)
    {
        if (!task.IsCompletedSuccessfully)
            _banners[venueId] = null;

        var response = task.Result;
        var stream = await response.Content.ReadAsByteArrayAsync();
        _banners[venueId] = _pluginInterface.UiBuilder.LoadImage(stream);
    }
}

public interface IVenueService
{

    //IDalamudTextureWrap? GetVenueBanner(string venueId);

}