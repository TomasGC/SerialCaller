# Introduction 
First, thanks to 15Dkatz for his simple API: https://github.com/15Dkatz/official_joke_api.
This project is an example of how to integrate an API.

# Architecture
For reach API project you'll need this files

## &emsp;1. An executor factory
*Example: OfficialJokeApiExecutorFactory*

This class will simplify the way to call the Executors.
It avoid to have numerous dependency injections too.

## &emsp;2. A dependency injection register
*Example: OfficialJokeApiDependencyInjectionRegister*

The dependency injection is automatized thanks to the inheritance with _IProjectDependencyInjectionRegister_.
Having a register like this one in your API project is mandatory to avoid modifying _Program.cs_ every time.

## &emsp;3. _(optional)_ a base executor model
*Example: BaseOfficialJokeApiExecutorModel*

This class is only needed if you have specific data for your API (ApiKey, ID & Password).

## &emsp;4. _(optional)_ A Models folder

It will contains the data in your requests and responses.
You may do differently if you prefer.

## &emsp;5. Different call folders
*Example: GetJoke, GetRandomJoke*

### &emsp;&emsp;a. A request
*Example: GetJokeRequest*

Must exist and inherit from IRequest, even if it's empty.

### &emsp;&emsp;b. A response
*Example: GetJokeResponse*

Must exist and inherit from IResponse, even if it's empty.

### &emsp;&emsp;c. A model executor
*Example: GetJokeExecutorModel*

This will contains all the requests and responses (as successes and failures) for a specific endpoint.

### &emsp;&emsp;d. An executor
*Example: GetJokeExecutor*

This simple class will specify the endpoint to reach.