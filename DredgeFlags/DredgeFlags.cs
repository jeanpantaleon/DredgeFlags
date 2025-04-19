using System;
using System.Collections.Generic;
using System.IO;
using FluffyUnderware.DevTools.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Winch.Core;

namespace DredgeFlags
{
    public class DredgeFlags : MonoBehaviour
    {
        public static DredgeFlags Instance;

        public Material mat;
        public Shader litcutout_shader;

        AssetBundle bundle;

        GameObject windowPrefab;
        GameObject gridElementPrefab;
        int flag_index = 0;
        public List<Flag> flagList = new List<Flag> ();

        public GameObject actualGameObject;
        public GameObject container;

        public Boolean showWindow = false;

        public void Awake()
        {
            Instance = this;

            // Loading assets (local flags)
            FlagLoader.LoadAssets();

            // Loading prefabs
            CheckForPrefabs("dredgeflags");
            windowPrefab = LoadPrefab("window");
            if (windowPrefab == null) WinchCore.Log.Error("Window prefab is null");
            gridElementPrefab = LoadPrefab("gridelement");
            if (windowPrefab == null) WinchCore.Log.Error("GridElement prefab is null");

            // Adding a listener
            GameManager.Instance.OnGameStarted += OnGameLoaded;

            WinchCore.Log.Debug($"{nameof(DredgeFlags)} has loaded!");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (showWindow)
                {
                    HideWindow();
                } else
                {
                    ShowWindow();
                }
            }
        }

        private void ShowWindow()
        {
            actualGameObject.SetActive(true);
            Cursor.visible = true;
            showWindow = true;
        }

        private void HideWindow()
        {
            actualGameObject.SetActive(false);
            Cursor.visible = false;
            showWindow = false;
        }

        public void AddFlag(Flag flag)
        {

        }

        public void AddFlags()
        {
            int i = 0;
            foreach (var flag in flagList)
            {
                int j = i;
                GameObject gridElement = Instantiate<GameObject>(gridElementPrefab);
                gridElement.transform.parent = actualGameObject.transform.GetChild(0).GetChild(0).GetChild(0);
                gridElement.transform.GetChild(0).GetComponent<Image>().sprite = flag.GetSprite();
                gridElement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = flag.name;
                gridElement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = flag.author;

                EventTrigger eventTrigger = gridElement.AddComponent<EventTrigger>();

                EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
                pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
                pointerEnterEntry.callback.AddListener((eventData) => {
                    gridElement.GetComponent<Image>().color = new Color(0, 0, 0.056f, 0.3f);
                });
                eventTrigger.triggers.Add(pointerEnterEntry);

                EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
                pointerExitEntry.eventID = EventTriggerType.PointerExit;
                pointerExitEntry.callback.AddListener((eventData) => {
                    gridElement.GetComponent<Image>().color = new Color(0, 0, 0.056f, 0.752f);
                });
                eventTrigger.triggers.Add(pointerExitEntry);

                EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
                pointerClickEntry.eventID = EventTriggerType.PointerClick;
                pointerClickEntry.callback.AddListener((eventData) => {
                    ChangeFlag(j);
                    HideWindow();
                });
                eventTrigger.triggers.Add(pointerClickEntry);

                i++;
            }
        }

        public void ChangeFlag(int flag_index)
        {
            try
            {
                WinchCore.Log.Info("Get Object");
                GameObject flag = GameObject.Find($"PlayerContainer(Clone)/Player/Boat{GameManager.Instance.SaveData.HullTier}/FlagAccessory/Flag");

                WinchCore.Log.Info("Get component");
                SkinnedMeshRenderer skinnedMeshRenderer = flag.GetComponent<SkinnedMeshRenderer>();

                WinchCore.Log.Info("Set texture");
                skinnedMeshRenderer.material.SetTexture("Texture2D_9aa7ba2263944b48bbf43c218dc48459", flagList[flag_index].texture);

            } catch (Exception e)
            {
                WinchCore.Log.Error(e);
            }
        }

        public void CheckForPrefabs(string assetBundleName)
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Mods", Path.Combine("DaSea.DredgeFlags", Path.Combine("Assets", assetBundleName)))));
            bundle.GetAllAssetNames().ForEach(WinchCore.Log.Error);
        }

        public GameObject LoadPrefab(string prefabName)
        {
            return bundle.LoadAsset<GameObject>($"assets/prefabs/{prefabName}.prefab");
        }

        public void OnGameLoaded()
        {
            try
            {
                container = Instantiate(GameObject.Find("GameCanvases/TimePassCanvas"));
                DontDestroyOnLoad(container);
                container.transform.parent = GameObject.Find("GameCanvases/").transform;
                container.name = "WindowCanvas";

                actualGameObject = Instantiate<GameObject>(windowPrefab);
                actualGameObject.SetActive(false);
                actualGameObject.transform.parent = container.transform;
                actualGameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-100, -100);
                actualGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(100, 100);

                AddFlags();
            } catch (Exception e)
            {
                WinchCore.Log.Error(e);
            }
        }

        public class UIControls : EventTrigger
        {
            public override void OnPointerEnter(PointerEventData data)
            {
                WinchCore.Log.Error("test");
                WinchCore.Log.Error(data.selectedObject.name);
                WinchCore.Log.Error("test2");
            }
        }
    }
}
