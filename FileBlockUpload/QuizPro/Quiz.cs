using System.Collections.Generic;
using System.Linq;

namespace FileBlockUpload.QuizPro
{
    // TODO: consider creating another class QuizDetails to manage some of the properties
    public abstract class Quiz
    {
        // TODO: may not need to put this here. Def needed to create the instance
        public QuizSettings Settings { get; }

        public QuizStats Stats { get; }

        public int TotalPoints => Questions?.Sum(x => x.Points) ?? 0;  

        public IReadOnlyList<Question> Questions { get; }

        public IReadOnlyList<ActivityLog> Activities { get; }
    }
}
