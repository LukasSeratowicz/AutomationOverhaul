using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutomationOverhaul.Core.Abstracts
{
    public abstract class KineticMachine : ModTileEntity
    {
        public int CooldownTimer { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        
        public abstract int MaxCooldown { get; }

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (!IsActive) return;

            // REMOVED: WoodenPistonTE.ManageGlobalReset(); <--- This was causing the error

            if (CooldownTimer > 0) {
                CooldownTimer--;
                return;
            }

            if (OnProcess()) {
                CooldownTimer = MaxCooldown;
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }

        protected abstract bool OnProcess();

        public override void SaveData(TagCompound tag) {
            tag["timer"] = CooldownTimer;
            tag["active"] = IsActive;
        }

        public override void LoadData(TagCompound tag) {
            CooldownTimer = tag.GetInt("timer");
            IsActive = tag.GetBool("active");
        }
    }
}