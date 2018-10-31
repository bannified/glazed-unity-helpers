using UnityEngine;

/// <summary>
/// Contains data and logic of an animation. 
/// The basic (and default) FA is a simple swapping of sprites. 
/// This class can be further extended to introduce different kinds of animations (such as colour/transparency changing over time).
/// </summary>
[CreateAssetMenu(fileName = "FAnim_", menuName = "Feature Animation/Default")]
public class FeatureAnimation : ScriptableObject {
    
	public Sprite Sprite;

	protected virtual void SpriteChange(AnimatableFeature feature) {
		feature.ChangeSprite(Sprite);
	}

	public virtual void AnimStart(AnimatableFeature feature) {
		SpriteChange(feature);
	}

	public virtual void AnimUpdate(AnimatableFeature feature)
    {
		
    }

	public virtual void AnimEnd(AnimatableFeature feature)
    {
		feature.ResetFeature();
    }

}
