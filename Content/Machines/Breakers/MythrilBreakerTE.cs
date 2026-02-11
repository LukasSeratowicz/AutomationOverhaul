using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class MythrilBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 3;
        public override float TierSpeedMultiplier => 40f;
        public override int ValidTileType => ModContent.TileType<MythrilBreaker>();
    }
}