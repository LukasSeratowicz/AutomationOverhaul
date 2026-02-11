using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Terraria.GameContent;
using AutomationOverhaul.Content.Machines.Placers;
using AutomationOverhaul.Content.Machines.Pistons;
using AutomationOverhaul.Content.Machines.Breakers;
using AutomationOverhaul.Content.Machines.Pushers;
using AutomationOverhaul.Core.Abstracts;

namespace AutomationOverhaul.Content.Systems
{
    public class DebugSystem : ModSystem
    {
        public override void PostDrawTiles() {
            if (!ModContent.GetInstance<AutomationOverhaulConfig>().DebugMode) return;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (var kvp in TileEntity.ByID) {
                if (kvp.Value is not KineticMachine machine) continue;

                Vector2 worldPos = new Vector2(machine.Position.X * 16 + 8, machine.Position.Y * 16 + 8);
                Vector2 screenPos = worldPos - Main.screenPosition;

                if (screenPos.X < -50 || screenPos.X > Main.screenWidth + 50 || 
                    screenPos.Y < -50 || screenPos.Y > Main.screenHeight + 50) 
                    continue;

                Color color = Color.White;
                string text = $"{machine.CooldownTimer}";

                // --- DISABLED (RED) ---
                if (!machine.IsActive) {
                    color = Color.Red;
                    text = $"F{machine.CooldownTimer}";
                }
                else {
                    Tile tile = Main.tile[machine.Position.X, machine.Position.Y];
                    
                    int frameX = tile.TileFrameX;
                    int dirX = 0, dirY = 0;
                    
                    // Simple rotation extraction (standard 4-way)
                    // 0=Up(0,-1), 1=Right(1,0), 2=Down(0,1), 3=Left(-1,0)
                    int style = (frameX / 18) % 4;
                    if (style == 0) dirY = -1;
                    else if (style == 1) dirX = 1;
                    else if (style == 2) dirY = 1;
                    else if (style == 3) dirX = -1;

                    int targetX = machine.Position.X + dirX;
                    int targetY = machine.Position.Y + dirY;
                    bool targetHasTile = WorldGen.InWorld(targetX, targetY) && Main.tile[targetX, targetY].HasTile;

                    // --- PLACER LOGIC ---
                    if (machine is BasePlacerTE placer) {
                        if (placer.internalItem.IsAir || placer.internalItem.createTile < 0) {
                            color = Color.Cyan; // Idle (Empty)
                        } else if (targetHasTile) {
                            color = Color.Orange; // Blocked
                        } else {
                            color = Color.Lime; // Working
                        }
                    }
                    // --- PISTON LOGIC ---
                    else if (machine is BasePistonTE) {
                        if (!targetHasTile) {
                            color = Color.Cyan; // Idle (Nothing to push)
                        } else {
                            color = Color.Lime; // Working (Ready to push)
                        }
                    }
                    // --- BREAKER LOGIC ---
                    else if (machine is BaseBreakerTE breaker) {
                        int range = System.Math.Min(breaker.CurrentRangeSetting, breaker.GetMaxPossibleRange());
                        int breakX = machine.Position.X + (dirX * range);
                        int breakY = machine.Position.Y + (dirY * range);
                        bool breakTargetExists = WorldGen.InWorld(breakX, breakY) && Main.tile[breakX, breakY].HasTile;

                        if (breaker.internalItem.IsAir || breaker.internalItem.pick <= 0) {
                            color = Color.Cyan; // Idle (No Tool)
                        } else if (!breakTargetExists) {
                            color = Color.Orange; // Waiting (No Block)
                        } else {
                            color = Color.Lime; // Working (Mining)
                        }
                    }
                    // --- PUSHER LOGIC ---
                    else if (machine is BasePusherTE pusher) {
                        int destX = machine.Position.X + (dirX * 2);
                        int destY = machine.Position.Y + (dirY * 2);
                        
                        bool destHasTile = WorldGen.InWorld(destX, destY) && Main.tile[destX, destY].HasTile;
                        bool destIsBlocked = destHasTile && !Main.tileCut[Main.tile[destX, destY].TileType];
                        
                        bool targetIsMachine = TileEntity.ByPosition.ContainsKey(new Point16(targetX, targetY));

                        if (!targetHasTile) {
                            color = Color.Cyan; // Idle
                        } 
                        else if (destIsBlocked) {
                            color = Color.Orange; // Blocked by wall
                        } 
                        else if (targetIsMachine && !pusher.CanPushMachines) {
                            color = Color.Purple; // Blocked by Machine (Tier too low)
                        } 
                        else {
                            color = Color.Lime; // Working
                        }
                    }
                }

                // 3. DRAW
                Vector2 textSize = FontAssets.MouseText.Value.MeasureString(text);
                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch, 
                    FontAssets.MouseText.Value, 
                    text, 
                    screenPos - (textSize * 0.2f),
                    color, 
                    0f, 
                    Vector2.Zero, 
                    new Vector2(0.4f, 0.4f)
                );
            }

            spriteBatch.End();
        }
    }
}