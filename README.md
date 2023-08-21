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
Also foreign keys between the tables.