FROM alpine:3.20.3

ENV STORAGE_ACCOUNT_NAME ""
ENV STORAGE_ACCOUNT_URI ""
ENV API_URI ""
ENV CONTAINER_NAME ""
ENV AZURE_CLIENT_ID ""
ENV AZURE_SUBSCRIPTION_ID ""

RUN apk update
RUN apk add --no-cache bash curl py3-pip
RUN apk add --no-cache --virtual=az-build gcc musl-dev python3-dev libffi-dev openssl-dev cargo make
RUN pip install --upgrade --break-system-packages pip
RUN pip install --break-system-packages azure-cli
RUN apk del az-build
RUN apk update && apk add --update nodejs-current npm
RUN apk add nodejs npm

WORKDIR /
COPY . .
RUN chmod +x ./entrypoint.sh

ENTRYPOINT [ "./entrypoint.sh" ]