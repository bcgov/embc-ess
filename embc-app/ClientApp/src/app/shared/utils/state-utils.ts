// Turns an array into an indexed object
// see https://redux.js.org/recipes/structuring-reducers/normalizing-state-shape
export function normalize(subjects: any[], primaryKey: string = 'id'): { [id: string]: any; } {
  const hash = {};
  subjects.forEach(s => hash[s[primaryKey]] = s);
  return hash;
}

export function flattenedSubjects(subjects: { [id: string]: any; }) {
  return Object.keys(subjects).map(k => subjects[k]);
}
