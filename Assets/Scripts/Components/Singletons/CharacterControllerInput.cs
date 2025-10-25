using Unity.Entities;

public struct CharacterControllerInput : IComponentData
{
    public bool Ability0Pressed;
    public bool Ability0Held;
    public bool Ability0Canceled; //Canceled is unused for now
    
    public bool Ability1Pressed;
    public bool Ability1Held;
    public bool Ability1Canceled;
    
    public bool Ability2Pressed;
    public bool Ability2Held;
    public bool Ability2Canceled;
    
    public bool Ability3Pressed;
    public bool Ability3Held;
    public bool Ability3Canceled;
    
    public bool Ability4Pressed;
    public bool Ability4Held;
    public bool Ability4Canceled;
}