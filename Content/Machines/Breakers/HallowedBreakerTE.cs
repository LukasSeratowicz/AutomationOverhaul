using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class HallowedBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 6;
        public override float TierSpeedMultiplier => 20f;
        public override int ValidTileType => ModContent.TileType<HallowedBreaker>();
    }
}