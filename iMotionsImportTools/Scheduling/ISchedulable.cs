namespace iMotionsImportTools.Scheduling
{
    public interface ISchedulable
    {
        bool IsScheduled { get; set; }

        void OnScheduledEvent(object sender, SchedulerEventArgs args);

    }
}