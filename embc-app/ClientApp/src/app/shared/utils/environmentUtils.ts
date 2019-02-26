/**
 * detect IE
 */
export const detectIE10orLower = () => {
  const jscriptVersion = new Function('/*@cc_on return @_jscript_version; @*/')();
  if (jscriptVersion !== undefined) {
    return true;
  }
  return false;
};
