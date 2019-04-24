export interface Config {
  basePath: string | null;
  baseUri: string | null;
  environment: string | null;
  fileCreationTime: string | null;
  fileVersion: string | null;
  sourceCommit: string | null;
  sourceReference: string | null;
  sourceRepository: string | null;
  clientTimeoutWarningInMinutes: number | null;
  clientTimeoutWarningDurationInMinutes: number | null;
  defaultTimeoutWarningInMinutes: number | null;
  defaultWarningDurationInMinutes: number | null;
}
