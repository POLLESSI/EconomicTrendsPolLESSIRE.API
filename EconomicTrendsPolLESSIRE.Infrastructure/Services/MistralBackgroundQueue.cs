using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.Mistral;
using System.Threading.Channels;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class MistralBackgroundQueue : IMistralBackgroundQueue
    {
        private readonly Channel<MistralWorkItem> _queue;

        public MistralBackgroundQueue() : this(capacity: 16)
        {
        }

        public MistralBackgroundQueue(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            var options =
                new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,

                    SingleReader = true,

                    SingleWriter = false,

                    AllowSynchronousContinuations = false
                };

            _queue = Channel.CreateBounded<MistralWorkItem>(options);
        }

        public ValueTask QueueAsync(MistralWorkItem workItem, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(workItem);

            return _queue.Writer.WriteAsync(workItem, cancellationToken);
        }

        public ValueTask<MistralWorkItem> DequeueAsync(CancellationToken cancellationToken = default)
        {
            return _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.