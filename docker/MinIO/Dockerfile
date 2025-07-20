ARG MINIO_VERSION
FROM minio/minio:$MINIO_VERSION

ENTRYPOINT ["/usr/bin/docker-entrypoint.sh"]
CMD ["server", "/data"]