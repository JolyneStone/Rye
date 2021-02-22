namespace Rye.Notify
{
    public class ServiceBrokerConfiguration
	{
		public ServiceBrokerConfiguration()
		{
			this.AutoScaleChannels = true;
			this.MaxAutoScaleChannels = 20;
			this.Channels = 1;
		}

		public bool AutoScaleChannels { get; set; }
		public int MaxAutoScaleChannels { get; set; }
		public int Channels { get; set; }
	}
}
