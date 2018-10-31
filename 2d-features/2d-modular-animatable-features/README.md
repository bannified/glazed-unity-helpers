# 2d-modular-animatable-features

Introduces the CharacterEmoteSystem (CES).

The Character Emote System (CES) is a modular system which allows for emotes to be created based on the separated facial features (AnimatableFeature) of characters.

This system consists of the following scripts:

### MonoBehaviours
**AnimatableController.cs [AC]**

The highest level class and main API of the CES. Emotes should be played using this component. Manages all AnimatableFeatures that are its children.

**AnimatableFeature.cs [AF]**

This component is attached to a gameObject that acts as a facial feature that can be animated. The AC will animate the feature if the input EmoteSO states so. For example, a left eye (eye_L) and a right eye (eye_R) can each have their own AF components, a pair of eyes (eyes) can also have its own AF component. The gameObject’s name is used as the AF’s key in AC’s StringToAnimatableFeatureDictionary.

### ScriptableObjects
**FeatureAnimation [FA]**

Contains data and logic of an animation. The basic (and default) FA is a simple swapping of sprites. This class can be further extended to introduce different kinds of animations (such as colour/transparency changing over time).

**EmoteSO [ESO]**

ESO contains the instructions to carry out an emote, and is used as input to AC to animate AFs. Contains a StringToFeatureAnimationInstructionListDictionary, where timings of a FA can be adjusted.

_____

### Dependencies:

- [RotaryHeart’s SerializableDictionary](https://assetstore.unity.com/packages/tools/integration/serializabledictionary-90477)
- [Trinary Software's More Effective Coroutines](https://assetstore.unity.com/packages/tools/animation/more-effective-coroutines-free-54975)

_____

### Future Work:
- Emote Creation Tool in Unity
- Colour/alpha swap FeatureAnimation
- Colour/alpha tween FeatureAnimation
- Integrate with Particle Systems
- Example package
