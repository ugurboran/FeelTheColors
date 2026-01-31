# 🌈 Feel The Colors!

<div align="center">

![Feel The Colors Banner](https://via.placeholder.com/800x200/FF6B6B/FFFFFF?text=FEEL+THE+COLORS!)

**A fast-paced color-matching arcade game for Android**

[![Unity Version](https://img.shields.io/badge/Unity-6000.2.6f2-black.svg?style=flat&logo=unity)](https://unity.com)
[![Platform](https://img.shields.io/badge/Platform-Android-green.svg?style=flat&logo=android)](https://www.android.com)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Released-success.svg?style=flat)]()

[Download on Google Play](#) | [View Demo](#) | [Report Bug](https://github.com/ugurboran/feelthecolors/issues)

</div>

---

## 📖 About

**Feel The Colors!** is an addictive one-tap arcade game where you switch your ball's color to match incoming obstacles. Test your reflexes, beat your high score, and express yourself with dynamic facial animations!

### ✨ Key Features

- 🎮 **Simple Controls** - One tap to switch colors
- 🌈 **Beautiful Effects** - Particle systems, smooth animations, and color trails
- 😊 **Expressive Character** - Dynamic facial expressions that react to gameplay
- 📈 **Progressive Difficulty** - Game speeds up as you improve
- 🎵 **Audio System** - Background music and sound effects with independent controls
- 🏆 **High Score Tracking** - Beat your personal best
- ⚙️ **Customizable Settings** - Toggle music/sound independently
- 📱 **Offline Play** - No internet required
- 🆓 **Completely Free** - No ads, no in-app purchases

---

## 🎮 Gameplay

<div align="center">

| Start Screen | Gameplay | Game Over |
|:---:|:---:|:---:|
| ![Start](https://via.placeholder.com/250x445) | ![Gameplay](https://via.placeholder.com/250x445) | ![GameOver](https://via.placeholder.com/250x445) |

</div>

### How to Play

1. **Tap** anywhere to switch your ball's color
2. **Match** the color of incoming obstacles
3. **Pass through** matching obstacles to score points
4. **Avoid** mismatched colors - wrong color = Game Over!
5. **Survive** as long as you can and beat your high score!

---

## 🛠️ Technical Details

### Built With

- **Engine:** Unity 6000.2.6f2
- **Language:** C#
- **Animation:** DOTween
- **Platform:** Android (API 22+)
- **Architecture:** ARM64

### Core Systems

- **Player Controller** - One-tap color switching with input buffering
- **Obstacle Spawner** - Dynamic difficulty scaling based on score
- **Particle System** - Color-matched particle effects for visual feedback
- **Face Controller** - Expression system with 4 states (normal, happy, excited, sad)
- **Audio Manager** - Singleton pattern with separate music/SFX controls
- **Trail Renderer** - Smooth color-matched trail effect
- **Data Persistence** - PlayerPrefs for high score and settings

### Project Structure
```
Assets/
├── Scenes/
│   ├── MainScene.unity          # Main menu
│   └── GameScene.unity          # Gameplay
├── Scripts/
│   ├── Player/
│   │   ├── BallController.cs
│   │   ├── FaceController.cs
│   │   └── PlayerSubtleMovement.cs
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── AudioManager.cs
│   │   ├── PauseManager.cs
│   │   └── MenuManager.cs
│   ├── Obstacles/
│   │   ├── Obstacle.cs
│   │   └── ObstacleSpawner.cs
│   └── Effects/
│       └── BackgroundGradient.cs
├── Sprites/
│   ├── Player/
│   ├── Obstacles/
│   └── Faces/
├── Audio/
│   ├── Music/
│   └── SFX/
└── Prefabs/
    ├── Player.prefab
    └── Obstacle.prefab
```

---

## 🚀 Getting Started

### Prerequisites

- Unity 6000.2.6f2 or later
- Android Build Support module
- DOTween (Free version)

### Installation

1. **Clone the repository**
```bash
   git clone https://github.com/yourusername/feelthecolors.git
   cd feelthecolors
```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" and select the project folder
   - Open the project with Unity 6000.2.6f2

3. **Install DOTween**
   - Window > Package Manager
   - Search for "DOTween" in Asset Store
   - Import DOTween (free)
   - Tools > Demigiant > DOTween Utility Panel > Setup

4. **Build Settings**
   - File > Build Settings
   - Switch platform to Android
   - Add MainScene and GameScene to build
   - Configure Player Settings (see below)

### Build Configuration

**Player Settings (Edit > Project Settings > Player):**
```
Product Name: Feel The Colors!
Package Name: com.yourname.feelthecolors
Version: 1.0
Bundle Version Code: 1

Minimum API Level: Android 5.1 (API 22)
Target API Level: Automatic
Scripting Backend: IL2CPP
Target Architectures: ARM64 ✓
```

---

## 📱 Building for Android

### APK Build
```bash
# In Unity
1. File > Build Settings
2. Platform: Android
3. Build System: Gradle
4. Click "Build"
5. Save as FeelTheColors_v1.0.apk
```

### AAB Build (Google Play)
```bash
# In Unity
1. File > Build Settings
2. Player Settings > Publishing Settings
3. Build App Bundle (Google Play): ✓
4. Create/Select Keystore
5. Build > Save as FeelTheColors_v1.0.aab
```

---

## 🎨 Customization

### Adding New Colors

Edit `BallController.cs`:
```csharp
public Color[] availableColors = new Color[]
{
    new Color(1f, 0f, 0f),    // Red
    new Color(0f, 1f, 0f),    // Green
    new Color(0f, 0f, 1f),    // Blue
    new Color(1f, 1f, 0f),    // Yellow
    // Add more colors here
};
```

### Adjusting Difficulty

Edit `ObstacleSpawner.cs`:
```csharp
private float baseSpawnInterval = 2f;    // Initial spawn rate
private float minSpawnInterval = 0.5f;   // Fastest spawn rate
private int speedIncreaseScore = 5;      // Points needed for speed increase
```

### Changing Face Expressions

Replace sprites in `Assets/Sprites/Faces/`:
- `face_normal.png` - Default expression
- `face_happy.png` - On successful pass
- `face_excited.png` - On color change
- `face_sad.png` - On game over

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 Changelog

### Version 1.0.0 (2025-01-19)

#### Features
- ✨ Initial release
- 🎮 One-tap color switching gameplay
- 😊 Dynamic facial expression system
- 🌈 Particle effects and trail renderer
- 🎵 Background music with independent volume control
- 🔊 Sound effects system
- 📈 Progressive difficulty scaling
- 🏆 High score persistence
- ⏸️ Pause menu
- ⚙️ Settings panel (music/sound toggles)

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **DOTween** by Demigiant - Animation library
- **Unity Technologies** - Game engine
- **Face Sprites** - [Source if applicable]
- **Sound Effects** - [Source if applicable]
- **Music** - [Source if applicable]

---

## 📧 Contact

**Developer:** Uğur BORAN 
**Email:** ugurborangamedeveloper@gmail.com  

**Project Link:** [https://github.com/ugurboran/feelthecolors](https://github.com/ugurboran/feelthecolors)

---

<div align="center">

### ⭐ Star this repository if you enjoyed the game!

Made with ❤️ and Unity

</div>
