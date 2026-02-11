using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class LeadPusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<LeadPusher>();
        public override int MaxCooldown => 2400; 
        public override bool CanPushMachines => false; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<LeadPusher>();
        }
    }
}