Things that are implemented, progress tracker of sorts:

- Multiple controllable droids, switchable with Q
- Completely physics-based player controller, which has
	- Custom gravity, pulling it towards different objects and treating it like a solid floor
	- Physics-based movement which transitions seamlessly between gravity and zero-gravity spaces
	- Gravity lifts, to move around and transition between different orientated gravity spaces
- Custom gravity system
	- Different types of gravity shapes, which attract objects in different ways:
		- Point-based gravity, where all objects are pulled to a spherical center, for example on a planet.
		- Line-based gravity, where objects are pulled towards a point on a line, for example walking around a ring or cylinder
		- Flat gravity, which works on a plane with no tilting rotation for the pulled object
		- Gravity lifts, which raise an object against the force of gravity, keeping them roughly centralized within the lift


Things that still need to be done/known issues:
- Velocity clamping is applying to gravity, so gravity feels weak/slow. Trying to avoid the clamp just for gravity results in gravity lift centralization no longer working.