using Terraria.ModLoader;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class MeteoriteBreakerTE : BaseBreakerTE 
    {
        public override int TierRangeBonus => 2;
        public override float TierSpeedMultiplier => 70f;
        public override int ValidTileType => ModContent.TileType<MeteoriteBreaker>();
    }
}