namespace FluentCMS.Web.Plugins.ContentViewer;

public partial class ContentDetailEditPlugin
{
    public const string FORM_NAME = "CONTENT_DETAIL_EDIT_FORM";

    private SettingsModel Model { get; set; } = default!;
    private List<ContentTypeDetailResponse> ContentTypes { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var contentTypeResponse = await ApiClient.ContentType.GetAllAsync(ViewState.Site.Id);
        if (contentTypeResponse?.Data != null)
        {
            ContentTypes = contentTypeResponse.Data.ToList();
        }

        if (Model is null)
        {
            Plugin.Settings.TryGetValue("ContentTypeSlug", out var slug);
            Plugin.Settings.TryGetValue("Template", out var template);

            Model = new()
            {
                Template = template ?? string.Empty,
                ContentTypeSlug = slug ?? string.Empty,
            };
        }
    }

    private async Task HandleSubmit()
    {
        if (Plugin is null)
            return;

        var request = new SettingsUpdateRequest
        {
            Id = Plugin.Id,
            Settings = new Dictionary<string, string> {
                { "Template", Model.Template },
                { "ContentTypeSlug", Model.ContentTypeSlug },
            }
        };

        await ApiClient.Settings.UpdateAsync(request);

        await OnSubmit.InvokeAsync();
    }

    class SettingsModel
    {
        public string Template { get; set; } = string.Empty;
        public string ContentTypeSlug { get; set; } = string.Empty;
    }
}
