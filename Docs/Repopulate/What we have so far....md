Things that are implemented, progress tracker of sorts:

- Multiple controllable droids, switchable with Q
- Station core segments, which can be stacked.
	- Control panels which can add one ring prefab
- One ring prefab, which has 12 doors
	- Control panels to add modules to the doors
- Each module has a buildable grid, where multi-block items can be rotated and placed

Resource management!
- Resource producers, consumers and storage can be registered with the station.
- Energy is the first implemented resource; solar panels can generate it, and batteries can store it. Nothing consumes it yet.
	- There's also nothing that happens when it runs out, yet.