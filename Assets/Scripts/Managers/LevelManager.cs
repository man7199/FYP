using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using FusionHelper.Utility;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : NetworkSceneManagerBase
    {
        public const int LAUNCH_SCENE = 0;
        public const int LOBBY_SCENE = 1;

        [SerializeField] private UIScreen _dummyScreen;
        [SerializeField] private UIScreen _lobbyScreen;
        [SerializeField] private CanvasFader fader;

        public static LevelManager Instance => Singleton<LevelManager>.Instance;

        public static void LoadMenu()
        {
            Instance.Runner.SetActiveScene(LOBBY_SCENE);
        }

        public static void LoadMission(int sceneIndex)
        {
            Instance.Runner.SetActiveScene(sceneIndex);
        }

        protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
        {
            Debug.Log($"Loading scene {newScene}");

            PreLoadScene(newScene);

            List<NetworkObject> sceneObjects = new List<NetworkObject>();

            if (newScene >= LOBBY_SCENE)
            {
                yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
                Scene loadedScene = SceneManager.GetSceneByBuildIndex(newScene);
                Debug.Log($"Loaded scene {newScene}: {loadedScene}");
                sceneObjects = FindNetworkObjects(loadedScene, disable: false);
            }

            finished(sceneObjects);

            yield return null;

            // Spawning player
            if (GameManager.CurrentMission != null && newScene > LOBBY_SCENE)
            {
                if (Runner.GameMode == GameMode.Host)
                {
                    GameManager.CurrentMission.SpawnPlayer();
                }
            }

            PostLoadScene();
        }

        private void PreLoadScene(int scene)
        {
            if (scene > LOBBY_SCENE)
            {
                Debug.Log("Showing Dummy");
                UIScreen.Focus(_dummyScreen);
            }
            else if (scene == LOBBY_SCENE)
            {
                foreach (RoomPlayer player in RoomPlayer.Players)
                {
                    player.IsReady = false;
                }
                UIScreen.activeScreen.BackTo(_lobbyScreen);
            }
            else
            {
                UIScreen.BackToInitial();
            }
            fader.gameObject.SetActive(true);
            fader.FadeIn();
        }

        private void PostLoadScene()
        {
            fader.FadeOut();
        }
    }
}