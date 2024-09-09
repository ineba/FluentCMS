﻿namespace FluentCMS.Web.Plugins.Admin.LayoutManagement;

public partial class LayoutListPlugin
{
    private List<LayoutDetailResponse> Layouts { get; set; } = [];

    private async Task Load()
    {
        var response = await ApiClient.Layout.GetAllAsync(ViewState.Site.Id);
        Layouts = response?.Data?.ToList() ?? [];
    }

    protected override async Task OnInitializedAsync()
    {
        await Load();
    }

    #region Delete Layout

    private LayoutDetailResponse? SelectedLayout { get; set; }
    public async Task OnDelete()
    {
        if (SelectedLayout == null)
            return;

        await ApiClient.Layout.DeleteAsync(SelectedLayout.Id);
        await Load();
        SelectedLayout = default;
    }

    public async Task OnConfirm(LayoutDetailResponse role)
    {
        SelectedLayout = role;
        await Task.CompletedTask;
    }
    public async Task OnConfirmClose()
    {
        SelectedLayout = default;
        await Task.CompletedTask;
    }
    #endregion

}
