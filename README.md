This project is  reporting tool that provides information on a table with data about the time employees arrive to the building.

As the project is built on Database-First approach, a backup of the database is added under a folder database backup.

In the folder Comments there are 2 files with recommendations.

Things that can be improved in version 2 of this project:
1. Exceptions can be logged on a file
2. Scheduled daily job using Quartz that will delete tokens that have expired(by checking their property Expires and comparing it to Utc.Now)
3. Ajax in order to not reload the page
4. Another alternative which could be looked into is Store the token in the session.
5. Pagination can be improved and filtration can be improved to work using DateTime and not text field.
6. More Unit Tests.

Version 2 changes:
1. Pagination now works with DateTime and pagination shows results of 20 per page. Previous and Next pages now work correctly.
2. Session cannot be stored in the token due to a new httpcontext instance. Another option is to use InMemory Database to store the token in Redis and set ttl for each token.
3. DateTime can be implemented with a range (from and to) and return the result which covers the range.
4. Logs from Exceptions can be logged at C:\Windows\logs so they are only hosted on the local machine and not on production.
