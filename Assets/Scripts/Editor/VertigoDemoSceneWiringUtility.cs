using System.Collections.Generic;
using CardMiniGame.Game;
using CardMiniGame.Input;
using CardMiniGame.Popups;
using CardMiniGame.Rewards;
using CardMiniGame.UI;
using CardMiniGame.Wheel;
using CardMiniGame.Zones;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class VertigoDemoSceneWiringUtility
{
    private const string ScenePath = "Assets/Scenes/MainScene.unity";
    private const string DataRoot = "Assets/ScriptableObjects";
    private const string ArtRoot = "Assets/Art/demo_content";
    private const string PrefabRoot = "Assets/Prefabs/UI";

    public static void ExecuteFromBatch()
    {
        WireSceneAndData();
    }

    [MenuItem("Tools/Vertigo Demo/Wire Scene And Demo Data")]
    public static void WireSceneAndData()
    {
        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        DemoData data = CreateDemoData();
        WireScene(scene, data);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Vertigo Demo/Apply Card Style Landscape UI")]
    public static void ApplyCardStyleLandscapeUi()
    {
        WireSceneAndData();
    }

    private static DemoData CreateDemoData()
    {
        EnsureFolder(DataRoot);
        EnsureFolder(DataRoot + "/Rewards");
        EnsureFolder(DataRoot + "/Wheels");
        EnsureFolder(DataRoot + "/Zones");
        EnsureFolder(DataRoot + "/Visuals");

        DemoSprites sprites = LoadDemoSprites();

        RewardDefinition coinSmall = CreateReward("CoinSmall", "Coin Small", 25, RewardType.Currency, RewardRarity.Common, sprites.CashIcon);
        RewardDefinition coinMedium = CreateReward("CoinMedium", "Coin Medium", 50, RewardType.Currency, RewardRarity.Common, sprites.GoldIcon);
        RewardDefinition coinLarge = CreateReward("CoinLarge", "Coin Large", 100, RewardType.Currency, RewardRarity.Rare, sprites.GoldChestIcon);
        RewardDefinition goldSpecial = CreateReward("GoldSpecial", "Gold Special", 200, RewardType.Special, RewardRarity.Epic, sprites.BigChestIcon);
        RewardDefinition gemSpecial = CreateReward("GemSpecial", "Gem Special", 350, RewardType.Special, RewardRarity.Legendary, sprites.SuperChestIcon);
        RewardDefinition bomb = CreateReward("Bomb", "Bomb", 0, RewardType.Bomb, RewardRarity.None, sprites.BombIcon);

        SpinVisualProfile visualProfile = LoadOrCreate<SpinVisualProfile>(DataRoot + "/Visuals/DefaultSpinVisualProfile.asset");
        visualProfile.SpinDuration = 2.5f;
        visualProfile.MinimumFullRotations = 4;
        visualProfile.WheelTint = Color.white;
        visualProfile.PointerTint = Color.white;
        EditorUtility.SetDirty(visualProfile);

        WheelConfig normalWheel = LoadOrCreate<WheelConfig>(DataRoot + "/Wheels/NormalWheel.asset");
        normalWheel.WheelType = WheelType.Normal;
        normalWheel.WheelBaseSprite = sprites.BronzeWheelBase;
        normalWheel.PointerSprite = sprites.BronzePointer;
        normalWheel.VisualProfile = visualProfile;
        normalWheel.Slices = new List<WheelSliceDefinition>
        {
            Slice(coinSmall, 1, false),
            Slice(coinMedium, 1, false),
            Slice(coinSmall, 2, false),
            Slice(bomb, 0, true),
            Slice(coinLarge, 1, false),
            Slice(coinMedium, 2, false),
            Slice(coinSmall, 3, false),
            Slice(coinLarge, 2, false)
        };
        EditorUtility.SetDirty(normalWheel);

        WheelConfig safeWheel = LoadOrCreate<WheelConfig>(DataRoot + "/Wheels/SafeWheel.asset");
        safeWheel.WheelType = WheelType.Safe;
        safeWheel.WheelBaseSprite = sprites.SilverWheelBase;
        safeWheel.PointerSprite = sprites.SilverPointer;
        safeWheel.VisualProfile = visualProfile;
        safeWheel.Slices = new List<WheelSliceDefinition>
        {
            Slice(coinSmall, 3, false),
            Slice(coinMedium, 2, false),
            Slice(coinLarge, 1, false),
            Slice(coinMedium, 3, false),
            Slice(coinLarge, 2, false),
            Slice(goldSpecial, 1, false),
            Slice(coinSmall, 5, false),
            Slice(coinMedium, 4, false)
        };
        EditorUtility.SetDirty(safeWheel);

        WheelConfig superWheel = LoadOrCreate<WheelConfig>(DataRoot + "/Wheels/SuperWheel.asset");
        superWheel.WheelType = WheelType.Super;
        superWheel.WheelBaseSprite = sprites.GoldenWheelBase;
        superWheel.PointerSprite = sprites.GoldenPointer;
        superWheel.VisualProfile = visualProfile;
        superWheel.Slices = new List<WheelSliceDefinition>
        {
            Slice(goldSpecial, 1, false),
            Slice(gemSpecial, 1, false),
            Slice(coinLarge, 4, false),
            Slice(goldSpecial, 2, false),
            Slice(gemSpecial, 2, false),
            Slice(coinLarge, 6, false),
            Slice(goldSpecial, 3, false),
            Slice(gemSpecial, 3, false)
        };
        EditorUtility.SetDirty(superWheel);

        ZoneConfig zoneConfig = LoadOrCreate<ZoneConfig>(DataRoot + "/Zones/DefaultZoneConfig.asset");
        zoneConfig.SafeZoneInterval = 5;
        zoneConfig.SuperZoneInterval = 30;
        zoneConfig.RewardScalingPerZone = 1.15f;
        zoneConfig.NormalWheel = normalWheel;
        zoneConfig.SafeWheel = safeWheel;
        zoneConfig.SuperWheel = superWheel;
        EditorUtility.SetDirty(zoneConfig);

        return new DemoData(zoneConfig, sprites, bomb);
    }

    private static RewardDefinition CreateReward(
        string rewardId,
        string displayName,
        int baseAmount,
        RewardType rewardType,
        RewardRarity rarity,
        Sprite icon)
    {
        RewardDefinition reward = LoadOrCreate<RewardDefinition>(DataRoot + "/Rewards/" + rewardId + ".asset");
        reward.RewardId = rewardId;
        reward.DisplayName = displayName;
        reward.Icon = icon;
        reward.BaseAmount = baseAmount;
        reward.RewardType = rewardType;
        reward.Rarity = rarity;
        EditorUtility.SetDirty(reward);
        return reward;
    }

    private static WheelSliceDefinition Slice(RewardDefinition reward, int multiplier, bool isBomb)
    {
        return new WheelSliceDefinition
        {
            Reward = reward,
            AmountMultiplier = multiplier,
            IsBomb = isBomb
        };
    }

    private static void WireScene(Scene scene, DemoData data)
    {
        ConfigureLandscapeProjectSettings();

        GameObject bootstrapperObject = FindRequired(scene, "game_bootstrapper");
        GameObject controllerObject = FindRequired(scene, "game_session_controller");
        GameObject hudObject = FindRequired(scene, "ui_screen_game");
        GameObject wheelObject = FindRequired(scene, "ui_container_wheel");
        GameObject wheelAnimatorObject = FindRequired(scene, "ui_animator_wheel_spin");
        GameObject rewardPanelObject = FindRequired(scene, "ui_panel_collected_rewards");
        GameObject rewardListObject = FindRequired(scene, "ui_container_reward_list");
        GameObject rewardTemplateObject = FindRequired(scene, "ui_reward_item_template");
        GameObject bombPopupObject = FindRequired(scene, "ui_popup_bomb");
        GameObject cashoutPopupObject = FindRequired(scene, "ui_popup_cashout");
        CardStyleObjects cardStyleObjects = ConfigureCardStyleLayout(scene, data);

        GameBootstrapper bootstrapper = GetOrAdd<GameBootstrapper>(bootstrapperObject);
        GameSessionController controller = GetOrAdd<GameSessionController>(controllerObject);
        GetOrAdd<MobileInputController>(bootstrapperObject);

        GameHudView hudView = GetOrAdd<GameHudView>(hudObject);
        WheelView wheelView = GetOrAdd<WheelView>(wheelObject);
        WheelSpinAnimator wheelSpinAnimator = GetOrAdd<WheelSpinAnimator>(wheelAnimatorObject);
        RewardListView rewardListView = GetOrAdd<RewardListView>(rewardPanelObject);
        RewardItemView rewardItemView = GetOrAdd<RewardItemView>(rewardTemplateObject);
        BombPopupView bombPopupView = GetOrAdd<BombPopupView>(bombPopupObject);
        CashoutPopupView cashoutPopupView = GetOrAdd<CashoutPopupView>(cashoutPopupObject);
        PopupScaleAnimator bombPopupAnimator = GetOrAdd<PopupScaleAnimator>(FindRequired(scene, "ui_animator_popup_bomb"));
        PopupScaleAnimator cashoutPopupAnimator = GetOrAdd<PopupScaleAnimator>(FindRequired(scene, "ui_animator_popup_cashout"));

        AutoButtonBinder spinButton = ConfigureButton(FindRequired(scene, "ui_button_spin"));
        AutoButtonBinder leaveButton = ConfigureButton(FindRequired(scene, "ui_button_leave"));
        AutoButtonBinder restartButton = ConfigureButton(FindRequired(scene, "ui_button_restart"));
        AutoButtonBinder cashoutRestartButton = ConfigureButton(FindRequired(scene, "ui_button_cashout_restart"));
        AutoButtonBinder continueOptionalButton = ConfigureButton(FindRequired(scene, "ui_button_continue_optional"));

        SetObject(bootstrapper, "zoneConfig", data.ZoneConfig);
        SetObject(bootstrapper, "gameSessionController", controller);

        SetObject(controller, "hudView", hudView);
        SetObject(controller, "rewardListView", rewardListView);
        SetObject(controller, "wheelView", wheelView);
        SetObject(controller, "bombPopupView", bombPopupView);
        SetObject(controller, "cashoutPopupView", cashoutPopupView);

        SetObject(hudView, "zoneTitleText", FindRequired(scene, "ui_text_zone_title_value").GetComponent<TMP_Text>());
        SetObject(hudView, "zoneTypeText", FindRequired(scene, "ui_text_zone_type_value").GetComponent<TMP_Text>());
        SetObject(hudView, "totalRewardText", FindRequired(scene, "ui_text_total_reward_value").GetComponent<TMP_Text>());
        SetObject(hudView, "zoneTrackView", cardStyleObjects.ZoneTrackView);
        SetObject(hudView, "zoneBadgeView", cardStyleObjects.ZoneBadgeView);
        SetObject(hudView, "spinButton", spinButton);
        SetObject(hudView, "leaveButton", leaveButton);

        SetObject(wheelSpinAnimator, "animatedRoot", wheelAnimatorObject.GetComponent<RectTransform>());
        SetObject(wheelView, "previewConfig", data.ZoneConfig.NormalWheel);
        SetObject(wheelView, "wheelBaseImage", FindRequired(scene, "ui_image_wheel_base").GetComponent<Image>());
        SetObject(wheelView, "pointerImage", FindRequired(scene, "ui_image_pointer").GetComponent<Image>());
        SetObject(wheelView, "spinAnimator", wheelSpinAnimator);
        SetObject(wheelView, "resultCardView", cardStyleObjects.ResultCardView);
        SetWheelSlices(wheelView, scene);

        SetObject(rewardListView, "rewardListContainer", rewardListObject.transform);
        SetObject(rewardListView, "rewardItemTemplate", rewardItemView);
        ConfigureRewardItemTemplate(rewardItemView, rewardTemplateObject);

        SetObject(bombPopupAnimator, "animatedRoot", bombPopupAnimator.transform);
        SetObject(cashoutPopupAnimator, "animatedRoot", cashoutPopupAnimator.transform);

        SetObject(bombPopupView, "titleText", FindRequired(scene, "ui_text_bomb_title_value").GetComponent<TMP_Text>());
        SetObject(bombPopupView, "descriptionText", FindRequired(scene, "ui_text_bomb_description_value").GetComponent<TMP_Text>());
        SetObject(bombPopupView, "restartButton", restartButton);
        SetObject(bombPopupView, "continueOptionalButton", continueOptionalButton);
        SetObject(bombPopupView, "popupAnimator", bombPopupAnimator);
        SetObject(bombPopupView, "bombCardView", cardStyleObjects.BombCardView);
        SetObject(bombPopupView, "bombReward", data.BombReward);

        SetObject(cashoutPopupView, "titleText", FindRequired(scene, "ui_text_cashout_title_value").GetComponent<TMP_Text>());
        SetObject(cashoutPopupView, "amountText", FindRequired(scene, "ui_text_cashout_amount_value").GetComponent<TMP_Text>());
        SetObject(cashoutPopupView, "restartButton", cashoutRestartButton);
        SetObject(cashoutPopupView, "popupAnimator", cashoutPopupAnimator);

        bombPopupObject.SetActive(false);
        cashoutPopupObject.SetActive(false);

        ApplySceneSprites(scene, data.Sprites);
        wheelView.Build(data.ZoneConfig.NormalWheel);
        SaveReusablePrefabs(scene);
    }

    private static void ConfigureLandscapeProjectSettings()
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
    }

    private static CardStyleObjects ConfigureCardStyleLayout(Scene scene, DemoData data)
    {
        GameObject canvasRoot = FindRequired(scene, "ui_canvas_root");
        CanvasScaler canvasScaler = canvasRoot.GetComponent<CanvasScaler>();

        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            EditorUtility.SetDirty(canvasScaler);
        }

        GameObject screen = FindRequired(scene, "ui_screen_game");
        GameObject background = FindRequired(scene, "ui_panel_background");
        GameObject rewardPanel = FindRequired(scene, "ui_panel_collected_rewards");
        GameObject rewardList = FindRequired(scene, "ui_container_reward_list");
        GameObject wheel = FindRequired(scene, "ui_container_wheel");
        GameObject actions = FindRequired(scene, "ui_container_actions");
        GameObject spinButton = FindRequired(scene, "ui_button_spin");
        GameObject leaveButton = FindRequired(scene, "ui_button_leave");
        GameObject bombPopup = FindRequired(scene, "ui_popup_bomb");
        GameObject cashoutPopup = FindRequired(scene, "ui_popup_cashout");

        StretchToParent(screen.GetComponent<RectTransform>());
        StretchToParent(background.GetComponent<RectTransform>());
        Image backgroundImage = GetOrAdd<Image>(background);
        ConfigureImage(backgroundImage, data.Sprites.PanelBackground, false, false);
        backgroundImage.color = new Color(0.04f, 0.035f, 0.04f, 1f);
        EditorUtility.SetDirty(backgroundImage);

        ConfigureWatermark(screen.transform);
        ConfigureLeftRewardPanel(rewardPanel, rewardList, leaveButton, data.Sprites);
        ConfigureWheelArea(wheel, data.Sprites);
        ConfigureActions(actions, spinButton);
        ZoneTrackView zoneTrackView = ConfigureZoneTrack(screen.transform, data.Sprites);
        ZoneBadgeView zoneBadgeView = ConfigureZoneBadges(screen.transform, data.Sprites);
        RewardCardView resultCardView = ConfigureRewardCard(wheel.transform, "ui_result_card", data.Sprites.PopupFrame, false);
        RewardCardView bombCardView = ConfigureBombPopupLayout(bombPopup, data.Sprites);
        ConfigureCashoutPopupLayout(cashoutPopup, data.Sprites);

        return new CardStyleObjects(zoneTrackView, zoneBadgeView, resultCardView, bombCardView);
    }

    private static void ConfigureWatermark(Transform parent)
    {
        GameObject watermark = GetOrCreateChild(parent, "ui_text_brand_watermark_value");
        RectTransform rectTransform = watermark.GetComponent<RectTransform>();
        ConfigureRect(
            rectTransform,
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(1000f, 140f),
            new Vector2(80f, -40f));

        TextMeshProUGUI text = GetOrAdd<TextMeshProUGUI>(watermark);
        ConfigureText(text, "CHARLIE REDD", 68f, TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 0.12f));
        text.characterSpacing = 36f;
    }

    private static void ConfigureLeftRewardPanel(GameObject rewardPanel, GameObject rewardList, GameObject leaveButton, DemoSprites sprites)
    {
        RectTransform panelRect = rewardPanel.GetComponent<RectTransform>();
        ConfigureRect(
            panelRect,
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(300f, 760f),
            new Vector2(28f, -20f));
        ConfigureImage(GetOrAdd<Image>(rewardPanel), sprites.PanelFrame, false, false);

        leaveButton.transform.SetParent(rewardPanel.transform, false);
        ConfigureRect(
            leaveButton.GetComponent<RectTransform>(),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(250f, 68f),
            new Vector2(0f, -52f));
        SetButtonText(leaveButton, "EXIT");

        GameObject title = Find(rewardPanel.scene, "ui_text_collected_title");

        if (title != null)
        {
            title.transform.SetParent(rewardPanel.transform, false);
            ConfigureRect(
                title.GetComponent<RectTransform>(),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(240f, 44f),
                new Vector2(0f, -120f));
            TMP_Text titleText = title.GetComponent<TMP_Text>();

            if (titleText != null)
            {
                titleText.text = "REWARDS";
                titleText.alignment = TextAlignmentOptions.Center;
                titleText.fontSize = 26f;
                titleText.color = Color.white;
                EditorUtility.SetDirty(titleText);
            }
        }

        rewardList.transform.SetParent(rewardPanel.transform, false);
        ConfigureRect(
            rewardList.GetComponent<RectTransform>(),
            new Vector2(0f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0.5f, 0.5f),
            Vector2.zero,
            Vector2.zero);
        rewardList.GetComponent<RectTransform>().offsetMin = new Vector2(28f, 30f);
        rewardList.GetComponent<RectTransform>().offsetMax = new Vector2(-22f, -156f);
        EditorUtility.SetDirty(rewardList);
    }

    private static void ConfigureWheelArea(GameObject wheel, DemoSprites sprites)
    {
        ConfigureRect(
            wheel.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(700f, 700f),
            new Vector2(130f, -95f));

        GameObject animatedRoot = FindRequired(wheel.scene, "ui_animator_wheel_spin");
        ConfigureRect(
            animatedRoot.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(460f, 460f),
            new Vector2(0f, -80f));

        GameObject wheelBase = FindRequired(wheel.scene, "ui_image_wheel_base");
        ConfigureRect(
            wheelBase.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(460f, 460f),
            Vector2.zero);
        ConfigureImage(GetOrAdd<Image>(wheelBase), sprites.BronzeWheelBase, false, true);

        GameObject pointer = FindRequired(wheel.scene, "ui_image_pointer");
        pointer.transform.SetParent(wheel.transform, false);
        ConfigureRect(
            pointer.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(84f, 104f),
            new Vector2(0f, 185f));
        ConfigureImage(GetOrAdd<Image>(pointer), sprites.BronzePointer, false, true);

        GameObject slices = FindRequired(wheel.scene, "ui_container_wheel_slices");
        StretchToParent(slices.GetComponent<RectTransform>());
    }

    private static void ConfigureActions(GameObject actions, GameObject spinButton)
    {
        ConfigureRect(
            actions.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(360f, 100f),
            new Vector2(130f, 54f));

        spinButton.transform.SetParent(actions.transform, false);
        ConfigureRect(
            spinButton.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(260f, 78f),
            Vector2.zero);
        SetButtonText(spinButton, "SPIN");
    }

    private static ZoneTrackView ConfigureZoneTrack(Transform parent, DemoSprites sprites)
    {
        GameObject track = GetOrCreateChild(parent, "ui_container_zone_track");
        ConfigureRect(
            track.GetComponent<RectTransform>(),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(980f, 76f),
            new Vector2(80f, -34f));
        ConfigureImage(GetOrAdd<Image>(track), sprites.PanelFrame, false, false);

        GameObject marker = GetOrCreateChild(track.transform, "ui_image_zone_track_current");
        ConfigureRect(
            marker.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(86f, 70f),
            Vector2.zero);
        ConfigureImage(GetOrAdd<Image>(marker), sprites.PanelBackground, false, false);

        List<Object> zoneTexts = new List<Object>();

        for (int i = 0; i < 7; i++)
        {
            GameObject textObject = GetOrCreateChild(track.transform, "ui_text_zone_track_" + i.ToString("00") + "_value");
            ConfigureRect(
                textObject.GetComponent<RectTransform>(),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(100f, 60f),
                new Vector2((i - 3) * 112f, 0f));
            TextMeshProUGUI text = GetOrAdd<TextMeshProUGUI>(textObject);
            ConfigureText(text, (i + 1).ToString(), 34f, TextAlignmentOptions.Center, Color.white);
            zoneTexts.Add(text);
        }

        ZoneTrackView zoneTrackView = GetOrAdd<ZoneTrackView>(track);
        SetObject(zoneTrackView, "currentZoneMarker", GetOrAdd<Image>(marker));
        SetObjectList(zoneTrackView, "zoneTexts", zoneTexts);
        return zoneTrackView;
    }

    private static ZoneBadgeView ConfigureZoneBadges(Transform parent, DemoSprites sprites)
    {
        GameObject badgeContainer = GetOrCreateChild(parent, "ui_container_zone_badges");
        ConfigureRect(
            badgeContainer.GetComponent<RectTransform>(),
            new Vector2(1f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 1f),
            new Vector2(260f, 160f),
            new Vector2(-42f, -86f));

        GameObject superBadge = ConfigureBadge(badgeContainer.transform, "ui_badge_super_zone", sprites.PanelFrame, "SUPER\nZONE 30", 42f);
        GameObject safeBadge = ConfigureBadge(badgeContainer.transform, "ui_badge_safe_zone", sprites.PanelFrame, "SAFE\nZONE 5", -42f);

        ZoneBadgeView badgeView = GetOrAdd<ZoneBadgeView>(badgeContainer);
        SetObject(badgeView, "superZoneBadgeImage", GetOrAdd<Image>(superBadge));
        SetObject(badgeView, "safeZoneBadgeImage", GetOrAdd<Image>(safeBadge));
        SetObject(badgeView, "superZoneText", FindInChildrenRequired(superBadge.transform, "ui_text_super_zone_badge_value").GetComponent<TMP_Text>());
        SetObject(badgeView, "safeZoneText", FindInChildrenRequired(safeBadge.transform, "ui_text_safe_zone_badge_value").GetComponent<TMP_Text>());
        return badgeView;
    }

    private static GameObject ConfigureBadge(Transform parent, string objectName, Sprite sprite, string textValue, float y)
    {
        GameObject badge = GetOrCreateChild(parent, objectName);
        ConfigureRect(
            badge.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(235f, 66f),
            new Vector2(0f, y));
        ConfigureImage(GetOrAdd<Image>(badge), sprite, false, false);

        string textName = objectName == "ui_badge_super_zone"
            ? "ui_text_super_zone_badge_value"
            : "ui_text_safe_zone_badge_value";
        GameObject textObject = GetOrCreateChild(badge.transform, textName);
        StretchToParent(textObject.GetComponent<RectTransform>());
        TextMeshProUGUI text = GetOrAdd<TextMeshProUGUI>(textObject);
        ConfigureText(text, textValue, 24f, TextAlignmentOptions.Center, Color.white);
        return badge;
    }

    private static RewardCardView ConfigureRewardCard(Transform parent, string objectName, Sprite frameSprite, bool visible)
    {
        GameObject card = GetOrCreateChild(parent, objectName);
        ConfigureRect(
            card.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(320f, 430f),
            new Vector2(0f, 68f));
        Image frameImage = GetOrAdd<Image>(card);
        ConfigureImage(frameImage, frameSprite, false, false);

        GameObject title = GetOrCreateChild(card.transform, "ui_text_result_card_title_value");
        ConfigureRect(
            title.GetComponent<RectTransform>(),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(280f, 70f),
            new Vector2(0f, -56f));
        TextMeshProUGUI titleText = GetOrAdd<TextMeshProUGUI>(title);
        ConfigureText(titleText, "REWARD", 30f, TextAlignmentOptions.Center, Color.white);

        GameObject icon = GetOrCreateChild(card.transform, "ui_image_result_card_icon_value");
        ConfigureRect(
            icon.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(160f, 160f),
            new Vector2(0f, 16f));
        Image iconImage = GetOrAdd<Image>(icon);
        iconImage.raycastTarget = false;
        iconImage.maskable = false;
        iconImage.preserveAspect = true;
        EditorUtility.SetDirty(iconImage);

        GameObject amount = GetOrCreateChild(card.transform, "ui_text_result_card_amount_value");
        ConfigureRect(
            amount.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(260f, 72f),
            new Vector2(0f, 52f));
        TextMeshProUGUI amountText = GetOrAdd<TextMeshProUGUI>(amount);
        ConfigureText(amountText, "x0", 34f, TextAlignmentOptions.Center, Color.white);

        RewardCardView rewardCardView = GetOrAdd<RewardCardView>(card);
        SetObject(rewardCardView, "root", card);
        SetObject(rewardCardView, "frameImage", frameImage);
        SetObject(rewardCardView, "iconImage", iconImage);
        SetObject(rewardCardView, "titleText", titleText);
        SetObject(rewardCardView, "amountText", amountText);
        card.SetActive(visible);
        return rewardCardView;
    }

    private static RewardCardView ConfigureBombPopupLayout(GameObject bombPopup, DemoSprites sprites)
    {
        StretchToParent(bombPopup.GetComponent<RectTransform>());

        GameObject panel = FindRequired(bombPopup.scene, "ui_panel_popup_bomb");
        ConfigureRect(
            panel.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(860f, 760f),
            Vector2.zero);
        ConfigureImage(GetOrAdd<Image>(panel), sprites.PopupFrame, false, false);

        GameObject title = FindRequired(bombPopup.scene, "ui_text_bomb_title_value");
        ConfigureRect(
            title.GetComponent<RectTransform>(),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(780f, 76f),
            new Vector2(0f, -52f));
        TMP_Text titleText = title.GetComponent<TMP_Text>();

        if (titleText != null)
        {
            titleText.text = "OH NO, A BOMB EXPLODED RIGHT IN YOUR HANDS!";
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.fontSize = 34f;
            titleText.color = Color.white;
            EditorUtility.SetDirty(titleText);
        }

        GameObject description = FindRequired(bombPopup.scene, "ui_text_bomb_description_value");
        ConfigureRect(
            description.GetComponent<RectTransform>(),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(680f, 56f),
            new Vector2(0f, -112f));

        RewardCardView bombCardView = ConfigureRewardCard(panel.transform, "ui_popup_bomb_card", sprites.PopupFrame, true);
        ConfigureRect(
            bombCardView.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(330f, 440f),
            new Vector2(0f, 10f));

        GameObject restartButton = FindRequired(bombPopup.scene, "ui_button_restart");
        ConfigureRect(
            restartButton.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(250f, 70f),
            new Vector2(-150f, 54f));
        SetButtonText(restartButton, "GIVE UP");

        GameObject continueButton = FindRequired(bombPopup.scene, "ui_button_continue_optional");
        ConfigureRect(
            continueButton.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(250f, 70f),
            new Vector2(150f, 54f));
        continueButton.SetActive(false);
        return bombCardView;
    }

    private static void ConfigureCashoutPopupLayout(GameObject cashoutPopup, DemoSprites sprites)
    {
        StretchToParent(cashoutPopup.GetComponent<RectTransform>());

        GameObject panel = FindRequired(cashoutPopup.scene, "ui_panel_popup_cashout");
        ConfigureRect(
            panel.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(560f, 360f),
            Vector2.zero);
        ConfigureImage(GetOrAdd<Image>(panel), sprites.PopupFrame, false, false);
    }

    private static DemoSprites LoadDemoSprites()
    {
        return new DemoSprites(
            LoadRequiredSprite("ui_spin_bronze_base.png"),
            LoadRequiredSprite("ui_spin_silver_base.png"),
            LoadRequiredSprite("ui_spin_golden_base.png"),
            LoadRequiredSprite("ui_spin_bronze_indicator.png"),
            LoadRequiredSprite("ui_spin_silver_indicator.png"),
            LoadRequiredSprite("ui_spin_golden_indicator.png"),
            LoadRequiredSprite("UI_icon_cash.png"),
            LoadRequiredSprite("UI_icon_gold.png"),
            LoadRequiredSprite("UI_icon_chest_gold_nolight.png"),
            LoadRequiredSprite("UI_icon_chest_big_nolight.png"),
            LoadRequiredSprite("UI_icon_chest_super_nolight.png"),
            LoadRequiredSprite("ui_card_icon_death.png"),
            LoadRequiredSprite("ui_spin_generic_button.png"),
            LoadRequiredSprite("UI_button_grey_standard.png"),
            LoadRequiredSprite("UI_button_orange_standard.png"),
            LoadRequiredSprite("ui_card_panel_zone_bg.png"),
            LoadRequiredSprite("ui_card_frame_12px_neutral.png"),
            LoadRequiredSprite("ui_card_frame_gardient.png"));
    }

    private static Sprite LoadRequiredSprite(string fileName)
    {
        string path = ArtRoot + "/" + fileName;
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

        if (sprite == null)
        {
            throw new System.InvalidOperationException("Missing required sprite: " + path);
        }

        return sprite;
    }

    private static void ApplySceneSprites(Scene scene, DemoSprites sprites)
    {
        SetImageSprite(scene, "ui_image_wheel_base", sprites.BronzeWheelBase, false, true);
        SetImageSprite(scene, "ui_image_pointer", sprites.BronzePointer, false, true);

        SetImageSprite(scene, "ui_button_spin", sprites.SpinButton, true, false);
        SetImageSprite(scene, "ui_button_leave", sprites.SecondaryButton, true, false);
        SetImageSprite(scene, "ui_button_restart", sprites.SecondaryButton, true, false);
        SetImageSprite(scene, "ui_button_continue_optional", sprites.PrimaryButton, true, false);
        SetImageSprite(scene, "ui_button_cashout_restart", sprites.PrimaryButton, true, false);

        SetImageSprite(scene, "ui_panel_background", sprites.PanelBackground, false, false);
        Image backgroundImage = FindRequired(scene, "ui_panel_background").GetComponent<Image>();
        backgroundImage.color = new Color(0.04f, 0.035f, 0.04f, 1f);
        EditorUtility.SetDirty(backgroundImage);

        SetImageSprite(scene, "ui_panel_collected_rewards", sprites.PanelFrame, false, false);
        SetImageSprite(scene, "ui_panel_popup_bomb", sprites.PopupFrame, false, false);
        SetImageSprite(scene, "ui_panel_popup_cashout", sprites.PopupFrame, false, false);
        SetImageSprite(scene, "ui_reward_item_template", sprites.PanelFrame, false, false);

        for (int i = 0; i < 8; i++)
        {
            GameObject sliceObject = FindRequired(scene, "ui_container_slice_" + i.ToString("00"));
            Image background = FindInChildrenRequired(sliceObject.transform, "ui_image_slice_background").GetComponent<Image>();
            background.enabled = false;
            background.raycastTarget = false;
            background.maskable = false;
            EditorUtility.SetDirty(background);
        }
    }

    private static void SetImageSprite(Scene scene, string objectName, Sprite sprite, bool isButton, bool preserveAspect)
    {
        Image image = FindRequired(scene, objectName).GetComponent<Image>();

        if (image == null)
        {
            throw new System.InvalidOperationException("Missing Image component: " + objectName);
        }

        image.sprite = sprite;
        image.type = sprite != null && sprite.border.sqrMagnitude > 0f ? Image.Type.Sliced : Image.Type.Simple;
        image.color = Color.white;
        image.preserveAspect = preserveAspect;
        image.raycastTarget = isButton;
        image.maskable = false;
        EditorUtility.SetDirty(image);
    }

    private static void ConfigureImage(Image image, Sprite sprite, bool isButton, bool preserveAspect)
    {
        if (image == null)
        {
            return;
        }

        image.sprite = sprite;
        image.type = sprite != null && sprite.border.sqrMagnitude > 0f ? Image.Type.Sliced : Image.Type.Simple;
        image.color = Color.white;
        image.preserveAspect = preserveAspect;
        image.raycastTarget = isButton;
        image.maskable = false;
        EditorUtility.SetDirty(image);
    }

    private static void ConfigureRect(
        RectTransform rectTransform,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 pivot,
        Vector2 sizeDelta,
        Vector2 anchoredPosition)
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.localScale = Vector3.one;
        EditorUtility.SetDirty(rectTransform);
    }

    private static void StretchToParent(RectTransform rectTransform)
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        EditorUtility.SetDirty(rectTransform);
    }

    private static void ConfigureText(
        TextMeshProUGUI text,
        string value,
        float fontSize,
        TextAlignmentOptions alignment,
        Color color)
    {
        if (text == null)
        {
            return;
        }

        text.text = value;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = color;
        text.enableWordWrapping = false;
        text.raycastTarget = false;
        EditorUtility.SetDirty(text);
    }

    private static void SetButtonText(GameObject buttonObject, string value)
    {
        TMP_Text text = buttonObject.GetComponentInChildren<TMP_Text>(true);

        if (text == null)
        {
            return;
        }

        text.text = value;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 28f;
        text.color = Color.white;
        EditorUtility.SetDirty(text);
    }

    private static void SaveReusablePrefabs(Scene scene)
    {
        EnsureFolder(PrefabRoot);
        EnsureFolder(PrefabRoot + "/Buttons");
        EnsureFolder(PrefabRoot + "/Panels");
        EnsureFolder(PrefabRoot + "/Popups");
        EnsureFolder(PrefabRoot + "/Wheel");

        SavePrefab(scene, "ui_button_spin", PrefabRoot + "/Buttons/ui_button_primary.prefab");
        SavePrefab(scene, "ui_button_leave", PrefabRoot + "/Buttons/ui_button_secondary.prefab");
        SavePrefab(scene, "ui_container_slice_00", PrefabRoot + "/Wheel/ui_wheel_slice.prefab");
        SavePrefab(scene, "ui_container_wheel", PrefabRoot + "/Wheel/ui_wheel.prefab");
        SavePrefab(scene, "ui_reward_item_template", PrefabRoot + "/Panels/ui_reward_item.prefab");
        SavePrefab(scene, "ui_result_card", PrefabRoot + "/Panels/ui_reward_result_card.prefab");
        SavePrefab(scene, "ui_container_zone_track", PrefabRoot + "/Panels/ui_zone_track.prefab");
        SavePrefab(scene, "ui_container_zone_badges", PrefabRoot + "/Panels/ui_zone_badges.prefab");
        SavePrefab(scene, "ui_panel_collected_rewards", PrefabRoot + "/Panels/ui_collected_rewards_panel.prefab");
        SavePrefab(scene, "ui_popup_bomb", PrefabRoot + "/Popups/ui_popup_bomb.prefab");
        SavePrefab(scene, "ui_popup_cashout", PrefabRoot + "/Popups/ui_popup_cashout.prefab");
    }

    private static void SavePrefab(Scene scene, string objectName, string path)
    {
        GameObject gameObject = FindRequired(scene, objectName);
        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, path, InteractionMode.AutomatedAction);
    }

    private static void SetWheelSlices(WheelView wheelView, Scene scene)
    {
        SerializedObject serializedObject = new SerializedObject(wheelView);
        SerializedProperty sliceViews = serializedObject.FindProperty("sliceViews");
        sliceViews.arraySize = 8;

        for (int i = 0; i < 8; i++)
        {
            GameObject sliceObject = FindRequired(scene, "ui_container_slice_" + i.ToString("00"));
            WheelSliceView sliceView = GetOrAdd<WheelSliceView>(sliceObject);
            SetObject(sliceView, "root", sliceObject);
            SetObject(sliceView, "iconImage", FindInChildrenRequired(sliceObject.transform, "ui_image_slice_icon_value").GetComponent<Image>());
            SetObject(sliceView, "amountText", FindInChildrenRequired(sliceObject.transform, "ui_text_slice_amount_value").GetComponent<TMP_Text>());
            sliceViews.GetArrayElementAtIndex(i).objectReferenceValue = sliceView;
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(wheelView);
    }

    private static void ConfigureRewardItemTemplate(RewardItemView rewardItemView, GameObject rewardTemplateObject)
    {
        ConfigureRect(
            rewardTemplateObject.GetComponent<RectTransform>(),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(230f, 58f),
            Vector2.zero);

        TMP_Text text = rewardTemplateObject.GetComponentInChildren<TMP_Text>(true);

        if (text == null)
        {
            GameObject textObject = new GameObject("ui_text_reward_amount_value", typeof(RectTransform));
            textObject.transform.SetParent(rewardTemplateObject.transform, false);
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            TextMeshProUGUI textMesh = textObject.AddComponent<TextMeshProUGUI>();
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.fontSize = 24f;
            textMesh.raycastTarget = false;
            text = textMesh;
        }

        ConfigureRect(
            text.GetComponent<RectTransform>(),
            new Vector2(0f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0.5f, 0.5f),
            Vector2.zero,
            Vector2.zero);
        text.GetComponent<RectTransform>().offsetMin = new Vector2(72f, 0f);
        text.GetComponent<RectTransform>().offsetMax = new Vector2(-12f, 0f);
        text.alignment = TextAlignmentOptions.MidlineLeft;
        text.fontSize = 22f;
        text.color = Color.white;
        text.raycastTarget = false;
        EditorUtility.SetDirty(text);

        Transform iconTransform = FindInChildren(rewardTemplateObject.transform, "ui_image_reward_icon_value");
        Image icon = iconTransform == null ? null : iconTransform.GetComponent<Image>();

        if (icon == null)
        {
            GameObject iconObject = new GameObject("ui_image_reward_icon_value", typeof(RectTransform));
            iconObject.transform.SetParent(rewardTemplateObject.transform, false);
            RectTransform rectTransform = iconObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);
            rectTransform.sizeDelta = new Vector2(48f, 48f);
            rectTransform.anchoredPosition = new Vector2(32f, 0f);
            icon = iconObject.AddComponent<Image>();
            icon.raycastTarget = false;
            icon.maskable = false;
        }

        ConfigureRect(
            icon.GetComponent<RectTransform>(),
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(48f, 48f),
            new Vector2(16f, 0f));
        icon.raycastTarget = false;
        icon.maskable = false;
        icon.preserveAspect = true;
        EditorUtility.SetDirty(icon);

        SetObject(rewardItemView, "amountText", text);
        SetObject(rewardItemView, "iconImage", icon);
        rewardTemplateObject.SetActive(false);
    }

    private static AutoButtonBinder ConfigureButton(GameObject buttonObject)
    {
        Button button = buttonObject.GetComponent<Button>();

        if (button != null)
        {
            while (button.onClick.GetPersistentEventCount() > 0)
            {
                UnityEventTools.RemovePersistentListener(button.onClick, 0);
            }

            EditorUtility.SetDirty(button);
        }

        AutoButtonBinder binder = GetOrAdd<AutoButtonBinder>(buttonObject);
        SetObject(binder, "button", button);
        return binder;
    }

    private static T LoadOrCreate<T>(string path) where T : ScriptableObject
    {
        T asset = AssetDatabase.LoadAssetAtPath<T>(path);

        if (asset != null)
        {
            return asset;
        }

        asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        return asset;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        string parent = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
        string folderName = System.IO.Path.GetFileName(path);
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, folderName);
    }

    private static T GetOrAdd<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        return component == null ? gameObject.AddComponent<T>() : component;
    }

    private static GameObject GetOrCreateChild(Transform parent, string objectName)
    {
        Transform existing = FindInChildren(parent, objectName);

        if (existing != null)
        {
            return existing.gameObject;
        }

        GameObject gameObject = new GameObject(objectName, typeof(RectTransform));
        gameObject.transform.SetParent(parent, false);
        return gameObject;
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

        property.objectReferenceValue = value;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
    }

    private static void SetObjectList(Object target, string propertyName, IList<Object> values)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty property = serializedObject.FindProperty(propertyName);

        if (property == null || !property.isArray)
        {
            Debug.LogWarning("Missing serialized list property " + propertyName + " on " + target.name);
            return;
        }

        property.arraySize = values.Count;

        for (int i = 0; i < values.Count; i++)
        {
            property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
    }

    private static GameObject FindRequired(Scene scene, string objectName)
    {
        GameObject result = Find(scene, objectName);

        if (result == null)
        {
            throw new System.InvalidOperationException("Missing scene object: " + objectName);
        }

        return result;
    }

    private static GameObject Find(Scene scene, string objectName)
    {
        GameObject[] roots = scene.GetRootGameObjects();

        for (int i = 0; i < roots.Length; i++)
        {
            Transform result = FindInChildren(roots[i].transform, objectName);

            if (result != null)
            {
                return result.gameObject;
            }
        }

        return null;
    }

    private static Transform FindInChildrenRequired(Transform root, string objectName)
    {
        Transform result = FindInChildren(root, objectName);

        if (result == null)
        {
            throw new System.InvalidOperationException("Missing child object: " + objectName + " under " + root.name);
        }

        return result;
    }

    private static Transform FindInChildren(Transform root, string objectName)
    {
        if (root.name == objectName)
        {
            return root;
        }

        for (int i = 0; i < root.childCount; i++)
        {
            Transform result = FindInChildren(root.GetChild(i), objectName);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private readonly struct DemoData
    {
        public DemoData(ZoneConfig zoneConfig, DemoSprites sprites, RewardDefinition bombReward)
        {
            ZoneConfig = zoneConfig;
            Sprites = sprites;
            BombReward = bombReward;
        }

        public ZoneConfig ZoneConfig { get; }
        public DemoSprites Sprites { get; }
        public RewardDefinition BombReward { get; }
    }

    private readonly struct CardStyleObjects
    {
        public CardStyleObjects(
            ZoneTrackView zoneTrackView,
            ZoneBadgeView zoneBadgeView,
            RewardCardView resultCardView,
            RewardCardView bombCardView)
        {
            ZoneTrackView = zoneTrackView;
            ZoneBadgeView = zoneBadgeView;
            ResultCardView = resultCardView;
            BombCardView = bombCardView;
        }

        public ZoneTrackView ZoneTrackView { get; }
        public ZoneBadgeView ZoneBadgeView { get; }
        public RewardCardView ResultCardView { get; }
        public RewardCardView BombCardView { get; }
    }

    private readonly struct DemoSprites
    {
        public DemoSprites(
            Sprite bronzeWheelBase,
            Sprite silverWheelBase,
            Sprite goldenWheelBase,
            Sprite bronzePointer,
            Sprite silverPointer,
            Sprite goldenPointer,
            Sprite cashIcon,
            Sprite goldIcon,
            Sprite goldChestIcon,
            Sprite bigChestIcon,
            Sprite superChestIcon,
            Sprite bombIcon,
            Sprite spinButton,
            Sprite secondaryButton,
            Sprite primaryButton,
            Sprite panelBackground,
            Sprite panelFrame,
            Sprite popupFrame)
        {
            BronzeWheelBase = bronzeWheelBase;
            SilverWheelBase = silverWheelBase;
            GoldenWheelBase = goldenWheelBase;
            BronzePointer = bronzePointer;
            SilverPointer = silverPointer;
            GoldenPointer = goldenPointer;
            CashIcon = cashIcon;
            GoldIcon = goldIcon;
            GoldChestIcon = goldChestIcon;
            BigChestIcon = bigChestIcon;
            SuperChestIcon = superChestIcon;
            BombIcon = bombIcon;
            SpinButton = spinButton;
            SecondaryButton = secondaryButton;
            PrimaryButton = primaryButton;
            PanelBackground = panelBackground;
            PanelFrame = panelFrame;
            PopupFrame = popupFrame;
        }

        public Sprite BronzeWheelBase { get; }
        public Sprite SilverWheelBase { get; }
        public Sprite GoldenWheelBase { get; }
        public Sprite BronzePointer { get; }
        public Sprite SilverPointer { get; }
        public Sprite GoldenPointer { get; }
        public Sprite CashIcon { get; }
        public Sprite GoldIcon { get; }
        public Sprite GoldChestIcon { get; }
        public Sprite BigChestIcon { get; }
        public Sprite SuperChestIcon { get; }
        public Sprite BombIcon { get; }
        public Sprite SpinButton { get; }
        public Sprite SecondaryButton { get; }
        public Sprite PrimaryButton { get; }
        public Sprite PanelBackground { get; }
        public Sprite PanelFrame { get; }
        public Sprite PopupFrame { get; }
    }
}
