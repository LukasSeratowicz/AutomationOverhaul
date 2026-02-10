using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AutomationOverhaul.Core.Abstracts;
using System;

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public abstract class BaseBreakerTE : KineticMachine
    {
        public override int TargetTileID => ValidTileType; 
        public abstract int ValidTileType { get; }
        public abstract int TierRangeBonus { get; } 
        public abstract float TierSpeedMultiplier { get; }

        public Item internalItem = new Item(); 
        protected int _currentMaxCooldown = 60; 
        public override int MaxCooldown => _currentMaxCooldown;
        public int CurrentRangeSetting = 1;
        
        public int GetMaxPossibleRange() {
            int toolBoost = internalItem.IsAir ? 0 : internalItem.tileBoost;
            // Copper(-1) -> 2 + (-1) = 1.
            // Iron(0) -> 2 + 0 = 2.
            int val = 2 + toolBoost + TierRangeBonus;
            return Math.Max(1, val);
        }

        public bool IsItemValidForSlot(Item item) => item.pick > 0;

        public override void SaveData(TagCompound tag) {
            tag["Timer"] = CooldownTimer;
            tag["IsActive"] = IsActive;
            tag["Max"] = _currentMaxCooldown;
            tag["Range"] = CurrentRangeSetting;
            tag["Item"] = ItemIO.Save(internalItem);
        }

        public override void LoadData(TagCompound tag) {
            if (tag.ContainsKey("IsActive")) IsActive = tag.GetBool("IsActive");
            if (tag.ContainsKey("Timer")) CooldownTimer = tag.GetInt("Timer");
            if (tag.ContainsKey("Max")) _currentMaxCooldown = tag.GetInt("Max");
            if (tag.ContainsKey("Range")) CurrentRangeSetting = tag.GetInt("Range");
            internalItem = ItemIO.Load(tag.GetCompound("Item"));
        }
        
        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            if (!IsActive || internalItem.IsAir || internalItem.pick <= 0) {
                CalculateSpeed(); 
                CooldownTimer = MaxCooldown;
                return;
            }

            Point16 target = GetTargetTile();
            
            // PASSIVE BLOCKING
            if (!WorldGen.InWorld(target.X, target.Y) || !Main.tile[target.X, target.Y].HasTile) {
                CooldownTimer = MaxCooldown;
                return;
            }

            if (CooldownTimer > 0) {
                CooldownTimer--;
                return;
            }

            bool success = OnProcess();
            CalculateSpeed(); 
            CooldownTimer = MaxCooldown; 

            if (success) {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }

        protected override bool OnProcess() {
            Point16 target = GetTargetTile();
            if (!WorldGen.InWorld(target.X, target.Y)) return false;

            Tile tile = Main.tile[target.X, target.Y];
            if (!tile.HasTile) return false;

            WorldGen.KillTile(target.X, target.Y, false, false, false);

            if (!Main.tile[target.X, target.Y].HasTile) {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Tink, new Vector2(target.X * 16, target.Y * 16));
                WorldGen.SquareTileFrame(target.X, target.Y, true);
                NetMessage.SendTileSquare(-1, target.X, target.Y, 1);
                return true;
            }
            return false;
        }

        public void CalculateSpeed() {
            if (internalItem.IsAir) {
                _currentMaxCooldown = 60;
                return;
            }
            
            float useTime = internalItem.useAnimation; 
            float power = internalItem.pick;

            if (useTime < 2) useTime = 2;
            
            float baseDelay = useTime * TierSpeedMultiplier;
            float powerRefund = power * 2.0f;

            float finalDelay = baseDelay - powerRefund;

            if (finalDelay < 20) finalDelay = 20;

            _currentMaxCooldown = (int)finalDelay;
        }

        private Point16 GetTargetTile() {
            Tile tile = Main.tile[Position.X, Position.Y];
            int style = (tile.TileFrameX / 18) % 4;
            Vector2 dir = style switch {
                0 => new Vector2(0, -1), // Up
                1 => new Vector2(1, 0),  // Right
                2 => new Vector2(0, 1),  // Down
                3 => new Vector2(-1, 0), // Left
                _ => new Vector2(0, -1)
            };
            
            return new Point16(Position.X + (int)(dir.X * CurrentRangeSetting), Position.Y + (int)(dir.Y * CurrentRangeSetting));
        }

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ValidTileType;
        }
    }
}