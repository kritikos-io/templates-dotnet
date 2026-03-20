# syntax=docker/dockerfile:1.20
ARG RUNTIME_BASE=web
ARG CONFIGURATION=Release
ARG DOTNET_VERSION=10.0
ARG RUNTIME_ID=linux-musl-x64

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-alpine-composite-extra AS web
WORKDIR /app

FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION}-alpine-extra AS app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/runtime-deps:${DOTNET_VERSION}-alpine-extra AS self-contained
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS sdk
ENV DOTNET_NOLOGO=1
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ARG RUNTIME_ID
WORKDIR /source

FROM sdk AS restore

COPY --link .editorconfig .globalconfig global.json icon.png nuget.config *.slnx *.sln *.sln.DotSettings stylecop.json testconfig.json xunit.runner.json ./
COPY --link --parents ./**Directory*.props ./**Directory.*.targets ./
COPY --link --parents ./**.csproj ./**/packages.lock.json ./
RUN ls -lR .

RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore

FROM sdk AS gitversion
RUN --mount=type=cache,target=/root/.nuget/packages dotnet tool install --global GitVersion.Tool
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY --link --parents **/GitVersion.yml ./
COPY --link .git/ ./.git/
RUN dotnet-gitversion /output json > /version.json

FROM restore AS build
ARG CONFIGURATION

RUN mkdir -p /source/artifacts/OpenApi && \
  mkdir -p /source/artifacts/bin && \
  mkdir -p /source/artifacts/obj && \
  mkdir -p /source/artifacts/nugets && \
  mkdir -p /source/artifacts/upload

COPY LICENSE.md README.md ./
COPY --link src/ src/
COPY --link tests/ tests/
COPY --link samples/ samples/
COPY --link --from=gitversion /version.json ./version.json

RUN GV_MAJOR=$(grep -o '"Major": *[0-9]*' version.json | grep -o '[0-9]*') && \
  GV_MINOR=$(grep -o '"Minor": *[0-9]*' version.json | grep -o '[0-9]*') && \
  GV_PATCH=$(grep -o '"Patch": *[0-9]*' version.json | grep -o '[0-9]*') && \
  GV_SEMVER=$(grep -o '"SemVer": *"[^"]*"' version.json | cut -d'"' -f4) && \
  GV_FULLSEMVER=$(grep -o '"FullSemVer": *"[^"]*"' version.json | cut -d'"' -f4) && \
  GV_INFORMATIONAL=$(grep -o '"InformationalVersion": *"[^"]*"' version.json | cut -d'"' -f4) && \
  { echo "GV_MAJOR=${GV_MAJOR}"; \
    echo "GV_MINOR=${GV_MINOR}"; \
    echo "GV_PATCH=${GV_PATCH}"; \
    echo "GV_SEMVER=${GV_SEMVER}"; \
    echo "GV_FULLSEMVER=${GV_FULLSEMVER}"; \
    echo "GV_INFORMATIONAL=${GV_INFORMATIONAL}"; \
    echo "VERSION_PROPS=\"/p:Version=\${GV_SEMVER} /p:AssemblyVersion=\${GV_MAJOR}.\${GV_MINOR}.\${GV_PATCH}.0 /p:FileVersion=\${GV_MAJOR}.\${GV_MINOR}.\${GV_PATCH}.0 /p:InformationalVersion=\${GV_INFORMATIONAL} /p:PackageVersion=\${GV_FULLSEMVER}\""; \
  } > /source/version.env

RUN --mount=type=cache,target=/root/.nuget/packages \
  . /source/version.env && \
  eval dotnet build --no-restore --configuration ${CONFIGURATION} ${VERSION_PROPS}

FROM build AS test
ARG CONFIGURATION

RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet test --no-build --configuration ${CONFIGURATION}

