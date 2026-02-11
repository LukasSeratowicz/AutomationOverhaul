using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class PalladiumPusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<PalladiumPusher>();
        public override int MaxCooldown => 750; 
        public override bool CanPushMachines => true; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<PalladiumPusher>();
        }
    }
}