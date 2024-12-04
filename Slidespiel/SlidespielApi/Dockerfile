FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
COPY . ./app
RUN ls -a
COPY /MediaToolkitTemp/ffmpeg .
WORKDIR /app/
RUN dotnet build -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
COPY --from=build /app/output .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SlidespielApi.dll"]