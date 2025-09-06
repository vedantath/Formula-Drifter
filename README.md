# Formula Drifter

Formula Drifter is a Unity-based multiplayer top-down racing game. It features Formula 1 style cars, custom drift physics, and an immersive driving experience

## Features
- Formula 1 inspired car models and assets
- Multiple tracks and racing environments
- Realistic physics powered by Fusion Physics
- ML-Agents integration for AI and machine learning
- EasyRoads3D for custom road and track creation
- SFX and visual effects for enhanced gameplay
- TextMesh Pro for advanced text rendering

## Project Structure
- `Assets/` - Contains all game assets, including models, textures, scenes, scripts, prefabs, audio, and plugins.
- `Library/`, `obj/`, `Temp/`, `UserSettings/` - Unity-generated folders for build and project settings.
- `Packages/` - Unity package management files.
- `ProjectSettings/` - Project configuration files.

## To Play
1. Clone or download the repository.
2. Open Build/FDrifter 4-30-24
3. Select Host client or Shared Client multiplayer style. (SharedClient is the most recent)
   
Shared Client vs. Host Client

Host Client: The player who creates the game lobby. They run both the game server logic and play as a client at the same time. The host is responsible for synchronizing the game state for all other players.
Shared Client: Any other player who joins the lobby. They connect to the host client, receive game state updates, and send their inputs to be processed by the host.

4. Open the Formula Drifter executable application

## Requirements
- Unity (recommended version specified in `ProjectSettings/`)
- .NET Framework (for C# scripts)
- ML-Agents (for AI features)
- EasyRoads3D (for track editing)

## Packages
- EasyRoads3D
- ML-Agents
- TextMesh Pro
- Fusion Physics
