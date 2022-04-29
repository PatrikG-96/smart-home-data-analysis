using System;
using iMotionsImportTools.Scheduling;

namespace tests.Mocks
{
    public class MockScheduler: IScheduler
    {
        public event EventHandler<SchedulerEventArgs> Events;

        private int _repetitions;

        public MockScheduler(int repetitions)
        {
            _repetitions = repetitions;
        }
        public void Start()
        {
            while (_repetitions > 0)
            {
                var handler = Events;
                handler?.Invoke(this, null);

                _repetitions--;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}