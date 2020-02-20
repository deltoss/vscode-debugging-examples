# About

This project is based on:
  https://github.com/Microsoft/vscode-remote-try-dotnetcore

The difference is this project is minimal, and trims out unnecessary features to make it easier to understand. The original project is also very opinionated, i.e. it installs git, iproute, and various other tools you may not actually use depending on your setup.

As a result, the `Dockerfile` & `devcontainer.json` was heavily slimmed down.

# Recommended Resources

It's still strongly recommended to read the README in the repository this project was based on:
  https://github.com/Microsoft/vscode-remote-try-dotnetcore

This will tell you how to use this project for debugging. Otherwise, the steps are below:

1. Install the prerequisites, which includes:
   * Docker
   * Microsoft's VSCode's [Remote Development Extension](https://github.com/Microsoft/vscode-remote-try-dotnetcore).
2. Open this folder with `VSCode` and hit `F1`, then type and select `Remote-Containers: Reopen Folder in Container`.
3. Put breakpoints and press `F5` to start debugging.
4. Browse to [http://localhost:5000](http://localhost:5000).