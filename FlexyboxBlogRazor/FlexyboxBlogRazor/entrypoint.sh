#!/bin/bash
# Import the self-signed certificate to the trusted root
cp /https/dockercert.pfx /usr/local/share/ca-certificates/dockercert.crt
update-ca-certificates
# Start the application
exec "$@"