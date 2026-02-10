using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AutomationOverhaul
{
    public class AutomationOverhaulConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        
        [DefaultValue(false)]
        public bool DebugMode { get; set; }
    }
}