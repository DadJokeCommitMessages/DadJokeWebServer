INSERT INTO "Joke" ("Story", "UserID", "JokeTypeID")
VALUES
    ('Created a byte-sized innovation',
     (SELECT "UserID" FROM "User" WHERE "User"."EmailAddress" = 'test@gmail.com'),
     (SELECT "JokeTypeID" FROM "JokeType" WHERE "JokeType"."Description" = 'feature')
    ),
    ('I had a problem, so I used multiple threads. Now problems I. two have', 
     (SELECT "UserID" FROM "User" WHERE "User"."EmailAddress" = 'test@gmail.com'),
     (SELECT "JokeTypeID" FROM "JokeType" WHERE "JokeType"."Description" = 'fix')
    ),
    ('Why don''t programmers like nature? It has too many bugs.',
        (SELECT "UserID" FROM "User" WHERE "User"."EmailAddress" = 'test@gmail.com'),
        (SELECT "JokeTypeID" FROM "JokeType" WHERE "JokeType"."Description" = 'fix')
    ),
    ('Why was the documentation tired? Because it spent all night trying to keep up with the code changes!',
        (SELECT "UserID" FROM "User" WHERE "User"."EmailAddress" = 'test@gmail.com'),
        (SELECT "JokeTypeID" FROM "JokeType" WHERE "JokeType"."Description" = 'docs')
    ),
    ('I ran my programmer dad joke through a linter a code formatter. Pun indented.',
        (SELECT "UserID" FROM "User" WHERE "User"."EmailAddress" = 'anotherTest@gmail.com'),
        (SELECT "JokeTypeID" FROM "JokeType" WHERE "JokeType"."Description" = 'style')
    ),
    ('Why did the dev miss her plane? Because she failed the preflight check, of cors',
        (SELECT "UserID" FROM "User" WHERE "User"."EmailAddress" = 'anotherTest@gmail.com'),
        (SELECT "JokeTypeID" FROM "JokeType" WHERE "JokeType"."Description" = 'test')
    );