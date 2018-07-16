using Rocket.API.DependencyInjection;
using Rocket.API.Eventing;
using Rocket.API.User;
using Rocket.Core.Eventing;
using Rocket.Core.I18N;
using Rocket.Core.Player.Events;
using Rocket.Core.Plugins;
using Rocket.Core.User;
using System;
using System.IO;
using System.Numerics;

namespace DeathManager
{
    public class Main : Plugin<Config>
    {
        public string DeathLogFile = System.IO.Directory.GetCurrentDirectory() + @"\Plugins\DeathManager\DeathLogs.txt";
        public Main(IDependencyContainer container) : base("DeathManager", container)
        {
            
        }
        protected override void OnLoad(bool isFromReload)
        {
            Logger.Log("[DeathManager] Made by: Jonnyfencing1");
            Logger.Log("[DeathManager] Check out my personal plugin website https://tridentplugins.me");
            Logger.Log("----------------------------------------------");
            if (ConfigurationInstance.PluginEnabled == true)
            {
                Logger.Log("[DeathManager] KickOnDeath " + ConfigurationInstance.KickOnDeath);
                Logger.Log("[DeathManager] Message Colour set to " + ConfigurationInstance.MessageColour);
                Logger.Log("[DeathManager] MakeDeathLogFile " + ConfigurationInstance.DeathLogFile);
                if (File.Exists(DeathLogFile)) {
                    Logger.Log("[DeathManager] DeathLog File Exists, Skipping Creation Process!");
                }
                else
                {
                    Logger.Log("[DeathManager] No DeathLog File Found, Creating Now!");
                    System.IO.File.Create(DeathLogFile).Close();
                }
            }
            else
            {
                Logger.Log("[DeathManager] This Plugin is not enabled!");
            }
            Logger.Log("[DeathManager] Loaded Successfully! Thanks for Downloading!");
        }
    }
    public class Config
    {
        public bool PluginEnabled = true;
        public bool KickOnDeath = true;
        public bool DeathLogFile = false;
        public string MessageColour = "Cyan";
    }
    public class EventListener : IEventListener<PlayerDeathEvent>
    {
        private Main _plugin;

        public EventListener(Main plugin)
        {
            _plugin = plugin;
        }
        public void HandleEvent(IEventEmitter emitter, PlayerDeathEvent @event)
        {
            if (_plugin.ConfigurationInstance.PluginEnabled != true)
            {
                return;
            }
            if (_plugin.ConfigurationInstance.KickOnDeath == true)
            {
                @event.User.Kick();
                File.AppendAllText(System.IO.Directory.GetCurrentDirectory() + @"\Plugins\AdvancedLogger\DeathLogs.txt", "[" + DateTime.Now + "] " + @event.Player.Name + " (" + @event.Player.Id + ")" + ": \"" + @event.Killer + "\"" + System.Environment.NewLine);
                IUserManager globalUserManager = _plugin.Container.Resolve<IUserManager>();
                string message = "[Kill] " + @event.Player.Name + "By " + @event.Killer;
                globalUserManager.Broadcast(null, message, Rocket.API.Drawing.Color.Purple);
            }
        }
    }
    internal class DeathNote
    {
        public DeathNote()
        {
        }

        public Vector3 Position { get; set; }
        public DateTime TimeOfDeath { get; set; }
    }
}
