pipeline {
  agent any
  stages {
    stage('BuildDeploy') {
      agent any
      steps {
        sh '''#!/bin/bash
REGISTRY="docker.voidwell.com"
REPOSITORY="weatherman/discord"
REGISTRY_USER=$REGISTRY_USER
REGISTRY_PASSWORD=$REGISTRY_PASSWORD

REGISTRY_CRED="${REGISTRY_USER}:${REGISTRY_PASSWORD}"

TAGS="`curl -s --user ${REGISTRY_CRED} https://${REGISTRY}/v2/${REPOSITORY}/tags/list | jq -r \'.tags\' | sed \'s/[^0-9]*//g\'`"
LATEST=`echo "${TAGS[*]}" | sort -nr | head -n1`
BUILDTAG=$((LATEST + 1))

docker build -t ${REGISTRY}/${REPOSITORY}:${BUILDTAG} -t ${REGISTRY}/${REPOSITORY}:latest .

docker push ${REGISTRY}/${REPOSITORY}:${BUILDTAG}
docker push ${REGISTRY}/${REPOSITORY}:latest

echo -e "\\nCompleted ${REGISTRY}/${REPOSITORY}:${BUILDTAG}"'''
      }
    }
  }
  environment {
    REGISTRY_USER = credentials('docker-registry-user')
    REGISTRY_PASSWORD = credentials('docker-registry-password')
  }
}