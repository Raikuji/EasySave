dotnet build --configuration Release
xcopy /s/e/y EasyGui\bin\Release\net8.0-windows %APPDATA%\EasySave\
xcopy /s/e/y CryptoSoft\bin\Release\net8.0\win-x64 %APPDATA%\EasySave\CryptoSoft\
xcopy /s/e/y dependencies\* %APPDATA%\EasySave\resources\
md %APPDATA%\EasySave\log