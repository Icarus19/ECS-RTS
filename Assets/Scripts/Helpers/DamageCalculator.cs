using Unity.Mathematics;

namespace Helpers
{
    public static class DamageCalculator
    {
        public static float ArmorScaling = 0.06f;

        public static float ArmorReduction(float armor)
        {
            return 1f - (ArmorScaling * armor) / (1f + ArmorScaling * math.abs(armor)); ;
        }
        public static float ApplyExponentialArmor(float damage, float armor, float decay = 0.05f)
        {
            return damage * math.exp(-decay * armor);
        }
    }
}