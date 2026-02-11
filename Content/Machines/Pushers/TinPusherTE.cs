using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class TinPusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<TinPusher>();
        public override int MaxCooldown => 3000; 
        public override bool CanPushMachines => false; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<TinPusher>();
        }
    }
}