# Formula-Drifter

🏎️ **A thrilling 2D Formula One-style racing game with advanced drifting mechanics, AI opponents, and multiplayer support.**

## Overview

Formula-Drifter is an action-packed 2D racing game built in Unity that combines the excitement of Formula One racing with sophisticated drifting physics. Race against AI opponents powered by machine learning or challenge your friends in online multiplayer battles across custom-designed tracks.

## 🎮 Key Features

### Core Gameplay
- **Advanced Drifting Physics**: Realistic drift mechanics with customizable drift factors
- **Multiple Game Modes**: 
  - Single Player vs AI
  - Local Multiplayer (up to 2 players)
  - Online Multiplayer via Photon Fusion
- **AI Opponents**: Smart AI drivers trained using Unity ML-Agents
- **Dynamic Racing**: Lap-based racing with checkpoint systems
- **Position Tracking**: Real-time leaderboard and position management

### Technical Features
- **Photon Fusion Networking**: Seamless online multiplayer experience
- **ML-Agents Integration**: Intelligent AI behavior and learning
- **Custom Track System**: Built with EasyRoads3D for varied racing experiences
- **Advanced Physics**: 2D Rigidbody-based car physics with realistic handling
- **Audio System**: Dynamic sound effects and engine audio
- **Particle Effects**: Tire smoke, sparks, and visual feedback
- **UI System**: Comprehensive menus, HUD, and leaderboards

### Visual & Audio
- **Formula One Aesthetics**: Classic F1-inspired car designs
- **Trail Rendering**: Dynamic tire marks and drift trails
- **Particle Systems**: Realistic tire smoke and environmental effects
- **Professional UI**: Clean, racing-themed interface
- **Sound Design**: Engine sounds, tire screeching, and ambient audio

## 🚀 Getting Started

### Prerequisites
- **Unity 2021.3 LTS** or later
- **Unity ML-Agents Package** (for AI functionality)
- **Photon Fusion** (for multiplayer)
- **TextMeshPro** (included with Unity)

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/vedantath/Formula-Drifter.git
   cd Formula-Drifter
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Open" and select the project folder
   - Unity will automatically import all dependencies

3. **Setup Dependencies**
   - Ensure ML-Agents package is installed via Package Manager
   - Configure Photon Fusion settings for multiplayer (if using online features)

4. **Build and Run**
   - Open `Assets/Scenes/Menu.unity` to start from the main menu
   - Or open `Assets/Scenes/SampleScene.unity` for direct gameplay
   - Press Play to test in the editor

## 🎯 How to Play

### Controls

#### Player 1
- **Steering**: `A` / `D` or `←` / `→` Arrow Keys
- **Accelerate**: `W` or `↑` Arrow Key
- **Brake/Reverse**: `S` or `↓` Arrow Key

#### Player 2 (Local Multiplayer)
- **Steering**: Custom input axes (configurable in Unity Input Manager)
- **Accelerate/Brake**: Custom input axes

### Gameplay Tips
- **Master the Drift**: Use controlled drifting around corners to maintain speed
- **Learn the Tracks**: Study checkpoint layouts and optimal racing lines
- **AI Strategy**: AI opponents adapt to your driving style - keep them guessing!
- **Multiplayer**: Coordinate with friends or compete in ranked online matches

## 🛠️ Development

### Project Structure
```
Assets/
├── Scripts/
│   ├── CarController.cs       # Core vehicle physics
│   ├── CarAI.cs              # ML-Agents AI behavior
│   ├── CarInputHandler.cs    # Input management
│   ├── Network/              # Photon Fusion networking
│   ├── UI/                   # User interface components
│   └── ...
├── Scenes/
│   ├── Menu.unity           # Main menu
│   ├── SampleScene.unity    # Primary racing scene
│   └── OnlineMultiplayer.unity
├── Prefabs/
│   ├── Cars/                # Car prefabs and variants
│   └── ...
└── Materials/               # Visual materials and shaders
```

### Key Components

#### CarController.cs
The heart of the vehicle physics system featuring:
- Configurable acceleration and top speed
- Advanced drift mechanics
- Steering responsiveness
- Physics-based movement using Rigidbody2D

#### CarAI.cs
ML-Agents powered AI system providing:
- Intelligent path following
- Dynamic difficulty adjustment
- Learning from player behavior
- Checkpoint-based navigation

#### Networking
Photon Fusion integration for:
- Real-time multiplayer synchronization
- Lag compensation
- State management
- Player spawning and management

### Customization

#### Car Physics
Modify these parameters in `CarController.cs`:
- `driftFactor`: Controls drift intensity (0.95 default)
- `accelerationFactor`: Vehicle acceleration (30.0 default)
- `turnFactor`: Steering sensitivity (3.5 default)
- `maxSpeed`: Top speed limit (10 default)

#### AI Behavior
Train custom AI models using Unity ML-Agents:
1. Configure training environment in `CarAI.cs`
2. Set up reward systems for desired behaviors
3. Train using ML-Agents Python package
4. Import trained models into Unity

## 🔧 Dependencies

### Unity Packages
- **ML-Agents**: `com.unity.ml-agents` (AI functionality)
- **TextMeshPro**: `com.unity.textmeshpro` (UI text rendering)
- **2D Physics**: Built-in Unity 2D physics system

### Third-Party Assets
- **Photon Fusion**: Networking and multiplayer
- **EasyRoads3D**: Track and road creation tools

## 📝 Contributing

We welcome contributions! Here's how you can help:

1. **Fork the Repository**
2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Make Your Changes**
4. **Test Thoroughly**
5. **Submit a Pull Request**

### Contribution Guidelines
- Follow Unity C# coding conventions
- Test all changes in both single-player and multiplayer modes
- Update documentation for new features
- Ensure compatibility with Unity 2021.3 LTS

## 🐛 Known Issues & Troubleshooting

### Common Issues
- **Networking Problems**: Ensure Photon Fusion is properly configured
- **AI Not Working**: Verify ML-Agents package installation
- **Performance**: Adjust physics timestep for better performance on lower-end devices

### Support
- Open an issue on GitHub for bug reports
- Check Unity Console for error messages
- Verify all dependencies are correctly installed

## 📄 License

This project is open source. Please check the repository for license details.

## 🙏 Acknowledgments

- **Unity Technologies** for the game engine and ML-Agents
- **Photon** for networking solutions
- **EasyRoads3D** for track creation tools
- All contributors and testers

---

**Ready to race? Clone the repo and start your engines! 🏁**
