using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using AutomationOverhaul.Content.Items.Placeable; 

namespace AutomationOverhaul.Content.Machines.Breakers
{
    public class PlatinumBreaker : ModTile
    {
        public override string Texture => "AutomationOverhaul/Assets/Tiles/Breakers/PlatinumBreaker";

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileBlockLight[Type] = true;

            MineResist = 2.5f;
            MinPick = 45;
            HitSound = SoundID.Tink;
            DustType = DustID.Platinum;

            RegisterItemDrop(ModContent.ItemType<PlatinumBreakerItem>());

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true; 
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }

        public override void PlaceInWorld(int i, int j, Item item) {
            Point16 pos = new Point16(i, j);
            if (TileEntity.ByPosition.TryGetValue(pos, out TileEntity existingTE)) {
                TileEntity.ByID.Remove(existingTE.ID);
                TileEntity.ByPosition.Remove(pos);
            }
            
            int id = ModContent.GetInstance<PlatinumBreakerTE>().Place(i, j);
            if (id != -1 && TileEntity.ByID.TryGetValue(id, out TileEntity te) && te is PlatinumBreakerTE machine) {
                machine.CalculateSpeed();
                machine.CooldownTimer = machine.MaxCooldown;
            }
        }
        
        public override bool CanPlace(int i, int j) {
            Tile tile = Main.tile[i, j];
            if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileCut[tile.TileType]) return false;
            return HasSolidNeighbor(i, j) || Main.tile[i, j].WallType > WallID.None;
        }

        private bool HasSolidNeighbor(int x, int y) {
            if (IsSolid(x, y - 1)) return true;
            if (IsSolid(x, y + 1)) return true;
            if (IsSolid(x - 1, y)) return true;
            if (IsSolid(x + 1, y)) return true;
            return false;
        }

        private bool IsSolid(int x, int y) {
            if (!WorldGen.InWorld(x, y)) return false;
            Tile t = Main.tile[x, y];
            return t.HasTile && Main.tileSolid[t.TileType] && !Main.tileCut[t.TileType];
        }

        public override bool RightClick(int i, int j) {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te) && te is BaseBreakerTE breaker) {
                return HandleInteraction(i, j, breaker);
            }
            return false;
        }

        private bool HandleInteraction(int i, int j, BaseBreakerTE breaker) {
            Player player = Main.LocalPlayer;
            Item heldItem = player.HeldItem;

            if (!heldItem.IsAir && (heldItem.pick > 0 || heldItem.axe > 0)) {
                
                if (!breaker.IsItemValidForSlot(heldItem)) return true;

                if (breaker.internalItem.IsAir) {
                    breaker.internalItem = heldItem.Clone();
                    heldItem.TurnToAir();
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(i * 16, j * 16));
                }
                else {
                    Item temp = breaker.internalItem.Clone();
                    breaker.internalItem = heldItem.Clone();
                    heldItem.SetDefaults(temp.type); 
                    heldItem.Prefix(temp.prefix);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(i * 16, j * 16));
                }
                
                breaker.CalculateSpeed();
                breaker.CooldownTimer = breaker.MaxCooldown;
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, breaker.ID, i, j);
                return true;
            }
            
            // 2. EXTRACT (Empty Hand)
            else if (heldItem.IsAir && !breaker.internalItem.IsAir) {
                player.QuickSpawnItem(new EntitySource_TileInteraction(player, i, j), breaker.internalItem, breaker.internalItem.stack);
                breaker.internalItem.TurnToAir();
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(i * 16, j * 16));
                
                breaker.CalculateSpeed();
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, breaker.ID, i, j);
                return true;
            }

            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            if (!fail) {
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te) && te is BaseBreakerTE breaker) {
                    if (!breaker.internalItem.IsAir) {
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, breaker.internalItem);
                    }
                }
                ModContent.GetInstance<PlatinumBreakerTE>().Kill(i, j);
            }
        }
    }
}