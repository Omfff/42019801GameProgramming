                      __________           _____              ___________
_________________________(_)_  /_____      __  /_____________ ___(_)__  /
__  ___/__  __ \_  ___/_  /_  __/  _ \     _  __/_  ___/  __ `/_  /__  / 
_(__  )__  /_/ /  /   _  / / /_ /  __/     / /_ _  /   / /_/ /_  / _  /  
/____/ _  .___//_/    /_/  \__/ \___/      \__/ /_/    \__,_/ /_/  /_/   
       /_/                                                               

Greetings and thanks for using Sprite trail.
contact : me@julien-foucher.com
twitter : @Sephius_Mojo
online doc + webgl demo : http://memory-leaks.org/spritetrail/


Special thanks to Fausto Cheder for helping improving this asset! ( twitter.com/SugoiDev )

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
HOW TO START USING SPRITE TRAIL :
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Method 1 :
- Drag and drop the prefab "SpriteTrail" located in "SpriteTrail/PREFAB/SpriteTrail"
- Hit the play button
- Move the Gameobject
- Enjoy

Method 2 :
- The first step is to attach a "SpriteTrail" component to the GameObject you want to be affected by the trail effect.
You can also attach the sprite trail to another GameObject and assign the sprite renderer into the "SpriteToDuplicate" variable of SpriteTrail.

- drag and drop a TrailPreset in the "CurrentTrailPreset" field on the "SpriteTrail" component.
You can find some presets in the folder : SpriteTrail/PREFAB/TRAIL_PRESETS

That's it ! if the "trail activation condition" on the component SpriteTrail is set to "Always Enabled", 
you will see your trail appears when you move the item.

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
HOW TO PERSONALIZE YOUR TRAIL:
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
TRAIL PRESET

On the TrailPreset, you have multiple fields. Here is a description of these fields :

- Trail color : use this to set the trail color (rgba) over time.

- Use Only Alpha : Check this if you want your trail to use only the alpha channel of the gradient "TrailColor".
The rgb channels of the gradient will be ignored.

- Special Mat : You can assign a material to the trail. 
If you leave it empty, the material used by the SpriteRenderer "SpriteToDuplicate" in "SpriteTrail" will be used.
If you want your trail to be the current sprite's silhouette, you can use the material "SolidColor" in the folder SpriteTrail/GRAPH/MATERIALS

- Trail Element Duration Condition : Define if the trail length will be defined by time or element count. There are 2 choices :
                - Time : The elements in the trail will dissapear after (n) seconds defined in "Trail Duration".
                - Element count : The trail length max will be (n) elements defined in "Trail Max Length".

- Trail Duration : the duration of the trail in seconds. It represents the life time of a TrailElement. -1 is infinity (not recommanded)

- Trail Element Spawn Condition : There are 3 possible spawn conditions. You can choose between :
            - Time : The TrailElements are generated at a specific time interval (Time between Spawns).
            - FrameCount : The TrailElements are generated at a specific frame interval (Frames between Spawns).
            - Distance : The TrailElements are generated if the GameObject with a SpriteTrail component have moved enough since the last spawn.
                        You can set the Distance minimum using "Distance between Spawns"
                        If the Distance Correction is checked, it will calculate the error between each iteration and fill the gap (recommended)

- Use Size Modifier : If checked, it will enable the trail scaling over time and display the "Trail Size X" and "Trail Size Y" curves.
  Uses these curves to modify the size of the tail over time.

-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SPRITE TRAIL

- Trail Name : Set a name to the trail so it will be easier to recognise it

- Current Trail Preset : Drag and drop here the preset you want to use on the trail. It must be set in order to use SpriteTrail

- Hide trail on disabled : check this if you want the trail to instantly dissapear when the trail effect is stopped.
If not checked, the trail will dissapear according to the "Trail Duration" variable

- Trail Activation condition : 3 choices :
                        - Always enabled (The trail is always enabled)
                        - Manual : You have to start the trail with code by using EnableTrail();
                        - Velocity magnitude :  - The trail start if velocity magnitude is reached. (Velocity Needed To Start)
                                                - If "Start if Under Velocity" is checked, the velocity must be lower instead of higher
                                                in order to trigger the trail
                                                - If "Velocity Start Is Local Space" is checked, the velocity must be local instead of world
                                                in order to trigger the trail

- Trail Disactivation condition : 3 choices :
                        - Time : the trail will be disabled after (n) seconds (Trail Activation Duration)
                        - Manual : You have to stop the trail with code by using DisableTrail();
                        - Velocity magnitude :  - The trail stop if velocity magnitude is reached. (Velocity Needed To Stop)
                                                - If "Start if Over Velocity" is checked, the velocity must be higher instead of lower
                                                in order to trigger the trail stop
                                                - If "Velocity Stop Is Local Space" is checked, the velocity must be local instead of world
                                                in order to trigger the trail stop

- Trail Parent : Set this if you want your trail to be in the local space of the transform set

- Sprite To Duplicate : Keep it null if your SpriteTrail is attached to the spriteRenderer that generate the trail.
Else, set this if you want your SpriteTrail on an external GameObject

- Layer Name : Set this if you want the sprites created stored in a specific layer

- Z Move Step : In order to render the trail elements in the right order, we have to move slowly each element on the Zaxis. 
Change the step if you have z precision problems

- Z Move Max : The maximum displacement on the z axis. If reached, it reset to zero. Increase this if you want to prevent sprites overlapping. 
Decrease it if your trails are behind other elements.

- Trail Order In Layer : Modify this if you want your trail to be rendered before or after inside the layer 
(Can be usefull if the trail is hide by something else).

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
HOW TO USE SPRITE TRAIL WITH CODE
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

There are a few public functions you can access on the SpriteTrail.cs class :

- SetTrailParent(Transform trailParent)
  Set the trail parent if your item is in another moving item, and you want the trail to be in local space
  eg : your character enters a moving train : you probably want the trail to be in the train local space, so SetTrailParent(TheTrainTransform)
  PARAM : Transform trailParent : The parent transform. Set it to null if you want it in world space

- SetTrailPreset(TrailPreset preset)
  Set the trail effect
  PARAM : TrailPreset preset : The trail preset you want to set

- EnableTrail()
  Enable the trail

- DisableTrail()
  Disable the trail

  //USE NOT RECOMMANDED, use EnableTrail() instead
- EnableTrailEffect(bool forceTrailCreation = true)
  Enable the trail effect
  PARAM : bool forceTrailCreation : Set it to true if you want to force the creation of the trail immediately and spawn the first element

  //USE NOT RECOMMANDED, use DisableTrail() instead
- DisableTrailEffect()
  Disable the trail effect

- bool IsEffectEnabled()
  Return true if the trail effect is active

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

If you have any questions, if you want a new feature or if you found a bug, please send me a message.
If you have a bug report, please explain what you want to achieve, what your problem is and how to reproduce the bug.
