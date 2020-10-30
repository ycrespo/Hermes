#!/usr/bin/env bash
# infrastructure

#Install Kibana and Elastic
docker-compose -f docker-compose-kibana-elasticsearch.yml up -d
#Install Seq
docker run --name seq -e ACCEPT_EULA=Y -p 5341:80 -v C:\Users\crespo\OneDrive\Repositories\MyProjects\Tristan\Logs:\data datalust/seq:latest
#install TristanDb
docker-compose -f docker-compose-postgres-tristan.yml up -d