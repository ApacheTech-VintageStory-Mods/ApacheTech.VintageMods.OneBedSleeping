using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Abstractions.Extensions;
using ApacheTech.VintageMods.FluentChatCommands;
using Gantry.Core;
using Gantry.Core.DependencyInjection;
using Gantry.Core.DependencyInjection.Registration;
using Gantry.Core.Extensions.GameContent;
using Gantry.Core.ModSystems;
using Gantry.Services.FileSystem.DependencyInjection;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.OneBedSleeping.Features.OneBedSleeping
{
    public sealed class OneBedSleeping : UniversalModSystem, IServerServiceRegistrar
    {
        private OneBedSleepingSettings _settings;

        public void ConfigureServerModServices(IServiceCollection services)
        {
            services.AddFeatureWorldSettings<OneBedSleepingSettings>();
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            _settings = IOC.Services.Resolve<OneBedSleepingSettings>();

            FluentChat.RegisterCommand("obs", api)!
                .WithDescription(LangEntry("Command.Description"))
                .HasSubCommand("hunger", h => h.WithHandler(OnServerChatCommand).Build());
        }

        private void OnServerChatCommand(IPlayer player, int groupId, CmdArgs args)
        {
            var value = args.PopFloat().GetValueOrDefault(1f);
            _settings.SaturationMultiplier = GameMath.Clamp(value, 0f, 2f);
            player.SendMessage(groupId, LangEntry("Command.UpdatedHunger", value), EnumChatType.CommandSuccess);
        }

        private string LangEntry(string code, params object[] args)
            => LangEx.FeatureString("OneBedSleeping", code, args);
        
    }
}