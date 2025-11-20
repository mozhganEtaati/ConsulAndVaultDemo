using ConsulAndVaultSample.Configurations;

namespace ConsulAndVaultSample.VaultSharp;

public static class Registration
{
    public static IConfigurationBuilder AddVaultSecurityManagment(this IConfigurationBuilder builder, Action<VaultConfiguration> configurator, ILogger? logger = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configurator);

        VaultConfiguration config = new();
        configurator(config);

        return builder.Add(new VaultConfigurationSource(config, logger));
    }

    public static IConfigurationBuilder AddVaultSecurity(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        var vaultToken = configuration["VaultConfiguration:Token"];
        var vaultBaseAddress = configuration["VaultConfiguration:BaseAddress"];
        var vaultMountPoint = configuration["VaultConfiguration:MountPoint"];
        var vaultSecretLocationPath = configuration["VaultConfiguration:SecretLocationPath"];

        return builder.AddVaultSecurityManagment(options =>
        {
            options.Token = vaultToken!;
            options.BaseAddress = vaultBaseAddress!;
            options.MountPoint = vaultMountPoint!;
            options.SecretLocationPath = vaultSecretLocationPath!;
        });
    }

}
