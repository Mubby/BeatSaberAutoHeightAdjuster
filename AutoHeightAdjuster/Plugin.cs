using IllusionPlugin;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace BeatSaberAutoHeightAdjuster
{
    public class Plugin : IPlugin
    {
        public string Name => "AutoHeightAdjuster";
        public string Version => "0.0.1";
        private const string GameSceneName = "StandardLevel";
        private bool _init;

        public void OnApplicationStart()
        {
            if (_init) return;
            _init = true;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene newScene, LoadSceneMode arg1)
        {
            Log("Scene Change");
            if (newScene.name == "GameCore")
            {
                Log("Measure player height");
                var mainSettingModel = Resources.FindObjectsOfTypeAll<MainSettingsModel>().FirstOrDefault();
                var sceneSetupData = Resources.FindObjectsOfTypeAll<StandardLevelSceneSetupDataSO>().FirstOrDefault();
                if(mainSettingModel == null || sceneSetupData == null)
                {
                    Log("Missing stuff");
                    return;
                }
                var res = UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.Head).y + mainSettingModel.roomCenter.y + MainSettingsModel.headPosToPlayerHeightOffset;
                sceneSetupData.gameplayCoreSetupData.playerSpecificSettings.playerHeight = res;
                Log($"Update height {res}");
            }
        }

        private void Log(string message)
        {
            Console.WriteLine($"[{Name}] {message}");
        }

        public void OnApplicationQuit()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {
            
        }

        public void OnLevelWasInitialized(int level)
        {
            
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
