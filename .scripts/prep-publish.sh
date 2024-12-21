#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./publish.sh

if [[ -z "$1" ]]; then
    printf "\nMissing parameter:\n  ./prep-publish.sh [AppGoodMusicWebApi | AppGoodMusicRazor | AppGoodMusicMVC]\n"
    exit 1
fi
ApplicationToPublish=$1
#Step1: Set the Azure Keyvault access parameters as operating system environment variables.
ApplicationProjectFile="/Users/Martin/Development/goldenProjects/GoodMusic_with_lesson_branches/Configuration/Configuration.csproj"

PWDIR=$(pwd)
echo $PWDIR
cd /Users/Martin/Development/scripts/azure
AzureProjectSettings=$(sed -n 's:.*<AzureProjectSettings>\(.*\)</AzureProjectSettings>.*:\1:p' $ApplicationProjectFile)

printf "\n\nSetting the azure key vault access as environent variables"
export AZURE_TENANT_ID=$(./az-access.sh $AzureProjectSettings tenantId)
export AZURE_KeyVaultUri=$(./az-access.sh $AzureProjectSettings kvUri)
export AZURE_KeyVaultSecret=$(./az-access.sh $AzureProjectSettings kvSecret)

export AZURE_CLIENT_ID=$(./az-access-secrets.sh $AzureProjectSettings app appId)
export AZURE_CLIENT_SECRET=$(./az-access-secrets.sh $AzureProjectSettings app password)
cd $PWDIR

#verify environment variables
echo "AZURE_TENANT_ID=" $AZURE_TENANT_ID
echo "AZURE_KeyVaultUri=" $AZURE_KeyVaultUri
echo "AZURE_KeyVaultSecret=" $AZURE_KeyVaultSecret
echo "AZURE_CLIENT_ID=" $AZURE_CLIENT_ID
echo "AZURE_CLIENT_SECRET=" $AZURE_CLIENT_SECRET

#Step2: Generate the release files
printf "\n\nPublish the webapi...\n"
# #remove any previous publish
rm -rf ../$ApplicationToPublish/publish

cd ../$ApplicationToPublish
dotnet publish --configuration Release --output ./publish


#Step3: Run the application from the folder containing the release files.
printf "\n\nRun the webapi from the published directory...\n"
cd ./publish

exec ./$ApplicationToPublish


