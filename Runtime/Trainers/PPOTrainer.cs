using MLAgents.Configuration;
using MLAgents.Trainers;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Serialization;

namespace MLAgents.Configuration
{
    [YamlObjectUnion("!ppo", typeof(PPOTrainerHyperparameters))]
    public abstract partial class Hyperparameters { }
}

namespace MLAgents.Trainers
{
    [YamlObject]
    [System.Serializable]
    public partial class PPOTrainerHyperparameters : Hyperparameters
    {
        public float Beta = 5e-3f;
        public float Epsilon = 0.2f;
        public LearningRateSchedule BetaSchedule = LearningRateSchedule.Linear;
        public LearningRateSchedule EpsilonSchedule = LearningRateSchedule.Linear;
        public float Lambd = 0.95f;
        public int NumEpoch = 3;
        public bool SharedCritic = false;

        public override void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context)
        {
            if (this is null)
            {
                emitter.WriteNull();
                return;
            }

            var formatter = StandardResolver.Instance.GetFormatterWithVerify<PPOTrainerHyperparameters>();
            formatter.Serialize(ref emitter, this, context);
        }
    }

    [System.Serializable]
    public class PPOTrainer : ITrainer
    {
        public PPOTrainer() { }

        public string TrainerType => "ppo";

        public PPOTrainerHyperparameters PPOHyperparameters = new PPOTrainerHyperparameters();
        public Hyperparameters Hyperparameters => PPOHyperparameters;
    }
}
