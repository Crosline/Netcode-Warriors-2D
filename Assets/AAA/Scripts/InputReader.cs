using Subsystems;
using UnityEngine;

namespace Game
{
    public sealed class InputReader : GameSubsystem<InputReader>
    {
        public override void OnAwake()
        {
            Debug.LogError("InputReader STATO");
        }
    }
}