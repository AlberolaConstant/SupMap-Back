CREATE TABLE "Incidents" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "UserName" VARCHAR(255) NOT NULL DEFAULT '',
    "Latitude" DOUBLE PRECISION NOT NULL,
    "Longitude" DOUBLE PRECISION NOT NULL,
    "Type" VARCHAR(50) NOT NULL DEFAULT '',
    "Description" TEXT NOT NULL DEFAULT '',
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "ExpiresAt" TIMESTAMP NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE "IncidentVotes" (
    "Id" SERIAL PRIMARY KEY,
    "IncidentId" INT NOT NULL,
    "UserId" INT NOT NULL,
    "Vote" INT NOT NULL CHECK ("Vote" IN (1, -1)),
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT "fk_incident"
        FOREIGN KEY ("IncidentId")
        REFERENCES "Incidents"("Id")
        ON DELETE CASCADE
);