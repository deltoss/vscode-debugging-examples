# Limitation

## Where The Application Code Resides

Due to the way build works, the application code MUST be strictly within the context directory, i.e. in this case, where the docker-compose.yml file is. You can figure out where the context directory is by looking at the docker-compose.yml file, where the below exists:

```yaml
version: '3'
services:
  CONTAINERNAME:
    build:
      context: .
```

Notice the "." refers to the current directory, i.e. where the "docker-compose.yml" file is located.

## Remote Debugging to Server

True remote debugging is not provisioned for. At point of writing, there were not enough information and articles out there and to set up remote debugging to a remote computer with a docker container.