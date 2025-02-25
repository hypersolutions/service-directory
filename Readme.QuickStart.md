# Quick Start

The instructions below will get the API and UI running.

## API

The _API_ uses a _SQLite_ database which has migrations. To create the database you can run the following command line from the root:

```bash
dotnet ef database update -c ApplicationDbContext --project src/ServiceDirectory.Infrastructure --startup-project src/ServiceDirectory.Api
```

**or**

Use your IDE of choice with the following settings:

- **Migrations Project** ServiceDirectory.Infrastructure
- **Startup Project** ServiceDirectory.Api
- **DbContext** ApplicationDbContext
- **Target Migration** 20250214154155_SeedData (if required)

Check that all the tests run. You can use your IDE of choice or run the following command from the root:

```bash
dotnet test
```

Run the _API_ via your IDE or the following command:

```bash
cd src/ServiceDirectory.Api
dotnet run
```

And browse to https://localhost:7297/

This will launch scalar. Select one of the get operations such as GET _/api/service/{id}_ and use Id = 1. You should get back a service. 

This wil confirm that your database is up and running.

The GET endpoints are anonymous but the POST, PUT and DELETE ones require a token. Under the _util_ folder there is a _BearerTokenGenerator_ project.

Run this from your IDE or from the command line (from root):

```bash
cd util/BearerTokenGenerator
dotnet run
```

This will print a bearer token that you can use.

## UI

The _ui_ is built using _VueJS 3_ and _Vuetify_. It is built against the node version _23.8.0_.

For ease, use _nvm_ and install the required node version:

```bash
nvm install 23.8.0
nvm use 23.8.0
```

To install all the packages run the following command from the root:

```bash
cd ui
npm install
```

Ensure the _api_ is running (as above) and then run the _ui_:

```bash
npm run dev
```

And browse to http://localhost:3000/

**Note** sometimes the page is blank so refresh and all should be fine.

The home page has a search. Enter a postcode. Suggested ones are:

- **Hampshire** SO14 7LY
- **Bristol** BS1 5TR

Consult the migration seed data or find other postcodes based upon the regions Bristol and Southampton - which has service data.
