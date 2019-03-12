FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

ARG SourceDir
COPY $SourceDir /app

ENTRYPOINT ["dotnet", "Identity.Api.dll"]
