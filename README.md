# MotionMatching
Simple motion matching implementation in Unity

Preview video

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/JuY8QjJK_48/0.jpg)](https://www.youtube.com/watch?v=JuY8QjJK_48)


Currently the system only match trajectory but can be extended easily if you want it to match specific bone or entire pose.

You need:
1. Set of locomotions animations.
2. Character with animators

QuickStart:
1. Create your animation library in asset window right click and Create/MotionMatching/AssetLibrary
2. Fill your animation clips in the animations list
3. Drag and drop your character
4. Add following components to your character: SampleMotionMatching, MMAgent, MMService
5. Fill in required field like LineRenderer, Camera, Curves (just linear will do but feel free to experiment with it).

