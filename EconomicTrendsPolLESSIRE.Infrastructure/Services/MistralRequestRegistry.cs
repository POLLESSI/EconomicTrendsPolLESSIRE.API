using EconomicTrendsPolLESSIRE.Application.Interfaces;
using System.Collections.Concurrent;

namespace EconomicTrendsPolLESSIRE.Infrastructure.Services
{
    public sealed class MistralRequestRegistry
        : IMistralRequestRegistry
    {
        private sealed record Registration(
            string RequestId,
            CancellationTokenSource Cts);

        private readonly ConcurrentDictionary<int, Registration>
            _registrations = new();

        public string Register(
            int interactionId,
            CancellationTokenSource cts)
        {
            if (interactionId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(interactionId));

            ArgumentNullException.ThrowIfNull(cts);

            var requestId =
                Guid.NewGuid().ToString("N");

            var registration =
                new Registration(
                    requestId,
                    cts);

            if (!_registrations.TryAdd(
                    interactionId,
                    registration))
            {
                throw new InvalidOperationException(
                    $"Interaction {interactionId} is already registered.");
            }

            return requestId;
        }

        public bool TryGet(
            int interactionId,
            out string requestId,
            out CancellationTokenSource? cts)
        {
            if (_registrations.TryGetValue(
                    interactionId,
                    out var registration))
            {
                requestId =
                    registration.RequestId;

                cts =
                    registration.Cts;

                return true;
            }

            requestId = string.Empty;
            cts = null;

            return false;
        }

        public bool TryCancel(
            int interactionId,
            string? requestId = null)
        {
            if (!_registrations.TryGetValue(
                    interactionId,
                    out var registration))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(requestId) &&
                !string.Equals(
                    registration.RequestId,
                    requestId,
                    StringComparison.Ordinal))
            {
                return false;
            }

            try
            {
                registration.Cts.Cancel();
                return true;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        public void Remove(
            int interactionId,
            string? requestId = null)
        {
            if (!_registrations.TryGetValue(
                    interactionId,
                    out var registration))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(requestId) &&
                !string.Equals(
                    registration.RequestId,
                    requestId,
                    StringComparison.Ordinal))
            {
                return;
            }

            if (_registrations.TryRemove(
                    interactionId,
                    out var removed))
            {
                removed.Cts.Dispose();
            }
        }
    }
}



































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.