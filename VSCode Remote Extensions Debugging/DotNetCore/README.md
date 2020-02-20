# Debugging ASP.Net Core + VS Code + docker-compose + Remote Container Extension

This project demonstrates a debugging setup with ASP.Net Core with VS Code, using `docker-compose`. It uses:
* `Remote Container` extension to debug the containers.
* `docker-compose` to manage image builds and containers. It is especially useful if you have a multi-container setup.

## Why?

> The Remote - Containers extension lets you use a Docker container as a full-featured development environment. Whether you deploy to containers or not, containers make a great development environment because you can:
> * Develop with a consistent, easily reproducible toolchain on the same operating system you deploy to.
> * Quickly swap between different, isolated development environments and safely make updates without worrying about impacting your local machine.
> * Make it easy for new team members / contributors to get up and running in a consistent development environment.
> * Try out new technologies or clone a copy of a code base without impacting your local setup.

In addition, if you use `docker-compose` for your local development environment with the extension, you can mimic the container setup with your other environments, such as production to a high degree. Note that isn't a 100% exact imitation due to various intrincancies between the development and your environments. Such as you may want to use the SDK image for your local development to you can debug your application, but you'd use the runtime image for your production environment for optimisation purposes.

However, this means you can easily bring up containers such as database, code analysis, search engines, etc to be similar to your production environment container setup.

## How It Works

### Overview

This uses the `Remote Container` extension set up a `workspace` inside the containers and connect to it. A `workspace` can be considered as a development environment with all the development tools installed. In the case of the `Remote Container` extension, the `workspace` can provide us with command-line tools such as git (installed as part of the `.devcontainer/Dockerfile`), as well as VS Code extensions (installed through the `Remote Container` extension) in the `./devcontainer/devcontainer.json file`.

This effectively means that:
* We can easily debug our project through the container, as if it was local.
* Each developer in your team doesn't have to manually install all the command-line tools & extensions to develop the project.
  * GUI tools such as SourceTree would still need to be installed individually, as GUI tools can't be incorporated as part of the `Remote Container` workflow.

When you use `Remote Container` to open your project, it'll re-open the project with `VSCode` on container mode. This means it'll:
* Build your images & containers (if not built already).
  * It'll use `.devcontainer/docker-compose.yml` & `.devcontainer/Dockerfile` which defines how the image gets built. You can add more installation steps to install command-line tools such as `git`.
* Installs & sets up `VSCode Server` when you connect to the container. This lets your local VSCode instance communicate with the container and send certain commands to the container to provide various features. For example:
  * Installs the development VSCode extensions, as specified in the `extensions` field inside `.devcontainer/devcontainer.json`.
  * When ever we open a new terminal window through VSCode, it'll open a terminal to the container, rather then uses your host machine's terminal.
  * For docker-compose setups, you use the usual `docker-compose` volume mounts declaration inside `.devcontainer.json/docker-compose.yml` to set up volume mounts. For docker setups however, you can use `appPort` in the `.devcontainer/devcontainer.json`.

### Docker Setup

VS Code's `Remote Container` extension can use `docker-compose`, and it uses the `Dockerfile` to create development containers. We use `docker-compose` as it easily let us configure and build multiple docker containers & images.

Notice if you browse through the project, you'd find multiple `docker-compose` yaml files, and multiple `Dockerfile` files. The reason is the ones inside `.devcontainer` is designed specifically to be used for development with the VSCode's `remote-containers` extension. They are not to be used for deploying docker images to your servers. Whereas the other set of yaml files and `Dockerfile` file is designed to build images that can be deployed to your servers.

We have a base yaml file `docker-compose.base.yml` which contains core common declaration between both development and for release purposes. For more information, see [Releasing](#releasing).

## Debugging

To debug this project, you'll need:
* `VSCode` with the `Remote Container` extension installed
* `docker`
* `docker-compose` as we're using `Remote Containers` with `docker-compose` so we can manage multiple containers.

After having the pre-requisites, follow the below steps:
1. Open the folder which contains the `.vscode` folder with `VSCode`.
2. Press `F1` -> `Remote Containers: Reopen in Container`.
3. Press `F5`, or alternatively click on VSCode's `Debug` tab and press the green play button.
   Alternatively, you can open up the terminal through `CTRL` + `SHIFT` + `\s`, then enter the command `dotnet run`.

## Releasing

If you want to deploy your docker images so it can be deployed to your server, then it's usually better to not use the debugging set up. The debugging set up:

* Uses an SDK image to host the app. The SDK image has tools for debugging, and thus is more heavyweight compared to the runtime image counterpart.
* Use volume mounts to sync the code, instead of docker ADD/COPY. Thus the code isn't packaged inside the image through copying.
  * This is useful for development purposes so code is synced.
  * https://stackoverflow.com/questions/27735706/docker-add-vs-volume
* Doesn't publish the source code. Publishing the code grants various benefits, such as optimisation.

Thus, if you want to release your app through containers, whether its for test, stage or production server, it's better to not use the development image. That is, it's not a good idea to use the `.devcontainer/Dockerfile` & `.devcontainer/docker-compose.yml` to construct the image for you to deploy.

It's recommended to instead use the release-specific setup of:
* `docker-compose.base.yml`
* `docker-compose.release.yml`
* `.docker/WebApp/Dockerfile`
* `.docker/WebApp/docker-entrypoint.sh`

To use the above files, you can follow the below instructions:

1. Open up a terminal and change the working directory to where the project's `docker-compose.base.yml` & `docker-compose.release.yml`.
2. Edit the `.env` file. There's various variables you may want to configure to suit your purposes. You can change whether it deploys using development or production configs, or whether it'll set up HTTPS.
3. Run the below command:
   ```bash
   docker-compose -f docker-compose.base.yml -f docker-compose.release.yml build
   ```
   This will build the production-ready image(s), which you can release or push to a docker hub or use for your CI/CD pipeline.
4. **(Optional)** To test if the application & image works under release configuration, run the below command to run the containers locally:
   ```bash
   docker-compose -f docker-compose.base.yml -f docker-compose.release.yml up -d
   ```
   Now browse to [http://localhost:5000](http://localhost:5000)

   Note the above port would depend on what you've set for in your `.env` file.

> 💡 **Notes:**
> 
> If you re-open the project with `Remote - Containers` to resume development, it'd use the pre-built release image, meaning it won't behave correctly. Thus, you should simply rebuild the image using `devcontainer.json`. You can do this through:
> 1. Re-opening the project by pressing `F1` > `Remote-Containers: Reopen in Container`.
> 2. Rebuild the container through `F1` > `Remote-Containers: Rebuild Container`.
> 
> With the release, we don't have the HTTPS port. This is because it requires a certificate. You can make use of self-signed certificates. For an example on this, see the HTTPS modified version of this project.