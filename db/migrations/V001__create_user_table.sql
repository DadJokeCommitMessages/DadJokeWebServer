CREATE TABLE "User" (
    UserID SERIAL PRIMARY KEY,
    GoogleID VARCHAR(255),
    AuthorName VARCHAR(255) NOT NULL
);