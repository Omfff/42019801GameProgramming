/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_HotkeyBar : MonoBehaviour {

    private Transform abilitySlotTemplate;
    private HotkeyAbilitySystem hotkeyAbilitySystem;
    public Transform specialPotion;
    public Transform shieldSkill;
    public Transform key;

    private void Awake() {
        abilitySlotTemplate = transform.Find("abilitySlotTemplate");
        abilitySlotTemplate.gameObject.SetActive(false);
    }

    public void SetHotkeyAbilitySystem(HotkeyAbilitySystem hotkeyAbilitySystem) {
        this.hotkeyAbilitySystem = hotkeyAbilitySystem;

        hotkeyAbilitySystem.OnAbilityListChanged += HotkeyAbilitySystem_OnAbilityListChanged;

        UpdateVisual();
    }

    private void HotkeyAbilitySystem_OnAbilityListChanged(object sender, System.EventArgs e) {
        UpdateVisual();
    }
    public void Update()
    {
        //for test
        if(Input.GetKeyDown(KeyCode.M))
        {
            hotkeyAbilitySystem.removeAbility();
            UpdateVisual();
        }
        if (Time.time < GameController.instance.lastShield + GameController.SheildCoolDown)
        {
            shieldSkill.GetComponent<Image>().fillAmount += 1 / (GameController.SheildCoolDown) * Time.deltaTime;
        }

    }
    private void UpdateVisual() {
        // Clear old objects
        foreach (Transform child in transform) {
            if (child == abilitySlotTemplate) continue; // Don't destroy Template
            Destroy(child.gameObject);
        }

        List<HotkeyAbilitySystem.HotkeyAbility> hotkeyAbilityList = hotkeyAbilitySystem.GetHotkeyAbilityList();
        for (int i = 0; i < hotkeyAbilityList.Count; i++) {
            HotkeyAbilitySystem.HotkeyAbility hotkeyAbility = hotkeyAbilityList[i];
            Transform abilitySlotTransform = Instantiate(abilitySlotTemplate, transform);
            abilitySlotTransform.gameObject.SetActive(true);
            RectTransform abilitySlotRectTransform = abilitySlotTransform.GetComponent<RectTransform>();
            abilitySlotRectTransform.anchoredPosition = new Vector2(100f * i, 0f);
            abilitySlotTransform.Find("itemImage").GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
            //abilitySlotTransform.Find("numberText").GetComponent<TMPro.TextMeshProUGUI>().SetText((i + 1).ToString());

            abilitySlotTransform.GetComponent<UI_HotkeyBarAbilitySlot>().Setup(hotkeyAbilitySystem, i, hotkeyAbility);
        }
        
        // Set up extras
        hotkeyAbilityList = hotkeyAbilitySystem.GetExtraHotkeyAbilityList();
        for (int i = 0; i < hotkeyAbilityList.Count; i++) {
            HotkeyAbilitySystem.HotkeyAbility hotkeyAbility = hotkeyAbilityList[i];
            Transform abilitySlotTransform = Instantiate(abilitySlotTemplate, transform);
            abilitySlotTransform.gameObject.SetActive(true);
            RectTransform abilitySlotRectTransform = abilitySlotTransform.GetComponent<RectTransform>();
            abilitySlotRectTransform.anchoredPosition = new Vector2(600f + 100f * i, 0f);
            switch (hotkeyAbility.abilityType)
            {
                case HotkeyAbilitySystem.AbilityType.SpecialPotion:
                    specialPotion = abilitySlotTransform.Find("itemImage");
                    specialPotion.GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
                    specialPotion.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    break;
                case HotkeyAbilitySystem.AbilityType.ShieldSkill:
                    shieldSkill = abilitySlotTransform.Find("itemImage");
                    shieldSkill.GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
                    shieldSkill.GetComponent<Image>().type = Image.Type.Filled;
                    shieldSkill.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
                    shieldSkill.GetComponent<Image>().fillOrigin = (int)Image.Origin360.Top;
                    shieldSkill.GetComponent<Image>().fillAmount = 0.0f;
                    break;
                case HotkeyAbilitySystem.AbilityType.Key:
                    key = abilitySlotTransform.Find("itemImage");
                    key.GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
                    key.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
                    break;
                default:
                    abilitySlotTransform.Find("itemImage").GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
                    break;
            }
            /*
            if (hotkeyAbility.abilityType == HotkeyAbilitySystem.AbilityType.SpecialPotion)
            {
                specialPotion = abilitySlotTransform.Find("itemImage");
                specialPotion.GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
                specialPotion.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }else if(hotkeyAbility.abilityType == HotkeyAbilitySystem.AbilityType.ShieldSkill)
            {
                shieldSkill = abilitySlotTransform.Find("itemImage");
                shieldSkill.GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
                shieldSkill.GetComponent<Image>().type = Image.Type.Filled;
                shieldSkill.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
                shieldSkill.GetComponent<Image>().fillOrigin = (int) Image.Origin360.Top;
                shieldSkill.GetComponent<Image>().fillAmount = 0.0f;
            }
            abilitySlotTransform.Find("itemImage").GetComponent<Image>().sprite = hotkeyAbility.GetSprite();
            */
            //abilitySlotTransform.Find("numberText").GetComponent<TMPro.TextMeshProUGUI>().SetText("");

            abilitySlotTransform.GetComponent<UI_HotkeyBarAbilitySlot>().Setup(hotkeyAbilitySystem, -1, hotkeyAbility);
        }
    }

    public void ItemChanged()
    {
        if (GameController.instance.storageItems.Count > 0)
        {
            Item item = GameController.instance.storageItems[GameController.CurrentItems];
            switch (item.name)
            {
                case "time":
                    specialPotion.GetComponent<Image>().sprite = Testing.Instance.timePotionSprite;
                    specialPotion.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
                case "stealth":
                    specialPotion.GetComponent<Image>().sprite = Testing.Instance.stealthPotionSprite;
                    specialPotion.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
                case "count":
                    specialPotion.GetComponent<Image>().sprite = Testing.Instance.CountSprite;
                    specialPotion.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
                case "bomb":
                    specialPotion.GetComponent<Image>().sprite = Testing.Instance.BombSprite;
                    specialPotion.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
            }
        }
        else
        {
            specialPotion.GetComponent<Image>().sprite = null;
            specialPotion.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    public void KeyStateChange(bool isHoldingKey)
    {
        if (isHoldingKey)
        {
            key.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            key.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
        }
    }

    public void ShieldCooldown()
    {
        shieldSkill.GetComponent<Image>().fillAmount = 0.0f;
    }
}
