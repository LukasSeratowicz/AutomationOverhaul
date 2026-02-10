using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

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

            UpdateVisualState();

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

        private void UpdateVisualState() {
            if (Position == Point16.NegativeOne) return;
            Tile tile = Main.tile[Position.X, Position.Y];
            
            // Expected FrameY: 0 if Active, 18 if Inactive
            short expectedFrameY = (short)(IsActive ? 0 : 18);
            
            if (tile.TileFrameY != expectedFrameY) {
                tile.TileFrameY = expectedFrameY;
                if (Main.netMode == NetmodeID.Server) {
                    NetMessage.SendTileSquare(-1, Position.X, Position.Y, 1);
                }
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