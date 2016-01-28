using System;
using WhoWhat.Domain.Notification.Data;

namespace WhoWhat.Api.Contract.Payload
{
    public class Notification
    {
        public string Id { get; set; }
        public string ProducerUserId { get; set; }
        public string TargetUserId { get; set; }
        public NotificationType Type { get; set; }

        //Question related
        public string QuestionId { get; set; }
        public string QuestionBody { get; set; }
        public string QuestionThumbnailUri { get; set; }

        /// <summary>
        /// Indicated value how the rating changed.
        /// </summary>
        /// <remarks> Can be nagavite.</remarks>
        public int RatingShift { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
