FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443



FROM node:14 AS client 
# ARG skip_client_build=false 
WORKDIR /app 
COPY techpro.zapp/ClientApp .
RUN  npm install 
RUN  npm run-script build


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["techpro.zapp/techpro.zapp.csproj", "techpro.zapp/"]
RUN dotnet restore "techpro.zapp/techpro.zapp.csproj"
RUN dotnet restore "techpro.zapp/techpro.zapp.csproj"
COPY . .
WORKDIR "/src/techpro.zapp"
RUN dotnet build "techpro.zapp.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish --configuration Release -o /app/publish



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist
ENTRYPOINT ["dotnet", "techpro.zapp.dll"]