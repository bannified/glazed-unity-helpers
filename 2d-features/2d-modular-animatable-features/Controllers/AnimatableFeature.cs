using System.Collections.Generic;
using UnityEngine;
using MEC;

/// <summary>
/// This component is attached to a gameObject that acts as a facial feature that can be animated. 
/// The AC will animate the feature if the input EmoteSO states so. 
/// For example, a left eye (eye_L) and a right eye (eye_R) can each have their own AF components, a pair of eyes (eyes) can also have its own AF component. 
/// The gameObject’s name is used as the AF’s key in AC’s StringToAnimatableFeatureDictionary.
/// </summary>
public class AnimatableFeature : MonoBehaviour {

	[SerializeField]
	private Sprite defaultSprite;

	[SerializeField]
	private SpriteRenderer featureSpriteRenderer;

	private List<FeatureAnimationInstruction> activeFeatureAnimations;
    
	public System.Action<List<FeatureAnimation>> FeatureAnimationsUpdate;

	public System.Action<List<FeatureAnimation>> FeatureAnimationsEnd;

	private List<Coroutine> animCoroutines;

	private void Awake()
	{
		initFeatureProperties();
	}

	private void initFeatureProperties()
	{
		featureSpriteRenderer = GetComponent<SpriteRenderer>();

		if (!featureSpriteRenderer)
		{
			Debug.Log(string.Format("WARNING: AnimatableFeature {0} does not have SpriteRenderer.", gameObject.name));
		}
	}

    /// <summary>
    /// Loads the animations.
    /// </summary>
    /// <param name="animations">Animations to be loaded to this feature.</param>
    /// <param name="emote">the EmoteSO used</param>
    /// <param name="startAnimating">If set to <c>true</c>, start animating feature immediately.</param>
	public void LoadAnimations(List<FeatureAnimationInstruction> animations, EmoteSO emote, bool startAnimating = true) {
		activeFeatureAnimations = animations;

		foreach (var inst in activeFeatureAnimations)
		{
			inst.animationLifeTime = (inst.animationLifeTime < 0) ? 
				emote.emoteTime : 
				Mathf.Min(inst.animationLifeTime, 
				          emote.emoteTime - inst.startOffset);
		}

		if (startAnimating) {
			Play();
		}
	}

    /// <summary>
	/// Stops all animation coroutines running (if any).
    /// </summary>
	public void Stop() {
		foreach (var cr in animCoroutines)
		{
			if (cr != null)
			{
				StopCoroutine(cr);
			}
		}
		animCoroutines.Clear();
	}

    /// <summary>
	/// Starts coroutines of all animations in <code>activeFeatureAnimations</code>.
    /// </summary>
	public void Play() {
		animCoroutines = new List<Coroutine>();
		foreach (var anim in activeFeatureAnimations)
        {
			animCoroutines.Add(StartCoroutine(_animCR(anim)));
        }
	}

	private void animStart(FeatureAnimation animation) {
        animation.AnimStart(this);
    }

	private void animUpdate(FeatureAnimation animation) {
		animation.AnimUpdate(this);
	}

	private void animEnd(FeatureAnimation animation) {
		animation.AnimEnd(this);
	}

	IEnumerator<float> _animCR(FeatureAnimationInstruction inst) {

		float runningTime = 0f;

		while (runningTime < inst.startOffset) {
			runningTime += Time.deltaTime;
			yield return Timing.WaitForOneFrame;
		}

		runningTime = 0f;

		animStart(inst.animation);

		while (runningTime < inst.animationLifeTime)
        {
            runningTime += Time.deltaTime;
			animUpdate(inst.animation);
            yield return Timing.WaitForOneFrame;
        }

		animEnd(inst.animation);
	}

    /// <summary>
    /// Changes the sprite of this feature.
	/// Used by FeatureAnimations.
    /// </summary>
    /// <param name="sprite">Sprite.</param>
	public void ChangeSprite(Sprite sprite) {
		featureSpriteRenderer.sprite = sprite;
	}

    /// <summary>
    /// Resets the feature to its defaults.
    /// </summary>
	public void ResetFeature() {
		featureSpriteRenderer.sprite = defaultSprite;
	}

}
