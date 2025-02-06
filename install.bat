dotnet build --configuration Release
xcopy /s/e EasyCmd\bin\Release\net8.0\* %APPDATA%\EasyCmd\
xcopy /s/e dependencies\* %APPDATA%\EasyCmd\resources\