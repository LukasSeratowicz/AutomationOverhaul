using Terraria;
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public class CrimtanePusherTE : BasePusherTE
    {
        public override int TargetTileID => ModContent.TileType<CrimtanePusher>();
        public override int MaxCooldown => 1350; 
        public override bool CanPushMachines => true; 
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<CrimtanePusher>();
        }
    }
}