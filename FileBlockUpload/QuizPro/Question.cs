using System;
using System.Collections.Generic;

namespace FileBlockUpload.QuizPro
{
    public abstract class Question
    {
        public Guid Id { get; }

        public QuestionType Type { get; }

        public string Text { get; }

        public int Points { get; } 

        public bool Deleted { get; }

        public bool CreatedDateUtc { get; }

        public bool CreatedById { get; }

        public IReadOnlyList<ActivityLog> Activities { get; }

        public QuestionStats Stats { get; set; }
    }
}
