using System;

namespace FileBlockUpload.QuizPro
{
    public class ActivityLog
    {
        public Guid Id { get; set; }

        public Guid CreatedById { get; set; }

        public Guid CreatedByUser { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public ActivityEvent Event { get; set; }

        public Guid ObjectId { get; set; }

        public Guid ObjectName { get; set; }

        public string ObjectType { get; set; }

        /// <summary>
        /// Supporting data in JSON
        /// </summary>
        public string Data { get; set; }
    }
}
