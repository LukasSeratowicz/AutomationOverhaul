using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AutomationOverhaul.Content.Items.Placeable;
using AutomationOverhaul.Core.Abstracts;
using AutomationOverhaul.Content.Machines.Pistons;

namespace AutomationOverhaul.Content.Machines.Placers
{
    public abstract class BasePlacerTE : KineticMachine
    {
        public override int TargetTileID => ValidTileType; 
        public abstract int ValidTileType { get; }
        public Item internalItem = new Item(); 
        public virtual float PitchVariance => 0.1f;
        public virtual bool CanPlaceMachines => false; 

        public bool IsItemValidForSlot(Item item) {
            if (CanPlaceMachines) return true;

            // Reject Vanilla Containers
            if (item.createTile > -1 && Main.tileContainer[item.createTile]) return false;

            // DYNAMIC MACHINE CHECK
            if (IsMachineTile(item.createTile)) {
                return false; 
            }

            return true;
        }

        private static bool[] _machineTileCache;

        private bool IsMachineTile(int tileID) {
            if (tileID < 0) return false;

            if (_machineTileCache == null) {
                _machineTileCache = new bool[TileLoader.TileCount];
                
                foreach (var te in ModContent.GetContent<KineticMachine>()) {
                    int id = te.TargetTileID;
                    if (id >= 0 && id < _machineTileCache.Length) {
                        _machineTileCache[id] = true;
                    }
                }
            }

            // O(1) Lookup
            if (tileID < _machineTileCache.Length) {
                return _machineTileCache[tileID];
            }
            return false;
        }

        public override void SaveData(TagCompound tag) {
            tag["Item"] = ItemIO.Save(internalItem);
        }

        public override void LoadData(TagCompound tag) {
            internalItem = ItemIO.Load(tag.GetCompound("Item"));
        }
        
        public override void Update() {
             if (Main.netMode == NetmodeID.MultiplayerClient) return;
             if (CooldownTimer > 0) { CooldownTimer--; return; }
             
             if (OnProcess()) {
                 CooldownTimer = MaxCooldown;
                 NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
             }
        }
        
        protected override bool OnProcess() {
            if (internalItem.IsAir || internalItem.createTile < TileID.Dirt) return false;

            Tile tile = Main.tile[Position.X, Position.Y];
            Vector2 dir = GetDirectionFromFrame(tile);
            int targetX = Position.X + (int)dir.X;
            int targetY = Position.Y + (int)dir.Y;

            if (!WorldGen.InWorld(targetX, targetY)) return false;

            if (Main.tile[targetX, targetY].HasTile) {
                CooldownTimer = MaxCooldown; 
                return false; 
            }

            bool success = WorldGen.PlaceTile(targetX, targetY, internalItem.createTile, false, false, -1, internalItem.placeStyle);

            if (success) {
                // "Undo" Check (Security measure against cheats/bugs)
                if (!CanPlaceMachines) {
                    if (TileEntity.ByPosition.TryGetValue(new Point16(targetX, targetY), out TileEntity te)) {
                        if (te is BasePistonTE || te is BasePlacerTE) {
                            WorldGen.KillTile(targetX, targetY, false, false, true);
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Tink, new Vector2(targetX * 16, targetY * 16));
                            CooldownTimer = MaxCooldown;
                            return false;
                        }
                    }
                }

                internalItem.stack--;
                if (internalItem.stack <= 0) internalItem.TurnToAir();
                
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, new Vector2(targetX * 16, targetY * 16));
                WorldGen.SquareTileFrame(targetX, targetY, true);
                NetMessage.SendTileSquare(-1, targetX, targetY, 1);
                
                return true;
            }
            return false;
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