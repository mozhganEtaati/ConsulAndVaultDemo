namespace ConsultAndVaultSample.VaultSharp;

public class VaultConfigurationSource(VaultConfiguration config, ILogger? logger) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new VaultConfigurationProvider(config, logger);
    }
}
