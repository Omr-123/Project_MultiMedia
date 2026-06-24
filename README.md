# Project_MultiMedia — WinForms action platformer (overview from Form1.cs)

This repository contains a Windows Forms (C#) 2D action platformer implementation. The following README describes the architecture and runtime behavior inferred directly and only from Form1.cs, how hitboxes and collisions are handled, the boss behavior structure, runtime loop, controls, required assets and how to clone/build/run the project.

## What this is
A single-player 2D platformer implemented as a WinForms application. Form1.cs contains the entire game: entity models, game loop, input handling, drawing, collision detection and boss/enemy logic. Assets (images) are referenced from an `assets/` folder and are required at runtime.

### Stack
- Language(s): C# (Windows Forms)
- Framework / runtime: Windows Forms (classic WinForms / .NET Framework or .NET WindowsForms runtime)
- Notable libraries: System.Drawing, System.Windows.Forms (no external NuGet dependencies shown in Form1.cs)

## Architecture (from Form1.cs)
Form1.cs is structured around a small set of model classes plus the main Form that implements the game loop and rendering:

- Obs
  - Generic "object" used for obstacles, coins, potions, lasers, elevator segments, ladders, etc.
  - Fields: X,Y,W,H, List<Bitmap> imgs, IF (frame index), counters, Timer, spd, Type.

- CMap
  - Represents the current map and holds: Bitmap img (the map graphic), lists of Obs for Obsticals, Coins, Poition (potions), Laser, Ladder, an Elevator (Obs) and Hole (Obs).
  - Also contains Rectangle Dst/Src used when drawing the map with scaling.

- CHero
  - The player character model. Contains position (PosX/PosY), screen-relative X/Y, size W/H, movement state booleans, sprites lists for different animations (Idle, Move, Run, Jump, Bullet, etc.), bullet tracking (single and multi), hearts (life), sprint data and counters, direction, hit state (HP), and other animation counters.

- Enemy
  - Generic enemy model used for multiple enemy types (flying, soldier). Fields for animations (Idle, Move, Fly, Ready, Fist), bullets fired, position/size, HP and AI counters.

- planes
  - Small struct for the plane object (used to spawn flying enemies).

- Big_Boss
  - Boss model containing: phases/status, many animation lists (Idle, Move, Fist, Sword, Blade, Canonn, Teleport, Damage, End, Health bars), special attack coordinates (blade/teleport/cannon), animation counters and state machine fields (Status, Action, CoolDown, Time, Hit, IF_Health, etc.).

- Form1 (main form)
  - Contains all runtime state: Map1, Hero, Boss, Enemy lists, Timer `tt` (interval 5 ms), double-buffer bitmap (`off`), StartX/StartY camera offsets, ScaleX/ScaleY, level boundaries, and core logic.

How it fits together (runtime shape)
- Initialization: CreateMap(), CreateHero(), CreateBoss() load Bitmaps using explicit paths under `assets/`.
- Main loop: Timer `tt` (Tick handler Tt_Tick) calls in order:
  - Hero_Move() — update hero physics/controls
  - Map1Move() — elevator, coins, plane movement
  - LaserMove() — handle lasers and collisions
  - MoveEnemy() — enemy spawning and movement
  - MoveEnemy3() — special enemy logic
  - MoveBoss() — boss AI & attacks
  - Hero_ability() — bullets, sprinting and projectile collision handling
  - DrawBubb(CreateGraphics()) — draws the frame to an in-memory bitmap then blits to screen

## Collision and hitbox handling
- Collision detection: single helper isHit(int X1, int Y1, int W1, int H1, int X2, int Y2, int W2, int H2)
  - Implements axis-aligned rectangular collision by checking many containment cases (corner/edge overlap permutations).
  - Accepts negative widths by normalizing (if W2 < 0, convert to positive and adjust X2).
  - Many game systems call isHit using:
    - Hero.PosX, Hero.PosY, Hero.W, Hero.H for player hitbox
    - Observables' X,Y,W,H (Obs, Enemy, Boss components, bullets) for other hitboxes
  - Camera: collisions are computed in world coordinates (PosX/PosY). Rendering uses StartX/StartY and Scale to convert to screen coordinates.

- Hitbox responsibilities in code:
  - Player hitbox: computed from Hero.PosX/PosY and Hero.W/H; Hero.X and Hero.Y are screen offsets computed as Pos - Start (camera).
  - Enemy and boss hitboxes: use their X/Y and W/H fields; for some projectiles negative width is used to indicate flipping (direction) and isHit handles W2 < 0.
  - Bullets: tracked as positions (SBulletX/SBulletY for single bullet; lists for multi bullets) and tested against enemy/boss rectangles using isHit.

- "Both sides" handling (player vs enemy)
  - The code differentiates "friendly" vs "hostile" via the caller: hero bullets call isHit against enemy/boss; enemy bullets call isHit against hero.
  - Direction flipping: some sprites use negative width (W *= -1) to indicate facing direction; drawing accounts for negative width when rendering.

## Boss behavior & state machine (structure)
Big_Boss uses:
- Status: "Phase1", "Phase2", "End"
- Action: enumerated actions (Idle, Fist, Sword, Canonn, Blade, Teleport, etc.)
- Per-action animations are lists: Idle, Move, Fist, Sword, Blade, Canonn, Teleport, Damage, End
- Counters and timers:
  - IF_C and IF_Counter control animation frame stepping
  - CoolDown and Time/Time_Count/Count_Down manage when boss chooses next action and how long actions last
  - Hit/IF_Health count damage progress across health frames

Action flow (conceptual):
- While boss is active and in view, CoolDown increases until reaching threshold → choose a random Action according to the current Phase.
- Each Action sets: Boss.Current = appropriate animation list, Boss.IF = 0, Boss.IF_C and Boss.Time.
- During action:
  - Fist/Sword: boss attempts to move toward player and then perform close attack (HeroDamage with small damage values).
  - Canonn: boss prepares a cannon projectile (CanonnX), launches it across the map; BossHit checks collisions with hero.
  - Blade: boss fires a fast blade projectile across horizontally (BladeX). BladeHit uses BossHit to apply damage.
  - Teleport: boss sets TeleX near the player's X and attacks briefly.
- Phase transitions: boss accumulates Hit/IF_Health; if IF_Health reaches the end of CurrentHealth sprites, it transitions Phase1 → Phase2 or Phase2 → End.
- End state triggers a screen effect and final cutscene text.

This structure separates:
- Data (animations, coordinates) in Big_Boss fields
- Decision/timing in MoveBoss()
- Collision application via BossHit(...) → HeroDamage(...)

## Input / Controls (as implemented)
- Movement: Left / A = move left ; Right / D = move right
- Climb: Up / W; Down / S
- Jump: Space (double-jump: second Space triggers JumpDou)
- Sprint: Hold Shift
- Fire single bullet: B (sets Hero.SBulletX/Single bullet)
- Fire multi-bullets: hold left mouse button (MouseDown starts isDrag, Hero_ability adds MBullets while dragging)
- Elevator controls: Y (down) / U (up) when standing on elevator
- Restart on death: R

## Assets required (paths used verbatim in Form1.cs)
Place a folder named `assets` adjacent to the executable (project root during development). Form1.cs references many assets; representative paths include:
- assets/Maps/1.png
- assets/Hero1/Right/{frames}.png, assets/Hero1/Right/Jump/{frames}.png
- assets/Hero1/Bullets/Bullet.png and Bullet2.png
- assets/enemy1/*, assets/enemy3/*
- assets/Plane.png
- assets/big_boss/... (multiple subfolders: health, 11, 1, 3, 5, 8, 9, 2, 6, 10)
- assets/assets/coins/*, assets/assets/hearts/*, assets/assets/sprint/*, assets/assets/poitions/*, assets/assets/Elevator/*, assets/assets/laser/*
Ensure the folder structure and filenames match exactly, or Bitmap constructors will throw exceptions on load.

## How to clone and run
1. Clone the repository:
   ```
   git clone https://github.com/Omr-123/Project_MultiMedia.git
   cd Project_MultiMedia
   ```

2. Open and run in Visual Studio (recommended):
   - If a .sln/.csproj exists: open the solution with Visual Studio and set the WinForms project as the startup project.
   - If there is no solution/project file: create a new "Windows Forms App" (C#) in Visual Studio (choose .NET Framework or .NET Windows Forms runtime), then:
     - Add the existing `Form1.cs` file into the project.
     - Copy the `assets/` directory into the project output folder (or add it to the project and set files to "Copy to Output Directory").
     - Ensure `Program.cs` starts the Form1 form (standard WinForms Program entry).
   - Build and Run (F5).

3. Minimal CLI build (if you want to compile manually)
   - Create a WinForms project via dotnet or Visual Studio and include Form1.cs and assets. (Form1.cs depends on WinForms/System.Drawing types; a simple csc compile is not enough without a project file.)

Important: Form1.cs expects the `assets/` folder at runtime; missing image files will crash at Bitmap constructor calls.

## Notes, caveats and improvement suggestions (observations from Form1.cs)
- Bitmaps are created directly throughout initialization. Consider preloading once and disposing them on exit to avoid memory growth.
- Many runtime operations happen on the UI thread (Timer callbacks). If frame rate or long Bitmap operations cause stuttering, consider:
  - Using a lower-frequency timer or more efficient frame stepping
  - Pre-scaling or caching scaled images where possible
- isHit implements axis-aligned rectangle overlap via many boolean checks; it works but could be simplified and made more robust by normalizing rectangles and testing overlap via range intersection.
- Negative widths for flipped sprites are used; be careful when normalizing or storing values to avoid inconsistencies.
- Form1 uses CreateGraphics and direct drawing; consider overriding OnPaint and using double-buffering with `off` as already done (good) but avoid CreateGraphics() in non-paint events for safe painting.

## Extending the project
- Add more maps: add images under `assets/Maps` and add corresponding Map region and obstacle definitions in CreateMap or an external map loader.
- Replace hard-coded positions with level data (JSON/XML) to make maps editable without code changes.
- Move asset-loading code into a separate ResourceManager class to centralize loading and disposal.
- Improve collision system to use Rectangle/structs for clarity.

---

Author: Omar (Omr-123)  
Contributors: Zahretalola
