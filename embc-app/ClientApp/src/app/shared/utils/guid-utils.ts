// For the purpose of accessibility this number is likely unique.
// If it breaks and isn't unique it won't break the form. (poor man's guid)
export function uuid(): string {
  return new Date().valueOf().toString();
}
