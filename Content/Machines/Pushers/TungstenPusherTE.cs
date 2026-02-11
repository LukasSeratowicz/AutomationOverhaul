using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class TungstenPusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<TungstenPusher>();
        public override int MaxCooldown => 1800; 
        public override bool CanPushMachines => false; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<TungstenPusher>();
        }
    }
}