using System.Collections.Generic;

namespace FileBlockUpload.QuizPro
{
    public class MultipleChoiceQuestion : Question
    {
        public IReadOnlyList<AnswerOption> Options { get; }

        public IReadOnlyList<Question> OlderVersions { get; }
    }
}
