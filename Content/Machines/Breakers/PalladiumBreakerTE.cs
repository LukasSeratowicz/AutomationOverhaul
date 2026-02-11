using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class PalladiumBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 3;
        public override float TierSpeedMultiplier => 50f;
        public override int ValidTileType => ModContent.TileType<PalladiumBreaker>();
    }
}