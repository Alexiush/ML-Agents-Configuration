using MLAgents.Configuration;
using MLAgents.Trainers;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Serialization;

namespace MLAgents.Configuration
{
    [YamlObjectUnion("!poca", typeof(POCATrainerHyperparameters))]
    public abstract partial class Hyperparameters { }
}

namespace MLAgents.Trainers
{
    [YamlObject]
    [System.Serializable]
    public partial class POCATrainerHyperparameters : Hyperparameters
    {
        public float Beta = 5e-3f;
        public float Epsilon = 0.2f;
        public LearningRateSchedule BetaSchedule = LearningRateSchedule.Linear;
        public LearningRateSchedule EpsilonSchedule = LearningRateSchedule.Linear;
        public float Lambd = 0.95f;
        public int NumEpoch = 3;

        public override void Serialize(ref Utf8YamlEmitter emitter, YamlSerializationContext context)
        {
            if (this is null)
            {
                emitter.WriteNull();
                return;
            }

            var formatter = StandardResolver.Instance.GetFormatterWithVerify<POCATrainerHyperparameters>();
            formatter.Serialize(ref emitter, this, context);
        }
    }

    [System.Serializable]
    public class POCATrainer : ITrainer
    {
        public POCATrainer() { }

        public string TrainerType => "poca";

        public POCATrainerHyperparameters POCAHyperparameters = new POCATrainerHyperparameters();
        public Hyperparameters Hyperparameters => POCAHyperparameters;
    }
}
