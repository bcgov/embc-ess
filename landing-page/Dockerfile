FROM image-registry.openshift-image-registry.svc:5000/9edb33-tools/caddy:latest
COPY Caddyfile /etc/caddy/Caddyfile
COPY html/ /site/
ENV LOG_LEVEL=INFO
