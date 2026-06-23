using System.Collections.Generic;
using CardMiniGame.Popups;
using CardMiniGame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class VertigoDemoSceneWiringUtility
{
    public static void ExecuteFromBatch()
    {
        Debug.LogWarning("Batch scene wiring is disabled to avoid overwriting active scene work. Use Tools/Vertigo Demo/Wire Feedback Effects in the open Unity scene.");
    }

    [MenuItem("Tools/Vertigo Demo/Wire Feedback Effects")]
    public static void WireFeedbackEffectsForActiveScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogWarning("No loaded active scene found. Feedback wiring was skipped.");
            return;
        }

        WireFeedbackEffects(scene);
        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("Vertigo Demo feedback effects wired. Scene was marked dirty but not saved automatically.");
    }

    [MenuItem("Tools/Vertigo Demo/Wire Scene And Demo Data")]
    public static void WireSceneAndData()
    {
        WireFeedbackEffectsForActiveScene();
    }

    [MenuItem("Tools/Vertigo Demo/Apply Card Style Landscape UI")]
    public static void ApplyCardStyleLandscapeUi()
    {
        WireFeedbackEffectsForActiveScene();
    }

    private static void WireFeedbackEffects(Scene scene)
    {
        List<GameObject> sceneObjects = GetSceneObjects(scene);

        for (int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject sceneObject = sceneObjects[i];

            WireButtonFeedback(sceneObject);
            WireAppearFeedback(sceneObject);
        }

        WirePopupAppearReferences(sceneObjects);
    }

    private static void WireButtonFeedback(GameObject sceneObject)
    {
        Button button = sceneObject.GetComponent<Button>();

        if (button == null)
        {
            return;
        }

        AutoButtonBinder buttonBinder = GetOrAdd<AutoButtonBinder>(sceneObject);
        SetObject(buttonBinder, "button", button);
        GetOrAdd<ButtonSound>(sceneObject);
        UIScaleHoverEffect scaleHoverEffect = GetOrAdd<UIScaleHoverEffect>(sceneObject);
        Transform scaleTarget = FindAnimatorTarget(sceneObject.transform);
        SetObject(scaleHoverEffect, "scaleTarget", scaleTarget == null ? sceneObject.transform : scaleTarget);
    }

    private static void WireAppearFeedback(GameObject sceneObject)
    {
        bool shouldHaveAppearAnimator =
            sceneObject.GetComponent<RewardCardView>() != null ||
            sceneObject.GetComponent<RewardItemView>() != null ||
            sceneObject.name == "ui_result_card" ||
            sceneObject.name == "ui_popup_bomb_card" ||
            sceneObject.name == "ui_reward_item_template" ||
            sceneObject.name == "ui_animator_popup_bomb" ||
            sceneObject.name == "ui_animator_popup_cashout";

        if (!shouldHaveAppearAnimator)
        {
            return;
        }

        UIAppearAnimator appearAnimator = ConfigureAppearAnimator(sceneObject);
        RewardCardView rewardCardView = sceneObject.GetComponent<RewardCardView>();

        if (rewardCardView != null)
        {
            SetObject(rewardCardView, "appearAnimator", appearAnimator);
        }

        RewardItemView rewardItemView = sceneObject.GetComponent<RewardItemView>();

        if (rewardItemView != null)
        {
            SetObject(rewardItemView, "appearAnimator", appearAnimator);
        }
    }

    private static void WirePopupAppearReferences(List<GameObject> sceneObjects)
    {
        for (int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject sceneObject = sceneObjects[i];
            BombPopupView bombPopupView = sceneObject.GetComponent<BombPopupView>();

            if (bombPopupView != null)
            {
                GameObject animatorObject = FindChildByName(sceneObject.transform, "ui_animator_popup_bomb");
                UIAppearAnimator appearAnimator = ConfigureAppearAnimator(animatorObject == null ? sceneObject : animatorObject);
                SetObject(bombPopupView, "appearAnimator", appearAnimator);
            }

            CashoutPopupView cashoutPopupView = sceneObject.GetComponent<CashoutPopupView>();

            if (cashoutPopupView != null)
            {
                GameObject animatorObject = FindChildByName(sceneObject.transform, "ui_animator_popup_cashout");
                UIAppearAnimator appearAnimator = ConfigureAppearAnimator(animatorObject == null ? sceneObject : animatorObject);
                SetObject(cashoutPopupView, "appearAnimator", appearAnimator);
            }
        }
    }

    private static UIAppearAnimator ConfigureAppearAnimator(GameObject sceneObject)
    {
        CanvasGroup canvasGroup = GetOrAdd<CanvasGroup>(sceneObject);
        UIAppearAnimator appearAnimator = GetOrAdd<UIAppearAnimator>(sceneObject);
        SetObject(appearAnimator, "animatedRoot", sceneObject.transform);
        SetObject(appearAnimator, "canvasGroup", canvasGroup);
        return appearAnimator;
    }

    private static List<GameObject> GetSceneObjects(Scene scene)
    {
        List<GameObject> sceneObjects = new List<GameObject>();
        GameObject[] roots = scene.GetRootGameObjects();

        for (int i = 0; i < roots.Length; i++)
        {
            AddWithChildren(roots[i], sceneObjects);
        }

        return sceneObjects;
    }

    private static void AddWithChildren(GameObject gameObject, List<GameObject> sceneObjects)
    {
        sceneObjects.Add(gameObject);
        Transform transform = gameObject.transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            AddWithChildren(transform.GetChild(i).gameObject, sceneObjects);
        }
    }

    private static GameObject FindChildByName(Transform root, string childName)
    {
        if (root.name == childName)
        {
            return root.gameObject;
        }

        for (int i = 0; i < root.childCount; i++)
        {
            GameObject result = FindChildByName(root.GetChild(i), childName);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private static Transform FindAnimatorTarget(Transform root)
    {
        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);

            if (child.name.StartsWith("ui_animator"))
            {
                return child;
            }

            Transform nestedResult = FindAnimatorTarget(child);

            if (nestedResult != null)
            {
                return nestedResult;
            }
        }

        return null;
    }

    private static T GetOrAdd<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component != null)
        {
            return component;
        }

        component = gameObject.AddComponent<T>();
        EditorUtility.SetDirty(gameObject);
        return component;
    }

    private static void SetObject(Object target, string propertyName, Object value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property == null)
        {
            Debug.LogWarning("Missing serialized property " + propertyName + " on " + target.name);
            return;
        }

        if (property.objectReferenceValue == value)
        {
            return;
        }

        property.objectReferenceValue = value;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
    }
}
