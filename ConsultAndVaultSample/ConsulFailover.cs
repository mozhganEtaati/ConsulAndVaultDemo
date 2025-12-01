namespace ConsulAndVaultSample;

public static class ConsulFailover
{
    public static async Task<string> SelectHealthyConsul(string[] addresses)
    {
        foreach (var address in addresses)
        {
            try
            {
                using var http = new HttpClient();
                var result =await http.GetAsync($"{address}/v1/status/leader");

                if (result.IsSuccessStatusCode)
                    return address;
            }
            catch { /* ignore */ }
        }

        throw new Exception("No healthy Consul nodes available!");
    }
}
