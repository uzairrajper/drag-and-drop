# Unity Drag & Drop Machines Project

## Overview
This Unity project demonstrates drag-and-drop mechanics used in games for inventory, puzzle matching, and machine placement.

### Features
- Two main scripts:
  - **UIDragTo3D.cs** → Handles spawning coloured prefabs from UI.
  - **Object.cs** → Handles object matching, scaling, and material copying.
- Prefabs:
  - **Default prefab** → Placed in scene, no colors.
  - **Coloured prefabs** → Spawned via drag & drop, interact with default prefabs.

### How it Works
1. Drag UI prefab → spawns coloured prefab in scene.
2. If dragged object matches a target (tag check), it scales up slightly.
3. On drop → copies materials into target object and destroys dragged prefab.

### Setup
1. Clone repo:
   ```bash
   git clone https://github.com/YourUsername/Unity-DragDrop-Machines.git
