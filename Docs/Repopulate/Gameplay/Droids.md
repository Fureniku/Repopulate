Droids are your primary interaction with the world. They are modulars, and come in various levels, which can be balanced later. Each level of droid will increase its base stats, and the number of attachable modules. All stats should be data driven in some form.

### Stats might be... (without modules)
- Energy capacity/current level
- Energy consumption (hidden)
- Inventory size
- Equipped tool
- Equipped modules
- Module capacity
- Movement speed
- Damage/defense of some kind - need to figure out how durability works.

## Modules it can equip might be...
- Energy banks - more modules = more energy = more tasks before recharge
- Inventory spaces - more modules = more inventory space = carry more things around
- Tools - modules = perform specific tasks (collect resources, build objects, repair objects, various colonist-related tasks to be planned later)
- Mobility options - At least one is required but multiple non-conflicting can be used.
	- Legs - able to walk around in any gravity areas, and kick off walls in zero G (conflicts with wheels)
	- Station rail connection system (required for automation)
	- Wheels - much faster, but can only move on a solid surface. Fine in station, but not on surface. Different controls (more like a car, can't instant turn)
	- EVCS - extra vehicular control system - to move around in free space
- Armour/shielding/some way to improve durability
- Range upgrade



## Droids are my next project to work on. Here's a TODO list.
1. [x] Switch to entirely independent droids. Each droid should have its own camera and data settings. It should be theoretically possible to control multiple droids in split screen (although this won't be part of the game). Where does general UI go? Droid makes more sense actually, its currently on root.
2. [ ] Inventory system. Needs implementing on an abstract level, and droids will then utilise it
3. [ ] Module system - expands from inventory really.
4. [ ] Crafting system - creative-esque for now; ability to create new droids and switch to them, ability to destroy droids and no longer switch to them.
	1. [ ] This needs to account for droids availability. Only ever the same system but range matters too.
5. [ ] Tools
6.  [ ] Refine movement. Core "legs" movement in gravity is pretty good so shouldn't be too much work, but variables need considering. jump might need tweaking, but these balances can come later. Make it easier to balance these!
7. [ ] Rail system. Need to be able to attach/detatch from a rail freely - for now all droids can do that.
8. Automation (time to do navmeshes again! must work with rail system too)
9. Further movement refinement. Space movement needs work (wall mount and kickoff specifically) and wheel movement doesn't exist yet.