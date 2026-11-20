#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HumanGrinder.Editor
{
    public static class CreatePrototypeScene
    {
        private const string ScenePath = "Assets/Scenes/Prototype.unity";

        [MenuItem("Tools/Human Grinder/Create Prototype Scene")]
        public static void Create()
        {
            if (!Directory.Exists("Assets/Scenes"))
            {
                Directory.CreateDirectory("Assets/Scenes");
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5.6f;
            camera.backgroundColor = new Color(0.055f, 0.057f, 0.09f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            GameObject game = new GameObject("Human Grinder Prototype");
            game.AddComponent<HumanGrinderPrototype>();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.Refresh();
            Debug.Log("Human Grinder: created Assets/Scenes/Prototype.unity and added it to Build Settings.");
        }
    }
}
#endif
