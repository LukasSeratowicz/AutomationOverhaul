using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class TinBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 0;
        public override float TierSpeedMultiplier => 145f;
        public override int ValidTileType => ModContent.TileType<TinBreaker>();
    }
}