# Debugging Overview

There's a variety of debugging techniques when it comes to debugging through to a docker container. It's critical to use the most suitable debugging technique for your requirements, especially because debugging is a crucial part to our work as developers.

This document explains the approaches to debugging for ASP.NET Core with VSCode.

## vsdbg

This is an approach where you install and run the vsdbg process on the container for your VS Code to attach itself onto. This has been around longer compared to Microsoft Remote Extensions.

The way this works is you'd build an image with the vsdbg tool installed. When debugging with VS Code, it'd exec into the container (i.e. similar ssh into the container), start the vsdbg process, and attach the VS Code onto it. 

Advantages:
* Can use optimised runtime images pretty easily.

Disadvantages:
* Rather complicated, as you'd need an intermediate understanding of `launch.json`.
* For code updates, you'll need to rebuild your container, which makes this very slow.
  * Can be mitigated using volume mounts.
* Relatively difficult to get remote debugging configured.
  * https://github.com/OmniSharp/omnisharp-vscode/wiki/Attaching-to-remote-processes
* Requires you to modify your docker image to install the vsdbg tool.

## Microsoft Remote Extensions

A suite of extensions which provides you the ability to debug and update your code smoothly through the docker containers. The concept is it essentially sets up a `workspace` in the container which you can use for development purposes. All you need to do is to connect to that workspace which has all the necessary tools for you to start debugging.

The way this works is when this extension runs or attaches to the container, it'll set up VS Code Server, which would be used for communication to your local VS Code. It'll also set up a volume mount behind the scenes so that when you change your files, it'll syncs it with files on the container.

Together, this provides developers with local-quality debugging experience.

> For more information & a diagram of the whole process, see the official [getting started guide](https://code.visualstudio.com/docs/remote/containers).

Advantages:
* Developers doesn't have to install much:
  * With the concept of workspaces, the extensions and tools are installed on the container.
  * Thus, it's easy to set up a consistent environment for developers.
* Works with more then just ASP.NET.
* For code updates, it doesn't need to rebuild the image(s).
  * It syncs the file changes between the container & VS Code using volume mounts behind the scenes.

Disadvantages:
* Have to use SDK images, rather then optimised runtime images for this setup to work.
* At time of writing, still in preview.