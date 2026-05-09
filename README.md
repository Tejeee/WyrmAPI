

# 🐍 Wyrm API

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
The main feature. Paste in multiple .ROBLOSECURITY cookies, enter your Windows username and the universe ID, and hit Run Update. For each account it will:

- Verify the cookie and fetch the username
- Get a CSRF token
- Find an existing private server for that game, or create one
- Generate a new private server join link
- Save the result to C:\Users\<you>\AppData\Roaming\Jaram\users.json

A Sols RNG shortcut button auto-fills the universe ID for Sols RNG (5361032378).

# Download options: 

**Building**
- Requires .NET 8 SDK and Windows (WinForms).

`dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./out`
- The output is a single standalone .exe — no installer or runtime needed on the target machine.



**Requirements**

- Windows 10 or later
- A valid Roblox .ROBLOSECURITY cookie for each account you want to manage

**Created by Wyvern and Tej (+ claude)**
