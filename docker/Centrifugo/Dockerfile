ARG CENTRIFUGO_VERSION
FROM centrifugo/centrifugo:$CENTRIFUGO_VERSION

COPY ./config.json /centrifugo/config.json

CMD [ "centrifugo", "-c", "config.json"]