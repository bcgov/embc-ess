import { Injectable } from '@angular/core';
import { map, catchError } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { Config, Country, Region, Community, RelationshipType } from '../models';

import * as ConfigActions from 'app/store/lookups/config.actions';
import * as CountryActions from 'app/store/lookups/country.actions';
import * as RegionActions from 'app/store/lookups/region.actions';
import * as CommunityActions from 'app/store/lookups/community.actions';
import * as RelationshipTypeActions from 'app/store/lookups/relationship-type.actions';

@Injectable({
  providedIn: CoreModule
})
export class ControlledListService extends RestService {

  // Loaded once at init time, as they do not change very often, and
  // certainly not within the app.

  getConfig() {
    this.store.dispatch(new ConfigActions.LoadConfig());
    return this.http.get<Config>('/api/config', { headers: this.headers })
      .pipe(
        map((config: Config) => {
          // everything went OK
          this.store.dispatch(new ConfigActions.LoadConfigSuccess({ config }));
        }),
        catchError(error => {
          // dispatch a fail action if there were errors
          this.store.dispatch(new ConfigActions.LoadConfigFail(error));
          return this.handleError(error);
        }),
      );
  }

  getAllCountries() {
    this.store.dispatch(new CountryActions.LoadCountries());
    return this.http.get<Country[]>('/api/countries', { headers: this.headers })
      .pipe(
        map((countries: Country[]) => {
          // everything went OK
          this.store.dispatch(new CountryActions.LoadCountriesSuccess({ countries }));
        }),
        catchError(error => {
          // dispatch a fail action if there were errors
          this.store.dispatch(new CountryActions.LoadCountriesFail(error));
          return this.handleError(error);
        }),
      );
  }

  getAllRegions() {
    this.store.dispatch(new RegionActions.LoadRegions());
    return this.http.get<Region[]>('/api/regions', { headers: this.headers })
      .pipe(
        map((regions: Region[]) => {
          this.store.dispatch(new RegionActions.LoadRegionsSuccess({ regions }));
        }),
        catchError(error => {
          this.store.dispatch(new RegionActions.LoadRegionsFail(error));
          return this.handleError(error);
        }),
      );
  }

  getAllCommunities() {
    this.store.dispatch(new CommunityActions.LoadCommunities());
    return this.http.get<Community[]>('/api/communities', { headers: this.headers })
      .pipe(
        map((communities: Community[]) => {
          // sort the list of communities alphabetically.
          // communities = communities.sort((a, b) => {
          //   if (a.name < b.name) { return -1 }
          //   if (a.name > b.name) { return 1 }
          //   return 0;
          // });
          this.store.dispatch(new CommunityActions.LoadCommunitiesSuccess({ communities }));
        }),
        catchError(error => {
          this.store.dispatch(new CommunityActions.LoadCommunitiesFail(error));
          return this.handleError(error);
        }),
      );
  }

  getAllFamilyRelationshipTypes() {
    this.store.dispatch(new RelationshipTypeActions.LoadRelationshipTypes());
    return this.http.get<RelationshipType[]>('/api/familyRelationships', { headers: this.headers })
      .pipe(
        map((relationshipTypes: RelationshipType[]) => {
          this.store.dispatch(new RelationshipTypeActions.LoadRelationshipTypesSuccess({ relationshipTypes }));
        }),
        catchError(error => {
          this.store.dispatch(new RelationshipTypeActions.LoadRelationshipTypesFail(error));
          return this.handleError(error);
        }),
      );
  }
}
