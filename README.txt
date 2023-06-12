Specifications

GET /launches?per_page={x}&page={y}

    Returns a JSON object with the `names` property containing the names of the missions.    

    * The number of elements is specified by `per_page` and will default to `10` if nothing is provided, is not a valid number or is less than or equal to `0`

    * The page number is specified by `page` will default to `1` if nothing is provided, is not a valid number or is less than or equal to `0`


GET /launches/{id}

    Returns details for the specified launch identifier. When a launch is found, the following as JSON object:
	{	DateCached ,
                MissionName ,
                DateLunch ,
                RocketName ,
                FirstRocketlaunch ,
                RateSucessRocket
	}

    

    If the launch is not found a `404` should be returned instead.

    The launch information should be cached when the launch exists.The information will be in cache until the element have been deleted
	only the first query for an item will be to the database, all other requests for the same item will be to the cache

Execution Details
Configure connection to DB, in the file "appsettings.json"
	run migrations to create database and test data, use the command:
		`update-database from visual` studio
		`dotnet ef database update` fromCMD
	run project
	run unit tests with the command
		`dotnet test`

