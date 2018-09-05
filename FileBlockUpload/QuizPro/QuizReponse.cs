using System;
using System.Collections.Generic;

namespace FileBlockUpload.QuizPro
{
    public class QuizReponse
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public DateTime StartedDateUtc { get; }

        public DateTime CompletedDateUtc { get; }

        public IReadOnlyList<QuestionResponse> QuestionResponses { get; }

        public int TotalScore { get; }

        public int? Score { get; }
    }
}
