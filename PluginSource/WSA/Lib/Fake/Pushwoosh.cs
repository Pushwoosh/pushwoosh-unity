using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public event EventHandler<PushEventArgs> OnPushAccepted;
        public event EventHandler<TokenErrorEventArgs> OnPushTokenFailed;
        public event EventHandler<TokenEventArgs> OnPushTokenReceived;

        public Pushwoosh(string PW_APP_ID)
        {
        }

        public void SubscribeForPushNotifications()
        {
            TokenEventArgs args = new TokenEventArgs();
            args.Token = PushToken;
            if (OnPushTokenReceived!= null)
                OnPushTokenReceived(this, args);
        }

        public void UnsubscribeForPushNotifications()
        {
        }

        public void SetTags(List<KeyValuePair<string, object>> tags)
        {
        }

        public void StartGeoLocation()
        {
        }

        public void StopGeoLocation()
        {
        }

        public string PushToken
        {
            get
            {
                return "Not Windows Phone PushToken";
            }
        }

        public string HWID
        {
            get
            {
                return "Not Windows Phone HWID";
            }
        }
    }
}