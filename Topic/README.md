# Topic

![topic flow](https://github.com/kapozade/rabbitmq/blob/main/topic-flow.png?raw=true)

* Routes a received message to queues where binding key matches to the routing key. Example: Binding-Key => *.notifications.error, Routing-Key => website-1.notifications.errors