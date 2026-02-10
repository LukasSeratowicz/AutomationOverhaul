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

        // --- 1. RANGE LOGIC ---
        public int GetMaxPossibleRange() {
            int toolBoost = internalItem.IsAir ? 0 : internalItem.tileBoost;
            
            // Axes get +1 range automatically (to reach tree tops or behind walls)
            int axeBonus = (internalItem.axe > 0) ? 1 : 0;

            // Base(2) + ToolBoost + TierBonus + AxeBonus
            int val = 2 + toolBoost + TierRangeBonus + axeBonus;
            return Math.Max(1, val);
        }

        public bool IsItemValidForSlot(Item item) => item.pick > 0 || item.axe > 0;

        // --- SAVING ---
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
        
        private int _lastTileType = -1;

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            if (!IsActive || internalItem.IsAir || (internalItem.pick <= 0 && internalItem.axe <= 0)) {
                _lastTileType = -1;
                CooldownTimer = MaxCooldown; 
                return;
            }

            Point16 target = GetTargetTile();
            Tile tile = Main.tile[target.X, target.Y];
            
            int currentType = tile.HasTile ? tile.TileType : -1;

            if (currentType != _lastTileType) {
                CalculateSpeed(); 
                CooldownTimer = MaxCooldown;
                _lastTileType = currentType;
            }

            if (!WorldGen.InWorld(target.X, target.Y) || !tile.HasTile) {
                return;
            }

            if (!CanMineTile(target.X, target.Y, tile)) {
                return;
            }

            if (CooldownTimer > 0) {
                CooldownTimer--;
                return;
            }

            bool success = OnProcess();
            
            if (success) {
                _lastTileType = -1; 
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
            
            CooldownTimer = MaxCooldown; 
        }

        private bool CanMineTile(int x, int y, Tile tile) {
            // MODDED CONTAINER & TILE ENTITY CHECK
            if (TileEntity.ByPosition.ContainsKey(new Point16(x, y))) {
                return false; 
            }

            // VANILLA CHEST
            if (TileID.Sets.BasicChest[tile.TileType] || TileID.Sets.BasicChestFake[tile.TileType]) {
                 return false;
            }

            bool isTree = tile.TileType == TileID.Trees || tile.TileType == TileID.PalmTree || tile.TileType == TileID.Cactus;
            
            if (internalItem.axe > 0) {
                if (isTree) return true;
                if (internalItem.pick <= 0) return false; 
            }

            if (internalItem.pick > 0) {
                if (isTree && internalItem.axe <= 0) return false;

                int requiredPower = GetPickaxeRequirement(tile.TileType);
                if (internalItem.pick < requiredPower) {
                    return false;
                }
                return true;
            }

            return false;
        }

        private int GetPickaxeRequirement(int type) {
            // Modded Tiles handling
            if (TileLoader.GetTile(type) is ModTile modTile) {
                return modTile.MinPick;
            }

            return type switch {
                // Unbreakable by machines
                TileID.DemonAltar => 9999, 

                // Jungle Temple
                TileID.LihzahrdBrick => 210,
                TileID.LihzahrdAltar => 210,
                TileID.Traps => 210,
                
                // Hardmode Tier 5
                TileID.Chlorophyte => 200,
                
                // Hardmode Tier 3
                TileID.Adamantite => 150,
                TileID.Titanium => 150,
                
                // Hardmode Tier 2
                TileID.Mythril => 110,
                TileID.Orichalcum => 110,
                
                // Hardmode Tier 1 & Dungeon
                TileID.Cobalt => 100,
                TileID.Palladium => 100,
                TileID.BlueDungeonBrick => 100,
                TileID.GreenDungeonBrick => 100,
                TileID.PinkDungeonBrick => 100,
                
                // Corruption / Crimson / Hell / Hallow
                TileID.Ebonstone => 65,
                TileID.Crimstone => 65,
                TileID.Pearlstone => 65,
                TileID.Hellstone => 65,
                
                // Evil Ores & Obsidian
                TileID.Demonite => 55,
                TileID.Crimtane => 55,
                TileID.Obsidian => 55,
                
                // Meteorite
                TileID.Meteorite => 50,
                
                // Default (Dirt, Wood, Stone, Copper, Iron, Gold, etc.)
                _ => 0 
            };
        }

        protected override bool OnProcess() {
            Point16 target = GetTargetTile();
            if (!WorldGen.InWorld(target.X, target.Y)) return false;

            Tile tile = Main.tile[target.X, target.Y];
            if (!tile.HasTile) return false;

            bool isTree = tile.TileType == TileID.Trees || tile.TileType == TileID.PalmTree || tile.TileType == TileID.Cactus;
            bool isStaff = internalItem.type == ItemID.StaffofRegrowth || internalItem.type == ItemID.AcornAxe;

            WorldGen.KillTile(target.X, target.Y, false, false, false);

            if (!Main.tile[target.X, target.Y].HasTile) {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Tink, new Vector2(target.X * 16, target.Y * 16));
                
                if (isTree && isStaff) {
                    WorldGen.PlaceTile(target.X, target.Y, TileID.Saplings, mute: true);
                }

                CalculateSpeed();

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
            if (useTime < 2) useTime = 2;

            Point16 target = GetTargetTile();
            bool targetIsTree = false;

            if (WorldGen.InWorld(target.X, target.Y)) {
                Tile tile = Main.tile[target.X, target.Y];
                if (tile.HasTile) {
                    if (tile.TileType == TileID.Trees || tile.TileType == TileID.PalmTree || tile.TileType == TileID.Cactus) {
                        targetIsTree = true;
                    }
                }
            }

            bool useAxeLogic = targetIsTree && internalItem.axe > 0;

            if (useAxeLogic) {
                float axePower = internalItem.axe * 5f; 
                float baseDelay = (300f - axePower); 
                
                float speedFactor = useTime * TierSpeedMultiplier;

                float finalDelay = baseDelay + speedFactor;
                
                if (finalDelay < 20) finalDelay = 20;
                _currentMaxCooldown = (int)finalDelay;
            }
            else {
                float baseDelay = useTime * TierSpeedMultiplier;
                float powerRefund = internalItem.pick * 2.0f;

                float finalDelay = baseDelay - powerRefund;
                
                if (finalDelay < 20) finalDelay = 20;
                _currentMaxCooldown = (int)finalDelay;
            }
        }

        private Point16 GetTargetTile() {
            Tile tile = Main.tile[Position.X, Position.Y];
            int style = (tile.TileFrameX / 18) % 4;
            Vector2 dir = style switch {
                0 => new Vector2(0, -1),
                1 => new Vector2(1, 0),
                2 => new Vector2(0, 1),
                3 => new Vector2(-1, 0),
                _ => new Vector2(0, -1)
            };
            
            int maxPossible = GetMaxPossibleRange();
            int actualRange = System.Math.Min(CurrentRangeSetting, maxPossible);

            return new Point16(Position.X + (int)(dir.X * actualRange), Position.Y + (int)(dir.Y * actualRange));
        }

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ValidTileType;
        }
    }
}