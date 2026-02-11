using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class ChlorophyteBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 8;
        public override float TierSpeedMultiplier => 10f;
        public override int ValidTileType => ModContent.TileType<ChlorophyteBreaker>();
    }
}