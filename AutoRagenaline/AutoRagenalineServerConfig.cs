using System.ComponentModel;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AutoRagenaline
{
	class AutoRagenalineServerConfig : ModConfig
	{
        public static AutoRagenalineServerConfig Instance;
		public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        public bool EnableOrDisableSecret { get; set; }
        
		[DefaultValue("n")]
		public string Secret { get; set; }

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) {
			if (!AutoRagenaline.IsPlayerLocalServerOwner(Main.player[whoAmI])) {
				message = this.GetLocalization("YouAreNotServerOwnerCantChangeConfig").ToNetworkText();
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
	}
}