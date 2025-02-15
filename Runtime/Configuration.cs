using MLAgents.Utilities;
using System.Collections.Generic;
using UnityEngine;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Serialization;

namespace MLAgents.Configuration
{
    [YamlObject]
    [System.Serializable]
    public partial class EnvironmentSettings
    {
        public NullWhenEmptyString EnvPath = null;
        public NullWhenEmptyString EnvArgs = null;
        public int BasePort = 5005;
        public int NumEnvs = 1;
        public int TimeoutWait = 60;
        public int Seed = -1;
        public int MaxLifetimeRestarts = 10;
        public int RestartsRateLimitN = 1;
        public int RestartsRateLimitPeriodS = 60;
    }

    [YamlObject]
    [System.Serializable]
    public partial class CheckpointSettings
    {
        // public string RunId = string.Empty;
        public NullWhenEmptyString InitializeFrom = null;
        public bool LoadModel = false;
        public bool Resume = false;
        public bool Force = false;
        public bool TrainModel = false;
        public bool Inference = false;
    }

    [YamlObject]
    [System.Serializable]
    public partial class EngineConfiguration
    {
        public int Width = 84;
        public int Height = 84;
        public int QualityLevel = 5;
        public int TimeScale = 20;
        public int TargetFrameRate = -1;
        public int CaptureFrameRate = 60;
        public bool NoGraphics = false;
        public bool NoGraphicsMonitor = false;
    }

    [YamlObject]
    [System.Serializable]
    public partial class TorchConfiguration
    {
        // Could be enum, but there are possible arguments like cuda:0
        public NullWhenEmptyString Device = null;
    }


    [YamlObject]
    [System.Serializable]
    public partial class Config
    {
        public EnvironmentSettings EnvSettings;
        public EngineConfiguration EngineSettings;
        public CheckpointSettings CheckpointSettings;
        public TorchConfiguration TorchSettings;

        public EnvironmentParameters EnvironmentParameters;

        [HideInInspector]
        public List<Behavior> Behaviors = new List<Behavior>();
    }

    [DefaultConfigurationFormatter]
    public class ConfigurationFormatter : WriteOnlyYamlFormatter<Config>
    {
        public override void Serialize(ref Utf8YamlEmitter emitter, Config value, YamlSerializationContext context)
        {
            if (value is null)
            {
                emitter.WriteNull();
                return;
            }

            emitter.BeginMapping();
            {
                emitter.WriteString("behaviors");
                var formatter = context.Resolver.GetFormatterWithVerify<Behavior>();

                emitter.BeginMapping();
                {
                    foreach (var behavior in value.Behaviors)
                    {
                        emitter.WriteString(behavior.BehaviorId);
                        context.Serialize(ref emitter, behavior);
                    }
                }
                emitter.EndMapping();

                emitter.WriteString("env_settings");
                context.Serialize(ref emitter, value.EnvSettings);

                emitter.WriteString("engine_settings");
                context.Serialize(ref emitter, value.EngineSettings);

                emitter.WriteString("checkpoint_settings");
                context.Serialize(ref emitter, value.CheckpointSettings);

                emitter.WriteString("torch_settings");
                context.Serialize(ref emitter, value.TorchSettings);

                emitter.WriteString("environment_parameters");
                context.Serialize(ref emitter, value.EnvironmentParameters);
            }
            emitter.EndMapping();
        }
    }
}
