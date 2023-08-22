# About application
Application has next projects:
- TestApp - web application
- Data - intecations with DB
- Services - business logic
- Consumer - consumes ServiceBus topic
- Infrastructure - contains interfaces, DTOs, models, validator
- Tests

# How to run
1) Fill missing settings in appsettings.json in TestingApp
2) Fill missing settings in local.settings.json in Consumer
3) Run TestingApp and Consumer.
TestingApp has enpoints to interact, but count of orders for customers won't increase until you run consumer
Consumer asumes that data always correct from the queue.

# Room for improvement
Probably OrderRepo should have method to fetch count of customer's active orders instead of fetching all of them just to filter in application and count.
But as for me efficient way is to add LastUpdated entities via SQL queries, e.g.:
UPDATE X
SET X.{Property1} = {Value1}, ...., X.{ValueLast} = Value.Last, X.LastUpdated = GETUTCDATE()
OUTPUT inserted.*
FROM {TABLE} X
WHERE X.LastUpdated = {PreviousLastUpdated} AND X.ID = {ValueId}

and if query return nothing, then nothing was updated.
But for updating OrderCount we could event simply rely on its own transaction to not mess up

UPDATE X
SEX X.OrderCount = X.OrderCount + @Diff
FROM Customers X
WHERE X.Id = @Id

Also foreign keys between the tables.