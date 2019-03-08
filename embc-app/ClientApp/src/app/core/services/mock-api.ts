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
  'api/countries': useEnvelope([
    { id: '1', name: 'Canada', active: true },
    { id: '2', name: 'United States', active: true },
    { id: '3', name: 'Australia', active: true },
    { id: '4', name: 'Bahamas, The', active: true },
    { id: '5', name: 'Cote d\'Ivoire', active: true },
    { id: '6', name: 'Japan', active: true },
  ]),

  'api/regions': useEnvelope([
    { id: '1', name: 'Vancouver Island', active: true },
    { id: '2', name: 'South West', active: true },
    { id: '3', name: 'Central', active: true },
    { id: '4', name: 'South East', active: true },
    { id: '5', name: 'North East', active: true },
    { id: '6', name: 'North West', active: true },
  ]),

  'api/relationshiptypes': useEnvelope([
    { code: '1', description: 'Spouse', active: true },
    { code: '2', description: 'Son', active: true },
    { code: '3', description: 'Daughter', active: true },
    { code: '4', description: 'Father', active: true },
    { code: '5', description: 'Mother', active: true },
  ]),
};

// add data relationships
routes['api/regionaldistricts'] = useEnvelope([
  { id: '1', name: 'Capital Region', active: true, region: byId('api/regions', '1') },
  { id: '2', name: 'Alberni-Clayoquot (Regional District)', active: true, region: byId('api/regions', '2') },
]);

routes['api/communities'] = useEnvelope([
  { id: '1', name: 'Victoria (City)', regionalDistrict: byId('api/regionaldistricts', '1') },
  { id: '2', name: 'Armstrong (City)', regionalDistrict: byId('api/regionaldistricts', '2') },
  { id: '3', name: 'Belcarra (Village)', regionalDistrict: byId('api/regionaldistricts', '2') },
]);

// admin routes (IDIR)
routes['api/incidenttasks'] = useEnvelope([
  { id: '1', taskNumber: '123456', description: 'Some details about the incident here', community: byId('api/communities', '1'), regionalDistrict: null, region: null },
  { id: '2', taskNumber: '999888', description: 'Some details about the incident here', community: null, regionalDistrict: byId('api/regionaldistricts', '2'), region: null },
  { id: '3', taskNumber: '789012', description: 'Some details about the incident here', community: null, regionalDistrict: null, region: byId('api/regions', '2') },
]);

// registrations
routes['api/registrations'] = useEnvelope([]);

// orgs
routes['api/organizations'] = useEnvelope([]);
routes['api/organizations/1/users'] = useEnvelope([]);

// auth route
routes['api/user/current'] = useEnvelope({
  id: '1',
  name: 'Doe, John',
  firstname: 'John',
  lastname: 'Doe',
  email: 'jdoe@some-email.com',
  isNewUser: false,
});


// mocking server-side API for now
export function httpGet(url = 'api/not-found', options?) {
  console.log(`fetching '${url}'`)
  const mockResponse = routes[url];
  return of(mockResponse);
}

// private
function byId(url = 'api/not-found', id: string): any {
  const payload: { data?: any[]; error?: any; } = routes[url];
  const { data, error } = payload;
  if (error) {
    return null;
  }
  return (data || []).find(x => x.id == id);
}

function useEnvelope(response: any, skip = false) {
  if (skip) {
    return response;
  }
  return { data: response };
}
