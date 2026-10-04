using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ParkMinDev.UPM.Workflow.Default.Editor.ProjectStructures
{
	internal static class CreateScriptsStructureMenu
	{
		const string ScriptsMenuPath = "Assets/" + nameof(ParkMinDev) + "/Create/Scripts Folder Structure";
		const string DomainMenuPath = "Assets/" + nameof(ParkMinDev) + "/Create/Domain Folder Structure";

		static readonly string[] ScriptsFolderPaths =
		{
			"Scripts",
			"Scripts/Components",
			"Scripts/Components/Actors",
			"Scripts/Components/UIs",
			"Scripts/Objects",
			"Scripts/Interfaces",
			"Scripts/Enums",
			"Scripts/Extensions"
		};

		static readonly string[] DomainFolderPaths =
		{
			"Domain",
			"Domain/Animations",
			"Domain/Materials",
			"Domain/Prefabs",
			"Domain/Scenes",
			"Domain/Scripts",
			"Domain/Scripts/Components",
			"Domain/Scripts/Components/Actors",
			"Domain/Scripts/Components/UIs",
			"Domain/Scripts/Objects",
			"Domain/Scripts/Interfaces",
			"Domain/Scripts/Enums",
			"Domain/Scripts/Extensions",
			"Domain/Textures",
			"Domain/Audio",
			"Domain/Models",
			"Domain/ScriptableObjects",
			"Domain/Shaders"
		};

		[MenuItem(DomainMenuPath, priority = 0)]
		static void CreateDomain() {
			CreateFolderStructure(DomainFolderPaths, "Domain");
		}

		[MenuItem(ScriptsMenuPath, priority = 1)]
		static void CreateScripts() {
			CreateFolderStructure(ScriptsFolderPaths, "Scripts");
		}

		static void CreateFolderStructure(string[] folderPaths, string structureName) {
			string targetAssetPath = GetSelectedFolderAssetPath();
			string targetAbsolutePath = GetAbsolutePath(targetAssetPath);

			try {
				foreach (string folderPath in folderPaths) {
					Directory.CreateDirectory(Path.Combine(targetAbsolutePath, folderPath));
				}
			}
			catch (IOException exception) {
				ShowCreationError(exception);
				return;
			}
			catch (UnauthorizedAccessException exception) {
				ShowCreationError(exception);
				return;
			}

			AssetDatabase.Refresh();
			string rootFolderAssetPath = $"{targetAssetPath}/{structureName}";
			UnityEngine.Object rootFolder = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(rootFolderAssetPath);
			Selection.activeObject = rootFolder;
			EditorGUIUtility.PingObject(rootFolder);
		}

		[MenuItem(ScriptsMenuPath, validate = true)]
		static bool ValidateCreateScripts() {
			return ValidateCreate();
		}

		[MenuItem(DomainMenuPath, validate = true)]
		static bool ValidateCreateDomain() {
			return ValidateCreate();
		}

		static bool ValidateCreate() {
			string targetAssetPath = GetSelectedFolderAssetPath();
			if (string.IsNullOrEmpty(targetAssetPath))
				return false;

			bool isProjectAsset = targetAssetPath == "Assets" || targetAssetPath.StartsWith("Assets/");
			bool isPackageAsset = targetAssetPath.StartsWith("Packages/");
			return (isProjectAsset || isPackageAsset)
			       && !string.IsNullOrEmpty(GetAbsolutePath(targetAssetPath));
		}

		static string GetSelectedFolderAssetPath() {
			string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(assetPath))
				return "Assets";

			if (!AssetDatabase.IsValidFolder(assetPath))
				assetPath = Path.GetDirectoryName(assetPath)?.Replace('\\', '/');

			return assetPath;
		}

		static string GetAbsolutePath(string assetPath) {
			if (assetPath == "Assets" || assetPath.StartsWith("Assets/")) {
				string projectPath = Path.GetDirectoryName(Application.dataPath);
				return Path.Combine(projectPath, assetPath);
			}

			if (assetPath.StartsWith("Packages/")) {
				PackageInfo packageInfo = PackageInfo.FindForAssetPath(assetPath);
				if (packageInfo == null)
					return null;

				string packageAssetPath = $"Packages/{packageInfo.name}";
				string relativePath = assetPath.Substring(packageAssetPath.Length).TrimStart('/');
				return Path.Combine(packageInfo.resolvedPath, relativePath);
			}

			return null;
		}

		static void ShowCreationError(Exception exception) {
			EditorUtility.DisplayDialog(
				"Folder Structure",
				$"The selected package folder is read-only or unavailable.\n\n{exception.Message}",
				"OK"
			);
		}
	}
}