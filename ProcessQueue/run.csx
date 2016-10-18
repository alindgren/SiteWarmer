using System;
using System.Net;

public static void Run(string myQueueItem, TraceWriter log)
{
    WebClient client = new WebClient();

    string credentials = ConfigurationManager.AppSettings["SiteCredentials"];
    if (!String.IsNullOrEmpty(credentials))
        client.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", credentials);
    string retValue = client.DownloadString(myQueueItem);
    log.Info($"Loaded: {myQueueItem}");
}