using System;
using System.Collections.Generic;

namespace FileBlockUpload.QuizPro
{
    public class QuestionVersionEntity
    {
        public Guid VersionId { get; set; }

        public Guid Text { get; set; }

        public IList<Guid> AnswerOptionIds { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
