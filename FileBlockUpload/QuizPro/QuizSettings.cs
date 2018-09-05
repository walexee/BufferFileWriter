namespace FileBlockUpload.QuizPro
{
    public class QuizSettings
    {
        public QuizType QuizType { get; }

        /// <summary>
        /// Number of questions a quiz take will attempt
        /// </summary>
        public int QuestionsCount { get; }

        public int DurationInMinutes { get; } // TODO: change to timespan

        public int? PointsPerQuestion { get; }
    }
}
