FROM microsoft/dotnet:2.1-sdk

# copy csproj and restore as distinct layers
COPY *.csproj ./build/
RUN cd build && dotnet restore

# copy and build everything else
COPY . ./build/
RUN cd build && dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "build/out/IoTSink.dll"]
