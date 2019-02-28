// tslint:disable
import { Observable, of } from 'rxjs';

const routes = {
  // default (not-found) route
  'api/not-found': {
    error: {
      status: 404,
      error: 'NOT_FOUND',
      message: 'Route not found!',
    },
  },
  // lookup tables
  'api/lookup/supports': {
    data: [
      { id: 1, name: 'Food' },
      { id: 2, name: 'Clothing' },
      { id: 3, name: 'Accommodation' },
      { id: 4, name: 'Incidentals' },
      { id: 5, name: 'Transportation' },
    ],
  },

  'api/lookups/regions': {
    data: [
      { id: 1, name: 'Region 1' },
      { id: 2, name: 'Region 2' },
      { id: 3, name: 'Region 3' },
      { id: 4, name: 'Region 4' },
      { id: 5, name: 'Region 5' },
    ]
  },

  'api/lookups/regionaldistricts': {
    data: [
      { id: 1, name: 'CRD', regionId: 1 },
      { id: 2, name: 'Alberni-Clayoquot (Regional District)', regionId: 2 },
    ]
  },

  'api/lookups/communities': {
    data: [
      { id: 1, name: 'Victoria (City)', regionalDistrictId: 1 },
      { id: 2, name: 'Armstrong (City)', regionalDistrictId: 2 },
      { id: 3, name: 'Belcarra (Village)', regionalDistrictId: 2 },
    ]
  },

  'api/lookups/familyrelationships': {
    data: [
      { id: 1, name: 'Spouse' },
      { id: 2, name: 'Son' },
      { id: 3, name: 'Daugther' },
      { id: 4, name: 'Father' },
      { id: 5, name: 'Mother' },
    ]
  },

  'api/lookups/dietaryneeds': {
    data: [
      { id: 1, name: 'No nuts' },
      { id: 2, name: 'No gluten' },
      { id: 3, name: 'Vegetarian' },
      { id: 4, name: 'Vegan' },
      { id: 5, name: 'Pescatarian' },
    ]
  },

  'api/lookups/referrals': {
    data: [
      { id: 1, name: 'Inquiry' },
      { id: 2, name: 'Health Services' },
      { id: 3, name: 'First Aid' },
      { id: 4, name: 'Personal Services' },
      { id: 5, name: 'Child Care' },
      { id: 6, name: 'Pet Care' },
    ]
  },

  'api/registrations': { data: [] },

  // auth route
  'api/me': { data: [] },

  // admin routes (IDIR)
  'api/incidenttasks': {
    data: [
      { id: 1, taskNumber: '123456', description: 'Some details about the incident here', communityId: 1, regionaDistrictId: null, regionId: null },
      { id: 2, taskNumber: '999888', description: 'Some details about the incident here', communityId: null, regionaDistrictId: 2, regionId: null },
      { id: 3, taskNumber: '789012', description: 'Some details about the incident here', communityId: null, regionaDistrictId: null, regionId: 2 },
    ]
  },

  'api/organizations': { data: [] },
  'api/organizations/1/users': { data: [] },
};


// mocking server-side API for now
export function httpGet(url = 'api/not-found', options?) {
  const mockResponse = routes[url];
  return of(mockResponse);
}
