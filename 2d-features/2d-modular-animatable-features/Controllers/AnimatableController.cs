using UnityEngine;
using System.Collections.Generic;
using MEC;

/// <summary>
/// The highest level class and main API of the Character Emote System.
/// Emotes should be played using this component.
/// Manages all AnimatableFeatures that are its children.
/// </summary>
public class AnimatableController : MonoBehaviour {

	[SerializeField]
	private StringToAnimatableFeatureDictionary _animatableFeaturesDict = new StringToAnimatableFeatureDictionary();

	[SerializeField]
	public StringToAnimatableFeatureDictionary animatableFeaturesDict {
		get { return _animatableFeaturesDict; }
	}

	private Queue<EmoteSO> emoteQueue = new Queue<EmoteSO>();

	public Queue<EmoteSO> emoteVisibleList
	{
		get { return emoteQueue; }
	}

	public System.Action<AnimatableController, EmoteSO> OnEmoteComplete;

	private Coroutine animationCR;

    /// <summary>
    /// Initializes the AnimatableController.
    /// </summary>
	public void Init()
	{
		emoteQueue = new Queue<EmoteSO>();
		populateAnimateableFeaturesDict();
	}

	private void populateAnimateableFeaturesDict() {
		AnimatableFeature[] features = GetComponentsInChildren<AnimatableFeature>();
		_animatableFeaturesDict = new StringToAnimatableFeatureDictionary();

		foreach (var f in features) {
			animatableFeaturesDict.Add(f.name, f);
		}
	}

	public void QueueEmote(EmoteSO emote) {
		emoteQueue.Enqueue(emote);
	}

    /// <summary>
    /// Plays an Emote on this AnimatableController.
    /// </summary>
    /// <param name="emote">Emote</param>
    /// <param name="interrupt">If set to <c>true</c>, interrupt.</param>
	public void PlayEmote(EmoteSO emote, bool interrupt = true) {
		if (interrupt) {
			StopEmote();
			ClearEmoteQueue();
			ResetAnimatableFeatures();
		}

		QueueEmote(emote);
		PlayNextEmote();
	}

    /// <summary>
    /// Plays the next emote.
    /// </summary>
	public void PlayNextEmote()
	{
		NextEmote();
	}

	private void ResetAnimatableFeatures() {
		foreach (var f in animatableFeaturesDict) {
			f.Value.ResetFeature();
		}
	}

	private void NextEmote() {

		if (emoteQueue.Count <= 0)
		{
			if (animationCR != null)
			{
				StopEmote();
			}
			return;
		}

		animationCR = StartCoroutine(_EmoteCR(emoteQueue.Dequeue()));
	}

	private void EmoteStart(EmoteSO emote) {

		emote.EmoteStart(this);
	}

	private void EmoteUpdate(EmoteSO emote)
    {

        emote.EmoteUpdate(this);
    }

    /// <summary>
    /// Handles this AnimatableController's behaviour when an emote is complete.
    /// </summary>
    /// <param name="emote">Emote ScriptableObject that completed</param>
	private void EmoteEnd(EmoteSO emote) {

		emote.EmoteComplete(this);
		NextEmote();
	}

    /// <summary>
	/// Stops the current emote.
    /// </summary>
	public void StopEmote()
	{
		if (animationCR != null)
		{
			StopCoroutine(animationCR);
			animationCR = null;
		}
	}

    /// <summary>
    /// Clears the emote queue.
    /// </summary>
	public void ClearEmoteQueue()
	{
		emoteQueue.Clear();
	}

	IEnumerator<float> _EmoteCR(EmoteSO emote) {
		float runningTime = 0f;

		EmoteStart(emote);

		while (runningTime < emote.emoteTime) {
			runningTime += Time.deltaTime;
			yield return Timing.WaitForOneFrame;
		}

		EmoteEnd(emote);

		OnEmoteComplete?.Invoke(this, emote);

	}

}
