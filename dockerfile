FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app
COPY SchoolApp.IdentityProvider.Api SchoolApp.IdentityProvider.Api/
COPY SchoolApp.IdentityProvider.Application SchoolApp.IdentityProvider.Application/
COPY SchoolApp.IdentityProvider.Ioc SchoolApp.IdentityProvider.Ioc/
COPY SchoolApp.IdentityProvider.Sql SchoolApp.IdentityProvider.Sql/

COPY SchoolApp.Shared.Utils SchoolApp.Shared.Utils/
COPY SchoolApp.Shared.Utils.Sql SchoolApp.Shared.Utils.Sql/
COPY SchoolApp.Shared.Utils.HttpApi SchoolApp.Shared.Utils.HttpApi/


RUN dotnet restore SchoolApp.IdentityProvider.Api/SchoolApp.IdentityProvider.Api.csproj
RUN dotnet publish SchoolApp.IdentityProvider.Api/SchoolApp.IdentityProvider.Api.csproj -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/SchoolApp.IdentityProvider.Api.dll" ]