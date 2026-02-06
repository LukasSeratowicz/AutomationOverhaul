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
    public abstract class BasePistonTE : KineticMachine
    {
        public abstract int PushDistance { get; }
        public abstract bool CanPushMachines { get; } 
        public virtual float PitchVariance => 0.2f; 

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
            Vector2 dir = GetDirectionFromFrame(tile);
            
            int blocksToPush = 0;
            bool foundGap = false;

            for (int i = 1; i <= PushDistance + 1; i++) {
                int checkX = Position.X + (int)(dir.X * i);
                int checkY = Position.Y + (int)(dir.Y * i);

                if (!WorldGen.InWorld(checkX, checkY)) {
                    DoJamEffects(new Point(checkX, checkY));
                    CooldownTimer = MaxCooldown;
                    return false;
                }

                Tile checkTile = Main.tile[checkX, checkY];

                if (!checkTile.HasTile) {
                    if (i == 1) return false; 
                    
                    foundGap = true;
                    blocksToPush = i - 1; 
                    break;
                }

                if (TileEntity.ByPosition.TryGetValue(new Point16(checkX, checkY), out TileEntity te)) {
                    if (!CanPushMachines) {
                        DoJamEffects(new Point(checkX, checkY));
                        CooldownTimer = MaxCooldown;
                        return false;
                    }
                }
            }

            // LIMIT CHECK
            if (!foundGap) {
                Point jamPos = new Point(Position.X + (int)(dir.X), Position.Y + (int)(dir.Y));
                DoJamEffects(jamPos);
                CooldownTimer = MaxCooldown;
                return false;
            }

            // MOVE PHASE (Back to Front)
            // We iterate backwards from the gap to the start
            // Example: Push 2 blocks. Gap at 3.
            // Move 2 -> 3
            // Move 1 -> 2
            // Clear 1
            for (int i = blocksToPush; i >= 1; i--) {
                int srcX = Position.X + (int)(dir.X * i);
                int srcY = Position.Y + (int)(dir.Y * i);
                
                int destX = Position.X + (int)(dir.X * (i + 1));
                int destY = Position.Y + (int)(dir.Y * (i + 1));

                Tile srcTile = Main.tile[srcX, srcY];
                Tile destTile = Main.tile[destX, destY];

                destTile.CopyFrom(srcTile);
                srcTile.ClearTile();

                if (TileEntity.ByPosition.TryGetValue(new Point16(srcX, srcY), out TileEntity srcTE)) {
                    MoveTileEntity(srcTE, destX, destY);
                }

                WorldGen.SquareTileFrame(destX, destY, true);
                WorldGen.SquareTileFrame(srcX, srcY, true);
                NetMessage.SendTileSquare(-1, destX, destY, 3);
                NetMessage.SendTileSquare(-1, srcX, srcY, 3);
            }

            // Audio
            var pistonSound = new Terraria.Audio.SoundStyle("AutomationOverhaul/Assets/Sounds/PistonPush") {
                Volume = 0.8f,
                PitchVariance = PitchVariance, 
                MaxInstances = 10,
            };
            Terraria.Audio.SoundEngine.PlaySound(pistonSound, new Vector2(Position.X * 16, Position.Y * 16));
            
            return true;
        }

        protected void MoveTileEntity(TileEntity sourceTE, int destX, int destY) {
            TileEntity.ByPosition.Remove(sourceTE.Position);
            sourceTE.Position = new Point16(destX, destY);
            TileEntity.ByPosition[sourceTE.Position] = sourceTE;

            if (sourceTE is KineticMachine machine) {
                 machine.CooldownTimer = machine.MaxCooldown;
            }
        }

        protected void DoJamEffects(Point pos) {
            if (Main.netMode == NetmodeID.Server) return;
            for (int k = 0; k < 16; k++) {
                Dust d = Dust.NewDustDirect(new Vector2(pos.X * 16, pos.Y * 16), 16, 16, DustID.Torch);
                d.velocity *= 1.69f; 
                d.noGravity = true;
            }
        }

        protected Vector2 GetDirectionFromFrame(Tile tile) {
            int style = (tile.TileFrameX / 18) % 4; 
            return style switch {
                0 => new Vector2(0, -1),
                1 => new Vector2(1, 0),
                2 => new Vector2(0, 1),
                3 => new Vector2(-1, 0),
                _ => new Vector2(0, -1)
            };
        }
    }
}