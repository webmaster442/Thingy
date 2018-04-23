[Components]
Name: "base"; Description: "Base Program"; Types: compact custom full
Name: "mpvplayer"; Description: "MPV player"; Types: full custom

[Dirs]
Name: "{app}\Native\x64\"
Name: "{app}\Apps\x64\"

[Files]
Source: "..\bin\Release\AppLib.Common.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\AppLib.Maths.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\AppLib.MVVM.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\AppLib.WPF.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\CommonMark.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\ControlzEx.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Dragablz.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\FontInstaller.vbs"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\HtmlRenderer.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\HtmlRenderer.WPF.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\ICSharpCode.AvalonEdit.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\init.cmd"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\IronPython.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\IronPython.Modules.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\IronPython.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\IronPython.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\LiteDB.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\MahApps.Metro.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\MahApps.Metro.SimpleChildWindow.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\ManagedBass.PInvoke.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Microsoft.Dynamic.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Microsoft.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Microsoft.Scripting.Metadata.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Microsoft.VisualStudio.CodeCoverage.Shim.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\System.IO.Compression.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\System.Windows.Interactivity.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.API.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.Calculator.module.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.CalculatorCore.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.CalculatorCore.dll.config"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.Cmd.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.Cmd.exe.config"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.CoreModules.module.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.CoreModules.module.dll.config"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.Db.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.exe.config"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.ExternalLibs.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.GitBash.module.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.MusicPlayer.module.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.MusicPlayer.module.dll.config"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.MusicPlayerCore.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.Resources.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Thingy.XAML.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: base
Source: "..\bin\Release\Native\*"; DestDir: "{app}\Native\"; Flags: ignoreversion createallsubdirs recursesubdirs; Components: base
;-----------------------------------------------------------------------------------------------------------------------------
Source: "..\bin\Release\Apps\x64\d3dcompiler_43.dll"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer
Source: "..\bin\Release\Apps\x64\libaacs.dll"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer
Source: "..\bin\Release\Apps\x64\libbdplus.dll"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer
Source: "..\bin\Release\Apps\x64\manual.pdf"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer
Source: "..\bin\Release\Apps\x64\mpv.com"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer
Source: "..\bin\Release\Apps\x64\mpv.exe"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer
Source: "..\bin\Release\Apps\x64\youtube-dl.exe"; DestDir: "{app}\Apps\x64\"; Flags: ignoreversion; Components: mpvplayer