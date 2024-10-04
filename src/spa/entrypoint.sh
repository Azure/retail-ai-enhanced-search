#!/bin/bash
export APPSETTING_WEBSITE_SITE_NAME=DUMMY

echo "Authenticating to Azure with Managed Identity..."
az login --identity
az account set --subscription $AZURE_SUBSCRIPTION_ID

echo "enabling static website"
az storage blob service-properties update \
    --account-name $STORAGE_ACCOUNT_NAME \
    --static-website \
    --404-document error.html \
    --index-document index.html \
    --auth-mode login

echo "setting environment variables"
touch ./.env
echo "VITE_API_URI=https://${API_URI}/products" > ./.env
echo "VITE_STORAGE_ACCOUNT_URL=${STORAGE_ACCOUNT_URI}${CONTAINER_NAME}/" >> ./.env

echo "compiling Javascript"
npm install
npm run build

echo "copying error.html to dist"
cp ./error.html ./dist/

echo "copying built javascript to storage account"
az storage blob upload-batch -s ./dist/ -d '$web' --account-name $STORAGE_ACCOUNT_NAME --overwrite --auth-mode login 

echo "copying images to storage account"
az storage blob upload-batch -s ./images -d $CONTAINER_NAME --account-name $STORAGE_ACCOUNT_NAME --overwrite --auth-mode login
