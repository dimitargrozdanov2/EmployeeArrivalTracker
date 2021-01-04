namespace ReportingTool.Web.Utils
{
    public class TokenConstants
    {
        public static readonly string ServiceTokenHeader = "X-Fourth-Token";

        public static readonly string ClientHeaderValue = "Fourth-Monitor";

        public static readonly string SubscriptionUrl = "api/clients/subscribe";

        public static readonly string WebSiteUrl = "http://localhost:51396/";

        public static readonly string CallbackUrl = "http://localhost:62243/Home/ReceiveArrivalInfoFromService";

    }
}
