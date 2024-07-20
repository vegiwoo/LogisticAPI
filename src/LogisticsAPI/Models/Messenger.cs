namespace LogisticsAPI.Models
{
    public class Messenger
    {
        #region Variables and constants 
        private const string TELEGRAM_URL = "http://t.me/";

        #endregion

        #region Properties
        public MessengerType Type { get; set; }
        public Uri? Link { get; set; }
        #endregion

        #region Constructors
        public Messenger(MessengerType type, string uriString) 
        {
            Type = type;

            switch (type)
            {
                case MessengerType.Telegram:
                    if (uriString.StartsWith('@')) 
                    {
                        uriString = uriString.Remove(0, 1);
                    }

                    uriString = TELEGRAM_URL + uriString;

                    if(Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out Uri? uri)) 
                    {
                        Link = uri;
                    } 
                    else 
                    {
                        Link = null;
                        throw new Exception($"Невозожно создать url из строки '{uriString}'.\nПроверьте правильность ее формирования.");
                    }
                    break;
            }
        }
        #endregion
    }
}