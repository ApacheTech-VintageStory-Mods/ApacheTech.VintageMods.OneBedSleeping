﻿// ReSharper disable StringLiteralTypo

[assembly: ModDependency("game", "1.16.4")]
[assembly: ModDependency("survival", "1.16.4")]

[assembly:ModInfo(
    "One Bed Sleeping",
    "onebedsleeping",
    Description = "For use on SMP servers. Only one person needs to sleep in order to pass the time.",
    Side = "Universal",
    Version = "2.1.1",
    RequiredOnClient = true,
    RequiredOnServer = true,
    NetworkVersion = "1.0.0",
    IconPath = "modicon.png",
    Website = "https://apachetech.co.uk",
    Contributors = new[] { "ApacheTech Solutions" },
    Authors = new []{ "ApacheTech Solutions" })]