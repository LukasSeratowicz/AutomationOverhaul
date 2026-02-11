using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class HellstonePusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<HellstonePusher>();
        public override int MaxCooldown => 1050; 
        public override bool CanPushMachines => true; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<HellstonePusher>();
        }
    }
}