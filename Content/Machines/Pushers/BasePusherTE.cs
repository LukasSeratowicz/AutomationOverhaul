using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO; 
using Microsoft.Xna.Framework;
using AutomationOverhaul.Core.Abstracts;

namespace AutomationOverhaul.Content.Machines.Pushers
{
    public abstract class BasePusherTE : KineticMachine
    {
        public abstract bool CanPushMachines { get; } 
        public virtual float PitchVariance => 0.2f; 

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (!IsActive) { CooldownTimer = MaxCooldown; return; }

            // [Pusher(0)] -> [Target(1)] -> [Destination(2)]
            Tile tile = Main.tile[Position.X, Position.Y];
            Vector2 dir = GetDirectionFromFrame(tile);
            
            int targetX = Position.X + (int)dir.X;
            int targetY = Position.Y + (int)dir.Y;
            
            int destX = Position.X + (int)(dir.X * 2);
            int destY = Position.Y + (int)(dir.Y * 2);

            if (!WorldGen.InWorld(targetX, targetY) || !Main.tile[targetX, targetY].HasTile) {
                CooldownTimer = MaxCooldown; 
                return; 
            }
            
            if (!WorldGen.InWorld(destX, destY)) {
                CooldownTimer = MaxCooldown;
                return; 
            }

            if (CooldownTimer > 0) {
                CooldownTimer--;
                return;
            }

            if (OnProcess()) {
                CooldownTimer = MaxCooldown;
            }
        }

        protected override bool OnProcess() {
            Tile tile = Main.tile[Position.X, Position.Y];
            Vector2 dir = GetDirectionFromFrame(tile);

            // Coordinates
            int myX = Position.X;
            int myY = Position.Y;

            int targetX = myX + (int)dir.X;
            int targetY = myY + (int)dir.Y;

            int destX = myX + (int)(dir.X * 2);
            int destY = myY + (int)(dir.Y * 2);

            Tile destTile = Main.tile[destX, destY];
            if (destTile.HasTile && !Main.tileCut[destTile.TileType]) {
                DoJamEffects(new Point(targetX, targetY));
                CooldownTimer = MaxCooldown;
                return false;
            }

            if (TileEntity.ByPosition.TryGetValue(new Point16(targetX, targetY), out TileEntity targetTE)) {
                if (!CanPushMachines) {
                    DoJamEffects(new Point(targetX, targetY));
                    CooldownTimer = MaxCooldown;
                    return false;
                }
            }

            Tile targetTile = Main.tile[targetX, targetY];
            
            if (destTile.HasTile) WorldGen.KillTile(destX, destY, false, false, true);

            destTile.CopyFrom(targetTile);
            targetTile.ClearTile();

            if (TileEntity.ByPosition.TryGetValue(new Point16(targetX, targetY), out TileEntity te)) {
                MoveTileEntity(te, destX, destY);
            }

            Tile myTile = Main.tile[myX, myY];
            targetTile.CopyFrom(myTile);
            myTile.ClearTile();

            MoveTileEntity(this, targetX, targetY);

            // Visuals
            WorldGen.SquareTileFrame(destX, destY, true);
            WorldGen.SquareTileFrame(targetX, targetY, true);
            WorldGen.SquareTileFrame(myX, myY, true);

            NetMessage.SendTileSquare(-1, destX, destY, 1);
            NetMessage.SendTileSquare(-1, targetX, targetY, 1);
            NetMessage.SendTileSquare(-1, myX, myY, 1);

            // Audio
            var pistonSound = new Terraria.Audio.SoundStyle("AutomationOverhaul/Assets/Sounds/PistonPush") {
                Volume = 0.8f,
                PitchVariance = PitchVariance, 
                MaxInstances = 10,
            };
            Terraria.Audio.SoundEngine.PlaySound(pistonSound, new Vector2(targetX * 16, targetY * 16));

            // Sync
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);

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