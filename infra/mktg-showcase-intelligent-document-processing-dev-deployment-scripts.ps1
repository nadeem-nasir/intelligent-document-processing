# Prerequisites for using Bicep:
# - Install the Azure CLI: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
# - Install Bicep: https://github.com/Azure/bicep/blob/main/docs/installing.md
# - Install Bicep VS Code extension
# - Sign in to Azure: Run 'az login' and follow the authentication steps
# --tenant only needed if you have multiple tenants and need to specify which one to use
#  azure role Mappings and more. https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles 


# This line logs in to Azure using the Azure CLI (Command-Line Interface) and specifies the tenant ID to authenticate against
az login --tenant ""

# This line lists all the subscriptions associated with the logged-in Azure account.
az account subscription list

# This line sets the active subscription to the one specified by the subscription ID or name. It determines the subscription where the subsequent deployment will take place.
az account set --subscription ""

# This line assigns the value "westeurope" to the variable $Location. It represents the Azure region where the resource group will be created.
$Location = "westeurope"

#It represents the name of the resource group that will be created.
$ResourceGroupName = "rg-mktg-crmio-dev-01"

# This line checks if a resource group with the specified name already exists. It uses the Azure CLI command az group exists to determine if the resource group exists and assigns the result to the variable
$rsgExists = az group exists -n $ResourceGroupName
if ($rsgExists -eq 'false') {
    az group create -l $Location -n $ResourceGroupName
}

# This line deploys a template using the Azure CLI command 
az deployment group create --resource-group "rg-mktg-crmio-dev-01" --template-file "mktg-showcase-intelligent-document-processing-main.bicep" --parameters "mktg-showcase-intelligent-document-processing-main-dev.bicepparam"


#Storage File Data Privileged Contributor
#Storage Blob Data Contributor
# Get the current user id and assign the role to the storage account
$currentUserId = $(az ad signed-in-user show --query id -o tsv)
az deployment group create --resource-group "rg-mktg-crmio-dev-01" --template-file '.\templates\storage-accounts-role-assignments.bicep'  --parameters storageAccountName='stcrmiodevwe01' principalId="$currentUserId" principalType='User' roleDefinitionId='ba92f5b4-2d11-453d-a403-e96b0029c9fe' 

# delete resource group and all resources in it
az group delete --name "rg-mktg-crmio-dev-01" --yes 

