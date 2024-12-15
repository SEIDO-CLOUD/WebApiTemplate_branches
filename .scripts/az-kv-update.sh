#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./az-kv-update.sh

ApplicationProjectFile="/Users/Martin/Development/goldenProjects/WebApiTemplate_branches/Configuration/Configuration.csproj"

PWDIR=$(pwd)
cd /Users/Martin/Development/scripts/azure

AzureProjectSettings=$(sed -n 's:.*<AzureProjectSettings>\(.*\)</AzureProjectSettings>.*:\1:p' $ApplicationProjectFile)

./az-login.sh $AzureProjectSettings
./az-kv-sec-copy.sh $AzureProjectSettings $ApplicationProjectFile

cd $PWDIR