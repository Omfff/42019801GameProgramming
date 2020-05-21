/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyAbilitySystem {

    public event EventHandler OnAbilityListChanged;

    public enum AbilityType {
        Pistol,
        Shotgun,
        Sword,
        Punch,
        HealthPotion,
        ManaPotion,
        SpecialPotion,
        ShieldSkill,
    }

    private PlayerSwapWeapons player;
    private List<HotkeyAbility> hotkeyAbilityList;
    private List<HotkeyAbility> extraHotkeyAbilityList;
    public void removeAbility()
    {
        hotkeyAbilityList.RemoveAt(2);
    }
    public HotkeyAbilitySystem(PlayerSwapWeapons player) {
        this.player = player;
        hotkeyAbilityList = new List<HotkeyAbility>();
        extraHotkeyAbilityList = new List<HotkeyAbility>();
        
        // Health Potion
        hotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.HealthPotion, 
            activateAbilityAction = () => player.ConsumeHealthPotion()
        });
        // Pistol
        hotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.Pistol, 
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Pistol) 
        });
        // Sword
        hotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.Sword, 
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Sword) 
        });
        // Shotgun
        hotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.Shotgun, 
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Shotgun) 
        });
        // Punch
        hotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.Punch, 
            activateAbilityAction = () => player.SetWeaponType(PlayerSwapWeapons.WeaponType.Punch) 
        });


        // Mana Potion
        extraHotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.ManaPotion, 
            activateAbilityAction = () => player.ConsumeManaPotion()
        });
        // Storage Items
        extraHotkeyAbilityList.Add(new HotkeyAbility
        {
            abilityType = AbilityType.SpecialPotion,
            activateAbilityAction = () => GameController.UseItems()
        });
        // Shield Skill
        extraHotkeyAbilityList.Add(new HotkeyAbility
        {
            abilityType = AbilityType.ShieldSkill,
            activateAbilityAction = () => GameController.AddShield()
        });

    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            hotkeyAbilityList[0].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            hotkeyAbilityList[1].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            hotkeyAbilityList[2].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            hotkeyAbilityList[3].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            hotkeyAbilityList[4].activateAbilityAction();
        }
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    removeAbility();
        //}
    }
    
    public List<HotkeyAbility> GetHotkeyAbilityList() {
        return hotkeyAbilityList;
    }
    
    public List<HotkeyAbility> GetExtraHotkeyAbilityList() {
        return extraHotkeyAbilityList;
    }

    public void SwapAbility(int abilityIndexA, int abilityIndexB) {
        HotkeyAbility hotkeyAbility = hotkeyAbilityList[abilityIndexA];
        hotkeyAbilityList[abilityIndexA] = hotkeyAbilityList[abilityIndexB];
        hotkeyAbilityList[abilityIndexB] = hotkeyAbility;
        OnAbilityListChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void SwapAbility(HotkeyAbility hotkeyAbilityA, HotkeyAbility hotkeyAbilityB) {
        if (extraHotkeyAbilityList.Contains(hotkeyAbilityA)) {
            // A is on Extra List
            int indexB = hotkeyAbilityList.IndexOf(hotkeyAbilityB);
            hotkeyAbilityList[indexB] = hotkeyAbilityA;

            extraHotkeyAbilityList.Remove(hotkeyAbilityA);
            extraHotkeyAbilityList.Add(hotkeyAbilityB);
        } else {
            if (extraHotkeyAbilityList.Contains(hotkeyAbilityB)) {
                // B is on the Extra List
                int indexA = hotkeyAbilityList.IndexOf(hotkeyAbilityA);
                hotkeyAbilityList[indexA] = hotkeyAbilityB;

                extraHotkeyAbilityList.Remove(hotkeyAbilityB);
                extraHotkeyAbilityList.Add(hotkeyAbilityA);
            } else {
                // Neither are on the Extra List
                int indexA = hotkeyAbilityList.IndexOf(hotkeyAbilityA);
                int indexB = hotkeyAbilityList.IndexOf(hotkeyAbilityB);
                HotkeyAbility tmp = hotkeyAbilityList[indexA];
                hotkeyAbilityList[indexA] = hotkeyAbilityList[indexB];
                hotkeyAbilityList[indexB] = tmp;
            }
        }

        OnAbilityListChanged?.Invoke(this, EventArgs.Empty);
    }

    /*
     * Represents a single Hotkey Ability of any Type
     * */
    public class HotkeyAbility {
        public AbilityType abilityType;
        public Action activateAbilityAction;

        public Sprite GetSprite(String name="") {
            switch (abilityType) {
                default:
                case AbilityType.Pistol:        
                    return Testing.Instance.pistolSprite;
                case AbilityType.Shotgun:       
                    return Testing.Instance.shotgunSprite;
                case AbilityType.Sword:         
                    return Testing.Instance.swordSprite;
                case AbilityType.Punch:         
                    return Testing.Instance.punchSprite;
                case AbilityType.HealthPotion:  
                    return Testing.Instance.healthPotionSprite;
                case AbilityType.ManaPotion:    
                    return Testing.Instance.manaPotionSprite;
                case AbilityType.SpecialPotion:
                    if (name == "time")
                    {
                        return Testing.Instance.timePotionSprite;
                    }
                    else if(name == "stealth")
                    {
                        return Testing.Instance.stealthPotionSprite;
                    }
                    else
                    {
                        return null;
                    }
                case AbilityType.ShieldSkill:
                    return Testing.Instance.shieldSkillSprite;
            }
        }

        public override string ToString() {
            return abilityType.ToString();
        }
    }

}
