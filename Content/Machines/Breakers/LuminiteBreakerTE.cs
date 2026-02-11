using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class LuminiteBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 10;
        public override float TierSpeedMultiplier => 3f;
        public override int ValidTileType => ModContent.TileType<LuminiteBreaker>();
    }
}