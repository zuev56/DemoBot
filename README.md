# DemoBot

The project demonstrates the base bot functionality:
- Using [SQLite](https://github.com/zuev56/Zs.Bot.Data.SQLite)
- Using [Scheduler](https://github.com/zuev56/Zs.Common.Services/tree/main/src/Scheduler)
- Using [CommandManager](https://github.com/zuev56/Zs.Bot.Services/tree/main/src/Commands)
- Using /bash, /pwsh commands to interact with the bot's server

## appsettings.json
- SecretsPath - You can remove this property from appsettings.json and add real secret values (e.g. Bot:Token) in the file.
Or you can use separated from the project [file with secrets](https://github.com/zuev56/Zs.Bot/blob/master/secrets_exapmle.json) and substitute the secret values using {interpolation}.
- Bot:Token - TODO
- Bot:BashPath - TODO
- Bot:PowerShellPath - TODO
- ConnectionStrings:Default - TODO
- EnableDemoJobs - TODO

## /bash and /pwsh commands
The commands are awailable only to users with the role OWNER. You have to send at least one message to the bot, then you can find your data in the database and change default role USER to OWNER.
