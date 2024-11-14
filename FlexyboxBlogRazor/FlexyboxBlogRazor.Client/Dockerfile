# Build the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution
COPY . .

# Restore dependencies specifically for Blazor WebAssembly project
RUN dotnet restore "FlexyboxBlogRazor/FlexyboxBlogRazor.Client/FlexyboxBlogRazor.Client.csproj"

# Set the working directory to the Blazor WebAssembly project for publishing
WORKDIR /src/FlexyboxBlogRazor/FlexyboxBlogRazor.Client

# Build and publish
RUN dotnet publish -c Release -o /out

# Use Nginx to serve the static files
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /out/wwwroot .  # Ensure this points to the correct publish output

# Expose port for WebAssembly app
EXPOSE 80