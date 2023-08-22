Code-wise:
- Moves any entered rigidbody vertically, either up or down based on a parameter
- Gently pushes the rigidbody towards the centre, with a "dead zone" that can be freely moved around within. Moving to the edge should have some resistance but be allowed.
- Works at any orientation

Visual-wise:
- All controlled from a VFX object/script.
- Primary ripple effect on an external cylinder mesh, no top/bottom, and double sided (using VertexDisplacementMat_Distorted)
- Visual lighting effect from a billboarded texture. This needs to be axis locked on relative X- works on flat but probably don't work rotated yet. localRotation?
- LIGHT TEXTURE NEEDS CAP AT THE TOP
- Secondary ripple effect from a particle emitter within. Relatively low rate default texture particles using the exact same material for a doubled distortion. May need to remake material to get the right shape?
- Second/Third particle emitters, one giving a slowed lightning vibrant effect, the other some pale wind effects to indicate direction. Lightning should only be close to base, wind throughout.
- Model for emitter base