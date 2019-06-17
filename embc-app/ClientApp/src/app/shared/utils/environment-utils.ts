
export function ieVersion(userAgent?: string): number | null {
  const uaString = userAgent || window.navigator.userAgent;
  const match = /\b(MSIE |Trident.*?rv:|Edge\/)(\d+)/.exec(uaString);
  if (match) {
    return parseInt(match[2], 10);
  }
  return null; // not IE/Edge!
}

/**
 * detect IE
 */
export function detectIE10orLower(): boolean {
  const version = ieVersion();
  return version !== null && version <= 10;
}
