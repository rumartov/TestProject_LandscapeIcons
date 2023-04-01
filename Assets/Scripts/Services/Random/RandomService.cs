using UnityEngine;

namespace Services
{
    internal class RandomService : IRandomService
    {
        public float Range(float minInclusive, float maxInclusive)
        {
            return Random.Range(minInclusive, maxInclusive);
        }
    }
}