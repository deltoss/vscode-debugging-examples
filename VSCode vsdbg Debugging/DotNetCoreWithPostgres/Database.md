# Database

## Credentials

To figure out the credentials or ports for various applications, i.e. pgadmin or postgres, refer to the ".env" file.

## Database Migrations

### Automatic Migrations

EF Code database migrations creates, updates the database schema, and can seed dummy data into the database. This project is configured to run migrations and data seeding (i.e. inserting example data into the database) automatically when the web application starts up.

For more information, see the below files:
* `DataInitialiser.cs` - Containers the initialisation code for applying migration & seeding
* `Program.cs` - Where the DataInitialiser code is called

Note that we don't use `EnsureCreated()`, and instead we use the `migrate()` method. For more information, see this [SO post](https://thedatafarm.com/data-access/ef7-ensurecreated-vs-migrate-methods/).

### Other Approaches

For a real application that's deployed, you may want more control over your migrations rather then automatically applying migrations on start up. This way, you can have more control over the migration (i.e. you can choose what migrations to apply, rollback, etc).

You can disable the automatic migration through below steps:
1. Remove the class `DataInitialiser.cs`.
2. Remove the relevant code from `Program.cs`.
3. Open up terminal and navigate to the directory
   where `docker-compose.yml` file is in
4. Run below command to take down the pre-existing running
   containers as necessary:
   ```bash
   docker-compose down
   ```
5. Run the below command to build & bring up the containers:
   ```bash
   docker-compose up -d --build
   ```

Afterwards, you can manage migrations using alternative approaches.

#### Manage Migrations from your Machine

1. Open up `appsettings.json`, and change the connection
   string from:
     "Host=postgres;Port=5432;Username=postgres;Password=postgres;Database=postgres;"
   To:
     "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;"
   This is because the "postgres" host is resolved only within docker containers.
   If you're not targetting your local set up, you can just adjust
   the host, port, username & password as necessary as long as your
   computer has network connectivity to the postgres database.
2. Install the dotnet-ef tool, so you can manage migrations from command line:
     dotnet tool install --global dotnet-ef
3. Now run below command to apply migrations:
     dotnet ef database update
4. Revert the changes you've done in step 1, so next time you
   deploy your source code, it won't affect the DB connections.


#### Exec Into the Docker Container & Manage Migrations From There

You can exec into the web app docker container and run the migrations. However, this approach has it's own set of issues, and is rather complicated.

This project is setup to use the dotnet core runtime image hosting the published web application files. This would cause some issues with managing migrations from the container. First, the migration tool requires the dotnet sdk. Secondly, if you tried to run migration commands with the runtime image and with the published web application files, you'd hit into an error where'd you'd see the below message:
```
No project was found. Change the current working directory or use the --project option.
```

You get this message because you're running the command not on the original source code, but on the compiled DLL published files.

To work around these issues, you can either:
1. Change the project to use the SDK image instead (just modify the `Dockerfile`), or
2. Stick to using the runtime image, but run complicated commands as per in this [SO post](https://stackoverflow.com/questions/40084260/ef-core-running-migrations-without-sources-equivalent-of-ef6s-migrate-exe).
3. Just install the SDK image when you're inside the container.

For the later approach, you can follow the below steps:

1. Follow the steps outlined over in the official Microsoft article to install the SDK onto the container:
     https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1904
2. Run the below command to install the dotnet-ef command line tool:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
3. Run the below command to configure your terminal
   so that it can use the dotnet-ef tool:
   ```bash
   export PATH="$PATH:$HOME/.dotnet/tools/"
   ```
   For more information on this step, see:
     https://stackoverflow.com/questions/56862089/cannot-find-command-dotnet-ef
4. Run:
   ```bash
   dotnet ef database update
   ```