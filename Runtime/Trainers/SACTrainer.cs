using MLAgents.Configuration;
using MLAgents.Trainers;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Serialization;

namespace MLAgents.Configuration
{
    [YamlObjectUnion("!sac", typeof(SACTrainerHyperparameters))]
    public abstract partial class Hyperparameters { }
}

namespace MLAgents.Trainers
{
    [YamlObject]
    [System.Serializable]
    public partial class SACTrainerHyperparameters : Hyperparameters
    {
        public int BufferInitSteps = 1;
        public float InitEntcoef = 1.0f;
        public bool SaveReplayBuffer = false;
        public float Tau = 0.05f;
        public int StepsPerUpdate = 1;
        public int RewardSignalStepsPerUpdate = 1;

        public override void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context)
        {
            if (this is null)
            {
                emitter.WriteNull();
                return;
            }

            var formatter = StandardResolver.Instance.GetFormatterWithVerify<SACTrainerHyperparameters>();
            formatter.Serialize(ref emitter, this, context);
        }
    }

    [System.Serializable]
    public class SACTrainer : ITrainer
    {
        public SACTrainer() { }

        public string TrainerType => "sac";

        public SACTrainerHyperparameters SACHyperparameters = new SACTrainerHyperparameters();
        public Hyperparameters Hyperparameters => SACHyperparameters;
    }
}
