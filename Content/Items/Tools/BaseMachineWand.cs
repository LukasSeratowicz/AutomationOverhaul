using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using AutomationOverhaul.Core.Abstracts;

namespace AutomationOverhaul.Content.Items.Tools
{
    public abstract class BaseStateFlipper : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WireKite;  //todo: replace with custom texture
        public int Mode = 0; //  0 = Toggle, 1 = Enable (On), 2 = Disable (Off)
        public bool isDragging = false;
        public Point startPoint = Point.Zero;
        public virtual int MaxRange => 5;

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.White;
            Item.autoReuse = false;
        }

        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player) {
            if (player.altFunctionUse == 2) {
                Mode++;
                if (Mode > 2) Mode = 0;

                string textChat = Mode switch {
                    0 => "Mode: [c/FFFF00:Toggle]",
                    1 => "Mode: [c/00FF00:Enable All]",
                    2 => "Mode: [c/FF0000:Disable All]",
                    _ => ""
                };
                Main.NewText(textChat);

                string text = "";
                Color color = Color.White;

                switch (Mode) {
                    case 0: 
                        text = "Mode: Toggle"; 
                        color = Color.Yellow; 
                        break;
                    case 1: 
                        text = "Mode: Enable All"; 
                        color = Color.Lime; 
                        break;
                    case 2: 
                        text = "Mode: Disable All"; 
                        color = Color.Red; 
                        break;
                }

                PopupText.NewText(new AdvancedPopupRequest() {
                    Text = text,
                    Color = color,
                    DurationInFrames = 60,
                    Velocity = new Vector2(0, -3) // up slowly
                }, player.Center - new Vector2(0, 40)); // above head

                

                return true;
            }
            return false;
        }
        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) {
            tooltips.Add(new TooltipLine(Mod, "Controls", "Left click to Select\nRight click to Switch Mode"));
            tooltips.Add(new TooltipLine(Mod, "Area", $"Area: {MaxRange}x{MaxRange}"));

            string modeName = "";
            Color modeColor = Color.White;

            switch (Mode) {
                case 0: 
                    modeName = "Toggle"; 
                    modeColor = Color.Yellow; 
                    break;
                case 1: 
                    modeName = "Enable All"; 
                    modeColor = Color.Lime; 
                    break;
                case 2: 
                    modeName = "Disable All"; 
                    modeColor = Color.Red; 
                    break;
            }

            var modeLine = new TooltipLine(Mod, "Mode", $"Current Mode: {modeName}") {
                OverrideColor = modeColor
            };
            tooltips.Add(modeLine);
        }

        public override void HoldItem(Player player) {
            if (player.whoAmI != Main.myPlayer) return;

            if (Main.mouseLeft && !isDragging) {
                isDragging = true;
                startPoint = Main.MouseWorld.ToTileCoordinates(); 
            }

            if (isDragging) {
                Point rawEnd = Main.MouseWorld.ToTileCoordinates();
                
                int rangeLimit = MaxRange - 1;
                int dx = Math.Clamp(rawEnd.X - startPoint.X, -rangeLimit, rangeLimit);
                int dy = Math.Clamp(rawEnd.Y - startPoint.Y, -rangeLimit, rangeLimit);
                
                int x1 = Math.Min(startPoint.X, startPoint.X + dx);
                int x2 = Math.Max(startPoint.X, startPoint.X + dx);
                int y1 = Math.Min(startPoint.Y, startPoint.Y + dy);
                int y2 = Math.Max(startPoint.Y, startPoint.Y + dy);

                DrawSelectionBox(x1, y1, x2, y2);

                if (!Main.mouseLeft) {
                    isDragging = false;
                    ApplyAction(x1, y1, x2, y2);
                }
            }
        }

        private void ApplyAction(int x1, int y1, int x2, int y2) {
            bool playedSound = false;

            for (int i = x1; i <= x2; i++) {
                for (int j = y1; j <= y2; j++) {
                    if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te)) {
                        if (te is KineticMachine machine) {
                            
                            bool oldState = machine.IsActive;
                            switch (Mode) {
                                case 0: machine.IsActive = !machine.IsActive; break; 
                                case 1: machine.IsActive = true; break;              
                                case 2: machine.IsActive = false; break;             
                            }

                            if (machine.IsActive != oldState) {
                                Tile tile = Main.tile[i, j];
                                tile.TileFrameY = (short)(machine.IsActive ? 0 : 18); 
                                
                                Color dustColor = machine.IsActive ? Color.LimeGreen : Color.Red;
                                for(int k=0; k<5; k++) {
                                    Dust d = Dust.NewDustPerfect(new Vector2(i * 16 + 8, j * 16 + 8), DustID.TintableDust, Main.rand.NextVector2Circular(2, 2), 0, dustColor, 1.5f);
                                    d.noGravity = true;
                                }

                                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, machine.ID, i, j);
                                NetMessage.SendTileSquare(-1, i, j, 1);
                                playedSound = true;
                            }
                        }
                    }
                }
            }
            if (playedSound) Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
        }

        private void DrawSelectionBox(int x1, int y1, int x2, int y2) {
            Color color = Mode switch {
                0 => Color.Yellow, 
                1 => Color.Lime,  
                2 => Color.Red,    
                _ => Color.White
            };

            Vector2 pos1 = new Vector2(x1 * 16, y1 * 16);
            Vector2 pos2 = new Vector2((x2 + 1) * 16, (y2 + 1) * 16); 
            
            float width = pos2.X - pos1.X;
            float height = pos2.Y - pos1.Y;

            int step = 8; // half block

            for (float k = 0; k <= width; k += step) {
                SpawnSafeDust(pos1 + new Vector2(k, 0), color);      // Top
                SpawnSafeDust(pos1 + new Vector2(k, height), color); // Bottom
            }

            for (float k = 0; k <= height; k += step) {
                SpawnSafeDust(pos1 + new Vector2(0, k), color);      // Left
                SpawnSafeDust(pos1 + new Vector2(width, k), color);  // Right
            }
        }

        private void SpawnSafeDust(Vector2 pos, Color color) {
            int dustType = ModContent.DustType<Content.Dusts.SelectionDust>();
            Dust d = Dust.NewDustPerfect(pos, dustType, Vector2.Zero, 0, color, 0.5f);
            d.noGravity = true; 
            d.noLight = true;
        }
    }
}