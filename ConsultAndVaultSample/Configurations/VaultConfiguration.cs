namespace ConsulAndVaultSample.Configurations;

public class VaultConfiguration
{
    public string Token { get; set; } 
    public string BaseAddress { get; set; } 
    public string MountPoint { get; set; } 
    public string SecretLocationPath { get; set; } 
}
