using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using AutomationOverhaul.Content.Items.Placeable; 

namespace AutomationOverhaul.Content.Machines.Placers
{
    public class CopperPlacer : ModTile
    {
        public override string Texture => "AutomationOverhaul/Assets/Tiles/Placers/CopperPlacer";

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoAttach[Type] = false;
            Main.tileBlockLight[Type] = true;

            MineResist = 1.5f;
            MinPick = 0;
            HitSound = SoundID.Dig; 
            DustType = DustID.Copper;

            RegisterItemDrop(ModContent.ItemType<CopperPlacerItem>());

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
            
            int id = ModContent.GetInstance<CopperPlacerTE>().Place(i, j);
            if (id != -1 && TileEntity.ByID.TryGetValue(id, out TileEntity te) && te is CopperPlacerTE machine) {
                machine.CooldownTimer = machine.MaxCooldown;
            }
        }

        public override bool CanPlace(int i, int j) {
            Tile tile = Main.tile[i, j];
            if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileCut[tile.TileType]) {
                return false;
            }
            if (HasSolidNeighbor(i, j) || Main.tile[i, j].WallType > WallID.None) {
                return true;
            }
            return false;
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
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te) && te is BasePlacerTE placer) {
                Player player = Main.LocalPlayer;
                Item heldItem = player.HeldItem;

                if (!heldItem.IsAir && heldItem.createTile >= TileID.Dirt) {
                    
                    if (!placer.IsItemValidForSlot(heldItem)) {
                        return true;
                    }

                    // ------------------------------------

                    if (placer.internalItem.IsAir) {
                        placer.internalItem = heldItem.Clone();
                        heldItem.TurnToAir();
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(i * 16, j * 16));
                    }
                    else if (placer.internalItem.type == heldItem.type && placer.internalItem.stack < placer.internalItem.maxStack) {
                        int spaceLeft = placer.internalItem.maxStack - placer.internalItem.stack;
                        int toMove = System.Math.Min(spaceLeft, heldItem.stack);
                        placer.internalItem.stack += toMove;
                        heldItem.stack -= toMove;
                        if (heldItem.stack <= 0) heldItem.TurnToAir();
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(i * 16, j * 16));
                    }
                }
                else if (heldItem.IsAir && !placer.internalItem.IsAir) {
                    int itemIndex = Item.NewItem(new EntitySource_TileInteraction(player, i, j), i * 16, j * 16, 16, 16, placer.internalItem.type, placer.internalItem.stack);
                    if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1f);
                    placer.internalItem.TurnToAir();
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(i * 16, j * 16));
                }
                
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, placer.ID, i, j);
                return true;
            }
            return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            if (!fail) {
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te) && te is BasePlacerTE placer) {
                    if (!placer.internalItem.IsAir) {
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, placer.internalItem.type, placer.internalItem.stack);
                    }
                }
                ModContent.GetInstance<CopperPlacerTE>().Kill(i, j);
            }
        }

        public override bool Slope(int i, int j) => false;
    }
}