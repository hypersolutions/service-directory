# Service Directory

## Overview

This uses a basic service directory to demonstrate a clean architecture implementation. The database used is the SQLite db but 
can easily be switched out for SQL Server or another option if required.

There are migrations to bring the database up to date with some basic data harvested from a couple of Open Referral-enabled local authorities.

The quality of the data isn't great as there appears to be poor/incorrect implementation of the OR standard generally.

The model is basic for demo purposes. All organisations have at least one location but could have multiple locations. The same
is applied to services. There is currently no concept of services with no location such as services that may exist solely online.

There are CRUD operations for organisation, service and location plus a search operation.

## API

The API is built using the _FastEndpoints_ package. User secrets are supported if you wish to utilise this approach.

This uses the Scalar.AspNetCore package as a UI instead of the traditional Swagger UI. 

To load the UI add:

```
/scalar
```

The _launchSettings.json_ will launch this page by default.

There are both unit and integration (API) tests to provide a solid level of coverage.

### Search

You can search for services based upon a postcode. It will resolve the search postcode into a lat/lon using the _postcodes.io_ API.

For simplicity and given the small range of the search distance allowed, it uses the Haversine algorithm for determining the available 
services within the search radius.

The algorithm is in-lined within the linq query, so looks complicated but the raw form is documented in the _ServiceSearchQueryHandler_ class.

## Build

This supports _github_ actions which trigger on main changes. This will build the code and run the tests - that is all.

## Next Steps

Things being looked at to explore:

1. A basic UI using VUE
2. Playwright UI tests
3. Local OAuth server for authentication and some basic role-based permissions for management of organisations, services and locations