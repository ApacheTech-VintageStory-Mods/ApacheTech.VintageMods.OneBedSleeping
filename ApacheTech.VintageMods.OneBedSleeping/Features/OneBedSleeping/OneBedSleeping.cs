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
                .HasSubCommand("hunger", h => h.WithHandler(OnHungerSubCommand).Build())
                .HasSubCommand("players", h => h.WithHandler(OnPercentSubCommand).Build());
        }

        private void OnHungerSubCommand(IPlayer player, int groupId, CmdArgs args)
        {
            if (args.Length != 0)
            {
                var value = args.PopFloat().GetValueOrDefault(1f);
                _settings.SaturationMultiplier = GameMath.Clamp(value, 0f, 2f);
            }
            player.SendMessage(groupId,
                LangEntry("Command.Hunger", _settings.SaturationMultiplier * 100), EnumChatType.CommandSuccess);
        }

        private void OnPercentSubCommand(IPlayer player, int groupId, CmdArgs args)
        {
            if (args.Length > 0)
            {
                var value = args.PopFloat().GetValueOrDefault(0f);
                if (value > 1f) value /= 100;
                _settings.PlayerThreshold = GameMath.Clamp(value, 0f, 1f);
            }
            player.SendMessage(groupId, LangEntry("Command.Players", _settings.PlayerThreshold * 100), EnumChatType.CommandSuccess);
        }

        private static string LangEntry(string code, params object[] args) 
            => LangEx.FeatureString("OneBedSleeping", code, args);
        
    }
}