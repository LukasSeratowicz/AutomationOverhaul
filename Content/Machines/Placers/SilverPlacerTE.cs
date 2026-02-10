using Terraria; // Needed for Main
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Placers
{
    public class SilverPlacerTE : BasePlacerTE 
    {
        public override int MaxCooldown => 1800; 
        public override int ValidTileType => ModContent.TileType<SilverPlacer>();

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ValidTileType; 
        }
        public override bool CanPlaceMachines => false; 
    }
}