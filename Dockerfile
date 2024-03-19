FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

EXPOSE 5000

ENV ASPNETCORE_HTTP_PORTS 5000

RUN apt-get update \
    && apt-get install -y --no-install-recommends libfontconfig1 \
    && rm -rf /var/lib/apt/lists/*

COPY build/ .
RUN mkdir Resources
COPY Resources/ ./Resources

ENTRYPOINT ["dotnet", "DisplayUtil.dll"]