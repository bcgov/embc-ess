import { Injectable } from '@angular/core';
import { map, catchError } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { Country, Region, RegionalDistrict, Community, RelationshipType } from '../models';

import * as CountryActions from 'src/app/store/lookups/country.actions';
import * as RegionActions from 'src/app/store/lookups/region.actions';
import * as RegionalDistrictActions from 'src/app/store/lookups/regional-district.actions';
import * as CommunityActions from 'src/app/store/lookups/community.actions';
import * as RelationshipTypeActions from 'src/app/store/lookups/relationship-type.actions';

@Injectable({
  providedIn: CoreModule
})
export class ControlledListService extends RestService {

  // Loaded once at init time, as they do not change very often, and
  // certainly not within the app.

  getAllCountries() {
    this.store.dispatch(new CountryActions.LoadCountries());
    return this.get('/countries')
      .pipe(
        map((countries: Country[]) => {
          // everything went OK
          this.store.dispatch(new CountryActions.LoadCountriesSuccess({ countries }));
        }),
        catchError(error => {
          // dispatch a fail action if there were errors
          this.store.dispatch(new CountryActions.LoadCountriesFail(error));
          throw error;
        }),
      );
  }

  getAllRegions() {
    this.store.dispatch(new RegionActions.LoadRegions());
    return this.get('/regions')
      .pipe(
        map((regions: Region[]) => {
          this.store.dispatch(new RegionActions.LoadRegionsSuccess({ regions }));
        }),
        catchError(error => {
          this.store.dispatch(new RegionActions.LoadRegionsFail(error));
          throw error;
        }),
      );
  }

  getAllRegionalDistricts() {
    this.store.dispatch(new RegionalDistrictActions.LoadRegionalDistricts());
    return this.get('/regionaldistricts')
      .pipe(
        map((regionalDistricts: RegionalDistrict[]) => {
          this.store.dispatch(new RegionalDistrictActions.LoadRegionalDistrictsSuccess({ regionalDistricts }));
        }),
        catchError(error => {
          this.store.dispatch(new RegionalDistrictActions.LoadRegionalDistrictsFail(error));
          throw error;
        }),
      );
  }

  getAllCommunities() {
    this.store.dispatch(new CommunityActions.LoadCommunities());
    return this.get('/communities')
      .pipe(
        map((communities: Community[]) => {
          this.store.dispatch(new CommunityActions.LoadCommunitiesSuccess({ communities }));
        }),
        catchError(error => {
          this.store.dispatch(new CommunityActions.LoadCommunitiesFail(error));
          throw error;
        }),
      );
  }

  getAllFamilyRelationshipTypes() {
    this.store.dispatch(new RelationshipTypeActions.LoadRelationshipTypes());
    return this.get('/relationshiptypes')
      .pipe(
        map((relationshipTypes: RelationshipType[]) => {
          this.store.dispatch(new RelationshipTypeActions.LoadRelationshipTypesSuccess({ relationshipTypes }));
        }),
        catchError(error => {
          this.store.dispatch(new RelationshipTypeActions.LoadRelationshipTypesFail(error));
          throw error;
        }),
      );
  }

}
