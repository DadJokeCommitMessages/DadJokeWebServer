# Dad Joke Commit Message: DadJokeWebServer

The DadJokeWebServer contains the REST API that is used by the CLI and the Web Frontend to communicate. It handles
the CRUD operations via HTTP and offers a central point for managing 'Dad Joke Commit Message'

## Local API Setup

### Environment Variables

The application needs a database connection string before it can be run. A connection string can contain 
sensitive information (like the username and password) and thus utilizes environment variables to hide 
the secrets.

The connection string has to be set up on the local. The setup is dependent on the operating system the local
machine is running on. It will be easier to link docs, than to outline an article that has already been written:

- [Windows](https://computerhope.com/issues/ch000549.htm)
- [Mac OS](https://phoenixnap.com/kb/set-environment-variable-mac)
- [Linux](https://www.freecodecamp.org/news/how-to-set-an-environment-variable-in-linux/)

If you would like to know more about environment variables in .Net, feel free to read this 
[documentation](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0).

The following environment variable will need to be defined: The connection string is a `Postgres` connection string. 
The server, database, user and password will need to be populated with your unique combination.

```
DATABASE_CONNECTION_STRING = Server=127.0.0.1;Database=postgres;Password=mysecretpassword;User Id=postgres;
```

The following code in the Program.cs file will use the variable:

```c#
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"));
});
```

### Running a Postgres Docker Instance

If you do not have a Postgres instance running, the fastest way would be to run the following command. This will
create a Docker container to connect to and no other software will need to be installed on local.

```bash
docker run --name some-postgres -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgres
```

Note that the DB will be empty now because Flyway isn't setup locally to run the scripts. The easiest fix would be to 
run the scripts found in `db/migrations` manually.