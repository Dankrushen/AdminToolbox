﻿using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Linq;

namespace AdminToolbox
{
    class LastAttacked
    {
        public static Player lastAttacker = null;
        public static Player lastVictim = null;
        public static DamageType lastDamageType = DamageType.NONE;
        public static DamageType last106Damage;
    }
    class DamageDetect : IEventHandlerPlayerHurt
    {
        private Plugin plugin;
        public DamageDetect(Plugin plugin)
        {
            this.plugin = plugin;
        }
        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            if (AdminToolbox.playerdict.ContainsKey(ev.Player.SteamId)) { if (AdminToolbox.playerdict[ev.Player.SteamId][1]) { ev.Damage = 0f; ev.DamageType = DamageType.NONE; ; return; }; }
            if (AdminToolbox.playerdict.ContainsKey(ev.Attacker.SteamId)) { if (AdminToolbox.playerdict[ev.Attacker.SteamId][2]) { ev.Damage = 0f; ev.DamageType = DamageType.NONE; ; return; }; }
            ev.Damage = ev.Damage;
            ev.DamageType = ev.DamageType;
            int[] allowedDmg = ConfigManager.Manager.Config.GetIntListValue("admintoolbox_tutorial_dmg_allowed",new int[] { -1 }, false);
            int[] DebugDmg = ConfigManager.Manager.Config.GetIntListValue("admintoolbox_debug_damagetypes", new int[] { 5, 13, 14, 15, 16, 17 },false);
            int[] scpDamagesTypes = { 2, 6, 7, 9, 10 };

