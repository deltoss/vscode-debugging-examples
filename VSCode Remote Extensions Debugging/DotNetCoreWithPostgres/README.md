# Debugging ASP.Net Core + Postgres + PGAdmin + VS Code + docker-compose + Remote Container Extension

Based on the basic example, but also uses Postgres as the database, and pgadmin as a interface to the database.

## Credentials

To figure out the credentials or ports for various applications, i.e. pgadmin or postgres, refer to the ".env" file.

## Database Migrations

### Automatic Migrations

EF Code database migrations creates, updates the database schema, and can seed dummy data into the database. This project is configured to run migrations and data seeding (i.e. inserting example data into the database) automatically when the web application starts up.

For more information, see the below files:
* `DataInitialiser.cs` - Containers the initialisation code for applying migration & seeding
* `Program.cs` - Where the DataInitialiser code is called

Note that we don't use `EnsureCreated()`, and instead we use the `migrate()` method. For more information, see this [SO post](https://thedatafarm.com/data-access/ef7-ensurecreated-vs-migrate-methods/).