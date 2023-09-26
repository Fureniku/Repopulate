# Grids
The solar system is loosely grid-based. The solar system is split into grids which are 20000 cubic units across, origin 0,0,0 from -10000 to 10000.
Upon reaching the edge of an area, an object teleports to the opposing coordinate and modifies its grid value. For example the ship flies from true origin and passes X10000. It is then transported to X-10000, and its grid value X becomes 1. The ship can then reference its solar position at any time as a value of gridX*2*10000 + current X. Purely in terms of travel and distance, these units are considered kilometers; effectively making any star or planet 1:1000 in size compared to how it would be in real life.

In reality, only the ship is actually moving and being teleported. All other objects movement and position is actually controlled from the game map on the ship's bridge, which contains a micro scale of the solar system. The objects on this micro scale take their position and grid position on that scale, and can convert it up to the solar scale. If a body comes within visual range on the solar scale, the object will then be rendered at the solar scale and be visible to the ship. To assist with camera ranges, the object will first be rendered at a low scale, and scale will change dynamically based on the ships relative position.

The system's star(s) are an exception to this rule. The star is always visible, and is a child of the ship object. The position is fixed in space as a vector between the ship and true solar origin, and the scale changes based on the ship's distance to the solar origin. If the ship gets close enough to true solar origin, the star will de-parent and allow the ship to enter orbit similar to other planets. Leaving the range will re-parent the star.

For performance, the solar system manager keeps track of the closest registered celestial object (not counting stars). This is updated every few seconds.

# Types of Celestial Objects
- Stars: Always visible. Primary is at 0,0,0 and others can orbit it like a moon orbits a planet. Solar position is calculated based on the mass/position of all stars.
- Planets: Dynamically appearing based on distance to ship. Planets can have other objects orbiting such as moons (functionally similar to planets but smaller) or constructed objects (such as abandoned modules, space elevator platforms or launched satellites/probes)
- Comets: Dynamically appearing, similar to planets on a smaller scale. Orbit is physics-based
- Asteroids: Procedurally spawn and are deleted when a player leaves the area.
- Player Objects: Things such as modules which can be abandoned in space. Do not move and are locked to a specific gridspace. Visibility based on range but only activated when a player is at the gridspace or a neighbouring gridspace.
# Lighting
- Lighting is added to each celestial object as a large scale point light, positioned towards the solar origin. This gives much better shadowing than a directional light.

# Notes and thoughts
performance distance scaling notes:
- Scale vector components by the largest size (largest component ~10,000, divide whole vector by 10, largest ~100,000, divide whole vector by 100 etc)
- Get the distance and angle between the new scaled vector and 0
- Multiply the distance output by the scale to get actual distance

star position:
every grid position has a point that's closest to world origin (opposition). Put star there. Scale star with ship distance to origin.
If grid position is greater than 1 in any axis (check if we can use magnitude for this), offset star by gridSize in direction of world origin to avoid overlapping when ship is in opposition with star



New idea:
celestial bodies begin to render when in a direct neighbour gridspace

smaller objects pool and spawn/despawn onto that child as needed and do not rescale