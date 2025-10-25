using Unity.Entities;

public struct ConfigComponent : IComponentData
{

}

public struct UIStatBarVisibility : IComponentData
{
    public bool ShowHealth;
    public bool ShowMana;
    public bool ShowShield;
    public bool ShowStamina;
}