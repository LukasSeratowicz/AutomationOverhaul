using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class ChlorophytePusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<ChlorophytePusher>();
        public override int MaxCooldown => 180; 
        public override bool CanPushMachines => true; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<ChlorophytePusher>();
        }
    }
}