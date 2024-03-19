# VagaTranslator

VagaTranslator is a project developed using [VS Code](https://code.visualstudio.com/) version 1.87.0 and [.Net Core](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) version 8.0.3

## Prerequisites

The application requires an Azure Translator API key. To ensure successful execution of the app, please add your API key to the `appSettings.json` file.

## Running the Application Locally

To run the application on your local machine, navigate to the root folder and execute the following command:

```bash
dotnet run --project .\src\MinimalTranslator.Api\
```

Alternatively, you can open the project in Visual Studio Code and press F5.

## API Endpoints

The API provides two endpoints:  

- `/api/translation`: Accepts a JSON object containing a text (e.g., { "text": "your text" }) and returns a translation ID.
- `/api/translation/{id}`: Returns the translated text corresponding to the provided ID.

## Docker

The application is also available as a [Docker image](https://www.docker.com/get-started). To run the Dockerized version of the API, navigate to the project root folder and execute the following commands

```bash

docker build -t <name-you-wish> .
docker run -p 5000:8080 -it <name-you-wish>
```

Alternatively, you can use Docker Compose:

```bash
docker-compose up -d
```

## Testing

A collection is available for testing the application using [Postman](https://www.postman.com/). The base URL for the Dockerized app is `http://localhost:5000` (e.g., `http://localhost:5000/api/translation`), and for the local instance, it’s `http://localhost:5082` or `https://localhost:7087` (e.g., `http://localhost:5082/api/translation`)

## Unit tests

To run the unit tests, navigate to the root folder and execute the following command

```bash
dotnet test .\tests\MinimalTranslator.Core.Tests\
```
