# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution
COPY . .

# Restore dependencies
RUN dotnet restore "FlexyboxBlogRazor/FlexyboxBlogRazor/FlexyboxBlogRazor.csproj"

# Build and publish
RUN dotnet publish -c Release -o /out "FlexyboxBlogRazor/FlexyboxBlogRazor/FlexyboxBlogRazor.csproj"

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Install Kerberos library
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

# Copy the certificates from the certs directory to the container
COPY certs/dockercert.pfx /https/dockercert.pfx
COPY certs/dockercert.crt /usr/local/share/ca-certificates/dockercert.crt

# Install the .crt file as a trusted certificate
RUN update-ca-certificates

# Expose only the HTTPS port
EXPOSE 443

ENTRYPOINT ["dotnet", "FlexyboxBlogRazor.dll"]