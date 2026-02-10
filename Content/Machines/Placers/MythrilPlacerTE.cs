using Terraria; // Needed for Main
using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Placers
{
    public class MythrilPlacerTE : BasePlacerTE 
    {
        public override int MaxCooldown => 600; 
        public override int ValidTileType => ModContent.TileType<MythrilPlacer>();

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ValidTileType; 
        }
        public override bool CanPlaceMachines => true; 
    }
}