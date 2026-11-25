# CyberOps — Ethical Hacking Training Simulator

**A scenario-based cybersecurity training game built in Unity 6.**

CyberOps is a California State University Channel Islands Computer Science Capstone project that blends a first-person 3D workstation with simulated security investigations. Players interact with a Linux-style terminal and a fictional Windows-style reconnaissance browser to analyze evidence, solve objectives, and complete missions in a controlled environment.

> **Project status:** Playable Capstone prototype (Fall 2026) · **Target:** Windows desktop · **Engine:** Unity 6 / C#

## Why CyberOps?

Reading about security concepts is different from investigating a system. CyberOps explores a game-based approach to foundational offensive and defensive cybersecurity education by giving learners structured scenarios, hands-on simulated tools, optional hints, and feedback on their progress.

The virtual systems, accounts, documents, credentials, and search results are fictional. **CyberOps does not execute attacks against real machines, websites, or networks.**

## Playable scenarios

| Scenario | Training focus |
| --- | --- |
| **Suspicious SSH Login** | Inspect simulated authentication records and identify suspicious login activity. |
| **Compromised Backup Account** | Investigate account permissions and credentials using simulated command-line tools, including a dictionary-attack exercise. |
| **Public Exposure Reconnaissance** | Use a simulated search browser and search operators to uncover an exposed document. |
| **Exposed Service Account Investigation** | Correlate browser-discovered service-account information with evidence in the simulated Linux environment. |

## Gameplay and implemented features

- **Interactive 3D workstation:** Navigate a first-person environment and focus on the Linux and Windows monitors.
- **Custom Linux-style terminal:** Explore a virtual filesystem, inspect files and logs, and execute supported simulated commands.
- **Simulated reconnaissance browser:** Search fictional indexed content and investigate exposed documents.
- **Mission framework:** Receive briefings, track objectives, and complete scenario-specific investigation steps.
- **Progressive hints and scoring:** Request up to three tiers of hints per objective; hints reduce the mission score. Completed missions display performance feedback.
- **Player interface:** Main menu, scenario selection, phone-based mission briefings, objective display, pause/resume, restart, and return-to-menu controls.

### Basic controls

| Action | Control |
| --- | --- |
| Point at an interactive object | Mouse |
| Interact with / focus a monitor or object | **E** |
| Leave the current focused interaction | **Esc** |
| Open or navigate back through the pause menu | **Esc** |
| Run a terminal command or browser search | Type and press **Enter** |

When a terminal is active, type `help` to see available commands. The current simulated command set includes `pwd`, `ls`, `cd`, `cat`, `whoami`, `sudo`, `grep`, and `crack`. You can also use `hint` for mission assistance and `score` to view progress. These are **training simulations**, not operating-system shell access.

### Training loop

**Briefing → Investigation → Objectives → Feedback**

Each scenario introduces a problem, lets the player gather evidence through the virtual interfaces, evaluates the corresponding objectives, and provides completion feedback.

## Technology

- **Unity Editor:** `6000.3.23f1` (version recorded in `UnityProject/ProjectSettings/ProjectVersion.txt`)
- **Language:** C#
- **3D modeling:** Blender
- **Unity systems:** Input System, UI / TextMesh Pro, and Universal Render Pipeline
- **Source control:** Git and GitHub

## Run from source

1. Install **Unity Hub** and Unity Editor **6000.3.23f1**.
2. Clone the repository:

   ```bash
   git clone https://github.com/masonpamarang586/Capstone.git
   ```

3. In Unity Hub, choose **Add project from disk** and select the `UnityProject/` folder.
4. Open the project in the listed editor version and allow Unity to import its dependencies.
5. Open `Assets/MainMenu.unity` and press **Play**.
6. Choose a scenario from the in-game menu.

**Windows standalone build:** The project includes configured scenes under Unity's Build Profiles / build settings. Select the Windows desktop target and build from Unity to create a playable executable. Generated build folders are intentionally excluded from version control; a downloadable executable has **not** been published as a GitHub Release.

## Repository organization

```text
Capstone/
├── Blender/                    # Workstation and room source models
├── UnityProject/
│   ├── Assets/
│   │   ├── Scenes/             # Investigation scenes
│   │   ├── Scripts/
│   │   │   ├── Interaction/    # World-object / monitor focus
│   │   │   ├── Missions/       # Objectives, completion, scoring
│   │   │   ├── Phone/          # Mission briefings and notifications
│   │   │   ├── Simulation/     # Virtual files, users, passwords
│   │   │   ├── Terminal/       # Command processing and console UI
│   │   │   ├── UI/             # Menus and objective displays
│   │   │   └── Windows/        # Browser and recon search simulation
│   │   └── MainMenu.unity
│   ├── Packages/
│   └── ProjectSettings/
└── README.md
```

## Current scope and future development

The current build implements **four playable investigation scenarios** with simulated tools and mission tracking. It is a training prototype rather than a production security laboratory or a validated measure of learning outcomes.

Potential next steps include additional missions, a larger command vocabulary, more realistic system-administration interactions, interface refinement, and formal usability / educational-effectiveness testing.

## Academic context

**Project:** CyberOps — Ethical Hacking Training Simulator  
**Developer:** Mason Pamarang  
**Faculty advisor:** Professor John Dempsey  
**Institution:** California State University Channel Islands (CSUCI)  
**Program:** B.S. Computer Science Capstone · Fall 2026

---

*CyberOps is intended for ethical, educational use within a self-contained simulated environment.*
