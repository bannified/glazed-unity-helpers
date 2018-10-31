using UnityEngine;

/// <summary>
/// ESO contains the instructions to carry out an emote, and is used as input to AC to animate AFs. 
/// Contains a StringToFeatureAnimationInstructionListDictionary, where timings of a FA can be adjusted.
/// </summary>
[CreateAssetMenu(fileName = "Emote_", menuName = "Emotes/Default")]
public class EmoteSO : ScriptableObject {

	public string emoteName;

	public float emoteTime = -1;

	public StringToFeatureAnimationInstructionListDictionary stringToAnimationListDict;

	public virtual void EmoteStart(AnimatableController animatableController) {
		foreach (var f in stringToAnimationListDict)
		{
			try
			{
				AnimatableFeature feature = null;
				animatableController.animatableFeaturesDict.TryGetValue(f.Key, out feature);

				feature.LoadAnimations(f.Value.list, this, true);       
			} catch (System.Exception e)
			{
				Debug.Log(e);
			}
		}
	}

	public virtual void EmoteComplete(AnimatableController animatableController)
    {

    }

	public virtual void EmoteUpdate(AnimatableController animatableController) {
		
	}
    
}
