FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG PROJECT
WORKDIR /src
COPY ${PROJECT}/${PROJECT}.csproj ./
RUN dotnet restore
COPY ${PROJECT}/ ./
RUN dotnet publish -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
ARG PROJECT
ENV APP_DLL=${PROJECT}.dll
ENV ASPNETCORE_HTTP_PORTS=8080
WORKDIR /app
COPY --from=build /out ./
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "exec dotnet \"$APP_DLL\""]
