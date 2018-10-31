using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimatableController))]
public class AnimatableControllerEditor : Editor {

	List<EmoteSO> testEmoteList = new List<EmoteSO>();

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Label(new GUIContent("AnimatableController Debug", "This is used for quick testing of emotes."));

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add"))
		{
			testEmoteList.Add(null);
		}

		if (GUILayout.Button("Delete"))
		{
			testEmoteList.RemoveAt(testEmoteList.Count - 1);
		}
		GUILayout.EndHorizontal();

		AnimatableController controller = (AnimatableController)target;

		GUILayout.Label(new GUIContent("Test Emotes", "List of emotes to test"));
		for (int i = 0; i < testEmoteList.Count; i++)
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Test Emote", GUILayout.Width(120));
			testEmoteList[i] = (EmoteSO)EditorGUILayout.ObjectField(testEmoteList[i], typeof(EmoteSO), false, GUILayout.ExpandWidth(true));
			if (GUILayout.Button(new GUIContent("+"), GUILayout.Width(40)))
			{
				controller.QueueEmote(testEmoteList[i]);
			}
			GUILayout.EndHorizontal();
		}	

		if (Application.isPlaying)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Controller Play"))
			{
				controller.PlayNextEmote();
			}

			if (GUILayout.Button("Clear queue"))
			{
				controller.ClearEmoteQueue();
				controller.StopEmote();
			}
			GUILayout.EndHorizontal();

			GUILayout.Label(new GUIContent("Emote Queue", ""));
			GUILayout.BeginVertical();
			EmoteSO[] queueArr = controller.emoteVisibleList.ToArray();
			for (int i = 0; i < queueArr.Length; i++)
			{
				EditorGUILayout.ObjectField(queueArr[i], typeof(EmoteSO), false);
			}
			GUILayout.EndVertical();
			Repaint();
		}
	}

}
