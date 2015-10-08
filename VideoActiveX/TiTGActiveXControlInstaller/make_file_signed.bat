call "C:\Program Files (86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat"
signtool.exe sign /f TiTG.pfx /p titg /t http://timestamp.verisign.com/scripts/timstamp.dll /v TiTGActiveXControls.cab
