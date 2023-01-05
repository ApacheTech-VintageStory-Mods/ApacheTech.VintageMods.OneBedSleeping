using Gantry.Services.FileSystem.Configuration.Abstractions;

namespace ApacheTech.VintageMods.OneBedSleeping.Features.OneBedSleeping
{
    public sealed class OneBedSleepingSettings : FeatureSettings
    {
        public float SaturationMultiplier { get; set; } = 1f;
    }
}