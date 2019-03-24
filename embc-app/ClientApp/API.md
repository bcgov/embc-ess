# EMBC API endpoints

All endpoints are accessed through `https://server.gov/api` endpoint. That endpoint will be protected when real data is held there.



## Expected

url | Description | PUT/POST/GET/DELETE | status
-|-|-|-
`api/not-found` | Not found page  | |
`api/countries` | A list of current countries available to the front-end. | |
`api/regions` - BC regions for the front-end. | |
`api/regionaldistricts` | BC regional districts. | |
`api/familyrelationships` | Family relationships that we acknowledge as choices in the front-end. | |
`api/communities` | BC communities | |
`api/incidenttasks` | Incident tasks | |
`api/registrations` | Registrations retrievable as a list. | GET | Works
`api/registrations/:id` | Registrations retrievable by id | PUT POST |
`api/volunteer/` | A list of volunteers | GET | need api endpoint
`api/volunteer/:bceid` | A volunteer collected by bceid | GET POST DELETE | 
`api/organizations` | Organizations for volunteers. | |
`api/organizations/1/users` | Users belonging to an organization | | 
`api/user/current` | Details about the current user. |  |
`api/gender` | The collection of gender options. | GET | In discussion
`api/provinces` | A list of provinces. | GET | In discussion