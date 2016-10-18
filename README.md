# SiteWarmer
Azure Function App for warming a site. Loads each page in a given XML Sitemap.

## Functions

### QueueFromSitemap

A manual trigger function that loads a XML Sitemap for a given URL and queues all the pages using Azure Storage Queue.

Note: each queue message is added to the queue so there is a 2 second separation in its `NextVisibleTime` between messages. 
This is done to throttle the process so the site does not get overloaded.

### ProcessQueue

A queue trigger function that processes the queue of URLs.


## Settings

`warmup_STORAGE`: The Azure Storage account connection string
`SiteCredentials` (optional): The basic authentication credentials.  

