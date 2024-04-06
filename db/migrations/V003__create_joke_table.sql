CREATE TABLE "Joke" (
    "JokeID" SERIAL PRIMARY KEY,
    "Story" VARCHAR(255) NOT NULL CHECK (length("Story") >= 10),
    "UserID" INT NOT NULL,
    "JokeTypeID" INT NOT NULL,
    CONSTRAINT fk_joke_creator FOREIGN KEY ("UserID") REFERENCES "User"("UserID"),
    CONSTRAINT fk_joke_type FOREIGN KEY ("JokeTypeID") REFERENCES "JokeType"("JokeTypeID")
);
