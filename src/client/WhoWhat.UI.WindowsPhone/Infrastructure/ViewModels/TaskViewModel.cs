using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Caliburn.Micro;
using WhoWhat.UI.WindowsPhone.Resources;
using WhoWhat.UI.WindowsPhone.Services;

namespace WhoWhat.UI.WindowsPhone.Infrastructure.ViewModels
{
    public class TaskViewModel : WhoWhatViewModel
    {
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        protected override void OnInitialize()
        {
            SetPropertiesFromNavigation();
            base.OnInitialize();
        }

        private void SetPropertiesFromNavigation()
        {
            //Retrive list of properties of the current screen
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (string key in NavigationParams.Keys)
            {
                PropertyInfo prop = properties.FirstOrDefault(x => x.Name == key);
                if (prop != null)
                {
                    //If cache contains a property with such a name - set it.
                    object value = NavigationParams.GetAndRemove(key);

                    if (value != null)
                    {
                        prop.SetValue(this, value);
                    }
                }
            }

            //NavigationParams is used only to send data from one page to another. 
            //There is no need to keep any data in it after navigation.
            NavigationParams.Clear();
        }


        public async Task RunTaskAsync(Func<Task> task, Action<Exception> failure = null)
        {
            await RunTaskAsync(task, (value) => IsBusy = value, failure);
        }

        public async Task RunTaskAsync(Func<Task> task, Action<bool> setBusy, Action<Exception> failure = null)
        {
            try
            {
                setBusy(true);
                await task();
            }
            catch (System.Net.WebException)
            {
                setBusy(false);
                IoC.Get<ToastService>().ShowError(AppResources.Message_NoInternet);
            }
            catch (RestException ex)
            {
                setBusy(false);
                FlurryWP8SDK.Api.LogError("TaskViewModel -> RestException", ex);

                DebugHelper.RevealException(ex);

                if (failure != null)
                {
                    failure(ex);
                }
                else
                {
                    IoC.Get<ToastService>().ShowError(AppResources.Message_ServerError);
                }
            }
            catch (Exception ex)
            {
                setBusy(false);
                FlurryWP8SDK.Api.LogError("TaskViewModel -> Unspecified", ex);

                DebugHelper.RevealException(ex);

                if (failure != null)
                {
                    failure(ex);
                }
            }
            finally
            {
                setBusy(false);
            }
        }
    }
}
