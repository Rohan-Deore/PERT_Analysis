;NSIS Modern User Interface
;Basic Example Script
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "PERT Analyser"
  OutFile "PERT_Analyser_Installer.exe"
  Unicode True
  BrandingText "Rohan Deore"
  !define MUI_ICON "PERT analysis software logo 256.ico"

  ; !define MUI_HEADERIMAGE
  ; !define MUI_HEADERIMAGE_BITMAP "PERT Analyser banner image.bmp"
  ; !define MUI_HEADERIMAGE_RIGHT

  ;Default installation folder
  InstallDir "$LOCALAPPDATA\PERT_Analyser"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\PERT_Analyser" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel user

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "Main Application" SecDummy

  SetOutPath "$INSTDIR"
  
  ;ADD YOUR OWN FILES HERE...
  ;File "D:\Code\Samples\PERT_Analysis\Installer\Release\net8.0-windows\PERT_Analyser.exe"
  File ".\Release\net8.0-windows\*.*"
  ;File "Release/net8.0-windows/NLog.config"
  ;File "Release/net8.0-windows/NLog.dll"
  ;File "Release/net8.0-windows/PERT_Analyser.dll"
  ;File "Release/net8.0-windows/PERT_Analyser.exe"
  
  ;Store installation folder
  WriteRegStr HKCU "Software\PERT_Analyser" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecDummy ${LANG_ENGLISH} "This is main application."

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...
  ; File "Release/net8.0-windows/NLog.config"
  ; File "Release/net8.0-windows/NLog.dll"
  ; File "Release/net8.0-windows/PERT_Analyser.dll"
  ; File "Release/net8.0-windows/PERT_Analyser.exe"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\PERT_Analyser"

SectionEnd