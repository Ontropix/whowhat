using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using WhoWhat.UI.WindowsPhone.Core;
using WhoWhat.UI.WindowsPhone.Infrastructure;
using WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;
using WhoWhat.UI.WindowsPhone.Services.Model;

namespace WhoWhat.UI.WindowsPhone.ViewModels.Question
{
    public class ViewQuestionViewModel : TaskViewModel
    {
        private readonly INavigationService navigationService;
        private readonly AppSettings appSettings;
        private readonly QuestionRestService questionRestService;
        private readonly ToastService toastService;
        private readonly AnswerRestService answerRestService;

        //From navigation
        public string QuestionId { get; set; }

        //From navigation
        public bool FocusAnswer { get; set; }

        /// <summary>
        /// Indicates that answer of the current user should be highlighted
        /// </summary>
        public bool HighlightAnswer { get; set; }

        public ViewQuestionViewModel(
            INavigationService navigationService,
            AppSettings appSettings,
            QuestionRestService questionRestService,
            ToastService toastService,
            AnswerRestService answerRestService)
        {
            this.navigationService = navigationService;
            this.appSettings = appSettings;
            this.questionRestService = questionRestService;
            this.toastService = toastService;
            this.answerRestService = answerRestService;


            Question = new Services.Model.Question()
            {
                Author = new Author()
            };

            Answers = new OrderedObservableCollection<Answer>((x, y) => y.CreatedAt.CompareTo(x.CreatedAt));
            Votes = new Dictionary<string, VoteDirection>();
        }

        public bool IsQuestionExist
        {
            get { return isQuestionExist; }
            set
            {
                isQuestionExist = value;
                NotifyOfPropertyChange(() => IsQuestionExist);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            IsQuestionExist = true;
            IsAuthenticated = appSettings.IsAuthenticated;
        }

        protected async override void OnActivate()
        {
            base.OnActivate();

            await RunTaskAsync(async () =>
            {
                await PopulateQuestion();

                //Scroll to user answer
                if (FocusAnswer)
                {
                    Answer ownAnswer = Answers.FirstOrDefault(answer => answer.Author.UserId == appSettings.UserId);
                    SelectedAnswer = ownAnswer;
                    FocusAnswer = false;
                }
            },
            (ex) =>
            {
                RestException restException = ex as RestException;
                if (restException != null && restException.StatusCode == HttpStatusCode.NotFound)
                {
                    IsQuestionExist = false;
                }
                else
                {
                    IoC.Get<ToastService>().ShowError(AppResources.Message_ServerError);
                }
            });
        }

        private async Task PopulateQuestion()
        {
            QuestionDetailsResponse response = await questionRestService.Details(QuestionId);


            //Update question
            Question = response.Question;

            IsCurrentUserAuthor = IsAuthenticated && Question.Author.UserId == appSettings.UserId;

            //The user can only edit own question within 30 minutes
            CanQuestionBeEdited = (response.ServerTimeUtc - Question.CreatedAt) < TimeSpan.FromMinutes(30);

            MergeAnswers(response.Answers);
            Votes = response.Votes;

            IsQuestionEdit = false;

            Question.IsVotedUp = response.Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Up);
            Question.IsVotedDown = response.Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Down);

            //Copy body to EditedBody
            foreach (Answer answer in Answers)
            {
                answer.EditedBody = answer.Body;
                answer.IsCurrentUserAuthor = IsAuthenticated && answer.Author.UserId == appSettings.UserId;
                answer.IsVotedUp = answer.Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Up);
                answer.IsVotedDown = answer.Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Down);
                answer.IsEdit = false;
                answer.CanBeAccepted = IsCurrentUserAuthor && answer.Author.UserId != appSettings.UserId;

