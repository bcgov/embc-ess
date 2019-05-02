// VOLN = volunteer
// HOH = head of household
// FMBR = family member
type PersonType = ('VOLN' | 'HOH' | 'FMBR');

export interface Person {
  id: string | null;
  active?: boolean;
  personType: PersonType;
  firstName: string;
  lastName: string;
  nickname: string;
  initials: string;
  gender: string;
  dob: string | null;
}
