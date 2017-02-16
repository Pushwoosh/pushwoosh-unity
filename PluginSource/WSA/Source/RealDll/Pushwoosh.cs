using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSDK;
using PushSDK.Classes;

namespace PushwooshForWindows
{
    public class TokenEventArgs : EventArgs
    {
        public string Token { get; set; }
    }

    public class TokenErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
    }

    public class PushEventArgs : EventArgs
    {
        public string PushPayload { get; set; }
    }

    public class Pushwoosh
    {
        private readonly NotificationService service = null;

        public event EventHandler<PushEventArgs> OnPushAccepted;
        public event EventHandler<TokenErrorEventArgs> OnPushTokenFailed;
        public event EventHandler<TokenEventArgs> OnPushTokenReceived;

        private void internalPushAccepted(object sender, ToastPush push)
        {
            string pushPayload = push.ToString();
            PushEventArgs args = new PushEventArgs();
            args.PushPayload = pushPayload;

            if (OnPushAccepted != null)
                OnPushAccepted(this, args);
        }
        private void internalPushTokenReceived(object sender, string token)
        {
            TokenEventArgs args = new TokenEventArgs();
            args.Token = token;

            if (OnPushTokenReceived != null)
                OnPushTokenReceived(this, args);
        }
        private void internalPushTokenFailed(object sender, string errorMessage)
        {
            TokenErrorEventArgs args = new TokenErrorEventArgs();
            args.ErrorMessage = errorMessage;

            if (OnPushTokenFailed != null)
                OnPushTokenFailed(this, args);
        }


        public Pushwoosh(string PW_APP_ID)
        {
            service = NotificationService.GetCurrent(PW_APP_ID);
            service.OnPushAccepted += internalPushAccepted;
            service.OnPushTokenReceived += internalPushTokenReceived;
            service.OnPushTokenFailed += internalPushTokenFailed;
        }

        public void SubscribeForPushNotifications()
        {
            service.SubscribeToPushService();
        }

        public void UnsubscribeForPushNotifications()
        {
            service.UnsubscribeFromPushes(null, null);
        }

        public void SetTags(List<KeyValuePair<string, object>> tags)
        {
            service.SendTag(tags, null, null);
        }

        public void StartGeoLocation()
        {
            service.StartGeoLocation();
        }

        public void StopGeoLocation()
        {
            service.StopGeoLocation();
        }

        public string PushToken
        {
            get {
                return service.PushToken;
            }
        }

        public string HWID
        {
            get
            {
                return service.DeviceUniqueID;
            }
        }
    }
}
