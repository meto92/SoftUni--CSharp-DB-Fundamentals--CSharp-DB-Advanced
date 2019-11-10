using System;

namespace Stations.DTOs.Export
{
    public class DelayedTrainDto
    {
        public string TrainNumber { get; set; }

        public int DelayedTimes { get; set; }

        public TimeSpan MaxDelayedTime { get; set; }
    }
}