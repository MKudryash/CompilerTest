# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.



FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS compile
USER root
# Устанавливаем Python, C, Java, C#
RUN apk add --no-cache gcc musl-dev python3 py3-pip openjdk11-jdk
ENV PATH=/root/.dotnet/tools:$PATH
RUN dotnet tool update -g dotnet-execute && echo 'export "PATH=$PATH:/root/.dotnet/tools"' >> /etc/profile


# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM  compile AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CompilerTestC/CompilerTestC.csproj", "CompilerTestC/"]
RUN dotnet restore "./CompilerTestC/CompilerTestC.csproj"
COPY . .
WORKDIR "/src/CompilerTestC"
RUN dotnet build "./CompilerTestC.csproj" -c $BUILD_CONFIGURATION -o /app/build



# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CompilerTestC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompilerTestC.dll"]