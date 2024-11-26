using Terraria.ModLoader;
using Terraria;

namespace AutoRagenaline
{
	public class AutoRagenaline : Mod
	{
		internal static ModKeybind ragee;
		internal static ModKeybind adrenalinee;
		internal static ModKeybind secretmode;
		public override void Load() { 
			ragee = KeybindLoader.RegisterKeybind(this, "Auto Rage", "H");
			adrenalinee = KeybindLoader.RegisterKeybind(this, "Auto Adrenaline", "G");
			secretmode = KeybindLoader.RegisterKeybind(this, "Secret Mode", "J");
		}
		public static bool IsPlayerLocalServerOwner(Player player) {
			if (AutoRagenalineServerConfig.Instance.Owner == player.name) {
				return true;
			}
			if (Main.netMode == 1) {
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}
			for (int plr = 0; plr < Main.maxPlayers; plr++)
				if (Netplay.Clients[plr].State == 10 && Main.player[plr] == player && Netplay.Clients[plr].Socket.GetRemoteAddress().IsLocalHost())
					return true;
			return false;
		}
	}
}