FROM node:lts-alpine AS openapi-lint
RUN npm install -g @stoplight/spectral-cli
WORKDIR /openapi
COPY --link .spectral.yaml /tmp/.spectral.yaml
COPY --link --from=build /source/artifacts/OpenApi/ .
RUN if ls *.json 1>/dev/null 2>&1; then \
  spectral lint ./*.json -r /tmp/.spectral.yaml --fail-severity=error; \
  else \
  echo "No OpenAPI documents found, skipping lint"; \
  fi

FROM test AS publish
ARG CONFIGURATION
ARG PUBLISHED_PROJECT
ARG RUNTIME_BASE
ARG PUBLISH_READY_TO_RUN=false
ENV PROJECT=src/${PUBLISHED_PROJECT}/${PUBLISHED_PROJECT}.csproj

RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore --runtime ${RUNTIME_ID} ${PROJECT}

RUN --mount=type=cache,target=/root/.nuget/packages \
  . /source/version.env && \
  if [ "${RUNTIME_BASE}" = "self-contained" ]; then \
  eval dotnet publish --configuration ${CONFIGURATION} \
  --self-contained --runtime ${RUNTIME_ID} \
  /p:PublishReadyToRun=${PUBLISH_READY_TO_RUN} \
  /p:OpenApiGenerateDocuments=false \
  ${VERSION_PROPS} \
  -o /app/publish ${PROJECT}; \
  else \
  eval dotnet publish --configuration ${CONFIGURATION} \
  --no-self-contained --runtime ${RUNTIME_ID} \
  /p:PublishReadyToRun=${PUBLISH_READY_TO_RUN} \
  /p:OpenApiGenerateDocuments=false \
  ${VERSION_PROPS} \
  -o /app/publish ${PROJECT}; \
  fi

FROM build AS pack

RUN --mount=type=cache,target=/root/.nuget/packages \
  . /source/version.env && \
  eval dotnet pack --configuration Release ${VERSION_PROPS} --output /app/publish

FROM sdk AS nuget-push
WORKDIR /packages
COPY --link --from=pack /app/publish/*.nupkg .
ENTRYPOINT ["sh", "-c", "dotnet nuget push /packages/*.nupkg --source ${NUGET_SOURCE} --api-key ${NUGET_API_KEY} --skip-duplicate"]
CMD []

FROM build AS migration-bundle
RUN --mount=type=cache,target=/root/.dotnet/tool \
  dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
ARG CONFIGURATION
ARG PUBLISHED_PROJECT
ARG DATABASE_PROJECT
ARG DBCONTEXT
ENV CONTEXT="${DBCONTEXT:+--context ${DBCONTEXT}}"
ENV EFPROJECT=src/${DATABASE_PROJECT}/${DATABASE_PROJECT}.csproj
ENV RUNNER=src/${PUBLISHED_PROJECT}/${PUBLISHED_PROJECT}.csproj

RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet ef migrations bundle --configuration ${CONFIGURATION} --no-build --project ${EFPROJECT} --startup-project ${RUNNER} --output /app/migrations ${CONTEXT}

FROM sdk AS migrations
COPY --link --chown=root:root --chmod=755 --from=migration-bundle /app/migrations .
USER app
ENTRYPOINT ["./migrations"]
CMD []

FROM alpine:latest AS pyroscope-agent
ARG PYROSCOPE_VERSION
RUN mkdir -p /opt/pyroscope && \
  if [ -n "${PYROSCOPE_VERSION}" ]; then \
  wget -qO- "https://github.com/grafana/pyroscope-dotnet/releases/download/v${PYROSCOPE_VERSION}-pyroscope/pyroscope.${PYROSCOPE_VERSION}-musl-x86_64.tar.gz" \
  | tar xz -C /opt/pyroscope; \
  fi

FROM ${RUNTIME_BASE} AS final
ARG PUBLISHED_PROJECT
ARG RUNTIME_BASE
ENV LC_ALL=en_US.UTF-8 LANG=en_US.UTF-8

COPY --link --from=publish --chown=root:root /app/publish .
COPY --link --from=openapi-lint --chown=root:root /openapi/ /app/openapi/
COPY --link --from=pyroscope-agent --chown=root:root /opt/pyroscope/ /pyroscope/
RUN if [ "${RUNTIME_BASE}" = "self-contained" ]; then \
  chmod +x ${PUBLISHED_PROJECT}; \
  printf '#!/bin/sh\nexec /app/%s "$@"\n' "${PUBLISHED_PROJECT}" > /app/entrypoint.sh; \
  else \
  printf '#!/bin/sh\nexec dotnet /app/%s.dll "$@"\n' "${PUBLISHED_PROJECT}" > /app/entrypoint.sh; \
  fi && chmod +x /app/entrypoint.sh

USER $APP_UID
ENTRYPOINT ["/app/entrypoint.sh"]
