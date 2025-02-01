using UnityEngine;

namespace Subsystems.Core
{
    [CreateAssetMenu(fileName = "SubsystemManagerSettings", menuName = "AAA/SubsystemManagerSettings")]
    public class SubsystemManagerSettings : ScriptableObject
    {
        [field: SerializeField] public GameSubsystem[] Subsystems { get; private set; }
    }
}