# Automation Overhaul

![Banner Image Placeholder :)](https://github.com/LukasSeratowicz/AutomationOverhaul/blob/main/MechanicalOverhaulThumbnail.jpg)
placeholder :)

**Current Version:** 0.02 (pre Alpha)  
**TModLoader Version:** 1.4.4+  
**Steam Workshop:** [Subscribe Here](https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXXXXXX) *(No workshop yet sorry)*

## ⚙️ Overview
**Automation Overhaul** introduces true kinetic machinery to Terraria. Unlike magic-based tech mods, afk chests, auto gatherers, etc., this project focuses on physical interaction: moving blocks, pushing machines, and automated extraction.

The core philosophy is **Balanced Progression** and **Tools not solutions**. You start with primitive wooden components, and end up with luminite god-tiered machines. The whole system aims to enable creativity and doesn't offer ANY actual all-in-one tiles. You need stone? Build a quary. How? We give you pistons, pushers, diggers, hoppers, item suckers, and many other tools. Go figure it out.

## 📸 Gallery

| **Pistons in action** | **Rotator Tool** |
|:---:|:---:|
| ![Piston GIF](https://github.com/LukasSeratowicz/AutomationOverhaul/blob/main/loopingPistons.gif) | ![Rotator GIF](https://github.com/LukasSeratowicz/AutomationOverhaul/blob/main/RotatorLooping.gif) |
| *Pushing a block in an endless loop* | *Rotating machines for convenience* |

---

## 🛠 Features & Mechanics

### 1. The Piston System
The key element of this mod is to introduce tools, not solutions. For example pistons:

* **Stack Pushing:** Pistons detect the entire line of blocks in front of them.
* **Machine Pushing:** Unlocked at **Tier 3 (Silver)**. Allows moving other machines.
* **Tiers:** Speed and push limit for extra grind and feel of progression.

### 2. Tiered Progression
Performance scales drastically. Do not expect end-game speed from a Wooden Piston.

| Tier | Material | Cooldown | Push Limit | Can Push Machines? |
| :--- | :--- | :--- | :--- | :--- |
| **0** | Wood | 60s | 1 | ❌ |
| **1** | Copper / Tin | 50s | 1 | ❌ |
| **2** | Iron / Lead | 40s | 2 | ❌ |
| **3** | Silver / Tungsten | 30s | 2 | ✅ |
| **...** | ... | ... | ... | ... |
| **7** | Cobalt / Palladium | 12.5s | 5 | ✅ |
| **12** | **Luminite** | **1s** | **10** | ✅ |
(mod support in the future)

## 📝 Roadmap / To-Do List

### Engineering Tools
- [x] **Rotator:** A smart tool that detects `Machine` entities. Allows rotating placed machines without breaking them or resetting their internal buffers.
- [x] **Range Tuner:** Modifies the range of the machine. Works for: Breaker
- [x] **State Flipper:** Turns Machines ON or OFF in an area. Three modes: ALL ON | ALL OFF | TOGGLE STATES.
- [ ] **Configurator (name placeholder):** Tool to change how a machine behaves. Requires machines that have different modes etc.
---

### Machines
- [x] **Piston:** Moves set of tiles forward. Has maximum push limit. Can move other machines if upgraded. Tiered throughout the whole progression.
- [x] **Placer:** Has 1 equipment slot. Places blocks periodically from the way it is facing. Tiered throughout the whole progression.
- [x] **Breaker:** Has 1 equipment slot for a pickaxe/drill or Axe (hammer in the future). Mines a block in front of it with a fraction of the speed of the tool inside. Tiered throughout the whole progression.
- [x] **Pusher:** Pushes a block AND itself forwards 1 tile. It has a static push limit of itself + 1. Only goes forward if it has a block in front of it. Tiered throughout the whole progression.
- [ ] **Collector:** Collects an area of items on the ground to its inventory. Speed and the area of the sweep depends on its tier. Tiered throughout the whole progression.
- [ ] **Hopper:** Takes items from machines (and maybe chests) and forwards them. Tiered throughout the whole progression.
- [ ] **Item Teleporter(placeholder name):** Teleports items within to a connected chest. Tiered throughout the whole progression.
- [ ] **Fan Blower(placeholder name):** Blows items on the ground away. Tiered throughout the whole progression.
- [ ] **many more to come**

### Armor
Nothing planned yet.
### Accessories
- [ ] **State Switcher(placeholder name):** Placed machines will automatically be turned on or off depending on the mode of this accessory.

### To-Do
Here is what I'm working on right now:
- [ ] **Mod Icon:** An AI generated (modified by me) placeholder is to be changed for an actual human-painted one.
- [ ] **All assets:** Placeholders need to be replaced
- [ ] **Alpha Workshop Release** After an Icon and Assets we already have 4 machines and 3 tools. The project grew and with it bugs and UX mistakes. It's perfect time for playtesting. 
- [ ] **Crafting and Balancing:** After release the next main focus will be on changing the materials required for each tier and balancing all machines a bit.
- [ ] **Collector:** After that it will be time for new features like next machine Collector that picks up items on the ground and stores them inside its equipment.
---

## 💻 Installation (For Developers)
This project is and most likely will remain open source :) I always wanted a mod like this, loved LuiAFK, and finally decided to make one myself. Feel free to clone it, change aspects you don't like, or add your own features.

1.  Clone the repo.
2.  Modify whatever you want.
3.  Build inside tModLoader.
Details will be provided after I test it on another device.

---

## 📜 License
Code: GPL-3.0 license
Assets: Attribution-Non Commercial-No Derivatives 4.0 International
