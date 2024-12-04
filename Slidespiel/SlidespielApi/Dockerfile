FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
COPY . ./app
COPY /MediaToolkitTemp/ffmpeg /MediaToolkitTemp/ffmpeg
RUN chmod +x /MediaToolkitTemp/ffmpeg
WORKDIR /app/
RUN dotnet build -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
COPY --from=build /app/output .
COPY /MediaToolkitTemp/ffmpeg /MediaToolkitTemp/ffmpeg
RUN chmod +x /MediaToolkitTemp/ffmpeg
EXPOSE 8080
ENTRYPOINT ["dotnet", "SlidespielApi.dll"]