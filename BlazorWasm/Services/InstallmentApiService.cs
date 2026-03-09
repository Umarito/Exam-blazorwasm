using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class InstallmentApiService
{
    private readonly HttpClient _httpClient;

    public InstallmentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<InstallmentModel>> GetInstallmentsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("installments");
            if (!response.IsSuccessStatusCode) return GetDefaultPlans();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            List<InstallmentModel>? result = null;
            if (doc.RootElement.TryGetProperty("items", out var items))
                result = JsonSerializer.Deserialize<List<InstallmentModel>>(items.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            else
                result = JsonSerializer.Deserialize<List<InstallmentModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Count > 0 ? result : GetDefaultPlans();
        }
        catch { return GetDefaultPlans(); }
    }

    private static List<InstallmentModel> GetDefaultPlans() => new()
    {
        new InstallmentModel { Id = 1, MonthCount = 3, InterestRate = 0 },
        new InstallmentModel { Id = 2, MonthCount = 6, InterestRate = 2.5m },
        new InstallmentModel { Id = 3, MonthCount = 12, InterestRate = 5m },
        new InstallmentModel { Id = 4, MonthCount = 24, InterestRate = 9m },
    };

    public decimal CalculateMonthlyPayment(decimal price, InstallmentModel plan)
    {
        if (plan.InterestRate == 0)
            return Math.Round(price / plan.MonthCount, 2);
        var totalWithInterest = price * (1 + plan.InterestRate / 100);
        return Math.Round(totalWithInterest / plan.MonthCount, 2);
    }
}
