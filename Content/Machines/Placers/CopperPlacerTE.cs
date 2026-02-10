using Terraria; // Needed for Main
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Placers
{
    public class CopperPlacerTE : BasePlacerTE 
    {
        public override int MaxCooldown => 3000; 
        public override int ValidTileType => ModContent.TileType<CopperPlacer>();

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ValidTileType; 
        }
        public override bool CanPlaceMachines => false; 
    }
}