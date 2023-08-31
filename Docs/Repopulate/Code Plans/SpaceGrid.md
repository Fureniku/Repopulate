The solar system is grid based. First, a tiny scale version of the solar system exists on the ship's bridge, which doubles as a map for the player. Undiscovered planets simply have their rendering disabled on the map. The star at the centre is equal to 0,0,0. Each directly orbiting object (e.g. planets, but not moons which orbit the planet) calculates its relative gridspace from its current position.

This map is a 1:5000 scale of the solar system, with planets and stars at appropriate scale. An object representing the player's ship is at a much higher scale to make it easily visible on the map.

The spacegrid system then checks if any given object is in the same space or a directly neighboring space (the 27 gridspace area including player's current) as the player. If so, it creates a copy of the object which is visible to the player, and moves it along its orbital path. The object will scale in size depending on its distance from the player.


World star: 5000 - Map star: 1
Avg. Planet: 1000 - Map planet: 0.2