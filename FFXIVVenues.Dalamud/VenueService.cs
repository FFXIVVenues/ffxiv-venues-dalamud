using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Plugin;
using ImGuiScene;

namespace FFXIVVenues.Dalamud;

public class VenueService : IVenueService
{
    private readonly DalamudPluginInterface _pluginInterface;
    private readonly HttpClient _httpClient;
    private Dictionary<string, TextureWrap> _banners;
    private readonly TextureWrap _loadingTexture;

    public VenueService(DalamudPluginInterface pluginInterface, HttpClient httpClient)
    {
        this._pluginInterface = pluginInterface;
        this._httpClient = httpClient;
        this._banners = new();
        var loadingImage = Path.Combine(this._pluginInterface.AssemblyLocation.Directory?.FullName!, "loading.png");
        this._loadingTexture = this._pluginInterface.UiBuilder.LoadImage(loadingImage);
    }

    public TextureWrap GetVenueBanner(string venueId)
    {
        var bannerExists = this._banners.TryGetValue(venueId, out var banner);
        if (bannerExists)
            return banner;

        this._httpClient.GetAsync($"venue/banner/{venueId}").ContinueWith(this.OnBannerResponse);
        Task.Factory.StartNew(() =>
        {
            
        });
        return this._loadingTexture;
    }

    private Task OnBannerResponse(Task<HttpResponseMessage> response)
    {
        if (!response.IsCompletedSuccessfully)
            this._pluginInterface
    }
}

public interface IVenueService
{

    TextureWrap GetVenueBanner(string venueId);

}