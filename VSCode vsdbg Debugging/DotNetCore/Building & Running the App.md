# Building & Running the App

## Compiling and Building Code

### VSCode Build Tasks Approach

There's two build tasks which you can use VSCode IDE to trigger:
* `Docker Up and Build`
* `Docker Down`

You can trigger the above tasks with VSCode through:
* Pressing `F1`, and type in `Tasks: Run Build Task`, then select the task to execute.
* Use the shortcut key `CTRL + SHIFT + B`, then select the task to execute.

### Manual Approach

You can build your code through the docker command:

```bash
docker-compose build
```

And finally, bring up your container:

```bash
docker-compose up -d
```

Alternatively, you can do this through a single command:

```bash
docker-compose up -d --build
```

To take down the application, use the below command:

```bash
docker-compose down
```

If you also want to take down the image as well, you can use:
```bash
docker-compose down --rmi all
```


## Updating the Source Code

Note after you made changes to the code, you must rebuild the container. The reason for this is due to how the container was set up. On build, it copies the application files, and does not attempt to sync the application code post-build in real-time.

Thus, to get the code up to date, you'd run the below commands:

```bash
docker-compose down
docker-compose build
docker-compose up -d
```

Or:

```bash
docker-compose down
docker-compose up -d --build
```

Alternatively, you can chain these commands using the powershell `;` operator or `&&` operator if you're on bash. For example:

Powershell:

```powershell
docker-compose down; docker-compose up -d --build
```

Bash:

```bash
docker-compose down && docker-compose up -d --build
```


## Setting Up Build Tasks

You can set up VSCode tasks to automate the above sequence of scripts, which you can then assign it to be keyboard shortcuts for convenience.

In fact, this project has done exactly that. You can look at the ".vscode/tasks.json" file for more information. It has a number of build tasks configured.

To run build tasks, simply use the shortcut `CTRL + SHIFT + B`.

For more information, see https://code.visualstudio.com/docs/editor/tasks.


## Improve Build Performance

Building with docker can be considered rather slow. Each time you build, you bring up a docker container based on the Microsoft DotNet core image.

It contains all the tools such as DotNet core SDK, and after you download that image the first time, it gets cached so you don't have to download it again and again.

However, the problem is it doesn't container VSDebugging tools by default, as such, we have a "Dockerfile" which installs vsdbg, but that process requires installation of various packages aside from vsdbg itself.

Thus, to speed up the container build or performance, there are a few possible ways.

### Download vsdbg & Configure dockerfile

You can download the vsdbg file, and then put it somewhere in your project where you can configure your dockerfile to simply copy it across to the image, much like how it's copying the application source code. This means you won't need to download the vsdbg every time you build.

### Change Base Docker Image

Make your own Docker image that has the necessary tools so it doesn't need to be downloaded over, and over again. Namely the VSDebug as it takes time to download and install it.

You can make the image, and then host it on DockerHub or another external docker images registry. However, you can also just have it exist locally only.

For more information on having it exist locally, see:
  https://solidfish.com/building-net-core-apps-docker-with-vscode-on-mac-or-windows/

Alternatively, you can go to DockerHub to search for a modified version that has VSDebug already installed in their image.

### Source Code Updates

Running docker builds each time you make changes to your source code takes too much time. There are a few approaches you can use to resolve this issue:
1. Use volume mounts. Configure the `Dockerfile` to use SDK image only, and use the command `dotnet watch` as the entrypoint command against the volume mounted source code files.
2. Run the command manually to publish the file on your local machine, and then copy the files to the container via the `docker cp` command. You can use VSCode tasks to automate this process.


## Adjustment Notes

For any of the below changes, it'll require adjusting the `.vscode/launch.json` file:
* Container name in docker-compose.yml
* Path of vsdbg or publish path in the dockerfile
* Path or name of the web application project
  * This would also require a change in the ".env" file

This is because launch.json, in order to debug the application, it uses:
* The container name to exec (i.e. ssh) into the container and start the debugging session.
* After getting into the container, to start and debug the project, it uses:
  * The files in the vsdbg path.
  * The DLL file which takes the name of the project.