import { Profile } from '.';

export interface Person {
  id?: string;
  firstName: string;
  lastName: string;
  nickname: string;
  initials: string;
  gender: string;
  dob: number;

  // --> contact details (phone, email, address, etc)
  profile: Profile;

  // TODO: ????

  isEvacuee?: boolean;
  isVolunteer?: boolean;
  isFamilyMember?: boolean;
}
