# Work Queue

![workqueue flow](https://github.com/kapozade/rabbitmq/blob/main/workqueue-flow.png?raw=true)

* Round robin dispatching
* Prefetch: RabbitMQ dispatches a message when it enters queue. You can define your consumer to handle number of X message at time.