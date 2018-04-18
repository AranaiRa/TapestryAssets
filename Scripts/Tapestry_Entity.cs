using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_Entity : Tapestry_Actor {

    [Range(0, 1000)]
    public float stamina = 1000;
    public Tapestry_Inventory inventory;
    public Tapestry_AttributeProfile attributeProfile;
    public Tapestry_SkillProfile skillProfile;
    public Tapestry_EquipmentProfile equipmentProfile;
    public Tapestry_Prop pushTarget;
    public bool
        isRunning = false,
        isPushing = false,
        isLifting = false;
    public GameObject
        attachPoint;
    public int 
        carrySmall = 8,
        carryMedium = 3,
        carryLarge = 1;
    public GameObject
        holdContainerLeft,
        holdContainerRight;

    protected float speed;


    // Use this for initialization
    void Start () {
        if(effects == null)
            effects = new List<Tapestry_Effect>();
        if (ReferenceEquals(equipmentProfile, null))
            equipmentProfile = (Tapestry_EquipmentProfile)ScriptableObject.CreateInstance("Tapestry_EquipmentProfile");
    }
	
	// Update is called once per frame
	protected override void Update () {
        HandleEntityEffects();
        base.Update();
    }

    protected override void Reset()
    {
        if (ReferenceEquals(inventory, null))
            inventory = (Tapestry_Inventory)ScriptableObject.CreateInstance("Tapestry_Inventory");
        if (damageProfile == null)
            damageProfile = new Tapestry_DamageProfile();
        if(attributeProfile == null)
            attributeProfile = new Tapestry_AttributeProfile();
        if(skillProfile == null)
            skillProfile = new Tapestry_SkillProfile();
        if (keywords == null)
            keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");
        if (effects == null)
            effects = new List<Tapestry_Effect>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Points")
            {
                GameObject pointContainer = transform.GetChild(i).gameObject;
                for (int j = 0; j < pointContainer.transform.childCount; j++)
                {
                    if (transform.GetChild(i).name == "P_Attach")
                    {
                        attachPoint = pointContainer.transform.GetChild(j).gameObject;
                    }
                }
            }
        }

        base.Reset();
    }

    private void HandleEntityEffects()
    {

    }

    public override Tapestry_HealthState GetHealthState()
    {
        if (health > 500) return Tapestry_HealthState.Stable;
        else if (health > 200) return Tapestry_HealthState.Bloodied;
        else if (health > 0) return Tapestry_HealthState.Dying;
        else return Tapestry_HealthState.Dead;
    }

    public Tapestry_StaminaState GetStaminaState()
    {
        if (stamina > 500) return Tapestry_StaminaState.Fresh;
        else if (stamina > 200) return Tapestry_StaminaState.Winded;
        else if (stamina > 0) return Tapestry_StaminaState.Exhausted;
        else return Tapestry_StaminaState.Unconscious;
    }

    public virtual void Equip(Tapestry_ItemData item, Tapestry_EquipSlot slot)
    {
        if (ReferenceEquals(equipmentProfile, null))
            equipmentProfile = (Tapestry_EquipmentProfile)ScriptableObject.CreateInstance("Tapestry_EquipmentProfile");

        //foreach(Tapestry_ItemStack id in inventory.items)
        for (int i = inventory.items.Count - 1; i >= 0; i--)
        {
            if(inventory.items[i].item.Equals(item))
            {
                Debug.Log("Item exists");
                equipmentProfile.Equip(slot, item);
                inventory.RemoveItem(item, 1);
            }
        }
    }

    public virtual void Unequip(Tapestry_EquipSlot slot)
    {
        inventory.AddItem(equipmentProfile.GetInSlot(slot), 1);
        equipmentProfile.Equip(slot, null);
    }
}

public enum Tapestry_StaminaState
{
    Fresh, Winded, Exhausted, Unconscious
}