# Etapa 1: Build de la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /build

# Etapa 2: Servir archivos estáticos con nginx
FROM nginx:alpine
COPY --from=build /build/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
