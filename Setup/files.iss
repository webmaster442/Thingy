[Components]
Name: "base"; Description: "Base Program"; Types: compact custom full
Name: "media"; Description: "Media utils (mpv, youtube-dl, ffmpeg)"; Types: full custom

[Dirs]
Name: "{app}\Native\x64\"
Name: "{app}\Apps\x64\"

[Files]
Source: "..\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\*.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\*.config"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\FontInstaller.vbs"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Native\*"; DestDir: "{app}\Native"; Flags: replacesameversion recursesubdirs; Components: base
;------------------------------------------------------------------------------
Source: "..\bin\Release\Apps\*"; DestDir: "{app}\Apps"; Flags: replacesameversion recursesubdirs; Components: media