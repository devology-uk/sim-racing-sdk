namespace SimRacingSdk.DataManager.Shared.Session;

// Where Mike was working when the app last ran, so a rebuild/restart lands him back in the same
// place. Mutable on purpose - each page updates just its own part as the selection changes. A game
// with pages beyond the catalog ones derives from this to add their state.
public class SessionState
{
    public Dictionary<string, CatalogPageState> CatalogPages { get; set; } = [];
    public string? Page { get; set; }
    public double? WindowHeight { get; set; }
    public bool WindowIsMaximized { get; set; }
    public double? WindowLeft { get; set; }
    public double? WindowTop { get; set; }
    public double? WindowWidth { get; set; }

    public CatalogPageState CatalogPage(string pageKey)
    {
        if(!this.CatalogPages.TryGetValue(pageKey, out var pageState))
        {
            pageState = new CatalogPageState();
            this.CatalogPages[pageKey] = pageState;
        }

        return pageState;
    }
}
