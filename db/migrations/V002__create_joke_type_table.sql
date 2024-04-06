CREATE TABLE "JokeType" (
    "JokeTypeID" SERIAL PRIMARY KEY,
    "Description" VARCHAR(255) NOT NULL CHECK (length("Description") >= 3)
);
