/**
 * detect IE
 */
export function detectIE10orLower(): boolean {
  const jscriptVersion = new Function('/*@cc_on return @_jscript_version; @*/')();
  if (jscriptVersion !== undefined) {
    return true;
  }
  return false;
}
