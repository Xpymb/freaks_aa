ARG RABBITMQ_VERSION
FROM rabbitmq:$RABBITMQ_VERSION

COPY rabbitmq.conf /etc/rabbitmq
COPY definitions.json /etc/rabbitmq

EXPOSE 5672

RUN chown rabbitmq:rabbitmq /etc/rabbitmq/rabbitmq.conf /etc/rabbitmq/definitions.json

CMD ["rabbitmq-server"]
