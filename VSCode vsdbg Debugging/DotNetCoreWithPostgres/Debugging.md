# Debugging

## Modes of Debugging

There's two modes for VSCode debugging:
* Launch
* Attach

Launch brings up the application, and attaches to the debugger to debug your code. This is useful to debug code that runs when the application first starts up. Attach mode attaches the debugger to an already running application.

Attach doesn't let you debug your code in all cases. If you have code that only runs when it first starts up, or your application only runs for a few seconds, then the attach approach isn't feasible. Attach however can be used to debug remote applications more easily compared to the launch approach.

This project supports both modes.

## Launch Mode Debugging

1. Open up the `.env` with a text editor.
2. Adjust the variable `LAUNCH_APP` as per below:
   ```
   LAUNCH_APP=false
   ```
3. Follow the steps in the [Building & Running the App](./Building%20&%20Running%20the%20App.md) documentation to run the app.
4. Go to the `Debug` tab of VSCode.
5. Change the dropdown value to `Launch Website (Docker)`
6. Press F5, or click on the green play button on the `Debug` tab.


## Attach Mode Debugging

### Launch App via LAUNCH_APP Variable

1. Open up the `.env` with a text editor.
2. Adjust the variable `LAUNCH_APP` as per below:
   ```
   LAUNCH_APP=true
   ```
3. Follow the steps in the [Building & Running the App](./Building%20&%20Running%20the%20App.md) documentation to run the app.
4. Go to the `Debug` tab of VSCode.
5. Change the dropdown value to `Attach to Website (Docker)`
6. Press F5, or click on the green play button on the `Debug` tab.

### Launch App via Docker Exec

1. Open up the `.env` with a text editor.
2. Adjust the variable `LAUNCH_APP` as per below:
   ```
   LAUNCH_APP=false
   ```
3. Follow the steps in the [Building & Running the App](./Building%20&%20Running%20the%20App.md) documentation to run the app.
4. Start the app through opening a terminal, and then run the below commands:
   
   Basic syntax:
   ```bash
   docker-compose exec -d webapp dotnet <PATH_TO_YOUR_APPLICATION_DDL_FILE>
   ```

   For example:
   ```bash
   docker-compose exec -d webapp dotnet /publish/WebApp.dll
   ```

   `-d` flag is to run the command in detached mode, where
   it won't use the current terminal. This means you can
   use the current terminal for other commands. This
   can be omitted if you'd like to see the logs
   for launching the application, in case there's
   any warnings/errors.

   Alternatively you can use the below commands:
   ```bash
   docker-compose exec webapp sh
   dotnet /publish/WebApp.dll
   ```

5. Go to the `Debug` tab of VSCode.
6. Change the dropdown value to `Attach to Website (Docker)`
7. Press F5, or click on the green play button on the `Debug` tab.