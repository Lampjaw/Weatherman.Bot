pipeline {
  agent any
  stages {
    stage('Build') {
      agent any
      steps {
        sh '''#!/bin/bash
REGISTRY=$REGISTRY_ENDPOINT
REPOSITORY=$REPOSITORY
DOCKERFILE_PATH=$DOCKERFILE_PATH

docker build -t ${REGISTRY}/${REPOSITORY}:latest -f ${DOCKERFILE_PATH} .

echo -e "\\nBuild Completed"'''
      }
    }
    stage('Docker Push') {
      steps {
        sh '''#!/bin/bash
REGISTRY=$REGISTRY_ENDPOINT
REPOSITORY=$REPOSITORY
REGISTRY_USER=$REGISTRY_USER
REGISTRY_PASSWORD=$REGISTRY_PASSWORD

REGISTRY_CRED="${REGISTRY_USER}:${REGISTRY_PASSWORD}"

TAGS="`curl -s --user ${REGISTRY_CRED} https://${REGISTRY}/v2/${REPOSITORY}/tags/list | jq -r \'.tags\' | sed \'s/[^0-9]*//g\'`"
LATEST=`echo "${TAGS[*]}" | sort -nr | head -n1`
BUILDTAG=$((LATEST + 1))

docker tag ${REGISTRY}/${REPOSITORY}:latest ${REGISTRY}/${REPOSITORY}:${BUILDTAG}

docker push ${REGISTRY}/${REPOSITORY}:${BUILDTAG}
docker push ${REGISTRY}/${REPOSITORY}:latest

echo -e "\\nPushed ${REGISTRY}/${REPOSITORY}:${BUILDTAG}"'''
      }
    }
  }
  environment {
    REGISTRY_USER = credentials('docker-registry-user')
    REGISTRY_PASSWORD = credentials('docker-registry-password')
    REGISTRY_ENDPOINT = 'docker.voidwell.com'
    REPOSITORY = 'weatherman/discord'
    DOCKERFILE_PATH = 'Weatherman-Discord.Dockerfile'
  }
}