            if (AdminToolbox.isRoundFinished)
            {
                int damageMultiplier = ConfigManager.Manager.Config.GetIntValue("admintoolbox_endedRound_damageMultiplier", 1, true);
                ev.Damage = ev.Damage * damageMultiplier;
                ev.DamageType = ev.DamageType;
                if((int)ev.Player.TeamRole.Role!=14)
                    return;
            }
            switch ((int)ev.Player.TeamRole.Role)
            {
                case -1:
                    break;
                case 2:
                    break;
                case 14:
                    if (!allowedDmg.Contains((int)ev.DamageType))
                    {
                        if (DebugDmg.Contains((int)ev.DamageType) && (ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_tutorial", false,false))) plugin.Info(ev.Player.TeamRole.Name + " " + ev.Player.Name + " not allowed damagetype: " + ev.DamageType);
                        ev.DamageType = DamageType.NONE;
                        ev.Damage = 0f;
                        return;
                    }
                    else
                        goto default;
                default:
                    if ((int)ev.Player.TeamRole.Role == 3) { LastAttacked.last106Damage = ev.DamageType; }; 
                    if ((ev.Attacker.Name == "Server" && !(ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_server", false,false))) || (ev.Attacker.Name == "Spectator" && !(ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_spectator",false, false)))) return;
                    if (AdminToolbox.nineTailsTeam.Contains((int)ev.Player.TeamRole.Team) && AdminToolbox.nineTailsTeam.Contains((int)ev.Attacker.TeamRole.Team))
                    {
                        if (ev.DamageType == DamageType.TESLA)
                        {
                            LastAttacked.lastAttacker = ev.Attacker;
                            LastAttacked.lastVictim = ev.Player;
                            LastAttacked.lastDamageType = ev.DamageType;
                            //string x = LastAttacked.lastAttacker.Name + " " + LastAttacked.lastVictim.Name + " " + LastAttacked.lastDamageType;
                            //plugin.Info(x);
                        }
                        if (ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_friendly_damage",false, false))
                        {
                            if (!DebugDmg.Contains((int)ev.DamageType)) return;
                            plugin.Warn(ev.Attacker.TeamRole.Name + " " + ev.Attacker.Name + " attacked fellow " + ev.Player.TeamRole.Name + " " + ev.Player.Name + /*" for " + damage +^*/ " with " + ev.DamageType);
                        }
                    }
                    else if(AdminToolbox.chaosTeam.Contains((int)ev.Player.TeamRole.Team) && AdminToolbox.chaosTeam.Contains((int)ev.Attacker.TeamRole.Team))
                    {
                        if (ev.DamageType == DamageType.TESLA)
                        {
                            LastAttacked.lastAttacker = ev.Attacker;
                            LastAttacked.lastVictim = ev.Player;
                            LastAttacked.lastDamageType = ev.DamageType;
                        }
                        if (ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_friendly_damage",false, false))
                        {
                            if (!DebugDmg.Contains((int)ev.DamageType)) return;
                            plugin.Warn(ev.Attacker.TeamRole.Name + " " + ev.Attacker.Name + " attacked fellow " + ev.Player.TeamRole.Name + " " + ev.Player.Name + /*" for " + damage +^*/ " with " + ev.DamageType);
                        }
                    }
                    else if (ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_player_damage",false, false))
                    {
                        if (!DebugDmg.Contains((int)ev.DamageType)) return;
                        plugin.Info(ev.Attacker.TeamRole.Name + " " + ev.Attacker.Name + " attacked " + ev.Player.TeamRole.Name + " " + ev.Player.Name + " for " + ev.Damage + " damage" + " with: " + ev.DamageType);
                    }
                    break;
            }
        }
    }
    class DieDetect : IEventHandlerPlayerDie
    {
        private Plugin plugin;
        public DieDetect(Plugin plugin)
        {
            this.plugin = plugin;
        }
        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            ev.SpawnRagdoll = true;
            if (AdminToolbox.isRoundFinished) return;
            if (ev.Player.Name == "Server" || ev.Killer.Name == "Server") { ev.SpawnRagdoll = false; return; }
            switch ((int)ev.Player.TeamRole.Role)
            {
                case -1:
                    return;
                case 2:
                    return;
                case 3:
                    if ((int)LastAttacked.last106Damage == 1 || (int)LastAttacked.last106Damage == 8)
                        ev.SpawnRagdoll = false;
                    goto default;
                default:
                    //plugin.Info("OnPlayerDie: \n" + LastAttacked.lastAttacker.Name + " " + LastAttacked.lastVictim.Name + " " + LastAttacked.lastDamageType);

                    if (!(ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_scp_and_self_killed",false, false)) && ev.Player.Name == ev.Killer.Name) return;
                    if (AdminToolbox.nineTailsTeam.Contains((int)ev.Player.TeamRole.Team) && AdminToolbox.nineTailsTeam.Contains((int)ev.Killer.TeamRole.Team))
                    {
                        if (ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_friendly_kill", true, false))
                            if ((LastAttacked.lastDamageType == DamageType.TESLA) && (ev.Player.SteamId == LastAttacked.lastVictim.SteamId && ev.Killer.SteamId == LastAttacked.lastAttacker.SteamId))
                            {
                                plugin.Warn(ev.Killer.TeamRole.Name + " " + ev.Killer.Name + " granaded fellow " + ev.Player.TeamRole.Name + " " + ev.Player.Name);
                                LogHandler.WriteToLog(ev.Killer.TeamRole.Name.ToString() + " " + ev.Killer.Name.ToString() + " granaded fellow " + ev.Player.TeamRole.Name.ToString() + " " + ev.Player.Name.ToString());
                                LastAttacked.lastAttacker = null;
                                LastAttacked.lastVictim = null;
                                LastAttacked.lastDamageType = DamageType.NONE;
                            }
                            else
                            {
                                plugin.Warn(ev.Killer.TeamRole.Name + " " + ev.Killer.Name + " killed fellow " + ev.Player.TeamRole.Name + " " + ev.Player.Name);
                                LogHandler.WriteToLog(ev.Killer.TeamRole.Name.ToString() + " " + ev.Killer.Name.ToString() + " killed fellow " + ev.Player.TeamRole.Name.ToString() + " " + ev.Player.Name.ToString());
                            }
                    }
                    else if (AdminToolbox.chaosTeam.Contains((int)ev.Player.TeamRole.Team) && AdminToolbox.chaosTeam.Contains((int)ev.Killer.TeamRole.Team))
                    {
                        if (ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_friendly_kill", true, false))
                            if ((LastAttacked.lastDamageType == DamageType.TESLA) && (ev.Player.SteamId == LastAttacked.lastVictim.SteamId && ev.Killer.SteamId == LastAttacked.lastAttacker.SteamId))
                            {
                                plugin.Warn(ev.Killer.TeamRole.Name + " " + ev.Killer.Name + " granaded fellow " + ev.Player.TeamRole.Name + " " + ev.Player.Name);
                                LogHandler.WriteToLog(ev.Killer.TeamRole.Name.ToString() + " " + ev.Killer.Name.ToString() + " granaded fellow " + ev.Player.TeamRole.Name.ToString() + " " + ev.Player.Name.ToString());
                                LastAttacked.lastAttacker = null;
                                LastAttacked.lastVictim = null;
                                LastAttacked.lastDamageType = DamageType.NONE;
                            }
                            else
                            {
                                plugin.Warn(ev.Killer.TeamRole.Name + " " + ev.Killer.Name + " killed fellow " + ev.Player.TeamRole.Name + " " + ev.Player.Name);
                                LogHandler.WriteToLog(ev.Killer.TeamRole.Name.ToString() + " " + ev.Killer.Name.ToString() + " killed fellow " + ev.Player.TeamRole.Name.ToString() + " " + ev.Player.Name.ToString());
                            }
                    }
                    else if ((ConfigManager.Manager.Config.GetBoolValue("admintoolbox_debug_player_kill",false, false)))
                    {
                        plugin.Info(ev.Killer.Name + " killed: " + ev.Player.Name);
                    }
                    break;
            }
        }
    }
}