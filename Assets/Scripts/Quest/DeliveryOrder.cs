namespace LanternCourier.Quest
{
    public sealed class DeliveryOrder
    {
        public DeliveryOrder(int id, string origin, string destination, int reward)
        {
            Id = id;
            Origin = origin;
            Destination = destination;
            Reward = reward;
        }

        public int Id { get; }
        public string Origin { get; }
        public string Destination { get; }
        public int Reward { get; }
    }
}
