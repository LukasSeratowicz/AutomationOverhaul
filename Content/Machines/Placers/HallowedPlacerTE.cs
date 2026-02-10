using Terraria; // Needed for Main
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Placers
{
    public class HallowedPlacerTE : BasePlacerTE 
    {
        public override int MaxCooldown => 300; 
        public override int ValidTileType => ModContent.TileType<HallowedPlacer>();

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ValidTileType; 
        }
        public override bool CanPlaceMachines => true; 
    }
}