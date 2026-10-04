using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Default.Editor.BuildSettings
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Editor.BuildSettings", sourceAssembly: "ParkMinPackages.Workflow.Default.Editor", sourceClassName: "BuildSettingProfile")]
	public class BuildSettingProfile : ScriptableObject
	{
		const string CreateMenuPath = "Assets/" + nameof(ParkMinDev) + "/Create/Build Settings Profile";

		[MenuItem(CreateMenuPath, priority = 20)]
		static void CreateProfile() {
			ProjectWindowUtil.CreateAsset(
				CreateInstance<BuildSettingProfile>(),
				"BuildSettingProfile.asset"
			);
		}
#if ODIN_INSPECTOR
		[Button, PropertyOrder(0)]
#endif
		public virtual void CopyFromBuildSettings() {
			Undo.RecordObject(this, "Copy Build Settings");

			companyName = PlayerSettings.companyName;
			productName = PlayerSettings.productName;
			androidApplicationIdentifier =
				PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);

			EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
			scenes = new SerializableEditorBuildSettingsScene[buildScenes.Length];

			for (int i = 0; i < buildScenes.Length; i++) {
				scenes[i] = new SerializableEditorBuildSettingsScene
				{
					enable = buildScenes[i].enabled,
					sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScenes[i].path)
				};
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssetIfDirty(this);
		}

		// Backwards-compatible name retained for existing callers.
		public virtual void CopyFromBuildSetting() {
			CopyFromBuildSettings();
		}

#if ODIN_INSPECTOR
		[Button, PropertyOrder(1)]
#endif
		public virtual void Apply() {
			List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();
			HashSet<string> addedScenePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			int skippedSceneCount = 0;

			if (scenes != null) {
				foreach (SerializableEditorBuildSettingsScene scene in scenes) {
					if (scene == null || scene.sceneAsset == null) {
						skippedSceneCount++;
						continue;
					}

					string scenePath = AssetDatabase.GetAssetPath(scene.sceneAsset);
					if (string.IsNullOrEmpty(scenePath) || !addedScenePaths.Add(scenePath)) {
						skippedSceneCount++;
						continue;
					}

					buildScenes.Add(new EditorBuildSettingsScene(scenePath, scene.enable));
				}
			}

			EditorBuildSettings.scenes = buildScenes.ToArray();
			PlayerSettings.companyName = companyName ?? string.Empty;
			PlayerSettings.productName = productName ?? string.Empty;

			if (applyAndroidApplicationIdentifier
			    && !string.IsNullOrWhiteSpace(androidApplicationIdentifier)) {
				PlayerSettings.SetApplicationIdentifier(
					NamedBuildTarget.Android,
					androidApplicationIdentifier.Trim()
				);
			}

			AssetDatabase.SaveAssets();

			if (skippedSceneCount > 0) {
				Debug.LogWarning(
					$"{nameof(BuildSettingProfile)} skipped {skippedSceneCount} missing or duplicate scene entries.",
					this
				);
			}
		}

		// Backwards-compatible name retained for existing callers.
		public virtual void Use() {
			Apply();
		}

		public string CompanyName
		{
			get { return companyName; }
		}
		public string ProductName
		{
			get { return productName; }
		}
		public string AndroidApplicationIdentifier
		{
			get { return androidApplicationIdentifier; }
		}
		public int SceneCount
		{
			get { return scenes?.Length ?? 0; }
		}

		[SerializeField] protected string companyName;
		[SerializeField] protected string productName;
		[SerializeField] protected bool applyAndroidApplicationIdentifier = true;
		[SerializeField] protected string androidApplicationIdentifier;

#if ODIN_INSPECTOR
		[SerializeField, PropertyOrder(2)]
#else
		[SerializeField]
#endif
		protected SerializableEditorBuildSettingsScene[] scenes =
			Array.Empty<SerializableEditorBuildSettingsScene>();

		[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Default.Editor.BuildSettings", sourceAssembly: "ParkMinPackages.Workflow.Default.Editor", sourceClassName: "BuildSettingProfile+SerializableEditorBuildSettingsScene")]
		[Serializable]
		protected class SerializableEditorBuildSettingsScene
		{
#if ODIN_INSPECTOR
			[HorizontalGroup("Scene"), LabelText("")]
#endif
			public bool enable;

#if ODIN_INSPECTOR
			[HorizontalGroup("Scene"), LabelText("")]
#endif
			public SceneAsset sceneAsset;
		}
	}
}