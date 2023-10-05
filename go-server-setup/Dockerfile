FROM heroiclabs/nakama-pluginbuilder:3.3.0 AS builder

ENV GO111MODULE on
ENV CGO_ENABLED 1

WORKDIR /backend
COPY go.mod .
COPY *.go .
COPY vendor/ vendor/

RUN go build --trimpath --mod=vendor --buildmode=plugin -o .bakcend.so

FROM heroiclabs/nakama:3.3.0

COPY --from=builder /backend/backend.so /nakama/data/modules/
COPY local.yml /nakama/data