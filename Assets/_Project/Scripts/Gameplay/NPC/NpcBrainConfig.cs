using System;
using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    [Serializable]
    public class NpcBrainConfig
    {
        [Range(0f, 1f)]
        [Tooltip("Probability that NPC decides to buy a toy (0 = never, 1 = always)")]
        public float BuyProbability = 0.7f;
    }
}