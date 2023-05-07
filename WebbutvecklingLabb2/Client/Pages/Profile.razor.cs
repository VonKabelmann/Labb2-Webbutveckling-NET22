using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Security.Claims;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.Client.Pages;

partial class Profile : ComponentBase
{
    private CustomerDto? CustomerProfile { get; set; } = new();
    private bool IsNewCustomer { get; set; } = true;
    private ClaimsPrincipal user = null;

    protected override async Task OnInitializedAsync()
    {
        user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;

        var email = user.Claims.Single(c => c.Type == "name").Value;
        CustomerProfile.EMail = email;

        var response = await HttpClient.GetAsync($"customers/{email}");

        if (response.IsSuccessStatusCode)
        {
            IsNewCustomer = false;
            CustomerProfile = await response.Content.ReadFromJsonAsync<CustomerDto>();
        }

        await base.OnInitializedAsync();
    }


    private async Task SaveCustomerProfile()
    {
        if (IsNewCustomer)
        {
            await HttpClient.PostAsJsonAsync("customers", CustomerProfile);
        }
        else
        {
            await HttpClient.PutAsJsonAsync($"customers/{CustomerProfile.Id}", CustomerProfile);
        }
        NavigationManager.NavigateTo("");
    }
}