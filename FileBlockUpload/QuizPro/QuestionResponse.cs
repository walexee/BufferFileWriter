using System;

namespace FileBlockUpload.QuizPro
{
    public abstract class QuestionResponse
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        public Guid QuestionVersionId { get; set; }

        public DateTime SubmittedDateUtc { get; set; }

        public int DurationInMilliSeconds { get; set; }

        public int? Points { get; set; }
    }
}
