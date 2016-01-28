namespace WhoWhat.Domain.User
{
    public static class UserScoreTable
    {
        public const int QuestionVotedUp = 5;
        public const int QuestionVotedDown = -2;

        public const int AnswerVotedUp = 10;
        public const int AnswerVotedDown = -2;

        public const int QuestionResolved = 2;
        public const int AnswerAccepted = 15;
    }
}
