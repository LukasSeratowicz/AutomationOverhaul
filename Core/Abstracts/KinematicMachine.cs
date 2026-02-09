using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutomationOverhaul.Core.Abstracts
{
    public abstract class KineticMachine : ModTileEntity
    {
        public abstract int TargetTileID { get; }
        public int CooldownTimer { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        
        public abstract int MaxCooldown { get; }

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (!IsActive) return;

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

        public override void NetSend(System.IO.BinaryWriter writer) {
            writer.Write(CooldownTimer);
            writer.Write(IsActive);
        }

        public override void NetReceive(System.IO.BinaryReader reader) {
            CooldownTimer = reader.ReadInt32();
            IsActive = reader.ReadBoolean();
        }
    }
}