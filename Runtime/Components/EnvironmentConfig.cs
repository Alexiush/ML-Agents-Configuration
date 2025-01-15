using UnityEngine;

namespace MLAgents.Configuration
{
    public class EnvironmentConfig : MonoBehaviour
    {
        [field: SerializeField]
        public Config Configuration { get; private set; }
    }
}