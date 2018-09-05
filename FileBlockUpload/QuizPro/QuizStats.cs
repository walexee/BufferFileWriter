using System;

namespace FileBlockUpload.QuizPro
{
    public class QuizStats
    {
        public int TotalAttempts { get; set; }

        public int TotalUsersCount { get; set; }

        public double AverageScore { get; set; }

        public double MedianScore { get; set; }

        public double HighestScore { get; set; }

        public double LowestScore { get; set; }

        public double ScoreStandardDeviation { get; set; }

        public double ScoreVariance { get; set; }

        public int MaxAttemptsBySingleUser { get; set; }

        public int AverageDurationInMinutes { get; set; }

        public DateTime LastAttemptDateUtc { get; set; }
    }
}
