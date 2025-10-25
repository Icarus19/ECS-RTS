using Unity.Entities;

public struct Selectable :
IComponentData, IEnableableComponent
{

}

public struct PrimarySelection : IComponentData
{
    public Entity Value;
}

public struct GroupIdComponent : IComponentData
{
    public GroupId Value;
}

public enum GroupId
{
    Player,
    Enemy,
    Neutral
}