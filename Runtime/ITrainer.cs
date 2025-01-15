namespace MLAgents.Configuration
{
    public interface ITrainer
    {
        public string TrainerType { get; }
        public Hyperparameters Hyperparameters { get; }
    }
}
