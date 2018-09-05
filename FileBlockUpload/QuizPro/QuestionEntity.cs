using System;
using System.Collections.Generic;

namespace FileBlockUpload.QuizPro
{
    public class QuestionEntity
    {
        public Guid Id { get; set; }

        public Guid VersionId { get; set; }

        public QuestionType Type { get; set; }

        public string Text { get; set; }

        public int Points { get; set; }

        public bool Deleted { get; set; }

        public bool CreatedDateUtc { get; set; }

        public bool CreatedById { get; set; }

        public IList<AnswerOptionEntity> AnswersOptions { get; set; }

        public IList<ActivityLog> Activities { get; set; }

        public IList<QuestionVersionEntity> OlderVersions { get; set; }

        public QuestionStats Stats { get; set; }
    }
}
