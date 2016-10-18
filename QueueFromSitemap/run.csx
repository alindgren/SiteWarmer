#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Net;
using System.Xml;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

public static void Run(string input, TraceWriter log)
{
    log.Info($"Queuing URLs from " + input);

    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
    ConfigurationManager.AppSettings["warmup_STORAGE"]);

    // Create the queue client.
    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

    // Retrieve a reference to a queue.
    CloudQueue queue = queueClient.GetQueueReference("warmupqueue");

    // Create the queue if it doesn't already exist.
    queue.CreateIfNotExists();

        WebClient client = new WebClient();

    string credentials = ConfigurationManager.AppSettings["SiteCredentials"];
    if (!String.IsNullOrEmpty(credentials))
        client.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", credentials);

    XmlDocument doc = new XmlDocument();
    doc.LoadXml(client.DownloadString(input));

    int count = 0;

    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
    {
        foreach (XmlNode locNode in node)
        {
            if (locNode.Name == "loc")
            {
                string url = locNode.InnerText;

                // Create a message and add it to the queue.
                CloudQueueMessage message = new CloudQueueMessage(url);
                queue.AddMessage(message, null, new TimeSpan(0, 0, 0, count*3), null, null);

                log.Info(url + " added to queue.");
            }
        }
        count++;
        //if (count > 3) break;
    }

}
