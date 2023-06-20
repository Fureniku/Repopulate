Oxygen is the primary gas that aliens breathe, much like humans. Oxygen exists as a constant value on the ship, compromised of adding together all the values from oxygen creating items, and then reducing by everything that consumes oxygen. This is displayed as an oxygen pressure value, which should be around 1. Anything at or below zero means there's no air to breathe, and going significantly higher than 1 can cause station damage. The value should be considered as being per square meter.

Oxygen functions as follows:
- Oxygen production happens within modules. An oxygen producer creates a set amount of oxygen. For example, one oxygen producer might create 1000 units.
- Every module, ring and station segment has a required oxygen budget. For example, in a module which is 10x16x3 = 480 square meters, the required budget would be 480. The ring might have an area of 500, meaning the total consumption is 980.
- The station balances the oxygen, considering the space holds 980 units and 1000 are being produced. This sets the oxygen pressure at 1000/980, or 1.02. This is a great target value.
- Every living creature has an oxygen requirement. For example, a typical alien might be 0.001. If there are 20 aliens, they are consuming 0.02 units, and oxygen balances to exactly 1.

Oxygen is a *constant* resource, meaning it cannot be stored, stockpiled etc. It exists simply as a value.

When Oxygen gets below zero, meaning the level of oxygen on the ship is decreasing faster than it is being produced, the player will have some time to rectify the problem. A slight oxygen deficit will have little impact, but a severe one will begin to kill living creatures. This does of course have a natural self balance - once some creatures die, less things are consuming oxygen, and the value may stabilise and be enough for anything still alive.