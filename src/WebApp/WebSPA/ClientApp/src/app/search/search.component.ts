import { Component, OnInit } from '@angular/core';
import { SkilledWrokersService } from '../services/skilled-wrokers/skilled-wrokers.service';
import { Profession } from '../models/Profession';
import { SkillLevel } from '../models/SkillLevel';
import PlaceResult = google.maps.places.PlaceResult;
import { UserProfile } from '../models/UserProfile';
import { PaginatedUserProfiles } from '../models/PaginatedUserProfile';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  //this should be loaded from server
  professions : Profession[] = [
        new Profession(
          "Driver",
              [
                new SkillLevel("Medium"),
                new SkillLevel("Pro")
              ]
          ),
        new Profession(
            "Chef",
                [
                  new SkillLevel("Junior"),
                  new SkillLevel("Medium"),
                  new SkillLevel("Pro")
                ]
            ),
        new Profession(
              "Bartender",
                  [
                    new SkillLevel("Junior"),
                    new SkillLevel("Medium"),
                  ]
              )
  ];
  addressToSearch = '';
  radius = 100.0;
  selectedProfession: Profession;
  selectedSkillLevelName = '';
  isLoading = false;

  foundProfiles: PaginatedUserProfiles;
  foundError = false;

  constructor(public skilledWorkersService: SkilledWrokersService) { }

  Search() {
    console.log('address ' + this.addressToSearch);
    console.log('radius' + this.radius);
    console.log('selected profession ' + this.selectedProfession.name);
    console.log('selected skill ' + this.selectedSkillLevelName);
    this.isLoading = true;
    this.skilledWorkersService
      .searchUsers$(this.addressToSearch, this.radius, this.selectedProfession.name, this.selectedSkillLevelName).subscribe(
      (successResponse) => {
        if (successResponse !== null) {
          console.log(successResponse);
          this.isLoading = false;
          this.foundProfiles = successResponse;
          console.log('profiles are ');
          console.log(this.foundProfiles);
        }
      },
      (errorResponse) => {
        this.isLoading =false;
        this.foundProfiles = null;
        this.foundError = true;
        setTimeout(function() {
          console.log('hide');
          this.foundError = false;
        }.bind(this), 5000);
      }
    );

  }

  onProfessionChanged(professionName){
    this.selectedProfession = this.professions.find(x => x.name == professionName);
  }

  onAutocompleteSelected(result: PlaceResult) {
    console.log('this is address object onAutocompleteSelected: ', JSON.stringify(result));
    this.addressToSearch = result.formatted_address;
  }



  ngOnInit() {
  }

}
