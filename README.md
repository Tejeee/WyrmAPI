# 🐍 Wyrm API
<img width="386" height="276" alt="image" src="https://github.com/user-attachments/assets/89a5bd29-3ebf-4b7b-8dc4-c6f590c87cac" />

# 

A Windows desktop tool for managing Roblox private servers and user accounts. Built with C# and WinForms.
Features
Wyrm API is organised into tabs, each handling a different task:

- Get Root Place ID — look up the root place ID for any universe ID
- CSRF Token — fetch a fresh CSRF token for your account
- Create Server — create a new private/VIP server for a game
- Rename Server — rename an existing private server
- Generate Link — generate a new join link for a private server
- Get Metadata — fetch details about a specific private server
- Private Servers — list all private servers on your account
- Update Users — bulk tool that processes multiple Roblox accounts at once: logs in with each cookie, finds or creates a private server, generates a fresh join link, and writes everything to users.json for use with Jaram

**Update Users**

The main feature. Paste in multiple .ROBLOSECURITY cookies, choose your Windows username and the universe ID, and hit Run Update. For each account it will:

- Verify the cookie and fetch the username
- Get a CSRF token
- Find an existing private server for that game, or create one
- Generate a new private server join link
- Save the result to C:\Users\<you>\AppData\Roaming\Jaram\users.json

- If JARAM is running when you use this feature, it will automatically launch the account.

A Sols RNG shortcut button auto-fills the universe ID for Sols RNG (5361032378).

# Download options: 

Download standalone .exe file below:

https://github.com/Tejeee/WyrmAPI/releases

**Building**
- Requires .NET 8 SDK and Windows (WinForms).
1. Extract File
2. Open file to "WyrmApp" 
3. Right click and open in terminal
4. Run command below
   
`dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./out`
- The output is a single standalone .exe — no installer or runtime needed on the target machine.


**Requirements**

- Windows 10 or later
- A valid Roblox .ROBLOSECURITY cookie for each account you want to manage

**Created by Wyvern and Tej (+ Claude)**

**Virus Total Report:** [here](https://www.virustotal.com/gui/file/6821f005c374a174d6a7f211e79786243e17b85be5cf50b03a4e6d3b351677cb?nocache=1)
