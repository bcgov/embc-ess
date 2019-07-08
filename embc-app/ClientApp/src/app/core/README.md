# Core

The core module establishes the core functionlity for the front-end.

## Guards

### Landing Page

The landing page guard directs the user to the correct route for their role.

### Logged In

If the user is "logged in" let them pass otherwise redirect to login page.

### Module Import

Alerts a developer that they have double-loaded a module.

### Redirect

Route to an external URL using a router.

### Role

Check the user's role against the one specified in the routing.

## Interceptors

### Unauthorized

All http requests that the client makes that have a 401 when the application state is "logged in" logs out and causes session expired page.

### Watchdog

This is a timer resetter. All http requests that generate a 200 response reset a timer in the watchdog service. Dogs don't wear watches normally but when they do they should do it like this.

## Models

All of these models are interfaces that describe the expected properties of each API response model. At the beginning of the software development, these models directly matched the ones returned in the API and database. Now there are some divergences.

**NOTE:** The registration model has a collection of properties that are inverted before submission in the front-end. (requiresAccommodation, requiresClothing, requiresFood, requiresIncidentals, requiresTransportation) These are all represented in the application as "hasFood" instead of requires. Before submission to the API they must be inverted otherwise they have an opposite meaning. This made its way to the end of the project because it was an end-to-end change that was unclear if it would persist. Instead of performing data migrations and API changes we inverted the checkboxes. As the application increased in size more instances of this inversion happened. The developers priority was on requested features instead of "invisible changes" which on an agile budget resulted in a few application-wide workarounds persisting until the end of the project. This will be a confusing model change if you don't know about it when resuming the development.

## Services

### rest
This is an abstract class that provides some handy features for the http services.

### auth
This service holds some observables for making decisions about application state. It includes login and logout methods as well as ways to get/set the current user. It has hard-coded routing paths inside.

### captcha-data
Verify and fetch captchas. Captcha functionality is based on https://github.com/bcgov/MyGovBC-CAPTCHA-Widget
 
### controlled-list
Load infrequently changing data at init time. Puts things into the ngrx store.

### cookie
Get and set cookies.

### evacuee
Http service to perform R with params on the `api/evacuees` endpoint.

### incident-task
Http service to perform CRU with params on the `api/incidenttasks/` endpoint.

### notification-queue
Add notifications for users into this service. A notifier component can observe this notification queue and display the notifications.

### organization
Http service to perform CRU with params on the `api/organizations` endpoint.

### referral
Http service to perform CRUD with params on the `api/registrations/${registrationId}/referrals/` endpoint.

### registration
Http service to perform CRU with params on the `api/registrations/` endpoint. For some reason this service also contains PDF collection for referrals.

### unique-key
The unique key service holds a single uuid for lookup after a navigation. Some of the unique keys were put back into routes and parameters instead of making this service accept multiple arguments. No time to refactor; this change will be one that is a caveat for whoever resumes the project.

### volunteer
Http service to perform CRU with params on the `api/volunteers` endpoint. This also includes the API call for bcid duplication checks.


### watchdog
The watchdog is a service with a timer. When the timer runs out it pops up a modal box that lets the user choose to hit the users endpoint and stay logged in.
