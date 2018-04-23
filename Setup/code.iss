#include "scripts\products.iss"
#include "scripts\lang\english.iss"
#include "scripts\products\stringversion.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"
#include "scripts\products\dotnetfxversion.iss"
#include "scripts\products\dotnetfx47.iss"

[Code]
function IsUpgrade(): Boolean;
var
   sPrevPath: String;
begin
  sPrevPath := '';
  if not RegQueryStringValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppID")}_is1', 'UninstallString', sPrevpath) then
    RegQueryStringValue(HKLM,  ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppID")}_is1'), 'UninstallString', sPrevpath);
  Result := (sPrevPath <> '');
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;
  if IsUpgrade() then
  begin
    if PageID = wpSelectDir then Result := True;
    if PageID = wpSelectProgramGroup then Result := True;
  end;
end;

function InitializeSetup(): Boolean;
begin
  initwinversion();
  Result := True;
  if not Is64BitInstallMode then begin
    SuppressibleMsgBox('Thingy requires a 64 bit Operating system.', mbError, MB_OK, IDOK);
    Result := False;
    Exit;
  end;
    dotnetfx47(50);
end;