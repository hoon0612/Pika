CartoonFX, version 1.3
2012/08/06
© 2012, Jean Moreno
======================


PREFABS
-------
Particle Systems prefabs are located in "CFX_Prefabs" folder.
Particle Systems optimized for Mobile are located in "CFX Prefabs (Mobile)" folder.
They should work out of the box for most needs. If you need looping effect, check the according "Looping" checkbox for each Particle System.
All Assets have a CFX_ (Desktop) or CFXM_ (Mobile) prefix so that you don't mix them with your own Assets.


MOBILE OPTIMIZED PREFABS
------------------------
As of v1.3, Prefabs have all been converted to optimized versions for mobile devices.
Changes are:
- Added a particle additive shader that uses only the alpha channel to save up on texture memory usage
- Monochrome textures' format has been set to Alpha8 to get a much smaller memory size while retaining the same quality
- Other textures' formats have been set to PVRTC compression
- Textures have all been resized to small resolution sizes through Unity; you can however scale them up if you need better quality
- MipMaps have been deactivated in most cases to save up on texture file size; reactivate them if you need better quality when viewing the effects from far away
- Particle Systems have been changed accordingly to retain color/transparency and overall quality compared to their desktop counterparts
- Swapped alpha blended shaders to additive ones when possible (alpha blending should be avoided as much as possible on mobile)


SCALING EDITOR SCRIPT
---------------------
You can find the Particle System scaling editor script in the menu:
GameObject -> CartoonFX Easy Particle System Scale
Select the GameObject from which you want to resize the particles, set a multiplier value, and scale!
If the ParticleSystem uses a Mesh as Shape, it will automatically create a new scaled Mesh.
It will also scale lights' intensity accordingly if any are found.


TROUBLESHOOTING
---------------
* Some prefabs may have CFX_Demo_ scripts to randomize rotation or direction for the Demo scene, just remove the scripts if needed!
* Almost all prefabs have auto-destruction scripts for the Demo scene; remove them if you do not want your particle system to destroy itself upon completion.
* If you have problems with z-sorting (transparent objects appearing in front of other when their position is actually behind), try changing the values in the Particle System -> Renderer -> Sorting Fudge; as long as the relative order is respected between the different particle systems of a same prefab, it should work ok.
* CFX_ElectricMesh is meant to be edited with whatever Mesh you want; Replace it in the Particle System Inspector -> Shape -> Mesh.


PLEASE LEAVE A REVIEW OR RATE THE PACKAGE IF YOU FIND IT USEFUL!
Enjoy! :)


CONTACT
-------
Questions, suggestions, help needed?
Contact me at:
jean.moreno.public+unity@gmail.com