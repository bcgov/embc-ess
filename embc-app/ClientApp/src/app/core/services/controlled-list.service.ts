import { Injectable } from '@angular/core';

import { RestService } from './rest.service';

@Injectable()
export class ControlledListService extends RestService {

  ////////////////////
  // Look-ups
  ////////////////////

  getAllSupportTypes() {
    // this.store.dispatch()
    return this.get('/lookup/supports');
  }

  getAllRegions() {
    return this.get('/lookup/regions');
  }

  getAllRegionalDistricts() {
    return this.get('/lookup/regionaldistricts');
  }

  getAllCommunities() {
    return this.get('/lookup/communities');
  }

  getAllFamilyRelationshipTypes() {
    return this.get('/lookup/familyrelationshiptypes');
  }

  getAllDietaryNeeds() {
    return this.get('/lookup/dietaryneeds');
  }

  getAllReferralTypes() {
    return this.get('/lookup/referrals');
  }
}
