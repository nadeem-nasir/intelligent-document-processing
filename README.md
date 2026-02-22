# Title
This is a simple project to demonstrate how to use the azure form recognizer service in Azure functions in an isolated worker process model.
This project also shows how to read email attachments from office365 and save them to azure blob storage using logic app and then trigger azure function to extract data from saved attachments in blob using azure form recognizer service.
# Introduction 
- It also shows how to extract using build-int invoice models and parse from file,url and blob storage.
- It also shows how to read email attachments from office365 and save them to azure blob storage using logic app.
- The form recognizer service is a cognitive service that uses machine learning to extract data from forms. 
- The form recognizer service can be used to extract data from different types of forms such as invoices, receipts, business cards, and identity documents.
- Azure AI service enables you to build intelligent document processing solutions. Massive amounts of data, spanning a wide variety of data types, are stored in forms and documents
- The service can be trained to extract data from different types of forms. 
- The Azure.AI.FormRecognizer nuget package can be used to extract data from forms in different formats such as PDF, JPEG, PNG, and TIFF. 
- The Azure.AI.FormRecognizer nuget package can also be used to extract data from forms in different languages such as English, French, Spanish, and Portuguese. 
- For more info visit https://learn.microsoft.com/en-us/azure/ai-services/document-intelligence/overview?view=doc-intel-4.0.0
# Prerequisites
- Azure subscription 
- Azure Form Recognizer resource 
- Azure form recognizer Url and Key
- Azure storage account
- Azure logic app
- Office365 account
- Configure the logic app connections in azure portal to read email attachments and save them to azure blob storage
- Visual Studio or Visual Studio Code
- .NET 8.0
- Azure Functions Core Tools v4.x
- Azure storage account
# Azure resources used in the project
- Azure Function App in .NET 8.0, isolated worker process  Linux OS, 
- Azure hosting plan
- Azure Confugurations in Bicep template
- Azure Storage
- Azure blob storage
- Azure Storage account role assignment in Bicep template
- Azure Logic App
- Azure Logic App connections
- Azure Form Recognizer
- Azure Cognitive Services Account
# To run the application locally
- Clone the repository
- Open the solution in Visual Studio or Visual Studio Code
- Rename template.settings.json to local.settings.json
- Update the local.settings.json file with the Azure form recognizer Url and Key and storage accouunt connection string
- Deploy the bicep template from infra folder with logic app and connections
- Go to azure portal and configure the logic app connections to read email attachments and save them to azure blob storage
- Need to configure two connections in logic app: one for office365 and another for azure blob storage
- Currenlty the logic app is configured to read email attachments from a office365 (work email) and save them to a specific folder in azure blob storage
- Email subject should be "Invoice" and email body should contain atachments according to format in "Template" folder
- Must send an email to same email address that is configured in logic app office365 connection
- Bicep template deploy all the required resources and also set the required configurations. so it is easy to deploy the infrastructure and test the application
# Deploying Azure Function Apps using Azure DevOps
- Azure subscription
- Deploy infrastructure using Azure Bicep temnplate from infra folder
- Create a new project in Azure DevOps or GitHub
- Create a new service connection in Azure DevOps or GitHub
- Create variables group in Azure DevOps or GitHub
- Create a new pipeline in Azure DevOps or GitHub using the pipelines.yaml file from deploy folder
- Push the code to the repository			
# Learning Outcomes of the Case Study 
- Use the azure form recognizer service in Azure functions in an isolated worker process model to extract data from invoices.
- Use logic app integration with office365 to read email attachments
- Create logic app in azure bicp
- Use logic app to save email attachments to azure blob storage
- Parse form recognizer response into a strongly typed object.
- Extract data from invoices using file,URL or blob with form recognizer.
- Deploy Azure Function Apps using Azure DevOps and YAML pipelines.
- Use Azure Bicep to deploy Azure resources.
- Use Azure Functions isolated worker process model.
- Use HTTP trigger in Azure Functions.
- Use environment variables to store secrets
- Dependency injection in Azure Functions
- Use service and provider pattern in Azure Functions
- Use ConfigureAppConfiguration ,ConfigureFunctionsWorkerDefaults and ConfigureServices methods in Azure Functions
- Write extension method in C#.
- Demonstrate switch expression pattern matching in C#.
- Utilize global using directives in C#.
- Utilize primary constructors in C#.
- Implement expression-bodied members in C#.
- Demonstrates how to structure and organize your code and project 
# Extend the Case Study: Todo For Learning 
- Deploy the infrastructure using Azure Bicep template
- Update AzureWebJobsStorage, FormRecognizerEndpoint,FormRecognizerKey1 in  local.settings.json		
- Run the application locally
- Run PingInvoice function and see the output
- Create a new queue trigger function that take file name as input and read the file from json-customer-invoice-email-attachments and output the result to json-customer-invoice-email-attachments-output
- Or you can also output the result to a table storage or cosmos db
