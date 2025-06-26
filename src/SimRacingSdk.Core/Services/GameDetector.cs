using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Abstractions;

namespace SimRacingSdk.Core.Services
{
    public abstract class GameDetector : IGameDetector
    {
        private readonly Subject<bool> isRunning = new();
        private bool isGameRunning = false;
        private IDisposable? updateSubscription;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IObservable<bool> Start(double updateIntervalMs = 1000)
        {
            this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                                .Subscribe(this.OnNextInterval);

            return this.isRunning.AsObservable();
        }

        protected abstract bool IsGameRunning();

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing)
            {
                return;
            }

            this.isRunning?.Dispose();
            this.updateSubscription?.Dispose();
        }

        private void OnNextInterval(long _)
        {
            var processIsRunning = this.IsGameRunning();
            if(processIsRunning == this.isGameRunning)
            {
                return;
            }

            this.isGameRunning = processIsRunning;
            this.isRunning.OnNext(this.isGameRunning);
        }
    }
}