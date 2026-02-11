using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class LuminitePusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<LuminitePusher>();
        public override int MaxCooldown => 60; 
        public override bool CanPushMachines => true; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<LuminitePusher>();
        }
    }
}