import { Component, OnInit, Input } from '@angular/core';
import { VersionService } from 'src/app/core/services/version.service';
import { AppVersion } from 'src/app/core/models/app-version.model';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersionComponent } from 'src/app/shared/modals/app-version/app-version.component';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  @Input() versionInfo: any = {};
  appVersion: AppVersion;
  private versionModal: NgbModalRef = null;

  constructor(private modals: NgbModal,
    private versionService: VersionService) { 
    this.versionService.getVersion()
    .subscribe((version: AppVersion) => {
      this.appVersion = version;
    })
  }

  ngOnInit() {
  }

  showVersion(){
    const modalRef =this.modals.open(AppVersionComponent);
    modalRef.componentInstance.version = this.appVersion;
  }
}
