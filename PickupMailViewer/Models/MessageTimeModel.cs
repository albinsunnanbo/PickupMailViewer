using System;

namespace PickupMailViewer.Models
{
    public class MessageTimeModel
    {
        public Lazy<MessageModel> Model { get; set; }

        public DateTime MessageTime { get; set; }
    }
}