                //The user can only edit own answer within 30 minutes.
                answer.CanBeEdited = IsAuthenticated && (response.ServerTimeUtc - answer.CreatedAt) < TimeSpan.FromMinutes(30);
            }
        }

        private void MergeAnswers(IList<Answer> remote)
        {
            List<string> localIds = Answers.Select(answer => answer.AnswerId).ToList();
            List<string> remoteIds = remote.Select(answer => answer.AnswerId).ToList();

            //Get new answers
            List<Answer> @new = remote.Where(answer => !localIds.Contains(answer.AnswerId)).ToList();
            List<Answer> @removed = Answers.Where(answer => !remoteIds.Contains(answer.AnswerId)).ToList();

            //Add
            foreach (Answer answer in @new)
            {
                Answers.Add(answer);
            }

            //Remove
            foreach (Answer answer in @removed)
            {
                Answers.Remove(answer);
            }

            //Update
            foreach (Answer localAnswer in Answers)
            {
                Answer remoteAnswer = remote.FirstOrDefault(answer => answer.AnswerId == localAnswer.AnswerId);
                if (remoteAnswer != null)
                {
                    UpdateAnswer(localAnswer, remoteAnswer);
                }
            }
        }

        private static void UpdateAnswer(Answer localAnswer, Answer remoteAnswer)
        {
            localAnswer.Author = remoteAnswer.Author;
            localAnswer.Body = remoteAnswer.Body;
            localAnswer.CreatedAt = remoteAnswer.CreatedAt;
            localAnswer.EditedAt = remoteAnswer.EditedAt;
            localAnswer.EditedBody = remoteAnswer.EditedBody;
            localAnswer.IsAccepted = remoteAnswer.IsAccepted;
            localAnswer.IsEdit = false;
            localAnswer.QuestionId = remoteAnswer.QuestionId;
            localAnswer.Rating = remoteAnswer.Rating;
            localAnswer.Votes = remoteAnswer.Votes;
        }

        private Services.Model.Question question;
        public Services.Model.Question Question
        {
            get { return question; }
            set
            {
                if (Equals(value, question)) return;
                question = value;
                NotifyOfPropertyChange(() => Question);
            }
        }

        public Dictionary<string, VoteDirection> Votes { get; set; }

        private bool isCurrentUserAuthor;
        public bool IsCurrentUserAuthor
        {
            get { return isCurrentUserAuthor; }
            set
            {
                if (value.Equals(isCurrentUserAuthor)) return;
                isCurrentUserAuthor = value;
                NotifyOfPropertyChange(() => IsCurrentUserAuthor);
            }
        }

        private bool canQuestionBeEdited;
        public bool CanQuestionBeEdited
        {
            get { return canQuestionBeEdited; }
            set
            {
                canQuestionBeEdited = value;
                NotifyOfPropertyChange(() => CanQuestionBeEdited);
            }
        }

        private OrderedObservableCollection<Answer> answers;
        public OrderedObservableCollection<Answer> Answers
        {
            get { return answers; }
            set
            {
                if (Equals(value, answers)) return;
                answers = value;
                NotifyOfPropertyChange(() => Answers);
            }
        }

        private Answer selectedAnswer;
        public Answer SelectedAnswer
        {
            get { return selectedAnswer; }
            set
            {
                selectedAnswer = value;
                NotifyOfPropertyChange(() => SelectedAnswer);
            }
        }

        #region Validation
        private bool Validate()
        {
            if (string.IsNullOrEmpty(Body))
            {
                toastService.ShowWarning(
                    AppResources.Validation_QuestionBodyRequired
                );

                return false;
            }

            Body = Body.Trim();

            if (Body.Length < appSettings.MinQuestionLength)
            {
                toastService.ShowWarning(string.Format(
                    AppResources.Validation_BodyValidationFrmt,
                    appSettings.MinQuestionLength,
                    Body.Length)
                );
                return false;
            }

            if (Tags.Count == 0)
            {
                toastService.ShowWarning(AppResources.Validation_TagRequired);
                return false;
            }

            return true;
        }

        #endregion

        #region Actions

        public void Answer()
        {
            if (!appSettings.IsAuthenticated)
            {
                navigationService.UriFor<Login.LoginViewModel>().Navigate();
                return;
            }

            navigationService.UriFor<AnswerQuestionViewModel>()
                .WidthData(x => x.Question, Question).Navigate();
        }

        public async void QuestionDown()
        {
            if (!appSettings.IsAuthenticated)
            {
                navigationService.UriFor<Login.LoginViewModel>().Navigate();
                return;
            }

            //Author can not vote for own question
            if (IsCurrentUserAuthor) return;

            await RunTaskAsync(async () =>
            {
                bool alreadyVotedDown = Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Down);

                if (alreadyVotedDown)
                {
                    await questionRestService.Unvote(Question.QuestionId);
                }
                else
                {
                    await questionRestService.VoteDown(Question.QuestionId);
                }

                await PopulateQuestion();
            }, busy => Question.IsVotingBusy = busy);

        }

        public async void QuestionUp()
        {
            if (!appSettings.IsAuthenticated)
            {
                navigationService.UriFor<Login.LoginViewModel>().Navigate();
                return;
            }

            //Author can not vote for own question
            if (IsCurrentUserAuthor) return;

            await RunTaskAsync(async () =>
            {
                bool alreadyVoteUp = Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Up);

                if (alreadyVoteUp)
                {
                    await questionRestService.Unvote(Question.QuestionId);
                }
                else
                {
                    await questionRestService.VoteUp(Question.QuestionId);
                }

                await PopulateQuestion();
            }, busy => Question.IsVotingBusy = busy);

        }

        public async void AnswerUp(Answer answer)
        {
            if (!appSettings.IsAuthenticated)
            {
                navigationService.UriFor<Login.LoginViewModel>().Navigate();
                return;
            }

            await RunTaskAsync(async () =>
            {
                bool alreadyVotedUp = answer.Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Up);

                if (alreadyVotedUp)
                {
                    await answerRestService.Unvote(answer.AnswerId);
                }
                else
                {
                    await answerRestService.VoteUp(answer.AnswerId);
                }

                await PopulateQuestion();
            }, busy => answer.IsVotingBusy = busy);

        }

        public async void AnswerDown(Answer answer)
        {
            if (!appSettings.IsAuthenticated)
            {
                navigationService.UriFor<Login.LoginViewModel>().Navigate();
                return;
            }

            await RunTaskAsync(async () =>
            {
                bool alreadyVotedDown = answer.Votes.Any(x => x.Key == appSettings.UserId && x.Value == VoteDirection.Down);

                if (alreadyVotedDown)
                {
                    await answerRestService.Unvote(answer.AnswerId);
                }
                else
                {
                    await answerRestService.VoteDown(answer.AnswerId);
                }

                await PopulateQuestion();
            }, (busy) => answer.IsVotingBusy = busy);


        }

        public async void MarkAsAccepted(Answer answer)
        {
            await RunTaskAsync(async () =>
            {
                //Only one answer can be accepted.
                Answer accepted = Answers.FirstOrDefault(x => x.IsAccepted);

                if (accepted != null)
                {
                    await answerRestService.UnmarkAsAccepted(answer.AnswerId);
                }
                else
                {
                    await answerRestService.MarkAsAccepted(answer.AnswerId);
                }

                await PopulateQuestion();
            }, busy => answer.IsVotingBusy = busy);

        }

        public async void SaveEditedAnswer()
        {
            editingAnswer.EditedBody = editingAnswer.EditedBody.Trim();

            if (editingAnswer.EditedBody.Length < appSettings.MinAnswerLength)
            {
                toastService.ShowWarning(string.Format(
                    AppResources.Validation_BodyValidationFrmt,
                    appSettings.MinAnswerLength,
                    editingAnswer.EditedBody.Length
                ));
                return;
            }

            await RunTaskAsync(async () =>
            {
                await answerRestService.ChangeBody(editingAnswer.AnswerId, editingAnswer.EditedBody);
                editingAnswer.IsEdit = false;
                await PopulateQuestion();
                editingAnswer = null;
            },
             (ex) =>
             {
                 RestException restException = ex as RestException;
                 if (restException != null)
                 {
                     editingAnswer.IsEdit = false;
                     editingAnswer = null;
                 }
             });
        }

        public async void UnmarkAsAccepted(Answer answer)
        {
            await RunTaskAsync(async () =>
            {
                await answerRestService.UnmarkAsAccepted(answer.AnswerId);
                await PopulateQuestion();
            }, busy => answer.IsVotingBusy = busy);
        }

        private Answer editingAnswer;

        public void EditAnswer(Answer answer)
        {
            answer.IsEdit = true;
            editingAnswer = answer;
        }

        public void CancelEditAnswer()
        {
            if (editingAnswer == null) return;

            editingAnswer.EditedBody = editingAnswer.Body;
            editingAnswer.IsEdit = false;

            editingAnswer = null;
        }

        public async void RemoveAnswer(Answer answer)
        {
            if (MessageBox.Show(AppResources.Confirm_RemoveAnswer, AppResources.Confirm_Title, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            await RunTaskAsync(async () =>
            {
                await answerRestService.Remove(answer.AnswerId);
                await PopulateQuestion();
            });
        }

        public void ViewImage()
        {
            navigationService.UriFor<ImageViewModel>()
                .WithParam(x => x.Image, new Uri(Question.ImageUri))
                .WithParam(x => x.Body, Question.Body)
                .Navigate();
        }

        public void OpenTag(string tag)
        {
            navigationService.UriFor<Search.SearchByTagViewModel>()
                .WithParam(x => x.Tag, tag).Navigate();
        }

        public void OpenUserProfile(Author author)
        {
            if (appSettings.IsAuthenticated)
            {
                navigationService.UriFor<UserProfile.UserProfileViewModel>()
                    .WithParam(vm => vm.UserId, author.UserId)
                    .Navigate();
            }
        }

        #region EditQuestion

        private bool isQuestionEdit;
        public bool IsQuestionEdit
        {
            get { return isQuestionEdit; }
            set
            {
                isQuestionEdit = value;
                NotifyOfPropertyChange(() => IsQuestionEdit);
            }
        }

        public void EditQuestion()
        {
            Body = Question.Body;
            Tags = new List<string>(Question.Tags);
            IsQuestionEdit = true;
        }

        public void CancelEditQuestion()
        {
            IsQuestionEdit = false;
            Body = Question.Body;
            Tags = Question.Tags;
        }

        private string body;
        public string Body
        {
            get { return body; }
            set
            {
                if (value == body) return;
                body = value;
                NotifyOfPropertyChange(() => Body);
            }
        }

        private List<string> tags = new List<string>();
        private bool isQuestionExist;

        public List<string> Tags
        {
            get { return tags; }
            set
            {
                if (Equals(value, tags)) return;
                tags = value;
                NotifyOfPropertyChange(() => Tags);
            }
        }

        public async void SaveEditedQuestion()
        {
            bool valid = Validate();

            if (!valid)
            {
                return;
            }

            //Hide the keyboard
            ((PhoneApplicationPage)this.GetView()).Focus();

            await RunTaskAsync(async () =>
            {
                IsQuestionEdit = false;
                await questionRestService.ChangeInfo(QuestionId, Body, Tags);
                await PopulateQuestion();
            },
            (ex) =>
            {
                RestException restException = ex as RestException;
                if (restException != null)
                {
                    IsQuestionEdit = false;
                }
            });

        }

        #endregion

        public async void RemoveQuestion()
        {
            if (MessageBox.Show(AppResources.Confirm_RemoveQuestion, AppResources.Confirm_Title, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }

            await RunTaskAsync(async () =>
            {
                await questionRestService.Remove(question.QuestionId);

                App.ReloadFeeds = true;

                //The question does not exist so simple go back.
                navigationService.GoBack();
            });

        }

        #endregion


    }
}
