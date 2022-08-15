# Lizard

Lizard is a system monitoring service designed for integration with applications running alongside RabbitMQ.

Client applications instantiate `Lizard.Monitor.LizardMessageClient` and send log messages, exception details or 
http logs via RabbitMQ.  Those messages are picked up by the `Lizard` service, listening on the message queue, and
stored in an SQL database where they can be assessed.

