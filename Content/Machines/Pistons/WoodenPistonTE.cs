using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO; 
using Microsoft.Xna.Framework;
using AutomationOverhaul.Core.Abstracts;

namespace AutomationOverhaul.Content.Machines.Pistons
{
    public class WoodenPistonTE : KineticMachine
    {
        public override int MaxCooldown => 61; // 1 Second

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (!IsActive) return;

            Tile tile = Main.tile[Position.X, Position.Y];
            Vector2 dir = GetDirectionFromFrame(tile);
            int targetX = Position.X + (int)dir.X;
            int targetY = Position.Y + (int)dir.Y;

            if (!WorldGen.InWorld(targetX, targetY) || !Main.tile[targetX, targetY].HasTile) {
                CooldownTimer = MaxCooldown; 
                return;
            }

            if (CooldownTimer > 0) {
                CooldownTimer--;
                return;
            }

            if (OnProcess()) {
                CooldownTimer = MaxCooldown;
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }

        protected override bool OnProcess() {
            Tile tile = Main.tile[Position.X, Position.Y];
            Vector2 direction = GetDirectionFromFrame(tile);
            
            Point targetPos = new Point(Position.X + (int)direction.X, Position.Y + (int)direction.Y);
            Point movePos   = new Point(targetPos.X + (int)direction.X, targetPos.Y + (int)direction.Y);

            if (!WorldGen.InWorld(targetPos.X, targetPos.Y) || !WorldGen.InWorld(movePos.X, movePos.Y)) return false;

            Tile sourceTile = Main.tile[targetPos.X, targetPos.Y];
            Tile destTile = Main.tile[movePos.X, movePos.Y];

            if (!sourceTile.HasTile) return false; 

            if (destTile.HasTile) {
                DoJamEffects(targetPos);
                CooldownTimer = MaxCooldown; 
                return false; 
            }

            bool hasBrain = TileEntity.ByPosition.TryGetValue(new Point16(targetPos.X, targetPos.Y), out TileEntity sourceTE);

            destTile.CopyFrom(sourceTile);
            sourceTile.ClearTile();

            if (hasBrain && sourceTE != null) {
                MoveTileEntity(sourceTE, movePos.X, movePos.Y);
            }

            WorldGen.SquareTileFrame(movePos.X, movePos.Y, true);
            WorldGen.SquareTileFrame(targetPos.X, targetPos.Y, true);

            var pistonSound = new Terraria.Audio.SoundStyle("AutomationOverhaul/Assets/Sounds/PistonPush") {
                Volume = 0.8f,
                PitchVariance = 0.2f,
                MaxInstances = 10,
            };

            Terraria.Audio.SoundEngine.PlaySound(pistonSound, new Vector2(Position.X * 16, Position.Y * 16));

            NetMessage.SendTileSquare(-1, targetPos.X, targetPos.Y, 3);
            NetMessage.SendTileSquare(-1, movePos.X, movePos.Y, 3);
            
            return true;
        }

        private void MoveTileEntity(TileEntity sourceTE, int destX, int destY) {
            TileEntity.ByPosition.Remove(sourceTE.Position);
            sourceTE.Position = new Point16(destX, destY);
            TileEntity.ByPosition[sourceTE.Position] = sourceTE;

            // Reset cooldown of pushed machine to prevent air-firing
            if (sourceTE is KineticMachine machine) {
                 machine.CooldownTimer = machine.MaxCooldown;
            }
        }

        private void DoJamEffects(Point pos) {
            if (Main.netMode == NetmodeID.Server) return;

            for (int k = 0; k < 5; k++) {
                Dust d = Dust.NewDustDirect(new Vector2(pos.X * 16, pos.Y * 16), 16, 16, DustID.Torch);
                d.velocity *= 2.5f;
                d.noGravity = true;
            }
        }

        private Vector2 GetDirectionFromFrame(Tile tile) {
            int style = (tile.TileFrameX / 18) % 4; 
            return style switch {
                0 => new Vector2(0, -1),
                1 => new Vector2(1, 0),
                2 => new Vector2(0, 1),
                3 => new Vector2(-1, 0),
                _ => new Vector2(0, -1)
            };
        }

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<WoodenPiston>();
        }
    }
}