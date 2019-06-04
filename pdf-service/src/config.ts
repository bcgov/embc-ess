
const parseJSONParam = (param: string | undefined, defaultParam: number | boolean | string[]) => {
  if (param) {
    try {
      return JSON.parse(param);
    } catch (err) {
      console.log(`Couldn't parse parameter: "${param}". Saw error "${err}"`);
      return defaultParam;
    }
  }
  return defaultParam;
};

// Server Options
export const PORT: number = parseJSONParam(process.env.PORT || process.env.OPENSHIFT_NODEJS_PORT, 3000);
export const IP: string = process.env.IP || process.env.OPENSHIFT_NODEJS_IP || '0.0.0.0';
export const MAX_PAYLOAD_SIZE: string = process.env.MAX_PAYLOAD_SIZE || '10mb';

// Security and accessibility
export const ENABLE_CORS: boolean = parseJSONParam(process.env.ENABLE_CORS, false);
export const ENABLE_DIAGNOSTICS: boolean = parseJSONParam(process.env.ENABLE_DIAGNOSTICS, false);

