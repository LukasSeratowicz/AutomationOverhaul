using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class WoodenPusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<WoodenPusher>();
        public override int MaxCooldown => 3600; 
        public override bool CanPushMachines => false; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<WoodenPusher>();
        }
    }
}