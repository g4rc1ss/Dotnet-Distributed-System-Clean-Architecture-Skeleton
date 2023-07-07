db.createUser({
    user: "dotnetSkeleton",
    pwd: "123456",
    roles: [{
        role: "readWrite",
        db: "CleanArchitecture"
    }
    ],
    mechanisms: ["SCRAM-SHA-1"]
});

db.createCollection("WeatherForecast");