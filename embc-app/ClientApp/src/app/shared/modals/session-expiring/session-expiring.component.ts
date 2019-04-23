import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './session-expiring.component.html',
  styleUrls: ['./session-expiring.component.scss']
})

export class SessionExpiringModalComponent implements OnInit {
  @Input() durationInSeconds = 10;

  private interval: number;

  get countdownStr() {
    const mins = Math.trunc(this.durationInSeconds / 60);
    const secs = Math.max(this.durationInSeconds % 60, 0);
    return `${mins}:${secs < 10 ? '0' + secs : secs}`;
  }

  constructor(
    public activeModal: NgbActiveModal,
  ) { }

  ngOnInit() {
    // start the warning countdown
    this.interval = window.setInterval(this.doCountDown.bind(this), 1000);
  }

  private doCountDown() {
    this.durationInSeconds--;
    if (this.durationInSeconds <= 0) {
      // stop the warning countdown
      clearInterval(this.interval);

      // dismiss this modal (rejects promise)
      this.activeModal.dismiss();
    }
  }

  public refreshSession() {
    // stop the warning countdown
    clearInterval(this.interval);

    // close this modal (resolves promise)
    this.activeModal.close();
  }
}
