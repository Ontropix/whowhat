namespace WhoWhat.View.Payload
{
    public class VotePayload
    {
        public VotePayload(VoteDirection direction, int ratingShift)
        {
            this.Direction = direction;
            this.RatingShift = ratingShift;
        }

        public VoteDirection Direction { get; private set; }
        public int RatingShift { get; private set; }
    }

    public enum VoteDirection
    {
        Up = 'U',
        Down = 'D'
    }
}