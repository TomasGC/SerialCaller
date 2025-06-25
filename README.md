# Introduction 
This tool will help you to create api calls scenarios and automatize them.
For example, if you need to do a lot of catch-up, it can be useful.

# Getting Started
You can fork the project to your own space, I do not mind it.
I already prepared azure and github yml files to be able to generate the dotnet tool.

You will be able to install it through this command:
	dotnet tool install dotnet-sc -g --interactive -v m

To run it after installation:
	dotnet-sc

To update it:
	dotnet tool update dotnet-sc -g

# Configuration
In order to avoid pushing sensitive data in your own repository, you'll have to create the appsettings.json in a specific folder:
	%USERPROFILE%\Documents\SerialCaller
You'll have also to put there your CSV or JSON files too, just for the example, you'll find a CSV and JSON file in Example scenario.

# Contribute
If you have any ideas on how to improve it, do not hesitate and share your ideas.
You can also do pull requests.