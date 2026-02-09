using Terraria;
using Terraria.ModLoader;
using AutomationOverhaul.Content.Machines.Pistons;

namespace AutomationOverhaul.Content.Machines.Pistons
{
    public class MythrilPistonTE : BasePistonTE {
        public override int TargetTileID => ModContent.TileType<MythrilPiston>();
        public override int MaxCooldown => 600; //12.5f*60;
        public override int PushDistance => 6; 
        public override bool CanPushMachines => true;
        public override float PitchVariance => 0.1f;
        
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<MythrilPiston>();
        }
    }
}