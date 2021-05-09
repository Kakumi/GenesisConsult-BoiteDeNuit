# GenesisConsult-BoiteDeNuit

## Project settings
Developed using **Clean Architecture** with ASP.NET Core 5.0 and EF Core with InMemory context.

## Using Postman
You can find a complete collection to import for testing at `Documents/Genesis Consult Nightclub.postman_collection.json`.</br>
Be sure that the variable URL is correctly set to the environment API URL.
```json
"variable": [
	{
		"key": "url",
		"value": "https://localhost:44351/api/members"
	}
]
```

## Units Tests
Unit test are present in Postman request but also in the project.