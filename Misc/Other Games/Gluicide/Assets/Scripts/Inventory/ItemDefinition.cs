using UnityEngine;

[CreateAssetMenu(menuName = "Item Pickup")]
public class ItemDefinition : ScriptableObject
{
    public StatSystem.StatType item;
    public int amount;

    public bool grantsGlueGun;
    public bool grantsGravityGun;
    public bool grantsWallet;

    public int healthGranted;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif 
}

// SOURCE Professor Funky Feeney's Repo