using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.FluentChatCommands;
using Gantry.Core.DependencyInjection;
using Gantry.Services.FileSystem.DependencyInjection;
using Gantry.Services.HarmonyPatches.DependencyInjection;
using Gantry.Services.Network.DependencyInjection;

namespace ApacheTech.VintageMods.OneBedSleeping
{
    /// <summary>
    ///     Mod entry-point.
    /// </summary>
    /// <seealso cref="ModHost" />
    public sealed class Program : ModHost
    {
        protected override void ConfigureServerModServices(IServiceCollection services)
        {
            services.AddFileSystemService(o => o.RegisterSettingsFiles = true);
        }

        /// <summary>
        ///     Configures any services that need to be added to the IO Container, on the client side.
        /// </summary>
        /// <param name="services">The as-of-yet un-built services container.</param>
        protected override void ConfigureUniversalModServices(IServiceCollection services)
        {
            services.AddHarmonyPatchingService(o => o.AutoPatchModAssembly = true);
            services.AddNetworkService();
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            FluentChat.ClearCommands(UApi);
        }
    }
}