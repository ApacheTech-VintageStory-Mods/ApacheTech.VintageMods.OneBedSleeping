// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment

namespace ApacheTech.VintageMods.OneBedSleeping.Features.OneBedSleeping.Patches
{
    [SettingsConsumer(EnumAppSide.Server)]
    [HarmonySidedPatch(EnumAppSide.Server)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ModSleepingServerPatches : WorldSettingsConsumer<OneBedSleepingSettings>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ModSleeping), "AreAllPlayersSleeping")]
        public static bool Patch_ModSleeping_AreAllPlayersSleeping_Prefix(ICoreServerAPI ___sapi, ref bool __result)
        {
            var allPlayers = ___sapi.World.AllOnlinePlayers
                .Cast<IServerPlayer>()
                .Where(p => p?.ConnectionState == EnumClientState.Playing)
                .Where(p => p.WorldData.CurrentGameMode != EnumGameMode.Spectator)
                .ToList();

            var playersSleeping = allPlayers.Count(p => p.Entity?.MountedOn is BlockEntityBed);
            var requiredNumberOfPlayers = GameMath.Clamp(Math.Ceiling(allPlayers.Count * Settings.PlayerThreshold), 1, allPlayers.Count);

            __result = playersSleeping >= requiredNumberOfPlayers;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ModSleeping), "ServerSlowTick")]
        public static bool Patch_ModSleeping_ServerSlowTick_Prefix(ModSleeping __instance, 
            ICoreServerAPI ___sapi, ref double ___lastTickTotalDays, IServerNetworkChannel ___serverChannel)
        {
            var value = Settings.SaturationMultiplier;
            var flag = __instance.AreAllPlayersSleeping();
            if (flag)
            {
                if (__instance.AllSleeping)
                {
                    foreach (var player in ___sapi.World.AllOnlinePlayers)
                    {
                        var behaviour = (player as IServerPlayer)?.Entity.GetBehavior<EntityBehaviorHunger>();
                        if (behaviour is null) continue;
                        var saturation = (float)(___sapi.World.Calendar.TotalDays - ___lastTickTotalDays) * (2000f * value);

                        behaviour.ConsumeSaturation(saturation);
                    }
                }
                ___lastTickTotalDays = ___sapi.World.Calendar.TotalDays;
            }

            if (flag == __instance.AllSleeping) return false;
            ___serverChannel.BroadcastPacket(new NetworksMessageAllSleepMode { On = flag });
            __instance.AllSleeping = flag;
            return false;
        }

        //[HarmonyTranspiler]
        //[HarmonyPatch(typeof(ModSleeping), "ServerSlowTick")]
        //public static IEnumerable<CodeInstruction> Patch_ModSleeping_ServerSlowTick_Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    var list = instructions.ToList();
        //    //list[35].operand = 2000 * Settings.SaturationMultiplier;
        //    return list;
        //}

    }
}