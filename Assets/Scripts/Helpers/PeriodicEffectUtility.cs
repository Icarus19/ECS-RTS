namespace Helpers
{
    public static class PeriodicEffectUtility
    {
        public static bool UpdateTick(ref float timer, float interval, float deltaTime)
        {
            timer -= deltaTime;

            if (timer <= 0f)
            {
                timer += interval; // reset timer (carry over)
                return true;               // it's time to tick!
            }

            return false;
        }
    }
}