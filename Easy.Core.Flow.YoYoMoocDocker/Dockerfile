#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 443

FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/sdk AS build
WORKDIR /src
COPY ["Easy.Core.Flow.YoYoMoocDocker/Easy.Core.Flow.YoYoMoocDocker.csproj", "Easy.Core.Flow.YoYoMoocDocker/"]
RUN dotnet restore "Easy.Core.Flow.YoYoMoocDocker/Easy.Core.Flow.YoYoMoocDocker.csproj"
COPY . .
WORKDIR "/src/Easy.Core.Flow.YoYoMoocDocker"
RUN dotnet build "Easy.Core.Flow.YoYoMoocDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Easy.Core.Flow.YoYoMoocDocker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Easy.Core.Flow.YoYoMoocDocker.dll"]