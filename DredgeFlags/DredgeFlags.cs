using System;
using UnityEngine;
using Winch.Core;

namespace DredgeFlags
{
    public class DredgeFlags : MonoBehaviour
    {
        public void Awake()
        {
            GameManager.Instance.OnGameStarted += OnGameLoaded;
            WinchCore.Log.Debug($"{nameof(DredgeFlags)} has loaded!");
        }

        public void OnGameLoaded()
        {
            try
            {
                GameObject flag = GameObject.Find("PlayerContainer(Clone)/Player/Boat5/FlagAccessory/Flag");
                SkinnedMeshRenderer skinnedMeshRenderer = flag.GetComponent<SkinnedMeshRenderer>();
                skinnedMeshRenderer.material.SetTexture("flag", Winch.Util.TextureUtil.GetTexture("custom_flag"));
            } catch (Exception e)
            {
                WinchCore.Log.Error(e);
            }
        }
    }
}
