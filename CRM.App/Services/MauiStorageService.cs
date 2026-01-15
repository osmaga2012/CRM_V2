

using CRM.App.Shared.Interfaces;

namespace CRM.App.Services
{
    public class MauiStorageService : ISecureStorageService
    {
        public async Task SaveTokenAsync(string token) => await SecureStorage.Default.SetAsync("auth_token", token);

        public async Task<string> GetTokenAsync() => await SecureStorage.Default.GetAsync("auth_token");

        public void RemoveTokenAsync() => SecureStorage.Default.Remove ("auth_token");
    }
}
